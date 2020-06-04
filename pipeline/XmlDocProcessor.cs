using Microsoft.Xna.Framework.Content.Pipeline;

using TInput = System.Xml.XmlDocument;
using TOutput = System.Xml.XmlDocument;


[ContentProcessor(DisplayName = "XML Document Processor - Ladybug")]
public class XmlDocProcessor : ContentProcessor<TInput, TOutput>
{
	public override TOutput Process(TInput input, ContentProcessorContext context)
	{
		//return default(TOutput);
		return input;
	}
}
