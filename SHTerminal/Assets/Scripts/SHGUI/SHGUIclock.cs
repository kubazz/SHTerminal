
using System;
using System.Collections.Generic;
using System.Text;


public class SHGUIclock: SHGUIview
{
	DateTime lastDate;
	string clock = "";

	public SHGUIclock (int X, int Y, char col): base()
	{
		x = X;
		y = Y;

		SetColor (col);

		lastDate = DateTime.Now;
		RebuildClockString ();
		
	}


	public override void Update ()
	{
		base.Update ();
		if (DateTime.Now.Hour != lastDate.Hour || DateTime.Now.Minute != lastDate.Minute) {
			PunchIn();
			RebuildClockString();
		}

		lastDate = DateTime.Now;
	}

	void RebuildClockString(){
		StringBuilder str = new StringBuilder();
		//str.Append ("[");
		str.Append(Fill(DateTime.Now.Hour + ""));
		str.Append (":");
		str.Append (Fill (DateTime.Now.Minute+ ""));
		//str.Append ("]");
		clock = str.ToString();
	}

	public override void Redraw (int offx, int offy)
	{
		if (hidden)
			return;
		SHGUI.current.DrawText (clock, x + offx - clock.Length, y + offy, color, fade);
	}

	string Fill(string s, int len = 2){
		while (s.Length < len) {
			s = "0" + s;
		}
		return s;
	}
	
}


