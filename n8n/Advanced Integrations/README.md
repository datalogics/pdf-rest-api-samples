### Tutorial

<br>

#### Objective

In this post we'll cover how to:

1. Retrieve a JPEG from a Dropbox URL
2. Upload the image to the pdfRest API Toolkit service
3. Convert the image to a .pdf document
4. Upload the newly converted PDF file back into Dropbox
6. Merge that PDF file with another

#### Uploading the Image

Files intended for the pdfRest API service will first need to be retrieved from somewhere. The file storage service Dropbox, used in this tutorial, is one of many options available within n8n. In the example below we download the file `image.jpeg` and stored that in the n8n workflow as `dropbox_jpeg`.

![pdfrest-n8n-solution-1.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-n8n-solution-1.png)

The next step is to create an **HTTP Request** that uploads the file to pdfRest using the /upload endpoint.

![pdfrest-n8n-solution-2.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-n8n-solution-2.png)

For the **HTTP Request**, set up a **POST** to `https://api.pdfrest.com/upload`. Leave **Authentication** set to `None`. Authorization will be done in the Header further down this guide.

![pdfrest-n8n-solution-3.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-n8n-solution-3.png)

Create one **Header Parameter** for `api-key` and set your API Key in the value. Your API Key can be found, when logged in, at the very bottom of the [Account page](https://pdfrest.com/account).

![pdfrest-n8n-solution-4.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-n8n-solution-4.png)

Lastly create a **Form-Data body type**. The only **Parameter Type** that we need here is a **n8n Binary File** referencing `dropbox_jpeg` from first step.

![pdfrest-n8n-solution-5.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-n8n-solution-5.png)

When run, the steps above will output a JSON formatted response from pdfRest with the `name` and `id` of the uploaded file(s), to be used later inside by n8n. This is an example of the output:

![pdfrest-n8n-solution-6.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-n8n-solution-6.png)

#### Converting the Image to PDF

Create another **HTTP Request** step, this time to convert the image to a PDF. This will be a **POST** to `https://api.pdfrest.com/pdf`.

![pdfrest-n8n-solution-7.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-n8n-solution-7.png)

Use the same Header Parameter for `api-key` as in the Upload call above.

This time we will have a **JSON Body Content Type**. Create a **Body Paramter** for `id`. The value of `id` will come from the JSON response of Upload call above. Note that you can click and drag on the `id` field in the **Schema** tab drop the correct variable directly into the `Value` tab (`{{ $json.files[0].id }}` in the image below).

![pdfrest-n8n-solution-8.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-n8n-solution-8.png)

The JSON output of this API call to /pdf will contain:
-	The `inputId` of our previously uploaded image file
-	The `outputId` of the newly converted PDF file
-	The `outputUrl` to download the converted PDF file

![pdfrest-n8n-solution-9.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-n8n-solution-9.png)

#### Downloading the PDF

In order to retrieve the newly converted PDF you will create a GET **HTTP Request** using the value of the `outputUrl`. No body or header needed for this step. This will produce a binary file data object (in this case, a technical way of saying a .pdf file) to be sent wherever you wish. In this example, we are downloading the results back into Dropbox.


![pdfrest-n8n-solution-10.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-n8n-solution-10.png)
![pdfrest-n8n-solution-11.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-n8n-solution-11.png)

#### Merging PDFs with n8n

Next we will use the `/merged-pdf` endpoint to merge the converted PDF with another PDF.

The first step is to download the new PDF from Dropbox just like we did before with the JPEG. Again, the files in your workflow can come from anywhere, we are using Dropbox in this example.

![pdfrest-n8n-solution-12.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-n8n-solution-12.png)

Make another **HTTP Request** to `https://api.pdfrest.com/upload` with the same `api-key` Header Parameter as before and the only difference being that we are uploading the PDF from Dropbox this time. The new **Input Data Field Name** will be `dropbox_pdf`.
![pdfrest-n8n-solution-13.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-n8n-solution-13.png)

To merge the files we will make an **HTTP Request** to `https://api.pdfrest.com/merged-pdf`. This will use the POST **Method** with the `api-key` Header set as before.

![pdfrest-n8n-solution-14.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-n8n-solution-14.png)

For the **Body Parameters**, set the **Content Type** to `JSON` and set **Specify Body** to `Using JSON`. In this merge example, we will be sending two PDF files using their `id` values, merging page 1 of each file into a new 2-page document. The `id` values are pulled from the `outputId` from the Convert to PDF from JPEG step, and the `id` of the Upload PDF to pdfRest step. Again, you can drag and drop the values, indicated by the red arrows in the image below:

![pdfrest-n8n-solution-15.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-n8n-solution-15.png)

Since the `id` values are being pulled from existing steps, they will be dynamically generated every time the workflow runs. This is an example of our JSON:

```
{
    "id": [
        "132143265-bbac-445d-80f0-43629d94a97e",
        "21abec90f-7c2e-4447-8711-85f470362940"
    ],
    "type": [
        "id",
        "id"
    ],
    "pages": [
        1,
        1
    ]
}
```

Just as when we called /pdf above, this **HTTP Request** will result in a JSON object with `inputId`s, an `outputId`, and an `outputUrl` that can be used to download the final, merged document.

![pdfrest-n8n-solution-16.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-n8n-solution-16.png)
