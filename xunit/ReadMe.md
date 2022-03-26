# Ladybug XUnit Test Suite

### Generating Coverage Reports

See [this article](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-code-coverage) for more info on the test frameworks used.

#### Step 1: Generate Coverage Data
```bash
dotnet test --collect:"Xplat Code Coverage"
```
This will create an XML file in `TestResults/{guid}/coverage.cobertura.xml`, which we will use to generate a report in the next step.

#### Step 2: Generate Coverage Report
```bash
reportgenerator -reports:"./TestResults/{guid}/coverage.cobertura.xml" -targetdir:"TestReports" -reporttypes:Html
```
This will create an HTML output report at `TestReports/index.html`