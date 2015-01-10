using UnityEngine;
using System.Collections;

public class InterestPointPresent : InterestPointTrigger {

	public override void Action (PlayerController player)
	{
		if (player.hasLight)
			base.Action (player);
	}
}
