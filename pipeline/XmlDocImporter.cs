using System.Xml;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;

using TImport = System.Xml.XmlDocument;


[ContentImporter(".xml", DisplayName = "XMLDoc Importer - Ladybug", DefaultProcessor = "XmlDocProcessor")]
public class XmlDocImporter : ContentImporter<TImport>
{
	public override TImport Import(string filename, ContentImporterContext context)
	{
		var doc = new XmlDocument();
		using (var streamReader = new StreamReader(filename))
		{
			doc.Load(streamReader);
		}
		return doc;
	}
}

