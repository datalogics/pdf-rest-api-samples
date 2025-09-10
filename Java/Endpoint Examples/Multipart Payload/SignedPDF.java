import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONObject;

public class SignedPDF {

  // By default, we use the US-based API service. This is the primary endpoint for global use.
  private static final String API_URL = "https://api.pdfrest.com";

  // For GDPR compliance and enhanced performance for European users, you can switch to the EU-based
  // service by uncommenting the URL below.
  // For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
  // private static final String API_URL = "https://eu-api.pdfrest.com";

  // Specify your API key here, or in the environment variable PDFREST_API_KEY.
  // You can also put the environment variable in a .env file.
  private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

  public static void main(String[] args) {
    File inputFile, credentialFile, passphraseFile, logoFile;
    if (args.length > 3) {
      inputFile = new File(args[0]);
      credentialFile = new File(args[1]);
      passphraseFile = new File(args[2]);
      logoFile = new File(args[3]);
    } else {
      inputFile = new File("/path/to/input.pdf");
      credentialFile = new File("/path/to/credentials.pfx");
      passphraseFile = new File("/path/to/passphrase.txt");
      logoFile = new File("/path/to/logo.png");
    }

    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();

    JSONObject bottomLeft = new JSONObject();
    bottomLeft.put("x", "0");
    bottomLeft.put("y", "0");

    JSONObject topRight = new JSONObject();
    topRight.put("x", "216");
    topRight.put("y", "72");

    JSONObject location = new JSONObject();
    location.put("bottom_left", bottomLeft);
    location.put("top_right", topRight);
    location.put("page", "1");

    JSONObject display = new JSONObject();
    display.put("include_distinguished_name", "true");
    display.put("include_datetime", "true");
    display.put("contact", "My contact information");
    display.put("location", "My signing location");
    display.put("name", "John Doe");
    display.put("reason", "My reason for signing");

    JSONObject signatureConfig = new JSONObject();
    signatureConfig.put("type", "new");
    signatureConfig.put("name", "esignature");
    signatureConfig.put("logo_opacity", "0.5");
    signatureConfig.put("location", location);
    signatureConfig.put("display", display);

    final RequestBody inputFileRequestBody =
        RequestBody.create(inputFile, MediaType.parse("application/pdf"));
    final RequestBody credentialFileRequestBody =
        RequestBody.create(credentialFile, MediaType.parse("application/x-pkcs12"));
    final RequestBody passphraseFileRequestBody =
        RequestBody.create(passphraseFile, MediaType.parse("text/plain"));
    final RequestBody logoFileRequestBody =
        RequestBody.create(logoFile, MediaType.parse("image/png"));
    RequestBody requestBody =
        new MultipartBody.Builder()
            .setType(MultipartBody.FORM)
            .addFormDataPart("file", inputFile.getName(), inputFileRequestBody)
            .addFormDataPart(
                "pfx_credential_file", credentialFile.getName(), credentialFileRequestBody)
            .addFormDataPart(
                "pfx_passphrase_file", passphraseFile.getName(), passphraseFileRequestBody)
            .addFormDataPart("logo_file", logoFile.getName(), logoFileRequestBody)
            .addFormDataPart("signature_configuration", signatureConfig.toString())
            .addFormDataPart("output", "example_out.pdf")
            .build();
    Request request =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url(API_URL + "/signed-pdf")
            .post(requestBody)
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
