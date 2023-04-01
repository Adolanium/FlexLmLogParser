﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FlexLmLogParser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: FlexLMLogParser.exe -f <log file path> -s <start date> -e <end date>");
                return;
            }

            string logFilePath = null;
            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;
            string specificUser = null;
            string outputFile = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-f")
                {
                    logFilePath = args[i + 1];
                }
                else if (args[i] == "-s")
                {
                    startDate = DateTime.Parse(args[i + 1]);
                }
                else if (args[i] == "-e")
                {
                    endDate = DateTime.Parse(args[i + 1]);
                }
                else if (args[i] == "-u")
                {
                    specificUser = args[i + 1];
                }
                else if (args[i] == "-o")
                {
                    outputFile = args[i + 1];
                }
            }

            if (logFilePath == null)
            {
                Console.WriteLine("Usage: FlexLMLogParser.exe -f <log file path> -s <start date> -e <end date> -u <user>");
                return;
            }

            List<LicenseUsage> licenseUsages = ParseLogFile(logFilePath, startDate, endDate);
            RemoveEmptyLines(licenseUsages);
            if (!string.IsNullOrEmpty(specificUser))
            {
                licenseUsages = licenseUsages.Where(usage => usage.User == specificUser).ToList();
            }

            if (!string.IsNullOrEmpty(outputFile))
            {
                ExportToCsv(licenseUsages, outputFile);
            }

            var dateGroup = licenseUsages.GroupBy(usage => usage.Date);
            foreach (var date in dateGroup)
            {
                Console.WriteLine("Date: {0}", date.Key);
                Console.WriteLine("Total licenses used: {0}", date.Count());
                Console.WriteLine("Total licenses used by user: {0}", date.GroupBy(usage => usage.User).Count());

                Console.WriteLine("Users: ");
                foreach (var user in date.GroupBy(usage => usage.User))
                {
                    Console.WriteLine("User: {0}", user.Key);
                    foreach (var license in user.GroupBy(usage => usage.License))
                    {
                        Console.WriteLine("License: {0}", license.Key);
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Total licenses used by license: {0}", date.GroupBy(usage => usage.License).Count());
                Console.WriteLine();
            }
        }

        private static List<LicenseUsage> ParseLogFile(string logFilePath, DateTime startDate, DateTime endDate)
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

        private static void RemoveEmptyLines(List<LicenseUsage> licenseUsages)
        {
            licenseUsages.RemoveAll(usage => usage.User == null);
            licenseUsages.RemoveAll(usage => usage.License == null);
            licenseUsages.RemoveAll(usage => usage.Time == null);
            licenseUsages.RemoveAll(usage => usage.Date == DateTime.MinValue);
            licenseUsages.RemoveAll(usage => usage.User == string.Empty);
            licenseUsages.RemoveAll(usage => usage.License == string.Empty);
            licenseUsages.RemoveAll(usage => usage.Time == string.Empty);
        }

        private static void ExportToCsv(IEnumerable<LicenseUsage> licenseUsages, string outputFilePath)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                writer.WriteLine("Date,Time,License,User");

                foreach (var usage in licenseUsages)
                {
                    writer.WriteLine("{0},{1},{2},{3}", usage.Date, usage.Time, usage.License, usage.User);
                }
            }
        }
    }
}
