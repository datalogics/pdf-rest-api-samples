import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONArray;
import org.json.JSONObject;

public class SignedPDFNonPFX {

  // By default, we use the US-based API service. This is the primary endpoint for global use.
  private static final String API_URL = "https://api.pdfrest.com";

  // For GDPR compliance and enhanced performance for European users, you can switch to the EU-based service by uncommenting the URL below.
  // For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
  //private static final String API_URL = "https://eu-api.pdfrest.com";

  // Specify your API key here, or in the environment variable PDFREST_API_KEY.
  // You can also put the environment variable in a .env file.
  private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

  public static void main(String[] args) {
    File inputFile, certificateFile, privateKeyFile;
    if (args.length > 3) {
      inputFile = new File(args[0]);
      certificateFile = new File(args[1]);
      privateKeyFile = new File(args[2]);
    } else {
      inputFile = new File("/path/to/input.pdf");
      certificateFile = new File("/path/to/certificate.pem");
      privateKeyFile = new File("/path/to/private_key.pem");
    }
    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();

    String inputId = uploadFile(inputFile);
    String certificateId = uploadFile(certificateFile);
    String privateKeyId = uploadFile(privateKeyFile);

    JSONObject bottomLeft = new JSONObject();
    bottomLeft.put("x", "0");
    bottomLeft.put("y", "0");

    JSONObject topRight = new JSONObject();
    topRight.put("x", "216");
    topRight.put("y", "72");

    JSONObject location = new JSONObject();
    location.put("bottom_left", bottomLeft);
    location.put("top_right", topRight);
    location.put("page", "1");

    JSONObject display = new JSONObject();
    display.put("include_datetime", "true");

    JSONObject signatureConfig = new JSONObject();
    signatureConfig.put("type", "new");
    signatureConfig.put("name", "esignature");
    signatureConfig.put("location", location);
    signatureConfig.put("display", display);

    JSONObject requestJson = new JSONObject();
    requestJson.put("id", inputId);
    requestJson.put("certificate_id", certificateId);
    requestJson.put("private_key_id", privateKeyId);
    requestJson.put("signature_configuration", signatureConfig.toString());

    final RequestBody requestBody =
        RequestBody.create(requestJson.toString(), MediaType.parse("application/json"));

    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url(API_URL + "/signed-pdf")
            .post(requestBody)
            .build();
    try {
      OkHttpClient client =
          new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();

      Response response = client.newCall(request).execute();
      System.out.println("Processing Result code " + response.code());
      if (response.body() != null) {
        System.out.println(prettyJson(response.body().string()));
      }
    } catch (IOException e) {
      throw new RuntimeException(e);
    }
  }

  private static String prettyJson(String json) {
    // https://stackoverflow.com/a/9583835/11996393
    return new JSONObject(json).toString(4);
  }

  private static String uploadFile(File inputFile) {

    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();

    final RequestBody requestBody =
        RequestBody.create(inputFile, MediaType.parse("application/pdf"));

    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .header("Content-Filename", inputFile.getName())
            .url(API_URL + "/upload")
            .post(requestBody)
            .build();
    try {
      OkHttpClient client = new OkHttpClient().newBuilder().build();
      Response response = client.newCall(request).execute();
      System.out.println("Upload Result code " + response.code());
      if (response.body() != null) {
        String inputUploadString = response.body().string();
        JSONObject inputUploadJSON = new JSONObject(inputUploadString);
        if (inputUploadJSON.has("error")) {
          System.out.println("Error during upload: " + inputUploadString);
          return "";
        }
        JSONArray fileArray = inputUploadJSON.getJSONArray("files");

        JSONObject fileObject = fileArray.getJSONObject(0);

        return fileObject.get("id").toString();
      }
    } catch (IOException e) {
      throw new RuntimeException(e);
    }
    return "";
  }
}
