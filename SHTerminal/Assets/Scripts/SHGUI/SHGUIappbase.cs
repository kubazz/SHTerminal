
using System;
using System.Collections.Generic;
using UnityEngine;


public class SHGUIappbase: SHGUIview
{
	public SHGUItext APPLABEL;
	public SHGUItext APPINSTRUCTION;
	public SHGUIframe APPFRAME;

	public SHGUIappbase (string title)
	{
		Init ();

		AddSubView (new SHGUIrect (0, 0, SHGUI.current.resolutionX - 1, SHGUI.current.resolutionY - 1));
		APPFRAME = AddSubView (new SHGUIframe (0, 0, SHGUI.current.resolutionX - 1, SHGUI.current.resolutionY - 1, 'z')) as SHGUIframe;
		APPLABEL = AddSubView (new SHGUItext (title, 3, 0, 'w')) as SHGUItext;
		APPINSTRUCTION = AddSubView (new SHGUItext (LocalizationManager.Instance.GetLocalized("QUIT_APP_INPUT"), SHGUI.current.resolutionX - 5, SHGUI.current.resolutionY - 1, 'z').GoFromRight()) as SHGUItext;

		allowCursorDraw = false;

		overrideFadeInSpeed = .35f;
		overrideFadeOutSpeed = .5f;
	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;

		if (key == SHGUIinput.esc)
			SHGUI.current.PopView ();

		if (key == SHGUIinput.enter)
			SHGUI.current.PopView ();
	}

    public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
    {	
		if (fadingOut)
			return;

		if (clicked)
			SHGUI.current.PopView ();
	}

    protected void MoveFrameToTop()
    {
        children.Remove(APPFRAME);
        children.Add(APPFRAME);
        children.Remove(APPLABEL);
        children.Add(APPLABEL);
    }
}


