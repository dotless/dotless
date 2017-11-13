using System.Collections.Generic;
using System.IO;

namespace dotless.CompatibilityTests
{
    public class Ignore
    {
        public static IDictionary<string, string> Load(string ignoreFile)
        {
            var ignores = new Dictionary<string, string>();

            foreach (var line in File.ReadLines(ignoreFile))
            {
                var parts = line.Split(';');
                if (parts.Length == 0) continue;

                var file = parts[0].Trim();
                if (file.Length == 0) continue;
                var reason = parts.Length > 1 ? parts[1] : null;

                ignores.Add(file, reason);
            }

            return ignores;
        }
    }
}