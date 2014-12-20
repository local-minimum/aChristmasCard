using UnityEngine;
using System.Collections;
using System.Linq;

public class DropPosition : MonoBehaviour {

	public string[] allowPocketableTags;

	void Start() {
		if (allowPocketableTags == null) {
			allowPocketableTags = FindObjectsOfType<Pocketable>().Select(p => p.tag).Distinct().ToArray();
		}
	}

	public bool CanTake(GameObject thing) {
		return transform.childCount == 0 && allowPocketableTags.Contains(thing.tag);
	}

	public bool Place(GameObject thing) {
		if (CanTake(thing)) {
			thing.transform.SetParent(transform);
			thing.transform.position = Vector3.zero;
			thing.transform.localRotation = Quaternion.identity;
			return true;
		}
		return false;
	}
}
