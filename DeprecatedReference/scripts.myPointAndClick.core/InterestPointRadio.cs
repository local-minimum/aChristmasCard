using UnityEngine;
using System.Collections.Generic;

namespace PointClick.Old {

	public class InterestPointRadio : InterestPointRestricted {
		 
		public AudioSource audioSource;

		public AudioClip changeStation;
		public List<AudioClip> stations = new List<AudioClip>();
		public List<string> words = new List<string>();

		private int station = -1;
		private bool allowNext = false;

		public bool hasEnergy = false;

		new void Start() {
			base.Start();
			while (words.Count < stations.Count)
				words.Add("");
		}

		public override void SpecificAction (PlayerController player)
		{
			if (!hasEnergy) {
				player.NeedEnergy();
				return;
			}

			if (!allowNext)
				return;

			station++;
			if (station > stations.Count)
				station = -1;

			if (station < 0)
				audioSource.Stop();
			else {
				audioSource.loop = true;
				allowNext = false;
				audioSource.clip = stations[station];
				audioSource.PlayOneShot(changeStation);
				StartCoroutine(newStation(player));
			}
		}

		IEnumerator<WaitForSeconds> newStation(PlayerController player) {
			yield return new WaitForSeconds(changeStation.length * 0.9f);
			audioSource.Play();
			yield return new WaitForSeconds(1f);
			player.Learn(words[station]);
			allowNext = true;
		}

		public override ApplyResults Apply (PlayerController player, GameObject tool)
		{
			if (!hasEnergy && tool.tag == "Battery") {
				if (player.target != restriction)
					return ApplyResults.REQUEST_ACTION;
				player.Drop(tool, false);
				hasEnergy = true;
				return ApplyResults.ACCEPTED;
			}
			return ApplyResults.REFUSED;
		}
		
	}
}