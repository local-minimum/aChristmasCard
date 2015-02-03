using UnityEngine;
using System.Collections.Generic;
using FunOps;
using System.Linq;

namespace PointClick {

	public class PlayerActions : PlayerAspect {

		public bool selected = true;

		public bool locked = false;

		[SerializeThis]
		private bool delayedAction = false;

		[SerializeThis]
		private Point actionTarget;

		[SerializeThis]
		private WalkingPoint actionLocation;

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
			actionLocation = PathFinder.GetWalkingPointLocationForPoint(actionTarget);
			if (!actionTarget) {
				delayedAction = false;
				return;
			}

			if (player.movement.location != actionLocation || player.movement.walking) {
				delayedAction = true;
				player.movement.SetTarget(actionLocation);
			} else {
				delayedAction = false;
				BroadcastActionMessage();
			}

		}

		void DelayedAct() {
			BroadcastActionMessage();
			delayedAction = false;
		}

		void BroadcastActionMessage() {
			ActionMessage msg = new ActionMessage(player, actionTarget, !delayedAction);

			Point[] recieverPoints = player.room.paths.ClosestPathBetween(actionTarget, (Point) actionLocation);
			recieverPoints = recieverPoints.Cons(actionTarget);
			foreach (GameObject node in UniqueRecievers(recieverPoints))
				node.BroadcastMessage(actionName, msg, SendMessageOptions.DontRequireReceiver);

		}

		IEnumerable<GameObject> UniqueRecievers(Point[] segment) {
			while (segment.Length > 0) {
				Point pt = segment.First();
				Point[] parentPoints = pt.transform.GetComponentsInParent<Point>();
				segment = segment.Rest().Where(p => !p.GetComponentsInParent<Point>().Contains(pt)).ToArray();
				if (!segment.Any(p => parentPoints.Contains(p)))
					yield return pt.gameObject;
			}
		}
	}

}
