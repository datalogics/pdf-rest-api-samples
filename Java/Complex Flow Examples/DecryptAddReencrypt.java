import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONObject;

/* In this sample, we will show how to take an encrypted file and decrypt, modify
 * and re-encrypt it to create an encryption-at-rest solution as discussed in
 * https://pdfrest.com/solutions/create-secure-document-workflows-with-pdf-password-protection/
 * We will be running the document through /decrypted-pdf to open the document
 * to modification, running the decrypted result through /pdf-with-added-image,
 * and then sending the output with the new image through /encrypted-pdf to
 * lock it up again.
 */

public class DecryptAddReencrypt {

  // Specify the path to your PDF file here, or as the first argument when running the program.
  private static final String DEFAULT_FILE_PATH = "/path/to/file.pdf";

  // Specify the path to your image file here, or as the second argument when running the
  // program.
  private static final String DEFAULT_IMAGE_PATH = "/path/to/file.jpg";

  // Specify your API key here, or in the environment variable PDFREST_API_KEY.
  // You can also put the environment variable in a .env file.
  private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

  public static void main(String[] args) {
    File inputFile, imageFile;
    if (args.length > 1) {
      inputFile = new File(args[0]);
      imageFile = new File(args[1]);
    } else {
      inputFile = new File(DEFAULT_FILE_PATH);
      imageFile = new File(DEFAULT_IMAGE_PATH);
    }

    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();

    final RequestBody inputFileRequestBody =
        RequestBody.create(inputFile, MediaType.parse("application/pdf"));
    RequestBody decryptRequestBody =
        new MultipartBody.Builder()
            .setType(MultipartBody.FORM)
            .addFormDataPart("file", inputFile.getName(), inputFileRequestBody)
            .addFormDataPart("current_open_password", "password")
            .addFormDataPart("output", "pdfrest_decrypted")
            .build();
    Request decryptRequest =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url("https://api.pdfrest.com/decrypted-pdf")
            .post(decryptRequestBody)
            .build();
    try {
      OkHttpClient decryptClient =
          new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();

      Response decryptResponse = decryptClient.newCall(decryptRequest).execute();

      System.out.println("Result code from decrypt call: " + decryptResponse.code());
      if (decryptResponse.body() != null) {
        String decryptResponseString = decryptResponse.body().string();

        JSONObject decryptJSON = new JSONObject(decryptResponseString);
        if (decryptJSON.has("error")) {
          System.out.println("Error during decrypt call: " + decryptResponse.body().string());
          return;
        }

        String decryptID = decryptJSON.get("outputId").toString();

        final RequestBody imageFileRequestBody =
            RequestBody.create(imageFile, MediaType.parse("application/png"));
        RequestBody addImageRequestBody =
            new MultipartBody.Builder()
                .setType(MultipartBody.FORM)
                .addFormDataPart("id", decryptID)
                .addFormDataPart("image_file", imageFile.getName(), imageFileRequestBody)
                .addFormDataPart("page", "1")
                .addFormDataPart("x", "0")
                .addFormDataPart("y", "0")
                .addFormDataPart("output", "pdfrest_added_image")
                .build();
        Request addImageRequest =
            new Request.Builder()
                .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
                .url("https://api.pdfrest.com/pdf-with-added-image")
                .post(addImageRequestBody)
                .build();
        try {
          OkHttpClient addImageClient =
              new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();

          Response addImageResponse = addImageClient.newCall(addImageRequest).execute();

          System.out.println("Result code from add image call: " + addImageResponse.code());
          if (addImageResponse.body() != null) {
            String addImageResponseString = addImageResponse.body().string();

            JSONObject addImageJSON = new JSONObject(addImageResponseString);
            if (addImageJSON.has("error")) {
              System.out.println(
                  "Error during add image call: " + addImageResponse.body().string());
              return;
            }

            String addImageID = addImageJSON.get("outputId").toString();

            RequestBody encryptRequestBody =
                new MultipartBody.Builder()
                    .setType(MultipartBody.FORM)
                    .addFormDataPart("id", addImageID)
                    .addFormDataPart("new_open_password", "password")
                    .addFormDataPart("output", "pdfrest_encrypted")
                    .build();
            Request encryptRequest =
                new Request.Builder()
                    .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
                    .url("https://api.pdfrest.com/encrypted-pdf")
                    .post(encryptRequestBody)
                    .build();
            try {
              OkHttpClient encryptClient =
                  new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();
              Response encryptResponse = encryptClient.newCall(encryptRequest).execute();
              System.out.println("Result code from encrypt call: " + encryptResponse.code());
              if (encryptResponse.body() != null) {
                System.out.println(prettyJson(encryptResponse.body().string()));
              }
            } catch (IOException e) {
              throw new RuntimeException(e);
            }
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
