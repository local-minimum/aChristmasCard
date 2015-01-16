using UnityEngine;
using System.Collections.Generic;

namespace PointClick {

	public class InterestingPointDoor : InterestPointTrigger {
		public InterestPoint altViewedFrom;
		public List<Transform> passageTrail = new List<Transform>();
		public string closeTrigger = "Close";
		protected bool inTransition = false;
		protected int pathIndex = 0;
		protected PlayerController player;

		public override void SpecificAction (PlayerController player)
		{
			player.playerLocked = true;
			this.player = player;
			player.room = altViewedFrom.room;
			inTransition = true;
		}

		protected void UnlockPlayer(InterestPoint pt) {

			if (pt != viewedFrom) {
				altViewedFrom = viewedFrom;
				viewedFrom = pt;
				passageTrail.Reverse();
				room = viewedFrom.room;
			} else {
				player.room = viewedFrom.room;
			}

			StartCoroutine( closeAnimation());
			player.playerLocked = false;
			player.SetTargetPath(viewedFrom);
			player.moveable = true;
			inTransition = false;
			pathIndex = 0;
		}

		IEnumerator<WaitForSeconds> closeAnimation() {
			yield return new WaitForSeconds(1f);
			animator.SetTrigger(closeTrigger);

		}

		protected override void Update ()
		{

			if (inTransition) {

				if (player.CheckProximity(passageTrail[pathIndex].position, pathIndex == passageTrail.Count - 1)) {
					pathIndex++;
					if (pathIndex >= passageTrail.Count) {
						UnlockPlayer(altViewedFrom);
						return;
					}
				}

				player.Move(player.GetAim(passageTrail[pathIndex].position));
			}
		}
	}

}