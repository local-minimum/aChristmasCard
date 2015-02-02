using UnityEngine;
using System.Collections;
using System.Linq;

namespace PointClick {

	public class WalkingPoint : Point {

		protected override void SetConnections ()
		{
			connections.Clear();
			connections.AddRange((Point[]) room.paths.GetWalkingPointsCloseTo(this).ToArray());
			
		}
	}

}
