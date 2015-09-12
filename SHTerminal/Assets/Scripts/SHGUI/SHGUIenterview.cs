using UnityEngine;
using System.Collections;

public class SHGUIenterview : SHGUIview {

	float delay = 0;
	public SHGUIenterview (float delay) {
		this.delay = delay;
	}

	public override void Update () {
		base.Update ();

		delay -= Time.unscaledDeltaTime;
	}

	public virtual void ReactToInputKeyboard(SHGUIinput key){
		base.ReactToInputKeyboard (key);

		if (delay < 0 && key == SHGUIinput.enter) {
			Kill ();
		}

	}
	
	public virtual void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll){
		base.ReactToInputMouse (x, y, clicked, scroll);

		if (delay < 0 && clicked) {
			Kill ();
		}
	}
}
