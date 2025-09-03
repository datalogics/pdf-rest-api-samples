#! /usr/bin/env Rscript

# Install required and recommended packages if missing.
cran <- "https://cloud.r-project.org"
pkgs <- c(
  # Required for samples
  "httr", "jsonlite",
  # Recommended tooling
  "lintr", "styler", "testthat"
)

install_if_missing <- function(p) {
  if (!requireNamespace(p, quietly = TRUE)) {
    install.packages(p, repos = cran)
  }
}

invisible(lapply(pkgs, install_if_missing))
cat("Dependencies ensured.\n")
