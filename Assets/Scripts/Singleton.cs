using UnityEngine;
using System.Collections.Generic;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	protected static T instance;

	public static T Instance
	{
		get {
			if (instance == null) 
			{
				T[] res = FindObjectsOfType<T>();

				if (res.Length == 1)
					instance = res[0];
				else {
					Debug.LogError(
						string.Format(
						"Exactly 1 instance of {0} required and allowed, found {1}.",
						typeof(T), res.Length));
					foreach (T inst in res)
						Debug.LogError(string.Format(
							"Instance of {0} on {1}",
							typeof(T), inst.gameObject));

				}
			}
			return instance;
		}
	}
}
