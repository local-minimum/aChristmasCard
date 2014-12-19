using UnityEngine;
using System.Collections;

public class InterestPointDoorOption : InterestingPointDoor {

	private int pathDirection = 1;
	public string learnWordFail = "";
	public string learnWordSucceed = "";

	protected override void Update ()
	{
		if (inTransition) {
			
			if (player.CheckProximity(passageTrail[pathIndex].position, pathIndex == passageTrail.Count - 1)) {
				pathIndex += pathDirection;
				if (pathIndex >= passageTrail.Count) {
					if (player.hasLight) {
						UnlockPlayer(altViewedFrom);
						player.Learn(learnWordSucceed);
						return;
					} else {
						pathDirection *= -1;
						pathIndex += pathDirection;
						player.Learn(learnWordFail);
					}
				} else if (pathIndex < 0) {
					UnlockPlayer(viewedFrom);
				}
			}
			
			player.Move(player.GetAim(passageTrail[pathIndex].position));
		}
	}
}
