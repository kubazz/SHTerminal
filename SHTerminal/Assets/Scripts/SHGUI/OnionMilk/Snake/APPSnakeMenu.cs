﻿using UnityEngine;
using System.Collections;

public class APPSnakeMenu {
	//Public
	
	public string	menuOptionsBar		= "  [ WITH BONUSES ]      [ CLASSIC ]           [ EXIT ]      ";
	public int		currentOption		= 0;
	public int		optionsOffset		= 0;
	public float	optionsOffsetTarget	= 0;
	public float	optionsAnimTimer	= 0;

	public int		gameSwitch			= 0;	/* 1 - klasyczna / 2 - z bonusami */

public string[]		menuScreen = new string[22] {
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                ",
	"                                                                "
};

	public APPSnakeMenu() {

	}

	public void Update() {
		optionsAnimTimer	+= Time.unscaledDeltaTime;
		if (optionsOffsetTarget != 0
		&&	optionsAnimTimer > 0.0012f
		) {
			if (optionsOffsetTarget < 0) {
				++optionsOffsetTarget;
				optionsOffset	-= 1;
			} else if (optionsOffsetTarget > 0) {
				--optionsOffsetTarget;
				optionsOffset	+= 1;
			}
			optionsOffset	%= menuOptionsBar.Length;
			
			optionsAnimTimer	= 0;
		}
	}

	public bool InputUpdate(SHGUIinput key) {
		if (optionsOffsetTarget == 0) {
			if (key == SHGUIinput.right || key == SHGUIinput.down) {
				optionsOffsetTarget	= 20;
				currentOption		+= 1;
			}
			if (key == SHGUIinput.left || key == SHGUIinput.up) {
				optionsOffsetTarget	= -20;
				currentOption		-= 1;
			}
			if (currentOption < 0)
				currentOption	= 3 + currentOption;
			//--
			currentOption	%= 3;
		}

		if (key == SHGUIinput.enter) {
			switch(currentOption) {
				case(0): {
					gameSwitch	= 1;
					break;
				}
				case(2): {
					gameSwitch	= 2;
					break;
				}
				case(1): {
					return false;
					break;
				}
			}
		}

		if (key == SHGUIinput.esc)
			return false;
		//--
		
		return true;
	}
}
