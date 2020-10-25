using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CPPSublimeConfigurator
{
    public static class Settings
    {
        const string ConfigFileName = "appsettings.json";
        const string PATH_WRONG = "Your path is wrong please write correct path";
        public static string GetExtension()
        {
            string ext = string.Empty;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                ext = ".exe";
            return ext;
        }

        public static void FillBasic(ref ProgramConfiguration Config)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                //C:\\Users\\userName\\AppData\\Roaming\\Sublime Text 3\\Packages\\User
                Config.BuildSysLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Sublime Text 3\\Packages\\User");
                // C:\\Program Files\\mingw-w64\\x86_64-8.1.0-posix-seh-rt_v6-rev0\\mingw64\\lib\\gcc\\x86_64-w64-mingw32\\8.1.0\\include\\c++\\x86_64-w64-mingw32\\bits\\stdc++.h
                Config.LocationPCHBITSTD = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "mingw-w64\\x86_64-8.1.0-posix-seh-rt_v6-rev0\\mingw64\\lib\\gcc\\x86_64-w64-mingw32\\8.1.0\\include\\c++\\x86_64-w64-mingw32\\bits\\stdc++.h");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                string usr = Environment.GetEnvironmentVariable("USER");
                Config.LocationPCHBITSTD = "/usr/include/x86_64-linux-gnu/c++/10/bits/stdc++.h";
                Config.BuildSysLocation = $"/home/{usr}/.config/sublime-text-3/Packages/User";
            }
        }
        public static ProgramConfiguration CreateConfigs(string path = "")
        {
            if (string.IsNullOrEmpty(path))
                path = Path.Combine(AppContext.BaseDirectory, ConfigFileName);

            ProgramConfiguration Configs = new ProgramConfiguration();
            FillBasic(ref Configs);
            Console.WriteLine("\nConfiguration File doesn't exists. So a new file will be created\nPlease specify the full location (including full name) of your source template?");
            Configs.SourceTemplate = Console.ReadLine();
            while (!File.Exists(Configs.SourceTemplate))
            {
                Console.WriteLine(PATH_WRONG);
                Configs.SourceTemplate = Console.ReadLine();
            }
            Console.WriteLine("Please type the location of a folder where you want to create project folders");
            Configs.ParentFolder = Console.ReadLine();

            while (!Directory.Exists(Configs.ParentFolder))
            {
                Console.WriteLine(PATH_WRONG);
                Configs.ParentFolder = Console.ReadLine();
            }

            Console.WriteLine("Checking the location of your build system folders.");
            
            while (!Directory.Exists(Configs.BuildSysLocation))
            {
                Console.WriteLine(PATH_WRONG+ "\nWrong Path: " + Configs.BuildSysLocation);
                Configs.BuildSysLocation = Console.ReadLine();
            }
            Configs.BuildSysName = "CPPCPP14BS.sublime-build";
            try
            {
                string ext = GetExtension();
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
                File.WriteAllText(Path.Combine(Configs.BuildSysLocation, Configs.BuildSysName), FS);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Some thing bad happened");
                Console.WriteLine(ex.Message);
                return null;
            }


            Console.WriteLine("Checking location of Mingw bits\\stdc++ file");

            while (!File.Exists(Configs.LocationPCHBITSTD))
            {
                Console.WriteLine(PATH_WRONG + "\nWrong Path: " + Configs.LocationPCHBITSTD);
                Configs.LocationPCHBITSTD = Console.ReadLine();
            }

            string output = JsonConvert.SerializeObject(Configs);
            File.WriteAllText(path, output);

            return Configs;
        }
        public static ProgramConfiguration GetProgramConfiguration()
        {
            ProgramConfiguration Configs;

            string path = Path.Combine(AppContext.BaseDirectory, ConfigFileName);
            if (File.Exists(path))
            {
                Configs = JsonConvert.DeserializeObject<ProgramConfiguration>(File.ReadAllText(path));
            }
            else
            {
                Configs = CreateConfigs(path);
            }
            return Configs;
        }
    }
}
