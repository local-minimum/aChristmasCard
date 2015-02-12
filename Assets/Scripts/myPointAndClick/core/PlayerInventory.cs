using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PointClick {

	public class PlayerInventory : PlayerAspect {

		public string[] allowTags = new string[0];

		public int[] inventoryShape;

		private Dictionary<GameObject, Interactable> slots = new Dictionary<GameObject, Interactable>();
			
		public bool PickUp(GameObject item) {

			Interactable interactable = item.GetComponentInChildren<Interactable>();

			if (interactable && interactable.pocketable && (allowTags.Length == 0 || allowTags.Contains(item.tag))) {
				return _take(item, interactable);
			} else
				return false;
		}

		public GameObject Drop(GameObject item) {
			return item;
		}

		bool _take(GameObject item, Interactable interactable) {

			return true;
		}
	

	}
}