using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class APPreadme: SHGUIview
{
	public APPreadme (string Label, string Content = "")
	{
		Init ();

		int centerX; int centerY; int halfwidth; int halfheight;
		centerX = (int)(SHGUI.current.resolutionX / 2);
		centerY = (int)(SHGUI.current.resolutionY / 2);
		halfwidth = 18;
		int lines = new SHGUItext (Content, 0, 0, 'g').CountLines ();
		halfheight = (int)(lines / 2 + 3);

		AddSubView (new SHGUIrect (0, 0, halfwidth * 2, halfheight * 2 - 1));
		AddSubView (new SHGUIframe (0, 0, halfwidth * 2, halfheight * 2 - 1, 'z'));
		AddSubView(new SHGUItext(Label, 2, 0, 'w'));
		SHGUItext t = AddSubView(new SHGUItext(Content, 2, 2, 'w').BreakCut(halfwidth * 2 - 4, halfheight * 2 - 5)) as SHGUItext;

		for (int i =0; i < t.text.Length;++i){
			//Debug.Log("["+t.text[i]+"]" + (int)(t.text[i]));
		}

		AddSubView(new SHGUItext("press-any-key", halfwidth * 2 - 14 - 2, halfheight * 2 - 1, 'w'));
		//AddSubView(new SHGUItext(c1, 1, h - 2, 'w');

		x = centerX - halfwidth;
		y = centerY - halfheight;

		dontDrawViewsBelow = false;
	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;
		
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView ();
		
		if (key == SHGUIinput.enter) {
			SHGUI.current.PopView ();
		//	LaunchNext();
		}
	}

    public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
    {	
		if (fadingOut)
			return;
		
		if (clicked) {
			//LaunchNext();
			SHGUI.current.PopView ();
		}
	}
	
}


