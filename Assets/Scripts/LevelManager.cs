using UnityEngine;
using System.Collections.Generic;

public class LevelManager : Singleton<LevelManager> {

	public MouseHover hoverCanvas;
	private PlayerController _player;
	public Camera mainCamera;

	public PlayerController player {
		get {
			if (_player == null)
				_player = FindObjectOfType<PlayerController>();
			return _player;
		}
	}

	// Use this for initialization
	void Start () {
		if (!mainCamera)
			mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
