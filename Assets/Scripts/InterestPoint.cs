using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InterestPoint : MonoBehaviour {

	public enum ApplyResults {REFUSED, ACCEPTED, REQUEST_ACTION}

	private Room _room;

	public bool editMode = false;

	public bool walkingPoint = true;

	public InterestPoint viewedFrom;

	public List<InterestPoint> connections = new List<InterestPoint>();

	private Pocketable _pocketable = null;
	private bool checkedPocketable = false;

	public Pocketable pocketable {
		get {
			if (!checkedPocketable) {
				_pocketable = GetComponentInParent<Pocketable>();
				if (_pocketable && _pocketable.tag != tag)
					_pocketable = null;
				checkedPocketable = true;
			}
			return _pocketable;
		}
	}

	private List<DropPosition> _dropPositions;
	public List<DropPosition> dropPositions {
		get {
			if (_dropPositions == null)
				_dropPositions = gameObject.GetComponentsInChildren<DropPosition>().ToList<DropPosition>();
			return _dropPositions;
		}
	}

	public Room room {
		get {
			return _room;
		}

		set {
			if (_room)
				_room.RemoveInterest(this);
			if (value != null) {
				value.AddInterest(this);
				if (!GetComponentsInParent<Transform>().Contains(value.ObjectsCollector.transform))
					transform.parent = value.ObjectsCollector.transform;
			}
			_room = value;
		}
	}

	// Use this for initialization
	protected void Awake () {
		_room = gameObject.GetComponentInParent<Room>();
	}

	protected void Start() {
		if (connections.Count() == 0 && viewedFrom == null && transform.parent != null) {
			InterestPoint ip = transform.parent.GetComponentInParent<InterestPoint>();
			if (ip && ip.walkingPoint)
				viewedFrom = ip;
			else if (ip)
				viewedFrom = ip.viewedFrom;

		}
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

	public virtual void Action(PlayerController player) {


	}

	public virtual void Action(PlayerController player, InterestPoint interest) {

	}

	public virtual void SpecificAction(PlayerController player) {
		if (pocketable && player.PickUp(this))
			return;
		Debug.Log(string.Format("{0} {1}", this, "No specific action"));
	}

	public virtual ApplyResults Apply(PlayerController player, GameObject tool) {

		DropPosition[] empties = dropPositions.Where(p => p.CanTake(tool)).ToArray();

		if (empties.Length > 0) {
			if (!empties[Random.Range(0, empties.Length - 1)].Place(player.Drop(tool)))
				Debug.LogError(string.Format("{0} was lost because of {1}", tool.name, name));

			return ApplyResults.ACCEPTED;
		}

		return ApplyResults.REFUSED;
	}

	public virtual ApplyResults Apply(PlayerController player, GameObject tool, InterestPoint interest) {
		return ApplyResults.REFUSED;
	}

	public virtual void AttachingTo(GameObject parent) {
		if (connections.Count == 0 && viewedFrom == null) {
			InterestPoint ip = parent.GetComponentInParent<InterestPoint>();
			if (ip.viewedFrom != null)
				viewedFrom = ip.viewedFrom;
			else
				viewedFrom = ip;
		}
		room = parent.GetComponentInParent<Room>();
	}
}
