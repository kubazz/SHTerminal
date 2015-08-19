
using System;

public class APPkill: SHGUIview
{
	public APPkill ()
	{
		dontDrawViewsBelow = false;
	}


	public override void Update(){

		if (fade > 0.99f && !fadingOut) {
			SHGUI.current.KillAll ();
			KillInstant();
		}


		base.Update ();
		
	}
}

