namespace Com.ValuePlus.FileAttach.Properties
{
    using System;
    using System.CodeDom.Compiler;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [CompilerGenerated, GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings) SettingsBase.Synchronized(new Settings()));

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [ApplicationScopedSetting, DebuggerNonUserCode, SpecialSetting(SpecialSetting.WebServiceUrl), DefaultSettingValue("http://localhost/HRSystem/service/Upload.asmx")]
        public string HRSystem_FileAttach_localhost_Upload
        {
            get
            {
                return (string)this["HRSystem_FileAttach_localhost_Upload"];
            }
        }
    }
}

