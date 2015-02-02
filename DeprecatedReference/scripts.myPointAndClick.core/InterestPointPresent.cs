using UnityEngine;
using System.Collections;

namespace PointClick.Old {

	public class InterestPointPresent : InterestPointTrigger {

		public string opensWith;
		private bool opened = false;
		public Pocketable presentUIVersion;

		public override void Action (PlayerController player)
		{

		}

		public override ApplyResults Apply (PlayerController player, GameObject tool) {

			if (!opened && player.Has(opensWith)) {
				opened = true;
				base.Action(player);
				GameObject presentContent = presentUIVersion.GetCorresponding();
				presentContent.transform.position = transform.position;
				room.AddInterest(presentContent.GetComponentInChildren<InterestPoint>());
				return ApplyResults.ACCEPTED;
			}
			return ApplyResults.REFUSED;
		}
	}

}
