﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace App.Cohab.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("App.Cohab.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to --
        ///SELECT 
        ///	Sistema_Nome,
        ///	Sistema_Sigla
        ///	FROM 
        ///		Sistema AS si
        ///	ORDER BY 
        ///		Sistema_Sigla ASC;
        ///--.
        /// </summary>
        internal static string SistemaListar {
            get {
                return ResourceManager.GetString("SistemaListar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --
        ///SELECT 
        ///	Sistema_Nome,
        ///	Sistema_Sigla
        ///	FROM 
        ///		Sistema AS si
        ///		LEFT JOIN UsuarioSistema AS us 
        ///			ON si.Sistema_Sigla = us.UsuarioSistema_SistemaSigla
        ///	WHERE 
        ///		us.UsuarioSistema_UsuarioLogin = @Login
        ///	ORDER BY 
        ///		Sistema_Sigla ASC;
        ///--.
        /// </summary>
        internal static string SistemaListarPorUsuario {
            get {
                return ResourceManager.GetString("SistemaListarPorUsuario", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --
        ///SELECT 
        ///	Sistema_Nome,
        ///	Sistema_Sigla
        ///	FROM 
        ///		Sistema AS si
        ///	WHERE 
        ///		(Sistema_Proprio = @proprio OR (@proprio = 0 OR @proprio IS NULL))
        ///	ORDER BY 
        ///		Sistema_Sigla ASC;
        ///--.
        /// </summary>
        internal static string SistemaListarProprio {
            get {
                return ResourceManager.GetString("SistemaListarProprio", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --
        ///SELECT 
        ///	Usuario_Login,
        ///	Usuario_Nome,
        ///	Usuario_Matricula,
        ///	Usuario_Ramal,
        ///	Usuario_Email,
        ///	Usuario_Chefia,
        ///	Usuario_Ativo,
        ///	Usuario_Visivel,
        ///	Usuario_Notes
        ///FROM Usuario AS us WITH (NOLOCK) 
        ///WHERE 1 = 1 
        ///	AND (Usuario_Ativo = @Ativo 
        ///		OR (@Ativo = 0 OR @Ativo IS NULL))
        ///	AND (Usuario_Login = @Login 
        ///		OR (@Login = &apos;&apos; OR @Login IS NULL))
        ///	AND (Usuario_Nome LIKE &apos;%&apos;+@Nome+&apos;%&apos; 
        ///		OR (@Nome = &apos;&apos; OR @Nome IS NULL))
        ///ORDER BY 
        ///	Usuario_Nome ASC;
        ///--.
        /// </summary>
        internal static string UsuarioListar {
            get {
                return ResourceManager.GetString("UsuarioListar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --
        ///SELECT 
        ///	U.Usuario_Matricula, 
        ///	U.Usuario_Nome, 
        ///	U.Usuario_Login, 
        ///	U.Usuario_Email, 
        ///	U.Usuario_Notes  
        ///FROM [vw_lotacaodp] L WITH (nolock) 
        ///	INNER JOIN Usuario U WITH (nolock) ON U.Usuario_Login = L.Usuario_Login 
        ///WHERE 1 = 1 
        ///	AND (COALESCE(@setor, &apos;&apos;) = &apos;&apos; OR l.setor_sigla = @setor OR l.Departamento_Sigla = @setor) 
        ///	AND (U.Usuario_Ativo = @ativos) 
        ///ORDER BY u.Usuario_Nome ASC;
        ///--.
        /// </summary>
        internal static string UsuarioListarPorSetor {
            get {
                return ResourceManager.GetString("UsuarioListarPorSetor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --
        ///UPDATE 
        ///	UsuarioSistema 
        ///	SET 
        ///		UsuarioSistema_Senha = @Senha,
        ///		UsuarioSistema_Validade = @Validade
        ///	WHERE 
        ///		UsuarioSistema_UsuarioLogin = @Login
        ///		AND UsuarioSistema_SistemaSigla = @Sistema;
        ///--.
        /// </summary>
        internal static string UsuarioTrocarSenha {
            get {
                return ResourceManager.GetString("UsuarioTrocarSenha", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --
        ///SELECT 
        ///	Usuario_Login
        ///	FROM 
        ///		Usuario AS us
        ///		LEFT JOIN UsuarioSistema AS usi 
        ///			ON us.Usuario_Login = usi.UsuarioSistema_UsuarioLogin
        ///	WHERE 
        ///		us.Usuario_Login = @Login
        ///		AND usi.UsuarioSistema_SistemaSigla = @Sistema
        ///	ORDER BY 
        ///		Usuario_Login ASC;
        ///--.
        /// </summary>
        internal static string UsuarioVerificarAcesso {
            get {
                return ResourceManager.GetString("UsuarioVerificarAcesso", resourceCulture);
            }
        }
    }
}
