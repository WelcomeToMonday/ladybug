using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Ladybug.TileMap
{
	public class TileMap
	{
		public enum TileOrientation {ORTHOGRAPHIC, ISOMETRIC}

		protected GraphicsDevice graphicsDevice;
		private string _filePath;

		private ContentManager _contentManager;

		private List<IDrawableLayer> _layers;

		private List<TileSet> _tileSets = new List<TileSet>();

		public TileMap(string filePath, ContentManager contentManager, GraphicsDevice graphicsDevice)
		{
			_contentManager = contentManager;
			this.graphicsDevice = graphicsDevice;
			_filePath = filePath;
			var data = _contentManager.Load<XmlDocument>(filePath);

			using (XmlReader xReader = data.GetReader())
			{
				ReadXml(xReader);
			}
		}

		public TileOrientation Orientation {get; set;} = TileOrientation.ORTHOGRAPHIC;

		public ContentManager Content { get => _contentManager; }

		public Texture2D MapTexture { get; private set; }

		public int Width { get; private set; }

		public int Height { get; private set; }

		public int TileWidth { get; private set; }

		public int TileHeight { get; private set; }

		public void ReadXml(XmlReader reader)
		{
			List<IDrawableLayer> layers = new List<IDrawableLayer>();

			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					switch (reader.Name)
					{
						case "map":
							reader.MoveToAttribute("width");
							if (int.TryParse(reader.Value, out int width)) Width = width;

							reader.MoveToAttribute("height");
							if (int.TryParse(reader.Value, out int height)) Height = height;

							reader.MoveToAttribute("tilewidth");
							if (int.TryParse(reader.Value, out int tileWidth)) TileWidth = tileWidth;

							reader.MoveToAttribute("tileheight");
							if (int.TryParse(reader.Value, out int tileHeight)) TileHeight = tileHeight;
							break;

						case "tileset":
							reader.MoveToAttribute("source");
							string tilesetPath = Path.ChangeExtension(Path.Combine(Path.GetDirectoryName(_filePath), reader.Value), null);
							var tileSet = new TileSet(tilesetPath, _contentManager, graphicsDevice);

							reader.MoveToAttribute("firstgid");
							int gid = 1;
							int.TryParse(reader.Value, out gid);

							if (gid > 0) tileSet.FirstGID = gid - 1;

							_tileSets.Add(tileSet);
							break;

						case "imagelayer":
							ImageLayer iLayer = new ImageLayer();
							layers.Add(iLayer);

							using (var subReader = reader.ReadSubtree())
							{
								while (subReader.Read())
								{
									if (subReader.NodeType == XmlNodeType.Element)
									{
										switch (reader.Name)
										{
											case "image":
												reader.MoveToAttribute("source");
												var imageSource = Path.ChangeExtension(Path.Combine(Path.GetDirectoryName(_filePath), reader.Value), null);

												reader.MoveToAttribute("width");
												int.TryParse(reader.Value, out int imageWidth);

												reader.MoveToAttribute("height");
												int.TryParse(reader.Value, out int imageHeight);

												var imageOffsetX = 0;
												var imageOffsetY = 0;

												if (reader.MoveToAttribute("offsetx"))
												{
													int.TryParse(reader.Value, out imageOffsetX);
												}

												if (reader.MoveToAttribute("offsety"))
												{
													int.TryParse(reader.Value, out imageOffsetY);
												}

												iLayer.SetData(imageSource, imageWidth, imageHeight, imageOffsetX, imageOffsetY);
												break;
										}
									}
								}
							}
							break;

						case "layer":
							TileLayer layer = new TileLayer();
							layers.Add(layer);

							using (var subReader = reader.ReadSubtree())
							{
								while (subReader.Read())
								{
									if (subReader.NodeType == XmlNodeType.Element)
									{
										switch (reader.Name)
										{
											case "data":
												reader.MoveToContent();
												layer.SetData(reader.ReadInnerXml(), Width, Height);
												break;
											case "properties":
												break;
										}
									}
								}
							}
							break;

						case "property":
							reader.MoveToAttribute("name");
							var propertyName = reader.Value;

							reader.MoveToAttribute("value");
							var propertyValue = reader.Value;

							HandleMapProperty(propertyName, propertyValue);
							break;

						case "object":
							reader.MoveToAttribute("type");
							var objectType = reader.Value;

							reader.MoveToAttribute("name");
							var objectName = reader.Value;

							float objectXPos, objectYPos, objectWidth, objectHeight;

							reader.MoveToAttribute("x");
							float.TryParse(reader.Value, out objectXPos);

							reader.MoveToAttribute("y");
							float.TryParse(reader.Value, out objectYPos);

							reader.MoveToAttribute("width");
							float.TryParse(reader.Value, out objectWidth);

							reader.MoveToAttribute("height");
							float.TryParse(reader.Value, out objectHeight);

							reader.MoveToElement();
							var props = new Dictionary<string, string>();
							using (var mSubReader = reader.ReadSubtree())
							{
								while (mSubReader.Read())
								{
									if (mSubReader.NodeType == XmlNodeType.Element)
									{
										switch (mSubReader.Name)
										{
											case "property":
												mSubReader.MoveToAttribute("name");
												var propName = mSubReader.Value;

												mSubReader.MoveToAttribute("value");
												var propValue = mSubReader.Value;

												// If the node has inner HTML, this should
												// replace propValue with the content. Otherwise,
												// it should leave propValue at its current value (somehow!)
												mSubReader.ReadInnerXml();
												propValue = mSubReader.Value;

												props[propName] = propValue;
												break;
										}
									}
								}
							}
							BuildMapObject(objectName, objectType, new Rectangle((int)objectXPos, (int)objectYPos, (int)objectWidth, (int)objectHeight), props);
							break;
					}
				}
			}
			_layers = layers;
		}

		public virtual void BuildMapObject(string name, string type, Rectangle bounds, Dictionary<string, string> properties)
		{
			// to be overridden in derived classes
		}

		public virtual void HandleMapProperty(string name, string value)
		{
			// to be overridden in derived classes
		}

		private Vector2 GetMapBounds()
		{
			var res = Vector2.Zero;
			switch (Orientation)
			{
				default:
				case TileOrientation.ORTHOGRAPHIC:
					res = new Vector2(
						Width * TileWidth,
						Height * TileHeight
					);
					break;
				case TileOrientation.ISOMETRIC:
					var side = Height + Width;
					res = new Vector2(
						(int)(side * (TileWidth / 2)),
						(int)(side * (TileHeight / 2))
					);	
					break;
			}

			return res;
		}

		public void BuildMapTexture()
		{
			var mapBounds = GetMapBounds();
			RenderTarget2D target2D = new RenderTarget2D
			(
				graphicsDevice,
				(int)mapBounds.X,
				(int)mapBounds.Y,
				false,
				graphicsDevice.PresentationParameters.BackBufferFormat,
				DepthFormat.Depth24
			);

			graphicsDevice.SetRenderTarget(target2D);
			graphicsDevice.Clear(Color.Transparent);

			SpriteBatch sb = new SpriteBatch(graphicsDevice);

			sb.Begin();

			foreach (var layer in _layers)
			{
				layer.Draw(this, sb);
			}

			sb.End();

			Texture2D fullTexture = new Texture2D(graphicsDevice, (int)mapBounds.X, (int)mapBounds.Y);

			Color[] texdata = new Color[fullTexture.Width * fullTexture.Height];

			target2D.GetData(texdata);
			fullTexture.SetData(texdata);

			graphicsDevice.SetRenderTarget(null);
			graphicsDevice.Clear(Color.Black);

			MapTexture = fullTexture;
		}

		public Vector2 CoordIso2Ortho(Vector2 isoCoord)
		{
			// Tiled is intersting in that the "origin" of an iso tile is at its foot and not its top.
			// Therefore, we offset the Y by tileheight so that cooordinates show in-game where they 
			// show in the Tiled editor.

			return new Vector2(Height * (TileWidth / 2), TileHeight) // anchor
			+ new Vector2(-isoCoord.Y, isoCoord.Y / 2)
			+ new Vector2(isoCoord.X, isoCoord.X / 2);
		}

		public TileSet FindTileSet(int id)
		{
			TileSet res = null;
			foreach (var ts in _tileSets)
			{
				if (id >= ts.TileRange.X && id <= ts.TileRange.Y) res = ts;
			}
			return res;
		}
	}
}