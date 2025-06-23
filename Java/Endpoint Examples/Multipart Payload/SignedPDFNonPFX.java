import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONObject;

public class SignedPDFNonPFX {

    // Specify your API key here, or in the environment variable PDFREST_API_KEY.
    // You can also put the environment variable in a .env file.
    private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

    public static void main(String[] args) {
        File inputFile, certificateFile, privateKeyFile;
        if (args.length > 3) {
            inputFile = new File(args[0]);
            certificateFile = new File(args[1]);
            privateKeyFile = new File(args[2]);
        } else {
            inputFile = new File("/path/to/input.pdf");
            certificateFile = new File("/path/to/certificate.pem");
            privateKeyFile = new File("/path/to/private_key.pem");
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
        display.put("include_datetime", "true");

        JSONObject signatureConfig = new JSONObject();
        signatureConfig.put("type", "new");
        signatureConfig.put("name", "esignature");
        signatureConfig.put("location", location);
        signatureConfig.put("display", display);

        final RequestBody inputFileRequestBody =
                RequestBody.create(inputFile, MediaType.parse("application/pdf"));
        final RequestBody certificateFileRequestBody =
                RequestBody.create(certificateFile, MediaType.parse("application/x-pem-file"));
        final RequestBody privateKeyFileRequestBody =
                RequestBody.create(privateKeyFile, MediaType.parse("application/x-pem-file"));
        RequestBody requestBody =
                new MultipartBody.Builder()
                        .setType(MultipartBody.FORM)
                        .addFormDataPart("file", inputFile.getName(), inputFileRequestBody)
                        .addFormDataPart("certificate_file", certificateFile.getName(), certificateFileRequestBody)
                        .addFormDataPart("private_key_file", privateKeyFile.getName(), privateKeyFileRequestBody)
                        .addFormDataPart("signature_configuration", signatureConfig.toString())
                        .addFormDataPart("output", "example_out.pdf")
                        .build();
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
