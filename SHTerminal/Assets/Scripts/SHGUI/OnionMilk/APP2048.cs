using UnityEngine;
using System.Collections;

public class APP2048 : SHGUIappbase {

	string[]	mapFrame	= new string[17]
	{
		"╔════════╦════════╦════════╦════════╗",
		"║        ║        ║        ║        ║",
		"║        ║        ║        ║        ║",
		"║        ║        ║        ║        ║",
		"╠════════╬════════╬════════╬════════╣",
		"║        ║        ║        ║        ║",
		"║        ║        ║        ║        ║",
		"║        ║        ║        ║        ║",
		"╠════════╬════════╬════════╬════════╣",
		"║        ║        ║        ║        ║",
		"║        ║        ║        ║        ║",
		"║        ║        ║        ║        ║",
		"╠════════╬════════╬════════╬════════╣",
		"║        ║        ║        ║        ║",
		"║        ║        ║        ║        ║",
		"║        ║        ║        ║        ║",
		"╚════════╩════════╩════════╩════════╝"};

	string[]	Title		= new string[19]
	{
		"███",
		"  █",
		"███",
		"█  ",
		"███",
		"   ",
		"███",
		"█ █",
		"███",
		"   ",
		"█ █",
		"███",
		"  █",
		"   ",
		"███",
		"█ █",
		"███",
		"█ █",
		"███"
	};
	/*	"/\\ ",
		" / ",
		"/_ ",
		" _ ",
		"/ \\",
		"| |",
		"\\_/",
		"   ",
		" /|",
		"/_|",
		"  |",
		" _ ",
		"/ \\",
		"> <",
		"\\_/"
	};*/



	int[,]		mapValue		= new int[4,4];
	
	int			lose			= 0; //0 gra | 1 przegrana | 2 zwycięstwo
	string		loseString		= "You Lose";

	string		scoreString		= "Score: ";
	int			score			= 0;

	bool[]		lockDirection	= new bool[4]; //oznaczenie który z kierunków jest zablokowany
	int			allLockCount	= 0; // ilosć zablokowanych kierunków jeżeli jest rónwa 4 przegrywasz
	int[]		rope			= new int[4];


	public APP2048()
	: base("2048-v1500.100.900-by-onionmilk") {
		for(int i = 0; i < 4; ++i) for(int j = 0; j < 4; ++j)
		{
			mapValue[i, j] = 0;
		}

		Rand2();
		Rand2();

		for(int i = 0; i < 4; ++i)
		{
			lockDirection[i] = false;
			rope[i] = 0;
		}

		CheckMove();
	}

	public override void Update() {
		base.Update();

		//sprawdzenie warunku przegranej
		for(int i = 0; i < 4; ++i) if(lockDirection[i]) ++allLockCount;
		if(allLockCount == 4) lose = 1;
		else allLockCount = 0;

		//sprawdzenie warunku wygranej
		for(int i = 0; i < 4; ++i) for(int j = 0; j < 4; ++j)
		{
			if(mapValue[i, j] == 2048) lose = 2;
		}
	}

