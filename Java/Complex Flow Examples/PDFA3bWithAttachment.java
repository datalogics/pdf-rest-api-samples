import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONObject;

/* In this sample, we will show how to attach an xml document to a PDF file and then
* convert the file with the attachment to conform to the PDF/A standard, which
* can be useful for invoicing and standards compliance. We will be running the
* input document through /pdf-with-added-attachment to add the attachment and
* then /pdfa to do the PDF/A conversion.

* Note that there is nothing special about attaching an xml file, and any appropriate
* file may be attached and wrapped into the PDF/A conversion.
*/

public class PDFA3bWithAttachment {

  // Specify the path to your file here, or as the first argument when running the program.
  private static final String DEFAULT_FILE_PATH = "/path/to/file.pdf";

  // Specify the path to your file attachment here, or as the second argument when running the
  // program.
  private static final String DEFAULT_ATTACHMENT_PATH = "/path/to/file.xml";

  // Specify your API key here, or in the environment variable PDFREST_API_KEY.
  // You can also put the environment variable in a .env file.
  private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

  public static void main(String[] args) {
    File inputFile, attachmentFile;
    if (args.length > 1) {
      inputFile = new File(args[0]);
      attachmentFile = new File(args[1]);
    } else {
      inputFile = new File(DEFAULT_FILE_PATH);
      attachmentFile = new File(DEFAULT_ATTACHMENT_PATH);
    }

    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();

    final RequestBody attachmentInputFileRequestBody =
        RequestBody.create(inputFile, MediaType.parse("application/pdf"));
    final RequestBody attachmentFileRequestBody =
        RequestBody.create(attachmentFile, MediaType.parse("application/xml"));
    RequestBody attachmentRequestBody =
        new MultipartBody.Builder()
            .setType(MultipartBody.FORM)
            .addFormDataPart("file", inputFile.getName(), attachmentInputFileRequestBody)
            .addFormDataPart("file_to_attach", attachmentFile.getName(), attachmentFileRequestBody)
            .addFormDataPart("output", "pdfrest_attachment")
            .build();
    Request attachmentRequest =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url("https://api.pdfrest.com/pdf-with-added-attachment")
            .post(attachmentRequestBody)
            .build();
    try {
      OkHttpClient attachmentClient =
          new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();

      Response attachmentResponse = attachmentClient.newCall(attachmentRequest).execute();

      System.out.println("Result code from attachment call: " + attachmentResponse.code());
      if (attachmentResponse.body() != null) {
        String attachmentResponseString = attachmentResponse.body().string();

        JSONObject attachmentJSON = new JSONObject(attachmentResponseString);
        if (attachmentJSON.has("error")) {
          System.out.println("Error during attachment call: " + attachmentResponse.body().string());
          return;
        }

        String attachmentID = attachmentJSON.get("outputId").toString();

        RequestBody pdfaRequestBody =
            new MultipartBody.Builder()
                .setType(MultipartBody.FORM)
                .addFormDataPart("id", attachmentID)
                .addFormDataPart("output_type", "PDF/A-3b")
                .addFormDataPart("output", "pdfrest_pdfa")
                .build();
        Request pdfaRequest =
            new Request.Builder()
                .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
                .url("https://api.pdfrest.com/pdfa")
                .post(pdfaRequestBody)
                .build();
        try {
          OkHttpClient pdfaClient =
              new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();
          Response pdfaResponse = pdfaClient.newCall(pdfaRequest).execute();
          System.out.println("Result code from pdfa call: " + pdfaResponse.code());
          if (pdfaResponse.body() != null) {
            System.out.println(prettyJson(pdfaResponse.body().string()));
          }
        } catch (IOException e) {
          throw new RuntimeException(e);
        }
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
