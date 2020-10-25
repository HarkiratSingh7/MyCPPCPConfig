using CPPSublimeConfigurator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CPPCPConfig
{
    public class VCConfig : BaseConfig
    {
        public VCConfig(string _Path, string _ProjectName)
        {
            SetVars(_Path, _ProjectName);
        }

        public override bool SetUpSourceFiles()
        {
            if (!BasicSetUp())
                return false;

            PPath = Path.Combine(PPath, ".vscode");
            Directory.CreateDirectory(PPath);
            string EXT = Settings.GetExtension();
            string PROP_TMP = @"
{
    ""configurations"": [
        {
            ""name"": ""G++"",
            ""includePath"": [
                ""${workspaceFolder}/**""
            ],
            ""defines"": [
                ""_DEBUG"",
                ""UNICODE"",
                ""_UNICODE""
            ],
            ""compilerPath"": ""g++" + EXT + @""",
            ""cStandard"": ""c11"",
            ""cppStandard"": ""c++14"",
            ""intelliSenseMode"": ""gcc-x64""
        }
    ],
    ""version"": 4
}
";
            File.WriteAllText(Path.Combine(PPath, "c_cpp_properties.json"), PROP_TMP);

            string PROP_LAUNCH = @"
{
    ""version"": ""0.2.0"",
    ""configurations"": [
      {
        ""name"": ""Run With G++"",
        ""type"": ""cppdbg"",
        ""request"": ""launch"",
        ""program"": ""${fileDirname}\\${fileBasenameNoExtension}.exe"",
        ""args"": [
            ""<inputf.txt"",
            "">outputf.txt""
        ],
        ""stopAtEntry"": false,
        ""cwd"": ""${workspaceFolder}"",
        ""environment"": [],
        ""externalConsole"": false,
        ""MIMode"": ""gdb"",
        ""miDebuggerPath"": ""gdb"+ EXT + @""",
        ""setupCommands"": [
          {
            ""description"": ""Enable pretty-printing for gdb"",
            ""text"": ""-enable-pretty-printing"",
            ""ignoreFailures"": true
          }
        ],
        ""preLaunchTask"": ""C/C++: g++ build active file""
      }
    ]
  }
";

            File.WriteAllText(Path.Combine(PPath, "launch.json"), PROP_LAUNCH);

            string sep = "\\\\";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                sep = "/";
            string PROP_TASK = @"
{
	""version"": ""2.0.0"",
	""tasks"": [
		{
			""type"": ""shell"",
			""label"": ""C/C++: g++ build active file"",
			""command"": ""g++"",
			""args"": [
				""-g"",
				""${file}"",
				""-o"",
				""${fileDirname}" + sep + @"${fileBasenameNoExtension}.exe""
			],
			""options"": {
				""cwd"": ""${workspaceFolder}""
			},
			""problemMatcher"": [
				""$gcc""
			],
			""group"": ""build""
		}
	]
}
";

            File.WriteAllText(Path.Combine(PPath, "tasks.json"), PROP_TASK);

            return true;
        }
    }
}