	public override void Redraw(int offx, int offy) {
		base.Redraw(offx, offy);

		scoreString = "Score: " + score;

		/*for(int i = 0; i < scoreString.Length; ++i)
		{
			SHGUI.current.SetPixelBack(scoreString[i], 22 + i, 7, 'w');
		}

		if(lose == 1)
		{
			for(int i = 0; i < loseString.Length; ++i)
			{
				SHGUI.current.SetPixelBack(loseString[i], 27 + i, 5, 'r');
			}
		}*/

		for(int i = 0; i < 17; ++i)
		{
			for(int j = 0; j < 37; ++j)
			{
				SHGUI.current.SetPixelBack(mapFrame[i][j], 10 + j, 4 + i, 'w');
			}
		}
		for(int i = 0; i < 19; ++i)
		{
			for(int j = 0; j < 3; ++j)
			{
				SHGUI.current.SetPixelBack(Title[i][j], 50 + j, 3 + i, 'z');
			}
		}

		for(int i = 0; i < 4; ++i)
		{
			for(int j = 0; j < 4; ++j)
			{
				if(mapValue[i, j] == 2) SHGUI.current.SetPixelFront('2', 15 + 9 * i, 6 + 4 * j, 'w');
				else if(mapValue[i, j] == 4) SHGUI.current.SetPixelFront('4', 15 + 9 * i, 6 + 4 * j, 'w');
				else if(mapValue[i, j] == 8) SHGUI.current.SetPixelFront('8', 15 + 9 * i, 6 + 4 * j, 'w');
				else if(mapValue[i, j] == 16)
				{
					SHGUI.current.SetPixelFront('1', 14 + 9 * i, 6 + 4 * j, 'w');
					SHGUI.current.SetPixelFront('6', 15 + 9 * i, 6 + 4 * j, 'w');
				}
				else if(mapValue[i, j] == 32)
				{
					SHGUI.current.SetPixelFront('3', 14 + 9 * i, 6 + 4 * j, 'w');
					SHGUI.current.SetPixelFront('2', 15 + 9 * i, 6 + 4 * j, 'w');
				}
				else if(mapValue[i, j] == 64)
				{
					SHGUI.current.SetPixelFront('6', 14 + 9 * i, 6 + 4 * j, 'w');
					SHGUI.current.SetPixelFront('4', 15 + 9 * i, 6 + 4 * j, 'w');
				}
				else if(mapValue[i, j] == 128)
				{
					SHGUI.current.SetPixelFront('1', 14 + 9 * i, 6 + 4 * j, 'w');
					SHGUI.current.SetPixelFront('2', 15 + 9 * i, 6 + 4 * j, 'w');
					SHGUI.current.SetPixelFront('8', 16 + 9 * i, 6 + 4 * j, 'w');
				}
				else if(mapValue[i, j] == 256)
				{
					SHGUI.current.SetPixelFront('2', 14 + 9 * i, 6 + 4 * j, 'w');
					SHGUI.current.SetPixelFront('5', 15 + 9 * i, 6 + 4 * j, 'w');
					SHGUI.current.SetPixelFront('6', 16 + 9 * i, 6 + 4 * j, 'w');
				}
				else if(mapValue[i, j] == 512)
				{
					SHGUI.current.SetPixelFront('5', 14 + 9 * i, 6 + 4 * j, 'w');
					SHGUI.current.SetPixelFront('1', 15 + 9 * i, 6 + 4 * j, 'w');
					SHGUI.current.SetPixelFront('2', 16 + 9 * i, 6 + 4 * j, 'w');
				}
				else if(mapValue[i, j] == 1024)
				{
					SHGUI.current.SetPixelFront('1', 13 + 9 * i, 6 + 4 * j, 'w');
					SHGUI.current.SetPixelFront('0', 14 + 9 * i, 6 + 4 * j, 'w');
					SHGUI.current.SetPixelFront('2', 15 + 9 * i, 6 + 4 * j, 'w');
					SHGUI.current.SetPixelFront('4', 16 + 9 * i, 6 + 4 * j, 'w');
				}
				else if(mapValue[i, j] == 2048)
				{
					SHGUI.current.SetPixelFront('2', 13 + 9 * i, 6 + 4 * j, 'r');
					SHGUI.current.SetPixelFront('0', 14 + 9 * i, 6 + 4 * j, 'r');
					SHGUI.current.SetPixelFront('4', 15 + 9 * i, 6 + 4 * j, 'r');
					SHGUI.current.SetPixelFront('8', 16 + 9 * i, 6 + 4 * j, 'r');
				}
				else if(mapValue[i, j] == 4096)
				{
					SHGUI.current.SetPixelFront('4', 13 + 9 * i, 6 + 4 * j, 'r');
					SHGUI.current.SetPixelFront('0', 14 + 9 * i, 6 + 4 * j, 'r');
					SHGUI.current.SetPixelFront('9', 15 + 9 * i, 6 + 4 * j, 'r');
					SHGUI.current.SetPixelFront('6', 16 + 9 * i, 6 + 4 * j, 'r');
				}
			}
		}

	}

