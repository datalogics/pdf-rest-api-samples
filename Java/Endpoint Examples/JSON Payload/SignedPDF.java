import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONArray;
import org.json.JSONObject;

public class SignedPDF {

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

        String inputId = uploadFile(inputFile);
        String credentialId = uploadFile(credentialFile);
        String passphraseId = uploadFile(passphraseFile);
        String logoId = uploadFile(logoFile);

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

        JSONObject requestJson = new JSONObject();
        requestJson.put("id", inputId);
        requestJson.put("pfx_credential_id", credentialId);
        requestJson.put("pfx_passphrase_id", passphraseId);
        requestJson.put("logo_id", logoId);
        requestJson.put("signature_configuration", signatureConfig.toString());

        final RequestBody requestBody =
                RequestBody.create(requestJson.toString(), MediaType.parse("application/json"));

        Request request =
                new Request.Builder()
                        .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
                        .url("https://api.pdfrest.com/signed-pdf")
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

    private static String uploadFile(File inputFile) {

        final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();

        final RequestBody requestBody =
                RequestBody.create(inputFile, MediaType.parse("application/pdf"));

        Request request =
                new Request.Builder()
                        .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
                        .header("Content-Filename", inputFile.getName())
                        .url("https://api.pdfrest.com/upload")
                        .post(requestBody)
                        .build();
        try {
            OkHttpClient client = new OkHttpClient().newBuilder().build();
            Response response = client.newCall(request).execute();
            System.out.println("Upload Result code " + response.code());
            if (response.body() != null) {
                String inputUploadString = response.body().string();
                JSONObject inputUploadJSON = new JSONObject(inputUploadString);
                if (inputUploadJSON.has("error")) {
                    System.out.println("Error during upload: " + inputUploadString);
                    return "";
                }
                JSONArray fileArray = inputUploadJSON.getJSONArray("files");

                JSONObject fileObject = fileArray.getJSONObject(0);

                return fileObject.get("id").toString();
            }
        } catch (IOException e) {
            throw new RuntimeException(e);
        }
        return "";
    }
}
