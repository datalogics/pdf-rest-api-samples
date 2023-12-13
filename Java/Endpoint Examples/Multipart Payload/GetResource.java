
import org.json.JSONObject;

import java.io.IOException;
import java.io.InputStream;
import java.net.URL;
import java.nio.file.Files;
import java.nio.file.Paths;

public class GetResource {


  // Resource UUIDs can be found in the JSON response of POST requests as "outputId".
  // Resource UUIDs usually look like this: '0950b9bdf-0465-4d3f-8ea3-d2894f1ae839'.
  private static final String FILE_ID   = "2e3c603d1-30b2-4c16-8c11-911a51bb2ba9"; // place resource uuid here

  // The response format can be 'file' or 'url'.
  // If 'url', then JSON containing the url of the resource file is returned.
  // If 'file', then the file itself is returned.
  private static final String OUTPUT_FORMAT = "file";

  public static void main(String[] args) {
    try {
      String urlString = String.format("https://api.pdfrest.com/resource/%s?format=%2s", FILE_ID, OUTPUT_FORMAT);
      InputStream in = new URL(urlString).openStream();
      Files.copy(in, Paths.get("/path/to/write/file")); // Set a path for the file to be written
    } catch (IOException e) {
      throw new RuntimeException(e);
    }
  }

  private static String prettyJson(String json) {
    // https://stackoverflow.com/a/9583835/11996393
    return new JSONObject(json).toString(4);
  }
}
