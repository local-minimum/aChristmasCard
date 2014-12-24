using UnityEngine;
using System.Collections;

public class InterestPointRestricted : InterestPoint {


	public InterestPoint restriction;

	new protected void Start() {
		base.Start();
		if (restriction == null && transform.parent != null)
			restriction = transform.parent.GetComponentInParent<InterestPoint>();
	}

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
