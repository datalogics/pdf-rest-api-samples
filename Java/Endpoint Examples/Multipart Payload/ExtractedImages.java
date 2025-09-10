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

public class ExtractedImages {

  // By default, we use the US-based API service. This is the primary endpoint for global use.
  private static final String API_URL = "https://api.pdfrest.com";

  // For GDPR compliance and enhanced performance for European users, you can switch to the EU-based
  // service by uncommenting the URL below.
  // For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
  // private static final String API_URL = "https://eu-api.pdfrest.com";

  // Specify the path to your file here, or as the first argument when running the program.
  private static final String DEFAULT_FILE_PATH = "/path/to/file.pdf";

  // Specify your API key here, or in the environment variable PDFREST_API_KEY.
  // You can also put the environment variable in a .env file.
  private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

  private static final String PAGES = "1-last";

  public static void main(String[] args) {
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
            .addFormDataPart("pages", PAGES)
            .addFormDataPart("output", "pdfrest_extracted_images")
            .build();
    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url(API_URL + "/extracted-images")
            .post(requestBody)
            .build();
    try {
      OkHttpClient client =
          new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();
      Response response = client.newCall(request).execute();
      System.out.println("Result code " + response.code());
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
}
