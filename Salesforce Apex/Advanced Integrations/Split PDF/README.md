<img src='https://pdfrest.com/_nuxt/image/ffb8c8.webp' alt='Salesforce Apex Integration'><br/>

# Integrate pdfRest with Salesforce Apex Code

### Learn how to send files from Salesforce to pdfRest for document processing and download output files back into Salesforce
<br/>

Just like thousands of companies around the world, we use Salesforce as a customer relationship management (CRM) service, customizing its many features to meet our specific business needs. Within our Salesforce environment, we generate, edit, and archive a trove of documents, including product license agreements, invoices, and customer support case files, and we have developed countless automated business processes, both within Salesforce and through integrations with external tools and services. Through conversations with users, we've discovered that we're not alone - many companies just like us are searching for efficient ways to bring together:

            Salesforce + document processing + automation

This combination is notoriously challenging to accomplish (for reasons we'll discuss below), and you won't find many solutions for integrating a complete toolkit of document processing capabilities directly with Salesforce.

Fortunately, you've come to the right place! In this tutorial, we'll step through a solution that connects Salesforce with our pdfRest API Toolkit, simply, and seemlessly. Whether you're aiming to compress, split, merge, convert, or password-protect PDFs stored in Salesforce, we've got you covered.
<br/><br/>

## The Challenge
Integrating Salesforce with a reliable toolkit for processing documents has traditionally been a challenge for developers. The native Salesforce coding language, Apex, has a class called [HttpRequest](https://developer.salesforce.com/docs/atlas.en-us.apexref.meta/apexref/apex_classes_restful_http_httprequest.htm) for sending http requests from within Salesforce to external API services, but it does not include a convenient way to send files along with these requests. Searching for answers online leads to a variety of dead ends, including broken tutorials, untested theoretical concepts, and posts that indicate this cannot be done. At pdfRest, we decided to accept the challenge and find a way to connect to our own PDF processing REST API toolkit using Salesforce Apex code.
<br/><br/>

## The Trick
As Salesforce Apex does not support a convenient way to include files with http requests to external services, we'll have to generate the low-level body payload of our request from scratch. This may sound complex, but we'll break it down and walk you through it.

We'll set our Content-Type to `multipart/form-data` and carefully construct the body payload, which can contain a mix of form field parameters with text values and files with binary content. This body payload must be formatted with perfect precision in order for the receiving service to interpret and process what it contains. Here is an example of a formatted body payload, including a boundary string, a form field parameter called "output", and a PDF file (with a placeholder for the binary content):
<br/><br/>

<pre>
   ----pdfRest-boundary-string 
   Content-Disposition: form-data; name="output"; 
    
   pdf_output_name 
   ----pdfRest-boundary-string 
   Content-Disposition: form-data; name="file"; filename="file_name.pdf" 
   Content-Type: text/octet-stream 
    
   BINARY_FILE_CONTENT
   ----pdfRest-boundary-string--
</pre>
<br/>

Note that the boundary string, which can be just about any random string, appears at the start, separates each section, and indicates where the body payload ends. 2 dashes are appended to the beginning of each instance of this string, and 2 dashes are also appended to the end for the final "footer" boundary. Each section includes header information to specify the type of field and information about the field, followed by the value of the field. For our file field, the value would be the full binary content of the file we are attaching to our request. Line breaks must be placed to match the above schema precisely.
<br/><br/>

## The Code
Next, let's apply some logic with Salesforce Apex code to generate a body payload that matches the above schema, and then we'll be able to send this out using the native `HttpRequest` Apex class. We'll create a sample to demonstrate splitting a PDF into two output files - one with all of the odd numbered pages and the other with the even numbered pages. Let's take a look at the following code sample:
<br/><br/>

    public static void splitFile(Id fileId){

    	ContentVersion file =  [SELECT Id, Title, VersionData, FileExtension 
        FROM ContentVersion WHERE ContentDocumentID =: fileId limit 1];

        String file_name = file.Title + '.' + file.FileExtension; 
 
        String boundary = '--pdfRest-boundary-string';
      
        String param_pages1 = '--' + boundary + 
        '\r\nContent-Disposition: form-data; name="pages[]"\r\n\r\n' + 'odd';    
        String param_pages2 = '\r\n--' + boundary + 
        '\r\nContent-Disposition: form-data; name="pages[]"\r\n\r\n' + 'even'; 
        
        String file_header = '\r\n--' + boundary + 
        '\r\nContent-Disposition: form-data; name="file"; filename="' + 
        file_name + '"\r\nContent-Type: application/octet-stream\r\n\r\n';
        
        Blob file_body = file.VersionData;
        
        String footer = '\r\n--'+boundary+'--';
    
        String combinedDataAsHex = 	
          EncodingUtil.convertToHex(Blob.valueOf(param_pages1)) + 
          EncodingUtil.convertToHex(Blob.valueOf(param_pages2)) +   
          EncodingUtil.convertToHex(Blob.valueOf(file_header)) + 
          EncodingUtil.convertToHex(file_body) + 
          EncodingUtil.convertToHex(Blob.valueOf(footer));
        
        Blob bodyBlob = EncodingUtil.convertFromHex(combinedDataAsHex);
            
        HttpRequest req = new HttpRequest();
        req.setHeader('Content-Type', 'multipart/form-data; boundary=' + boundary);
        req.setHeader('Accept', 'application/json');
        req.setHeader('Api-Key', 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx');
        req.setMethod('POST');
        req.setEndpoint('https://api.pdfrest.com/split-pdf');
        req.setBodyAsBlob(bodyBlob);
        req.setTimeout(120000);
        Http http = new Http();
        HTTPResponse res = http.send(req);
        System.debug(res.getBody());	
    }
<br/>
    
This code creates an Apex method called `splitFile` that takes in a `fileId` parameter. It performs a query to find the file based on its ID then creates the body payload with two `pages[]` text parameters, specifying the pages to include in each of two output files, and adds the file field with the binary content of the file included.

Our strings and binary data cannot be directly combined, so we convert both types of content to Hex encoding, which allows us to combine all of this data, and then we convert back to binary and store the full body payload in a `Blob` type variable. This payload can now be added to our http request using the [setBodyAsBlob()](https://developer.salesforce.com/docs/atlas.en-us.apexref.meta/apexref/apex_classes_restful_http_httprequest.htm#apex_System_HttpRequest_setBodyAsBlob) method from the `HttpRequest` class. At the end of this sample, we print the response from the external pdfRest service to the log with a `System.debug()` message.

Now that we're sending calls out with files attached and receiving back a response, let's complete the document processing solution by downloading the split PDF output files and attaching them to an object in Salesforce. We also have the option to add them to the Files related list for that object. Here's our complete and final code:
<br/><br/>

    public class split_pdf {
      
        public static void splitFile(Id fileId, String parentId){
  
      	  ContentVersion file =  [SELECT Id, Title, VersionData, FileExtension
            FROM ContentVersion WHERE ContentDocumentID =: fileId limit 1];
  
            String file_name = file.Title + '.' + file.FileExtension; 
   
            String boundary = '--pdfRest-boundary-string';
        
            String param_pages1 = '--' + boundary + 
            '\r\nContent-Disposition: form-data; name="pages[]"\r\n\r\n' + 'odd';    
            String param_pages2 = '\r\n--' + boundary + 
            '\r\nContent-Disposition: form-data; name="pages[]"\r\n\r\n' + 'even'; 
          
            String file_header = '\r\n--' + boundary + 
            '\r\nContent-Disposition: form-data; name="file"; filename="' + 
            file_name + '"\r\nContent-Type: application/octet-stream\r\n\r\n';
          
            Blob file_body = file.VersionData;
          
            String footer = '\r\n--'+boundary+'--';
      
            String combinedDataAsHex = 	
              EncodingUtil.convertToHex(Blob.valueOf(param_pages1)) + 
              EncodingUtil.convertToHex(Blob.valueOf(param_pages2)) +
              EncodingUtil.convertToHex(Blob.valueOf(file_header)) + 
              EncodingUtil.convertToHex(file_body) + 
              EncodingUtil.convertToHex(Blob.valueOf(footer));
          
            Blob bodyBlob = EncodingUtil.convertFromHex(combinedDataAsHex);
              
            HttpRequest req = new HttpRequest();
            req.setHeader('Content-Type','multipart/form-data; boundary='+boundary);
            req.setHeader('Accept', 'application/json');
            req.setHeader('Api-Key', 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx');
            req.setMethod('POST');
            req.setEndpoint('https://api.pdfrest.com/split-pdf');
            req.setBodyAsBlob(bodyBlob);
            req.setTimeout(120000);
            Http http = new Http();
            HTTPResponse res = http.send(req);
          
         	  Map<String, Object> m = (Map<String, Object>) 
            JSON.deserializeUntyped(res.getBody());
            List<Object> urls = (List<Object>)m.get('outputUrl');
          
            List<Blob> files = new List<Blob>();
            for (Object url : urls) {
                Blob b = downloadFile(url.toString());
                files.add(b);
            } 
            Integer i = 1;
            for (Blob f : files) {       
      
                // Attach to parent object
                attachFile(f, file.Title + '_split_' + i, parentId, false); 
      
                // Attach to parent object and add to Files list
            	  //attachFile(f, file.Title + '_split_' + i, parentId, true); 
      
            	  i++;
            }
        }
      
        public static Blob downloadFile(String fileUrl){              
            Http h = new Http();
            HttpRequest req = new HttpRequest();
            req.setEndpoint(fileUrl);
            req.setTimeout(60000);
            req.setMethod('GET');
            HttpResponse res = h.send(req);
            Blob body = res.getBodyAsBlob();
            return body;       
        }   
      
        public static void attachFile(Blob file, String fileName, String parentId, 
        Boolean addToFiles){
            if(!addToFiles) {
                Attachment att = new Attachment(Name = fileName, Body = file, 
                ContentType = 'application/pdf', ParentId = parentId);
            	  insert att;
            }
            else {     
                ContentVersion conVer = new ContentVersion();
                conVer.ContentLocation = 'S';
                conVer.PathOnClient = fileName + '.pdf';
                conVer.Title = fileName;
                conVer.VersionData = file;
                insert conVer;
              
                Id conDoc = [SELECT ContentDocumentId FROM ContentVersion 
                WHERE Id =:conVer.Id].ContentDocumentId;
                ContentDocumentLink conDocLink = New ContentDocumentLink();
                conDocLink.LinkedEntityId = parentId; 
                conDocLink.ContentDocumentId = conDoc;
                conDocLink.shareType = 'V';
                insert conDocLink;
            }
        }
    }
<br/>

This code adds a `parentId` parameter to the `splitPDF` method to indicate where to attach the output files we get back. It shows how to deserialize the JSON response from the pdfRest service and parse the `outputUrl` values we need to retrieve the files. It also introduces two new methods called `downloadFile` and `attachFile`, which perform the functions their names indicate to download the output files and attach them to a Salesforce object. By setting the `addToFiles` parameter to `true`, you can optionally add the attached files to the Files related list associated with the Salesforce object.
<br/><br/>

## The Takeaway
The concepts in this tutorial should apply more broadly for sending files with Apex via http request to other services as well, but we cannot guarantee that all other services will accept the format of requests demonstrated here. Of course, we can guarantee that our code samples will work with pdfRest API Toolkit!

Ready to test this integration? If you're set up to develop within your Salesforce account, all you'll need is the code above and a pdfRest API Key - [generate one now for free](https://pdfrest.com/getstarted)!
