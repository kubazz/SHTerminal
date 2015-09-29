using System;
using System.Collections.Generic;
using UnityEngine;


public class APPbirthday: SHGUIappbase
{
	public APPbirthday (): base("URODZINY-ALICJI-APP-by-piotr")
	{
		SHGUIsprite s = new SHGUIsprite ();
		s.AddFramesFromFile ("birthdaycake", 14);
		SHGUI.current.AddViewOnTop (s);
		s.x = 32 - 12;
		s.y = 3;

		s.loops = true;
		s.animationSpeed = .4f;

		AddSubView (s);
		phrase = "WSZYSTKIEGO NAJLEPSZEGO ALICJA !";
	}

	string phrase;

	public override void Update(){
		base.Update();



	}

	public override void Redraw(int offx, int offy){
		base.Redraw(offx, offy);

		int X = 32 - (int)(phrase.Length / 2);
		int Y = 19;
		for (int i = 0; i < phrase.Length; ++i){
			int offY =(int)( Mathf.Sin(UnityEngine.Time.realtimeSinceStartup * 5f + i / 2) * 2);
			SHGUI.current.SetPixelFront(phrase[i], X + i, Y + offY, 'w');
		}

	
	}
}