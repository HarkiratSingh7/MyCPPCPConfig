using CPPSublimeConfigurator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CPPCPConfig
{
    public abstract class BaseConfig
    {
        public string PPath { get; set; }
        public string ProjectName { get; set; }
        public ProgramConfiguration Configs { get; set; }

        public long buffer { get; set; }

        public void SetVars(string _Path, string _ProjectName)
        {
            PPath = _Path;
            ProjectName = _ProjectName;
            Configs = Settings.GetProgramConfiguration();
            if (Configs == null)
                throw new Exception("Configuration file failed to exists");
            if (string.IsNullOrEmpty(_Path))
            {
                PPath = Configs.ParentFolder;
            }
        }

        public bool BasicSetUp()
        {
            if (!Directory.Exists(PPath))
                return false;
            PPath = Path.Combine(PPath, ProjectName);
            Directory.CreateDirectory(PPath); // Root folder created

            // Create Source File, Inputf.txt and Outputf.txt
            string templateCode = File.ReadAllText(Configs.SourceTemplate);
            buffer = (new FileInfo(Configs.SourceTemplate)).Length;
            File.WriteAllText(Path.Combine(PPath, SourceName), templateCode);

            File.Create(Path.Combine(PPath, InputFName)).Close();
            File.Create(Path.Combine(PPath, OutputFName)).Close();

            //PrecompileHeader();
            return true;
        }

        public static void PrecompileHeader(ProgramConfiguration Configs)
        {
            string pFPath = Path.Combine(Directory.GetParent(Configs.LocationPCHBITSTD).FullName, "stdc++.h.gch");
            if (!File.Exists(pFPath))
            {
                //Process prc = Process.Start("g++", "-std=c++14 \"" + Configs.LocationPCHBITSTD + "\"");
                Process prc = new Process();
                prc.StartInfo.FileName = "g++";
                prc.StartInfo.Arguments = "-std=c++14 \"" + Configs.LocationPCHBITSTD + "\"";
                prc.StartInfo.Verb = "runas";
                prc.Start();
                prc.WaitForExit();
                if (!File.Exists(pFPath))
                {
                    Console.WriteLine("Unable to generate precompiled header file. May be you have not added g++ to your path yet (should be done manually), it might not support stdc++14 or it is not installed");
                }
            }
        }
        public abstract bool SetUpSourceFiles();

        public const string SourceName = "Source.cpp";
        public const string InputFName = "inputf.txt";	// These are not to be changed.
        public const string OutputFName = "outputf.txt";
    }
}
