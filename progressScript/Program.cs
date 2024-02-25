using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        string scriptPath = "create_folders_script.ps1";

        Console.Write("Count of Folders: ");
        string inputCount = Console.ReadLine();
        int count;
        string scriptArgument;
        List<string> nameList = new List<string>();

        if (!int.TryParse(inputCount, out count))
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
            return;
        }

        Console.WriteLine("Name of Folders:");
        for (int i = 0; i < count; i++) 
        {
            Console.Write($"{i + 1}. "); 
            string name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name)) 
                nameList.Add(name);
        }

        Console.Write("Path: ");
        string path = Console.ReadLine();

        scriptArgument = $"-count {count} -name '{string.Join(",", nameList)}' -path {path}";


        Process process = new Process();
        process.StartInfo.FileName = "powershell.exe";
        process.StartInfo.Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\" {scriptArgument}";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;


        Thread loadingThread = new Thread(() =>
        {
            int progress = 0;

            while (progress <= 100)
            {
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
