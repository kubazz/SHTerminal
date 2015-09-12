using System;
using UnityEngine;
using System.Collections.Generic;

public class APPshrlgame: SHGUIview
{
	int currentline = 0;
	SHGUIview lastView;
	List<SHGUIview> queue;

	public APPshrlgame()
	{
		queue = new List<SHGUIview> ();


		//AddIntermezzoMid ();
		//AddIntermezzoOut ();

		/*
		AddIntro ();
		queue.Add (new APPshrltalk ());

		SHGUItempview t = new SHGUItempview (1.5f);
		t.AddSubView (new APPshrlmenu (false));
		queue.Add (t);

		AddDownload ();
		queue.Add (new APPshrlmenu ());
		*/
		AddIntermezzoIn ();
		
		queue.Add (new APPshrl("shrlTESTLEVEL"));
		queue.Add (new APPshrltalk ());

		allowCursorDraw = false;
	}

	void AddIntro(){
		SHGUIview a = new SHGUItempview (3f);
		SHGUIview b = new SHGUIblinkview (.3f);
		a.AddSubView (b);
		string str = "NEW SMS";
		b.AddSubView(new SHGUItext(str, (int)(SHGUI.current.resolutionX / 2) - (int)(str.Length / 2), (int)(SHGUI.current.resolutionY / 2) - 1, 'w'));

		queue.Add (a);
	}

	void AddDownload(){
		string str = "DOWNLOAD COMPLETE";
		

		SHGUIview a1 = new SHGUItempview (2.5f);
		
		a1.AddSubView(new SHGUIframe((int)(SHGUI.current.resolutionX / 2) - (int)(str.Length / 2) - 1, (int)(SHGUI.current.resolutionY / 2) - 2, (int)(SHGUI.current.resolutionX / 2) + (int)(str.Length / 2) + 1, (int)(SHGUI.current.resolutionY / 2), 'z'));
		SHGUIprompter prom = new SHGUIprompter ((int)(SHGUI.current.resolutionX / 2) - (int)(str.Length / 2), (int)(SHGUI.current.resolutionY / 2) - 1, 'z');
		prom.SetInput ("^Cz████████████^W1█^W1████");
		a1.AddSubView (prom);

		queue.Add (a1);

		SHGUIview a = new SHGUItempview (2.5f);
		SHGUIview b = new SHGUIblinkview (.2f);
		a.AddSubView (b);

		b.AddSubView(new SHGUIframe((int)(SHGUI.current.resolutionX / 2) - (int)(str.Length / 2) - 1, (int)(SHGUI.current.resolutionY / 2) - 2, (int)(SHGUI.current.resolutionX / 2) + (int)(str.Length / 2) + 1, (int)(SHGUI.current.resolutionY / 2), 'z'));
		b.AddSubView(new SHGUItext(str, (int)(SHGUI.current.resolutionX / 2) - (int)(str.Length / 2), (int)(SHGUI.current.resolutionY / 2) - 1, 'w'));
		
		queue.Add (a);
	}

	void AddIntermezzoIn(){
		AddIntermezzoInternal (3.5f, false, false);
	}

	
	void AddIntermezzoOut(){
		AddIntermezzoInternal (3.5f, false, true);
	}

	void AddIntermezzoMid(){
		AddIntermezzoInternal (3.5f, false, true, true);
		
	}

	void AddIntermezzoInternal(float time = 3.5f, bool red = false, bool reverse = false, bool onlypulse = false){
		int x = (SHGUI.current.resolutionX / 2);
		int y = (SHGUI.current.resolutionY / 2);
		SHGUIview a = new SHGUItempview (time);

		var rect = new SHGUItvscramb (x, y, 32, 12);
		if (red)
			rect.color = 'r';
		if (reverse)
			rect.SetReverse ();

		if (onlypulse)
			rect.SetOnlyPulse ();

		a.AddSubView(rect);

		queue.Add (a);
	}

	public void AddViewToGame(SHGUIview v){
		queue.Add (v);
	}

	public override void Update(){
		base.Update();

		if (lastView == null || lastView.remove) {
			if (queue.Count > 0){
				AddSubView(queue[0]);
				lastView = queue[0];
				queue.RemoveAt(0);
			}
			else{
				Kill ();
			}
		}

	}

	public override void Redraw(int offx, int offy){

		base.Redraw (offx, offy);


	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;

		base.ReactToInputKeyboard (key);
		
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView ();
		
		if (key == SHGUIinput.enter) {
		}
	}
	
	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
	{	
		if (fadingOut)
			return;

		base.ReactToInputMouse (x, y, clicked, scroll);
		
		if (clicked)
			SHGUI.current.PopView ();
	}

}


