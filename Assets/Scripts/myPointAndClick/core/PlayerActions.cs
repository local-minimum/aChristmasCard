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
			if (!actionTarget)
				return;

			if (player.movement.location != actionLocation)
				player.movement.SetTarget(actionLocation);
			else 
				actionTarget.BroadcastMessage("OnPlayerApply", new ActionMessage(player), 
				                              SendMessageOptions.DontRequireReceiver);

		}
	}

}
