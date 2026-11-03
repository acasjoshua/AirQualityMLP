using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using AirQualityMLP.Experiment;
using AirQualityMLP.Training;

namespace AirQualityMLP
{
    /// <summary>Writes everything to the console AND to a log file.</summary>
    internal class TeeWriter : TextWriter
    {
        private readonly TextWriter a, b;
        public TeeWriter(TextWriter a, TextWriter b) { this.a = a; this.b = b; }
        public override Encoding Encoding { get { return a.Encoding; } }
        public override void Write(char value) { a.Write(value); b.Write(value); }
        public override void Write(string value) { a.Write(value); b.Write(value); }
        public override void WriteLine(string value) { a.WriteLine(value); b.WriteLine(value); }
        public override void Flush() { a.Flush(); b.Flush(); }
    }

    /// <summary>
    /// PM2.5 estimation (regression) with the hand-written 3-layer back-propagation network.
    ///
    ///   AirQualityMLP.exe                 opens the window (Form1)
    ///   AirQualityMLP.exe --run           same experiment from the command line (console output + results folder)
    ///   AirQualityMLP.exe --quick         tiny grid / few epochs (seconds) to check that everything works
    ///   AirQualityMLP.exe --gradcheck     only verify the back-propagation maths
    ///   --data  path\to\air-quality-data.csv    --out  folder for the results
    /// </summary>
    internal static class Program
    {
        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int processId);

        [STAThread]
        private static int Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;   // same output on every PC

            if (args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
                return 0;
            }

            // command-line mode: a windowed program has no console of its own, so borrow the parent's
            if (Environment.OSVersion.Platform == PlatformID.Win32NT) AttachConsole(-1);
            return RunConsole(args);
        }

        private static int RunConsole(string[] args)
        {
            bool quick = false, gradOnly = false;
            string dataPath = null, outDir = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--quick") quick = true;
                else if (args[i] == "--run") { }
                else if (args[i] == "--gradcheck") gradOnly = true;
                else if (args[i] == "--data" && i + 1 < args.Length) dataPath = args[++i];
                else if (args[i] == "--out" && i + 1 < args.Length) outDir = args[++i];
                else { Console.WriteLine("Unknown argument: " + args[i]); return 1; }
            }

            if (gradOnly) return GradientCheck.Run(Console.Out) ? 0 : 2;

            if (dataPath == null) dataPath = ProjectPaths.FindData();
            if (dataPath == null || !File.Exists(dataPath))
            {
                Console.WriteLine("Cannot find data\\air-quality-data.csv. Pass its location with --data <file>.");
                return 1;
            }
            if (outDir == null) outDir = ProjectPaths.DefaultResultsDir(dataPath, quick);
            Directory.CreateDirectory(outDir);

            using (StreamWriter logFile = new StreamWriter(Path.Combine(outDir, "run_log.txt")))
            {
                logFile.AutoFlush = true;
                TextWriter log = new TeeWriter(Console.Out, logFile);
                ExperimentOptions options = new ExperimentOptions();
                options.DataPath = dataPath; options.OutDir = outDir; options.Quick = quick;
                ExperimentResult result = ExperimentRunner.Run(options, log, null);
                return result.Cancelled ? 2 : 0;
            }
        }
    }

    /// <summary>Finds data\air-quality-data.csv by walking up from the .exe folder (bin\Debug -> project -> solution).</summary>
    internal static class ProjectPaths
    {
        public static string FindData()
        {
            string relative = Path.Combine("data", "air-quality-data.csv");
            foreach (string start in new string[] { AppDomain.CurrentDomain.BaseDirectory, Directory.GetCurrentDirectory() })
            {
                DirectoryInfo dir = new DirectoryInfo(start);
                for (int up = 0; up < 7 && dir != null; up++, dir = dir.Parent)
                {
                    string candidate = Path.Combine(dir.FullName, relative);
                    if (File.Exists(candidate)) return candidate;
                }
            }
            return null;
        }

        public static string DefaultResultsDir(string dataPath, bool quick)
        {
            string folder = Path.GetDirectoryName(Path.GetFullPath(dataPath));
            // usual layout: <repo>\data\air-quality-data.csv  ->  <repo>\results
            string root = string.Compare(Path.GetFileName(folder), "data", StringComparison.OrdinalIgnoreCase) == 0
                ? Path.GetDirectoryName(folder) : folder;
            return Path.Combine(root, quick ? "results_quick" : "results");
        }
    }
}
