using System;
using System.Collections.Generic;
using System.Text;


public class APPcarpets: SHGUIappbase
{
	protected int startx;
	protected int starty;
	protected int endx;
	protected int endy;	
	
	//mode is 0 for front, 1 for back, 2 for both
	public APPcarpets ():base("carpet-generator-by-3.14")
	{		
		startx = 1;
		starty = 1;
		endx = SHGUI.current.resolutionX - 1;
		endy = SHGUI.current.resolutionY - 1;

		RegenerateCarpet();
	}

	string simplerScramble = "▀▄█";
	protected void RegenerateCarpet(){
		string scrambSource = "▀▄ █ ▌ ░ ▒ ▓ ■▪";
		StringBuilder str = new StringBuilder();
		int r = (int)UnityEngine.Random.Range(2, 30);
		for (int i = 0; i < r; ++i){
			str.Append(scrambSource [UnityEngine.Random.Range (0, scrambSource.Length)]);
		}
		simplerScramble = str.ToString();

		fade = .2f;
		fadingIn = true;
	}

	public override void Redraw (int offx, int offy)
	{
		if (hidden)
			return;
		
		base.Redraw (x, y);

		int newstartx = (int)(startx + endx * (1 - fade)) + offx;
		int newendx = (int)(endx * fade) + offx;
		int newstarty = (int)(starty + endy * (1 - fade)) + offy;
		int newendy = (int)(endy * fade) + offy;		
		
		int i = 0;
		for (int X = newstartx; X < newendx; X++) {
			for (int Y = newstarty; Y < newendy; Y++) {
				i++;
				if (UnityEngine.Random.value < fade){
					
					SHGUI.current.SetPixelFront(simplerScramble [i % simplerScramble.Length], X, Y, 'z');
				}
			}
		}
	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;
		
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView ();
		
		if (key == SHGUIinput.enter){
			RegenerateCarpet();
		}
	}
	
	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
	{	
		if (fadingOut)
			return;
		
		if (clicked){
			RegenerateCarpet();
		}
	}
	
}


