using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	[Range(0, 4)]
	public float arriveIntermediate = 1f;

	[Range(0, 2)]
	public float arriveDestination = 0.1f;

	public bool moveable = true;
	private bool inTransition = false;

	public Room room;

	private List<InterestPoint> walkPath = new List<InterestPoint>();

	private InterestPoint target {
		get {
			if (walkPath.Count == 0)
				return null;
			
			return walkPath[walkPath.Count - 1];
		}

		set {
			if (inTransition)
				walkPath.RemoveRange(2, walkPath.Count - 2);
			else
				walkPath.RemoveRange(1, walkPath.Count - 1);
			walkPath.AddRange(value.FindPathTo(walkPath[walkPath.Count - 1]));
			inTransition = true;
		}
	}

	private InterestPoint location {
		get {
			if (walkPath.Count == 0)
				return null;

			return walkPath[0];
		}

		set {
			while (walkPath[0] != value)
				walkPath.RemoveAt(0);

			if (walkPath.Count == 0)
				walkPath.Add(value);
		}
	}

	private InterestPoint nextInWalk {
		get {
			if (walkPath.Count > 1)
				return walkPath[1];
			return null;
		}
	}

	// Use this for initialization
	void Start () {
		room = gameObject.GetComponentInParent<Room>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!inTransition)
			return;

		InterestPoint pt = nextInWalk;

		if (!pt)
			return;


	}

	void ArrivedAt(InterestPoint pt) {
		gameObject.layer = pt.gameObject.layer;
		room = pt.room;
		location = target;
	}

	public void SetInterest(InterestPoint pt) {
		if (target == pt)
			return;

		target = pt;
	}

}
