using CommandLine;
using System;
using System.Collections.Generic;

namespace FlexLmLogParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(Run)
                .WithNotParsed(HandleParseErrors);
        }

        private static void Run(Options options)
        {
            var logFileProcessor = new LogFileProcessor(options);
            logFileProcessor.ProcessLogFile();
            logFileProcessor.DisplayResults();
        }

        private static void HandleParseErrors(IEnumerable<Error> errors)
        {
            Console.WriteLine("Error parsing command-line arguments.");
            Console.WriteLine("Usage: FlexLMLogParser.exe -f <log file path> -s <start date> -e <end date> -u <user> -o <output file>");
            foreach (var error in errors)
            {
                Console.WriteLine($"- {error.Tag}: {error.ToString()}");
            }
        }
    }
}