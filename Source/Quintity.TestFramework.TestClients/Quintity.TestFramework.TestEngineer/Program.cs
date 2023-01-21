using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Quintity.TestFramework.Core;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Quintity.TestFramework.TestEngineer
{
    static class Program
    {
        static internal string TestPropertiesFile;
        static internal string TestSuiteFile;
        static internal string TestListenersFile;
        static internal string TestPerformanceFile;
        static internal string TestEnvironments;
        static internal bool SuppressExecution = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if (args.Length != 0)
            {
                //Cycle through arguments.
                for (int index = 0; index < args.Length; index++)
                {
                    string arg = args[index].ToUpper().Trim();

                    SuppressExecution = arg.ToUpper().Equals("/X") ? true : false;

                    arg = arg.Length > 2 ? arg.Substring(0, 2) : null;

                    switch (arg)
                    {
                        case "/s":
                        case "/S":
                            TestSuiteFile = extractUriFromArg(args[index]);
                            break;

                        case "/p":
                        case "/P":
                            TestPropertiesFile = extractUriFromArg(args[index]);
                            break;

                        case "/l":
                        case "/L":
                            TestListenersFile = extractUriFromArg(args[index]);
                            break;

                        case "/e":
                        case "/E":
                            TestEnvironments = extractUriFromArg(args[index]);
                            break;

                        //case "/r":
                        //case "/R":
                        //    TestPerformanceFile = extractUriFromArg(args[index]);
                        //    break;

                        case "/x":
                        case "/X":
                            SuppressExecution = true;
                            break;
                        default:
                            break;
                    }
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        static private string extractUriFromArg(string arg)
        {
            string file = null;

            string[] parts = arg.Split('=');

            if (parts.Length == 2)
            {
                file = parts[1].Trim();
            }

            return file;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                @"An unhandled exception has occurred:" + Environment.NewLine + 
                ((Exception)e.ExceptionObject).ToString(),
                "Quintity TestEngineer",
                MessageBoxButtons.OK, 
                MessageBoxIcon.Error);
        }
    }
}
