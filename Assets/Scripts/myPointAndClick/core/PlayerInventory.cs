using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PointClick {

	public class PlayerInventory : PlayerAspect {

		[SerializeField]
		List<PlayerInventoryMap> inventories = new List<PlayerInventoryMap>();

		public int size {
			get {
				return inventories.Count();
			}
		}

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

		public void AddInventory(string name, int[] shape) {
			inventories.Add(new PlayerInventoryMap(name, shape, AllTags.ToArray()));
		}

		public void RemoveInventory(PlayerInventoryMap inventory) {
			inventories = inventories.Where(i => i!=inventory).ToList();
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

		public IEnumerable<PlayerInventoryMap> Inventories {

			get {
				foreach (PlayerInventoryMap im in inventories)
					yield return im;
			}
		}

		public void Promote(PlayerInventoryMap inventory) {
			int nextPos = Inventories.IndexOf(inventory) - 1;
			if (nextPos >= 0)
				SwapPlaces(inventory, nextPos);
		}

		public void Demote(PlayerInventoryMap inventory) {
			int nextPos = Inventories.IndexOf(inventory) + 1;
			if (nextPos != inventories.Count())
				SwapPlaces(inventory, nextPos);

		}

		private void SwapPlaces(PlayerInventoryMap inventory, int position) {
			int currentPosition = Inventories.IndexOf(inventory);
			PlayerInventoryMap otherInventory = inventories[position];
			inventories[position] = inventory;
			inventories[currentPosition] = otherInventory;
		}
	}
}