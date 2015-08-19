using System.Collection;
using UnityEngine;

public class test : SHGUIappbase {
	public test()
		:	base("Tytuł")
	{
		//APPLABEL.text	= "Tytuł";
	}

	public override void Update() {
		base.Update();

	}

	public override void Redraw(int offx, int offy) {
		base.Redraw(offx, offy);

		//SHGUI.current.SetPixelFront(char character, int posX, int posY, char color);
	}

	public override void ReactToInputKeyboard(SHGUIinput key) {
		if (key == SHGUIinput.enter) {
			//Wciśnięto enter
		}
		
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView();
		//--
	}
}