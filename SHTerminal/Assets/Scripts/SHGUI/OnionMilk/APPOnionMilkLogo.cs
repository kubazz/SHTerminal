using UnityEngine;
using System.Collections;

public class APPOnionMilkLogo : SHGUIappbase {
	//Private
	string[]	onionMilkLogo	= new string[18] {
"    ____",
"   |    -----_____",
"   |____          |",
"   /    -----_____|",
"  /              / \\",
" /              /   |",
"|___           /    |",
"|   -----_____/     |",
"|            |      |",
"|            |      |",
"|            |      |",
"|  Onion     |      |",
"|     Milk   |      |",
"|            |      |",
"|            |      |",
"|            |      |",
"|___         |     _|",
"    -----____|__---"
	};

/*

*/
	//Public
	public APPOnionMilkLogo()
		:	base("onionmilk_logo-v6.6.6.23-by-onionmilk")
	{
		
	}

	public override void Redraw(int offx, int offy) {
		base.Redraw(offx, offy);

		//SHGUI.current.SetPixelFront(char character, int posX, int posY, char color);
		for(int y = 0; y < onionMilkLogo.Length; ++y)
			for(int x = 0; x < onionMilkLogo[y].Length; ++x)
				SHGUI.current.SetPixelFront(onionMilkLogo[y][x], 20 + x, 3 + y, 'w');
			//--
		//--
	}

	public override void ReactToInputKeyboard(SHGUIinput key) {
		//if (key == SHGUIinput.enter) {
			//Wciśnięto enter
		//}
		
		if (key == SHGUIinput.esc || key == SHGUIinput.enter)
			SHGUI.current.PopView();
		//--
	}
}
