using System.Xml;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using IPath = System.IO.Path;

namespace Ladybug.Tiles
{
	/// <summary>
	/// Describes a TileMap's tile orientation
	/// </summary>
	public enum Orientation
	{
		/// <summary>
		/// Orthogonal tile orientation
		/// </summary>
		Orthogonal,
		/// <summary>
		/// Isometric tile orientation
		/// </summary>
		Isometric
	}

	/// <summary>
	/// Represents an image constructed from layers containing tiles, images, and objects
	/// </summary>
	public class TileMap
	{
		private List<TileSet> _tileSets = new List<TileSet>();

		/// <summary>
		/// Create a TileMap
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="scene"></param>
		public TileMap(string filePath, Scene scene)
		{
			Path = filePath;
			Scene = scene;
			Content = scene.Content;
			GraphicsDevice = scene.Game.GraphicsDevice;

			var xml = Content.Load<XmlDocument>(Path);
			XmlDocument = xml;

			var mapNode = xml.SelectSingleNode("/map") as XmlElement;
			BuildMap(mapNode);
		}

		/// <summary>
		/// The Scene object containing this TileMap
		/// </summary>
		/// <value></value>
		public Scene Scene { get; private set; }

		/// <summary>
		/// The TileMap's content path
		/// </summary>
		/// <value></value>
		public string Path { get; private set; }

		/// <summary>
		/// The XmlDocument which describes the structure of this TileMap
		/// </summary>
		/// <value></value>
		public XmlDocument XmlDocument { get; private set; }

		/// <summary>
		/// The GraphicsDevice responsible for dynamically rendering this TileMap's
		/// textures
		/// </summary>
		/// <value></value>
		public GraphicsDevice GraphicsDevice { get; set; }

		/// <summary>
		/// The ContentManager responsible for loading this TileMap's content
		/// </summary>
		/// <value></value>
		public ContentManager Content { get; set; }

		/// <summary>
		/// The orientation of this TileMap's tiles
		/// </summary>
		/// <value></value>
		public Orientation Orientation { get; set; } = Orientation.Orthogonal;

		/// <summary>
		/// The TileMap's properties as described by its XML Definition
		/// </summary>
		public Dictionary<string, string> Properties { get; private set; } = new Dictionary<string, string>();

		/// <summary>
		/// The height of the TileMap's bounds, in pixels
		/// </summary>
		/// <returns></returns>
		public int Height => int.Parse(Properties["height"]);

		/// <summary>
		/// The width of the TileMap's bounds, in pixels
		/// </summary>
		/// <returns></returns>
		public int Width => int.Parse(Properties["width"]);

		/// <summary>
		/// The height of this TileMap's individual tiles, in pixels
		/// </summary>
		/// <returns></returns>
		public int TileHeight => int.Parse(Properties["tileheight"]);

		/// <summary>
		/// The height width of this TileMap's individual tiles, in pixels
		/// </summary>
		/// <returns></returns>
		public int TileWidth => int.Parse(Properties["tilewidth"]);

		/// <summary>
		/// This TileMap's TileLayer layers
		/// </summary>
		/// <returns></returns>
		public List<TileLayer> Layers = new List<TileLayer>();

		/// <summary>
		/// The TileMap's complete texture
		/// </summary>
		/// <value></value>
		public Texture2D Texture
		{
			get
			{
				if (m_Texture == null)
				{
					m_Texture = BuildTexture();
				}
				return m_Texture;
			}
		}
		private Texture2D m_Texture;

		/// <summary>
		/// Retrieve a tile's source tileset by the tile's ID
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public TileSet FindTileSet(int id)
		{
			TileSet res = null;
			foreach (var ts in _tileSets)
			{
				if (id >= ts.TileRange.X && id <= ts.TileRange.Y)
				{
					res = ts;
					break;
				}
				return res;
			}
			return res;
		}

		/// <summary>
		/// Convert an Isometric Coordinate to an Orthographic Coordinate
		/// </summary>
		/// <param name="isoCoord"></param>
		/// <returns></returns>
		public Vector2 CoordIso2Ortho(Vector2 isoCoord)
		{
			// Tiled is intersting in that the "origin" of an iso tile is at its foot and not its top.
			// Therefore, we offset the Y by tileheight so that coordinates show in-game where they 
			// show in the Tiled editor.

			return new Vector2(Height * (TileWidth / 2), TileHeight) // anchor
			+ new Vector2(-isoCoord.Y, isoCoord.Y / 2)
			+ new Vector2(isoCoord.X, isoCoord.X / 2);
		}

