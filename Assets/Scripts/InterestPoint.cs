using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InterestPoint : MonoBehaviour {

	private Room _room;

	public bool editMode = false;

	public bool walkingPoint = true;

	public InterestPoint viewedFrom;

	public List<InterestPoint> connections = new List<InterestPoint>();

	public Room room {
		get {
			return _room;
		}
	}

	// Use this for initialization
	void Awake () {
		_room = gameObject.GetComponentInParent<Room>();
	}
	
	// Update is called once per frame
	void Update () {
		if (editMode) {
			foreach (InterestPoint ip in connections)
				Debug.DrawLine(transform.position, ip.transform.position, Color.blue);
		}
	}

	public bool Contains(InterestPoint pt) {
		return connections.Contains(pt);
	}

	public InterestPoint[] FindPathTo(InterestPoint other) {
		if (connections.Contains(other))
			return new InterestPoint[] {this};
		if (viewedFrom == other)
			return new InterestPoint[] {other, this};
		
		List<InterestPoint[]> paths = new List<InterestPoint[]>();
		foreach (InterestPoint pt in connections)
			paths.Add(new InterestPoint[] {pt});

		if (viewedFrom)
			paths.Add(new InterestPoint[] {viewedFrom});

		int pos = 0;
		bool foundTarget = false;
		int foundAtLength = 0;

		while (pos < paths.Count()) {
			InterestPoint[] query = paths[pos];
			if (foundTarget && foundAtLength < query.Length )
				break;
			if (query.Last().AddNovelRelevantPaths(other, query, ref paths) && !foundTarget) {			    
				foundAtLength = query.Length;	
				foundTarget = true;
			}
			pos ++;
		}

//		Debug.Log(pos);
		if (!paths.Any(p => p.Contains(other)))
			return new InterestPoint[] {};
		return Concat(paths.Where(p => p.Contains(other)).OrderBy(p => Distance(p)).First().Reverse().Skip(1).ToArray(), this);
	}

	private float Distance(InterestPoint[] path) {

		float f = 0f;
		for (int i=0, l=path.Length-1; i<l;i++)
			f += Vector3.Distance(path[i].transform.position, path[i + 1].transform.position);
		return f;
	}

	private Type[] Concat<Type>(Type[] arr, Type item) {
		Type[] ret = new Type[arr.Length + 1];
		arr.CopyTo(ret, 0);
		ret[arr.Length] = item;
		return ret;
	}

	public bool AddNovelRelevantPaths(InterestPoint target, InterestPoint[] query, ref List<InterestPoint[]> paths) {
		if (connections.Contains(target)) {
			paths.Add(Concat<InterestPoint>(query, target));
		 	return true;
		} 

		foreach (InterestPoint pt in connections) {
			if (!paths.Any(p => p.Contains(pt)) || paths.Where(p => p.Contains(pt)).Max(p => System.Array.IndexOf((InterestPoint[]) p, pt)) >= query.Length + 1)
				paths.Add(Concat<InterestPoint>(query, pt));
		}

		return false;
	}
}
