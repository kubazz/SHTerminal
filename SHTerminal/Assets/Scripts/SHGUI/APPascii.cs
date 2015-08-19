
using System;
using UnityEngine;

public class APPascii: SHGUIappbase
{
	public APPascii (string label, string artname, bool centered): base("")
	{
		Init ();

		dontDrawViewsBelow = false;
		AddSubView (new SHGUIrect (0, 0, SHGUI.current.resolutionX - 1, SHGUI.current.resolutionY - 1));
		if (!centered)
			(AddSubView (new SHGUItext (SHGUI.current.GetASCIIartByName(artname), 1, 1, 'w')) as SHGUItext).CutTextForLineLength(SHGUI.current.resolutionX - 2).CutTextForMaxLines(SHGUI.current.resolutionY - 3);
		else
			AddSubView (SHGUI.current.GetCenteredAsciiArt(artname));
			
		AddSubView (new SHGUIframe (0, 0, SHGUI.current.resolutionX - 1, SHGUI.current.resolutionY - 1, 'z'));
		AddSubView (new SHGUItext ( "ASCIIview:-" + label, 3, 0, 'w'));
		APPINSTRUCTION = AddSubView (new SHGUItext (LocalizationManager.Instance.GetLocalized("QUIT_APP_INPUT"), SHGUI.current.resolutionX - 5, SHGUI.current.resolutionY - 1, 'w').GoFromRight()) as SHGUItext;
		
		allowCursorDraw = false;
	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;
		
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView ();
		
		if (key == SHGUIinput.enter)
			SHGUI.current.PopView ();
	}
	
	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll){	
		if (fadingOut)
			return;
		
		if (clicked)
			SHGUI.current.PopView ();
	}
}

