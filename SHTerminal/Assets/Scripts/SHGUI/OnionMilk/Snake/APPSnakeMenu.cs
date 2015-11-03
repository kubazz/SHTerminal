using UnityEngine;
using System.Collections;

public class APPSnakeMenu {
	//Public
	
	public string	menuOptionsBar		= "    [ CLASSIC ]       [ WITH BONUSES ]        [ EXIT ]      ";
	public int		currentOption		= 0;
	public int		optionsOffset		= 40;
	public float	optionsOffsetTarget	= 40;
	public float	optionsAnimTimer	= 0;

	public APPSnakeMenu() {

	}

	public void Update() {
		optionsAnimTimer	+= Time.unscaledDeltaTime;
		if (optionsOffset != optionsOffsetTarget
		&&	optionsAnimTimer > 0.025f
		) {
			if (optionsOffset >= optionsOffsetTarget) {
				optionsOffset		= (optionsOffset + 1)%menuOptionsBar.Length;
				--optionsOffsetTarget;
			} else {
				optionsOffset		= (optionsOffset - 1)%menuOptionsBar.Length;
				++optionsOffsetTarget;
			}//Licznik przesunięć
			
			optionsAnimTimer	= 0;
		}
	}

	public bool InputUpdate(SHGUIinput key) {
		if (optionsOffset == optionsOffsetTarget) {
			if (key == SHGUIinput.right || key == SHGUIinput.down)
				optionsOffsetTarget	= optionsOffset + 20;
			//--
			if (key == SHGUIinput.left || key == SHGUIinput.up)
				optionsOffsetTarget	= optionsOffset - 20;
			//--
		}

		if (key == SHGUIinput.esc)
			return false;
		//--
		
		return true;
	}
}
