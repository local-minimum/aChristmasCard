using UnityEngine;
using System.Collections;

namespace PointClick {

	public class InterestPointEvented : InterestPointTrigger {

		public bool requireFireStarted = false;
		public bool requireFireStopped = false;

		[SerializeThis]
		private bool fireStarted = false;

		[SerializeThis]
		private bool fireStopped = false;

		public override void Action (PlayerController player)
		{
			if ((!requireFireStarted || fireStarted) && (!requireFireStopped || fireStopped))
				base.Action (player);
		}

		public override void Action (PlayerController player, InterestPoint interest)
		{
			if ((!requireFireStarted || fireStarted) && (!requireFireStopped || fireStopped))
				base.Action (player, interest);
		}

		public void Fire() {
			fireStarted = true;
		}

		public void FireStop() {
			fireStopped = true;
		}
	}

}
