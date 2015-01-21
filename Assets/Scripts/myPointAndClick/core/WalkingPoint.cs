using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PointClick {

	/// <summary>
	/// Those points where the player can rest at
	/// </summary>
	public class WalkingPoint : Point {

		new void SetConnections() {
			connections.Clear();
			connections.Add(room.walkingPoints.Where(wp => wp != this && Vector3.Distance(wp.transform.position, transform.position) < room.walkPointMaxDistConnector));

		}

	}

}
