using System;
using UnityEngine;
using System.Collections.Generic;

public class APPshrlmenu: SHGUIview
{
	int currentline = 0;
	int currentrow = 2;

	bool showfirst = true;
	public APPshrlmenu(bool showfirst = true)
	{
		this.showfirst = showfirst;
		x = (SHGUI.current.resolutionX / 2) - 16;
		y = (SHGUI.current.resolutionY / 2) - 6;

		currentline = 2;

		if (showfirst) AddFile ("shrl.exe");
		AddFile ("MYFILES", true);
		AddFile ("ARCHIVE", true);
		AddFile ("INFINITE", true);
		AddFile ("rektagon.exe");
		AddFile ("machinat.exe");
		AddFile ("tetraval.exe");
		AddFile ("choochoo.exe");
		
		//AddSubView (new SHGUIframe (0, 0, 32, 12, 'z'));
		//AddSubView (new SHGUItext ("faketerminal", 2, 0, 'z'));
	}

	void AddFile(string name, bool folder = false){
		while (name.Length < 12)
			name += " ";

		if (!folder)
			name += " │--file--  ";
		else
			name += " │-folder-  ";
			
		AddSubView(new SHGUItext("      " + name, 1, currentrow, 'w'));
		currentrow++;
	}

	public override void Update(){
		base.Update();

	}

	public override void Redraw(int offx, int offy){

		base.Redraw (offx, offy);

		for (int i = 7;i < 29; ++i) {
			SHGUI.current.SetPixelBack('█', i + x, currentline + y, 'r');
		}


	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;

		base.ReactToInputKeyboard (key);
		
		if (key == SHGUIinput.esc)
			Kill ();
		
		if (key == SHGUIinput.enter) {
			if (currentline != 2){
				currentline = 2;
				PunchIn(.5f);
			}
			else{
				if (showfirst)
					Kill ();
			}
			
		}

		if (key == SHGUIinput.up) {
			currentline--;
			if (currentline < 2)
				currentline = 2;
		} else if (key == SHGUIinput.down) {
			currentline++;
			if (currentline > currentrow - 1){
				currentline = currentrow - 1;
			}
		}
	}
	
	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
	{	

	}
}


