using UnityEngine;
using System.Collections.Generic;

public class LevelManager : Singleton<LevelManager> {

	public MouseHover hoverCanvas;

	[Range(0,1)]
	public float cameraSmoothness = 0.5f;

	private PlayerController _player;
	public Camera mainCamera;

	public PlayerController player {
		get {
			if (_player == null)
				_player = FindObjectOfType<PlayerController>();
			return _player;
		}
	}

	public float plaayTime {
		get {
			//TODO; add pauesefeature
			return Time.timeSinceLevelLoad;
		}
	}
	
	private bool _uiView = false;
	
	public bool uiView {
		get {
			return _uiView;
		}
		set {
			//TODO: Remove hover effects
			_uiView = value;
		}
	}
	// Use this for initialization
	void Start () {
		if (!mainCamera)
			mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		if (!uiView)
			mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, player.room.cameraPosition, cameraSmoothness);

	}


}
