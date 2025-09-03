## Seamless Automation with pdfRest and Zapier

[Zapier](https://zapier.com/) is an automation platform that connects different apps and services so they can work together without coding. It lets users create workflows (“Zaps”) that automatically trigger actions across tools like Gmail, Slack, Google Sheets, and thousands more.

While pdfRest is not currently listed within the Zapier marketplace, pdfRest is callable as a REST API service, allowing for easy integration with Zapier workflows and other low/no-code services.

Take a minute to [sign up for a free Starter account](https://pdfrest.com/getstarted) with pdfRest to generate a dedicated API Key, which will be required for sending calls to the service.

### Tutorial

When working with pdfRest in Zapier, there will always be one or more initial steps that precede the document processing steps.  In other words, something has to happen first that triggers pdfRest to kick into action. These preceding hooks/steps/flows can be customized to meet your specific workflow needs. All that really matters is that at some point, you end up with one or more files that are ready to be processed using any of the [API Tools](https://pdfrest.com/apitools/) within the pdfRest toolkit. 

The following tutorial will demonstrate a flow that is triggered when an email with an image file attachment is labeled in Gmail. This image file will automatically be processed with the pdfRest [Convert to PDF](https://pdfrest.com/apitools/convert-to-pdf/) tool, and the resulting PDF will be downloaded back into a folder in Google Drive.

We'll start with creating the label in Gmail, labeling an email, and creating the folder in Google Drive. 

On the sidebar on the Gmail Interface hit the `+` symbol to create a Label. For this tutorial we'll name the label "pdfRest-Zapier".

![Gmail Labels.png](https://cms.pdfrest.com/content/images/2025/09/Gmail-Labels.png)

![New Gmail Label.png](https://cms.pdfrest.com/content/images/2025/09/New-Gmail-Label.png)

Then, select an email with an attachment that you want to convert to PDF, and label it with the "pdfRest-Zapier" label. For this example we have a demo email with a JPG image file as an attachment.

![Gmail Label the Email.png](https://cms.pdfrest.com/content/images/2025/09/Gmail-Label-the-Email-1.png)

Create a folder in Google Drive. For this tutorial we'll name the folder "pdfRest-Zapier".

![Google Drive New Folder.png](https://cms.pdfrest.com/content/images/2025/09/Google-Drive-New-Folder.png)

![Google Drive pdfRest-Zapier Folder.png](https://cms.pdfrest.com/content/images/2025/09/Google-Drive-pdfRest-Zapier-Folder.png)

Sign up for Zapier and create a Zap. Note that we'll be using premium features of Zapier which will require a Professional or above account.

![Create A Zap.png](https://cms.pdfrest.com/content/images/2025/09/Create-A-Zap-1.png)

Start by selecting an event that will start your Zap (Trigger).

![Zap Trigger.png](https://cms.pdfrest.com/content/images/2025/09/Zap-Trigger.png)

Select the Gmail Trigger. You may also search for the Gmail integration by typing 'gmail' into the search box.

![Gmail Trigger.png](https://cms.pdfrest.com/content/images/2025/09/Gmail-Trigger.png)

For the Gmail Trigger Setup step, configure the options like so:

1. Set the Trigger event to **New Labeled Email**.
2. Log into your Google account under Account. There should be a "Select" button that will open a dialog so that you may log in.
3. You may optionally name this step whatever you'd like.

![Gmail Trigger Setup.png](https://cms.pdfrest.com/content/images/2025/09/Gmail-Trigger-Setup.png)

For the Configure step, configure the options like so:
Select the label you created earlier. In this example it is "pdfRest-Zapier".

![Gmail Trigger Configure.png](https://cms.pdfrest.com/content/images/2025/09/Gmail-Trigger-Configure.png)

For the Test step, hit "Test trigger" to test detecting new labeled emails.

![Gmail Trigger Test.png](https://cms.pdfrest.com/content/images/2025/09/Gmail-Trigger-Test-1.png)

A newly detected labeled email will appear, and clicking into it reveals the email metadata. Note that the subject matches the email that was labeled. After confirming things are correct, you can proceed to the next step by hitting "Continue with selected record".

![Detected Labeled Email.png](https://cms.pdfrest.com/content/images/2025/09/Detected-Labeled-Email.png)

Now you're ready to send your first call to pdfRest, which will use the [Upload Files](https://pdfrest.com/apitools/upload-files/) tool to prepare files for subsequent processing steps.

In the next step, select "Webhooks" as the action. This can be found under Utilities, or search 'webhooks' in the search bar.

![Zapier Webhooks.png](https://cms.pdfrest.com/content/images/2025/09/Zapier-Webhooks.png)

In the Setup step, select <code>POST</code> as the Action event.

![Webhook Setup.png](https://cms.pdfrest.com/content/images/2025/09/Webhook-Setup.png)

In the Configure step:
1. Set the URL to https://api.pdfrest.com/upload, or https://eu-api.pdfrest.com/upload if you want to make this automation GDPR-compliant. 
2. Under Data, set the key as <code>url</code> and the value as "All Attachments"
![Webhook Configure All Attachments.png](https://cms.pdfrest.com/content/images/2025/09/Webhook-Configure-All-Attachments.png)
3. Finally, under Headers, set your API Key as the value for Api-Key.
![Webhook Configure Api-Key.png](https://cms.pdfrest.com/content/images/2025/09/Webhook-Configure-Api-Key.png)

In the Test step, you can test the upload step, and if this step succeeds, you will see a success with a return that looks similar to the following response:

![Webhook Test Successful Upload.png](https://cms.pdfrest.com/content/images/2025/09/Webhook-Test-Successful-Upload.png)

Now that you have a step that is uploading a file and returning a JSON response, you'll need to pass the results of that response to another webhook action to do the actual conversion step.

Add another step to the workflow with the + Add Step button, and select Webhooks again. In the Setup step, select <code>POST</code> as the Action event.

![Zapier Add Step.png](https://cms.pdfrest.com/content/images/2025/09/Zapier-Add-Step.png)

In the Configure step:
1. Set the URL to https://api.pdfrest.com/pdf, or https://eu-api.pdfrest.com/pdf if you want to make this automation GDPR-compliant. Follow the convention of the url you made the request to in the previous Webhook configuration.
2. Under Data, set the key as <code>id</code> and the value as "Files ID".
![Webhook Configure pdf endpoint options.png](https://cms.pdfrest.com/content/images/2025/09/Webhook-Configure-pdf-endpoint-options-1.png)
3. Finally, under Headers, set your API Key as the value for Api-Key, as done in the previous webhook.

In the Test step, you can test the conversion step, and if this step succeeds, you will see a success with a return that looks similar to the following response:

![Webhook Test Successful PDF Conversion.png](https://cms.pdfrest.com/content/images/2025/09/Webhook-Test-Successful-PDF-Conversion.png)

Finally, we'll add a step to upload the file to a Google Drive folder. Add another step to the workflow with the + Add Step button, and select the Google Drive integration. 

In the Setup step, configure the options like so:
1. Set the Action event to `Upload File`.
2. Log into your Google Drive account under Account.

![Webhook Setup Google Drive.png](https://cms.pdfrest.com/content/images/2025/09/Webhook-Setup-Google-Drive.png)

In the Configure step, configure the options like so:
1. Choose where the uploaded file will go under Drive and Folder. In this example it will be under "My Google Drive" and "pdfRest-Zapier".
2. Under File, choose the <code>outputUrl</code> returned from the previous Webhook step (step 3 in this example).
3. Optionally configure the "Convert to Document?", "File Name", and "File Extension" options to your needs.

![Webhook Configure Google Drive.png](https://cms.pdfrest.com/content/images/2025/09/Webhook-Configure-Google-Drive-1.png)

In the Test step, you can test the final upload step, and if this step succesds, you will see a success with a return that looks similar to the following response:

![Webhook Test Google Drive Upload.png](https://cms.pdfrest.com/content/images/2025/09/Webhook-Test-Google-Drive-Upload.png)

And you will see the final converted PDF file in Google Drive.

![Google Drive Uploaded Converted PDF.png](https://cms.pdfrest.com/content/images/2025/09/Google-Drive-Uploaded-Converted-PDF.png)


Of course, you can customize the end step to pass pdfRest output files anywhere you need to send them for the next steps in their journey.
<br/>

### Support
If you have any trouble getting this set up or would like more information about how pdfRest can solve your PDF processing challenges, please [let us know](https://pdfrest.com/support/) how we can help!