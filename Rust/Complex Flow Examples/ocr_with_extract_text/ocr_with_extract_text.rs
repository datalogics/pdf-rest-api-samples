use reqwest::Client;
use reqwest::header::{HeaderMap, HeaderValue, ACCEPT, CONTENT_TYPE};
use serde::{ Serialize, Deserialize };
use serde_json::json;
use tokio;
use tokio::fs::File;
use tokio::io::AsyncReadExt;

const INPUT_FILE_PATH: &str = "/path/to/input.pdf";
const INPUT_FILE_NAME: &str = "input.pdf";
const API_KEY: &str = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx";

#[tokio::main]
async fn main() -> Result<(), Box<dyn std::error::Error>> {
    // Send POST request to /upload with PDF file
    let client = Client::new();
    let mut file = File::open(INPUT_FILE_PATH).await?;
    let mut upload_contents = Vec::new();
    file.read_to_end(&mut upload_contents).await?;
    let mut upload_headers = HeaderMap::new();
    upload_headers.insert(CONTENT_TYPE, HeaderValue::from_static("application/pdf"));
    upload_headers.insert("Content-Filename", HeaderValue::from_static(INPUT_FILE_NAME));
    let upload_res = client.post("https://api.pdfrest.com/upload")
        .headers(upload_headers)
        .body(upload_contents)
        .send().await?;
    let upload_res_obj = upload_res.json::<UploadResponse>().await?;
    if upload_res_obj.error.is_some() {
        let upload_error = upload_res_obj.error.unwrap();
        panic!("ERROR: {}", upload_error);
    }

    // Get ID of uploaded file
    let uploaded_files = upload_res_obj.files.unwrap();
    let uploaded_file = uploaded_files.get(0).unwrap();

    // Use OCR to add text to PDF
    let mut ocr_headers = HeaderMap::new();
    ocr_headers.insert(ACCEPT, HeaderValue::from_static("application/json"));
    ocr_headers.insert(CONTENT_TYPE, HeaderValue::from_static("application/json"));
    ocr_headers.insert("Api-Key", HeaderValue::from_static(API_KEY));

    let ocr_res = client.post("https://api.pdfrest.com/pdf-with-ocr-text")
        .headers(ocr_headers)
        .json(&json!({
            "id": uploaded_file.id,
        }))
        .send()
        .await?;
    let ocr_res_obj = ocr_res.json::<PdfRestResponse>().await?;
    if ocr_res_obj.error.is_some() {
        let pdfrest_error = ocr_res_obj.error.unwrap();
        panic!("ERROR: {}", pdfrest_error);
    }

    // Get ID of PDF with OCR text
    let file_with_ocr_id = ocr_res_obj.outputId.unwrap();

    // Extract text from PDF
    let mut extract_text_headers = HeaderMap::new();
    extract_text_headers.insert(ACCEPT, HeaderValue::from_static("application/json"));
    extract_text_headers.insert(CONTENT_TYPE, HeaderValue::from_static("application/json"));
    extract_text_headers.insert("Api-Key", HeaderValue::from_static(API_KEY));

    let text_extract_res = client.post("https://api.pdfrest.com/extracted-text")
        .headers(extract_text_headers)
        .json(&json!({
            "id": file_with_ocr_id
        }))
        .send()
        .await?;
    let text_extract_res_obj = text_extract_res.json::<ExtractTextResponse>().await?;
    if text_extract_res_obj.error.is_some() {
        let pdfrest_error = ocr_res_obj.error.unwrap();
        panic!("ERROR: {}", pdfrest_error);
    }
    // Get and print text from the response
    let text_extract_res_str = serde_json::to_string_pretty(&text_extract_res_obj)?;
    println!("{}", text_extract_res_str);
    Ok(())
}

#[derive(Deserialize)]
struct PdfRestResponse {
    inputId: Option<String>,
    outputId: Option<String>,
    error: Option<String>,
}

#[derive(Deserialize)]
struct UploadResponse {
    files: Option<Vec<UploadedFile>>,
    error: Option<String>,
}

#[derive(Deserialize)]
struct UploadedFile {
    name: String,
    id: String,
}

#[derive(Serialize, Deserialize)]
struct ExtractTextResponse {
    inputId: Option<String>,
    fullText: Option<String>,
    error: Option<String>,
}
