using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PointClick {

	public class PlayerController : MonoBehaviour {

		public enum PlayerDirections {N, NW, W, SW, S, SE, E, NE, UP, DOWN, NONE};

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
		
		private RoomManager _room = null;
		
		public List<Transform> inventoryPositions = new List<Transform>();
		public List<Transform> batteryPositions = new List<Transform>();

		public RoomManager room {
			get {
				return _room;
			}

			set {
				transform.parent = value.transform;
				if (_room)
					_room.BroadcastMessage("onPlayerExit", this, SendMessageOptions.DontRequireReceiver);
				_room = value;
				if (_room)
					_room.BroadcastMessage("onPlayerEnter", this, SendMessageOptions.DontRequireReceiver);
			}
		}

		public Vector3 offset;

		private List<InterestPoint> walkPath = new List<InterestPoint>();
		private bool _nextNotApply = false;
		private float nextDistance = 0f;
		private GameObject _using = null;
		private Animator anim;

		private GameObject managers;

		public PlayerDirections currentDirection {
			get {
				Vector3 v = rigidbody.velocity.normalized;

				if (rigidbody.velocity.magnitude < 0.001f)
					return PlayerDirections.NONE;
				else if (v.y > 0.001f)
					return PlayerDirections.UP;
				else if (v.y < -0.001f)
					return PlayerDirections.DOWN;

				int a = Mathf.RoundToInt((Vector3.Angle(-Vector3.right, v) - 180) / 45);
				if (a == 0)
					return PlayerDirections.E;
				else if (a == 1)
					return PlayerDirections.NE;
				else if (a == 2)
					return PlayerDirections.N;
				else if (a == 3)
					return PlayerDirections.NW;
				else if (a == -1)
					return PlayerDirections.SE;
				else if (a == -2)
					return PlayerDirections.S;
				else if (a == -3)
					return PlayerDirections.SW;

				return PlayerDirections.W;
			}
		}

		public Transform cursor {
			get {
				if (_using)
					return _using.transform;
				return null;
			}
		}

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
	
			if (room == null)
				room = gameObject.GetComponentInParent<RoomManager>();

			if (LevelManager.Instance.transform.parent)
				managers = LevelManager.Instance.transform.parent.gameObject;
			else
				managers = LevelManager.Instance.gameObject;

			SetTargetPath(room.GetWalkingPointClosestTo(transform.position));
			ArrivedAt(location);
			transform.position = location.transform.position + offset;
			anim = GetComponent<Animator>();
		}
		

		// Update is called once per frame
		void Update () {
			SetPlayerDirection();

			if (LevelManager.Instance.uiView || playerLocked)
				return;

			if (CheckProximity())
				return;

			Move(aim);
		}

		private void SetPlayerDirection() {
			switch (currentDirection) {
				case PlayerDirections.NONE:
					anim.SetTrigger("still");
					break;
				case PlayerDirections.E:
					anim.SetTrigger("east");
					break;
				case PlayerDirections.W:
					anim.SetTrigger("west");
					break;
				case PlayerDirections.N:
					anim.SetTrigger("north");
					break;
				case PlayerDirections.S:
					anim.SetTrigger("south");
					break;
				case PlayerDirections.NE:
					anim.SetTrigger("east");
					break;
				case PlayerDirections.SE:
					anim.SetTrigger("east");
					break;
				case PlayerDirections.NW:
					anim.SetTrigger("west");
					break;
				case PlayerDirections.SW:
					anim.SetTrigger("west");
					break;


			}
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
			if (walkPath.Count < 2) {
				inTransition = false;
				return true;
			}

			bool arrived =  CheckProximity(walkPath[1].transform.position,nextIsWalkTarget);

			if (arrived)
				ArrivedAt(walkPath[1]);
			return arrived;
		}
		
		void ArrivedAt(InterestPoint pt) {
			gameObject.layer = pt.gameObject.layer;
			room = pt.room;
			location = pt;
//			SaveState.Instance.SetPlayerPosition(pt.transform.position);
			inTransition = pt != walkTarget;
			if (!inTransition) {
				rigidbody.velocity = Vector3.zero;
	//			Debug.Log(target);
				if (_using != null && !_nextNotApply) {
					InterestPoint.ApplyResults ar = target.Apply(this, _using);
					Debug.Log(string.Format("Attempt to apply {0} on {1} gave {2}", _using.name, target.name, ar));
					if (ar == InterestPoint.ApplyResults.REFUSED) {
						Sigh();
						StopUsing();
					} else if (ar == InterestPoint.ApplyResults.REQUEST_ACTION) {
						_nextNotApply = true;
						target.Action(this);
					}
				} else {
					_nextNotApply = false;
					target.Action(this);
				}

			}
		}

		public void SetInterest(InterestPoint pt) {
			if (!moveable) {
				if (_using != null && !_nextNotApply) {
					InterestPoint.ApplyResults ar = target.Apply(this, _using, pt);
					Debug.Log(string.Format("Attempt to apply {0} on {1} via {2} gave {3}", _using.name, pt.name, target.name, ar));
					if (ar == InterestPoint.ApplyResults.REFUSED) {
						Sigh();
						StopUsing();
					} else if (ar == InterestPoint.ApplyResults.REQUEST_ACTION) {
						_nextNotApply = true;
						target.Action(this, pt);
					}
				} else {
					_nextNotApply = false;
					target.Action(this, pt);
				}
				return;
			}

			target = pt;
		}

		public void Sigh() {
			iTween.PunchRotation(LevelManager.MainCamera.gameObject,
			                     iTween.Hash("z", 5f, "duration", 0.5f)); 
		}

		public void Learn(string word) {
			managers.BroadcastMessage("Learn", new PointClick.LearnedMessage(this, word) ,SendMessageOptions.DontRequireReceiver);
//			WordList.Instance.Learn(this, word);
		}

		public void NeedEnergy() {

		}

		public bool[] Knows(IEnumerable<string> words) {
			return new bool[]{};
		}

		public bool hasLight {
			get {
				return Has("light");
			}
		}

		public bool Has(string objectTag) {
			return inventoryPositions.Where(t => t.GetComponentsInChildren<Transform>()
			                                .Select(t2 => t2.tag).Contains(objectTag)).Any();
	 	}

		private bool placeInEmptySlot(List<Transform> slots, InterestPoint item) {
			foreach (Transform t in slots) {
				if (t.childCount == 0) {
					item.room = null;
					GameObject uiItem = item.pocketable.GetCorresponding();
					uiItem.transform.SetParent(t);
	//				RectTransform rt = uiItem.GetComponent<RectTransform>();
	//				rt.anchoredPosition = Vector2.zero;
	//				rt.rotation = Quaternion.identity;
					uiItem.transform.rotation = Quaternion.identity;
					uiItem.transform.position = LevelManager.MainCamera.WorldToScreenPoint(item.transform.position);
					iTween.MoveTo(uiItem.gameObject,
					              iTween.Hash("position", t.position,
					            			  "space", Space.World,
					            			"duration", 1f,
					            "easetype", iTween.EaseType.easeOutBounce));
					Destroy(item.pocketable.gameObject, 0.1f);
					return true;
				}
				Debug.Log(string.Format("{0} {1}", t, t.childCount));
			}
			return false;
		}

		                                                                    
		public bool PickUp(InterestPoint thing) {
			Debug.Log("Checking thing");
			//Can thing be placed in inventory
			if (thing.pocketable == null || thing.pocketable.version != Pocketable.InstanceType.WORLD)
				return false;
			Debug.Log("Thing of right sort");
			return placeInEmptySlot(thing.tag == "Battery" ? batteryPositions : inventoryPositions, thing);
		}

		public GameObject Drop(GameObject thing) {
			return Drop(thing, true);
		}

		public GameObject Drop(GameObject thing, bool getCorresponding) {
			foreach (Transform t in (thing.tag == "Battery" ? batteryPositions : inventoryPositions)) {
				if (thing.transform.IsChildOf(t)) {
					Destroy(thing.gameObject, 0.1f);
					return getCorresponding ? thing.GetComponent<Pocketable>().GetCorresponding() : null;
				}
			}
			return null;
		}

		public void Using(GameObject thing) {
			if (_using == null) {
				_using = thing;
				_using.GetComponent<UnityEngine.UI.Button>().enabled = false;
				Screen.showCursor = false;
			}
		}

		public void StopUsing() {
			if (_using) {
				_using.transform.localPosition = Vector3.zero;
				_using.GetComponent<UnityEngine.UI.Button>().enabled = true;
				_using = null;
				Screen.showCursor = true;
			}
		}
	}

}