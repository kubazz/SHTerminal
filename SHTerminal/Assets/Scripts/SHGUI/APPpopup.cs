
using System;
using System.Collections.Generic;
using System.Text;


public class APPpopup: SHGUIview
{

	public APPpopup ()
	{
		Init ();

		StringBuilder c = new StringBuilder ();
		for (int i = 0; i < 60; ++i){
			c.Append("POP!");
		}

		int w = 17 + (int)(UnityEngine.Random.Range(0, 6));
		int h = 4 + (int)(UnityEngine.Random.Range(0, 6));
		AddSubView (new SHGUIrect (0, 0, w, h));
		AddSubView (new SHGUIframe (0, 0, w, h, 'z'));
		AddSubView(new SHGUItext(c.ToString(), 1, 1, 'z').BreakTextForLineLength(w - 1).CutTextForMaxLines(h - 2));

		string c1 = LocalizationManager.Instance.GetLocalized ("POPUP_INPUT");
		AddSubView(new SHGUItext(c1, 1, h - 2, 'w').BreakTextForLineLength(w - 1).CutTextForMaxLines(h - 2));

		x = UnityEngine.Random.Range (0, SHGUI.current.resolutionX - w);
		y = UnityEngine.Random.Range (0, SHGUI.current.resolutionY - h);

		dontDrawViewsBelow = false;
	}

	void LaunchNext(){
		SHGUI.current.LaunchAppByName ("APPpopup");
	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;
		
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView ();
		
		if (key == SHGUIinput.enter) {
		//	SHGUI.current.PopView ();
			LaunchNext();
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


