using UnityEngine;
using System.Collections;

namespace PointClick {

	public class InterestPointEvented : InterestPointTrigger {

		public bool fireEvent = false;

		public override void Action (PlayerController player)
		{
			if (!fireEvent || SaveState.Instance.GetEventFire())
				base.Action (player);
		}

		public override void Action (PlayerController player, InterestPoint interest)
		{
			if (!fireEvent || SaveState.Instance.GetEventFire())
				base.Action (player, interest);
		}
	}

}
