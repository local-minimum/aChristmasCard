using UnityEngine;
using System.Collections;

namespace PointClick {

	public class WalkingPoint : Point {

		protected override void SetConnections ()
		{
			connections.Clear();
			connections.AddRange(room.paths.GetWalkingPointsCloseTo(this));
			
		}
	}

}
