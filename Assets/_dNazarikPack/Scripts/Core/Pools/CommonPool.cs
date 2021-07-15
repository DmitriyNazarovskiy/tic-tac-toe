using System.Collections.Generic;

namespace Core
{
	public class CommonPool<T>
	{
		private readonly Queue<T> _pool;
		public CommonPool() => _pool = new Queue<T>();
		public void AddToPool(T poolObject) => _pool.Enqueue(poolObject);
		public T GetFromPool() => _pool.Count == 0 ? default : _pool.Dequeue();
	}
}
