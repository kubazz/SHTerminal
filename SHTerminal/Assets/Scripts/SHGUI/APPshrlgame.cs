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

		queue.Add (new APPshrltalk ());
		queue.Add (new APPshrltalk ());
		queue.Add (new APPshrltalk ());
		queue.Add (new APPshrl());
		

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


