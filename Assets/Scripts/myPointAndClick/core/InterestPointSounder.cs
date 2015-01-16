using UnityEngine;
using System.Collections;

public class InterestPointSounder : PointClick.InterestPoint {

	/// <summary>
	/// Average time between playing of audio clip
	/// </summary>
	public float interPlayTime = 0f;

	/// <summary>
	/// The fraction of interPlayTime around expected time to uniformly add randomness
	/// </summary>
	[Range(0, 1)]
	public float interPlayTimeVariance = 0f;

	[SerializeThis]
	private bool isSounding = true;

	[SerializeThis]
	private float nextPlay = 0f;

	protected override void Update ()
	{
		base.Update ();

		if (audio) {
			if (isSounding && !audio.isPlaying) {
				nextPlay = PointClick.LevelManager.PlayTime + interPlayTime + Random.Range(-interPlayTime * interPlayTimeVariance, interPlayTime * interPlayTimeVariance);
				isSounding = false;
			} else if (!isSounding && nextPlay < PointClick.LevelManager.PlayTime) {
				isSounding = true;
				audio.Play();
			}
		} else 
			isSounding = false;
	}

	public override void Action (PointClick.PlayerController player)
	{
		if (isSounding)
			base.Action (player);
	}

	public override void Action (PointClick.PlayerController player, PointClick.InterestPoint interest)
	{
		if (isSounding)
			base.Action (player, interest);
	}
}
