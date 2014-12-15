using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


	public bool moveable = true;

	public Room room;
	private InterestPoint location;
	private InterestPoint target;

	// Use this for initialization
	void Start () {
		room = gameObject.GetComponentInParent<Room>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ArrivedAt(InterestPoint pt) {
		gameObject.layer = pt.gameObject.layer;
		room = pt.room;
	}

	public void SetInterest(InterestPoint pt) {
		target = pt;
	}
}
