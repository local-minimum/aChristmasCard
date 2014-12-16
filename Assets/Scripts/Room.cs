using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Room : MonoBehaviour {

	public LayerMask interestPointLayers;

	private List<InterestPoint> interactions = new List<InterestPoint>();
	private List<InterestPoint> walkingPoints = new List<InterestPoint>();

	public bool playerInRoom = true;

	// Use this for initialization
	void Awake () {
		interactions.AddRange(gameObject.GetComponentsInChildren<InterestPoint>());
		walkingPoints.AddRange(interactions.Where(ip => ip.walkingPoint));

	}
	
	// Update is called once per frame
	void Update() {
		if (playerInRoom && LevelManager.Instance.player.moveable)
			IdentifyInteraction();
	}

	public InterestPoint GetPointClosestTo(Vector3 pos) {
		return interactions.OrderBy(ip => Vector3.Distance(pos, ip.transform.position)).First();
	}

	public InterestPoint GetWalkingPointClosestTo(Vector3 pos) {
		InterestPoint pt = GetPointClosestTo(pos);
		if (pt && !pt.walkingPoint)
			pt = pt.viewedFrom;
		return pt;
	}

	void IdentifyInteraction () {
		Ray ray = LevelManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (!Physics.Raycast(ray, out hit, 30f, interestPointLayers))
			return;

		InterestPoint pt = GetWalkingPointClosestTo(hit.point);

		if (Input.GetMouseButtonDown(0)) {
			LevelManager.Instance.player.SetInterest(pt);
		} else {
			LevelManager.Instance.hoverCanvas.SetHoverPoint(pt);
		}

	}
}
