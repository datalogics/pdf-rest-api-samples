import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONArray;
import org.json.JSONObject;

public class MergedPDF {

  // By default, we use the US-based API service. This is the primary endpoint for global use.
  private static final String API_URL = "https://api.pdfrest.com";

  // For GDPR compliance and enhanced performance for European users, you can switch to the EU-based
  // service by commenting out the URL above and uncommenting the URL below.
  // For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
  // private static final String API_URL = "https://eu-api.pdfrest.com";

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

    JSONArray idArray = new JSONArray();
    JSONArray typeArray = new JSONArray();
    JSONArray pagesArray = new JSONArray();
    for (String inputFilePath : inputFilePaths) {
      final File inputFile = new File(inputFilePath);
      String uploadString = uploadFile(inputFile);
      JSONObject uploadJSON = new JSONObject(uploadString);
      if (uploadJSON.has("error")) {
        System.out.println("Error during upload: " + uploadString);
        return;
      }
      JSONArray fileArray = uploadJSON.getJSONArray("files");

      JSONObject fileObject = fileArray.getJSONObject(0);

      String uploadedID = fileObject.get("id").toString();
      idArray.put(uploadedID);
      typeArray.put("id");
      pagesArray.put("1-last");
    }

    JSONObject requestJSON = new JSONObject();
    requestJSON.put("id[]", idArray);
    requestJSON.put("type[]", typeArray);
    requestJSON.put("pages[]", pagesArray);

    String JSONString = requestJSON.toString();

    final RequestBody requestBody =
        RequestBody.create(JSONString, MediaType.parse("application/json"));

    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url(API_URL + "/merged-pdf")
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
