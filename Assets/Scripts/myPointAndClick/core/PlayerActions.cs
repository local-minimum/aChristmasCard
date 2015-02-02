using UnityEngine;
using System.Collections;

namespace PointClick {

	public class PlayerActions : PlayerAspect {

		public bool selected = true;

		void Update () {
			if (selected && Input.GetButtonDown("Fire1"))
				Act();
		}

		void Act() {

		}
	}

}
