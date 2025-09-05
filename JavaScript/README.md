In this directory you will find sample calls to single endpoints, as well
as more advanced workflows presented in Javascript

#### How to run JavaScript samples

We recommend using Node.js >= 20 with our code samples.

1. `cd JavaScript/`
2. `npm install node-fetch`
3. `node name-of-sample-program.js`

Optional deletion toggle
- Some samples include an optional delete step to remove uploaded/generated files from pdfRest servers. This is controlled by a local `const DELETE_SENSITIVE_FILES = false;` near the top of the file. Set it to `true` to enable deletion; it is off by default.
