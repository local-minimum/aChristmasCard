﻿using UnityEngine;
using System.Collections;

public class InterestPointRestricted : InterestPoint {


	public InterestPoint restriction;

	public override void Action(PlayerController player) {
		if (player.target == this)
			player.target = restriction;

	}

	public override void AttachingTo (GameObject parent)
	{
		base.AttachingTo (parent);
		restriction = parent.GetComponentInParent<InterestPoint>();
	}

}
