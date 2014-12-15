using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public bool moveable = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ArrivedAt(InterestPoint pt) {
		gameObject.layer = pt.gameObject.layer;
	}
}
