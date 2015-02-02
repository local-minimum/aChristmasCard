using UnityEngine;
using System.Collections;

namespace PointClick.Old {

	public class InterestPointOven : InterestPointZoom {

		public string stopBurningWord;
		public string startCookingWord;

		[SerializeThis]
		private bool foodInOven = false;

		[SerializeThis]
		private bool foodCooking = false;

		[SerializeThis]
		private bool foodBurning = false;

		public override ApplyResults Apply (PlayerController player, GameObject tool)
		{
			ApplyResults res = base.Apply (player, tool);
			if (res == ApplyResults.ACCEPTED) 
				foodInOven = true;
			return res;
		}

		public override void Action (PlayerController player)
		{

			if (foodBurning) {
				foodInOven = false;
				foodBurning = false;
				foodCooking = false;
				LevelManager.BroadCastToRooms("FireStop");
				player.Learn(stopBurningWord);
			} else if (foodInOven && !foodCooking) {
				foodCooking = true;
				player.Learn(startCookingWord);
			}
		}

		public void LeftRoom() {
			if (foodCooking) {
				foodBurning = true;
				room.Broadcast("Fire");
			}
		}
	}

}
