using System;
using System.IO;
using System.Xml;

using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

[ContentTypeWriter]
public class XmlDocWriter : ContentTypeWriter<XmlDocument>
{
	protected override void Write(ContentWriter output, XmlDocument value)
	{
		using (var stringWriter = new StringWriter())
		{
			using (var xmlTextWriter = XmlWriter.Create(stringWriter))
			{
				value.WriteTo(xmlTextWriter);
				xmlTextWriter.Flush();
				output.Write(stringWriter.GetStringBuilder().ToString());
			}
		}
	}

	public override string GetRuntimeType(TargetPlatform targetPlatform)
	{
		return typeof(XmlDocument).AssemblyQualifiedName;
	}

	public override string GetRuntimeReader(TargetPlatform targetPlatform)
	{
		return "XmlDocReader, ladybug";
	}
}