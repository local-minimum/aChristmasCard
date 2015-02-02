using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PointClick.Old {

	public class InterestPointWasteBasket : InterestPoint {

		public List<string> acceptableTrash = new List<string>();
		private bool hasMissed = false;
		public InterestPoint missPlace;
		public string missWord;
		public string hitWord;

		public override ApplyResults Apply (PlayerController player, GameObject tool)
		{

			if (acceptableTrash.Contains(tool.tag)) {

				if (hasMissed) {
					Hit (player, tool);
				} else {
					Miss(player, tool);
					hasMissed = true;
				}
				return ApplyResults.ACCEPTED;
			}
			return ApplyResults.REFUSED;

		}

		private void Miss(PlayerController player, GameObject tool) {
			player.Learn(missWord);
		}

		public void Hit(PlayerController player, GameObject tool) {
			player.Learn(hitWord);
		}
	}

}
