import io.github.cdimascio.dotenv.Dotenv;
import java.io.IOException;
import okhttp3.*;
import org.json.JSONObject;

public class DeleteResource {

  // By default, we use the US-based API service. This is the primary endpoint for global use.
  private static final String API_URL = "https://api.pdfrest.com";

  // For GDPR compliance and enhanced performance for European users, you can switch to the EU-based
  // service by uncommenting the URL below.
  // For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
  // private static final String API_URL = "https://eu-api.pdfrest.com";

  // Resource UUIDs can be found in the JSON response of POST requests as "outputId".
  // Resource UUIDs usually look like this: '0950b9bdf-0465-4d3f-8ea3-d2894f1ae839'.
  private static final String FILE_ID =
      "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"; // place resource uuid here

  // Specify your API key here, or in the environment variable PDFREST_API_KEY.
  // You can also put the environment variable in a .env file.
  private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

  public static void main(String[] args) {
    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();
    try {
      final RequestBody requestBody = RequestBody.create("", MediaType.parse("text/plain"));
      OkHttpClient client = new OkHttpClient().newBuilder().build();
      Request request =
          new Request.Builder()
              .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
              .url(API_URL + "/resource/" + FILE_ID)
              .method("DELETE", requestBody)
              .build();
      Response response = client.newCall(request).execute();
      System.out.println("Processing Result code " + response.code());
      if (response.body() != null && response.body().contentLength() > 0) {
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
