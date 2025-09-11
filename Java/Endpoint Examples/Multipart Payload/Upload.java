import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import okhttp3.*;
import org.json.JSONObject;

public class Upload {

  // By default, we use the US-based API service. This is the primary endpoint for global use.
  private static final String API_URL = "https://api.pdfrest.com";

  // For GDPR compliance and enhanced performance for European users, you can switch to the EU-based
  // service by commenting out the URL above and uncommenting the URL below.
  // For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
  // private static final String API_URL = "https://eu-api.pdfrest.com";

  // Specify the paths to your file here, or as the arguments when running the program.
  private static final String[] DEFAULT_FILE_PATHS =
      new String[] {"/path/to/file1.pdf", "/path/to/file2.pdf"};

  // Specify your API key here, or in the environment variable PDFREST_API_KEY.
  // You can also put the environment variable in a .env file.
  private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

  public static void main(String[] args) {
    String[] inputFilePaths;
    if (args.length > 0) {
      inputFilePaths = args;
    } else {
      inputFilePaths = DEFAULT_FILE_PATHS;
    }

    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();

    MultipartBody.Builder bodyBuilder = new MultipartBody.Builder().setType(MultipartBody.FORM);

    for (String inputFilePath : inputFilePaths) {
      final File inputFile = new File(inputFilePath);
      final RequestBody inputFileRequestBody =
          RequestBody.create(inputFile, MediaType.parse("application/pdf"));
      bodyBuilder.addFormDataPart("file", inputFile.getName(), inputFileRequestBody);
    }

    RequestBody requestBody = bodyBuilder.build();

    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url(API_URL + "/upload")
            .post(requestBody)
            .build();
    try {
      OkHttpClient client = new OkHttpClient().newBuilder().build();
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
