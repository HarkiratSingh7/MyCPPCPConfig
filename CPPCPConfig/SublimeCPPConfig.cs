using CPPCPConfig;
using System;
using System.Diagnostics;
using System.IO;

namespace CPPSublimeConfigurator
{
    public class SublimeCPPConfig : BaseConfig
    {
        public SublimeCPPConfig(string _Path, string _ProjectName)
        {
			SetVars(_Path, _ProjectName);
        }

        public override bool SetUpSourceFiles()
        {
			if (!BasicSetUp())
				return false;

			// I am thinking to make a class based on the config structure.
            // Add the project and workspace configs
            string WORKSPACE = @"{
	""auto_complete"":
	{
		""selected_items"":
		[
		]
	},
	""buffers"":
	[
		{
			""file"": """ + SourceName + @""",
			""settings"":
			{
				""buffer_size"": " + buffer.ToString() + @",
				""encoding"": ""UTF-8"",
				""line_ending"": ""Windows""
			}
		},
		{
			""file"": """ + InputFName + @""",
			""settings"":
			{
				""buffer_size"": 0,
				""encoding"": ""UTF-8"",
				""line_ending"": ""Windows""
			}
		},
		{
			""file"": """ + OutputFName + @""",
			""settings"":
			{
				""buffer_size"": 0,
				""encoding"": ""UTF-8"",
				""line_ending"": ""Windows""
			}
		}
	],
	""build_system"": ""Packages/C++/" + Configs.BuildSysName + @".sublime-build"",
	""build_system_choices"":
	[
		[
			[
				[
					""Packages/C++/" + Configs.BuildSysName + @".sublime-build"",
					""""
				],
				[
					""Packages/C++/" + Configs.BuildSysName + @".sublime-build"",
					""Run""
				]
			],
			[
				""Packages/C++/" + Configs.BuildSysName + @".sublime-build"",
				""Run""
			]
		]
	],
	""build_varint"": """",
	""command_palette"":
	{
		""height"": 0.0,
		""last_filter"": """",
		""selected_items"":
		[
		],
		""width"": 0.0
	},
	""console"":
	{
		""height"": 0.0,
		""history"":
		[
		]
	},
	""distraction_free"":
	{
		""menu_visible"": true,
		""show_minimap"": false,
		""show_open_files"": false,
		""show_tabs"": false,
		""side_bar_visible"": false,
		""status_bar_visible"": false
	},
	""file_history"":
	[
		
	],
	""find"":
	{
		""height"": 0.0
	},
	""find_in_files"":
	{
		""height"": 0.0,
		""where_history"":
		[
		]
	},
	""find_state"":
	{
		""case_sensitive"": false,
		""find_history"":
		[
		],
		""highlight"": true,
		""in_selection"": false,
		""preserve_case"": false,
		""regex"": false,
		""replace_history"":
		[
		],
		""reverse"": false,
		""show_context"": true,
		""use_buffer2"": true,
		""whole_word"": false,
		""wrap"": true
	},
	""groups"":
	[
		{
			""selected"": 0,
			""sheets"":
			[
				{
					""buffer"": 0,
					""file"": """ + SourceName + @""",
					""semi_transient"": false,
					""settings"":
					{
						""buffer_size"": " + buffer.ToString() + @",
						""regions"":
						{
						},
						""selection"":
						[
							[
								138,
								138
							]
						],
						""settings"":
						{
							""syntax"": ""Packages/C++/C++.sublime-syntax"",
							""tab_size"": 4,
							""translate_tabs_to_spaces"": true
						},
						""translation.x"": 0.0,
						""translation.y"": 0.0,
						""zoom_level"": 1.0
					},
					""stack_index"": 2,
					""type"": ""text""
				}
			]
		},
		{
			""selected"": 0,
			""sheets"":
			[
				{
					""buffer"": 1,
					""file"": """ + InputFName + @""",
					""semi_transient"": false,
					""settings"":
					{
						""buffer_size"": 0,
						""regions"":
						{
						},
						""selection"":
						[
							[
								0,
								0
							]
						],
						""settings"":
						{
							""syntax"": ""Packages/Text/Plain text.tmLanguage""
						},
						""translation.x"": 0.0,
						""translation.y"": 0.0,
						""zoom_level"": 1.0
					},
					""stack_index"": 1,
					""type"": ""text""
				}
			]
		},
		{
			""selected"": 0,
			""sheets"":
			[
				{
					""buffer"": 2,
					""file"": """ + OutputFName + @""",
					""semi_transient"": false,
					""settings"":
					{
						""buffer_size"": 0,
						""regions"":
						{
						},
						""selection"":
						[
							[
								0,
								0
							]
						],
						""settings"":
						{
							""syntax"": ""Packages/Text/Plain text.tmLanguage""
						},
						""translation.x"": 0.0,
						""translation.y"": 0.0,
						""zoom_level"": 1.0
					},
					""stack_index"": 0,
					""type"": ""text""
				}
			]
		}
	],
	""incremental_find"":
	{
		""height"": 0.0
	},
	""input"":
	{
		""height"": 0.0
	},
	""layout"":
	{
		""cells"":
		[
			[
				0,
				0,
				1,
				2
			],
			[
				1,
				0,
				2,
				1
			],
			[
				1,
				1,
				2,
				2
			]
		],
		""cols"":
		[
			0.0,
			0.5,
			1.0
		],
		""rows"":
		[
			0.0,
			0.5,
			1.0
		]
	},
	""menu_visible"": true,
	""output.exec"":
	{
		""height"": 47.0
	},
	""output.find_results"":
	{
		""height"": 0.0
	},
	""pinned_build_system"": ""Packages/User/MyCPP14BS.sublime-build"",
	""project"": """ + ProjectName + @".sublime-project"",
	""replace"":
	{
		""height"": 0.0
	},
	""save_all_on_build"": true,
	""select_file"":
	{
		""height"": 0.0,
		""last_filter"": """",
		""selected_items"":
		[
		],
		""width"": 0.0
	},
	""select_project"":
	{
		""height"": 0.0,
		""last_filter"": """",
		""selected_items"":
		[
		],
		""width"": 0.0
	},
	""select_symbol"":
	{
		""height"": 0.0,
		""last_filter"": """",
		""selected_items"":
		[
		],
		""width"": 0.0
	},
	""selected_group"": 0,
	""settings"":
	{
		""last_automatic_layout"":
		[
			[
				0,
				0,
				1,
				2
			],
			[
				1,
				0,
				2,
				1
			],
			[
				1,
				1,
				2,
				2
			]
		]
	},
	""show_minimap"": true,
	""show_open_files"": false,
	""show_tabs"": true,
	""side_bar_visible"": true,
	""side_bar_width"": 150.0,
	""status_bar_visible"": true,
	""template_settings"":
	{
		""max_columns"": 2
	}
}
";
            File.WriteAllText(Path.Combine(PPath, ProjectName + ".sublime-workspace"), WORKSPACE);
            File.WriteAllText(Path.Combine(PPath, ProjectName + ".sublime-project"), "{\n}");
            return true;
        }

    }
}
