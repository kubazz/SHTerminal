using UnityEngine;
using System.Collections;

public class APP2048 : SHGUIappbase {

	string[]	mapFrame	= new string[9] {
	"╔════╦════╦════╦════╗",
	"║    ║    ║    ║    ║",
	"╠════╬════╬════╬════╣",
	"║    ║    ║    ║    ║",
	"╠════╬════╬════╬════╣",
	"║    ║    ║    ║    ║",
	"╠════╬════╬════╬════╣",
	"║    ║    ║    ║    ║",
	"╚════╩════╩════╩════╝"};

	int[,]		mapValue		= new int[4,4];
	
	int			lose			= 0; //0 gra | 1 przegrana | 2 zwycięstwo
	string		loseString		= "You Lose";

	string		scoreString		= "Score: ";
	int			score			= 0;


	public APP2048()
	: base("2048-v1500.100.900-by-onionmilk") {
		for(int i = 0; i < 4; ++i) for(int j = 0; j < 4; ++j)
		{
			mapValue[i, j] = 0;
		}

		Rand2();
		Rand2();
	}

	public override void Update() {
		base.Update();


		for(int i = 0; i < 4; ++i) for(int j = 0; j < 4; ++j)
		{
			if(mapValue[i, j] == 2048) lose = 2;
		}
	}

