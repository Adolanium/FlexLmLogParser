using System;
using CommandLine;

namespace FlexLmLogParser
{
    public class Options
    {
        [Option('f', "file", Required = true, HelpText = "Path to the log file.")]
        public string LogFilePath { get; set; }

        [Option('s', "start", Required = false, HelpText = "Start date.")]
        public DateTime? StartDate { get; set; }

        [Option('e', "end", Required = false, HelpText = "End date.")]
        public DateTime? EndDate { get; set; }

        [Option('u', "user", Required = false, HelpText = "Specific user.")]
        public string SpecificUser { get; set; }

        [Option('o', "output", Required = false, HelpText = "Output file path.")]
        public string OutputFile { get; set; }
    }
}