using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoguePlayer {
	//Private
	private int[,]		map			= null;

	//Public
	//Pozycja
	public int[]		position	= new int[2] {0, 0};
	//Dane
	public int[]		inventory	= new int[20];
	public int			helmet		= 0;
	public int			armor		= 0;
	public int			legs		= 0;
	public int			ring		= 0;
	public int			amulet		= 0;
	public int			weapon		= 0;

	public RoguePlayer(int[,] mapRef) {
		map	= mapRef;
	}

	public bool moveBy(int byX, int byY) {
		int[]	newPos	= new int[2] {
			position[0]	+ byX,
			position[1]	+ byY
		};
		
		if (newPos[0] >= map.GetLength(0)
		||	newPos[1] >= map.GetLength(1)
		||	newPos[0] < 0
		||	newPos[1] < 0
		||	(map[newPos[0], newPos[1]] > 0 && map[newPos[0], newPos[1]] < 1000 && map[newPos[0], newPos[1]] != 6)
		)
			return false;
		//--
		
		if (map[newPos[0], newPos[1]] == 6) {
			map[newPos[0], newPos[1]]	= 0;
			return true;
		}

		position	= newPos;
		return true;
	}
}
