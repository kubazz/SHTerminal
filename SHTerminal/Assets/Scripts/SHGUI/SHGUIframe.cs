
using System;
using System.Collections.Generic;


public class SHGUIframe: SHGUIview
{
	SHGUIview[] lines;

	public SHGUIframe (int Startx, int Starty, int Endx, int Endy, char Col)
	{
		Init ();

		SetColor (Col);

		lines = new SHGUIline[4];

		lines[0] = AddSubView (new SHGUIline (Startx, Endx, Starty, true, Col).SetStyle("┌─┐"));
		lines[1] = AddSubView (new SHGUIline (Startx, Endx, Endy, true, Col).SetStyle("└─┘"));
		
		lines[2] = AddSubView (new SHGUIline (Starty, Endy, Startx, false, Col).SetStyle("┌│└"));
		lines[3] = AddSubView (new SHGUIline (Starty, Endy, Endx, false, Col).SetStyle("┐│┘"));
	}

	public override SHGUIview SetColor(char color){
		this.color = color;

		if (lines != null) {
			for (int i = 0; i < lines.Length; ++i) {
				lines [i].color = color;
			}
		}

		return this;
	}

	/*
	public override void Redraw (int offx, int offy)
	{
	

		SHGUI.current.DrawRect (startx + offx, starty + offy, endx + offx, endy + offy, col, fade);

	}
*/

}


