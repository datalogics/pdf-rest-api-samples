**pdfRest API Example**

In this directory you will find sample calls to single endpoints, as well as more advanced workflows presented in PHP

### Prerequisites

- PHP 8.2 or higher installed on your system. If PHP is not installed, download it from the official PHP website: [https://www.php.net/downloads.php](https://www.php.net/downloads.php)

- Composer installed on your system. If you don't have Composer installed, download it from the official Composer website: [https://getcomposer.org/](https://getcomposer.org/)

### Installation

1. Clone this repository or download your file of choice.

2. Navigate to the directory containing the `php` file.

3. Install the required dependencies (Guzzle HTTP client) by following the instructions at https://docs.guzzlephp.org/en/stable/overview.html

### Usage

1. Set your pdfRest API key in the `$headers` array. Replace `'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx'` with your actual API key.

2. Provide the path to the PDF file you want to process in the `$options` array. Replace `'/path/to/file'` with the actual file path.

3. Execute the PHP script from the command line:

```bash
php pdfrest_api_example.php
```
