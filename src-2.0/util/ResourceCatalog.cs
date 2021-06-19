using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug
{
	public class ResourceCatalog
	{
		public Dictionary<Type, Dictionary<string, object>> Catalog { get; private set; } = new Dictionary<Type, Dictionary<string, object>>();

		private ContentManager Content;

		public ResourceCatalog(ContentManager contentManager)
		{
			Content = contentManager;
		}

		public ContentManager ContentManager {get => Content;}

		public bool ResourceExists<T>(string identifier)
		{
			bool res = true;
			
			if (!Catalog.ContainsKey(typeof(T)))
			{
				res = false;
			}
			else
			{
				res = Catalog[typeof(T)].ContainsKey(identifier);
			}

			return res;
		}

		public T LoadResource<T>(string identifier, string source)
		{
			var res = default(T);
			if (!Catalog.ContainsKey(typeof(T)))
			{
				Catalog[typeof(T)] = new Dictionary<string, object>();
			}

			if (!Catalog[typeof(T)].ContainsKey(identifier))
			{
				res = Content.Load<T>(source);

				Catalog[typeof(T)][identifier] = res as object;
			}
			else
			{
				res = GetResource<T>(identifier);
			}

			return res;
		}

		public T GetResource<T>(string name)
		{
			T res = default(T);

			try
			{
				res = (T)Catalog[typeof(T)][name];
			}
			catch (KeyNotFoundException)
			{
				res = default(T);
			}

			return res;
		}

		public void SaveResource<T>(string identifier, T resource)
		{
			if (!Catalog.ContainsKey(typeof(T)))
			{
				Catalog[typeof(T)] = new Dictionary<string, object>();
			}

			if (!Catalog[typeof(T)].ContainsKey(identifier))
			{
				Catalog[typeof(T)][identifier] = resource;
			}
		}
	}
}