	public override void ReactToInputKeyboard(SHGUIinput key) {
		//sterowanie
		if (key == SHGUIinput.up) {
			if(lose == 0) Move(0);
		}
		if (key == SHGUIinput.down) {
			if(lose == 0) Move(2);
		}
		if (key == SHGUIinput.right) {
			if(lose == 0) Move(1);
		}
		if (key == SHGUIinput.left) {
			if(lose == 0) Move(3);
		}
		
		
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView();
		//--
	}
	void Move(int direct)
	{
		if(direct == 0 && !lockDirection[0]) //góra
		{
			int tempPos = 0;

			for(int i = 0; i < 4; ++i)
			{
				tempPos = 0;

				for(int j = 0; j < 4; ++j)
				{
					if(mapValue[i, j] != 0) //szukanie nie zerowego elementu do przesunięcia
					{
						if(mapValue[i, tempPos] == 0) //jeżeli na drodze są same puste pola
						{
							mapValue[i, tempPos] = mapValue[i, j]; //przesunięcie do najdalszej wolnej pozycji
							mapValue[i, j] = 0; //wyzerowanie starego zajmowanego pola
						}
						else if(mapValue[i, tempPos] == mapValue[i, j]) //jeżeli na drodze jest drugi ten sam element
						{
							if(tempPos == j) continue; //pilnowanie żeby nie podwoiło się dokładnie to samo pole

							score += mapValue[i, j];
							mapValue[i, tempPos] += mapValue[i, j]; //zwiększanie tych samych wartości
							mapValue[i, j] = 0; //wyzerowanie starego zajmowanego pola
							++tempPos; //przesunięcie strażnika na następne pole by w jednym ruchu jakaś liczba nie zwiększyła się kilka razy
						}
						else //jeżeli elementy nie są równe, a pole jest zajęte
						{
							++tempPos;
							int tempVal = mapValue[i, j];
							mapValue[i, j] = 0; //wyzerowanie starego zajmowanego pola
							mapValue[i, tempPos] = tempVal; //przesunięcie do najdalszej wolnej pozycji

						}
					}
				}
			}
			Rand2();
		}
		else if(direct == 1 && !lockDirection[1]) //prawo
		{
			int tempPos = 3;
			
			for(int j = 0; j < 4; ++j)
			{
				tempPos = 3;
				
				for(int i = 3; i >= 0; --i)
				{
					if(mapValue[i, j] != 0) //szukanie nie zerowego elementu do przesunięcia
					{
						if(mapValue[tempPos, j] == 0) //jeżeli na drodze są same puste pola
						{
							mapValue[tempPos, j] = mapValue[i, j]; //przesunięcie do najdalszej wolnej pozycji
							mapValue[i, j] = 0; //wyzerowanie starego zajmowanego pola
						}
						else if(mapValue[tempPos, j] == mapValue[i, j]) //jeżeli na drodze jest drugi ten sam element
						{
							if(tempPos == i) continue; //pilnowanie żeby nie podwoiło się dokładnie to samo pole

							score += mapValue[i, j];
							mapValue[tempPos, j] += mapValue[i, j]; //zwiększanie tych samych wartości
							mapValue[i, j] = 0; //wyzerowanie starego zajmowanego pola
							--tempPos; //przesunięcie strażnika na następne pole by w jednym ruchu jakaś liczba nie zwiększyła się kilka razy
						}
						else //jeżeli elementy nie są równe, a pole jest zajęte
						{
							--tempPos;
							int tempVal = mapValue[i, j];
							mapValue[i, j] = 0; //wyzerowanie starego zajmowanego pola
							mapValue[tempPos, j] = tempVal; //przesunięcie do najdalszej wolnej pozycji
							
						}
					}
				}
			}
			Rand2();
		}
		else if(direct == 2 && !lockDirection[2]) //dół
		{
			int tempPos = 3;
			
			for(int i = 0; i < 4; ++i)
			{
				tempPos = 3;
				
				for(int j = 3; j >= 0; --j)
				{
					if(mapValue[i, j] != 0) //szukanie nie zerowego elementu do przesunięcia
					{
						if(mapValue[i, tempPos] == 0) //jeżeli na drodze są same puste pola
						{
							mapValue[i, tempPos] = mapValue[i, j]; //przesunięcie do najdalszej wolnej pozycji
							mapValue[i, j] = 0; //wyzerowanie starego zajmowanego pola
						}
						else if(mapValue[i, tempPos] == mapValue[i, j]) //jeżeli na drodze jest drugi ten sam element
						{
							if(tempPos == j) continue; //pilnowanie żeby nie podwoiło się dokładnie to samo pole

							score += mapValue[i, j];
							mapValue[i, tempPos] += mapValue[i, j]; //zwiększanie tych samych wartości
							mapValue[i, j] = 0; //wyzerowanie starego zajmowanego pola
							--tempPos; //przesunięcie strażnika na następne pole by w jednym ruchu jakaś liczba nie zwiększyła się kilka razy
						}
						else //jeżeli elementy nie są równe, a pole jest zajęte
						{
							--tempPos;
							int tempVal = mapValue[i, j];
							mapValue[i, j] = 0; //wyzerowanie starego zajmowanego pola
							mapValue[i, tempPos] = tempVal; //przesunięcie do najdalszej wolnej pozycji
							
						}
					}
				}
			}
			Rand2();
		}
		else if(direct == 3 && !lockDirection[3]) //lewo
		{
			int tempPos = 0;
			
			for(int j = 0; j < 4; ++j)
			{
				tempPos = 0;
				
				for(int i = 0; i < 4; ++i)
				{
					if(mapValue[i, j] != 0) //szukanie nie zerowego elementu do przesunięcia
					{
						if(mapValue[tempPos, j] == 0) //jeżeli na drodze są same puste pola
						{
							mapValue[tempPos, j] = mapValue[i, j]; //przesunięcie do najdalszej wolnej pozycji
							mapValue[i, j] = 0; //wyzerowanie starego zajmowanego pola
						}
						else if(mapValue[tempPos, j] == mapValue[i, j]) //jeżeli na drodze jest drugi ten sam element
						{
							if(tempPos == i) continue; //pilnowanie żeby nie podwoiło się dokładnie to samo pole

							score += mapValue[i, j];
							mapValue[tempPos, j] += mapValue[i, j]; //zwiększanie tych samych wartości
							mapValue[i, j] = 0; //wyzerowanie starego zajmowanego pola
							++tempPos; //przesunięcie strażnika na następne pole by w jednym ruchu jakaś liczba nie zwiększyła się kilka razy
						}
						else //jeżeli elementy nie są równe, a pole jest zajęte
						{
							++tempPos;
							int tempVal = mapValue[i, j];
							mapValue[i, j] = 0; //wyzerowanie starego zajmowanego pola
							mapValue[tempPos, j] = tempVal; //przesunięcie do najdalszej wolnej pozycji
							
						}
					}
				}
			}
			Rand2();
		}

		CheckMove();
	}

