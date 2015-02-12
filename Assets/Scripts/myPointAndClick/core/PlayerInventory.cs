using UnityEngine;
using System.Collections;

namespace PointClick {

	public class PlayerInventory : PlayerAspect {


		public bool PickUp(GameObject item) {
			return false;
		}

		public GameObject Drop(GameObject item) {
			return item;
		}
	}
}