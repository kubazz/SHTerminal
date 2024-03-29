using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class APPHotRoads : SHGUIappbase {
	
	//---------------------grafiki------------------------
	HRgfx				gfx				= new HRgfx();
	HRTextManager		HRTM			= new HRTextManager();
	
	//-------Kolizje i rysowanie podlega zasadzie lewogo górnego rogu-------
	
	//postać
	int				posX			= 30; //pozycja żabki
	int				posY			= 0;
	
	int				yourDirection	= 1; //0 - przód | 1 - tył
	
	float			jumpTimer		= 0f; //do góry i dołu
	float			jumpTimer2		= 0f; //na boki

	int				sideAnim		= 0; //dodatkowa wysokość przy animowaniu chodzenia w bok
	float			sideAnimTime	= 0f; //czas bycia w powietrzu podczas chodzenia na boki
	
	//kamera
	int				cameraHeight	= 0;
	float			cameraJump		= 1f; //czas pomiędzy kolejnym przejściem kamery
	
	//pojazdy
	public			List<crash>		vehicle = new List<crash>(); //lista przechowująca pojazdy
	float			stepTime		= 0.07f; //ruchu
	
	public			List<crash>		train = new List<crash>(); //lista przechowująca pociągi
	float			stepTrainTime	= 0.03f; //ruch pociągu

	
	//mechanika
	bool			menu			= true; //czy jesteś w menu
	bool			lose			= false; //czy przegrałeś
	int[]			roadsMap		= new int[15]; //rodzaj podłoża
	int[]			stepCounter		= new int[15]; //za ile ma się zrespić kolejne autko
	bool[]			roadsDirection	= new bool[15]; //w którą stronę po danej ulicy się jeździ
	int				actualStep		= 5; //ilość skoków mapy od początku gry
	int				backAnimal		= 0; //jeśli się cofniesz pilnowanie by trawa się nie rozjeżdżała

	//lose
	float			loseMoveTime	= 0.2f;
	int				heightLose		= 22;
	int				endVehicleID	= 0;
	bool			typeCrash		= true;

	public APPHotRoads() : base("hot_roads-v5.5.5-by-onionmilk")
	{
		for(int i = 0; i < roadsMap.Length; ++i)
		{
			//ustawianie ulic i trawy
			if(i == 0 || i == 1 || i == 2 || i == 3 || i == 4 || i == 5 || i == 7 || i == 9 || i == 12 || i == 13) roadsMap[i] = 0;
			else roadsMap[i] = 1;
			
			//losowanie kierunków w których się będą poruszać pojazdy
			if(Random.value > 0.5) roadsDirection[i] = false;
			else roadsDirection[i] = true;
			
			//za ile ma się zrespić kolejne autko
			stepCounter[i] = Random.Range(8, 15);
		}
	}

	//================================================================================================================
	//								Update
	//================================================================================================================	
	public override void Update() 
	{
		base.Update();
		
		if(!menu)
		{
			stepTime -= Time.unscaledDeltaTime;
			stepTrainTime -= Time.unscaledDeltaTime;

			if(sideAnimTime >= 0)
			{
				sideAnimTime -= Time.unscaledDeltaTime;
				sideAnim = 1;
			}
			else
			{
				sideAnim = 0;
			}
			
			//-----------------------podczas gry-------------------------------
			if(!lose) 
			{
				HRTM.UpdateBestScore(posY);
				
				if(cameraHeight - 2 > posY)
				{
					lose = true; //jeśli za długo stoisz w jednym miejscu
					endVehicleID = -1;
				}
				
				jumpTimer -= Time.unscaledDeltaTime;
				jumpTimer2 -= Time.unscaledDeltaTime;
				
				cameraJump -= Time.unscaledDeltaTime;  //przesuwanie kamery
				if(cameraJump <= 0f)
				{
					++cameraHeight;
					cameraJump = 1f;
				}
				
				//================================================================================================================
				//											RUCH POJAZDÓW
				//================================================================================================================
				if(stepTime <= 0f)
				{
					stepTime = 0.07f;
					
					for(int i = 0; i < vehicle.Count;) //jeżeli minąłeś już jakieś auto i wyleciało poza obszar renderu to je wywalamy
					{
						if(vehicle[i].posY < (posY - 8)) vehicle.RemoveAt(i);
						else ++i;
					}
					
					for(int b = 0; b < roadsMap.Length; ++b) //generowanie nowych pojazdów
					{
						if(roadsMap[b] == 1)
						{
							--stepCounter[b];
							if(stepCounter[b] <= 0)
							{
								int tempWidth = randVehicles(b, 1);
								stepCounter[b] = tempWidth + Random.Range(15, 30);
							}
						}
					}
					
					for(int i = 0; i < vehicle.Count; ++i) //przesuwanie pojazdu
					{
						if(vehicle[i].left)
						{
							--vehicle[i].posX;
							if(vehicle[i].posX < (-20 - vehicle[i].width)) vehicle.RemoveAt(i); //jak dojdą do krawędzi to wywalamy
						}
						else
						{
							++vehicle[i].posX;
							if(vehicle[i].posX > (70 + vehicle[i].width)) vehicle.RemoveAt(i); //jak dojdą do krawędzi to wywalamy
						}
					}
				}
				
				if(stepTrainTime <= 0) //pociągi
				{
					stepTrainTime = 0.03f;
					
					for(int i = 0; i < train.Count;) //jeżeli minąłeś już jakieś pociąg i wyleciał on poza obszar renderu to go wywalamy
					{
						if(train[i].posY < (posY - 8)) train.RemoveAt(i);
						else ++i;

					}
					
					for(int b = 0; b < roadsMap.Length; ++b) //generowanie nowych pociągów
					{
						if(roadsMap[b] == 2)
						{
							--stepCounter[b];
							if(stepCounter[b] <= 0)
							{
								int tempWidth = randVehicles(b, 2);
								stepCounter[b] = 180 + Random.Range(0, 40);
							}
						}
					}
					
					for(int i = 0; i < train.Count; ++i) //przesuwanie pociągów
					{
						if(train[i].left)
						{
							--train[i].posX;
							if(train[i].posX < -80) train.RemoveAt(i); //jak dojdą do krawędzi to wywalamy
						}
						else
						{
							++train[i].posX;
							if(train[i].posX > 100) train.RemoveAt(i); //jak dojdą do krawędzi to wywalamy
						}
					}
				}
					
				//================================================================================================================
				//											KOLIZJA
				//================================================================================================================
				for(int i = 0; i < vehicle.Count; ++i)
				{
					if(vehicle[i].posY == posY) //jeśli jest na wysokości pojazdu
					{
						if(posX < vehicle[i].posX) //jeśli zwierzak jest z lewej strony auta
						{
							if(vehicle[i].posX < posX + 7)
							{
								lose = true;
								endVehicleID = i;
								typeCrash = true;
							}
						}
						else if(posX > vehicle[i].posX) //jeśli zwierzak jest z prawej strony auta
						{
							if(vehicle[i].posX + vehicle[i].width > posX)
							{
								lose = true;
								endVehicleID = i;
								typeCrash = true;
							}
						}
						else //wpadł równo
						{
							lose = true;
							endVehicleID = i;
							typeCrash = true;
						}
					}
					
				}
				for(int i = 0; i < train.Count; ++i)
				{
					if(train[i].posY == posY) //jeśli jest na wysokości pojazdu
					{
						if(posX < train[i].posX) //jeśli zwierzak jest z lewej strony auta
						{
							if(train[i].posX < posX + 7)
							{
								lose = true;
								endVehicleID = i;
								typeCrash = false;
							}
						}
						else if(posX > train[i].posX) //jeśli zwierzak jest z prawej strony auta
						{
							if(train[i].posX + train[i].width > posX)
							{
								lose = true;
								endVehicleID = i;
								typeCrash = false;
							}
						}
						else //wpadł równo
						{
							lose = true;
							endVehicleID = i;
							typeCrash = false;
						}
					}
				}
			}
			else
			{
				HRTM.UpdateLoseInGame();

				if(HRTM.firstFeil)
				{
					HRTM.failPrize = "You score " + (HRTM.best + Random.Range(3, 6)) + " points to get prize";
					HRTM.firstFeil = false;
				}
			}
		}
		else //--------------------------uaktualnianie tekstów-----------------
		{
			HRTM.UpdateInMenu();
		}
	}
	
	//================================================================================================================
	//												RYSOWANIE
	//================================================================================================================
	void drawGrass(int l, int i, int j)
	{
		if(20 - (posY + backAnimal * 3 - cameraHeight) + j - (l * 3) + 15 < 23) //jeśli nie wychodzi za dolną krawędz
		{
			if(20 - (posY + backAnimal * 3 - cameraHeight) + j - (l * 3) + 15 > 0) //jesli nie wychodzi za górną krawędz
			{
				SHGUI.current.SetPixelFront(',', i + 1, 20 - (posY + backAnimal * 3 - cameraHeight) + j - (l * 3) + 15, 'z'); //rysowanie trawy
			}
		}
	}

	public override void Redraw(int offx, int offy)
	{
		//================================================================================================================
		//											RYSOWANIE MENU
		//================================================================================================================
		if(menu)
		{
			//----------------Rysowanie tytułu---------------------------------------------
			for(int j = 0; j < 5; ++j)
			{
				for(int i = 0; i < gfx.titleText[j].Length; ++i)
				{
					SHGUI.current.SetPixelFront(gfx.titleText[j][i], 31 - (gfx.titleText[j].Length / 2) + i, j + 3, 'w');
				}
			}
			//------------------hint czym się włącza grę---------------
			for(int i = 0; i < HRTM.pressText.Length; ++i) 
			{
				SHGUI.current.SetPixelFront(HRTM.pressText[i], 31 - (HRTM.pressText.Length / 2) + i, 10, 'w');
			}
			//---------------------------rysowanie postaci---------------------------------
			for(int j = 0; j < 3; ++j) 
			{
				for(int i = 0; i < 7; ++i)
				{
					if(gfx.animal[j][i] != '?')
					{
						SHGUI.current.SetPixelFront(gfx.animal[j][i], 31 - (gfx.animal[j].Length / 2) + i, 15 + j, 'w');
					}
				}
			}

			for(int i = 0; i < HRTM.bestText.Length; ++i)
			{
				SHGUI.current.SetPixelFront(HRTM.bestText[i], 31 - (HRTM.bestText.Length / 2) + i, 20, 'w');
			}
		}
		else if(!menu)
		{
			//================================================================================================================
			//											RYSOWANIE MAPY
			//================================================================================================================
			base.Redraw(offx, offy);
			//--------------------------------rysowanie mapy----------------------------------
			for(int l = 0; l < roadsMap.Length; ++l)
			{
				if(roadsMap[l] == 0) //rysowanie trawy
				{
					for(int j = 0; j < 3; ++j)
					{
						for(int i = 0; i < 62; ++i)
						{
							if((l%2 == 0 && posY%2 == 0) || (l%2 == 1 && posY%2 == 1))
							{
								if((i%6 == 0 && j%3 == 0) || (i%6 == 3 && j%3 == 1) || (i%6 == 0 && j%3 == 2))
								{
									drawGrass(l, i, j);
								}
							}
							if((l%2 == 1 && posY%2 == 0) || (l%2 == 0 && posY%2 == 1))
							{
								if((i%6 == 3 && j%3 == 0) || (i%6 == 0 && j%3 == 1) || (i%6 == 3 && j%3 == 2))
								{
									drawGrass(l, i, j);
								}
							}
						}
					}
				}
				else if(roadsMap[l] == 2) //rysowanie torów
				{
					for(int j = 1; j < 3; ++j)
					{
						for(int i = 0; i < 62; ++i)
						{
							if(j == 1 && i%2 == 1)
							{
								if(20 - (posY + backAnimal * 3 - cameraHeight) + j - (l * 3) + 15 < 23) //jeśli nie wychodzi za dolną krawędz
								{
									if(20 - (posY + backAnimal * 3 - cameraHeight) + j - (l * 3) + 15 > 0) //jesli nie wychodzi za górną krawędz
									{
										SHGUI.current.SetPixelFront('_', i + 1, 20 - (posY + 3 * backAnimal - cameraHeight) + j - (l * 3) + 15, 'z'); //rysowanie szyny
									}
									
								}
							}
							else if(j == 2 && i%2 == 0)
							{
								if(20 - (posY + backAnimal * 3 - cameraHeight) + j - (l * 3) + 15 < 23) //jeśli nie wychodzi za dolną krawędz
								{
									if(20 - (posY + backAnimal * 3 - cameraHeight) + j - (l * 3) + 15 > 0) //jesli nie wychodzi za górną krawędz
									{
										SHGUI.current.SetPixelFront('/', i + 1, 20 - (posY + 3 * backAnimal - cameraHeight) + j - (l * 3) + 15, 'z'); //rysowanie podkładów
									}
								}
								
							}
							else if(j == 2 && i%2 == 1)
							{
								if(20 - (posY + backAnimal * 3 - cameraHeight) + j - (l * 3) + 15 < 23) //jeśli nie wychodzi za dolną krawędz
								{
									if(20 - (posY + backAnimal * 3 - cameraHeight) + j - (l * 3) + 15 > 0) //jesli nie wychodzi za górną krawędz
									{
										SHGUI.current.SetPixelFront('_', i + 1, 20 - (posY + 3 * backAnimal - cameraHeight) + j - (l * 3) + 15, 'z'); //rysowanie szyny
									}
								}
							}
						}
					}
				}
			}

			//================================================================================================================
			//											RYSOWANIE POJAZDÓW
			//================================================================================================================
			for(int i = 0; i < vehicle.Count; ++i)
			{
				for(int y = 0; y < 3; ++y)
				{
					if(22 - (vehicle[i].posY - cameraHeight) + y > 0 && 20 - (vehicle[i].posY - cameraHeight) + y < 23) //żeby nie właziły na krawędź górną
					{
						for(int x = 0; x < gfx.cars[vehicle[i].graphic * 3 + y].Length; ++x)
						{
							if(vehicle[i].left) //rysowanie jadących w lewo
							{
								if(vehicle[i].posX + x > 0 && vehicle[i].posX + x < 63)
								{
									if((20 - (vehicle[i].posY - cameraHeight) + y) > 0 && (20 - (vehicle[i].posY - cameraHeight) + y) < 23)
									{
										if(gfx.cars[vehicle[i].graphic * 3 + y][x] != '?') //sprawdzanie maski przeźroczystości
										{
											SHGUI.current.SetPixelFront(gfx.cars[vehicle[i].graphic * 3 + y][x], vehicle[i].posX + x, 20 - (vehicle[i].posY - cameraHeight) + y, 'z');
										}
									}
								}
							}
							else //rysowanie jadących w prawo
							{
								if(vehicle[i].posX + x > 0 && vehicle[i].posX + x < 63)
								{
									if((20 - (vehicle[i].posY - cameraHeight) + y) > 0 && (20 - (vehicle[i].posY - cameraHeight) + y) < 23)
									{
										if(gfx.carsRev[vehicle[i].graphic * 3 + y][x] != '?') //sprawdzanie maski przeźroczystości
										{
											SHGUI.current.SetPixelFront(gfx.carsRev[vehicle[i].graphic * 3 + y][x], vehicle[i].posX + x, 20 - (vehicle[i].posY - cameraHeight) + y, 'z');
										}
									}
								}
							}
						}
					}
				}
			}
			for(int i = 0; i < train.Count; ++i) //pociągi
			{
				for(int y = 0; y < 3; ++y)
				{
					if(22 - (train[i].posY - cameraHeight) + y > 0 && 20 - (train[i].posY - cameraHeight) + y < 23) //żeby nie właziły na krawędź górną
					{
						for(int x = 0; x < gfx.cars[train[i].graphic * 3 + y].Length; ++x)
						{
							if(train[i].left) //rysowanie jadących w lewo
							{
								if(train[i].posX + x > 0 && train[i].posX + x < 63)
								{
									if((20 - (train[i].posY - cameraHeight) + y) > 0 && (20 - (train[i].posY - cameraHeight) + y) < 23)
									{
										if(gfx.cars[train[i].graphic * 3 + y][x] != '?')
										{
											SHGUI.current.SetPixelFront(gfx.cars[train[i].graphic * 3 + y][x], train[i].posX + x, 20 - (train[i].posY - cameraHeight) + y, 'z');
										}
									}
								}
							}
							else //rysowanie jadących w prawo
							{
								if(train[i].posX + x > 0 && train[i].posX + x < 63)
								{
									if((20 - (train[i].posY - cameraHeight) + y) > 0 && (20 - (train[i].posY - cameraHeight) + y) < 23)
									{
										if(gfx.carsRev[train[i].graphic * 3 + y][x] != '?')
										{
											SHGUI.current.SetPixelFront(gfx.carsRev[train[i].graphic * 3 + y][x], train[i].posX + x, 20 - (train[i].posY - cameraHeight) + y, 'z');
										}
									}
								}
							}
						}
					}
				}
			}
			//================================================================================================================
			//											RYSOWANIE POSTACI
			//================================================================================================================
			for(int j = 0; j < 3; ++j)
			{
				for(int i = 0; i < 7; ++i)
				{
					if(yourDirection == 0)
					{
						if(20 - (posY - cameraHeight) + j < 23)
						{
							if(gfx.animal[j][i] != '?')
							{
								if(lose)
									SHGUI.current.SetPixelFront(gfx.animal[j][i], posX + i, 20 - (posY - cameraHeight + sideAnim) + j, 'r');
								else
									SHGUI.current.SetPixelFront(gfx.animal[j][i], posX + i, 20 - (posY - cameraHeight + sideAnim) + j, 'w');
							}
						}
					}
					else
					{
						if(20 - (posY - cameraHeight) + j < 23)
						{
							if(gfx.animalBack[j][i] != '?')
							{
								if(lose)
									SHGUI.current.SetPixelFront(gfx.animalBack[j][i], posX + i, 20 - (posY - cameraHeight + sideAnim) + j, 'r');
								else
									SHGUI.current.SetPixelFront(gfx.animalBack[j][i], posX + i, 20 - (posY - cameraHeight + sideAnim) + j, 'w');
							}
						}
					}
				}
			}
			//================================================================================================================
			//										RYSOWANIE PUNKTÓW
			//================================================================================================================
			if(!lose)
			{				
				HRTM.gameBestText = "Best " + HRTM.best;
				for(int n = 0; n < HRTM.gameBestText.Length; ++n)
				{
					SHGUI.current.SetPixelFront(HRTM.gameBestText[n], n + 1, 7, 'w');
				}
				
				HRTM.gameScoreText = "" + HRTM.score;
				
				for(int k = 0; k < HRTM.gameScoreText.Length; ++k)
				{
					for(int i = 0; i < 5; ++i)
					{
						for(int j = 0; j < 5; ++j) //rysowanie wyniku
						{
							SHGUI.current.SetPixelFront(gfx.Number[i+(int.Parse(HRTM.gameScoreText[k].ToString()) * 5)][j], j + k * 6 + 1, 1 + i, 'w');
						}
					}
				}
			}
			//================================================================================================================
			//											RYSOWANIE INTERFACE PO PRZEGRANEJ
			//================================================================================================================
			if(lose)
			{
				if(heightLose > 0)
				{
					loseMoveTime -= Time.unscaledDeltaTime;
					if(loseMoveTime <= 0f)
					{
						loseMoveTime = 0.04f;
						--heightLose;
					}
				}

				for(int i = 1; i < 63; ++i) //zaciemnienie mapy
				{
					for(int j = 1; j < 23; ++j)
					{
						SHGUI.current.SetPixelFront(' ', i, j + heightLose, 'w'); 
					}
				}

				//rysowanie omyłkowej wiadomości
				for(int n = 0; n < HRTM.failPrize.Length; ++n)
				{
					SHGUI.current.SetPixelFront(HRTM.failPrize[n], (62 - HRTM.failPrize.Length) / 2 + n, 4 + heightLose, 'w');
				}

				//rysowanie wyniku
				HRTM.gameScoreText = "" + HRTM.score;
				int tempPaddingLeft = 31 - (HRTM.gameScoreText.Length*3); //wyznaczanie środka
				
				for(int k = 0; k < HRTM.gameScoreText.Length; ++k)
				{
					for(int i = 0; i < 5; ++i)
					{
						for(int j = 0; j < 5; ++j)
						{
							SHGUI.current.SetPixelFront(gfx.Number[i+(int.Parse(HRTM.gameScoreText[k].ToString()) * 5)][j], j + k * 6 + tempPaddingLeft, 8 + i + heightLose, 'w');
						}
					}
				}
				
				if(HRTM.score == HRTM.best) //nowy najlepszy wynik
				{
					for(int n = 0; n < HRTM.endBestText.Length; ++n)
					{
						SHGUI.current.SetPixelFront(HRTM.endBestText[n], (62 - HRTM.endBestText.Length) / 2 + n, 6 + heightLose, 'w');
					}
				}
				else //obecny najlepszy
				{
					HRTM.bestText = "PERSONAL BEST: " + HRTM.best;
					
					for(int n = 0; n < HRTM.bestText.Length; ++n)
					{
						SHGUI.current.SetPixelFront(HRTM.bestText[n], (62 - HRTM.bestText.Length) / 2 + n, 6 + heightLose, 'w');
					}
				}
				
				for(int j = 0; j < 3; ++j) //postać
				{
					for(int i = 0; i < 7; ++i)
					{
						if(gfx.animal[j][i] != '?')
						{
							SHGUI.current.SetPixelFront(gfx.animal[j][i], 31 - (gfx.animal[j].Length / 2) + i, 17 + j + heightLose, 'w');
						}
					}
				}
				
				for(int n = 0; n < HRTM.pressText.Length; ++n) //czym się resetuje grę
				{
					SHGUI.current.SetPixelFront(HRTM.pressText[n], (62 - HRTM.pressText.Length) / 2 + n, 22 + heightLose, 'w');
				}
					
			}
		}
	}
	
	//=======================================sterowanie======================================
	public override void ReactToInputKeyboard(SHGUIinput key)
	{
		if(key == SHGUIinput.enter)
		{
			if(menu || lose)
			{
				resetGame();
			}
		}
		if (key == SHGUIinput.up)
		{
			if(!menu && !lose && jumpTimer <= 0) //poruszanie w górę
			{
				jumpTimer = 0.2f;
				
				posY += 3; //przesuwanie do góry
				
				if(backAnimal == 0) moveMap(); //losowanie nowego pola
				cameraJump = 1f; //ustawianie czasu do następnego ruchu kamerą
				yourDirection = 1; //obrócenie plecami
				cameraHeight += 2;
				
				if(backAnimal > 0) backAnimal = backAnimal - 1;
				
				if(posY - 10 > cameraHeight)
				{
					cameraHeight += 1;
				}
			}
			
		}
		if (key == SHGUIinput.down)
		{
			if(!menu && !lose && jumpTimer <= 0) //poruszanie w dół
			{
				if(!(posY - 3 < cameraHeight)) //sprawdzanie, żeby postać nie wyszła poza obszar kamery
				{
					posY -= 3;
					yourDirection = 0;
					jumpTimer = 0.2f;
					backAnimal = backAnimal + 1;
				}
			}
		}
		if (key == SHGUIinput.right)
		{
			if(!menu && !lose && jumpTimer2 <= 0) //poruszanie w prawo
			{
				if(posX < 54)
				{
					posX+=3;
					jumpTimer2 = 0.15f;
					sideAnimTime = 0.1f;
				}
			}
		}
		if (key == SHGUIinput.left)
		{
			if(!menu && !lose && jumpTimer2 <= 0) //poruszanie w lewo
			{
				if(posX > 3)
				{
					posX-=3;
					jumpTimer2 = 0.15f;
					sideAnimTime = 0.1f;
				}
			}
		}
		
		if (key == SHGUIinput.esc)
		{
			SHGUI.current.PopView();
		}
	}
	//==============================koniec sterowania=================================
	
	int randVehicles(int road, int type)
	{
		int tempPosY = (road + actualStep - 10) * 3;
		int tempPosX = 0;
		
		int tempType = Random.Range(0, 5);
		int tempWidth = 0;
		
		if(tempType == 0) tempWidth = 4;
		else if(tempType == 1) tempWidth = 8;
		else if(tempType == 2) tempWidth = 8;
		else if(tempType == 3) tempWidth = 8;
		else if(tempType == 4) tempWidth = 13;
		
		if(roadsDirection[road])
		{
			tempPosX = 63;
		}
		else
		{
			tempPosX = (-tempWidth) - 1;
		}
		
		if(type == 1) //auto
		{
			crash newCar = new crash(tempType, roadsDirection[road], tempPosX, tempPosY);
			vehicle.Add(newCar);
			
		}
		else //pociąg
		{
			if(roadsDirection[road])
			{
				tempPosX = 70;
				
				crash newTrain = new crash(5, roadsDirection[road], tempPosX, tempPosY);
				train.Add(newTrain);
				tempPosX += 11;
				newTrain = new crash(6, roadsDirection[road], tempPosX, tempPosY);
				train.Add(newTrain);
				for(int p = 0; p < 5; ++p)
				{
					tempPosX += 15;
					newTrain = new crash(6, roadsDirection[road], tempPosX, tempPosY);
					train.Add(newTrain);
				}
			}
			else
			{
				tempPosX = -5;
				
				crash newTrain = new crash(5, roadsDirection[road], tempPosX, tempPosY);
				train.Add(newTrain);
				tempPosX -= 11;
				newTrain = new crash(6, roadsDirection[road], tempPosX, tempPosY);
				train.Add(newTrain);
				for(int p = 0; p < 5; ++p)
				{
					tempPosX -= 15;
					newTrain = new crash(6, roadsDirection[road], tempPosX, tempPosY);
					train.Add(newTrain);
				}
			}
			return 0;
		}
		
		return vehicle[vehicle.Count - 1].width;
	}
	
	void resetGame()
	{
		//postać
		posX			= 30; //pozycja żabki
		posY			= 0;
		
		yourDirection	= 0; //0 - przód | 1 - tył
		backAnimal		= 0;

		stepTime		= 0.07f; //ruchu
		sideAnimTime	= 0f;
		sideAnim		= 0;
		
		actualStep		= 5;
		
		//kamera
		cameraHeight	= -3;
		cameraJump		= 1f; //czas pomiędzy kolejnym przejściem kamery

		loseMoveTime	= 0.2f;
		heightLose		= 22;
		endVehicleID	= 0;
		
		//mapa
		//ustawianie ulic i trawy
		for(int i = 0; i < roadsMap.Length; ++i)
		{
			//ustawianie ulic i trawy
			if(i == 0 || i == 1 || i == 2 || i == 3 || i == 4 || i == 5 || i == 7 || i == 9 || i == 12 || i == 13) roadsMap[i] = 0;
			else roadsMap[i] = 1;
			
			//losowanie kierunków w których się będą poruszać pojazdy
			if(i == 0)
			{
				if(Random.value > 0.5f) roadsDirection[i] = false;
				else roadsDirection[i] = true;
			}
			else
			{
				if(roadsDirection[i - 1]) roadsDirection[i] = false;
				else roadsDirection[i] = true;
			}
			
			//za ile ma się zrespić kolejne autko
			stepCounter[i] = Random.Range(8, 15);
		}
		
		//mechanika
		for(int i = 0; i < vehicle.Count;) //niszczenie wszystkich aut
		{
			vehicle.RemoveAt(i);
		}
		for(int i = 0; i < train.Count;) //niszczenie wszystkich pociągów
		{
			train.RemoveAt(i);
		}
		menu = false;
		lose = false;
		HRTM.score = 0;
		HRTM.firstFeil = true;
		
		//wstepne autka
		for(int b = 5; b < roadsMap.Length; ++b)
		{
			if(roadsMap[b] == 1)
			{
				crash newCar = new crash(Random.Range(0, 5), roadsDirection[b], Random.Range(20, 40), (actualStep + b - 10) * 3);
				vehicle.Add(newCar);
			}
		}
	}
	
	void moveMap()
	{
		++actualStep;
		
		for(int i = 0; i < roadsMap.Length - 1; ++i) //przesuwanie mapy
		{
			roadsMap[i] = roadsMap[i + 1];
			roadsDirection[i] = roadsDirection[i + 1];
			stepCounter[i] = stepCounter[i + 1];
		}
		//generowanie nowego odcinka mapy
		if(roadsMap[13] == 1 && roadsMap[12] == 1 && roadsMap[11] == 1 && roadsMap[10] == 1) roadsMap[14] = 0;
		else if(roadsMap[12] == 0 && roadsMap[13] == 0) roadsMap[14] = 1;
		else if(roadsMap[12] == 2 && roadsMap[13] == 2) roadsMap[14] = 0;
		else
		{
			roadsMap[14] = Random.Range(0, 2);
			if(Random.value > 0.90)
			{
				if(roadsMap[12] != 2 || roadsMap[13] != 2)
				{
					roadsMap[14] = 2;
				}
			}
		}
		
		//losowanie kierunków w których się będą poruszać pojazdy
		if(roadsDirection[13] == true) roadsDirection[14] = false;
		else roadsDirection[14] = true;
		
		//za ile ma się zrespić kolejne autko
		stepCounter[14] = Random.Range(4, 10);
		
		if(roadsMap[14] == 1)
		{
			crash newCar = new crash(Random.Range(0, 5), roadsDirection[14], Random.Range(15, 40), (actualStep + 11) * 3);
			vehicle.Add(newCar);
		}
	}
}