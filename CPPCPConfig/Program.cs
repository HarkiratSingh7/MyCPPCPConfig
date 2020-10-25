using CPPCPConfig;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace CPPSublimeConfigurator
{
    class Program
    {
        static bool Running = true;
        static void PrintCommandDetail(string cmd, params string[] opts)
        {
            var def = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"<{cmd}> ");
            for (int i = 0; i < opts.Length; i++)
            {
                Console.Write($"<{opts[i]}> ");
            }
            Console.Write("\n");
            Console.ForegroundColor = def;
        }

        static void WriteError(string err)
        {
            var def = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(err);
            Console.ForegroundColor = def;
        }

        static void WriteDetail(string ttt)
        {
            var def = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(ttt);
            Console.ForegroundColor = def;
        }

        static readonly string[] CMDS = { "create", "make", "open", "exit" };
        static void ShowDetailOfCommand(string cmd)
        {
            switch (cmd.ToLower())
            {
                case "create":
                    PrintCommandDetail("create", "subl/vsc", "Project-Name");
                    PrintCommandDetail("create", "subl/vsc", "CustomPath", "Project-Name");
                    WriteDetail("\t\t create allows you to instantly create a sublime text workspace in the folder you think of");
                    break;

                case "set":
                    WriteDetail("To Configurate the settings");
                    break;
                case "make":
                    PrintCommandDetail("make, get, gen, generate", "configs, configuration", "build-system, bs, buildsystem", "info");
                    WriteDetail("\t\t config: Allows you to reset the configuration of your settings. This will erase all");
                    PrintCommandDetail("build-system, bs, buildsystem", "optional: set,s,apply");
                    WriteDetail("\t\t bs: To set up build system.");
                    WriteDetail("\t\t info: Generates info based on current configuration");
                    break;

                case "open":
                    PrintCommandDetail("open", "project name");
                    WriteDetail("Project name should be a valid project in the parent folder");
                    break;

                //case "modify-parent":
                //    PrintCommandDetail
                //    break;

                case "exit":
                    PrintCommandDetail("exit,quit,close");
                    WriteDetail("\t\t Exit this");
                    break;
                default:
                    break;
            }
            Console.Write("\n");
        }

        static void CMDCreate(string[] input)
        {
            BaseConfig cfg = null;
            switch (input[1].ToLower())
            {
                case "subl":
                    if (input.Length == 3)
                    {
                        cfg = new SublimeCPPConfig("", input[2]);
                    }
                    else if (input.Length == 4)
                    {
                        cfg = new SublimeCPPConfig(input[2], input[3]);
                    }
                    break;

                case "vsc":
                    if (input.Length == 3)
                    {
                        cfg = new VCConfig("", input[2]);
                    }
                    else if (input.Length == 4)
                    {
                        cfg = new VCConfig(input[2], input[3]);
                    }
                    break;
                default:
                    break;
            }

            if (cfg == null)
            {
                WriteError("Syntax Error");
                ShowDetailOfCommand("create");
                return;
            }
            if (!cfg.SetUpSourceFiles())
            {
                WriteError("Cannot create the workspace. Some problem happened");
            }
            else
            {
                WriteDetail(Directory.GetParent(cfg.PPath).FullName);
            }
        }

        static void WRKSPOpen(string[] input)
        {
            if (input.Length == 2)
            {
                string folderName = input[1];
                if (File.Exists(Path.Combine(Settings.GetProgramConfiguration().ParentFolder, folderName, folderName + ".sublime-project")))
                {
                    try
                    {
                        string fld = Path.Combine(Settings.GetProgramConfiguration().ParentFolder, folderName, folderName + ".sublime-project");
                        Process.Start("subl", "--project \"" + fld + "\"").WaitForExit();
                        Console.WriteLine("Note please goto tools->build system and select the CPPCPP14BS build system");
                        Console.WriteLine("Activate the source.cpp. Press Ctrl + B to build and run.");
                    }
                    catch (Exception ex)
                    {
                        WriteError(ex.Message);
                        WriteError("Try adding subl to your path.");
                    }
                }
                else if (Directory.Exists(Path.Combine(Settings.GetProgramConfiguration().ParentFolder, folderName, ".vscode")))
                {
                    try
                    {
                        string fld = Path.Combine(Settings.GetProgramConfiguration().ParentFolder, folderName);
                        var ss = new Process();
                        ss.StartInfo.FileName = "code";
                        ss.StartInfo.Arguments = "-n \"" + fld + "\"";
                        ss.StartInfo.UseShellExecute = true;
                        ss.Start();
                        ss.WaitForExit();

                        Console.WriteLine("Activate the source.cpp and press F5 to build.");
                    }
                    catch (Exception ex)
                    {
                        WriteError(ex.Message);
                        WriteError("Try adding 'code' to your path.");
                    }
                }

            }
            else
            {
                WriteError("Wrong usage of 'open' command. See help.");
            }
        }

        static void CMDMake(string[] input)
        {
            if (input.Length >= 2)
            {
                switch (input[1].ToLower())
                {
                    case "build-system":
                    case "bs":
                    case "buildsystem":
                        string ext = Settings.GetExtension();
                        string FS = @"
                                        {
                                        ""cmd"": [""g++" + ext + @""",""-std=c++14"", ""${file}"", ""-o"", ""${file_base_name}" + ext + @""", ""&&"" , ""${file_base_name}" + ext + @"<inputf.txt>outputf.txt""],
                                        ""shell"":true,
                                        ""working_dir"":""$file_path"",
                                        ""selector"":""Source.cpp""
                                        }
                                        ";
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                            FS = @"
{
""cmd"": [""g++ -std=c++14 \""${file}\""  -o \""${file_path}/${file_base_name}\"" && ${file_path}/${file_base_name}<inputf.txt>outputf.txt""],
""shell"":true,
""working_dir"":""${file_path}"",
""selector"":""Source.cpp""
}

";
                        bool generate = false;
                        if (input.Length == 3)
                        {
                            switch (input[2].ToLower())
                            {
                                case "set":
                                case "s":
                                case "apply":
                                    generate = true;
                                    break;
                            }
                        }
                        if (generate)
                        {
                            ProgramConfiguration cpf = Settings.GetProgramConfiguration();
                            string path = Path.Combine(cpf.BuildSysLocation, cpf.BuildSysName);
                            File.WriteAllText(path, FS);
                        }
                        else
                        {
                            Console.WriteLine("Please copy these lines\n" + FS);
                        }
                        break;

                    case "config":
                    case "configuration":
                    case "configs":
                    case "configurations":
                        Console.Write("Configuration file deleted successfully. Now");
                        Settings.CreateConfigs();
                        break;

                    case "info":
                        if (input[0].ToLower() != "get")
                        {
                            WriteError("Wrong command. Please use get info to know bs. And make bs to generate bs.");
                            break;
                        }
                        var cfg = Settings.GetProgramConfiguration();
                        WriteDetail("Current Working Dir: " + cfg.ParentFolder);
                        WriteDetail("C++ Template: " + cfg.SourceTemplate);
                        WriteDetail("C++ Build System: " + cfg.BuildSysName);
                        string info = File.ReadAllText(Path.Combine(cfg.BuildSysLocation, cfg.BuildSysName));
                        WriteError("Build Config is :");
                        WriteDetail(info);
                        break;
                    default:
                        WriteError("Your command '" + input[0] + "' is wrong");
                        ShowDetailOfCommand("config");
                        ShowDetailOfCommand("bs");
                        break;
                }

            }
        }
        static void Main()
        {
            while (Running)
            {
                Console.Write("$>");
                var input = Console.ReadLine().Split(" ");
                switch (input[0].ToLower())
                {
                    case "set":
                        var stg = Settings.CreateConfigs();
                        Console.WriteLine("Precompiling header to speed up");
                        BaseConfig.PrecompileHeader(stg);
                        Console.WriteLine("Always use this command as super user");
                        Console.WriteLine("Now you may start this program as normal user. For linux based OS start as normal user.");
                        Running = false;
                        break;

                    case "create":
                        CMDCreate(input);
                        break;

                    case "make":
                    case "get":
                    case "gen":
                    case "generate":
                        CMDMake(input);
                        break;

                    case "open":
                        WRKSPOpen(input);
                        break;

                    case "exit":
                    case "quit":
                    case "close":
                        Running = false;
                        break;


                    default:
                        WriteError("You entered a wrong command. Type help or ? for more info");
                        break;
                    case "help":
                    case "h":
                    case "?":
                        foreach (var item in CMDS)
                        {
                            ShowDetailOfCommand(item);
                        }
                        break;
                }
                Console.Write("\n");
            }
        }

    }
}
