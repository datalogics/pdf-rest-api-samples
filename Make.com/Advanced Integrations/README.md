![ntegrate pdfRest with Make Automation & Google Drive Storage](https://cms.pdfrest.com/content/images/2025/09/Solution-Integrate-pdfRest-with-Make-Automation.png)

## Low-code Automation with Make.com and pdfRest


[Make.com](https://www.make.com/) (formerly Integromat) is a convenient web application for streamlining the creation of automated workflows. Its web interface supports low/no-code incorporation of a wide variety of services. While pdfRest is not currently listed within their available services, pdfRest is callable via the built-in HTTP module, allowing for easy integration within Make scenarios.

To get started, take a minute and [sign up for a free Starter account](https://pdfrest.com/getstarted/) with pdfRest to generate a dedicated API Key, which will be required for sending calls to the service.

---

## Objective

This tutorial will demonstrate how to:

1.  Create a scenario that watches a Google Drive folder for new files.
2.  Upload the files to the pdfRest API.
3.  Process the uploaded files by compressing them with pdfRest.
4.  Retrieve the processed files and upload them to a different Google Drive folder.


---
## Step 1: Connect pdfRest to Google Drive

Begin by creating a new **Scenario**, either by clicking **+ Create scenario** (in the upper-right corner) or by opening the **Scenario Builder** in **Scenarios**.

![pdfrest-make-solution-1.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-1.png)

Before calling pdfRest, you will need to access your input file from an app such as Google Drive or OneDrive. This example will use Google Drive to watch a folder for newly created files. See [Make’s documentation](https://help.make.com/connect-to-google-services-using-a-custom-oauth-client) for directions on how to configure access to your particular file source.

In the new scenario view, click the **+ button** in the center to begin adding apps.

![pdfrest-make-solution-2.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-2.png)

Use the search field to locate and select the **Google Drive** app.

![pdfrest-make-solution-3.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-3.png)

View the available actions in the Google Drive app, either by clicking on **Google Drive** or by clicking **Show more**. Locate and select **Watch Files in a Folder**.

![pdfrest-make-solution-4.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-4.png)

Set **Connection** to your configured connection to Google Drive. In **Select the Folder to be Watched**, set the folder you wish to contain your input document(s).

Set **Limit** to the maximum number of files to be processed per each scenario run. (This example sets an arbitrarily large value of `99`.)

![pdfrest-make-solution-5.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-5.png)

Click **Save**. You will be prompted to set the initial file creation time to check for.

![pdfrest-make-solution-6.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-6.png)

Click **Save** to set the current time as the starting time and return to the **Scenario** view.

Before adding the next **Module**, upload a PDF file to your watched folder. Then, run the scenario by clicking the **Run once** button. This will enable you to view and select the output fields from this module when configuring the next module.

![pdfrest-make-solution-7.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-7.png)

Before calling pdfRest, any files detected during the first step must be made accessible. Click the **+** next to your first **Module** to add another **Google Drive** app, this time with the **Get a Share Link** action.

![pdfrest-make-solution-8.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-8.png)

Use the same **Connection** setting from the previous module.

In **File ID**, clicking the text field should reveal a context menu. Click `File ID` from the previous **Google Drive** **Module** to populate the field.

![pdfrest-make-solution-9.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-9.png)

Set **Role** to `Reader`, and set **Type** to `Anyone`. This will allow the next module to access new files detected by the first module.

![pdfrest-make-solution-10.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-10.png)

Click **Save** to confirm settings and return to the **Scenario** overview.

---
## Step 2: Upload from Google Drive to pdfRest

The next step is to upload the input file using the [**/upload** API endpoint in pdfRest](https://docs.pdfrest.com/pdfrest-api-toolkit-cloud/api-reference-guide/#/upload).

Click **+** to add a new **App**, then find the built-in **HTTP** **App**. (You may have to search for "http" first.)

![pdfrest-make-solution-11.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-11.png)

From the **HTTP App**, select the **Make an API Key Auth request** action.

![pdfrest-make-solution-12.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-12.png)

In **Credentials**, click **Add** to add your pdfRest API key. The API key must be placed `In the header` and named **Api-Key**.

![pdfrest-make-solution-13.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-13.png)

Click **Create**. This will ensure that the API key is added into the request header.

Ensure that your keychain is selected in **Credentials**. Next, enter `https://api.pdfrest.com/upload` into the **URL** field. Set the HTTP request **Method** to `POST`.

![pdfrest-make-solution-14.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-14.png)

Set **Body type** to `Multipart/form-data`.

In **Fields**, add a new item. Set **Field type** on the new item to `text` and the **Key** to `url`. Clicking the **Value** field should reveal a context menu containing response items from **Google Drive**. Scroll down to find and select `webContentLink`.

![pdfrest-make-solution-15.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-15.png)

Set **Parse response** to `Yes`. This will enable the following Module to read and use the value of the response obtained by this module.

![pdfrest-make-solution-16.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-16.png)

Click **Save** to add the app to the **Scenario** and return to the **Scenario** view.

With both **Apps** added, add a new file to your watched folder and then click **Run** once to run the **Scenario**. A green checkmark on all **Modules** means that the **Scenario** ran successfully.

![pdfrest-make-solution-17.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-17.png)

---
## Step 3: Compress PDF Files on Google Drive with pdfRest

The next step is to process the newly uploaded PDFs using the pdfRest API. This will use the [**/compressed-pdf** API endpoint](https://docs.pdfrest.com/pdfrest-api-toolkit-cloud/api-reference-guide/#/compressed-pdf).

Click **+** to add another **HTTP App** with the **Make an API Key Auth request** action.

Set the **URL**to the endpoint of the pdfRest tool you wish to use, in this **Scenario** we are using `https://api.pdfrest.com/compressed-pdf` to optimize the size of the document. This example will use the [Compress PDF API Tool to optimize the file size of the document](https://pdfrest.com/apitools/compress-pdf/).

Ensure that **Credentials** is set to the keychain you created earlier. Set **Method** to `POST`, and set **Body type** to `Multipart/form-data`.

![pdfrest-make-solution-18.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-18.png)

To set the input file, create a **Body Parameter** named `id` with a **Field Type** of `Text`. Set its value to the `id` field retrieved from the previous **HTTP App**.

![pdfrest-make-solution-19.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-19.png)

Add additional `Text` **Field Types** for any additional parameters in your request. For example, the Compress PDF API requires the `compression_level` parameter as shown below.

![pdfrest-make-solution-20.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-20.png)

Further down, set **Parse response** to `Yes`.

![pdfrest-make-solution-21.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-21.png)

Click **Save** to confirm settings.

Run your scenario once more by uploading a file and clicking **Run once** to ensure that everything is working and to obtain the field names of the response from the **File Processing Module**.

---
## Step 4: Send Compressed PDF Files to Google Drive

To retrieve the newly compressed file, add a third **HTTP App**, this time with the **Get a file** action.

![pdfrest-make-solution-22.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-22.png)


In **URL**, set the value to the `outputUrl` from the previous **HTTP App's** response.

![pdfrest-make-solution-23.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-23.png)

Click **Save** to add this Module. Run the **Scenario** once more with a new file to ensure that all requests run successfully.

From here, you may send the downloaded file to the location of your choice. A good example would be to use the **Upload a File** **Action** from the ** Google Drive** **App** to send the processed file to a specific Drive folder.

![pdfrest-make-solution-24.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-24.png)

Set **New Drive Location** and **New Folder Location** to indicate where to save the compressed file(s).

In **File**, use the value returned from the **Get a file** action of the **HTTP App**.

![pdfrest-make-solution-25.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-25.png)

Click **Save** to confirm.

Upload one or more files to the Google Drive folder and run the **Scenario** with **Run once**. If successful, all modules will display a green checkmark.

![pdfrest-make-solution-26.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-26.png)

Your processed file should now be available in your destination folder.

![pdfrest-make-solution-27.png](https://cms.pdfrest.com/content/images/2025/09/pdfrest-make-solution-27.png)


Of course, you can customize the end step to pass pdfRest output files anywhere you need to send them for the next steps in their journey.


If you have any trouble getting this set up or would like more information about how pdfRest can solve your PDF processing challenges, please [let us know](https://pdfrest.com/support/) how we can help!

---

## Conclusion

By following this guide, you've successfully created a powerful, automated workflow that integrates Make, Google Drive, pdfRest, and your documents. The pdfRest API Toolkit includes dozens of other endpoints for document signing, conversion, securing, and more - all of which can be integrated with Make and your document workflows.

Now that you've mastered the basics, what will you build next? Get started by [signing up for a free pdfRest Starter account](https://pdfrest.com/getstarted/) and explore the full range of what's possible with the pdfRest API.
