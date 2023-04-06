using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FlexLmLogParser
{
    public static class LogFileParser
    {
        public static List<LicenseUsage> ParseLogFile(string logFilePath, DateTime? startDate, DateTime? endDate)
        {
            List<LicenseUsage> licenseUsages = new List<LicenseUsage>();
            DateTime currentDate = DateTime.MinValue;

            using (StreamReader reader = new StreamReader(logFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string timestampPattern = @"^(\d{2}:\d{2}:\d{2}) \(\w+\) TIMESTAMP (\d{1,2}/\d{1,2}/\d{4})";
                    Match timestampMatch = Regex.Match(line, timestampPattern);

                    if (timestampMatch.Success)
                    {
                        currentDate = DateTime.Parse(timestampMatch.Groups[2].Value);
                        if (currentDate < startDate || currentDate > endDate)
                        {
                            break;
                        }

                        continue;
                    }

                    string usagePattern = @"^(\d{2}:\d{2}:\d{2}) \(\w+\) (OUT|IN): ""(\w+)"" (\w+@\w+-\w+)";
                    Match usageMatch = Regex.Match(line, usagePattern);

                    if (usageMatch.Success && currentDate != DateTime.MinValue)
                    {
                        LicenseUsage licenseUsage = new LicenseUsage();
                        licenseUsage.Time = usageMatch.Groups[1].Value;
                        licenseUsage.License = usageMatch.Groups[3].Value;
                        licenseUsage.User = usageMatch.Groups[4].Value;
                        licenseUsage.Date = currentDate;
                        licenseUsages.Add(licenseUsage);
                    }
                }
            }

            return licenseUsages;
        }

        public static List<LicenseUsage> FilterByUser(List<LicenseUsage> licenseUsages, string specificUser)
        {
            if (!string.IsNullOrEmpty(specificUser))
            {
                return licenseUsages.Where(usage => usage.User == specificUser).ToList();
            }

            return licenseUsages;
        }
    }
}