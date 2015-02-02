using UnityEngine;
using System.Collections;

namespace PointClick {

	public class PlayerMovement : PlayerAspect {


		[SerializeThis]
		private WalkingPoint _location;

		public WalkingPoint location {
			get {
				return _location;
			}
		}

		// Use this for initialization
		void Awake () {
			if (!_location)
				SetLocationByProximity();
		}

		void SetLocationByProximity() {
			_location = player.room.paths.GetWalkingPointClosestTo(transform.position);
			transform.position = _location.transform.position;
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
