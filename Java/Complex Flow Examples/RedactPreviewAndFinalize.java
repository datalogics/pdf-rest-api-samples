import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONObject;

/*
 * This sample demonstrates the workflow from unredacted document to fully
 * redacted document. The output file from the preview tool is immediately
 * forwarded to the finalization stage. We recommend inspecting the output from
 * the preview stage before utilizing this workflow to ensure that content is
 * redacted as intended.
 */

public class RedactPreviewAndFinalize {

  // Specify the path to your file here, or as the first argument when running the program.
  private static final String DEFAULT_FILE_PATH = "/path/to/file";

  // Specify your API key here, or in the environment variable PDFREST_API_KEY.
  // You can also put the environment variable in a .env file.
  private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

  //
  private static final String REDACTION_OBJECTS = "[{\"type\":\"regex\",\"value\":\"[Tt]he\"}]";

  public static void main(String[] args) {
    File inputFile;
    if (args.length > 0) {
      inputFile = new File(args[0]);
    } else {
      inputFile = new File(DEFAULT_FILE_PATH);
    }

    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();

    final RequestBody previewInputFileRequestBody =
        RequestBody.create(inputFile, MediaType.parse("application/pdf"));
    RequestBody previewRequestBody =
        new MultipartBody.Builder()
            .setType(MultipartBody.FORM)
            .addFormDataPart("file", inputFile.getName(), previewInputFileRequestBody)
            .addFormDataPart("redactions", REDACTION_OBJECTS)
            .addFormDataPart("output", "pdfrest_preview")
            .build();
    Request previewRequest =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url("https://api.pdfrest.com/pdf-with-redacted-text-preview")
            .post(previewRequestBody)
            .build();
    try {
      OkHttpClient pdfClient =
          new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();

      Response previewResponse = pdfClient.newCall(previewRequest).execute();

      System.out.println("Result code from preview call: " + previewResponse.code());
      if (previewResponse.body() != null) {
        String pdfResponseString = previewResponse.body().string();

        JSONObject pdfJSON = new JSONObject(pdfResponseString);
        if (pdfJSON.has("error")) {
          System.out.println("Error during pdf call: " + previewResponse.body().string());
          return;
        }

        String pdfID = pdfJSON.get("outputId").toString();

        RequestBody appliedRequestBody =
            new MultipartBody.Builder()
                .setType(MultipartBody.FORM)
                .addFormDataPart("id", pdfID)
                .addFormDataPart("output", "pdfrest_applied")
                .build();
        Request appliedRequest =
            new Request.Builder()
                .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
                .url("https://api.pdfrest.com/pdf-with-redacted-text-applied")
                .post(appliedRequestBody)
                .build();
        try {
          OkHttpClient appliedClient =
              new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();
          Response appliedResponse = appliedClient.newCall(appliedRequest).execute();
          System.out.println("Result code from applied call: " + appliedResponse.code());
          if (appliedResponse.body() != null) {
            System.out.println(prettyJson(appliedResponse.body().string()));
          }
        } catch (IOException e) {
          throw new RuntimeException(e);
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
