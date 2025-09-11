import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;
import org.json.JSONObject;

public class PDFWithRedactedTextApplied {

  // By default, we use the US-based API service. This is the primary endpoint for global use.
  private static final String API_URL = "https://api.pdfrest.com";

  // For GDPR compliance and enhanced performance for European users, you can switch to the EU-based
  // service by commenting out the URL above and uncommenting the URL below.
  // For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
  // private static final String API_URL = "https://eu-api.pdfrest.com";

  // Specify the path to your file here, or as the first argument when running the program.
  private static final String DEFAULT_FILE_PATH = "/path/to/file.pdf";

  // Specify your API key here, or in the environment variable PDFREST_API_KEY.
  // You can also put the environment variable in a .env file.
  private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

  public static void main(String[] args) {
    final boolean DELETE_SENSITIVE_FILES = false; // toggle deletion (default: false)
    File inputFile;
    if (args.length > 0) {
      inputFile = new File(args[0]);
    } else {
      inputFile = new File(DEFAULT_FILE_PATH);
    }

    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();

    final RequestBody inputFileRequestBody =
        RequestBody.create(inputFile, MediaType.parse("application/pdf"));
    RequestBody requestBody =
        new MultipartBody.Builder()
            .setType(MultipartBody.FORM)
            .addFormDataPart("file", inputFile.getName(), inputFileRequestBody)
            .addFormDataPart("output", "pdfrest_pdf-with-redacted-text-applied")
            .build();
    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url(API_URL + "/pdf-with-redacted-text-applied")
            .post(requestBody)
            .build();
    try {
      OkHttpClient client =
          new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();
      Response response = client.newCall(request).execute();
      System.out.println("Result code " + response.code());
      if (response.body() != null) {
        String respStr = response.body().string();
        System.out.println(prettyJson(respStr));

        // All files uploaded or generated are automatically deleted based on the
        // File Retention Period as shown on https://pdfrest.com/pricing.
        // For immediate deletion of files, particularly when sensitive data
        // is involved, an explicit delete call can be made to the API.
        //
        // Deletes all files in the workflow, including outputs. Save all desired files before
        // enabling this step.

        if (DELETE_SENSITIVE_FILES) {
          org.json.JSONObject parsed = new org.json.JSONObject(respStr);
          String inputId = parsed.getString("inputId");
          String outputId = parsed.getString("outputId");
          String deleteJson = String.format("{ \"ids\":\"%s, %s\" }", inputId, outputId);
          RequestBody deleteBody =
              RequestBody.create(deleteJson, MediaType.parse("application/json"));
          Request deleteRequest =
              new Request.Builder()
                  .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
                  .url(API_URL + "/delete")
                  .post(deleteBody)
                  .build();
          try (Response deleteResp =
              new OkHttpClient()
                  .newBuilder()
                  .readTimeout(60, TimeUnit.SECONDS)
                  .build()
                  .newCall(deleteRequest)
                  .execute()) {
            if (deleteResp.body() != null) {
              System.out.println(prettyJson(deleteResp.body().string()));
            }
          }
        }
      }
    } catch (IOException e) {
      throw new RuntimeException(e);
    }
  }

  private static String prettyJson(String json) {
    // https://stackoverflow.com/a/9583835/11996393
    return new JSONObject(json).toString(4);
  }
}
