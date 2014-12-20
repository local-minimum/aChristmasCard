using UnityEngine;
using System.Collections;
using System.Linq;

public class DropPosition : MonoBehaviour {

	public string[] allowPocketableTags;

	void Start() {
		if (allowPocketableTags.Length == 0) {
			allowPocketableTags = FindObjectsOfType<Pocketable>().Select(p => p.tag).Distinct().ToArray();
		}
	}

	public bool CanTake(GameObject thing) {
		return transform.childCount == 0 && allowPocketableTags.Contains(thing.tag);
	}

	public bool Place(GameObject thing) {
		if (CanTake(thing)) {
			thing.transform.parent = transform;
			thing.transform.localPosition = Vector3.zero;
			thing.transform.localRotation = Quaternion.identity;
			return true;
		}
		return false;
	}
}
