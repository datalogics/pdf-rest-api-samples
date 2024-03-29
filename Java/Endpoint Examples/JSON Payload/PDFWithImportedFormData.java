import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONArray;
import org.json.JSONObject;

public class PDFWithImportedFormData {

  // Specify the path to your file here, or as the first argument when running the program.
  private static final String DEFAULT_FILE_PATH = "/path/to/file.pdf";

  // Specify the path to your image here, or as the second argument when running the
  // program.
  private static final String DEFAULT_DATA_PATH = "/path/to/file.xml";

  // Specify your API key here, or in the environment variable PDFREST_API_KEY.
  // You can also put the environment variable in a .env file.
  private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

  public static void main(String[] args) {
    File inputFile, dataFile;
    if (args.length > 1) {
      inputFile = new File(args[0]);
      dataFile = new File(args[1]);
    } else {
      inputFile = new File(DEFAULT_FILE_PATH);
      dataFile = new File(DEFAULT_DATA_PATH);
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

    String uploadDataString = uploadFile(dataFile, "file.xml");
    JSONObject uploadDataJSON = new JSONObject(uploadDataString);
    if (uploadDataJSON.has("error")) {
      System.out.println("Error during upload: " + uploadDataString);
      return;
    }
    JSONArray dataFileArray = uploadDataJSON.getJSONArray("files");
    JSONObject dataObject = dataFileArray.getJSONObject(0);
    String uploadedDataID = dataObject.get("id").toString();

    String JSONString =
        String.format("{\"id\":\"%s\", \"data_file_id\":\"%s\"}", uploadedFileID, uploadedDataID);

    final RequestBody requestBody =
        RequestBody.create(JSONString, MediaType.parse("application/json"));

    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url("https://api.pdfrest.com/pdf-with-imported-form-data")
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
