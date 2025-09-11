import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONArray;
import org.json.JSONObject;

public class PDFWithAddedImage {

  // By default, we use the US-based API service. This is the primary endpoint for global use.
  private static final String API_URL = "https://api.pdfrest.com";

  // For GDPR compliance and enhanced performance for European users, you can switch to the EU-based
  // service by commenting out the URL above and uncommenting the URL below.
  // For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
  // private static final String API_URL = "https://eu-api.pdfrest.com";

  // Specify the path to your file here, or as the first argument when running the program.
  private static final String DEFAULT_FILE_PATH = "/path/to/file.pdf";

  // Specify the path to your image here, or as the second argument when running the
  // program.
  private static final String DEFAULT_IMAGE_PATH = "/path/to/file.png";

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

    String uploadFileString = uploadFile(inputFile, "file.pdf");
    JSONObject uploadFileJSON = new JSONObject(uploadFileString);
    if (uploadFileJSON.has("error")) {
      System.out.println("Error during upload: " + uploadFileString);
      return;
    }
    JSONArray fileArray = uploadFileJSON.getJSONArray("files");
    JSONObject fileObject = fileArray.getJSONObject(0);
    String uploadedFileID = fileObject.get("id").toString();

    String uploadImageString = uploadFile(imageFile, "file.png");
    JSONObject uploadImageJSON = new JSONObject(uploadImageString);
    if (uploadImageJSON.has("error")) {
      System.out.println("Error during upload: " + uploadImageString);
      return;
    }
    JSONArray imageFileArray = uploadImageJSON.getJSONArray("files");
    JSONObject imageObject = imageFileArray.getJSONObject(0);
    String uploadedImageID = imageObject.get("id").toString();

    String JSONString =
        String.format(
            "{\"id\":\"%s\", \"image_id\":\"%s\", \"page\":\"1\", \"x\":\"0\", \"y\":\"0\"}",
            uploadedFileID, uploadedImageID);

    final RequestBody requestBody =
        RequestBody.create(JSONString, MediaType.parse("application/json"));

    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url(API_URL + "/pdf-with-added-image")
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

  // This function is just a copy of the 'Upload.java' file to upload a binary file
  private static String uploadFile(File inputFile, String filename) {

    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();

    final RequestBody requestBody =
        RequestBody.create(inputFile, MediaType.parse("application/pdf"));

    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .header("Content-Filename", filename)
            .url(API_URL + "/upload")
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
