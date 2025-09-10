import io.github.cdimascio.dotenv.Dotenv;
import java.io.File;
import java.io.IOException;
import java.util.concurrent.TimeUnit;
import okhttp3.*;
import org.json.JSONObject;

/* In this sample, we will show how to merge different file types together as
* discussed in https://pdfrest.com/solutions/merge-multiple-types-of-files-together/.
First, we will upload an image file to the /pdf route and capture the output ID.
* Next, we will upload a PowerPoint file to the /pdf route and capture its output
* ID. Finally, we will pass both IDs to the /merged-pdf route to combine both inputs
* into a single PDF.
*
* Note that there is nothing special about an image and a PowerPoint file, and
* this sample could be easily used to convert and combine any two file types
* that the /pdf route takes as inputs.
*/

public class MergeDifferentFileTypes {

  // By default, we use the US-based API service. This is the primary endpoint for global use.
  private static final String API_URL = "https://api.pdfrest.com";

  // For GDPR compliance and enhanced performance for European users, you can switch to the EU-based
  // service by uncommenting the URL below.
  // For more information visit https://pdfrest.com/pricing#how-do-eu-gdpr-api-calls-work
  // private static final String API_URL = "https://eu-api.pdfrest.com";

  // Specify the path to your first file here, or as the first argument when running the program.
  private static final String DEFAULT_FIRST_FILE_PATH = "/path/to/file.png";

  // Specify the path to your second file here, or as the second argument when running the
  // program.
  private static final String DEFAULT_SECOND_FILE_PATH = "/path/to/file.ppt";

  // Specify your API key here, or in the environment variable PDFREST_API_KEY.
  // You can also put the environment variable in a .env file.
  private static final String DEFAULT_API_KEY = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

  public static void main(String[] args) {
    File firstFile, secondFile;
    if (args.length > 1) {
      firstFile = new File(args[0]);
      secondFile = new File(args[1]);
    } else {
      firstFile = new File(DEFAULT_FIRST_FILE_PATH);
      secondFile = new File(DEFAULT_SECOND_FILE_PATH);
    }

    final Dotenv dotenv = Dotenv.configure().ignoreIfMalformed().ignoreIfMissing().load();

    final RequestBody firstFileInputFileRequestBody =
        RequestBody.create(firstFile, MediaType.parse("application/png"));
    RequestBody firstFileRequestBody =
        new MultipartBody.Builder()
            .setType(MultipartBody.FORM)
            .addFormDataPart("file", firstFile.getName(), firstFileInputFileRequestBody)
            .addFormDataPart("output", "pdfrest_pdf")
            .build();
    Request firstFileRequest =
        new Request.Builder()
            .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
            .url(API_URL + "/pdf")
            .post(firstFileRequestBody)
            .build();
    try {
      OkHttpClient firstFileClient =
          new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();

      Response firstFileResponse = firstFileClient.newCall(firstFileRequest).execute();

      System.out.println("Result code from first PDF call: " + firstFileResponse.code());
      if (firstFileResponse.body() != null) {
        String firstFileResponseString = firstFileResponse.body().string();

        JSONObject firstFileJSON = new JSONObject(firstFileResponseString);
        if (firstFileJSON.has("error")) {
          System.out.println("Error during first PDF call: " + firstFileResponse.body().string());
          return;
        }

        String firstFileID = firstFileJSON.get("outputId").toString();

        final RequestBody secondFileInputFileRequestBody =
            RequestBody.create(secondFile, MediaType.parse("application/powerpoint"));
        RequestBody secondFileRequestBody =
            new MultipartBody.Builder()
                .setType(MultipartBody.FORM)
                .addFormDataPart("file", secondFile.getName(), secondFileInputFileRequestBody)
                .addFormDataPart("output", "pdfrest_pdf")
                .build();
        Request secondFileRequest =
            new Request.Builder()
                .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
                .url(API_URL + "/pdf")
                .post(secondFileRequestBody)
                .build();
        try {
          OkHttpClient secondFileClient =
              new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();

          Response secondFileResponse = secondFileClient.newCall(secondFileRequest).execute();

          System.out.println("Result code from second PDF call: " + secondFileResponse.code());
          if (secondFileResponse.body() != null) {
            String secondFileResponseString = secondFileResponse.body().string();

            JSONObject secondFileJSON = new JSONObject(secondFileResponseString);
            if (secondFileJSON.has("error")) {
              System.out.println(
                  "Error during second PDF call: " + secondFileResponse.body().string());
              return;
            }

            String secondFileID = secondFileJSON.get("outputId").toString();

            RequestBody mergeRequestBody =
                new MultipartBody.Builder()
                    .setType(MultipartBody.FORM)
                    .addFormDataPart("id[]", firstFileID)
                    .addFormDataPart("id[]", secondFileID)
                    .addFormDataPart("type[]", "id")
                    .addFormDataPart("type[]", "id")
                    .addFormDataPart("pages[]", "1-last")
                    .addFormDataPart("pages[]", "1-last")
                    .addFormDataPart("output", "pdfrest_merged")
                    .build();
            Request mergeRequest =
                new Request.Builder()
                    .header("Api-Key", dotenv.get("PDFREST_API_KEY", DEFAULT_API_KEY))
                    .url(API_URL + "/merged-pdf")
                    .post(mergeRequestBody)
                    .build();
            try {
              OkHttpClient mergeClient =
                  new OkHttpClient().newBuilder().readTimeout(60, TimeUnit.SECONDS).build();
              Response mergeResponse = mergeClient.newCall(mergeRequest).execute();
              System.out.println("Result code from merge call: " + mergeResponse.code());
              if (mergeResponse.body() != null) {
                System.out.println(prettyJson(mergeResponse.body().string()));
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
