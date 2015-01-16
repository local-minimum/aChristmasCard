using System.Collections.Generic;


namespace FunOps {

	/// <summary>
	/// Array extender extends array functionality.
	/// For all methods, no changes are ever made in place.
	/// Extensions methods work well chained.
	/// </summary>
	public static class ArrayExtender {

		/// <summary>
		/// Conjugates the array with an element (adds it last)
		/// </summary>
		/// <param name="arr">The Array</param>
		/// <param name="item">Item to extend the array with</param>
		/// <typeparam name="T">The type</typeparam>
		public static T[] Conj<T>(this T[] arr, T item) {
			T[] ret = new T[arr.Length + 1];
			arr.CopyTo(ret, 0);
			ret[arr.Length] = item;
			return ret;
		}

		/// <summary>
		/// Returns an array where the item is the first element followed by present array
		/// </summary>
		/// <param name="arr">The array</param>
		/// <param name="item">Item</param>
		/// <typeparam name="T">The type</typeparam>
		public static T[] Cons<T>(this T[] arr, T item) {
			return item.Cons(arr);
		}

		/// <summary>
		/// Concat the array with another
		/// </summary>
		/// <param name="arr">The current array</param>
		/// <param name="arr2">The other array</param>
		/// <typeparam name="T">The type</typeparam>
		public static T[] Concat<T>(this T[] arr, T[] arr2) {
			T[] ret = new T[arr.Length + arr2.Length];
			arr.CopyTo(ret, 0);
			arr2.CopyTo(ret, arr.Length);
			return ret;
		}

		/// <summary>
		/// First element
		/// </summary>
		/// <param name="arr">Array</param>
		/// <typeparam name="T">The type</typeparam>
		public static T First<T>(this T[] arr) {
			return arr[0];
		}

		/// <summary>
		/// Last element of the array
		/// </summary>
		/// <param name="arr">The array</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Last<T>(this T[] arr) {
			return arr[arr.Length - 1];
		}

		/// <summary>
		/// Subsection of the array.
		/// </summary>
		/// <returns>The array.</returns>
		/// <param name="arr">Array</param>
		/// <param name="start">Start index</param>
		/// <param name="length">Length</param>
		/// <typeparam name="T">The type</typeparam>
		public static T[] SubArray<T>(this T[] arr, int start, int length){
			T[] ret = new T[length];
			System.Array.Copy(arr, start, ret, 0, length);
			return ret;
		}

		/// <summary>
		/// Rest the array excluding first position.
		/// </summary>
		/// <param name="arr">Array</param>
		/// <typeparam name="T">The type</typeparam>
		public static T[] Rest<T>(this T[] arr) {
			return arr.SubArray(1, arr.Length - 1);
		}

		/// <summary>
		/// The array after popping one element
		/// </summary>
		/// <returns>The rest</returns>
		/// <param name="arr">Array</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T[] PopRest<T>(this T[] arr) {
			return arr.NFirst(-1);
		}

		/// <summary>
		/// Reaturns the n first elements of the present array.
		/// If n is negative, all but the n last elements. <code>[1, 2, 3, 4, 5].NFirst(-2)</code> returns <code>[1, 2, 3]</code>
		/// </summary>
		/// <returns>The first.</returns>
		/// <param name="arr">Array</param>
		/// <param name="n">Number of elements.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T[] NFirst<T>(this T[] arr, int n) {
			if (n < 0)
				n += arr.Length;

			return arr.SubArray(0, n);
		}

	}

	/// <summary>
	/// Base type extensions that relates to quick array construction
	/// </summary>
	public static class ElementExtender {

		/// <summary>
		/// Cons the specified item and arr.
		/// Item becomes first element in resulting array.
		/// </summary>
		/// <param name="item">Item</param>
		/// <param name="arr">Array</param>
		/// <typeparam name="T">The type</typeparam>
		public static T[] Cons<T> (this T item, T[] arr) {
			T[] ret = new T[arr.Length + 1];
			ret[0] = item;
			arr.CopyTo(ret, 1);
			return ret;
		}

		/// <summary>
		/// Pushes the item to the last position of array
		/// </summary>
		/// <returns>The new array</returns>
		/// <param name="item">Item</param>
		/// <param name="arr">Array</param>
		/// <typeparam name="T">The type</typeparam>
		public static T[] PushTo<T> (this T item, params T[] arr) {
			return arr.Conj(item);
		}

	}
}