import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONObject;

/* In this sample, we will show how to optimize a Word file for long-term preservation
 * as discussed in https://pdfrest.com/solutions/optimize-word-excel-and-powerpoint-files-for-long-term-preservation/
 * We will take our Word (or Excel or PowerPoint) document and first convert it to
 * a PDF with a call to the /pdf route. Then, we will take that converted PDF
 * and convert it to the PDF/A format for long-term storage.
 */

public class PreserveWordDocument {

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

    final RequestBody pdfInputFileRequestBody =
        RequestBody.create(inputFile, MediaType.parse("application/pdf"));
    RequestBody pdfRequestBody =
        new MultipartBody.Builder()
            .setType(MultipartBody.FORM)
            .addFormDataPart("file", inputFile.getName(), pdfInputFileRequestBody)
            .addFormDataPart("output", "pdfrest_pdf")
            .build();
    Request pdfRequest =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url("https://api.pdfrest.com/pdf")
            .post(pdfRequestBody)
            .build();
    try {
      OkHttpClient pdfClient =
          new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();

      Response pdfResponse = pdfClient.newCall(pdfRequest).execute();

      System.out.println("Result code from pdf call: " + pdfResponse.code());
      if (pdfResponse.body() != null) {
        String pdfResponseString = pdfResponse.body().string();

        JSONObject pdfJSON = new JSONObject(pdfResponseString);
        if (pdfJSON.has("error")) {
          System.out.println("Error during pdf call: " + pdfResponse.body().string());
          return;
        }

        String pdfID = pdfJSON.get("outputId").toString();

        RequestBody pdfaRequestBody =
            new MultipartBody.Builder()
                .setType(MultipartBody.FORM)
                .addFormDataPart("id", pdfID)
                .addFormDataPart("output_type", "PDF/A-3b")
                .addFormDataPart("output", "pdfrest_pdfa")
                .build();
        Request pdfaRequest =
            new Request.Builder()
                .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
                .url("https://api.pdfrest.com/pdfa")
                .post(pdfaRequestBody)
                .build();
        try {
          OkHttpClient pdfaClient =
              new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();
          Response pdfaResponse = pdfaClient.newCall(pdfaRequest).execute();
          System.out.println("Result code from pdfa call: " + pdfaResponse.code());
          if (pdfaResponse.body() != null) {
            System.out.println(prettyJson(pdfaResponse.body().string()));
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
