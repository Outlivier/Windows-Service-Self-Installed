﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceSelfInstaller {
    using System;
    
    
    /// <summary>
    ///   Une classe de ressource fortement typée destinée, entre autres, à la consultation des chaînes localisées.
    /// </summary>
    // Cette classe a été générée automatiquement par la classe StronglyTypedResourceBuilder
    // à l'aide d'un outil, tel que ResGen ou Visual Studio.
    // Pour ajouter ou supprimer un membre, modifiez votre fichier .ResX, puis réexécutez ResGen
    // avec l'option /str ou régénérez votre projet VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Locale {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Locale() {
        }
        
        /// <summary>
        ///   Retourne l'instance ResourceManager mise en cache utilisée par cette classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ServiceSelfInstaller.Locale", typeof(Locale).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Remplace la propriété CurrentUICulture du thread actuel pour toutes
        ///   les recherches de ressources à l'aide de cette classe de ressource fortement typée.
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
        ///   Recherche une chaîne localisée semblable à La longueur du nom de service ne doit pas excéder {0} caractères..
        /// </summary>
        internal static string InstallArgs_Constructor_NameCheck {
            get {
                return ResourceManager.GetString("InstallArgs_Constructor_NameCheck", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Les arguments {0} de l&apos;installation sont nul. Vérifier que le constructeur approprié de la classe {1} a été appelé. .
        /// </summary>
        internal static string ServiceInstallerComponent_CreateProcessInstaller_ArgsNullEx {
            get {
                return ResourceManager.GetString("ServiceInstallerComponent_CreateProcessInstaller_ArgsNullEx", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à L&apos;outil Sc.exe s&apos;est terminé avec le code d&apos;erreur {0}..
        /// </summary>
        internal static string ServiceInstallerComponent_StartSC_Error {
            get {
                return ResourceManager.GetString("ServiceInstallerComponent_StartSC_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Démarrage du service....
        /// </summary>
        internal static string ServiceSelfInstaller_OnAfterInstall_Start {
            get {
                return ResourceManager.GetString("ServiceSelfInstaller_OnAfterInstall_Start", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Recherche une chaîne localisée semblable à Arrêt du service....
        /// </summary>
        internal static string ServiceSelfInstaller_OnBeforeUninstall_Stop {
            get {
                return ResourceManager.GetString("ServiceSelfInstaller_OnBeforeUninstall_Stop", resourceCulture);
            }
        }
    }
}
