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
