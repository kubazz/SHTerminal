using System;
using UnityEngine;

public class APPinstantquit: SHGUIview
{
	public APPinstantquit ()
	{
		dontDrawViewsBelow = false;
		allowCursorDraw = false;
	}
	
	public override void Update(){
		base.Update();
		
		if (fade < .99f){
			return;
		}


		Debug.Log ("QUITTING VIA APPinstantquit");
		Application.Quit();

	}
}


