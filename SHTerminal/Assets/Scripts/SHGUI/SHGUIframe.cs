
using System;
using System.Collections.Generic;


public class SHGUIframe: SHGUIview
{
	/*
	int startx;
	int starty;
	int endx;
	int endy;
	char col;
	*/

	public SHGUIframe (int Startx, int Starty, int Endx, int Endy, char Col)
	{
		Init ();

		SetColor (Col);

		/*
		startx = Startx;
		starty = Starty;
		endx = Endx;
		endy = Endy;
		col = Col;
		*/

		AddSubView (new SHGUIline (Startx, Endx, Starty, true, Col).SetStyle("┌─┐"));
		AddSubView (new SHGUIline (Startx, Endx, Endy, true, Col).SetStyle("└─┘"));
		
		AddSubView (new SHGUIline (Starty, Endy, Startx, false, Col).SetStyle("┌│└"));
		AddSubView (new SHGUIline (Starty, Endy, Endx, false, Col).SetStyle("┐│┘"));

	}

	/*
	public override void Redraw (int offx, int offy)
	{
	

		SHGUI.current.DrawRect (startx + offx, starty + offy, endx + offx, endy + offy, col, fade);

	}
*/

}


