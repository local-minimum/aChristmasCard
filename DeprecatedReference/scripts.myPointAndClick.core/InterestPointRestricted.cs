using UnityEngine;
using System.Collections;


namespace PointClick.Old {

	public class InterestPointRestricted : InterestPoint {


		public InterestPoint restriction;

		new virtual protected void Start() {
			base.Start();
			if (restriction == null && transform.parent != null)
				restriction = transform.parent.GetComponentInParent<InterestPoint>();
		}

		public override void Action(PlayerController player) {
			Debug.Log("Trying action on restricted point " + gameObject.name);
			if (player.target == this)
				player.target = restriction;

		}

		public override void AttachingTo (GameObject parent)
		{
			base.AttachingTo (parent);
			restriction = parent.GetComponentInParent<InterestPoint>();
		}

	}
}