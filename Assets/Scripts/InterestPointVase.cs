using UnityEngine;
using System.Collections.Generic;

public class InterestPointVase : InterestPointTrigger {

	public List<string> triggerWords = new List<string>();

	public override void SpecificAction (PlayerController player)
	{
		base.SpecificAction(player);
		if (nextTrigger >= 0 && nextTrigger < triggerWords.Count && triggerWords[nextTrigger] != "") {
			player.Learn(triggerWords[nextTrigger]);
		}
	}
}
