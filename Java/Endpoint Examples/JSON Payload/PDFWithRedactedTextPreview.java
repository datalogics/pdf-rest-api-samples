import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONArray;
import org.json.JSONObject;

public class PDFWithRedactedTextPreview {

  // Specify the path to your file here, or as the first argument when running the program.
  private static final String DEFAULT_FILE_PATH = "/path/to/file";

  // Specify your API key here, or in the environment variable PDFREST_API_KEY.
  // You can also put the environment variable in a .env file.
  private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

  public static void main(String[] args) {
    File inputFile;
    if (args.length > 0) {
      inputFile = new File(args[0]);
    } else {
      inputFile = new File(DEFAULT_FILE_PATH);
    }
    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();

    String uploadString = uploadFile(inputFile);
    JSONObject uploadJSON = new JSONObject(uploadString);
    if (uploadJSON.has("error")) {
      System.out.println("Error during upload: " + uploadString);
      return;
    }
    JSONArray fileArray = uploadJSON.getJSONArray("files");

    JSONObject fileObject = fileArray.getJSONObject(0);

    String uploadedID = fileObject.get("id").toString();

    String redactionOptions =
        "[{\\\"type\\\":\\\"preset\\\",\\\"value\\\":\\\"email\\\"},{\\\"type\\\":\\\"regex\\\",\\\"value\\\":\\\"(\\\\\\\\+\\\\\\\\d{1,2}\\\\\\\\s)?\\\\\\\\(?\\\\\\\\d{3}\\\\\\\\)?[\\\\\\\\s.-]\\\\\\\\d{3}[\\\\\\\\s.-]\\\\\\\\d{4}\\\"},{\\\"type\\\":\\\"literal\\\",\\\"value\\\":\\\"word\\\"}]";

    String JSONString =
        String.format("{\"id\":\"%s\", \"redactions\":\"%s\"}", uploadedID, redactionOptions);

    final RequestBody requestBody =
        RequestBody.create(JSONString, MediaType.parse("application/json"));

    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url("https://api.pdfrest.com/pdf-with-redacted-text-preview")
            .post(requestBody)
            .build();
    try {
      OkHttpClient client =
          new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();

      Response response = client.newCall(request).execute();
      System.out.println("Processing Result code " + response.code());
      if (response.body() != null) {
        String respStr = response.body().string();
        System.out.println(prettyJson(respStr));

        // All files uploaded or generated are automatically deleted based on the

        // All files uploaded or generated are automatically deleted based on the
        // File Retention Period as shown on https://pdfrest.com/pricing.
        // For immediate deletion of files, particularly when sensitive data
        // is involved, an explicit delete call can be made to the API.
        //
        // The following code is an optional step to delete sensitive files
        // (unredacted, unencrypted, unrestricted, or unwatermarked) from pdfRest servers.
        // IMPORTANT: Do not delete the previewId (the preview PDF) file until after the redaction
        // is applied with the /pdf-with-redacted-text-applied endpoint.

        String previewId = new JSONObject(respStr).getString("outputId");
        String deleteJson = String.format("{ \"ids\":\"%s, %s\" }", uploadedID, previewId);
        RequestBody deleteBody =
            RequestBody.create(deleteJson, MediaType.parse("application/json"));
        Request deleteRequest =
            new Request.Builder()
                .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
                .url("https://api.pdfrest.com/delete")
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
    } catch (IOException e) {
      throw new RuntimeException(e);
    }
  }

  private static String prettyJson(String json) {
    // https://stackoverflow.com/a/9583835/11996393
    return new JSONObject(json).toString(4);
  }

  // This function is just a copy of the 'Upload.java' file to upload a binary file
  private static String uploadFile(File inputFile) {

    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();

    final RequestBody requestBody =
        RequestBody.create(inputFile, MediaType.parse("application/pdf"));

    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .header("Content-Filename", "File.pdf")
            .url("https://api.pdfrest.com/upload")
            .post(requestBody)
            .build();
    try {
      OkHttpClient client = new OkHttpClient().newBuilder().build();
      Response response = client.newCall(request).execute();
      System.out.println("Upload Result code " + response.code());
      if (response.body() != null) {
        return response.body().string();
      }
    } catch (IOException e) {
      throw new RuntimeException(e);
    }
    return "";
  }
}
