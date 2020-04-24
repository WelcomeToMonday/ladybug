namespace Ladybug.Core.TileMap
{
	public class TileLayer
	{
		public TileLayer()
		{

		}
		public TileLayer(string data, int width, int height)
		{
			SetData(data, width, height);
		}
		
		public int[,] Data { get; private set; }

		public void SetData(string data, int width, int height)
		{
			Data = new int[width, height];
			var dataList = data.Trim('\n').Split(',');
			int dataIndex = 0;
			for (var row = 0; row < height; row++)
			{
				for (var col = 0; col < width; col++)
				{
					int.TryParse(dataList[dataIndex], out Data[col, row]);
					dataIndex++;
				}
			}
		}
	}
}