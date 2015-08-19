
using System;
using System.Collections.Generic;


public class SHGUIinteractionparentpunch: SHGUIview
{


	public SHGUIinteractionparentpunch ()
	{


	}

	public override void ReactToInputKeyboard (SHGUIinput key)
	{
		base.ReactToInputKeyboard (key);

		if (key != SHGUIinput.none) {
			if (parent != null){
				parent.PunchIn(.8f);
			}
		}
	}

	public override void ReactToInputMouse (int x, int y, bool clicked, SHGUIinput scroll)
	{
		base.ReactToInputMouse (x, y, clicked, scroll);

		if (clicked) {
			if (parent != null){
				parent.overrideFadeInSpeed = 1f;
				parent.overrideFadeOutSpeed = 1f;
				parent.ForceFadeRecursive(.5f);

				if (parent.parent != null){
					parent.parent.overrideFadeInSpeed = 1f;
					parent.parent.overrideFadeOutSpeed = 1f;
					parent.parent.ForceFadeRecursive(.5f);
				}
			}
		}
	}


}


