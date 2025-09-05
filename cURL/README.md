In this directory you will find sample calls to single endpoints, as well
as more advanced workflows presented as cURL scripts.

Optional deletion of sensitive files: many samples include an optional step 
to delete uploaded or generated files from pdfRest servers. This step is off 
by default. To enable it, open the script and uncomment the local toggle line:

`# PDFREST_DELETE_SENSITIVE_FILES=true` → `PDFREST_DELETE_SENSITIVE_FILES=true`

Leave it commented (or set to anything other than `true`) to keep deletion off.
