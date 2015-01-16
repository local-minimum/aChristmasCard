using System.Collections.Generic;

namespace FunColOps {
	public static class ArrayExtender {

		public static T[] Conj<T>(this T[] arr, T item) {
			T[] ret = new T[arr.Length + 1];
			arr.CopyTo(ret, 0);
			ret[arr.Length] = item;
			return ret;
		}

		public static T[] Concat<T>(this T[] arr, T[] arr2) {
			T[] ret = new T[arr.Length + arr2.Length];
			arr.CopyTo(ret, 0);
			arr2.CopyTo(ret, arr.Length);
			return ret;
		}

		public static T First<T>(this T[] arr) {
			return arr[0];
		}

		public static T[] SubArray<T>(this T[] arr, int start, int length){
			T[] ret = new T[length];
			System.Array.Copy(arr, start, ret, 0, length);
			return ret;
		}

		public static T[] Rest<T>(this T[] arr) {
			return arr.SubArray(1, arr.Length - 1);
		}
	}

	public static class ElementExtender {

		public static T[] Cons<T> (this T item, T[] arr) {
			T[] ret = new T[arr.Length + 1];
			ret[0] = item;
			arr.CopyTo(ret, 1);
			return ret;
		}

		public static T[] Concat<T> (this T item, params T[] arr) {
			return item.Cons(arr);
		}

	}
}