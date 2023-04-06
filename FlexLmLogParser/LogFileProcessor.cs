using System.Collections.Generic;

namespace FlexLmLogParser
{
    public class LogFileProcessor
    {
        private Options _options;
        private List<LicenseUsage> _licenseUsages;

        public LogFileProcessor(Options options)
        {
            _options = options;
        }

        public void ProcessLogFile()
        {
            _licenseUsages = LogFileParser.ParseLogFile(_options.LogFilePath, _options.StartDate, _options.EndDate);
            _licenseUsages = LogFileParser.FilterByUser(_licenseUsages, _options.SpecificUser);
        }

        public void DisplayResults()
        {
            if (!string.IsNullOrEmpty(_options.OutputFile))
            {
                CsvExporter.ExportToCsv(_licenseUsages, _options.OutputFile);
            }

            ResultFormatter.DisplayResults(_licenseUsages);
        }
    }
}