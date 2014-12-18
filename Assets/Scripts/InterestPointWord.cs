using UnityEngine;
using System.Collections;

public class InterestPointWord : InterestPoint {

	public string word;

	public override void Action(PlayerController player) {
		player.Learn(word);
	}

}
