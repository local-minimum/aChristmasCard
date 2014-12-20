using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	[Range(0, 4)]
	public float arriveIntermediate = 1f;

	[Range(0, 2)]
	public float arriveDestination = 0.1f;

	[HideInInspector]
	public bool moveable = true;

	[HideInInspector]
	public bool playerLocked = false;

	private bool inTransition = false;

	public float force = 500;
	public float maxVelocity = 1f;
	
	private Room _room = null;
	
	public List<Transform> inventoryPositions = new List<Transform>();
	public List<Transform> batteryPositions = new List<Transform>();
	public GameObject uiBatteryPrefab;

	public Room room {
		get {
			return _room;
		}

		set {
			transform.parent = value.transform;
			_room = value;
		}
	}

	public Vector3 offset;

	private List<InterestPoint> walkPath = new List<InterestPoint>();

	private float nextDistance = 0f;

	public InterestPoint target {
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
			Debug.Log(string.Format("targeting {0}", value));
			if (value == target)
				return;
			walkPath.AddRange(value.FindPathTo(walkPath[walkPath.Count - 1]));
			inTransition = true;
		}
	}

	public InterestPoint location {
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
	
	private Vector3 aim {
		get {
			if (walkPath.Count > 1)
				return (walkPath[1].transform.position - (transform.position - offset)).normalized;
			return Vector3.zero;
		}
	}

	public Vector3 GetAim(Vector3 other) {
		return (other - (transform.position - offset)).normalized;
	}

	private bool nextIsWalkTarget {
		get {
			return walkPath[walkPath.Count - 1].walkingPoint ? (walkPath.Count == 2) : (walkPath.Count == 3);
		}
	}

	private InterestPoint walkTarget {
		get {
			if (walkPath.Count == 0)
				return null;

			if (walkPath[walkPath.Count - 1].walkingPoint)
				return walkPath[walkPath.Count - 1];

			if (walkPath.Count == 1)
				return null;

			return walkPath[walkPath.Count - 2];
		}
	}

	public void SetTargetPath(InterestPoint pt) {
		walkPath.Clear();
		walkPath.Add(pt);
	}

	// Use this for initialization
	void Start () {
		room = gameObject.GetComponentInParent<Room>();
		SetTargetPath(room.GetWalkingPointClosestTo(transform.position));
		ArrivedAt(location);
		transform.position = location.transform.position + offset;
//		foundWordsUI = GetComponentInChildren<FoundWords>();
	}
	

	// Update is called once per frame
	void Update () {
		if (LevelManager.Instance.uiView || playerLocked)
			return;

		if (CheckProximity())
			return;

		Move(aim);
	}

	public void Move(Vector3 aim) {
		
		rigidbody.AddForce(aim * force * Time.deltaTime, ForceMode.Force);
		rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxVelocity);
	}

	public bool CheckProximity(Vector3 other, bool nextIsWalkTarget) {
		nextDistance = Vector3.Distance(other, transform.position - offset);

		//		Debug.Log(nextDistance);
		if (nextIsWalkTarget) {
			if (nextDistance < arriveDestination)
				return true;
		} else if (nextDistance < arriveIntermediate)
			return true;
		return false;
	}

	bool CheckProximity() {

		if (!inTransition)
			return true;

		bool arrived = CheckProximity(walkPath[1].transform.position,nextIsWalkTarget);

		if (arrived)
			ArrivedAt(walkPath[1]);
		return arrived;
	}

	void ArrivedAt(InterestPoint pt) {
		gameObject.layer = pt.gameObject.layer;
		room = pt.room;
		location = pt;
		inTransition = pt != walkTarget;
		if (!inTransition) {
			rigidbody.velocity = Vector3.zero;
//			Debug.Log(target);
			target.Action(this);
		}
	}

	public void SetInterest(InterestPoint pt) {
		if (!moveable) {
			target.Action(this, pt);
			return;
		}
//		if (target == pt)
//			return;

		target = pt;
	}

	public void Learn(string word) {
		WordList.Instance.Learn(this, word);
	}

	public void NeedEnergy() {

	}

	public bool[] Knows(IEnumerable<string> words) {
		return new bool[]{};
	}

	public bool hasLight {
		get {
			return false;
		}
	}

	private bool placeInEmptySlot(List<Transform> slots, Pocketable item) {
		foreach (Transform t in slots) {
			if (t.childCount == 0) {
				item.GetCorresponding().transform.SetParent(t);
				Destroy(item.gameObject, 0.5f);
				return true;
			}
		}
		return false;
	}

	                                                                    
	public bool PickUp(GameObject thing) {
		Pocketable pocketThing = thing.GetComponent<Pocketable>();

		//Can thing be placed in inventory
		if (pocketThing == null || pocketThing.version != Pocketable.InstanceType.WORLD)
			return false;

		return placeInEmptySlot(thing.tag == "batteries" ? batteryPositions : inventoryPositions, pocketThing);
	}

	public void Drop(GameObject thing) {
		foreach (Transform t in (thing.tag == "battery" ? batteryPositions : inventoryPositions)) {
			if (thing.transform.IsChildOf(t)) {
				Destroy(thing.gameObject, 0.5f);
				return;
			}
		}
	}

}
