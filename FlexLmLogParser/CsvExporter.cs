using System.Collections.Generic;
using System.IO;

namespace FlexLmLogParser
{
    public static class CsvExporter
    {
        public static void ExportToCsv(IEnumerable<LicenseUsage> licenseUsages, string outputFilePath)
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