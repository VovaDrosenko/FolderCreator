using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        string scriptPath = "create_folders_script.ps1";

        Console.Write("Count of Folders: ");
        string inputCount = Console.ReadLine();
        int count;
        string scriptArgument;
        

        if (!int.TryParse(inputCount, out count))
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
            return;
        }

        Console.Write("Name of Folders:");
        string name = Console.ReadLine();


        Console.Write("Path:");
        string path = Console.ReadLine();

        scriptArgument = $"-count {count} -name {name} -path {path}";

        Process process = new Process();
        process.StartInfo.FileName = "powershell.exe";
        process.StartInfo.Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\" {scriptArgument}";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;


        // Запускаємо окремий потік для виводу тексту завантаження
        Thread loadingThread = new Thread(() =>
        {
            // Початковий відсоток прогресу
            int progress = 0;

            while (progress <= 100)
            {
                // Оновлення рядка прогресу
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Loading progress: {progress}%");

                progress += 10;

                Thread.Sleep(100);
            }
        });
        loadingThread.Start();

        process.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                if (e.Data.Contains("Progress:"))
                {
                    Console.WriteLine(e.Data);
                }
            }
        };

        process.ErrorDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Console.WriteLine($"Error: {e.Data}");
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        loadingThread.Join();

        if (process.ExitCode == 0)
        {
            Console.WriteLine("\nScript executed successfully.");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("\nAn error occurred while executing the script.");
        }
    }
}
