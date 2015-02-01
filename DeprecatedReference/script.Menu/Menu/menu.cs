using UnityEngine;
using System.Collections.Generic;

public class menu : MonoBehaviour {

	public GameObject resumeButton;
	private bool isPlaying = false;
	private string resetSaveName = "cleanStart.save";

	void Start() {
		resumeButton.SetActive(LevelSerializer.CanResume);
		LevelSerializer.SerializeLevelToFile(resetSaveName);
		Debug.Log("Number of saved games " + LevelSerializer.SavedGames.Count + "; items " + LevelSerializer.SavedGames[resetSaveName].Count);
	}

	public void Resume() {
		if (!isPlaying)
			LevelSerializer.Resume();
		isPlaying = true;
	}

	public void SaveProgress() {
		LevelSerializer.Checkpoint();
		resumeButton.SetActive(LevelSerializer.CanResume);
	}

	public void NewGame() {
		if (isPlaying) 
			LevelSerializer.LoadSavedLevelFromFile(resetSaveName);
		
		isPlaying = true;
		PointClick.LevelManager.Resume();
	}

	public void Reset() {
		PlayerPrefs.DeleteAll();
		LevelSerializer.LoadSavedLevelFromFile(resetSaveName);
		Start();

	}

	public void OnGUI() {
		if (Input.GetButtonDown("Menu"))
			GoMenu();
	}

	public void GoMenu() {
		PointClick.LevelManager.Pause();
		if (isPlaying)
			SaveProgress();
	}
}
