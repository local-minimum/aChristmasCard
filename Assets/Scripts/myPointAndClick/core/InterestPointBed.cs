using UnityEngine;
using System.Collections;

namespace PointClick {

	public class InterestPointBed : InterestPointZoom {

		protected override void Start ()
		{
			base.Start ();
			successfullApplications.Add("present", "together");
		}
	}

}
