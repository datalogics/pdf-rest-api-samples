# Java

The Java samples are all set up to:

- Take input paths from either the program source or the command line.
- Take credential information from a `.env` file (recommended) or the
  environment, but the key can also be set in the source.

## Requirements

- A Java SE 17 (LTS) compatible installation of the JDK
- Maven 3.9.4

## Preparation

Place your pdfRest API Key in a variable named `PDFREST_API_KEY` in a file
named `.env`:

```shell
PDFREST_API_KEY=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
```

## Build/run from the command line

To build the Java samples, run Maven:

```shell
mvn package
```

To run a sample, place the jar-with-dependencies on the classpath, specify the
class name of the sample, and then follow that with the input files.

```shell
java -cp target/pdf-rest-api-samples-1.0-SNAPSHOT-jar-with-dependencies.jar CompressedPDF input.pdf
```

## Build/run from IntelliJ IDEA

- Open the project directory in IntelliJ IDEA. IDEA should recognize the project
  to be a Maven project. If it asks any questions about importing the project as
  Maven, answer affirmatively.
- Open the Maven tool window (View->Tool Windows->Maven) and click the **Reload all Maven Projects** button (it looks like a pair of chasing arrows). This will make sure all the dependencies are downloaded.
- Open the source file for the sample you want to run.
- Click the green, triangular Run button next to the name of the main class.
- Pick **Modify Run Configuration...**
- Add your desired input file(s) to the **Program arguments** field.
- Click **OK**
- Click the run button next to the run configuration in the toolbar.

See: [IntelliJ IDEA Maven documentation](https://www.jetbrains.com/help/idea/maven-support.html)

## Do linting and formatting on files

This project uses [Spotless](https://github.com/diffplug/spotless) to apply
canonical formatting to the Java source code and `pom.xml`.

To check the files, either do

```shell
mvn verify
```

or

```shell
mvn spotless:check
```

To correct problems and format the files, do

```shell
mvn spotless:apply
```