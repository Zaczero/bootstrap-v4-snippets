using System;
using System.Collections.Generic;
using System.IO;

namespace bootstrap_v4_snippets_build
{
    public static class Program
    {
        private const string SourceDirectory = "../../../../../source/";
        private const string OutputFile = "../../../../../bootstrap-v4-snippets/snippets/snippets.json";

        public static void Main(string[] args)
        {
            var sourceFiles = new List<string>();
            DirectoryScanner.FindFilesRecursive(SourceDirectory, sourceFiles);
            Console.WriteLine($"Source files found: {sourceFiles.Count}");

            var outputStream = new FileStream(OutputFile, FileMode.Create);
            var writeStream = new StreamWriter(outputStream) {AutoFlush = false};

            writeStream.WriteLine("{");

            for (var i = 0; i < sourceFiles.Count; i++)
            {
                if (i % 10 == 9)
                {
                    // flush every 10 snippets
                    writeStream.Flush();
                }

                var isLastSnippet = i + 1 == sourceFiles.Count;
                var snippetSuffix = isLastSnippet ? "" : ",";

                var snippetName = Path.GetFileNameWithoutExtension(sourceFiles[i]);
                var snippetLines = File.ReadAllLines(sourceFiles[i]);
                Console.WriteLine($"{snippetName}...");

                writeStream.WriteLine($"\t\"{snippetName}\": {{");
                writeStream.WriteLine($"\t\t\"prefix\": \"{snippetName}\",");
                writeStream.WriteLine($"\t\t\"body\": [");

                for (var j = 0; j < snippetLines.Length; j++)
                {
                    var isLastSnippetLine = j + 1 == snippetLines.Length;
                    var snippetLineSuffix = isLastSnippetLine ? "" : ",";

                    var snippetLine = snippetLines[j]
                        .Replace("    ", "\\t")
                        .Replace("\"", "\\\"");

                    writeStream.WriteLine($"\t\t\t\"{snippetLine}\"{snippetLineSuffix}");
                }

                writeStream.WriteLine($"\t\t]");
                writeStream.WriteLine($"\t}}{snippetSuffix}");
            }

            writeStream.WriteLine("}");
            writeStream.Flush();

            writeStream.Dispose();
            outputStream.Dispose();
            Console.WriteLine("Done!");
        }
    }
}
