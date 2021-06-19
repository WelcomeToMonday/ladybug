namespace Ladybug
{
	public class CircularArray<T>
	{
		private T[] _list;

		private int _start;

		public CircularArray(int capacity = 100)
		{
			_list = new T[capacity];
		}

		public T this[int i]
		{
			get => _list[(_start + i) % _list.Length];
			set => _list[(_start + i) % _list.Length] = value;
		}

		public int Start
		{
			get => _start;
			set => _start = value % _list.Length;
		}

		public int Count { get; set; }

		public int Capacity { get => _list.Length; }

		public void Populate<K>() where K : T, new()
		{
			for (var i = 0; i < Capacity; i++)
			{
				this[i] = new K();
			}
		}

		public T Get()
		{
			T res = default(T);

			if (Count == Capacity)
			{
				res = this[0];
				Start++;
			}
			else
			{
				res = this[Count];
				Count++;
			}

			return res;
		}
	}
}