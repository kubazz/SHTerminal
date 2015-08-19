using System;
using System.Collections.Generic;
using UnityEngine;

/*
struct scrollmessage{
	public SHGUIview view;
	public int height;
	public float delay;

	public scrollmessage (SHGUIview View, int Height, float Delay){
		view = View;
		height = Height;
		delay = Delay;
	}
}
*/

public class APPhackconsole: APPscrollconsole
{
	private SHGUIprogressbar progress;
	public APPhackconsole ()
	{
		progress = new SHGUIprogressbar(30, 10, 20, "TYPE-TO-HACK", "");
		AddSubView(progress);
	}

	float nointeractiontime = 0f;
	float tickertimer = 1f;
	public override void Update(){
		base.Update ();

		this.RemoveView(progress);
		AddSubView(progress);
		progress.currentProgress = 1f - GetProgress();
		delay = 20;

		nointeractiontime += Time.unscaledDeltaTime;
		tickertimer -= Time.unscaledDeltaTime;
		if (tickertimer < 0 && nointeractiontime > 1f){
			KillMessages();
			tickertimer = 0.04f;
		}
	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;

		if (key == SHGUIinput.any){
			delay = -1f;
			tickertimer = 0.04f;
			nointeractiontime = 0;
		}

		
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
