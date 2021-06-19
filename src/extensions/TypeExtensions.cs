using System;
using System.Reflection;
using System.Linq;

namespace Ladybug
{
	public static class TypeExtensions
	{
		public static Type LocateType(string typeName, string[] assemblies = null)
		{
			Type res = null;
			foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
			{
				Type[] assemblyTypes = (assemblies == null || assemblies.Length < 1)
					?	a.GetTypes()
					: a.GetTypes().Where(s => assemblies.Contains(s.Name)).ToArray();

					if (res != null)
					{
						break;
					}
					else
					{
						res = assemblyTypes.FirstOrDefault(t => t.Name == typeName);
					}
			}
			return res;
		}
	}
}