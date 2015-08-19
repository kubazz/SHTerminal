
using System;
using System.Collections.Generic;


public class SHGUIline: SHGUIview
{
	string style = "+-+";
	int startpos;
	int endpos;
	int colpos;
	bool horizontal;

	public SHGUIline (int Startpos, int Endpos, int Colpos, bool Horizontal, char Col)
	{
		Init ();

		startpos = Startpos;
		endpos = Endpos;
		colpos = Colpos;
		horizontal = Horizontal;
		SetColor (Col);
		
	}

	public SHGUIline SetStyle(String Style){
		style = Style;
		return this;
	}
	
	public override void Redraw (int offx, int offy)
	{
		if (horizontal)
			SHGUI.current.DrawLine (style, startpos + offx, endpos + offx, colpos + offy, horizontal, color, fade);
		else
			SHGUI.current.DrawLine (style, startpos + offy, endpos + offy, colpos + offx, horizontal, color, fade);
			
	}
	
}


