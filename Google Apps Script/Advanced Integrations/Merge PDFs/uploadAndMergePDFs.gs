function uploadAndMergePDFs(pdfFiles) {
  // Replace with your actual API key
  const apiKey = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

  const uploadUrl = "https://api.pdfrest.com/upload";
  const mergeUrl = "https://api.pdfrest.com/merged-pdf";

  const uploadedFiles = [];
  
  const pagesArr = [];
  const typeArr = [];
  
  // Upload each PDF file
  for (const pdfFile of pdfFiles) {
    const uploadData = pdfFile.getBlob().getBytes();
    const uploadOptions = {
      "method" : "post",
      "payload" : uploadData,
      "headers" : {
        "Api-Key": apiKey,
        "Content-Filename": pdfFile.getName(),
        "Content-Type": "application/octet-stream"
      }
    };
    
    const uploadResponse = UrlFetchApp.fetch(uploadUrl, uploadOptions);
    if (uploadResponse.getResponseCode() !== 200) {
      throw new Error(`Failed to upload file: ${uploadResponse.getContentText()}`);
    }
    
    uploadedFiles.push(JSON.parse(uploadResponse.getContentText()).files[0].id);
    pagesArr.push("1-last");
    typeArr.push("id");
  }


  // Prepare merge request data
  const mergeData = {
    "id": uploadedFiles,
    "type": typeArr,
    "pages": pagesArr,
  };
  
  const mergeOptions = {
    "method" : "post",
    "payload" : JSON.stringify(mergeData),
    "headers" : {
      "Api-Key": apiKey,
      "Content-Type": "application/json"
    }
  };

  const mergeResponse = UrlFetchApp.fetch(mergeUrl, mergeOptions);
  if (mergeResponse.getResponseCode() !== 200) {
    throw new Error(`Failed to merge PDFs: ${mergeResponse.getContentText()}`);
  }
  
  console.log("PDFs merged successfully!");

  // You can access the response data here:
  const mergedPdfInfo = JSON.parse(mergeResponse.getContentText());

  return (mergedPdfInfo.outputId);
}


function getFile(fileId, folderId) {
  const options = {
    "method": "get",
    "responseType": UrlFetchApp.BLOB, // Specify response type as blob
  };

  const response = UrlFetchApp.fetch(`https://api.pdfrest.com/resource/${fileId}?format=file`, options);
  const pdfBlob = response.getBlob(); // Directly get the blob from the response

  const folder = DriveApp.getFolderById(folderId);
  const result = folder.createFile(pdfBlob);
  console.log("Merged PDF downloaded!");
}

// Example usage - merge all PDFs in Google Drive folder
const folderId = "xxxxx-xxxxxx-xxxxxxxx_xxxxxxxxxxx"; // Replace with the ID of your target folder
const folder = DriveApp.getFolderById(folderId);
const files = folder.getFiles();

const pdfFiles = [];
while (files.hasNext()) {
  const file = files.next();
  if (file.getMimeType() === MimeType.PDF) {
    pdfFiles.push(file);
  }
}

const outputId = uploadAndMergePDFs(pdfFiles);

getFile(outputId,folderId);
