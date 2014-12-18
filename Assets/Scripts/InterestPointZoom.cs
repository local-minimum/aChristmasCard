using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InterestPointZoom : InterestPoint {

	public Transform zoomPosition;
	bool zoomed = false;

	public List<InterestPoint> children = new List<InterestPoint>();

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

	public override void SpecificAction(PlayerController player){
//		Debug.Log(string.Format("{0} zoomed: {1}", this, zoomed));
		if (!zoomed) {
			player.room.SetZoomPosition(zoomPosition);
			player.moveable = false;
			zoomed = true;
		} else {
			player.moveable = true;
			zoomed = false;
			player.target = viewedFrom;
		}
	}
}
