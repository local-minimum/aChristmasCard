using UnityEngine;
using System.Collections.Generic;

public class InterestPointTrigger : InterestPoint {

	public bool loop = false;
	public bool loopBounce = false;

	public List<string> triggerSequence = new List<string>();

	public Animator animator;

	private int nextTrigger = 0;
	private int addToNext = 1;


	new void Start() {
		base.Start();
		if (animator == null)
			animator = GetComponentInParent<Animator>();
	}

	public override void Action (PlayerController player)
	{
		if (nextTrigger >= triggerSequence.Count) {
			if (loop) {
				if (loopBounce) {
					addToNext *= -1;
					nextTrigger += 2 * addToNext;
				} else {
					nextTrigger = 0;
				}
			} else {
				return;
			}
		}

		animator.SetTrigger(triggerSequence[nextTrigger]);
		SpecificAction(player);
		nextTrigger += addToNext;
	}
}
