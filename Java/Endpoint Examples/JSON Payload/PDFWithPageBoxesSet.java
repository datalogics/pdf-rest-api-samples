import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONArray;
import org.json.JSONObject;

public class PDFWithPageBoxesSet {

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

    JSONObject page = new JSONObject();
    page.put("range", "1");
    page.put("left", 100);
    page.put("top", 100);
    page.put("bottom", 100);
    page.put("right", 100);

    JSONArray pagesArray = new JSONArray();
    pagesArray.put(page);

    JSONObject box = new JSONObject();
    box.put("box", "media");
    box.put("pages", pagesArray);

    JSONArray boxesArray = new JSONArray();
    boxesArray.put(box);

    JSONObject boxOptionsObject = new JSONObject();
    boxOptionsObject.put("boxes", boxesArray);

    JSONObject requestJson = new JSONObject();
    requestJson.put("id", uploadedID);
    requestJson.put("boxes", boxOptionsObject.toString());
    requestJson.put("output", "exampleout.pdf");

    final RequestBody requestBody =
        RequestBody.create(requestJson.toString(), MediaType.parse("application/json"));

    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url("https://api.pdfrest.com/pdf-with-page-boxes-set")
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
