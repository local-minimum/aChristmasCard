using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InterestPointZoom : InterestPoint {

	public Transform zoomPosition;
	bool zoomed = false;

	public InterestPoint[] children {
		get {
			return GetComponentsInChildren<InterestPoint>();
		}
	}

	public override void Action(PlayerController player) {
//		Debug.Log("ZoomAction");
		SpecificAction(player);
	}

	public override void Action(PlayerController player, InterestPoint interest) {
//		Debug.Log(string.Format("{0} interest {1}", this, interest));
		if (children.Contains(interest))
		    interest.SpecificAction(player);
		else {
			SpecificAction(player);
		}
	}

	public override ApplyResults Apply (PlayerController player, GameObject tool, InterestPoint interest)
	{
		if (children.Contains(interest))
			return interest.Apply(player, tool);
		return ApplyResults.REFUSED;
	}

	public override void SpecificAction(PlayerController player){
//		Debug.Log(string.Format("{0} zoomed: {1}", this, zoomed));
		if (!zoomed) {
			if (zoomPosition) {
				player.room.SetZoomPosition(zoomPosition);
				player.moveable = false;
			}
			zoomed = true;
		} else {
			player.moveable = true;
			zoomed = false;
			player.target = viewedFrom;
		}
	}
}
