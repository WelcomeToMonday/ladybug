using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Ladybug.Graphics;

namespace Ladybug.Core.TileMap
{
	public class TileSet : IXmlSerializable
	{
		//public Vector2 TileDimensions;
		private int _tileWidth;
		private int _tileHeight;
		private int _columnCount;
		private int _rowCount;
		private Texture2D _sourceImage;
		private List<Texture2D> _tileTextures = new List<Texture2D>();
		private string _filePath;
		private ContentManager _contentManager;
		private GraphicsDevice _graphicsDevice;

		private int m_tileCount;

		public TileSet(string filePath, ContentManager contentManager, GraphicsDevice graphicsDevice)
		{
			_filePath = filePath;
			var fullFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, contentManager.RootDirectory, filePath);
			_contentManager = contentManager;
			_graphicsDevice = graphicsDevice;

			using (XmlReader xReader = XmlReader.Create(fullFilePath))
			{
				ReadXml(xReader);
			}
			BuildSpriteList();
		}

		public int TileCount { 
			get => m_tileCount; 
			private set => m_tileCount = value; 
			}

		public int FirstGID { get; set; } = 0;

		public Vector2 TileRange { get => new Vector2(FirstGID, (TileCount + FirstGID) - 1);}

		public Texture2D this[int i]
		{
			get => _tileTextures[i];
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

							reader.MoveToAttribute("columns");
							int.TryParse(reader.Value, out _columnCount);

							_rowCount = m_tileCount / _columnCount;
							break;

						case "image":
							reader.MoveToAttribute("source");
							var sourceImagePath = Path.Combine(Path.GetDirectoryName(_filePath),Path.GetFileNameWithoutExtension(reader.Value));
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
					var sourceRectangle = new Rectangle(col * _tileWidth, row * _tileHeight, _tileWidth, _tileHeight);
					var tex = new Texture2D(_graphicsDevice, _tileWidth, _tileHeight);
					Color[] colorData = new Color[sourceRectangle.Width * sourceRectangle.Height];
					_sourceImage.GetData(0,sourceRectangle,colorData,0,colorData.Length);
					tex.SetData(colorData);
					_tileTextures.Add(tex);
				}
			}
		}
	}
}