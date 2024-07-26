import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONObject;

/* In this sample, we will show how to convert a scanned document into a PDF with
 * searchable and extractable text using Optical Character Recognition (OCR), and then
 * extract that text from the newly created document.
 *
 * First, we will upload a scanned PDF to the /pdf-with-ocr-text route and capture the
 * output ID. Then, we will send the output ID to the /extracted-text route, which will
 * return the newly added text.
 */

public class OcrWithExtractText {

    // Specify the path to your PDF file here, or as the first argument when running the program.
    private static final String DEFAULT_PDF_FILE_PATH = "/path/to/file.pdf";

    // Specify your API key here, or in the environment variable PDFREST_API_KEY.
    // You can also put the environment variable in a .env file.
    private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

    public static void main(String[] args) {
        File pdfFile;
        if (args.length > 0) {
            pdfFile = new File(args[0]);
        } else {
            pdfFile = new File(DEFAULT_PDF_FILE_PATH);
        }

        final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();

        final RequestBody pdfFileRequestBody =
                RequestBody.create(pdfFile, MediaType.parse("application/pdf"));
        RequestBody ocrRequestBody =
                new MultipartBody.Builder()
                        .setType(MultipartBody.FORM)
                        .addFormDataPart("file", pdfFile.getName(), pdfFileRequestBody)
                        .addFormDataPart("output", "example_pdf-with-ocr-text_out")
                        .build();
        Request ocrRequest =
                new Request.Builder()
                        .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
                        .url("https://api.pdfrest.com/pdf-with-ocr-text")
                        .post(ocrRequestBody)
                        .build();
        try {
            OkHttpClient ocrClient =
                    new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();

            Response ocrResponse = ocrClient.newCall(ocrRequest).execute();

            System.out.println("Response status code: " + ocrResponse.code());
            if (ocrResponse.body() != null) {
                String ocrResponseString = ocrResponse.body().string();

                JSONObject ocrJSON = new JSONObject(ocrResponseString);
                if (ocrJSON.has("error")) {
                    System.out.println("Error during OCR call: " + ocrResponseString);
                    return;
                }

                String ocrPDFID = ocrJSON.get("outputId").toString();
                System.out.println("Got the output ID: " + ocrPDFID);

                RequestBody extractRequestBody =
                        new MultipartBody.Builder()
                                .setType(MultipartBody.FORM)
                                .addFormDataPart("id", ocrPDFID)
                                .build();
                Request extractRequest =
                        new Request.Builder()
                                .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
                                .url("https://api.pdfrest.com/extracted-text")
                                .post(extractRequestBody)
                                .build();
                try {
                    OkHttpClient extractClient =
                            new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();

                    Response extractResponse = extractClient.newCall(extractRequest).execute();

                    System.out.println("Response status code: " + extractResponse.code());
                    if (extractResponse.body() != null) {
                        String extractResponseString = extractResponse.body().string();

                        JSONObject extractJSON = new JSONObject(extractResponseString);
                        if (extractJSON.has("error")) {
                            System.out.println("Error during text extraction call: " + extractResponseString);
                            return;
                        }

                        System.out.println(extractJSON.getString("fullText"));
                    }
                } catch (IOException e) {
                    throw new RuntimeException(e);
                }
            }
        } catch (IOException e) {
            throw new RuntimeException(e);
        }
    }
}