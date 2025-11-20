import io.github.cdimascio.dotenv.Dotenv;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONObject;

public class BlankPDF {

  // By default, we use the US-based API service. This is the primary endpoint for global use.
  private static final String API_URL = "https://api.pdfrest.com";

  /* For GDPR compliance and enhanced performance for European users, you can switch to the EU-based
   * service by commenting out the URL above and uncommenting the URL below.
   * For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
   */
  // private static final String API_URL = "https://eu-api.pdfrest.com";

  // Specify your API key here, or in the environment variable PDFREST_API_KEY.
  // You can also put the environment variable in a .env file.
  private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

  public static void main(String[] args) {
    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();
    String apiKey = dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY);
    String apiBase = dotenv.get("PDFREST_URL", API_URL);
    if (apiBase == null || apiBase.isBlank()) {
      apiBase = API_URL;
    }
    apiBase = apiBase.replaceAll("/+$", "");

    JSONObject payload = new JSONObject();
    payload.put("page_size", "letter");
    payload.put("page_count", 3);
    payload.put("page_orientation", "portrait");

    RequestBody requestBody =
        RequestBody.create(payload.toString(), MediaType.parse("application/json"));

    Request request =
        new Request.Builder()
            .header("Api-Key", apiKey)
            .header("Content-Type", "application/json")
            .url(apiBase + "/blank-pdf")
            .post(requestBody)
            .build();

    try {
      OkHttpClient client =
          new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();

      Response response = client.newCall(request).execute();
      System.out.println("blank-pdf Result code " + response.code());
      if (response.body() != null) {
        System.out.println(response.body().string());
      }
    } catch (IOException e) {
      throw new RuntimeException(e);
    }
  }
}
