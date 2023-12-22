import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONObject;

/* In this sample, we will show how to watermark a PDF document and then restrict
 * editing on the document so that the watermark cannot be removed, as discussed in
 * https://pdfrest.com/solutions/add-pdf-watermarks-that-cannot-be-removed/.
 * We will be running the input file through /watermarked-pdf to apply the watermark
 * and then /restricted-pdf to lock the watermark in.
 */

public class ProtectedWatermark {

  // Specify the path to your file here, or as the first argument when running the program.
  private static final String DEFAULT_FILE_PATH = "/path/to/file.pdf";

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

    final RequestBody watermarkInputFileRequestBody =
        RequestBody.create(inputFile, MediaType.parse("application/pdf"));
    RequestBody watermarkRequestBody =
        new MultipartBody.Builder()
            .setType(MultipartBody.FORM)
            .addFormDataPart("file", inputFile.getName(), watermarkInputFileRequestBody)
            .addFormDataPart("watermark_text", "WATERMARK")
            .addFormDataPart("output", "pdfrest_watermarked")
            .build();
    Request watermarkRequest =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url("https://api.pdfrest.com/watermarked-pdf")
            .post(watermarkRequestBody)
            .build();
    try {
      OkHttpClient watermarkClient =
          new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();

      Response watermarkResponse = watermarkClient.newCall(watermarkRequest).execute();

      System.out.println("Result code from watermark call: " + watermarkResponse.code());
      if (watermarkResponse.body() != null) {
        String watermarkResponseString = watermarkResponse.body().string();

        JSONObject watermarkJSON = new JSONObject(watermarkResponseString);
        if (watermarkJSON.has("error")) {
          System.out.println("Error during watermark call: " + watermarkResponse.body().string());
          return;
        }

        String watermarkID = watermarkJSON.get("outputId").toString();

        RequestBody restrictRequestBody =
            new MultipartBody.Builder()
                .setType(MultipartBody.FORM)
                .addFormDataPart("id", watermarkID)
                .addFormDataPart("new_permissions_password", "password")
                .addFormDataPart("restrictions[]", "copy_content")
                .addFormDataPart("restrictions[]", "edit_annotations")
                .addFormDataPart("restrictions[]", "edit_content")
                .addFormDataPart("output", "pdfrest_restricted")
                .build();
        Request restrictRequest =
            new Request.Builder()
                .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
                .url("https://api.pdfrest.com/restricted-pdf")
                .post(restrictRequestBody)
                .build();
        try {
          OkHttpClient restrictClient =
              new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();
          Response restrictResponse = restrictClient.newCall(restrictRequest).execute();
          System.out.println("Result code from restrict call: " + restrictResponse.code());
          if (restrictResponse.body() != null) {
            System.out.println(prettyJson(restrictResponse.body().string()));
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
