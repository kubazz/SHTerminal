using System;
using UnityEngine;
using System.Collections.Generic;

public class APPshrltalk: SHGUIview
{
	int currentline = 0;
	SHGUIprompter lastPrompter;
	List<string> prompterQueue;

	public APPshrltalk()
	{
		prompterQueue = new List<string> ();

		AddOtherLine ("u hav 2 check out thiz gaem!?!");
		AddMyLine ("i want new gaem?");
		AddOtherLine ("u want! iz call 'shrl', realistic to the max 20000%");
		AddMyLine ("is dis legal crack?");
		AddOtherLine ("this is crack, no legal");
		AddMyLine ("i don konw");
		AddOtherLine ("u try, is ver fun");
		AddMyLine ("ok, I want try, what do?");
		AddOtherLine ("i send files nao");
		AddMyLine ("awsum");
		AddOtherLine ("^W8");
		
		currentline = 1;

		AddSubView (new SHGUIframe (0, 0, 32, 12, 'z'));
		
	}

	public void AddMyLine(string line){
		prompterQueue.Add ("a:" + line);
	}

	public void AddOtherLine(string line){
		prompterQueue.Add ("b:" + line);
	}

	void AddMyPrompter(string line){
		lastPrompter = new SHGUIprompter(1, currentline, 'w');
		lastPrompter.SetInput(line);
		lastPrompter.y = currentline;
		lastPrompter.maxLineLength = 30;
		lastPrompter.maxSmartBreakOffset = 1;
		lastPrompter.SwitchToManualInputMode ();

		AddSubView (lastPrompter);
		
	}

	void AddOtherPrompter(string line){
		lastPrompter = new SHGUIprompter(1, currentline, 'z');
		lastPrompter.SetInput(line);
		lastPrompter.y = currentline;
		lastPrompter.maxLineLength = 30;
		lastPrompter.maxSmartBreakOffset = 1;
		
		

		AddSubView (lastPrompter);
	}

	public override void Update(){
		base.Update();

		if (lastPrompter == null || lastPrompter.IsFinished ()) {
			if (lastPrompter != null) currentline += lastPrompter.CountLines();
			
			if (prompterQueue.Count > 0){
				if (prompterQueue[0][0] == 'a'){
					AddMyPrompter(prompterQueue[0].Substring(2));
				}
				else if (prompterQueue[0][0] == 'b'){
					AddOtherPrompter(prompterQueue[0].Substring(2));
				}
				prompterQueue.RemoveAt(0);
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


