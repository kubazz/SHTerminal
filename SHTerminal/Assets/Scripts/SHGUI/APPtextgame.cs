
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;


public class APPtextgame : APPscrollconsole
{
	public APPtextgame ():base()
	{
		AddTextToQueueBreakLines("You are in a long concrete corridor. You see three men with guns aiming at you at the end of the corridor. There is a gun mid-way between you and them. What do you do?", 1f);
		AddInteractivePrompterToQueue ("GRAB GUN");
		AddTextToQueueBreakLines ("It's to far away", 1f);
		AddInteractivePrompterToQueue ("MOVE FORWARD");
		AddTextToQueueBreakLines ("You slowly move forward.", 1f);
		AddTextToQueueBreakLines ("One of the men shoots!", 1f);
		
		AddTextToQueueBreakLines ("You are in a long concrete corridor. You see three men with guns aiming at you at the end of the corridor. There is a gun mid-way between you and them. There is a bullet in the distance. What do you do?", 3f);

		AddInteractivePrompterToQueue ("GRAB GUN");
		AddTextToQueueBreakLines ("It's to far away", 1f);
		AddInteractivePrompterToQueue ("GRAB GUN");
		AddTextToQueueBreakLines ("You are in a long concrete corridor. You see three men with guns aiming at you at the end of the corridor. There is a gun mid-way between you and them. There is a bullet right in front of you. What do you do?", 3f);
		AddInteractivePrompterToQueue ("MOVE LEFT");

		AddTextToQueueBreakLines ("You are in a long concrete corridor. You see three men with guns aiming at you at the end of the corridor. There is a gun mid-way between you and them. A bullet passes to your right. What do you do?", 3f);
		AddInteractivePrompterToQueue ("MOVE FORWARD");
		
		
	}
}


