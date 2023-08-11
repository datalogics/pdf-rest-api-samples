<img src='https://pdfrest.com/_nuxt/image/6fa0c8.webp' alt='Integration with ChatGPt'>

## Introduction

As technology enthusiasts, we've closely monitored AI's rapid advancements, particularly in Large Language Models (LLMs) like OpenAI's ChatGPT. However, a critical question arises: How do we feed input to these AI systems?

pdfRest is at the forefront of exploring this challenge. We're combining the prowess of pdfRest, a document processing toolkit, with the capabilities of ChatGPT, an LLM AI service. This integration aims to unearth valuable insights from digital documents by generating user-friendly summaries of complex PDF metadata and conditions.

## Evaluating ChatGPT's Abilities

Before diving into development, we experimented with ChatGPT's JSON comprehension. We fed JSON output from our Query PDF tool's /pdf-info endpoint to ChatGPT, instructing it:

"Summarize the JSON data as if it describes a PDF Document."

ChatGPT adeptly responded with an insightful summary of the PDF's details.

Refer to the original post on our website for the PDF summary:
[Integrate pdfRest with ChatGPT](https://pdfrest.com/solutions/page/integrate-pdfrest-with-chatgpt-to-generate-pdf-info-summary/)

## Implementing the Proof of Concept

Our focus shifted to implementing a programmatic demonstration using pdfRest's Cloud API and OpenAI's API, specifically with Python. We obtained API keys for both services and utilized pdfRest's /pdf-info endpoint code as a foundation. After some adaptation, we had a functional code snippet.

See `generate-pdf-info-summary.py` for the full code.

## Towards Innovative Integration

This integration venture bridges the gap between AI and document processing. For deeper insights and specifics, refer to the original post on our website: [Integrate pdfRest with ChatGPT](https://pdfrest.com/solutions/page/integrate-pdfrest-with-chatgpt-to-generate-pdf-info-summary/).

pdfRest's trajectory involves advanced AI-based document workflows. Expect upcoming solutions that streamline text extraction from PDFs, facilitating direct input for ChatGPT and other AI services enabling concise summarization, interpretation, and comprehensive analysis.

## Collaboration and Insights

We're eager to explore novel dimensions in digital document-AI integration. Join us in unlocking AI's true potential within document processing.

Do you have other ideas or requirements for integrating digitial document workflows with AI technologies? [Let us know](https://pdfrest.com/support/), and we'll be happy to help you on your journey!
