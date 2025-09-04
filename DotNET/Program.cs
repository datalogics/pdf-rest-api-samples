// Minimal .NET 8 dispatcher for DotNET samples
// - Loads .env for PDFREST_API_KEY and optional PDFREST_URL
// - Dispatches to endpoint samples located under folder structure
//   * markdown-json: two-step upload + JSON call to markdown endpoint

using DotNetEnv;

static void PrintUsage()
{
    Console.Error.WriteLine("Usage: dotnet run -- <command> [args]\n");
    Console.Error.WriteLine("Commands:");
    Console.Error.WriteLine("  markdown-json <inputFile>        Upload then convert to Markdown (JSON two-step)");
    Console.Error.WriteLine("  rasterized-pdf <inputFile>       Upload then rasterize PDF (JSON two-step)");
    Console.Error.WriteLine("  pdf-multipart <inputFile>        Convert to PDF via multipart");
    Console.Error.WriteLine("  png-multipart <file>             Convert to PNG via multipart");
    Console.Error.WriteLine("  jpg-multipart <file>             Convert to JPG via multipart");
    Console.Error.WriteLine("  gif-multipart <file>             Convert to GIF via multipart");
    Console.Error.WriteLine("  bmp-multipart <file>             Convert to BMP via multipart");
    Console.Error.WriteLine("  tif-multipart <file>             Convert to TIFF via multipart");
    Console.Error.WriteLine("  merge-different-file-types <imageFile> <pptFile>  Convert then merge (complex flow)");
    Console.Error.WriteLine("  decrypt-add-reencrypt <pdf> <image> [pass]  Decrypt, add image, re-encrypt");
    Console.Error.WriteLine("  ocr-with-extract-text <pdf>      OCR then extract text (complex flow)");
    Console.Error.WriteLine("  pdfa-3b-with-attachment <pdf> <xml>  Attach XML then PDF/A-3b");
    Console.Error.WriteLine("  preserve-word-document <file>    Office -> PDF -> PDF/A-3b");
    Console.Error.WriteLine("  protected-watermark <pdf>        Watermark then restrict permissions");
    Console.Error.WriteLine("  redact-preview-and-finalize <pdf> Preview then apply redactions");
    Console.Error.WriteLine("  extracted-text <inputFile>       Upload then extract text (JSON two-step)");
    Console.Error.WriteLine("  extracted-images <inputFile>     Upload then extract images (JSON two-step)");
    Console.Error.WriteLine("  pdf-info <inputFile>             Upload then query document info (JSON two-step)");
    Console.Error.WriteLine("  merged-pdf <file1> <file2>       Upload two files then merge by id");
    Console.Error.WriteLine("  split-pdf <inputFile>            Upload then split into ranges");
    Console.Error.WriteLine("  upload <inputFile>               Upload a file (resource id)");
    Console.Error.WriteLine("  upload-multipart <inputFile>     Upload via multipart (resource id)");
    Console.Error.WriteLine("  get-resource <id> [out]          Download resource file by id");
    Console.Error.WriteLine("  get-resource-multipart <id> [out] Download via multipart route");
    Console.Error.WriteLine("  delete-resource <id>             Delete a resource by id");
    Console.Error.WriteLine("  delete-resource-multipart <id>   Delete via multipart route");
    Console.Error.WriteLine("  batch-delete <id1> [id2] [...]   Delete multiple resources by ids");
    Console.Error.WriteLine("  batch-delete-multipart <id1> [id2] [...] Delete multiple via multipart route");
    Console.Error.WriteLine("  png|jpg|gif|bmp <inputFile>      Upload then convert to image format (JSON two-step)");
    Console.Error.WriteLine("  pdf|pdfa|pdfx <inputFile>        Upload then process to target PDF standard");
    Console.Error.WriteLine("  word|excel|powerpoint|tif <file> Upload then convert to Office/TIFF");
    Console.Error.WriteLine("  excel-multipart <file>           Convert to Excel via multipart");
    Console.Error.WriteLine("  powerpoint-multipart <file>      Convert to PowerPoint via multipart");
    Console.Error.WriteLine("  word-multipart <file>            Convert to Word via multipart");
    Console.Error.WriteLine("  compressed-pdf <inputFile>       Upload then compress PDF");
    Console.Error.WriteLine("  compressed-pdf-multipart <file>  Compress PDF via multipart");
    Console.Error.WriteLine("  linearized-pdf <inputFile>       Upload then linearize PDF");
    Console.Error.WriteLine("  linearized-pdf-multipart <file>  Linearize PDF via multipart");
    Console.Error.WriteLine("  encrypted-pdf <inputFile>        Upload then set open password");
    Console.Error.WriteLine("  encrypted-pdf-multipart <pdf> <password>  Encrypt via multipart");
    Console.Error.WriteLine("  decrypted-pdf <inputFile>        Upload then remove open password");
    Console.Error.WriteLine("  decrypted-pdf-multipart <pdf> <password>  Decrypt via multipart");
    Console.Error.WriteLine("  restricted-pdf <inputFile>       Upload then set permissions restrictions");
    Console.Error.WriteLine("  restricted-pdf-multipart <pdf> <permPass> Set restrictions via multipart");
    Console.Error.WriteLine("  unrestricted-pdf <inputFile>     Upload then remove permissions restrictions");
    Console.Error.WriteLine("  unrestricted-pdf-multipart <pdf> <permPass> Remove restrictions via multipart");
    Console.Error.WriteLine("  flattened-forms-pdf <inputFile>  Upload then flatten forms");
    Console.Error.WriteLine("  flattened-forms-pdf-multipart <file> Flatten forms via multipart");
    Console.Error.WriteLine("  flattened-annotations-pdf <file> Upload then flatten annotations");
    Console.Error.WriteLine("  flattened-annotations-pdf-multipart <file> Flatten annotations via multipart");
    Console.Error.WriteLine("  flattened-layers-pdf <file>      Upload then flatten layers");
    Console.Error.WriteLine("  flattened-layers-pdf-multipart <file> Flatten layers via multipart");
    Console.Error.WriteLine("  flattened-transparencies-pdf <file> Upload then flatten transparencies");
    Console.Error.WriteLine("  flattened-transparencies-pdf-multipart <file> Flatten transparencies via multipart");
    Console.Error.WriteLine("  pdf-with-ocr-text <file>         Upload then add OCR text layer");
    Console.Error.WriteLine("  pdf-with-ocr-text-multipart <file> Add OCR text via multipart");
    Console.Error.WriteLine("  pdf-with-imported-form-data <pdf> <xml/fdf>  Import form data");
    Console.Error.WriteLine("  pdf-with-imported-form-data-multipart <pdf> <data> Import form data via multipart");
    Console.Error.WriteLine("  pdf-with-added-image <pdf> <image> Add image to PDF");
    Console.Error.WriteLine("  pdf-with-added-image-multipart <pdf> <image> Add image via multipart");
    Console.Error.WriteLine("  pdf-with-added-attachment <pdf> <file> Attach file to PDF");
    Console.Error.WriteLine("  pdf-with-added-attachment-multipart <pdf> <file> Attach via multipart");
    Console.Error.WriteLine("  pdf-with-converted-colors <file> Convert colors to a profile");
    Console.Error.WriteLine("  pdf-with-converted-colors-multipart <file> Convert colors via multipart");
    Console.Error.WriteLine("  pdf-with-added-text <file>       Add text to PDF");
    Console.Error.WriteLine("  pdf-with-added-text-multipart <file> Add text via multipart");
    Console.Error.WriteLine("  pdf-with-acroforms <file>        Add acroforms to PDF");
    Console.Error.WriteLine("  pdf-with-acroforms-multipart <file> Add acroforms via multipart");
    Console.Error.WriteLine("  pdf-with-page-boxes-set <file>   Set page boxes");
    Console.Error.WriteLine("  pdf-with-page-boxes-set-multipart <file> Set page boxes via multipart");
    Console.Error.WriteLine("  pdf-with-redacted-text-preview <file>  Preview redactions");
    Console.Error.WriteLine("  pdf-with-redacted-text-preview-multipart <file> Preview redactions via multipart");
    Console.Error.WriteLine("  pdf-with-redacted-text-applied <file>  Apply redactions");
    Console.Error.WriteLine("  pdf-with-redacted-text-applied-multipart <file> Apply redactions via multipart");
    Console.Error.WriteLine("  watermarked-pdf <file>           Add text watermark");
    Console.Error.WriteLine("  watermarked-pdf-multipart <file> Add text watermark via multipart");
    Console.Error.WriteLine("  pdf-info-multipart <file>        Query PDF info via multipart");
    Console.Error.WriteLine("  extracted-text-multipart <file>  Extract text via multipart");
    Console.Error.WriteLine("  extracted-images-multipart <file> Extract images via multipart");
    Console.Error.WriteLine("  markdown-multipart <file>        Convert to Markdown via multipart");
    Console.Error.WriteLine("  pdfa-multipart <file>            Convert to PDF/A via multipart");
    Console.Error.WriteLine("  pdfx-multipart <file>            Convert to PDF/X via multipart");
    Console.Error.WriteLine("  rasterized-pdf-multipart <file>  Rasterize via multipart");
    Console.Error.WriteLine("  split-pdf-multipart <file>       Split via multipart");
    Console.Error.WriteLine("  exported-form-data-multipart <file> Export form data via multipart");
    Console.Error.WriteLine("  request-status-multipart <file>  Poll request status via multipart");
    Console.Error.WriteLine("  up-toolkit-multipart             Query toolkit status (multipart section)");
    Console.Error.WriteLine("  signed-pdf-multipart <pdf> <pfx> <pass> <logo>  Sign via PFX (multipart)");
    Console.Error.WriteLine("  signed-pdf-non-pfx-multipart <pdf> <cert> <key> Sign via PEM (multipart)");
    Console.Error.WriteLine("  zip <file1> <file2>              Zip two resources");
    Console.Error.WriteLine("  zip-multipart <f1> <f2>          Zip two files via multipart");
    Console.Error.WriteLine("  unzip <zipFile>                  Unzip resource");
    Console.Error.WriteLine("  unzip-multipart <zipFile>        Unzip via multipart");
    Console.Error.WriteLine("  up-toolkit                       Query toolkit status");
    Console.Error.WriteLine("  signed-pdf <pdf> <pfx> <pass> <logo>  Sign PDF with PFX");
    Console.Error.WriteLine("  signed-pdf-non-pfx <pdf> <cert> <key> Sign PDF with cert/key");
    Console.Error.WriteLine("");
    Console.Error.WriteLine("Environment (.env supported):");
    Console.Error.WriteLine("  PDFREST_API_KEY=...              Required API key");
    Console.Error.WriteLine("  PDFREST_URL=...                  Optional base URL (e.g., https://eu-api.pdfrest.com for EU/GDPR)");
}

