using UnityEngine;
using System.Collections;

public class Pocketable : MonoBehaviour {

	public enum InstanceType {WORLD, UI};

	public InstanceType version;
	public GameObject correspondingPrefab;

	public GameObject GetCorresponding() {
		return (GameObject) Instantiate(correspondingPrefab);
	}

	void Start() {
		if (correspondingPrefab == null) {
			Debug.LogError(string.Format("There must be a prefab to all pocketables, not present in {0}", this.name));
			return;
		}

		Pocketable prefabPocketable = correspondingPrefab.GetComponent<Pocketable>();
		if (prefabPocketable == null ) {
			Debug.LogError(string.Format("The prefab must contain a pocketable, not present in {0}", correspondingPrefab.name));
			return;
		}

		if (this.version == prefabPocketable.version) {
			Debug.LogError(string.Format("The corresponding prefabs must have different instance types, not so for {0) and {1}",
			                             this.name, correspondingPrefab.name));
		}

		if (this.tag != correspondingPrefab.tag) {
			Debug.LogError(string.Format("Corresponding pocketables must have the same tag, not true for {0} and {1}", this.name, correspondingPrefab.name));
		}

		if (this.version == InstanceType.UI && gameObject.GetComponent<UnityEngine.UI.Button>() == null)
			Debug.LogError(string.Format("UI versions of pocketables must be buttons, not so for {0}", this.name));
	}

	public void SetPlayerUsing () {
		if (version == InstanceType.UI) {
			LevelManager.Instance.player.Using(gameObject);
		}
	}
}
