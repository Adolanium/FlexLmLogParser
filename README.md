# FlexLmLogParser

This is a C# program for parsing FlexLM log files to gather license usage statistics. It is designed to provide detailed usage information for all dates and all users.



## Features

- Parses FlexLM log files to gather license usage information
- Groups license usage data by date and user
- Provides detailed usage statistics, including total license usage
- Outputs usage statistics to the console


## Usage/Examples

1. Clone the repository
2. Open the solution in Visual Studio
3. Build the solution to create the executable
4. Run the executable with the following argument:

```
FlexLMLogParser.exe -f <log file path> -s <start date> -e <end date> -u <user>
```

The "log file path" parameter is the path to the FlexLM license server log file that you want to parse. The "start date" and "end date" parameters specify the date range for the report. The dates should be specified in the format MM/dd/yyyy. The user should be specific in the format user@macine-name.


Example Usage:
```
FlexLMLogParser.exe -f "C:\FlexLM\lmgrd.log" -s 01/01/2023 -e 03/30/2023
```

## Contributing

Contributions are welcome! If you find a bug or have an idea for a new feature, please open an issue or submit a pull request.
