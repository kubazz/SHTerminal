using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MergeAnim 
{
	public	int			lifeTime			= 0; //obecna klatka animacji max 5
	public	float		changeCounter		= 0.02f;

	public	int			posX				= 0;
	public	int			posY				= 0;

	public MergeAnim(int pX, int pY)
	{
		posX = pX;
		posY = pY;
	}

	public bool MergeUpdate(float timeDelta)
	{
		changeCounter -= timeDelta;
		if(changeCounter <= 0f)
		{
			changeCounter = 0.02f;
			++lifeTime;
			if(lifeTime >= 6) return true;
		}

		return false;
	}

};

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
		"╚════════╩════════╩════════╩════════╝"
	};

	string[]	title	= new string[6]
	{
		".22222.  .0000.    4444  .88888. ",
		"22  `22 .00  00.  44~44  88   88 ",
		"   22D' 00  0'00 44' 44  `88888' ",
		" .22'   00 0' 00 4444444 .88888. ",
		"222.    `00  00'     44  88   88 ",
		"2222222  `0000'      44  `88888' "
	};

	string[]	boomAnim	= new string[18]
	{
		"      ",
		"  ..  ",
		"      ",
		"      ",
		"  **  ",
		"      ",
		" '  ' ",
		" -**- ",
		" '  ' ",
		" \\  / ",
		" -  - ",
		" /  \\ ",
		" \\  / ",
		"      ",
		" /  \\ ",
		" .  . ",
		"      ",
		" .  . "
	};

	string			hintMenu			= "PRESS ENTER TO START";
	string			hintGame			= "USE ARROW TO PLAY";
	string			hintDead			= "PRESS ESC BACK TO MENU";

	List<MergeAnim>	animList			= new List<MergeAnim>();

	int[,]			mapValue			= new int[4,4];
	int[,]			mapTemp				= new int[4,4];

	bool[]			lockDirection		= new bool[4]; //oznaczenie który z kierunków jest zablokowany
	int				allLockCount		= 0; // ilosć zablokowanych kierunków jeżeli jest rónwa 4 przegrywasz
	int[]			rope				= new int[4]; //sznur czyli wartości w sprawdzanej lini
	
	int				lose				= 0; //0 gra | 1 przegrana | 2 zwycięstwo
	string			loseString			= "YOU LOSE";
	string			winString			= "CONGRATULATIONS YOU WIN";

	string			scoreString			= "Score:";
	string			currScoreString		= "";
	int				score				= 0;

	string			bestString			= "Best:";
	string			currBestString		= "";
	int				best				= 0;

	float			gameTimer			= 0f; //czas trwania gry
		
	bool			menu				= true; //czy znajdujesz się w menu czy w grze

	bool			animBlock			= false; //czy jest właśnie wykonywana animacja
	float			animTimer			= 0f; //czas blokady

	//nowo stawiany klocuch
	int				newPosX				= 0;
	int				newPosY				= 0;
	float			newTimer			= 0.2f;

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

		currScoreString = "" + score;
		currBestString = "" + best;

		//animList.Add(new MergeAnim(1, 1));
	}

	public override void Update()
	{
		base.Update();

		if(menu) APPINSTRUCTION.text = "ESC-to-quit";
		else APPINSTRUCTION.text = "ESC-to-reset";

		if(best < score) best = score;

		if(!menu)
		{

			for(int i = 0; i < animList.Count;) //animacja łączenia obiektów
			{
				//sprawdzenie czy element jest do zniszczenia i ustawienie wartości zawartości
				if(animList[i].MergeUpdate(Time.unscaledDeltaTime))
				{
					animList.RemoveAt(i);
					continue;
				}
				++i;
			}

			gameTimer += Time.unscaledDeltaTime;
			if(newTimer > 0f) newTimer -= Time.unscaledDeltaTime;

			if(animBlock)
			{
				animTimer -= Time.deltaTime;
				if(animTimer <= 0)
				{
					animBlock = false;
					currScoreString = "" + score;
					currBestString = "" + best;
				}
			}

			//sprawdzenie warunku przegranej
			for(int i = 0; i < 4; ++i) if(lockDirection[i]) ++allLockCount;
			if(allLockCount == 4) lose = 1;
			else allLockCount = 0;

			//sprawdzenie warunku wygranej
			for(int i = 0; i < 4; ++i) for(int j = 0; j < 4; ++j)
			{
				if(mapValue[i, j] == 2048) lose = 2;
			}

			if(best < score) best = score;
		}
	}

	public override void Redraw(int offx, int offy)
	{
		base.Redraw(offx, offy);

		if(menu)
		{
			for(int i = 0; i < 6; ++i) //tytuł
			{
				for(int j = 0; j < title[i].Length; ++j)
				{
					SHGUI.current.SetPixelBack(title[i][j], (62 - title[i].Length)/2 + j, 5 + i, 'w');
				}
			}
			for(int i = 0; i < hintMenu.Length; ++i) //podpowiedź
			{
				SHGUI.current.SetPixelBack(hintMenu[i], (62 - hintMenu.Length)/2 + i, 14, 'w');
			}

			//najlepszy wynik
			for(int i = 0; i < bestString.Length; ++i) SHGUI.current.SetPixelBack(bestString[i], (62 - bestString.Length)/2 + i + 1, 16, 'w');
			for(int i = 0; i < currBestString.Length; ++i) SHGUI.current.SetPixelBack(currBestString[i], (62 - currBestString.Length)/2 + i, 17, 'w');
		}
		else
		{
			//obecny wynik
			for(int i = 0; i < scoreString.Length; ++i) SHGUI.current.SetPixelBack(scoreString[i], 20 + i, 2, 'w');
			for(int i = 0; i < currScoreString.Length; ++i) SHGUI.current.SetPixelBack(currScoreString[i], 22 - (currScoreString.Length/2) + i, 3, 'w');

			//najlepszy wynik
			for(int i = 0; i < bestString.Length; ++i) SHGUI.current.SetPixelBack(bestString[i], 40 + i, 2, 'w');
			for(int i = 0; i < currBestString.Length; ++i) SHGUI.current.SetPixelBack(currBestString[i], 42 - (currBestString.Length/2) + i, 3, 'w');


			for(int i = 0; i < 17; ++i) //ramka
			{
				for(int j = 0; j < 37; ++j)
				{
					SHGUI.current.SetPixelBack(mapFrame[i][j], 13 + j, 4 + i, 'z');
				}
			}

			if(lose == 0)
			{
				for(int i = 0; i < hintGame.Length; ++i) //podpowiedź na dole ekranu
				{
					SHGUI.current.SetPixelBack(hintGame[i], (62 - hintGame.Length)/2 + i, 21, 'w');
				}
			}

			for(int i = 0; i < 4; ++i) //rysowanie wartości w okienkach
			{
				for(int j = 0; j < 4; ++j)
				{
					char tempKolor = 'w';
					if(newPosX == i && newPosY == j && newTimer > 0f)
					{
						tempKolor = 'r';
					}
					if(mapValue[i, j] == 2) SHGUI.current.SetPixelFront('2', 18 + 9 * i, 6 + 4 * j, tempKolor);
					else if(mapValue[i, j] == 4) SHGUI.current.SetPixelFront('4', 18 + 9 * i, 6 + 4 * j, tempKolor);
					else if(mapValue[i, j] == 8) SHGUI.current.SetPixelFront('8', 18 + 9 * i, 6 + 4 * j, tempKolor);
					else if(mapValue[i, j] == 16)
					{
						SHGUI.current.SetPixelFront('1', 17 + 9 * i, 6 + 4 * j, tempKolor);
						SHGUI.current.SetPixelFront('6', 18 + 9 * i, 6 + 4 * j, tempKolor);
					}
					else if(mapValue[i, j] == 32)
					{
						SHGUI.current.SetPixelFront('3', 17 + 9 * i, 6 + 4 * j, tempKolor);
						SHGUI.current.SetPixelFront('2', 18 + 9 * i, 6 + 4 * j, tempKolor);
					}
					else if(mapValue[i, j] == 64)
					{
						SHGUI.current.SetPixelFront('6', 17 + 9 * i, 6 + 4 * j, tempKolor);
						SHGUI.current.SetPixelFront('4', 18 + 9 * i, 6 + 4 * j, tempKolor);
					}
					else if(mapValue[i, j] == 128)
					{
						SHGUI.current.SetPixelFront('1', 17 + 9 * i, 6 + 4 * j, tempKolor);
						SHGUI.current.SetPixelFront('2', 18 + 9 * i, 6 + 4 * j, tempKolor);
						SHGUI.current.SetPixelFront('8', 19 + 9 * i, 6 + 4 * j, tempKolor);
					}
					else if(mapValue[i, j] == 256)
					{
						SHGUI.current.SetPixelFront('2', 17 + 9 * i, 6 + 4 * j, tempKolor);
						SHGUI.current.SetPixelFront('5', 18 + 9 * i, 6 + 4 * j, tempKolor);
						SHGUI.current.SetPixelFront('6', 19 + 9 * i, 6 + 4 * j, tempKolor);
					}
					else if(mapValue[i, j] == 512)
					{
						SHGUI.current.SetPixelFront('5', 17 + 9 * i, 6 + 4 * j, tempKolor);
						SHGUI.current.SetPixelFront('1', 18 + 9 * i, 6 + 4 * j, tempKolor);
						SHGUI.current.SetPixelFront('2', 19 + 9 * i, 6 + 4 * j, tempKolor);
					}
					else if(mapValue[i, j] == 1024)
					{
						SHGUI.current.SetPixelFront('1', 16 + 9 * i, 6 + 4 * j, tempKolor);
						SHGUI.current.SetPixelFront('0', 17 + 9 * i, 6 + 4 * j, tempKolor);
						SHGUI.current.SetPixelFront('2', 18 + 9 * i, 6 + 4 * j, tempKolor);
						SHGUI.current.SetPixelFront('4', 19 + 9 * i, 6 + 4 * j, tempKolor);
					}
					else if(mapValue[i, j] == 2048)
					{
						SHGUI.current.SetPixelFront('2', 16 + 9 * i, 6 + 4 * j, 'r');
						SHGUI.current.SetPixelFront('0', 17 + 9 * i, 6 + 4 * j, 'r');
						SHGUI.current.SetPixelFront('4', 18 + 9 * i, 6 + 4 * j, 'r');
						SHGUI.current.SetPixelFront('8', 19 + 9 * i, 6 + 4 * j, 'r');
					}
					else if(mapValue[i, j] == 4096)
					{
						SHGUI.current.SetPixelFront('4', 16 + 9 * i, 6 + 4 * j, 'r');
						SHGUI.current.SetPixelFront('0', 17 + 9 * i, 6 + 4 * j, 'r');
						SHGUI.current.SetPixelFront('9', 18 + 9 * i, 6 + 4 * j, 'r');
						SHGUI.current.SetPixelFront('6', 19 + 9 * i, 6 + 4 * j, 'r');
					}
				}
			}
			//wyświetlanie splash efektów
			for(int i = 0; i < animList.Count; ++i)
			{
				for(int x = 0; x < 5; ++x)
				{
					for(int y = 0; y < 3; ++y)
					{
						if(boomAnim[y + 3 * animList[i].lifeTime][x] != ' ')
						{
							SHGUI.current.SetPixelFront(boomAnim[y + 3 * animList[i].lifeTime][x], 15 + 9 * animList[i].posX + x, 5 + 4 * animList[i].posY + y, 'w');
						}
					}
				}
			}

			if(lose == 1) //ekran przegranej
			{
				for(int i = 0; i < loseString.Length; ++i) //napis, że przegrałeś
				{
					SHGUI.current.SetPixelBack(loseString[i], (62 - loseString.Length)/2 + i, 9, 'r');
				}
				for(int i = 0; i < hintDead.Length; ++i) //co możesz zrobić jak zginiesz
				{
					SHGUI.current.SetPixelBack(hintDead[i], (62 - hintDead.Length)/2 + i, 12, 'r');
				}
			}
			if(lose == 2) //ekran wygranej
			{
				for(int i = 0; i < winString.Length; ++i) //napis, że przegrałeś
				{
					SHGUI.current.SetPixelBack(winString[i], (62 - winString.Length)/2 + i, 9, 'r');
				}
				for(int i = 0; i < hintDead.Length; ++i) //co możesz zrobić jak zginiesz
				{
					SHGUI.current.SetPixelBack(hintDead[i], (62 - hintDead.Length)/2 + i, 12, 'r');
				}
			}
		}
	}

	public override void ReactToInputKeyboard(SHGUIinput key) {
		if(menu)
		{
			if(key == SHGUIinput.esc) SHGUI.current.PopView();
			if(key == SHGUIinput.enter) //start / reset gry
			{
				for(int i = 0; i < 4; ++i) for(int j = 0; j < 4; ++j)
				{
					mapValue[i, j] = 0;
				}
				
				Rand2();
				Rand2();
				newTimer = 0f;
				
				for(int i = 0; i < 4; ++i)
				{
					lockDirection[i] = false;
					rope[i] = 0;
				}
				
				CheckMove();
				score = 0;
				allLockCount = 0;
				lose = 0;
				menu = false;

				currScoreString = "" + score;
				currBestString = "" + best;
			}
		}
		else
		{
			//sterowanie
			if(key == SHGUIinput.up) if(lose == 0) Move(0);
			if(key == SHGUIinput.down) if(lose == 0) Move(2);
			if (key == SHGUIinput.right) if(lose == 0) Move(1);
			if (key == SHGUIinput.left) if(lose == 0) Move(3);

			if(key == SHGUIinput.esc)
			{
				currScoreString = "" + score;
				currBestString = "" + best;
				menu = true;
			}
		}

	}
	void Move(int direct)
	{
		if(animBlock) return;

		//zapisanie tymczasowych wartości do mapy
		for(int i = 0; i < 4; ++i)
		{
			for(int j = 0; j < 4; ++j)
			{
				mapTemp[i,j] = mapValue[i,j];
			}
		}
		animBlock = true;
		animTimer = 0.3f;

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

							score += mapValue[i, j] * 2;
							mapValue[i, tempPos] += mapValue[i, j]; //zwiększanie tych samych wartości
							mapValue[i, j] = 0; //wyzerowanie starego zajmowanego pola
							++tempPos; //przesunięcie strażnika na następne pole by w jednym ruchu jakaś liczba nie zwiększyła się kilka razy

							animList.Add(new MergeAnim(i, tempPos - 1));
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

							score += mapValue[i, j] * 2;
							mapValue[tempPos, j] += mapValue[i, j]; //zwiększanie tych samych wartości
							mapValue[i, j] = 0; //wyzerowanie starego zajmowanego pola
							--tempPos; //przesunięcie strażnika na następne pole by w jednym ruchu jakaś liczba nie zwiększyła się kilka razy

							animList.Add(new MergeAnim(tempPos + 1, j));
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

							score += mapValue[i, j] * 2;
							mapValue[i, tempPos] += mapValue[i, j]; //zwiększanie tych samych wartości
							mapValue[i, j] = 0; //wyzerowanie starego zajmowanego pola
							--tempPos; //przesunięcie strażnika na następne pole by w jednym ruchu jakaś liczba nie zwiększyła się kilka razy

							animList.Add(new MergeAnim(i, tempPos + 1));
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

							score += mapValue[i, j] * 2;
							mapValue[tempPos, j] += mapValue[i, j]; //zwiększanie tych samych wartości
							mapValue[i, j] = 0; //wyzerowanie starego zajmowanego pola
							++tempPos; //przesunięcie strażnika na następne pole by w jednym ruchu jakaś liczba nie zwiększyła się kilka razy

							animList.Add(new MergeAnim(tempPos - 1, j));
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
		if(rev) //odwracanie sznura dla ułatwienia liczenia
		{
			int tempVal = rope[0];
			rope[0] = rope[3];
			rope[3] = tempVal;
			tempVal = rope[1];
			rope[1] = rope[2];
			rope[2] = tempVal;
		}
		for(int i = 0; i < 4; ++i) if(rope[i] == 0) ++FoundZero; //liczenie zer w sznurze

		if(FoundZero == 4) return true; //jeżeli są same zera
		else if(FoundZero == 3) //jeżeli są 3 zera
		{
			if(rope[0] != 0) return true; // jeżeli pierwszy element nie jest zerem
		}
		else if(FoundZero == 2) //jeżeli są 2 zera
		{
			if(rope[0] != 0) //jeżeli pierwszy element nie jest zerem
			{
				if(rope[1] != 0 && rope[1] != rope[0]) return true; //jeżeli nie zera lub rózne elementy zajują pierwsze pozycje
			}
		}
		else if(FoundZero == 1) //jeżeli jest 1 zero
		{
			if(rope[0] != 0) //jeżeli pierwszy element nie jest zerem
			{
				if(rope[1] != 0 && rope[1] != rope[0]) //jeżeli nie zera lub rózne elementy zajują pierwsze pozycje
				{
					if(rope[2] != 0 && rope[2] != rope[1]) return true; //jeżeli nie zera lub rózne elementy zajują pierwsze pozycje
				}
			}
		}
		else if(FoundZero == 0)
		{
			if(rope[0] != 0) //jeżeli pierwszy element nie jest zerem
			{
				if(rope[1] != 0 && rope[1] != rope[0]) //jeżeli nie zera lub rózne elementy zajują pierwsze pozycje
				{
					if(rope[2] != 0 && rope[2] != rope[1]) //jeżeli nie zera lub rózne elementy zajują pierwsze pozycje
					{
						if(rope[3] != 0 && rope[3] != rope[2]) //jeżeli nie zera lub rózne elementy zajują pierwsze pozycje
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	void Rand2() //losowanie pozycji na dołożenie dwujki do mapy
	{
		for(;;)
		{
			int tempPosX = Random.Range(0, 4);
			int tempPosY = Random.Range(0, 4);

			if(mapValue[tempPosX, tempPosY] == 0)
			{
				mapValue[tempPosX, tempPosY] = 2;
				newPosX = tempPosX;
				newPosY = tempPosY;
				newTimer = 0.2f;
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
								newPosX = i;
								newPosY = j;
								newTimer = 0.2f;
								return;
							}
						}
					}
				}
			}
		}
	}


}
