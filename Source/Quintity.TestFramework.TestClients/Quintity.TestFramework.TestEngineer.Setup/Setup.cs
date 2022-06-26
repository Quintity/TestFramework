using System;
using System.Linq;
using Microsoft.Win32;
using WixSharp;
using WixSharp.CommonTasks;

namespace Quintity.TestFramework.TestEngineer.Setup
{
    class Setup
    {
#if DEBUG
        static private string build = "Debug";
#else
        static private string build = "Release";
#endif
        static void Main(string[] args)
        {
            var workingFolders = new Feature("Working Folders");
            //var binaries = new Feature("TestFramework Binaries");
            //var documentation = new Feature("Documentation");
            var samples = new Feature("Sample Test Application");
            // var registry = new Feature("TestFramework Registry");

            Project project = new Project("Quintity.TestFramework",
            // Create program files folder
            //new Dir(@"%ProgramFiles%\Quintity\Quintity TestEngineer 3.0",
            new Dir(@"C:\Quintity Testframework",
                    //new FileShortcut("Quintity TestEngineer - Shortcut", "C:\\temp"),
                    // Add the TestEngineer to folder
                    new File($@"..\Quintity.TestFramework.TestEngineer\bin\{build}\Quintity.TestFramework.TestEngineer.exe",
                        // Add shortcut to program files folder
                        new FileShortcut("Quintity TestEngineer", @"%ProgramMenu%\Quintity\Quintity TestEngineer"),
                        // Add shortcut to desktop
                        new FileShortcut("Quintity TestEngineer", @"%Desktop%")),

                    new File($@"..\Quintity.TestFramework.TestEngineer\bin\{build}\Quintity.TestFramework.Core.dll"),
                    new File($@"..\Quintity.TestFramework.TestEngineer\bin\{build}\Quintity.TestFramework.Runtime.dll"),

                    // Add app config to folder
                    new File($@"..\Quintity.TestFramework.TestEngineer\bin\{build}\Quintity.TestFramework.TestEngineer.exe.Config"),

                    new File($@"..\Quintity.TestFramework.TestRunner\bin\{build}\Quintity.TestFramework.TestRunner.exe"),

                    new File($@"..\Quintity.TestFramework.TestRunner\bin\{build}\Quintity.TestFramework.TestRunner.exe.Config"),

                    new File($@"..\Quintity.TestFramework.TestRunner\bin\{build}\log4net.dll"),

                    new File($@"..\Quintity.TestFramework.TestRunner\bin\{build}\log4net.xml"),

                    // new File(@"..\Quintity.TestFramework.TestEngineer\bin\{build}\Quintity.TestFramework.Core.dll"),

                    // Add Desktop TestEngineer shortcut
                    new ExeFileShortcut("Quintity.TestFramework.Uninstall", "[System64Folder]msiexec.exe", "/x [ProductCode]"),
                    //new ExeFileShortcut("Quintity.TestFramework.TestEngineer.Shortcut",
                    //    @"[INSTALLDIR]Quintity.TestFramework.TestEngineer.exe", "/s="),

                    // Add TestEngineer to StartUp menu
                    new Dir("%Startup%",
                        new ExeFileShortcut("Quintity TestEngineer", "[INSTALLDIR]Quintity.TestFramework.TestEngineer.exe", "")),

                    // Add core library to GAC
                    //new Assembly(new Id("GAC"), $@"..\Quintity.TestFramework.TestEngineer\bin\{build}\Quintity.TestFramework.Core.dll", true),

                    // Install TestRunner folders
                    new Dir(workingFolders, @"TestSuites"),
                    new Dir(workingFolders, @"TestConfigs"),
                    new Dir(workingFolders, @"TestProperties"),
                    new Dir(workingFolders, @"TestOutput"),
                    new Dir(workingFolders, @"TestResults"),
                    new Dir(workingFolders, @"TestGolds"),
                    new Dir(workingFolders, @"TestData"),
                    new Dir(workingFolders, @"TestAssemblies")),

                //  Places QTF Core reference assembly for VS reference.
                new Dir(@"%ProgramFiles%\Reference Assemblies\Quintity LLC",
                    new File(new Id("CoreReference"), $@"..\Quintity.TestFramework.TestEngineer\bin\{build}\Quintity.TestFramework.Core.dll"),
                    new File(new Id("RuntimeReference"), $@"..\Quintity.TestFramework.TestEngineer\bin\{build}\Quintity.TestFramework.Runtime.dll")),

                // Registry values
                // Supports loading QTF Core assembly in VS Reference Assembly dialog.
                new RegValue(RegistryHive.LocalMachine, @"software\WOW6432Node\Microsoft\.NETFramework\v4.0.30319\AssemblyFoldersEx\Quintity TestFramework",
                    string.Empty, @"C:\Program Files (x86)\Reference Assemblies\Quintity LLC"),

                new RegValue(RegistryHive.LocalMachine, "Software\\QuintityLLC\\Quintity TestEngineer", "Version", "3.5.0"),
                new RegValue(RegistryHive.LocalMachine, "Software\\QuintityLLC\\Quintity TestEngineer", "Path", "[INSTALLDIR]")

                //new Dir(new Id("VS2015_ITEMTEMPLATES_DIR"), "VS2015ItemTemplates",
                //    new Dir(new Id("VS2015CItemsSharpFolder"), "CSHARP",
                //        new Dir(new Id("VS2015ItemQuintityFolder"), "Quintity",
                //            new File(new Id("VS2015BasicTestClassZip"), @"..\Quintity.Repository\ItemTemplates\BasicTestClass\BasicTestClass.zip"))))
            );

            project.OutDir = $@".\bin\{build}\";
            project.LicenceFile = @"..\License.rtf";
            project.ControlPanelInfo.Manufacturer = "Quintity LLC";
            project.ControlPanelInfo.InstallLocation = "[INSTALLDIR]";
            project.GUID = new Guid("0C4A79D1-AD9B-4FB1-906F-BB3B65DDCF18");
            project.UI = WUI.WixUI_Minimal;
            project.BuildMsi();
        }
    }
}