	public override void Redraw(int offx, int offy) {
		base.Redraw(offx, offy);

		scoreString = "Score: " + score;

		for(int i = 0; i < scoreString.Length; ++i)
		{
			SHGUI.current.SetPixelBack(scoreString[i], 22 + i, 7, 'w');
		}

		if(lose == 1)
		{
			for(int i = 0; i < loseString.Length; ++i)
			{
				SHGUI.current.SetPixelBack(loseString[i], 27 + i, 5, 'r');
			}
		}

		for(int i = 0; i < 9; ++i)
		{
			for(int j = 0; j < 21; ++j)
			{
				SHGUI.current.SetPixelBack(mapFrame[i][j], 20 + j, 8 + i, 'w');
			}
		}

		for(int i = 0; i < 4; ++i)
		{
			for(int j = 0; j < 4; ++j)
			{
				if(mapValue[i, j] == 2) SHGUI.current.SetPixelFront('2', 22 + 5 * i, 9 + 2 * j, 'w');
				else if(mapValue[i, j] == 4) SHGUI.current.SetPixelFront('4', 22 + 5 * i, 9 + 2 * j, 'w');
				else if(mapValue[i, j] == 8) SHGUI.current.SetPixelFront('8', 22 + 5 * i, 9 + 2 * j, 'w');
				else if(mapValue[i, j] == 16)
				{
					SHGUI.current.SetPixelFront('1', 22 + 5 * i, 9 + 2 * j, 'w');
					SHGUI.current.SetPixelFront('6', 23 + 5 * i, 9 + 2 * j, 'w');
				}
				else if(mapValue[i, j] == 32)
				{
					SHGUI.current.SetPixelFront('3', 22 + 5 * i, 9 + 2 * j, 'w');
					SHGUI.current.SetPixelFront('2', 23 + 5 * i, 9 + 2 * j, 'w');
				}
				else if(mapValue[i, j] == 64)
				{
					SHGUI.current.SetPixelFront('6', 22 + 5 * i, 9 + 2 * j, 'w');
					SHGUI.current.SetPixelFront('4', 23 + 5 * i, 9 + 2 * j, 'w');
				}
				else if(mapValue[i, j] == 128)
				{
					SHGUI.current.SetPixelFront('1', 22 + 5 * i, 9 + 2 * j, 'w');
					SHGUI.current.SetPixelFront('2', 23 + 5 * i, 9 + 2 * j, 'w');
					SHGUI.current.SetPixelFront('8', 24 + 5 * i, 9 + 2 * j, 'w');
				}
				else if(mapValue[i, j] == 256)
				{
					SHGUI.current.SetPixelFront('2', 22 + 5 * i, 9 + 2 * j, 'w');
					SHGUI.current.SetPixelFront('5', 23 + 5 * i, 9 + 2 * j, 'w');
					SHGUI.current.SetPixelFront('6', 24 + 5 * i, 9 + 2 * j, 'w');
				}
				else if(mapValue[i, j] == 512)
				{
					SHGUI.current.SetPixelFront('5', 22 + 5 * i, 9 + 2 * j, 'w');
					SHGUI.current.SetPixelFront('1', 23 + 5 * i, 9 + 2 * j, 'w');
					SHGUI.current.SetPixelFront('2', 24 + 5 * i, 9 + 2 * j, 'w');
				}
				else if(mapValue[i, j] == 1024)
				{
					SHGUI.current.SetPixelFront('1', 21 + 5 * i, 9 + 2 * j, 'w');
					SHGUI.current.SetPixelFront('0', 22 + 5 * i, 9 + 2 * j, 'w');
					SHGUI.current.SetPixelFront('2', 23 + 5 * i, 9 + 2 * j, 'w');
					SHGUI.current.SetPixelFront('4', 24 + 5 * i, 9 + 2 * j, 'w');
				}
				else if(mapValue[i, j] == 2048)
				{
					SHGUI.current.SetPixelFront('2', 21 + 5 * i, 9 + 2 * j, 'r');
					SHGUI.current.SetPixelFront('0', 22 + 5 * i, 9 + 2 * j, 'r');
					SHGUI.current.SetPixelFront('4', 23 + 5 * i, 9 + 2 * j, 'r');
					SHGUI.current.SetPixelFront('8', 24 + 5 * i, 9 + 2 * j, 'r');
				}
				else if(mapValue[i, j] == 4096)
				{
					SHGUI.current.SetPixelFront('4', 21 + 5 * i, 9 + 2 * j, 'r');
					SHGUI.current.SetPixelFront('0', 22 + 5 * i, 9 + 2 * j, 'r');
					SHGUI.current.SetPixelFront('9', 23 + 5 * i, 9 + 2 * j, 'r');
					SHGUI.current.SetPixelFront('6', 24 + 5 * i, 9 + 2 * j, 'r');
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
		if(!checkMove(direct)) return;

		if(direct == 0) //góra
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
		else if(direct == 1) //prawo
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
		else if(direct == 2) //dół
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
		else if(direct == 3) //lewo
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
		}
		Rand2();

	}

	bool checkMove(int direct)
	{
		int FoundZero = 0; //ilość zer
		int FirstZero = 4;
		
		int countLock = 0;

		if(direct == 0)
		{
			for(int i = 0; i < 4; ++i)
			{
				for(int j = 0; j < 4; ++j)
				{
					if(mapValue[i, j] == 0) ++FoundZero;
					if(mapValue[i, j] == 0 && FirstZero == 4) FirstZero = j; 
				}
				if(FoundZero == 4 - FirstZero)
				{
					if(FoundZero == 0 || FoundZero == 1)
					{
						++countLock;
						break;
					}
					else if(FoundZero == 2 && mapValue[i, 0] != mapValue[i, 1])
					{
						++countLock;
						break;
					}
					else if(FoundZero == 3 && mapValue[i, 0] != mapValue[i, 1] && mapValue[i, 1] != mapValue[i, 2])
					{
						++countLock;
						break;
					}
					else if(FoundZero == 4 && mapValue[i, 0] != mapValue[i, 1] && mapValue[i, 1] != mapValue[i, 2] && mapValue[i, 2] != mapValue[i, 3])
					{
						++countLock;
						break;
					}

					return true;
				}
				else return true;
			}
		}
		else if(direct == 1)
		{
			for(int j = 0; j < 4; ++j)
			{
				for(int i = 3; i >= 0; --i)
				{
					if(mapValue[i, j] == 0) ++FoundZero;
					if(mapValue[i, j] == 0 && FirstZero == 4) FirstZero = j;
				}
				if(FoundZero == 4 - FirstZero)
				{
					if(FoundZero == 0 || FoundZero == 1)
					{
						++countLock;
						break;
					}
					else if(FoundZero == 2 && mapValue[3, j] != mapValue[2, j])
					{
						++countLock;
						break;
					}
					else if(FoundZero == 3 && mapValue[3, j] != mapValue[2, j] && mapValue[2, j] != mapValue[1, j])
					{
						++countLock;
						break;
					}
					else if(FoundZero == 4 && mapValue[3, j] != mapValue[2, j] && mapValue[2, j] != mapValue[1, j] && mapValue[1, j] != mapValue[0, j])
					{
						++countLock;
						break;
					}
					
					return true;
				}
				else return true;
			}
		}
		else if(direct == 2)
		{
			for(int i = 0; i < 4; ++i)
			{
				for(int j = 3; j >= 0; --j)
				{
					if(mapValue[i, j] == 0) ++FoundZero;
					if(mapValue[i, j] == 0 && FirstZero == 4) FirstZero = j; 
				}
				if(FoundZero == 4 - FirstZero)
				{
					if(FoundZero == 0 || FoundZero == 1)
					{
						++countLock;
						break;
					}
					else if(FoundZero == 2 && mapValue[i, 3] != mapValue[i, 2])
					{
						++countLock;
						break;
					}
					else if(FoundZero == 3 && mapValue[i, 3] != mapValue[i, 2] && mapValue[i, 2] != mapValue[i, 1])
					{
						++countLock;
						break;
					}
					else if(FoundZero == 4 && mapValue[i, 3] != mapValue[i, 2] && mapValue[i, 2] != mapValue[i, 1] && mapValue[i, 1] != mapValue[i, 0])
					{
						++countLock;
						break;
					}
					
					return true;
				}
				else return true;
			}
		}
		else if(direct == 3)
		{
			for(int j = 0; j < 4; ++j)
			{
				for(int i = 0; i < 4; ++i)
				{
					if(mapValue[i, j] == 0) ++FoundZero;
					if(mapValue[i, j] == 0 && FirstZero == 4) FirstZero = j; 
				}
				if(FoundZero == 4 - FirstZero)
				{
					if(FoundZero == 0 || FoundZero == 1)
					{
						++countLock;
						break;
					}
					else if(FoundZero == 2 && mapValue[0, j] != mapValue[1, j])
					{
						++countLock;
						break;
					}
					else if(FoundZero == 3 && mapValue[0, j] != mapValue[1, j] && mapValue[1, j] != mapValue[2, j])
					{
						++countLock;
						break;
					}
					else if(FoundZero == 4 && mapValue[0, j] != mapValue[1, j] && mapValue[1, j] != mapValue[2, j] && mapValue[2, j] != mapValue[3, j])
					{
						++countLock;
						break;
					}
					
					return true;
				}
				else return true;
			}
		}

		if(countLock >= 4) return false;
		return true;
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
