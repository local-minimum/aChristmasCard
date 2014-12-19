using UnityEngine;
using System.Collections.Generic;

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
		inTransition = true;
	}

	protected void UnlockPlayer(InterestPoint pt) {

		if (pt != viewedFrom) {
			altViewedFrom = viewedFrom;
			viewedFrom = pt;
			passageTrail.Reverse();
			player.room = viewedFrom.room;
			room = viewedFrom.room;
		}
		animator.SetTrigger(closeTrigger);

		player.SetTargetPath(viewedFrom);
		player.playerLocked = false;
		player.moveable = true;
		inTransition = false;
		pathIndex = 0;
	}

	protected virtual void Update() {
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
