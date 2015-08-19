
using System;

public class APPprompter: SHGUIview
{
	public APPprompter ()
	{
		SHGUIprompter p = AddSubView(new SHGUIprompter(1, 1, 'w')) as SHGUIprompter;

		p.SetInput ("What did you see? Everybody gets something different.\n" +
		            "There was this car crash and shooting, then I was a jail guard and all the cells started opening, prisoners running at me. I shot them.\n" + 
					"Man, I know this jail. I was the one who opened the cells!\n" + 
					"So it's like co-op?\n" +
					"I'm not sure. I just got this thing from a friend. I'm going back in, bye.\n" +
					"Me too, see ya.\n");

		p.maxLineLength = SHGUI.current.resolutionX - 10;

	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;
		
		if (key == SHGUIinput.esc)
			Kill ();
		
		if (key == SHGUIinput.enter)
			Kill ();
	}

    public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
    {	
		if (fadingOut)
			return;
		
		if (clicked)
			Kill ();
	}
}

