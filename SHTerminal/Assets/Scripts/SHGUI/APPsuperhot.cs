using System;
using UnityEngine;
using System.Collections.Generic;


public class APPsuperhot: SHGUIappbase
{
	SHGUItext super;
	SHGUItext hot;
	
	public APPsuperhot (): base("")
	{
		//children = new List<SHGUIview>();
		super = SHGUI.current.GetCenteredAsciiArt("supersmall"); 
		AddSubView(super);
		super.y -= 1;
		super.hidden = true;

		hot = SHGUI.current.GetCenteredAsciiArt("hotsmall");
		AddSubView(hot);
		hot.hidden = true;
		hot.y -= 1;
		//hot.PunchIn(1f);
		//super.PunchIn(1f);
	}

	bool ticker = false;
	float waiter = 0;
	public override void Update(){

		waiter -= Time.unscaledDeltaTime;

		if (waiter < 0){
			waiter = .5f;

			if (!ticker){
				super.hidden = false;
				hot.hidden = true;

				super.PunchIn(.8f);

			}
			else{
				super.hidden = true;
				hot.hidden = false;
				
				hot.PunchIn(.8f);
			}
		
			ticker = !ticker;
		}
		base.Update();
	}
}

