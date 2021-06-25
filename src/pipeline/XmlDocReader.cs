using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Xna.Framework.Content;

/// <summary>
/// Ladybug Pipeline XMLDocument reader
/// </summary>
public class XmlDocReader : ContentTypeReader<XmlDocument>
{
	/// <summary>
	/// Reads an XML File and produces an XMLDocument
	/// </summary>
	/// <param name="input"></param>
	/// <param name="existingInstance"></param>
	/// <returns></returns>
	protected override XmlDocument Read(ContentReader input, XmlDocument existingInstance)
	{
		var s = input.ReadString();
		var serializer = new XmlSerializer(typeof(XmlDocument));
		XmlDocument res;
		using (var reader = new StringReader(s))
		{
			res = (XmlDocument)serializer.Deserialize(reader);
		}

		return res;
	}
}

