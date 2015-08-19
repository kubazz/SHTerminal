using System;
using System.Collections.Generic;
using System.Text;


public class SHGUIscrambrect: SHGUIview
{
	int startx;
	int starty;
	int endx;
	int endy;

	string currentScramble = "▀▄█";
	string scrabmleSource = "▀▄█▌░▒▓■▪";

	public bool allowSecretDisplay = true;
	
	//mode is 0 for front, 1 for back, 2 for both
	public SHGUIscrambrect (int Startx, int Starty, int Endx, int Endy)
	{
		Init ();
		
		startx = Startx;
		starty = Starty;
		endx = Endx;
		endy = Endy;

		color = 'z';

		RegenerateCarpet();
		currentDir = UnityEngine.Random.value > .5f;
		currentDir = true;
	}


	void RegenerateCarpet(){
		StringBuilder str = new StringBuilder();
		int r = (int)UnityEngine.Random.Range(2, 30);
		r = 26;
		bool ziom = true;
		for (int i = 0; i < r; ++i){
			ziom = !ziom;
			ziom = true;
			if (ziom)
				str.Append(scrabmleSource[UnityEngine.Random.Range(0, scrabmleSource.Length - 1)]);
			else{
				str.Append(' ');
			}
				
		}
		str.Append('A');
		
		currentScramble = str.ToString();

		//currentDir = UnityEngine.Random.value > .5f;	
	}

	float moveDelay = 0;
	bool currentDir = false;
	private int secretIndex = 0;

	public override void Redraw (int offx, int offy)
	{
		if (hidden)
			return;
		
		int newstartx = (int)(startx + endx * (1 - fade)) + offx;
		int newendx = (int)(endx * fade) + offx;
		int newstarty = (int)(starty + endy * (1 - fade)) + offy;
		int newendy = (int)(endy * fade) + offy;



		moveDelay -= UnityEngine.Time.unscaledDeltaTime;
		if (moveDelay < 0){
			if (UnityEngine.Random.value > .62f){
				//RegenerateCarpet();
			}
			else{
				if (currentDir){
					char first = currentScramble[0];
					currentScramble = currentScramble.Remove(0, 1);
					currentScramble += first;
				}
				else{
					char last = currentScramble[currentScramble.Length - 1];
					currentScramble = currentScramble.Remove(currentScramble.Length - 1, 1);
					currentScramble = last + currentScramble;
				}
			}
			moveDelay = .01f;
		}

		int i = 0;
		char colll = 'w';

		//string secretString = "YOU SHOULDN'T BE ABLE TO READ THIS";
		string secretString = "";
		secretString += " ";
		secretString = " " + secretString;
		int secretStringRep = 0;
		int thisSecretStringRep = UnityEngine.Random.Range (0, 4);

		for (int Y = newstarty; Y <= newendy; Y++) {
			i++;
			//colll = (UnityEngine.Random.value>.5f)?(color):('z');
			colll = color;
			for (int X = newstartx; X <= newendx; X++) {
				if (UnityEngine.Random.value < fade){
					if (currentScramble [i % currentScramble.Length] == 'A'){
						SHGUI.current.SetPixelFront('▒', X, Y, colll);
						if (secretIndex < secretString.Length && allowSecretDisplay){

							if (secretStringRep == thisSecretStringRep)
								SHGUI.current.SetPixelFront(secretString[secretIndex], X, Y, colll);

							secretIndex++;
							if (secretIndex >= secretString.Length){
								secretIndex = 0;
								secretStringRep++;
							}
						}
					}
					else{
						SHGUI.current.SetPixelFront(currentScramble [i % currentScramble.Length], X, Y, colll);
					}

					SHGUI.current.SetPixelBack(' ', X, Y, '0');
				}
			}
		}
		
		base.Redraw (x, y);
	}
	
}


