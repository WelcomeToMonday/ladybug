using System;
using System.Reflection;
using System.Linq;

namespace Ladybug
{
	/// <summary>
	/// Static class containing Type static helper and extension methods
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Attempts to locate a Type by name
		/// </summary>
		/// <param name="typeName">Name of type to locate</param>
		/// <param name="type">Matching type, if found</param>
		/// <param name="assemblies">Assemblies to search for Type in</param>
		/// <returns>True if matching Type found, otherwise False</returns>
		public static bool TryLocateType(string typeName, out Type type, string[] assemblies = null)
		{
			type = null;
			foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
			{
				Type[] assemblyTypes = (assemblies == null || assemblies.Length < 1)
					?	a.GetTypes()
					: a.GetTypes().Where(s => assemblies.Contains(s.Name)).ToArray();

					if (type != null)
					{
						break;
					}
					else
					{
						type = assemblyTypes.FirstOrDefault(t => t.Name == typeName);
					}
			}
			return type == default(Type);
		}
	}
}