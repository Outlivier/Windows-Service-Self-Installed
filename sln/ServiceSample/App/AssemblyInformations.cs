using System;
using System.Reflection;

namespace ServiceSample.App
{
	/// <summary>
	/// Informations sur l'assembly comme le nom de produit ou le n° de version.
	/// </summary>
	public class AssemblyInformations
	{
		public AssemblyInformations(Assembly assembly)
		{
			this.Product = GetAttributeInfo<AssemblyProductAttribute>(assembly, a => a?.Product ?? string.Empty);
			this.Company = GetAttributeInfo<AssemblyCompanyAttribute>(assembly, a => a?.Company ?? string.Empty);
			this.Title = GetAttributeInfo<AssemblyTitleAttribute>(assembly, a => a?.Title ?? string.Empty);
			this.Version = assembly.GetName().Version?.ToString() ?? string.Empty;
			this.InformationalVersion = GetAttributeInfo<AssemblyInformationalVersionAttribute>(assembly, a => a?.InformationalVersion ?? string.Empty);
		}

		/// <summary>Récupère la valeur d'un attribut de l'assembly.</summary>
		private static string GetAttributeInfo<T>(Assembly assembly, Func<T, string> propertySelector) where T : Attribute
		{
			T att = null;
			object[] attribs = assembly.GetCustomAttributes(typeof(T), inherit: true);
			if (attribs.Length > 0)
			{
				att = (T)attribs[0];
			}
			return propertySelector.Invoke(att);
		}

		/// <summary>Exemple : "My Commercial Application Name".</summary>
		public string Product { get; }
		/// <summary>Exemple : "MyCompany".</summary>
		public string Company { get; }
		/// <summary>Exemple : "MyApplication Service".</summary>
		public string Title { get; }
		/// <summary>AssemblyVersion, Exemple : "1.26.17.</summary>
		public string Version { get; }
		/// <summary>Exemple : "1.26.17-Xa9d962e".</summary>
		public string InformationalVersion { get; }
	}
}