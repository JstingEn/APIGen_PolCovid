﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace APIGen_PolCovid.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("N:/nonmotdrive/CovidFilePolicy/")]
        public string PathWriteFilePolicyCovid {
            get {
                return ((string)(this["PathWriteFilePolicyCovid"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D:/CovidFilePolicy/")]
        public string PathWriteFilePolicyCovid_Dev {
            get {
                return ((string)(this["PathWriteFilePolicyCovid_Dev"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("N:/nonmotdrive/CovidFilePolicy/")]
        public string PathWriteFilePolicyCovid_Pro {
            get {
                return ((string)(this["PathWriteFilePolicyCovid_Pro"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("N:/nonmotdrive/test/")]
        public string PathWriteFilePolicyCovid_UAT {
            get {
                return ((string)(this["PathWriteFilePolicyCovid_UAT"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://jasper1.smk.co.th:8080/jasperserver/rest_v2/reports/reports/DEV/Nonmotor")]
        public string Url_Js2 {
            get {
                return ((string)(this["Url_Js2"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://jasper3.smk.co.th:8080/jasperserver/rest_v2/reports/reports/DEV/Nonmotor")]
        public string Url_Js3 {
            get {
                return ((string)(this["Url_Js3"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://jasper3.smk.co.th:8080/jasperserver/rest_v2/reports/reports/DEV/Nonmotor")]
        public string Url_JsDev {
            get {
                return ((string)(this["Url_JsDev"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://jasper3.smk.co.th:8080/jasperserver/rest_v2/reports/reports/interactive/No" +
            "nmotor")]
        public string Url_JsPro {
            get {
                return ((string)(this["Url_JsPro"]));
            }
        }
    }
}
