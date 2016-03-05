using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class APP2048 : SHGUIappbase {

	public	AlphabetMaterial2048		AM2048;

	int 								moveCount			= 0;
	int									lose				= 0;

	int[,]								mapValue			= new int[4,4]; //obecne wartości pól na mapie
	int[,]								mapTemp				= new int[4,4];

	bool[]								lockDirection		= new bool[4]; //oznaczenie który z kierunków jest zablokowany
	int									allLockCount		= 0; // ilosć zablokowanych kierunków jeżeli jest rónwa 4 przegrywasz
	int[]								rope				= new int[4]; //sznur czyli wartości w sprawdzanej lini

	//początkowe wartości
	public APP2048(): base("Alphabet-v1.6.9-by-onionmilk")
	{
		AM2048 = new AlphabetMaterial2048();

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
//===========================================================================================================================================
//											Wszystko co się dzieje podczas odświerzania logiki
//===========================================================================================================================================
	public override void Update()
	{
		//sprawdzenie warunku przegranej
		for(int i = 0; i < 4; ++i) if(lockDirection[i]) ++allLockCount;
		if(allLockCount == 4) lose = 1;
		else allLockCount = 0;

		//sprawdzenie warunku wygranej
		for(int i = 0; i < 4; ++i) for(int j = 0; j < 4; ++j)
		{
			if(mapValue[i, j] == 24) lose = 2;
		}

	}
//===========================================================================================================================================
//													rysowanie wszystkiego po kolei
//===========================================================================================================================================
	public override void Redraw(int offx, int offy)
	{
		for(int i = 0; i < 4; ++i)
		{
			for(int j = 0; j < 4; ++j)
			{
				for(int n = 0; n < 5; ++n)
				{
					for(int m = 0; m < AM2048.Alphabet[n].Length; ++m)
					{
						SHGUI.current.SetPixelFront(AM2048.Alphabet[n][m], 13 + m + i * 9, 1 + n + j * 5, 'z');
					}
				}
			}
		}
	}

//===========================================================================================================================================
//											pobieranie klawiszy i reagowanie na nie
//===========================================================================================================================================
	public override void ReactToInputKeyboard(SHGUIinput key)
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
			
			for(int i = 0; i < 4; ++i)
			{
				lockDirection[i] = false;
				rope[i] = 0;
			}
			
			CheckMove();
			allLockCount = 0;
			lose = 0;

			moveCount = 0;
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
				SHGUI.current.PopView();
			}
		}

	}
//===========================================================================================================================================
//									przesuwanie pól w odpowiedznie miejsce i ustawienie parametrów animacji
//===========================================================================================================================================
	void Move(int direct)
	{

		++moveCount; //zliczenie ilości ruchów

		//zapisanie tymczasowych wartości do mapy
		for(int i = 0; i < 4; ++i)
		{
			for(int j = 0; j < 4; ++j)
			{
				mapTemp[i, j] = mapValue[i,j];
			} 
		}

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

							mapValue[i, tempPos] += 1; //zwiększanie tych samych wartości
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

							mapValue[tempPos, j] += 1; //zwiększanie tych samych wartości
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

							mapValue[i, tempPos] += 1; //zwiększanie tych samych wartości
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

							mapValue[tempPos, j] += 1; //zwiększanie tych samych wartości
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

		CheckMove(); //sprawdzenie w które strony mozna się przemieścić

		for(int i = 0; i < 4; ++i)
		{
			for(int j = 0; j < 4; ++j)
			{
				mapTemp[i, j] = mapValue[i,j];
			} 
		}

		//wyświetlanie mapy w konsoli
		Debug.Log(mapValue[0, 0] + " " + mapValue[1, 0] + " " + mapValue[2, 0] + " " + mapValue[3, 0] + "\n" + 
			mapValue[0, 1] + " " + mapValue[1, 1] + " " + mapValue[2, 1] + " " + mapValue[3, 1]);
		Debug.Log(mapValue[0, 2] + " " + mapValue[1, 2] + " " + mapValue[2, 2] + " " + mapValue[3, 2] + "\n" + 
			mapValue[0, 3] + " " + mapValue[1, 3] + " " + mapValue[2, 3] + " " + mapValue[3, 3]);
	}
//===========================================================================================================================================
//							sprawdzenie w które strony można się przemieścić i blokowanie tych, w które nie można
//===========================================================================================================================================
	void CheckMove()
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
				if(rope[1] != 0 && rope[1] != rope[0]) return true; //jeżeli nie zera lub rózne elementy zajmują pierwsze pozycje
			}
		}
		else if(FoundZero == 1) //jeżeli jest 1 zero
		{
			if(rope[0] != 0) //jeżeli pierwszy element nie jest zerem
			{
				if(rope[1] != 0 && rope[1] != rope[0]) //jeżeli nie zera lub rózne elementy zajmują pierwsze pozycje
				{
					if(rope[2] != 0 && rope[2] != rope[1]) return true; //jeżeli nie zera lub rózne elementy zajują pierwsze pozycje
				}
			}
		}
		else if(FoundZero == 0)
		{
			if(rope[0] != 0) //jeżeli pierwszy element nie jest zerem
			{
				if(rope[1] != 0 && rope[1] != rope[0]) //jeżeli nie zera lub rózne elementy zajmują pierwsze pozycje
				{
					if(rope[2] != 0 && rope[2] != rope[1]) //jeżeli nie zera lub rózne elementy zajmują pierwsze pozycje
					{
						if(rope[3] != 0 && rope[3] != rope[2]) //jeżeli nie zera lub rózne elementy zajmmują pierwsze pozycje
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

//===========================================================================================================================================
//												losowanie nowej wartości na którymś z pól
//===========================================================================================================================================
	void Rand2() //losowanie pozycji na dołożenie dwujki do mapy
	{
		for(;;)
		{
			int tempPosX = Random.Range(0, 4);
			int tempPosY = Random.Range(0, 4);

			if(mapValue[tempPosX, tempPosY] == 0)
			{
				mapValue[tempPosX, tempPosY] = 1;
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
								mapValue[i, j] = 1;
								return;
							}
						}
					}
				}
			}
		}
	}

}
