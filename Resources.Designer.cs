﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DelegateAS.SmartResource {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DelegateAS.SmartResource.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Smart Resource.
        /// </summary>
        internal static string DialogBox_DefaultTitle {
            get {
                return ResourceManager.GetString("DialogBox_DefaultTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to checkout item: .
        /// </summary>
        internal static string Main_CheckOut_Failed {
            get {
                return ResourceManager.GetString("Main_CheckOut_Failed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sorry couldn&apos;t find any project properties for this file. Is it part of any of your loaded projects?.
        /// </summary>
        internal static string Main_ReplaceStringWithResource_NoProjectProperties {
            get {
                return ResourceManager.GetString("Main_ReplaceStringWithResource_NoProjectProperties", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No resx file found, sorry.
        /// </summary>
        internal static string Main_ReplaceStringWithResource_NoResXFile {
            get {
                return ResourceManager.GetString("Main_ReplaceStringWithResource_NoResXFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This macro only works with the Text editor windows. 
        ///Right click the file, choose &quot;Open With..&quot;, choose &quot;Source code...&quot;.
        /// </summary>
        internal static string Main_ReplaceStringWithResource_SelectionError {
            get {
                return ResourceManager.GetString("Main_ReplaceStringWithResource_SelectionError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select some text first.
        /// </summary>
        internal static string Main_ReplaceStringWithResource_SelectSomeText {
            get {
                return ResourceManager.GetString("Main_ReplaceStringWithResource_SelectSomeText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter the default replace patterns for the resource replacement.
        /// </summary>
        internal static string Options_OnHelp_Msg {
            get {
                return ResourceManager.GetString("Options_OnHelp_Msg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred: .
        /// </summary>
        internal static string SmartResourcePackage_Exception {
            get {
                return ResourceManager.GetString("SmartResourcePackage_Exception", resourceCulture);
            }
        }
    }
}