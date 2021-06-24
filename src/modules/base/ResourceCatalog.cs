using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;

namespace Ladybug
{
	/// <summary>
	/// Ladybug ResourceCatalog instance
	/// </summary>
	public class ResourceCatalog
	{
		private Dictionary<Type, Dictionary<string, object>> _catalog = new Dictionary<Type, Dictionary<string, object>>();

		/// <summary>
		/// Creates a new RsourceCatalog instance
		/// </summary>
		/// <param name="contentManager">ContentManager to be used by the ResourceCatalog when loading content resources</param>
		public ResourceCatalog(ContentManager contentManager)
		{
			ContentManager = contentManager;
		}

		/// <summary>
		/// ResourceCatalog's resident ContentManager
		/// </summary>
		/// <value></value>
		public ContentManager ContentManager { get; private set; }

		/// <summary>
		/// Checks to see if resource exists
		/// </summary>
		/// <param name="identifier">Name of resource</param>
		/// <typeparam name="T">Type of resource</typeparam>
		/// <returns>True if resource exists, False if resource not found.</returns>
		public bool ResourceExists<T>(string identifier)
		{
			bool res = true;

			if (!_catalog.ContainsKey(typeof(T)))
			{
				res = false;
			}
			else
			{
				res = _catalog[typeof(T)].ContainsKey(identifier);
			}

			return res;
		}

		/// <summary>
		/// Loads a resource into the ResourceCatalog
		/// </summary>
		/// <param name="identifier">Name of new resource</param>
		/// <param name="source">Path to resource</param>
		/// <typeparam name="T">Type of resource</typeparam>
		/// <returns>Resource loaded from given path</returns>
		public T LoadResource<T>(string identifier, string source)
		{
			var res = default(T);
			if (!_catalog.ContainsKey(typeof(T)))
			{
				_catalog[typeof(T)] = new Dictionary<string, object>();
			}

			if (!_catalog[typeof(T)].ContainsKey(identifier))
			{
				res = ContentManager.Load<T>(source);

				_catalog[typeof(T)][identifier] = res as object;
			}
			else
			{
				res = GetResource<T>(identifier);
			}

			return res;
		}

		/// <summary>
		/// Gets the given resource from the ResourceCatalog
		/// </summary>
		/// <param name="name">Name of resource</param>
		/// <typeparam name="T">Type of resource</typeparam>
		/// <returns>Requested resource</returns>
		public T GetResource<T>(string name)
		{
			T res = default(T);

			try
			{
				res = (T)_catalog[typeof(T)][name];
			}
			catch (KeyNotFoundException)
			{
				res = default(T);
			}

			return res;
		}

		/// <summary>
		/// Attempts to find the given resource in this ResourceCatalog
		/// </summary>
		/// <param name="name">Name of resource</param>
		/// <param name="resource">Reference to resource, if found</param>
		/// <typeparam name="T">Type of resource</typeparam>
		/// <returns>True if resource is found, False otherwise</returns>
		public bool TryGetResource<T>(string name, out T resource)
		{
			var res = false;
			resource = default(T);
			
			if (ResourceExists<T>(name))
			{
				resource = GetResource<T>(name);
				res = true;
			}

			return res;
		}

		/// <summary>
		/// Saves an already loaded resource to the ResourceCatalog
		/// </summary>
		/// <param name="identifier">Name of resource</param>
		/// <param name="resource">Resource to save</param>
		/// <typeparam name="T">Type of resource</typeparam>
		public void SaveResource<T>(string identifier, T resource)
		{
			if (!_catalog.ContainsKey(typeof(T)))
			{
				_catalog[typeof(T)] = new Dictionary<string, object>();
			}

			if (!_catalog[typeof(T)].ContainsKey(identifier))
			{
				_catalog[typeof(T)][identifier] = resource;
			}
		}
	}
}