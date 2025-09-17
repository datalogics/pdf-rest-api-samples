![Integrate pdfRest with Bubble to Automate PDF Workflows](https://cms.pdfrest.com/content/images/2025/09/Solution-Integrate-pdfRest-with-Bubble-Automation.png)

# Enhancing Your Bubble App with pdfRest

[Bubble.io](https://bubble.io/) is a powerful no-code/low-code platform that lets you build and launch fully functional web applications visually, without writing any traditional code. Using its built-in **API Connector** plugin, you can connect your app to external APIs like pdfRest to create powerful, automated document workflows.

To get started, [sign up for a free Starter account](https://pdfrest.com/getstarted/) with pdfRest to generate your dedicated API Key. You’ll use this key to authenticate requests from Bubble.io.

---

## Objective

This tutorial will demonstrate how to:

1.  Install and configure the Bubble.io **API Connector** for pdfRest.
2.  Create Bubble visual elements for file upload and action buttons.
3.  Call pdfRest APIs for **Upload**, **Convert to PDF/A**, and **Merge PDFs**.
4.  Configure workflows to trigger the API calls and provide links to download your new files.

---
## Step 1: Install and Configure the API Connector Plugin

- In the Bubble **Editor**, open the **Plugins** tab, and select **Add plugins**.
- Search for and install the **API Connector**.
- Create a new API named `pdfRest`.

In the API setup:

- Add a **Shared header** for authentication:
  - **Key** = `Api-Key`
  - **Value** = your pdfRest API key
  - Check the box to mark this as **PRIVATE**

![Bubble API Connector.png](https://cms.pdfrest.com/content/images/2025/09/Bubble-API-Connector.png)

- For each endpoint, add a new POST call:
  - **Upload API** → `POST https://api.pdfrest.com/upload`
  - **Convert to PDF/A API** → `POST https://api.pdfrest.com/pdfa`
  - **Merge PDFs API** → `POST https://api.pdfrest.com/merged-pdf`
- Set the **Body type** as **Multipart/form-data**
- Ensure responses are returned in JSON for easy parsing

---
## Step 2: Define Your Desired API Endpoint Parameters

Each endpoint requires specific parameters in its API call. Click the name of the APIs below for direct links to their documentation.

- [**Upload API**](https://pdfrest.com/apitools/upload-files/)
  - Parameters
    - `file`: The file to be uploaded to the pdfRest server
    - `url`: The URL of a file publicly available on the internet to upload to the pdfRest server

![Upload API configuration.png](https://cms.pdfrest.com/content/images/2025/09/Upload-API-configuration.png)

- [**Convert to PDF/A API**](https://pdfrest.com/apitools/convert-to-pdfa/)
  - Parameters:
    - One of:
      - `file`: The file to be sent in the request's form data (dynamic)
      - `id`: The `id` of the uploaded file on the pdfRest server (dynamic)
    - `output_type`: The desired PDF/A type for the output file (optional)
    - `rasterize_if_errors_encountered`: Whether a page should be rasterized in the event an error is encountered (optional)
    - `output`: The name of the output file (optional)

![PDF:A API configuration.png](https://cms.pdfrest.com/content/images/2025/09/PDF-A-API-configuration.png)

- [**Merge PDFs API**](https://pdfrest.com/apitools/merge-pdfs/)
  - Parameters
    - `id`: The list of file id's uploaded to the pdfRest server
      - For `id`'s value, use `<idX>` where `X` is the number of the file, as shown in the image below. You may use as many `<idX>`'s as you like, but remember to include the same amount of values for the `type` and `pages` lists!
    - `type`: The list of input types for the given documents
      - Either `file` or `id`, but for the purposes of this example they will all be `id`
    - `pages`: What pages of each document should be in the final merged document (for this example, `1-last` uses all pages).

![Merge API configuration.png](https://cms.pdfrest.com/content/images/2025/09/Merge-API-configuration.png)

If you'd like to use a tool that isn't explicitly shown here, most tools can be easily added simply by defining the parameters as shown in the [pdfRest API Toolkit Reference Guide](https://docs.pdfrest.com/pdfrest-api-toolkit-cloud/api-reference-guide/).

---
## Step 3: Create Visual Elements in Bubble

1.  On your page, drag out a **Group**. This will contain the UI for the file action.
2.  Inside the **Group**, add a **File Uploader** element. This lets users select a PDF. For merging PDFs, use two or more **File Uploader** elements.
3.  Add a **Dropdown** if you want the user to select options (e.g., *PDF/A conformance level* via the `output_type` parameter).
4.  Add a **Button** (e.g. “Convert to PDF/A” or “Merge PDFs”).

These elements will connect to workflows that call the pdfRest API.

A simple visual implementation of Convert to PDF/A might look like the following:

![Convert to PDF:A.png](https://cms.pdfrest.com/content/images/2025/09/Convert-to-PDF-A-1.png)

For defining the options in the drop down, make sure to match the values *exactly* as defined in the [Convert to PDF/A documentation](https://docs.pdfrest.com/pdfrest-api-toolkit-cloud/api-reference-guide/#/pdfa).

![PDF:A Dropdown configuration.png](https://cms.pdfrest.com/content/images/2025/09/PDF-A-Dropdown-configuration.png)

A simple implementation of a Merge PDFs workflow might look like the following:

![Merge PDF.png](https://cms.pdfrest.com/content/images/2025/09/Merge-PDF.png)

Note that this uses *two* **File Uploader** components. You can customize this with as many as required for your particular implementation.

---
## Step 4: Configure Workflows

For each button, create a **Workflow** that connects the visual elements to the API calls:

#### Convert to PDF/A Workflow

A workflow for Convert to PDF/A might look like the following:

![PDF:A flow.png](https://cms.pdfrest.com/content/images/2025/09/PDF-A-flow.png)

To accomplish this, define the following:
- Click the **+** button, hover over **Plugins**, and select `pdfRest - PDF/A` from the options.
- Pass the `file` from the `FileUploader` for your `PDF/A` group.
- Also pass the `output_type` value from the `Dropdown` element you created earlier
  - Include any other optional values, such as `rasterize_if_errors_encountered` if you'd like to dynamically change the value from its default.

![PDF:A Parameters.png](https://cms.pdfrest.com/content/images/2025/09/PDF-A-Parameters.png)

- Finally, click the **+** button again and choose **Navigation** -> **Open an external website**
  - Here, you can use the `Result of step 1 (pdfRest - PDF/A)'s outputUrl`
  - If configured in this way, it will open the processed PDF into a new tab

![PDF:A URL destination.png](https://cms.pdfrest.com/content/images/2025/09/PDF-A-URL-destination.png)

#### Merge PDFs Workflow (Backend)

A **Workflow** for Merge PDFs might look like the following:

![Merge PDFs flow.png](https://cms.pdfrest.com/content/images/2025/09/Merge-PDFs-flow.png)

Before any other configuration, we'll define a new **Data type** called `OutputFile`. This will contain the `url` of the processed output file.
- Choose the **Data** tab in the navigation on the left side of the screen
- In the **New type** field, enter `OutputFile`

![New type.png](https://cms.pdfrest.com/content/images/2025/09/New-type.png)

- Add a **Field** on `OutputFile` called `url`

![Data type Define OutputFile.png](https://cms.pdfrest.com/content/images/2025/09/Data-type-Define-OutputFile.png)

Back in the **Workflow** tab, define the following:
- Click the **+** button, then choose **Plugins** -> `pdfRest - Upload`.
- Set either the `url` or `file` parameter to the user inputs (for the purposes of this example, `file` is used).
  - Repeat the above step for however many files the user is merging together.

![Merge PDFs upload files.png](https://cms.pdfrest.com/content/images/2025/09/Merge-PDFs-upload-files.png)

- Click the **+** button, then choose **Plugins** -> `pdfRest - Merge`.
- Define the `id` for each uploaded file with `Result of step X (pdfRest - Upload)'s files: first item's id`.
  - Make sure you choose `first item's id`, as there will always only be one item in the `files` array from each Upload call.
  - Note that for the purposes of this example, there are only two files, but this can be expanded to your needs.

![Merge PDFs ids.png](https://cms.pdfrest.com/content/images/2025/09/Merge-PDFs-ids.png)

- To use a **Download File** button, click the **+** button and choose, **Data (Things)** -> `Create a new thing...`
  - Choose the **Data type** `OutputFile` that was defined earlier
  - Set the `url` to be equal to `Result of step 3 (pdfRest - Merge)'s outputUrl`
- Finally, click the **+** button and choose, **Element Actions** -> `Display data in a group/popup`
  - Set the **Element** to `Group OutputFile`, and **Data to Display** as `Result of Step 4 (Create a new OutputFile...)`

![Display Data in Group OutputFile.png](https://cms.pdfrest.com/content/images/2025/09/Display-Data-in-Group-OutputFile.png)

When configured correctly, the **Download** button should then take you to the URL of the newly merged documents.

---
### Conclusion

By installing the API Connector, creating visual elements, and configuring workflows, you’ve now linked Bubble.io to the pdfRest API Toolkit's Upload, PDF/A, and Merge PDFs endpoints. The pdfRest API Toolkit includes many tools for Compression, Security, Digital Signatures, and so much more that can be connected in similar fashion.

Once you’ve mastered this foundation, you can extend your Bubble.io app into a complete document automation platform! [Sign up for a free pdfRest Starter account](https://pdfrest.com/getstarted/) and start exploring the possibilities.
