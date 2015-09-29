using System;
using System.Collections.Generic;


public class SHGUIrect: SHGUIview
{
	int startx;
	int starty;
	int endx;
	int endy;
	char c;
	int mode;


	//mode is 0 for front, 1 for back, 2 for both
	public SHGUIrect (int Startx, int Starty, int Endx, int Endy, char Col = '0', char C = ' ', int Mode = 2)
	{
		Init ();

		startx = Startx;
		starty = Starty;
		endx = Endx;
		endy = Endy;
		SetColor (Col);
		c = C;

		mode = Mode;


	}

	public SHGUIrect SetChar(char fillChar){
		c = fillChar;
		return this;
	}
	
	public override void Redraw (int offx, int offy)
	{
		if (hidden)
			return;

		int newstartx = (int)(startx + endx * (1 - fade)) + offx;
		int newendx = (int)(endx * fade) + offx;
		int newstarty = (int)(starty + endy * (1 - fade)) + offy;
		int newendy = (int)(endy * fade) + offy;

		for (int X = newstartx; X <= newendx; X++) {
			for (int Y = newstarty; Y <= newendy; Y++) {
				if (mode == 0 || mode == 2) SHGUI.current.SetPixelFront(c, X, Y, color);
				if (mode == 1 || mode == 2) SHGUI.current.SetPixelBack(c, X, Y, color);
				
			}
		}

		base.Redraw (x + offx, y + offy);
	}

}


