using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexLmLogParser
{
    public static class ResultFormatter
    {
        public static void DisplayResults(IEnumerable<LicenseUsage> licenseUsages)
        {
            var dateGroup = licenseUsages.GroupBy(usage => usage.Date);

            foreach (var date in dateGroup)
            {
                Console.WriteLine("Date: {0}", date.Key);
                Console.WriteLine("Total licenses used: {0}", date.Count());
                Console.WriteLine("Total licenses used by user: {0}", date.GroupBy(usage => usage.User).Count());
                Console.WriteLine();

                Console.WriteLine("Users:");

                int userColumnWidth = Math.Max("User".Length, dateGroup.Max(group => group.Max(usage => usage.User.Length))) + 2;
                int licenseColumnWidth = Math.Max("License".Length, dateGroup.Max(group => group.Max(usage => usage.License.Length))) + 2;

                Console.WriteLine(new string('-', userColumnWidth + licenseColumnWidth + 3));
                Console.WriteLine("| {0,-" + userColumnWidth + "}| {1,-" + licenseColumnWidth + "}|", "User", "License");
                Console.WriteLine(new string('-', userColumnWidth + licenseColumnWidth + 3));

                foreach (var user in date.GroupBy(usage => usage.User))
                {
                    Console.Write("| {0,-" + userColumnWidth + "}| ", user.Key);

                    foreach (var license in user.GroupBy(usage => usage.License))
                    {
                        Console.Write("{0,-" + licenseColumnWidth + "} |", license.Key);
                    }

                    Console.WriteLine();
                }

                Console.WriteLine(new string('-', userColumnWidth + licenseColumnWidth + 3));
                Console.WriteLine();

                Console.WriteLine("Total licenses used by license: {0}", date.GroupBy(usage => usage.License).Count());
                Console.WriteLine();
            }
        }
    }
}