		private void BuildMap(XmlElement mapNode)
		{
			switch (mapNode.Attributes["orientation"].Value)
			{
				default:
				case "orthogonal":
					Orientation = Orientation.Orthogonal;
					break;
				case "isometric":
					Orientation = Orientation.Isometric;
					break;
			}

			foreach (XmlAttribute att in mapNode.Attributes)
			{
				if (!Properties.TryAdd(att.Name, att.Value))
				{
					Properties[att.Name] = att.Value;
				}
			}

			var propNode = mapNode.SelectSingleNode("./properties");
			if (propNode != null)
			{
				foreach (XmlElement prop in propNode.SelectNodes("./property"))
				{
					Properties.TryAdd(prop.Attributes["name"].Value, prop.Attributes["value"].Value);
					BuildCustomProperty(prop);
				}
			}

			var tileSets = mapNode.SelectNodes("./tileset");

			if (tileSets != null)
			{
				foreach (XmlElement tileSet in tileSets)
				{
					BuildTileSet(tileSet);
				}
			}

			var layers = mapNode.SelectNodes("./layer | ./imagelayer | ./objectgroup");

			if (layers != null)
			{
				foreach (XmlElement layer in layers)
				{
					switch (layer.Name)
					{
						case "layer":
							var tl = new TileLayer(layer, this);
							Layers.Add(tl);
							BuildLayer(tl);
							break;
						case "imagelayer":
							var il = new ImageLayer(layer, this);
							Layers.Add(il);
							BuildLayer(il);
							break;
						case "objectgroup":
							var objects = layer.SelectNodes("./object");
							foreach (XmlElement obj in objects)
							{
								BuildObject(layer, obj);
							}
							break;
					}
				}
			}
		}

		/// <summary>
		/// Handle the processing of the TileMap's custom properties
		/// </summary>
		/// <param name="node"></param>
		protected virtual void BuildCustomProperty(XmlElement node)
		{
			// To be overridden by derived classes
		}

		/// <summary>
		/// Handle the process of building a tile layer
		/// </summary>
		/// <param name="layer"></param>
		protected virtual void BuildLayer(TileLayer layer)
		{
			// To be overridden by derived classes
		}

		/// <summary>
		/// Handle the process of building an object
		/// </summary>
		/// <param name="layerNode"></param>
		/// <param name="objectNode"></param>
		protected virtual void BuildObject(XmlElement layerNode, XmlElement objectNode)
		{
			// To be overridden by derived classes
		}

		private void BuildTileSet(XmlElement node)
		{
			var tileSetPath = IPath.ChangeExtension(
				IPath.Combine(
					IPath.GetDirectoryName(Path),
					node.Attributes["source"].Value),
				null
			);

			var tileSet = new TileSet(tileSetPath, this);

			var gid = int.Parse(node.Attributes["firstgid"].Value);

			if (gid > 0)
			{
				tileSet.FirstGID = gid - 1;
			}

			_tileSets.Add(tileSet);
		}

		private Vector2 GetDimensions()
		{
			var res = Vector2.Zero;

			switch (Orientation)
			{
				default:
				case Orientation.Orthogonal:
					res = new Vector2(
						Width * TileWidth,
						Height * TileHeight
					);
					break;
				case Orientation.Isometric:
					var side = Height + Width;
					res = new Vector2(
						(int)(side * (TileWidth / 2)),
						(int)(side * (TileHeight / 2))
					);
					break;
			}

			return res;
		}

		private Texture2D BuildTexture()
		{
			var dimensions = GetDimensions();
			RenderTarget2D target = new RenderTarget2D
			(
				GraphicsDevice,
				(int)dimensions.X,
				(int)dimensions.Y,
				false,
				GraphicsDevice.PresentationParameters.BackBufferFormat,
				DepthFormat.Depth24
			);

			GraphicsDevice.SetRenderTarget(target);
			GraphicsDevice.Clear(Color.Transparent);

			SpriteBatch sb = new SpriteBatch(GraphicsDevice);

			sb.Begin();

			foreach (var layer in Layers)
			{
				layer.Draw(sb);
			}

			sb.End();

			Texture2D fullTexture = new Texture2D(GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);

			Color[] texData = new Color[fullTexture.Width * fullTexture.Height];

			target.GetData(texData);
			fullTexture.SetData(texData);

			GraphicsDevice.SetRenderTarget(null);
			GraphicsDevice.Clear(Color.Black);

			return fullTexture;
		}
	}
}