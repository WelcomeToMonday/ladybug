using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Graphics;

namespace Ladybug.TileMap
{
	public class TileSet : IXmlSerializable
	{
		//public Vector2 TileDimensions;
		private int _tileWidth;
		private int _tileHeight;
		private int _columnCount;
		private int _rowCount;
		private int _spacing = 0;
		private Texture2D _sourceImage;
		//private List<Texture2D> _tileTextures = new List<Texture2D>();
		private List<Sprite> _tileSprites = new List<Sprite>();
		private string _filePath;
		private ContentManager _contentManager;
		private GraphicsDevice _graphicsDevice;

		private int m_tileCount;

		public TileSet(string filePath, ContentManager contentManager, GraphicsDevice graphicsDevice)
		{
			_filePath = filePath;
			//var fullFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, contentManager.RootDirectory, filePath);
			_contentManager = contentManager;
			_graphicsDevice = graphicsDevice;

			var data = _contentManager.Load<XmlDocument>(filePath);

			using (XmlReader xReader = data.GetReader())
			{
				ReadXml(xReader);
			}
			BuildSpriteList();
		}

		public int TileWidth {get => _tileWidth;}

		public int TileHeight{get => _tileHeight;}

		public int TileCount
		{
			get => m_tileCount;
			private set => m_tileCount = value;
		}

		public int FirstGID { get; set; } = 0;

		public Vector2 TileRange { get => new Vector2(FirstGID, (TileCount + FirstGID) - 1); }

		public Sprite this[int i]
		{
			get => _tileSprites[i];
		}

		public void ReadXml(XmlReader reader)
		{
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					switch (reader.Name)
					{
						case "tileset":

							reader.MoveToAttribute("tilewidth");
							int.TryParse(reader.Value, out _tileWidth);

							reader.MoveToAttribute("tileheight");
							int.TryParse(reader.Value, out _tileHeight);

							reader.MoveToAttribute("tilecount");
							int.TryParse(reader.Value, out m_tileCount);

							if (reader.GetAttribute("spacing") != null)
							{
								reader.MoveToAttribute("spacing");
								int.TryParse(reader.Value, out _spacing);
							}

							reader.MoveToAttribute("columns");
							int.TryParse(reader.Value, out _columnCount);

							_rowCount = m_tileCount / _columnCount;
							break;

						case "image":
							reader.MoveToAttribute("source");
							//var sourceImagePath = Path.Combine(Path.GetDirectoryName(_filePath), Path.GetFileNameWithoutExtension(reader.Value));
							var sourceImagePath = Path.ChangeExtension(Path.Combine(Path.GetDirectoryName(_filePath), reader.Value), null);
							_sourceImage = _contentManager.Load<Texture2D>(sourceImagePath);
							break;
					}
				}
			}
		}

		public void WriteXml(XmlWriter writer)
		{

		}

		public System.Xml.Schema.XmlSchema GetSchema()
		{
			return null;
		}

		private void BuildSpriteList()
		{
			for (int row = 0; row < _rowCount; row++)
			{
				for (int col = 0; col < _columnCount; col++)
				{
					var xSpacing = 0;
					var ySpacing = 0;
					if (col != 0)
					{
						xSpacing = _spacing * 1;
					}
					if (row != 0)
					{
						ySpacing = _spacing * 1;
					}
					
					var sourceRectangle = new Rectangle(col * (_tileWidth + xSpacing), row * (_tileHeight + ySpacing), _tileWidth, _tileHeight);
					/*
					var tex = new Texture2D(_graphicsDevice, _tileWidth, _tileHeight);
					Color[] colorData = new Color[sourceRectangle.Width * sourceRectangle.Height];
					_sourceImage.GetData(0, sourceRectangle, colorData, 0, colorData.Length);
					tex.SetData(colorData);
					_tileTextures.Add(tex);
					*/
					var tileSprite = new Sprite(_sourceImage, sourceRectangle);
					_tileSprites.Add(tileSprite);
				}
			}
		}
	}
}