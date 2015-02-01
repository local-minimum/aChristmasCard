using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace PointClick {
	

	public static class IExtender {

		public static IEnumerable<TSource[]> Window<TSource>(
			this IEnumerable<TSource> source)
		{
			return source.Window(2);
		}
		
		public static IEnumerable<TSource[]> Window<TSource>(
			this IEnumerable<TSource> source, int size)
		{
			if (size <= 0)
				throw new ArgumentOutOfRangeException("size");
			
			return source.Skip(size).WindowHelper(size, source.Take(size));
		}
		
		private static IEnumerable<TSource[]> WindowHelper<TSource>(
			this IEnumerable<TSource> source, int size, IEnumerable<TSource> init)
		{
			Queue<TSource> q = new Queue<TSource>(init);
			
			yield return q.ToArray();
			
			foreach (var value in source)
			{
				q.Dequeue();
				q.Enqueue(value);
				yield return q.ToArray();
			}
		}

	}

}
