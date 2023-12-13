import io.github.cdimascio.dotenv.Dotenv;
import okhttp3.*;
import org.json.JSONObject;

import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;

public class PdfWithAddedImage {

  // Specify the path to your file here, or as the first argument when running the program.
  private static final String DEFAULT_FILE_PATH = "/path/to/file.pdf";

  // Specify the path to your image file here, or as the second argument when running the
  // program.
  private static final String DEFAULT_IMAGE_PATH = "/path/to/file.xml";

  // Specify your API key here, or in the environment variable PDFREST_API_KEY.
  // You can also put the environment variable in a .env file.
  private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

  public static void main(String[] args) {
    File inputFile, imageFile;
    if (args.length > 1) {
      inputFile = new File(args[0]);
      imageFile = new File(args[1]);
    } else {
      inputFile = new File(DEFAULT_FILE_PATH);
      imageFile = new File(DEFAULT_IMAGE_PATH);
    }

    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();

    final RequestBody inputFileRequestBody =
        RequestBody.create(inputFile, MediaType.parse("application/pdf"));
    final RequestBody imageFileRequestBody =
        RequestBody.create(imageFile, MediaType.parse("application/png"));
    RequestBody requestBody =
        new MultipartBody.Builder()
            .setType(MultipartBody.FORM)
            .addFormDataPart("file", inputFile.getName(), inputFileRequestBody)
            .addFormDataPart("image_file", imageFile.getName(), imageFileRequestBody)
            .addFormDataPart("page", "1")
            .addFormDataPart("x", "0")
            .addFormDataPart("y", "0")
            .build();
    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url("https://api.pdfrest.com/pdf-with-added-image")
            .post(requestBody)
            .build();
    try {
      OkHttpClient client = new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();
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
