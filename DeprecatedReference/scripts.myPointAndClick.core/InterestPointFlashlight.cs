using UnityEngine;
using System.Collections;


namespace PointClick.Old {

	public class InterestPointFlashlight : InterestPointRestricted {


		public bool hasEnergy = false;
		private Light lightSource;

		public bool lit {
			get {
				return lightSource.enabled;
			}
		}

		protected override void Start ()
		{
			base.Start ();
			lightSource = GetComponentInChildren<Light>();
			lightSource.enabled = hasEnergy && !room.lit;

		}

		public override ApplyResults Apply (PlayerController player, GameObject tool)
		{
			if (!hasEnergy && tool.tag == "Battery") {
				if (player.target != restriction)
					return ApplyResults.REQUEST_ACTION;
				player.Drop(tool, false);
				hasEnergy = true;
				return ApplyResults.ACCEPTED;
			}
			return ApplyResults.REFUSED;
		}

		public override void SpecificAction (PlayerController player)
		{
			if (!hasEnergy) {
				player.NeedEnergy();
				return;
			}
		}

		protected override void Update ()
		{
			base.Update ();

			if (room.lit && lit)
				Pocket();
			else
				ShineALight();

			lightSource.enabled = hasEnergy && !room.lit;
		}

		void Pocket() {

		}

		void ShineALight() {

		}
	}

}
