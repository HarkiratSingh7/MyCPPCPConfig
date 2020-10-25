using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CPPSublimeConfigurator
{
    public class ProgramConfiguration
    {
        public string SourceTemplate { get; set; }

        public string ParentFolder { get; set; }
        
        public string LocationPCHBITSTD { get; set; }

        public string BuildSysName { get; set; }

        public string BuildSysLocation { get; set; }

    }
}
