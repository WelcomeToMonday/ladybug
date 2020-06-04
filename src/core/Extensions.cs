using System.Xml;
using System.IO;

namespace Ladybug.Core
{
	public static class Extensions
	{
		public static XmlReader GetReader(this XmlDocument document) => new XmlNodeReader(document);
	}
}