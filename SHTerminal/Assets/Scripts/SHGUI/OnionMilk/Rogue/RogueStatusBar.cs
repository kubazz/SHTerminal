using UnityEngine;
using System.Collections;

public class RogueStatusBar {
	//Private
	private char[,]		screen	= new char[62, 3];
	private string		msg		= "   ";

	//Public
	public RogueStatusBar() {
		update();
	}

	void update() {
		for(int y = 0; y < screen.GetLength(1); ++y)
			for(int x = 0; x < screen.GetLength(0); ++x)
				screen[x, y]	= ' ';
			//--
		//--

		for(int x = 0; x < screen.GetLength(0); ++x)
			screen[x, 0]	= '═';
		//--
		
		for(int x = 0; x < msg.Length && x < screen.GetLength(0); ++x)
			screen[x, 2]	= msg[x];
		//--
	}

	public void setMessage(string newMsg) {
		msg	= newMsg;
		update();
	}
	
	public char[,] screenMap() {
		update();
		return screen;
	}
}
