using UnityEngine;
using System.Collections.Generic;

public class InterestPointTrigger : InterestPoint {

	public bool loop = false;
	public bool loopBounce = false;

	public List<string> triggerSequence = new List<string>();

	public Animator animator;

	private int nextTrigger = 0;
	private int addToNext = 1;


	void Start() {
		if (animator == null)
			animator = GetComponentInParent<Animator>();
	}

	public override void Action (PlayerController player)
	{
		if (nextTrigger >= triggerSequence.Count) {
			if (loop) {
				addToNext *= -1;
				nextTrigger += 2 * addToNext;
			} else {
				return;
			}
		}

		animator.SetTrigger(triggerSequence[nextTrigger]);

		nextTrigger += addToNext;
	}
}
