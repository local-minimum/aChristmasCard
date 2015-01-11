using UnityEngine;
using System.Collections;

public class InterestPointOven : InterestPointZoom {

	public string stopBurningWord;
	public string startCookingWord;

	public override ApplyResults Apply (PlayerController player, GameObject tool)
	{
		ApplyResults res = base.Apply (player, tool);
		if (res == ApplyResults.ACCEPTED) 
			SaveState.Instance.SetFoodInOven(true);
		return res;
	}

	public override void Action (PlayerController player)
	{
		bool foodInOven = SaveState.Instance.GetFoodInOven();
		bool foodCooking = SaveState.Instance.GetFoodCooking();
		bool foodBurning = SaveState.Instance.GetEventFire();
		if (foodBurning) {
			SaveState.Instance.SetFoodInOven(false);
			SaveState.Instance.SetFoodCooking(false);
			SaveState.Instance.SetEventFire(false);
			player.Learn(stopBurningWord);
		} else if (foodInOven && !foodCooking) {
			SaveState.Instance.SetFoodCooking(true);
			player.Learn(startCookingWord);
		}
	}
}
