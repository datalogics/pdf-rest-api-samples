**PDFRest API Example**

This is an example PHP script demonstrating how to interact with the PDFRest API using the Guzzle HTTP client. The script makes various API requests to perform operations on PDF files, such as converting, watermarking, compressing, merging, and more.

### Prerequisites

- PHP 8.2 or higher installed on your system. If PHP is not installed, download it from the official PHP website: [https://www.php.net/downloads.php](https://www.php.net/downloads.php)

- Composer installed on your system. If you don't have Composer installed, download it from the official Composer website: [https://getcomposer.org/](https://getcomposer.org/)

### Installation

1. Clone this repository or download the `pdfrest_api_example.php` file.

2. Navigate to the directory containing the `pdfrest_api_example.php` file.

3. Run the following command to install the required dependencies (Guzzle HTTP client):

```bash
composer install
```

### Usage

1. Set your PDFRest API key in the `$headers` array. Replace `'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'` with your actual API key.

2. Provide the path to the PDF file you want to process in the `$options` array. Replace `'/path/to/file'` with the actual file path.

3. Execute the PHP script from the command line:

```bash
php pdfrest_api_example.php
```

