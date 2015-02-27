using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	private static string managerName = "Managers";

	protected static T instance;

	public static T Instance
	{
		get {
			if (instance == null) 
			{
				T[] res = FindObjectsOfType<T>();

				if (res.Length == 0) {
#if UNITY_EDITOR
					GameObject manager = GetOrCreateManager();
					instance = AddInstanceToManager(manager);

#endif
				} else {
					instance = res[0];
				}

			}
			return instance;
		}
	}

	static T AddInstanceToManager(GameObject manager) {
		return (T) manager.AddComponent<T>();
	}

	static GameObject GetOrCreateManager() {
		foreach (GameObject go in SceneRoots()) {
			if (go.name == managerName)
				return go;
		}

		return CreateManager();
	}

	static GameObject CreateManager() {
		GameObject manager = new GameObject();
		manager.name = managerName;
		return manager;
	}

	static IEnumerable<GameObject> SceneRoots()
	{
		HierarchyProperty prop = new HierarchyProperty(HierarchyType.GameObjects);
		int[] expanded = new int[0];
		while (prop.Next(expanded)) {
			yield return prop.pptrValue as GameObject;
		}
	}
}
