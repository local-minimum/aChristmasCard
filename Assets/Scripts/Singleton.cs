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

				if (res.Length == 0)
					Debug.LogError(string.Format(
						"There must be an instance of {0} in the scene", typeof(T).FullName));
				else
					instance = res[0];


			}
			return instance;
		}
	}
}
