using UnityEngine;
using System.Collections;

public class enemy
{
	int posX = 0;
	int posY = 0;

};

public class APPSpaceInv : SHGUIappbase {

	public APPSpaceInv()
	: base("hot_in_space-v4.2-by-onionmilk") {

	}
	
	public override void Update() {
		base.Update();

	}

	public override void Redraw(int offx, int offy)
	{
		base.Redraw(offx, offy);
		
		for(int i = 0; i < 23; ++i)
		{
			//SHGUI.current.SetPixelFront(ground, mapa[i].posX, mapa[i].posY, 'z');
			//SHGUI.current.SetPixelFront(ground, mapa[i].posX - 1, mapa[i].posY, 'z');
			//SHGUI.current.SetPixelFront(ground, mapa[i].posX - 2, mapa[i].posY, 'z');
		}
		
	}
	
	public override void ReactToInputKeyboard(SHGUIinput key)
	{
		//sterowanie
		if (key == SHGUIinput.esc) SHGUI.current.PopView();
		
		if (key == SHGUIinput.up)
		{

			
		}
		
		
	}
}
