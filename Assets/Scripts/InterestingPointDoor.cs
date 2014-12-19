﻿using UnityEngine;
using System.Collections.Generic;

public class InterestingPointDoor : InterestPointTrigger {
	public InterestPoint altViewedFrom;
	public List<Transform> passageTrail = new List<Transform>();
	public string closeTrigger = "Close";
	bool inTransition = false;
	int pathIndex = 0;
	PlayerController player;
	public override void SpecificAction (PlayerController player)
	{
		player.playerLocked = true;
		this.player = player;
		inTransition = true;
	}

	void UnlockPlayer() {

		InterestPoint pt = altViewedFrom;
		altViewedFrom = viewedFrom;
		viewedFrom = pt;
		passageTrail.Reverse();
		animator.SetTrigger(closeTrigger);
		player.room = viewedFrom.room;
		room = viewedFrom.room;
		player.SetTargetPath(viewedFrom);
		player.playerLocked = false;
		player.moveable = true;
//		player.target = viewedFrom;
//		player = null;
		inTransition = false;
		pathIndex = 0;
	}

	void Update() {
		if (inTransition) {

			if (player.CheckProximity(passageTrail[pathIndex].position, pathIndex == passageTrail.Count - 1)) {
				pathIndex++;
				if (pathIndex >= passageTrail.Count) {
					UnlockPlayer();
					return;
				}
			}

			player.Move(player.GetAim(passageTrail[pathIndex].position));
		}
	}
}
