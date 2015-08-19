
using System.Collections.Generic;
using UnityEngine;


public class SHGUIqueuepop: SHGUIview
{
	public SHGUIqueuepop ()
	{

	}

	public override void Update ()
	{
		base.Update ();

		SHGUI.current.PopViewFromQueue ();
		KillInstant ();
	}
}


