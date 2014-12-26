using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelManager : Singleton<LevelManager> {

	public MouseHover hoverCanvas;

	[Range(0,1)]
	public float cameraSmoothness = 0.5f;

	private PlayerController _player;
	public Camera mainCamera;

	public static PlayerController Player {
		get {
			return instance.player;
		}
	}

	public static Camera MainCamera {
		get {
			return instance.mainCamera;
		}
	}

//	public Texture2D mouseCursor;

	public PlayerController player {
		get {
			if (_player == null)
				_player = FindObjectOfType<PlayerController>();
			return _player;
		}
	}

	public float playTime {
		get {
			//TODO; add pauesefeature
			return Time.timeSinceLevelLoad;
		}
	}
	
	private HashSet<GameObject> _uiView = new HashSet<GameObject>();
	
	public bool uiView {
		get {
			return _uiView.Any();
		}

	}
	// Use this for initialization
	void Start () {
		if (!mainCamera)
			mainCamera = Camera.main;
//		Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.ForceSoftware);
	}
	
	// Update is called once per frame
	void Update () {
		if (uiView) {
			Screen.showCursor = true;
			return;
		} else 	if (player.cursor) {
			player.cursor.position = Input.mousePosition;
			Screen.showCursor = false;
		} else {
			Screen.showCursor = true;
		}


		if (player.moveable)
			mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, player.room.cameraPosition, cameraSmoothness);
		else
			mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, player.room.zoomPosition, cameraSmoothness);
	}

	public void SetUIFocus(GameObject ui) {
		_uiView.Add(ui);
	}

	public void RemoveUIFocus(GameObject ui) {
		_uiView.Remove(ui);
	}

	public bool UIhasFocus(GameObject ui) {
		return _uiView.Contains(ui);
	}

	public T UIwithFocusByType<T>() where T : UnityEngine.Component {
		GameObject uiGO = _uiView.Where(gO => gO.GetComponent<T>() != null).FirstOrDefault();
		Debug.Log(string.Format("Current UI focus ({0}): {1}", _uiView.Count(), string.Join(", ", _uiView.Select(gO => gO.name).ToArray())));
		if (uiGO != null) 
			return uiGO.GetComponent<T>();
		return null;
	}
}
