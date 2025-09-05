import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONObject;

public class RestrictedPDF {

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
            .addFormDataPart("new_permissions_password", "password")
            .addFormDataPart("restrictions[]", "print_high")
            .addFormDataPart("restrictions[]", "print_low")
            .addFormDataPart("restrictions[]", "edit_content")
            .addFormDataPart("output", "pdfrest_restricted")
            .build();
    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url("https://api.pdfrest.com/restricted-pdf")
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
        // The following code is an optional step to delete sensitive files
        // (unredacted, unencrypted, unrestricted, or unwatermarked) from pdfRest servers.

        if (DELETE_SENSITIVE_FILES) {
          String inputId = new org.json.JSONObject(respStr).getString("inputId");
          String deleteJson = String.format("{ \"ids\":\"%s\" }", inputId);
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
