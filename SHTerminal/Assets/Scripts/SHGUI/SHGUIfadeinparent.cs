
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;


public class SHGUIfadeinparent: SHGUIview
{
	public SHGUIfadeinparent(){
	}

	public override void Update ()
	{
		base.Update ();

		if (parent != null && !fadingOut) {
			parent.fade += .005f;		}
	}

}