	void CheckMove() //sprawdzenie w które strony mozna się przemieścić
	{
		int countLock = 0; //ilość zablokowanych sznurów z danym kierunku

		for(int i = 0; i < 4; ++i)//ruch w górę
		{
			for(int j = 0; j < 4; ++j)
			{
				rope[j] = mapValue[i, j]; //przygotowanie fragmentu mapy do sprawdzenia
			}
			if(CalcLock(false)) ++countLock; //zliczanie ilości sznórów których nie da się przesunąć
		}
		if(countLock >= 4) lockDirection[0] = true; //zablokowanie danego kierunku
		else lockDirection[0] = false; //odblokowanie danego kierunku
		countLock = 0; //zerownie ilości sznórów których nie da się przesunąć

		for(int i = 0; i < 4; ++i)//ruch w prawo
		{
			for(int j = 0; j < 4; ++j)
			{
				rope[j] = mapValue[j, i];
			}
			if(CalcLock(true)) ++countLock;
		}
		if(countLock >= 4) lockDirection[1] = true;
		else lockDirection[1] = false;
		countLock = 0;

		for(int i = 0; i < 4; ++i)//ruch w dół
		{
			for(int j = 0; j < 4; ++j)
			{
				rope[j] = mapValue[i, j];
			}
			if(CalcLock(true)) ++countLock;
		}
		if(countLock >= 4) lockDirection[2] = true;
		else lockDirection[2] = false;
		countLock = 0;

		for(int i = 0; i < 4; ++i)//ruch w lewo
		{
			for(int j = 0; j < 4; ++j)
			{
				rope[j] = mapValue[j, i];
			}
			if(CalcLock(false)) ++countLock;
		}
		if(countLock >= 4) lockDirection[3] = true;
		else lockDirection[3] = false;
		countLock = 0;
	}
	bool CalcLock(bool rev)
	{
		int FoundZero = 0; //ilość zer w sznurze
		if(rev) //odwracanie sznura
		{
			int tempVal = rope[0];
			rope[0] = rope[3];
			rope[3] = tempVal;
			tempVal = rope[1];
			rope[1] = rope[2];
			rope[2] = tempVal;
		}
		for(int i = 0; i < 4; ++i) if(rope[i] == 0) ++FoundZero;

		if(FoundZero == 4) return true;
		else if(FoundZero == 3)
		{
			if(rope[0] != 0) return true;
		}
		else if(FoundZero == 2)
		{
			if(rope[0] != 0)
			{
				if(rope[1] != 0 && rope[1] != rope[0]) return true;
			}
		}
		else if(FoundZero == 1)
		{
			if(rope[0] != 0)
			{
				if(rope[1] != 0 && rope[1] != rope[0])
				{
					if(rope[2] != 0 && rope[2] != rope[1]) return true;
				}
			}
		}
		else if(FoundZero == 0)
		{
			if(rope[0] != 0)
			{
				if(rope[1] != 0 && rope[1] != rope[0])
				{
					if(rope[2] != 0 && rope[2] != rope[1])
					{
						if(rope[3] != 0 && rope[3] != rope[2])
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	void Rand2()
	{
		for(;;)
		{
			int tempPosX = Random.Range(0, 4);
			int tempPosY = Random.Range(0, 4);

			if(mapValue[tempPosX, tempPosY] == 0)
			{
				mapValue[tempPosX, tempPosY] = 2;
				score += 2;
				break;
			}
			else
			{
				int tempCount = 0;

				for(int i = 0; i < 4; ++i)
				{
					for(int j = 0; j < 4; ++j)
					{
						if(mapValue[i, j] == 0) ++tempCount;
					}
				}
				if(tempCount == 0)
				{
					//lose = 1;
					return;
				}
				if(tempCount == 1 || tempCount == 2 || tempCount == 3)
				{
					for(int i = 0; i < 4; ++i)
					{
						for(int j = 0; j < 4; ++j)
						{
							if(mapValue[i, j] == 0)
							{
								mapValue[i, j] = 2;
								score += 2;
								return;
							}
						}
					}
				}
			}
		}
	}
}
