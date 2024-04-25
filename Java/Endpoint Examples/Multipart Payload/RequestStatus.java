import java.io.IOException;
import java.util.concurrent.TimeUnit;

import io.github.cdimascio.dotenv.Dotenv;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;
import org.json.JSONObject;

public class RequestStatus {

  // Request UUIDs can be found in the JSON response of POST requests as "requestId" when API Polling is enabled.
  // Resource UUIDs usually look like this: '0950b9bdf-0465-4d3f-8ea3-d2894f1ae839'.
  private static final String REQUEST_ID =
      "2e3c603d1-30b2-4c16-8c11-911a51bb2ba9"; // place requestID here

  // Specify your API key here, or in the environment variable PDFREST_API_KEY.
  // You can also put the environment variable in a .env file.
  private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

public static void main(String[] args) {

    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();
    
    String urlString =
          String.format("https://api.pdfrest.com/request-status/%s", REQUEST_ID);
    
    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url(urlString)
            .get()
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
