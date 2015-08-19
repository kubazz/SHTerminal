
using System;
using System.Collections.Generic;
using System.Text;


public class APPrestrict: SHGUItempview
{
	float timer;
	int counter = 0;

	public APPrestrict ():base(2f)
	{
		/*
		string txt = "RESTRICTED ACCESS";
		x = (int)(SHGUI.current.resolutionX / 2) - (int)(txt.Length / 2);
		y = 12 - 3;

		this.allowCursorDraw = false;
		this.dontDrawViewsBelow = false;
		this.AddSubView(new SHGUIframe(0, 0, txt.Length + 1, 2, 'r'));
		this.AddSubView(new SHGUItext(txt, 1, 1, 'r'));
		*/

		SHGUI.current.RestrictedAccess = true;
		//interactable = false;
	}

}


