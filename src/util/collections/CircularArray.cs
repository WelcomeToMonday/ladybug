namespace Ladybug
{
	/// <summary>
	/// Represents a collection of reusable elements
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class CircularArray<T>
	{
		private T[] _list;

		private int _start;

		/// <summary>
		/// Creates a new CircularArray
		/// </summary>
		/// <param name="capacity">Maximum capacity</param>
		public CircularArray(int capacity = 100)
		{
			_list = new T[capacity];
		}

		/// <summary>
		/// Retrieves the element at the given index
		/// </summary>
		/// <value></value>
		public T this[int i]
		{
			get => _list[(_start + i) % _list.Length];
			set => _list[(_start + i) % _list.Length] = value;
		}

		/// <summary>
		/// Current start position
		/// </summary>
		/// <value></value>
		public int Start
		{
			get => _start;
			set => _start = value % _list.Length;
		}

		/// <summary>
		/// Number of current items
		/// </summary>
		/// <value></value>
		public int Count { get; set; }

		/// <summary>
		/// Maximum capacity
		/// </summary>
		/// <value></value>
		public int Capacity { get => _list.Length; }

		/// <summary>
		/// Populates the collection, filling it
		/// to maximum capacity with new item instances
		/// </summary>
		/// <typeparam name="K"></typeparam>
		public void Populate<K>() where K : T, new()
		{
			for (var i = 0; i < Capacity; i++)
			{
				this[i] = new K();
			}
		}

		/// <summary>
		/// Retrieves the next item in the collection
		/// </summary>
		/// <returns></returns>
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