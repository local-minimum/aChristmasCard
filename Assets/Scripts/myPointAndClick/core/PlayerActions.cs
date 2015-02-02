using UnityEngine;
using System.Collections;

namespace PointClick {

	public class PlayerActions : GameEntity {

		public bool selected = true;

		private Room _room;

		void Update () {
			if (selected && Input.GetButtonDown("Fire1"))
				Act();
		}

		void Act() {

		}
	}

}
