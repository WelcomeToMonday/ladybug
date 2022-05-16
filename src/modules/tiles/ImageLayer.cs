using System.Xml;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ladybug.Tiles
{
	/// <summary>
	/// Represents an image that can be drawn onto a tilemap as a layer
	/// </summary>
	public class ImageLayer : TileLayer
	{
		/// <summary>
		/// Create an ImageLayer
		/// </summary>
		public ImageLayer(XmlElement node, TileMap tileMap) : base(node, tileMap)
		{

		}

		/// <summary>
		/// The image texture drawn by this ImageLayer
		/// </summary>
		/// <value></value>
		public Texture2D Texture { get; private set; }

		/// <summary>
		/// The bounds of the image drawn by this ImageLayer
		/// </summary>
		/// <value></value>
		public Rectangle Bounds { get; private set; }

		/// <inheritdoc />
		protected override void Initialize()
		{
			var imageNode = XmlElement.SelectSingleNode("./image") as XmlElement;
			
			var imagePath = Path.ChangeExtension(
				Path.Combine(
					Path.GetDirectoryName(TileMap.Path),
					imageNode.Attributes["source"].Value
					),
					null
				);

			Texture = TileMap.Content.Load<Texture2D>(imagePath);

			int.TryParse(imageNode.Attributes["width"].Value, out int width);
			int.TryParse(imageNode.Attributes["height"].Value, out int height);
			int.TryParse(XmlElement.Attributes["offsetx"]?.Value, out int offsetx);
			int.TryParse(XmlElement.Attributes["offsety"]?.Value, out int offsety);

			Bounds = new Rectangle(offsetx, offsety, width, height);
		}

		/// <summary>
		/// Renders the ImageLayer using the provided SpriteBatch
		/// </summary>
		/// <param name="spriteBatch"></param>
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(
				Texture,
				Bounds,
				null,
				Color.White
			);
		}
	}
}