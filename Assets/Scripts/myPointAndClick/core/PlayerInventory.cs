using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PointClick {

	public class PlayerInventory : PlayerAspect {

		List<PlayerInventoryMap> inventories = new List<PlayerInventoryMap>();

		public bool PickUp(GameObject item) {

			Interactable interactable = item.GetComponentInChildren<Interactable>();

			if (interactable && interactable.pocketable) {
				return _take(item, interactable);
			} else
				return false;
		}

		public GameObject Drop(GameObject item) {
			return item;
		}

		bool _take(GameObject item, Interactable interactable) {
			foreach (PlayerInventoryMap im in inventories) {
				if (im.CanTake(interactable)) {
					im.Take(item, interactable);
					return true;
				}
			}

			return false;
		}

		public void AddInventory(int[] shape) {
			inventories.Add(new PlayerInventoryMap(shape, AllTags.ToArray()));
		}
	
		public IEnumerable<string> AllTags {
			get {
				HashSet<string> knownTags = new HashSet<string>();

				foreach (PlayerInventoryMap im in inventories) {
					foreach (string tag in im.tagFilter.Tags) {
						if (!knownTags.Contains(tag)) {
							knownTags.Add(tag);
							yield return tag;
						}
					}
				}
			}
		}

	}
}