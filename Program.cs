using System.Diagnostics;

while (true)
{
    Console.Clear();
    Console.WriteLine("Latex vlna script");
    Console.WriteLine("*****************");

    Console.Write("Add root directory path (absolute): ");

    string[] ignoredFiles = new string[] { "appendix", "literature", "main" };
    string? path = Console.ReadLine();
    if (path == "" || path == null)
    {
        Console.WriteLine("Invalid path!");
        Console.WriteLine("Press any key to try it again");
        Console.ReadKey();
        return;
    }
    else
    {
        if (Directory.Exists(path))
        {
            string[] allFiles = Directory
            .GetFiles(path, "*.tex", SearchOption.AllDirectories)
            .Where(file =>
            {
                var arr = file.Split("\\");
                string fileName = arr[arr.Length - 1].Split('.')[0];
                return !(ignoredFiles.Contains(fileName));
            })
            .ToArray();

            Console.WriteLine($"Affected files ({allFiles.Length}):");
            foreach (var item in allFiles)
            {
                Console.WriteLine(item);
            }
            
            Console.WriteLine("Ready? (y/n)");
            if (Console.ReadKey().Key != ConsoleKey.Y) continue;

            var p = new Process();
            foreach (string file in allFiles)
            {
                if (File.Exists(file))
                {
                    p.StartInfo.FileName = "vlna";
                    p.StartInfo.Arguments = "-l -m -n \"" + file + "\"";
                    p.Start();
                    p.WaitForExit();
                }
                Console.WriteLine();
            }


            var rubbish = Directory.GetFiles(path, "*.te~", SearchOption.AllDirectories);
            foreach (string file in rubbish)
            {
                File.Delete(file);
            }

            Console.WriteLine("Script has been executed. Do you want to run it again? (y/n)");
            if (Console.ReadKey().Key == ConsoleKey.Y) continue;
            else break;
        }
    }
}
