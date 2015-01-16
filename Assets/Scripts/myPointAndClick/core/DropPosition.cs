using UnityEngine;
using System.Collections;
using System.Linq;

namespace PointClick {

	public class DropPosition : MonoBehaviour {

		public string[] allowedTags = new string[] {};

		protected void Start() {
//			if (allowedTags.Length == 0) {
//				Debug.Log("no tags");
//				allowedTags = GameObject.FindObjectsOfType<Pocketable>().Select(p => p.tag).Distinct().ToArray();
//			}
		}

		public bool CanTake(GameObject thing) {
			return transform.childCount == 0 && (allowedTags.Contains(thing.tag) || allowedTags.Length == 0);
		}

		public bool Place(GameObject thing) {
			if (CanTake(thing)) {
				thing.transform.parent = transform;
				foreach(Collider c in thing.GetComponentsInChildren<Collider>())
					c.gameObject.layer = LevelManager.Instance.player.room.ObjectsCollector.layer;
				thing.transform.localPosition = Vector3.zero;
				thing.transform.localRotation = Quaternion.identity;
				thing.BroadcastMessage("AttachingTo", gameObject, SendMessageOptions.DontRequireReceiver);
				return true;
			}
			return false;
		}
	}

}