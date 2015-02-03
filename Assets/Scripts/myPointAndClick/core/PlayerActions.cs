using UnityEngine;
using System.Collections.Generic;

namespace PointClick {

	public class PlayerActions : PlayerAspect {

		public bool selected = true;

		public bool locked = false;

		[SerializeThis]
		private bool delayedAction = false;

		[SerializeThis]
		private Point actionTarget;

		public enum ActionTypes {Apply, Use, Arrive, None};

		[SerializeThis]
		private ActionTypes currentAction = ActionTypes.Apply;

		public string actionName {
			get {
				return GetActionFuctionName(currentAction);
			}
		}

		public string GetActionFuctionName(ActionTypes action) {
			if (action == ActionTypes.Apply)
				return "OnPlayerApply";
			else if (action == ActionTypes.Use)
				return "OnPlayerUse";
			else if (action == ActionTypes.Arrive)
				return "OnPlayerArrive";
			else
				return "OnPlayer";
		}

		void Update () {
			if (selected && !locked && Input.GetButtonDown("Fire1"))
				Act();
			else if (delayedAction && !player.movement.walking)
				DelayedAct();
		}

		void Act() {
			actionTarget = player.room.paths.GetPointClosestToPointer();
			WalkingPoint actionLocation = PathFinder.GetWalkingPointLocationForPoint(actionTarget);
			if (!actionTarget)
				return;

			if (player.movement.location != actionLocation || player.movement.walking) {
				delayedAction = true;
				player.movement.SetTarget(actionLocation);
			} else {
				actionTarget.BroadcastMessage(actionName, new ActionMessage(player), 
				                              SendMessageOptions.DontRequireReceiver);
				delayedAction = false;
			}

		}

		void DelayedAct() {
			actionTarget.BroadcastMessage(actionName, new ActionMessage(player, false),
			                              SendMessageOptions.DontRequireReceiver);
			delayedAction = false;
		}
	}

}
