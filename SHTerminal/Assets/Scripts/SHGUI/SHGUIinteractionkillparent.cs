
using System;
using System.Collections.Generic;


public class SHGUIinteractionkillparent: SHGUIview
{


	public SHGUIinteractionkillparent ()
	{


	}

	public override void ReactToInputKeyboard (SHGUIinput key)
	{
		base.ReactToInputKeyboard (key);

		if (key != SHGUIinput.none) {
			if (parent != null){
				parent.Kill();
			}
		}
	}

	public override void ReactToInputMouse (int x, int y, bool clicked, SHGUIinput scroll)
	{
		base.ReactToInputMouse (x, y, clicked, scroll);

		if (clicked) {
			if (parent != null){
				parent.Kill();
			}
		}
	}


}


