package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"io"
	"mime/multipart"
	"net/http"
	"os"
)

// In this sample, we will show how to convert a scanned document into a PDF with
// searchable and extractable text using Optical Character Recognition (OCR), and then
// extract that text from the newly created document.
//
// First, we will upload a scanned PDF to the /pdf-with-ocr-text route and capture the
// output ID. Then, we will send the output ID to the /extracted-text route, which will
// return the newly added text.

func main() {
	baseUrl := "https://api.pdfrest.com/"

	// Replace the values below with your input file's location and name
	inputFilePath := "/path/to/input.pdf"
	inputFileName := "input.pdf"

	// Replace with your API key
	apiKey := "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"

	// Begin request to /pdf-with-ocr-text
	// Create a buffer and a multipart writer
	var ocrReqBody bytes.Buffer
	ocrReqWriter := multipart.NewWriter(&ocrReqBody)

	// Open the input file
	fileField := "file"
	file, err := os.Open(inputFilePath)
	if err != nil {
		panic(err)
	}
	defer file.Close()

	filePart, err := ocrReqWriter.CreateFormFile(fileField, inputFileName)
	if err != nil {
		panic(err)
	}
	_, err = io.Copy(filePart, file)
	if err != nil {
		panic(err)
	}

	err = ocrReqWriter.Close()
	if err != nil {
		panic(err)
	}

	// Create the HTTP request
	ocrReq, err := http.NewRequest("POST", baseUrl+"pdf-with-ocr-text", &ocrReqBody)
	if err != nil {
		panic(err)
	}

	// Set the headers
	ocrReq.Header.Set("Content-Type", ocrReqWriter.FormDataContentType())
	ocrReq.Header.Set("Api-Key", apiKey)

	// Send the request
	client := &http.Client{}
	ocrResp, err := client.Do(ocrReq)
	if err != nil {
		panic(err)
	}
	defer ocrResp.Body.Close()

	var ocrRespData map[string]interface{}
	err = json.NewDecoder(ocrResp.Body).Decode(&ocrRespData)
	if err != nil {
		panic(err)
	}
	errorMessage, hasErrorMessage := ocrRespData["error"]
	if hasErrorMessage {
		fmt.Println("ERR:", errorMessage)
	} else {
		// Begin request to /extracted-text
		var extractReqBody bytes.Buffer
		extractReqWriter := multipart.NewWriter(&extractReqBody)

		// Add the "id" form field
		err = extractReqWriter.WriteField("id", ocrRespData["outputId"].(string))
		if err != nil {
			panic(err)
		}
		err = extractReqWriter.Close()
		if err != nil {
			panic(err)
		}
		extractReq, err := http.NewRequest("POST", baseUrl+"extracted-text", &extractReqBody)
		if err != nil {
			panic(err)
		}

		extractReq.Header.Set("Content-Type", extractReqWriter.FormDataContentType())
		extractReq.Header.Set("Api-Key", apiKey)

		extractResp, err := client.Do(extractReq)
		if err != nil {
			panic(err)
		}
		defer extractResp.Body.Close()

		var extractRespData map[string]interface{}
		err = json.NewDecoder(extractResp.Body).Decode(&extractRespData)
		if err != nil {
			panic(err)
		}
		errorMessage, hasErrorMessage := extractRespData["error"]
		if hasErrorMessage {
			fmt.Println("ERR:", errorMessage)
		} else {
			fmt.Println(extractRespData["fullText"])
		}
	}
}