// Entry point
Env.Load();
var argv = Environment.GetCommandLineArgs().Skip(1).ToArray();
if (argv.Length == 0)
{
    PrintUsage();
    return;
}

var cmd = argv[0].Trim().ToLowerInvariant();
var rest = argv.Skip(1).ToArray();

switch (cmd)
{
    case "markdown-json":
        await Samples.EndpointExamples.JsonPayload.Markdown.Execute(rest);
        break;
    case "rasterized-pdf":
        await Samples.EndpointExamples.JsonPayload.RasterizedPdf.Execute(rest);
        break;
    case "pdf-multipart":
        await Samples.EndpointExamples.MultipartPayload.Pdf.Execute(rest);
        break;
    case "markdown-multipart":
        await Samples.EndpointExamples.MultipartPayload.Markdown.Execute(rest);
        break;
    case "png-multipart":
        await Samples.EndpointExamples.MultipartPayload.Png.Execute(rest);
        break;
    case "jpg-multipart":
        await Samples.EndpointExamples.MultipartPayload.Jpg.Execute(rest);
        break;
    case "gif-multipart":
        await Samples.EndpointExamples.MultipartPayload.Gif.Execute(rest);
        break;
    case "bmp-multipart":
        await Samples.EndpointExamples.MultipartPayload.Bmp.Execute(rest);
        break;
    case "tif-multipart":
        await Samples.EndpointExamples.MultipartPayload.Tif.Execute(rest);
        break;
    case "excel-multipart":
        await Samples.EndpointExamples.MultipartPayload.Excel.Execute(rest);
        break;
    case "powerpoint-multipart":
        await Samples.EndpointExamples.MultipartPayload.Powerpoint.Execute(rest);
        break;
    case "word-multipart":
        await Samples.EndpointExamples.MultipartPayload.Word.Execute(rest);
        break;
    case "merge-different-file-types":
    case "merge":
        await Samples.ComplexFlowExamples.MergeDifferentFileTypes.Execute(rest);
        break;
    case "decrypt-add-reencrypt":
        await Samples.ComplexFlowExamples.DecryptAddReencrypt.Execute(rest);
        break;
    case "ocr-with-extract-text":
        await Samples.ComplexFlowExamples.OcrWithExtractText.Execute(rest);
        break;
    case "pdfa-3b-with-attachment":
        await Samples.ComplexFlowExamples.Pdfa3bWithAttachment.Execute(rest);
        break;
    case "preserve-word-document":
        await Samples.ComplexFlowExamples.PreserveWordDocument.Execute(rest);
        break;
    case "protected-watermark":
        await Samples.ComplexFlowExamples.ProtectedWatermark.Execute(rest);
        break;
    case "redact-preview-and-finalize":
        await Samples.ComplexFlowExamples.RedactPreviewAndFinalize.Execute(rest);
        break;
    case "extracted-text":
        await Samples.EndpointExamples.JsonPayload.ExtractedText.Execute(rest);
        break;
    case "extracted-text-multipart":
        await Samples.EndpointExamples.MultipartPayload.ExtractedText.Execute(rest);
        break;
    case "extracted-images":
        await Samples.EndpointExamples.JsonPayload.ExtractedImages.Execute(rest);
        break;
    case "extracted-images-multipart":
        await Samples.EndpointExamples.MultipartPayload.ExtractedImages.Execute(rest);
        break;
    case "pdf-info":
        await Samples.EndpointExamples.JsonPayload.PdfInfo.Execute(rest);
        break;
    case "pdf-info-multipart":
        await Samples.EndpointExamples.MultipartPayload.PdfInfo.Execute(rest);
        break;
    case "merged-pdf":
        await Samples.EndpointExamples.JsonPayload.MergedPdf.Execute(rest);
        break;
    case "merged-pdf-multipart":
        await Samples.EndpointExamples.MultipartPayload.MergedPdf.Execute(rest);
        break;
    case "split-pdf":
        await Samples.EndpointExamples.JsonPayload.SplitPdf.Execute(rest);
        break;
    case "split-pdf-multipart":
        await Samples.EndpointExamples.MultipartPayload.SplitPdf.Execute(rest);
        break;
    case "upload":
        await Samples.EndpointExamples.JsonPayload.Upload.Execute(rest);
        break;
    case "upload-multipart":
        await Samples.EndpointExamples.MultipartPayload.Upload.Execute(rest);
        break;
    case "get-resource":
        await Samples.EndpointExamples.JsonPayload.GetResource.Execute(rest);
        break;
    case "get-resource-multipart":
        await Samples.EndpointExamples.MultipartPayload.GetResource.Execute(rest);
        break;
    case "delete-resource":
        await Samples.EndpointExamples.JsonPayload.DeleteResource.Execute(rest);
        break;
    case "delete-resource-multipart":
        await Samples.EndpointExamples.MultipartPayload.DeleteResource.Execute(rest);
        break;
    case "batch-delete":
        await Samples.EndpointExamples.JsonPayload.BatchDelete.Execute(rest);
        break;
    case "batch-delete-multipart":
        await Samples.EndpointExamples.MultipartPayload.BatchDelete.Execute(rest);
        break;
    case "png":
        await Samples.EndpointExamples.JsonPayload.Png.Execute(rest);
        break;
    case "jpg":
        await Samples.EndpointExamples.JsonPayload.Jpg.Execute(rest);
        break;
    case "gif":
        await Samples.EndpointExamples.JsonPayload.Gif.Execute(rest);
        break;
    case "bmp":
        await Samples.EndpointExamples.JsonPayload.Bmp.Execute(rest);
        break;
    case "pdf":
        await Samples.EndpointExamples.JsonPayload.Pdf.Execute(rest);
        break;
    case "pdfa-multipart":
        await Samples.EndpointExamples.MultipartPayload.Pdfa.Execute(rest);
        break;
    case "pdfx-multipart":
        await Samples.EndpointExamples.MultipartPayload.Pdfx.Execute(rest);
        break;
    case "pdfa":
        await Samples.EndpointExamples.JsonPayload.Pdfa.Execute(rest);
        break;
    case "pdfx":
        await Samples.EndpointExamples.JsonPayload.Pdfx.Execute(rest);
        break;
    case "excel":
        await Samples.EndpointExamples.JsonPayload.Excel.Execute(rest);
        break;
    case "powerpoint":
        await Samples.EndpointExamples.JsonPayload.Powerpoint.Execute(rest);
        break;
    case "word":
        await Samples.EndpointExamples.JsonPayload.Word.Execute(rest);
        break;
    case "tif":
        await Samples.EndpointExamples.JsonPayload.Tif.Execute(rest);
        break;
    case "rasterized-pdf-multipart":
        await Samples.EndpointExamples.MultipartPayload.RasterizedPdf.Execute(rest);
        break;
    case "compressed-pdf":
        await Samples.EndpointExamples.JsonPayload.CompressedPdf.Execute(rest);
        break;
    case "compressed-pdf-multipart":
        await Samples.EndpointExamples.MultipartPayload.CompressedPdf.Execute(rest);
        break;
    case "linearized-pdf":
        await Samples.EndpointExamples.JsonPayload.LinearizedPdf.Execute(rest);
        break;
    case "linearized-pdf-multipart":
        await Samples.EndpointExamples.MultipartPayload.LinearizedPdf.Execute(rest);
        break;
    case "encrypted-pdf":
        await Samples.EndpointExamples.JsonPayload.EncryptedPdf.Execute(rest);
        break;
    case "encrypted-pdf-multipart":
        await Samples.EndpointExamples.MultipartPayload.EncryptedPdf.Execute(rest);
        break;
    case "decrypted-pdf":
        await Samples.EndpointExamples.JsonPayload.DecryptedPdf.Execute(rest);
        break;
    case "decrypted-pdf-multipart":
        await Samples.EndpointExamples.MultipartPayload.DecryptedPdf.Execute(rest);
        break;
    case "restricted-pdf":
        await Samples.EndpointExamples.JsonPayload.RestrictedPdf.Execute(rest);
        break;
    case "restricted-pdf-multipart":
        await Samples.EndpointExamples.MultipartPayload.RestrictedPdf.Execute(rest);
        break;
    case "unrestricted-pdf":
        await Samples.EndpointExamples.JsonPayload.UnrestrictedPdf.Execute(rest);
        break;
    case "unrestricted-pdf-multipart":
        await Samples.EndpointExamples.MultipartPayload.UnrestrictedPdf.Execute(rest);
        break;
    case "flattened-forms-pdf":
        await Samples.EndpointExamples.JsonPayload.FlattenedFormsPdf.Execute(rest);
        break;
    case "flattened-forms-pdf-multipart":
        await Samples.EndpointExamples.MultipartPayload.FlattenedFormsPdf.Execute(rest);
        break;
    case "flattened-annotations-pdf":
        await Samples.EndpointExamples.JsonPayload.FlattenedAnnotationsPdf.Execute(rest);
        break;
    case "flattened-annotations-pdf-multipart":
        await Samples.EndpointExamples.MultipartPayload.FlattenedAnnotationsPdf.Execute(rest);
        break;
    case "flattened-layers-pdf":
        await Samples.EndpointExamples.JsonPayload.FlattenedLayersPdf.Execute(rest);
        break;
    case "flattened-layers-pdf-multipart":
        await Samples.EndpointExamples.MultipartPayload.FlattenedLayersPdf.Execute(rest);
        break;
    case "flattened-transparencies-pdf":
        await Samples.EndpointExamples.JsonPayload.FlattenedTransparenciesPdf.Execute(rest);
        break;
    case "flattened-transparencies-pdf-multipart":
        await Samples.EndpointExamples.MultipartPayload.FlattenedTransparenciesPdf.Execute(rest);
        break;
    case "pdf-with-ocr-text":
        await Samples.EndpointExamples.JsonPayload.PdfWithOcrText.Execute(rest);
        break;
    case "pdf-with-ocr-text-multipart":
        await Samples.EndpointExamples.MultipartPayload.PdfWithOcrText.Execute(rest);
        break;
    case "pdf-with-imported-form-data":
        await Samples.EndpointExamples.JsonPayload.PdfWithImportedFormData.Execute(rest);
        break;
    case "pdf-with-imported-form-data-multipart":
        await Samples.EndpointExamples.MultipartPayload.PdfWithImportedFormData.Execute(rest);
        break;
    case "pdf-with-added-image":
        await Samples.EndpointExamples.JsonPayload.PdfWithAddedImage.Execute(rest);
        break;
    case "pdf-with-added-image-multipart":
        await Samples.EndpointExamples.MultipartPayload.PdfWithAddedImage.Execute(rest);
        break;
    case "pdf-with-added-attachment":
        await Samples.EndpointExamples.JsonPayload.PdfWithAddedAttachment.Execute(rest);
        break;
    case "pdf-with-added-attachment-multipart":
        await Samples.EndpointExamples.MultipartPayload.PdfWithAddedAttachment.Execute(rest);
        break;
    case "pdf-with-converted-colors":
        await Samples.EndpointExamples.JsonPayload.PdfWithConvertedColors.Execute(rest);
        break;
    case "pdf-with-converted-colors-multipart":
        await Samples.EndpointExamples.MultipartPayload.PdfWithConvertedColors.Execute(rest);
        break;
    case "pdf-with-added-text":
        await Samples.EndpointExamples.JsonPayload.PdfWithAddedText.Execute(rest);
        break;
    case "pdf-with-added-text-multipart":
        await Samples.EndpointExamples.MultipartPayload.PdfWithAddedText.Execute(rest);
        break;
    case "pdf-with-acroforms":
        await Samples.EndpointExamples.JsonPayload.PdfWithAcroforms.Execute(rest);
        break;
    case "pdf-with-acroforms-multipart":
        await Samples.EndpointExamples.MultipartPayload.PdfWithAcroforms.Execute(rest);
        break;
    case "pdf-with-page-boxes-set":
        await Samples.EndpointExamples.JsonPayload.PdfWithPageBoxesSet.Execute(rest);
        break;
    case "pdf-with-page-boxes-set-multipart":
        await Samples.EndpointExamples.MultipartPayload.PdfWithPageBoxesSet.Execute(rest);
        break;
    case "pdf-with-redacted-text-preview":
        await Samples.EndpointExamples.JsonPayload.PdfWithRedactedTextPreview.Execute(rest);
        break;
    case "pdf-with-redacted-text-preview-multipart":
        await Samples.EndpointExamples.MultipartPayload.PdfWithRedactedTextPreview.Execute(rest);
        break;
    case "pdf-with-redacted-text-applied":
        await Samples.EndpointExamples.JsonPayload.PdfWithRedactedTextApplied.Execute(rest);
        break;
    case "pdf-with-redacted-text-applied-multipart":
        await Samples.EndpointExamples.MultipartPayload.PdfWithRedactedTextApplied.Execute(rest);
        break;
    case "watermarked-pdf":
        await Samples.EndpointExamples.JsonPayload.WatermarkedPdf.Execute(rest);
        break;
    case "watermarked-pdf-multipart":
        await Samples.EndpointExamples.MultipartPayload.WatermarkedPdf.Execute(rest);
        break;
    case "zip":
        await Samples.EndpointExamples.JsonPayload.Zip.Execute(rest);
        break;
    case "zip-multipart":
        await Samples.EndpointExamples.MultipartPayload.Zip.Execute(rest);
        break;
    case "unzip":
        await Samples.EndpointExamples.JsonPayload.Unzip.Execute(rest);
        break;
    case "unzip-multipart":
        await Samples.EndpointExamples.MultipartPayload.Unzip.Execute(rest);
        break;
    case "up-toolkit":
        await Samples.EndpointExamples.JsonPayload.UpToolkit.Execute(rest);
        break;
    case "up-toolkit-multipart":
        await Samples.EndpointExamples.MultipartPayload.UpToolkit.Execute(rest);
        break;
    case "signed-pdf":
        await Samples.EndpointExamples.JsonPayload.SignedPdf.Execute(rest);
        break;
    case "signed-pdf-multipart":
        await Samples.EndpointExamples.MultipartPayload.SignedPdf.Execute(rest);
        break;
    case "signed-pdf-non-pfx":
        await Samples.EndpointExamples.JsonPayload.SignedPdfNonPfx.Execute(rest);
        break;
    case "signed-pdf-non-pfx-multipart":
        await Samples.EndpointExamples.MultipartPayload.SignedPdfNonPfx.Execute(rest);
        break;
    case "exported-form-data-multipart":
        await Samples.EndpointExamples.MultipartPayload.ExportedFormData.Execute(rest);
        break;
    case "request-status-multipart":
        await Samples.EndpointExamples.MultipartPayload.RequestStatus.Execute(rest);
        break;

    default:
        Console.Error.WriteLine($"Unknown command: {cmd}\n");
        PrintUsage();
        Environment.Exit(1);
        break;
}
