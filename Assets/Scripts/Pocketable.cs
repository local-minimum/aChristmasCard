using UnityEngine;
using System.Collections;

public class Pocketable : MonoBehaviour {

	public enum InstanceType {WORLD, UI};

	public InstanceType version;
	public GameObject prefab;

	public GameObject GetCorresponding() {
		return (GameObject) Instantiate(prefab);
	}

	void Start() {
		if (prefab == null) {
			Debug.LogError(string.Format("There must be a prefab to all pocketables, not present in {0}", this.name));
			return;
		}

		Pocketable prefabPocketable = prefab.GetComponent<Pocketable>();
		if (prefabPocketable == null ) {
			Debug.LogError(string.Format("The prefab must contain a pocketable, not present in {0}", prefab.name));
			return;
		}

		if (this.version == prefabPocketable.version) {
			Debug.LogError(string.Format("The corresponding prefabs must have different instance types, not so for {0) and {1}",
			                             this.name, prefab.name));
		}

		if (this.tag != prefab.tag) {
			Debug.LogError(string.Format("Corresponding pocketables must have the same tag, not true for {0} and {1}", this.name, prefab.name));
		}
	}
}
