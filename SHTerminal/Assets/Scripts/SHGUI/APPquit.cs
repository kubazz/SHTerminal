using System;
using UnityEngine;

public class APPquit: SHGUIview
{
	private SHGUIview quitInstruction;

	public APPquit ()
	{
		dontDrawViewsBelow = false;

		int width = 30;
		int textoff = 7;

		SHGUIview container = AddSubView(new SHGUIview());
		container.x = (int)(SHGUI.current.resolutionX / 2) - (int)(width / 2);
		container.y = (int)(SHGUI.current.resolutionY / 2) - 4;

		container.AddSubView(new SHGUIrect(0, 0, width, 4));
		container.AddSubView(new SHGUIframe(0, 0, width, 4, 'r'));
		container.AddSubView(new SHGUItext("SHUTTING DOWN", 1 + textoff, 2, 'r'));
		container.AddSubView(new SHGUILoadingIndicator(16 + textoff, 2, .05f, 'r'));

		quitInstruction = container.AddSubView(new SHGUItext(LocalizationManager.Instance.GetLocalized("CANCEL_INPUT"), width - 3, 4, 'r').GoFromRight());

	}

	float timer = 2.5f;
	public override void Update(){
		base.Update();

		if (SHGUI.current.RestrictedAccess) {
			quitInstruction.hidden = true;
		}
		
		if (fade < .99f){
			return;
		}

		timer -= Time.unscaledDeltaTime;

		if (timer < 0 && !fadingOut){
			if (!SHGUI.current.RestrictedAccess){
				Debug.Log("QUITTING VIA APPquit");
				Application.Quit();
				Kill ();
			}
			else{
				Kill ();
				SHGUI.current.AddViewOnTop(new APPkill());
			}
		}

	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;
		
		if (key == SHGUIinput.esc && !SHGUI.current.RestrictedAccess)
			SHGUI.current.PopView ();
	}
}


