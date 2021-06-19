using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.Xna.Framework.Content;

public class XmlDocReader : ContentTypeReader<XmlDocument>
{
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

