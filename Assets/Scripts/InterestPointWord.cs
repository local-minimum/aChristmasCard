using UnityEngine;
using System.Collections;

public class InterestPointWord : InterestPoint {

	public string word;

	new public void Action(PlayerController player) {
		player.Learn(word);
	}

}
