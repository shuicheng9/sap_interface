﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.17929
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MiniSqlQuery.Core.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MiniSqlQuery.Core.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///
        ///&lt;DbConnectionDefinitionList xmlns:xsi=&quot;http://www.w3.org/2001/XMLSchema-instance&quot; xmlns:xsd=&quot;http://www.w3.org/2001/XMLSchema&quot;&gt;
        ///	&lt;Definitions&gt;
        ///		&lt;DbConnectionDefinition&gt;
        ///			&lt;Name&gt;Default - MSSQL Master@localhost&lt;/Name&gt;
        ///			&lt;ProviderName&gt;System.Data.SqlClient&lt;/ProviderName&gt;
        ///			&lt;ConnectionString&gt;Server=.; Database=master; Integrated Security=SSPI&lt;/ConnectionString&gt;
        ///		&lt;/DbConnectionDefinition&gt;
        ///		&lt;DbConnectionDefinition&gt;
        ///			&lt;Name&gt;Sample MSSQL Northwind SQL Expres [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        public static string DefaultConnectionDefinitionFile {
            get {
                return ResourceManager.GetString("DefaultConnectionDefinitionFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Mini SQLQuery 的本地化字符串。
        /// </summary>
        public static string FriendlyAppTitle {
            get {
                return ResourceManager.GetString("FriendlyAppTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 MiniSqlQuery 的本地化字符串。
        /// </summary>
        public static string ShortAppTitle {
            get {
                return ResourceManager.GetString("ShortAppTitle", resourceCulture);
            }
        }
    }
}
