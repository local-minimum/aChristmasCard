using UnityEngine;
using System.Collections;

namespace PointClick {

	public class PlayerActions : PlayerAspect {

		public bool selected = true;
		public bool locked = false;

		void Update () {
			if (selected && !locked && Input.GetButtonDown("Fire1"))
				Act();
		}

		void Act() {
			Point actionTarget = player.room.paths.GetPointClosestToPointer();
			WalkingPoint actionLocation = PathFinder.GetWalkingPointLocationForPoint(actionTarget);

			if (player.movement.location != actionLocation)
				player.movement.SetTarget(actionLocation);
			else 
				actionTarget.BroadcastMessage("Invoke", new ActionMessage(player), 
				                              SendMessageOptions.DontRequireReceiver);

		}
	}

}
