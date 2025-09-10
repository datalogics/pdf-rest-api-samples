import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONObject;

public class RequestStatus {

  // By default, we use the US-based API service. This is the primary endpoint for global use.
  private static final String API_URL = "https://api.pdfrest.com";

  // For GDPR compliance and enhanced performance for European users, you can switch to the EU-based
  // service by uncommenting the URL below.
  // For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
  // private static final String API_URL = "https://eu-api.pdfrest.com";

  // Specify your API key here, or in the environment variable PDFREST_API_KEY.
  // You can also put the environment variable in a .env file.
  private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";
  // Specify the path to your file here, or as the first argument when running the program.
  private static final String DEFAULT_FILE_PATH = "/path/to/file.pdf";

  public static void main(String[] args) {

    File inputFile;
    if (args.length > 0) {
      inputFile = new File(args[0]);
    } else {
      inputFile = new File(DEFAULT_FILE_PATH);
    }

    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();
    String apiKey = dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY);

    // Using PDF/A as an arbitrary example, send a request with a 'response-type' header.
    String pdfaResponse = getPdfaResponse(inputFile, apiKey);
    JSONObject pdfaJson = new JSONObject(pdfaResponse);
    if (pdfaJson.has("error")) {
      System.out.println("Error during PDFA call: " + pdfaJson.getString("error"));
    } else {
      // Get a request ID from the response.
      String requestID = pdfaJson.getString("requestId");

      // Check the request status.
      String requestStatusResponse = getRequestStatusResponse(requestID, apiKey);
      JSONObject requestStatusJson = new JSONObject(requestStatusResponse);

      // If still pending, check periodically for updates.
      while (requestStatusJson.getString("status").equals("pending")) {
        final int delay = 5000;
        try {
          Thread.sleep(delay);
          requestStatusResponse = getRequestStatusResponse(requestID, apiKey);
          requestStatusJson = new JSONObject(requestStatusResponse);
        } catch (InterruptedException e) {
          System.out.println(e);
        }
      }
    }
  }

  private static String getPdfaResponse(File inputFile, String apiKey) {

    final RequestBody inputFileRequestBody =
        RequestBody.create(inputFile, MediaType.parse("application/pdf"));
    RequestBody requestBody =
        new MultipartBody.Builder()
            .setType(MultipartBody.FORM)
            .addFormDataPart("file", inputFile.getName(), inputFileRequestBody)
            .addFormDataPart("output_type", "PDF/A-2u")
            .addFormDataPart("output", "pdfrest_pdfa")
            .build();
    Request request =
        new Request.Builder()
            .header("Api-Key", apiKey)
            .header("response-type", "requestId")
            .url(API_URL + "/pdfa")
            .post(requestBody)
            .build();
    try {
      OkHttpClient client =
          new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();

      Response response = client.newCall(request).execute();
      System.out.println("Result code " + response.code());
      String responseBody = response.body().string();
      System.out.println(prettyJson(responseBody));
      return responseBody;
    } catch (IOException e) {
      throw new RuntimeException(e);
    }
  }

  private static String getRequestStatusResponse(String requestId, String apiKey) {
    String urlString = String.format(API_URL + "/request-status/%s", requestId);
    Request request = new Request.Builder().header("Api-Key", apiKey).url(urlString).get().build();
    try {
      OkHttpClient client =
          new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();
      Response response = client.newCall(request).execute();
      System.out.println("Result code " + response.code());
      String responseBody = response.body().string();
      System.out.println(prettyJson(responseBody));
      return responseBody;
    } catch (IOException e) {
      throw new RuntimeException(e);
    }
  }

  private static String prettyJson(String json) {
    // https://stackoverflow.com/a/9583835/11996393
    return new JSONObject(json).toString(4);
  }
}
