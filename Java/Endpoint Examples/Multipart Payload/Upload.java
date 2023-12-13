import io.github.cdimascio.dotenv.Dotenv;
import okhttp3.*;
import org.json.JSONObject;

import java.io.File;
import java.io.IOException;

public class Upload {

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
      bodyBuilder
          .addFormDataPart("file", inputFile.getName(), inputFileRequestBody);
    }

    RequestBody requestBody = bodyBuilder.build();

    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url("https://api.pdfrest.com/upload")
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
