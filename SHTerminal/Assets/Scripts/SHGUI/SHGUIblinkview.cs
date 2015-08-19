using UnityEngine;
using System.Collections;

public class SHGUIblinkview : SHGUIview {

	private float timer = 0;
	public float blinkTime = .2f;
	private bool clicker = false;

	public SHGUIblinkview (float blinkTime) {
		timer = blinkTime;
		this.blinkTime = blinkTime;
	}

	public SHGUIblinkview SetFlipped(){
		hidden = !hidden;
		return this;
	}
	
	public override void Update () {
		base.Update ();

		timer -= Time.unscaledDeltaTime;
		if (timer < 0) {
			timer = blinkTime;
			hidden = !hidden;
		}
	}
}
