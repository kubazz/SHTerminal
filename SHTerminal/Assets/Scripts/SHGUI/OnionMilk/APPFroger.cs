using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class crash
{
	public	int			graphic; //grafika onazczająca trzy kolejne linijki w stringu z pojazdami
	public	int			posX; //pozycja pozioma
	public	int			posY; //pozycja pionowa
	public	bool		left; //w którą stronę jest odwrócony
	public	int			width; //ilośćpól zajmowanych w poziomie

	public crash(int type, bool dir, int posx, int posy)
	{
		left = dir;
		graphic = type;
		posY = posy;
		posX = posx;
		if(type == 0) //skuterek
		{
			width = 4;
		}
		else if(type == 1) //niskie autko
		{
			width = 8;
		}
		else if(type == 2) //zwykłe autko
		{
			width = 8;
		}
		else if(type == 3) //karetka
		{
			width = 8;
		}
		else if(type == 4) //ciężarówka
		{
			width = 13;
		}
		else if(type == 5) //pociąg
		{
			width = 10;
		}
		else if(type == 6) //wagon
		{
			width = 14;
		}
	}
}

public class APPFroger : SHGUIappbase {

	//---------------------grafiki------------------------
	private string[]			cars	= new string[]
	{
		"  o ",
		" /# ",
		"o--o",
		"   __",
		" _/0 \\_ ",
		"'-o--o-'",
		"   __   ",
		" _/# \\_",
		"'0---0--",
		" ______ ",
		"/][]  +|",
		"'-o---o'",
		" __ _________",
		"|#  |||||||||",
		"\\-0--0--0--0/",
		" _____<__ ",
		"(`===HOT=|",
		"=o-o--o-o^",
		"_____________ ",
		"H==EXPRESS==H|",
		"-oo-------oo-^"
	};
	private string[]			carsRev	= new string[]
	{
		" o  ",
		" #\\ ",
		"o--o",
		"   __   ",
		" _/ 0\\_ ",
		"'-o--o-'",
		"   __   ",
		" _/ #\\_",
		"--0---0-'",
		" ______ ",
		"|+  [][\\",
		"'o---o-'",
		"_________ __ ",
		"|||||||||  #|",
		"\\0--0--0--0-/",
		" __>_____ ",
		"|=HOT===`)",
		"^o-o--o-o=",
		" _____________",
		"|H==EXPRESS==H",
		"^-oo-------oo-"
	};

	private string[]			animal = new string[]
	{
		" ()_() ",
		" (O.O) ",
		"'()”()'",
		" /\\_/\\ ",
		"(=^.^=)",
		" (”_”)/",
		"  ___  ",
		" ('v') ",
		"((,_,))",
		" ^___^ ",
		"( 'o' )",
		"( u u )",
		"(o)_(o)",
		"(     )",
		"(||_||)"
	};
	private string[]			animalBack = new string[]
	{
		" ()_() ",
		" (   ) ",
		"'(_o_)'",
		" /\\_/\\ ",
		"(     )",
		" (___)/",
		"  ___  ",
		" (   ) ",
		"((___))",
		" ^___^ ",
		"(     )",
		"(__@__)",
		"( )_( )",
		"(     )",
		"(_._._)"
	};

	private string[]			titleText = new string[]
	{
		"█  █ ███ █████   ███  ███ ███ ██  ███",
		"█  █ █ █   █     █  █ █ █ █ █ █ █ █  ",
		"████ █ █   █     ███  █ █ ███ █ █ ███",
		"█  █ █ █   █     █  █ █ █ █ █ █ █   █",
		"█  █ ███   █     █  █ ███ █ █ ██  ███"
	};


	char grass = '░';

	//-------Kolizje i rysowanie podlega zasadzie lewogo górnego rogu-------

	//postać
	int				posX			= 30; //pozycja żabki
	int				posY			= 0;

	int				yourDirection	= 1; //0 - przód | 1 - tył
	int				yourLook		= 0; //0 - królik | 1 - kot | 2 - ptak | 3 - świnka | 4 - żaba

	float			jumpTimer		= 0f; //do góry i dołu
	float			jumpTimer2		= 0f; //na boki

	//kamera
	int				cameraHeight	= 0;
	float			cameraJump		= 1f; //czas pomiędzy kolejnym przejściem kamery
	
	//pojazdy
	public			List<crash>		vehicle = new List<crash>();
	float			stepTime		= 0.07f; //ruchu

	//mechanika
	bool			menu			= true;
	bool			lose			= false;
	int[]			roadsMap		= new int[13];
	
	//manu
	string			pressText		= "==PRESS ENTER TO START==";
	string			unlockText		= "UNLOCK COST: 100";
	string			bestText		= "PERSONAL BEST: 0";
	string			moneyText		= "YOUR MONEY: 0";

	//zapis
	bool[]			lockTab			= new bool[] {false, true, true, true , true}; //zablokowane / odblokowane postacie
	int				piniadze		= 100; //ilość pinieniędzy
	int				best			= 0; //najlepszy osobisty wynik
	int				score			= 0; //obecny wynik podczas rozgrywki


	//---------------------------------------------------------------------

	public APPFroger() : base("hot_roads-v5.5.5-by-onionmilk")
	{
		for(int i = 0; i < roadsMap.Length; ++i)
		{
			if(i == 0 || i == 1 || i == 2 || i == 3 || i == 6 || i == 7 || i == 10 || i == 11) roadsMap[i] = 0;
			else roadsMap[i] = 1;
		}
	}
	
	public override void Update() 
	{
		base.Update();

		if(!menu)
		{
			stepTime -= Time.unscaledDeltaTime;

			//-----------------------podczas gry-------------------------------
			if(!lose) 
			{
				if(cameraHeight - 2 > posY) lose = true; //jeśli za długo stoisz w jednym miejscu

				jumpTimer -= Time.unscaledDeltaTime;
				jumpTimer2 -= Time.unscaledDeltaTime;

				cameraJump -= Time.unscaledDeltaTime;
				if(cameraJump <= 0f)
				{
					++cameraHeight;
					cameraJump = 1f;
				}

				//---------------------------przesuwanie pojazdu-------------------------------
				if(stepTime <= 0f)
				{
					stepTime = 0.07f;

					for(int i = 0; i < vehicle.Count;) //jeżeli minąłeś już jakieś auto i wyleciało poza obszar renderu to je wywalamy
					{
						if(vehicle[i].posY < (posY - 8))
						{
							vehicle.RemoveAt(i);
						}
						else
						{
							++i;
						}
					}
					
					for(int i = 0; i < vehicle.Count; ++i) //przesuwanie pojazdu
					{
						if(vehicle[i].left)
						{
							--vehicle[i].posX;
							if(vehicle[i].posX < (0 - vehicle[i].width)) vehicle[i].posX = 70;
						}
						else
						{
							++vehicle[i].posX;
							if(vehicle[i].posX > (64 + vehicle[i].width)) vehicle[i].posX = -15;
						}
					}
				}
				//-----------------------koniec przesuwania pojazdu-------------------------------


				//-----------------------kolizja-----------------------
				for(int i = 0; i < vehicle.Count; ++i)
				{
					if(vehicle[i].posY == posY) //jeśli jest na wysokości pojazdu
					{
						if(posX < vehicle[i].posX) //jeśli zwierzak jest z lewej strony auta
						{
							if(vehicle[i].posX < posX + 7)
							{
								lose = true;
							}
						}
						else if(posX > vehicle[i].posX) //jeśli zwierzak jest z prawej strony auta
						{
							if(vehicle[i].posX + vehicle[i].width > posX)
							{
								lose = true;
							}
						}
						else //wpadł równo
						{
							lose = true;
						}
					}

				}

				//-----------------------koniec kolizji-----------------------
			}
			else
			{
				Debug.Log("Umarłeś");
			}
		}
		else //--------------------------uaktualnianie tekstów-----------------
		{
			bestText		= "PERSONAL BEST: " + best;
			moneyText		= "YOUR MONEY: " + piniadze;

			if(lockTab[yourLook])
			{
				if(piniadze >= 100)
				{
					pressText		= "==PRESS ENTER TO BUY==";
				}
				else
				{
					pressText		= "";
				}
			}
			else
			{
				pressText		= "==PRESS ENTER TO START==";
			}
		}//-----------------koniec uaktualniania tekstów-----------------
	}
	//*******************************************************************************************
	public override void Redraw(int offx, int offy)
	{
		base.Redraw(offx, offy);

		if(menu)
		{
			for(int j = 0; j < 5; ++j)
			{
				for(int i = 0; i < titleText[j].Length; ++i)
				{
					SHGUI.current.SetPixelFront(titleText[j][i], 31 - (titleText[j].Length / 2) + i, j + 3, 'w');
				}
			}

			for(int i = 0; i < pressText.Length; ++i) //hint czym się włącza grę lub kupuje zwierzaka
			{
				SHGUI.current.SetPixelFront(pressText[i], 31 - (pressText.Length / 2) + i, 10, 'w');
			}

			for(int j = 0; j < 3; ++j) //rysowanie postaci
			{
				for(int i = 0; i < 7; ++i)
				{
					SHGUI.current.SetPixelFront(animal[j + (yourLook * 3)][i], 31 - (animal[j + (yourLook * 3)].Length / 2) + i, 15 + j, 'w');
				}
			}

			SHGUI.current.SetPixelFront('>', 37, 16, 'w');
			SHGUI.current.SetPixelFront('<', 25, 16, 'w');

			if(lockTab[yourLook]) //zablokowany / odblokowany zwierzak
			{
				for(int i = 0; i < unlockText.Length; ++i)
				{
					SHGUI.current.SetPixelFront(unlockText[i], 31 - (unlockText.Length / 2) + i, 13, 'z');
				}
			}

			for(int i = 0; i < bestText.Length; ++i)
			{
				SHGUI.current.SetPixelFront(bestText[i], 31 - (bestText.Length / 2) + i, 20, 'w');
			}

			for(int i = 0; i < moneyText.Length; ++i)
			{
				SHGUI.current.SetPixelFront(moneyText[i], 31 - (moneyText.Length / 2) + i, 21, 'z');
			}
		}
		else //))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))
		{
			//--------------------------------rysowanie mapy----------------------------------
			for(int l = 0; l < roadsMap.Length; ++l)
			{
				if(roadsMap[l] == 0) //rysowanie trawy
				{
					for(int j = 0; j < 3; ++j)
					{
						for(int i = 0; i < 62; ++i)
						{
							if(20 - (posY - cameraHeight) + j - (l * 3) + 9 < 23) //jeśli nie wychodzi za dolną krawędz
							{
								if(20 - (posY - cameraHeight) + j - (l * 3) + 9 > 0) //jesli nie wychodzi za górną krawędz
								{
									SHGUI.current.SetPixelFront(grass, i + 1, 20 - (posY - cameraHeight) + j - (l * 3) + 9, 'z'); //rysowanie trawy
								}
							}
						}
					}
				}
			}
			//---------------------------koniec rysowania trawy----------------------------------

			//------------------------------rysowanie postaci---------------------------------
			for(int j = 0; j < 3; ++j)
			{
				for(int i = 0; i < 7; ++i)
				{
					if(yourDirection == 0)
					{
						if(20 - (posY - cameraHeight) + j < 23)
						{
							SHGUI.current.SetPixelFront(animal[j + (yourLook * 3)][i], posX + i, 20 - (posY - cameraHeight) + j, 'w');
						}
					}
					else
					{
						if(20 - (posY - cameraHeight) + j < 23)
						{
							SHGUI.current.SetPixelFront(animalBack[j + (yourLook * 3)][i], posX + i, 20 - (posY - cameraHeight) + j, 'w');
						}
					}
				}
			}
			//----------------------koniec rysowania postaci---------------------------

			//-------------------------rysowanie pojazdów------------------------------
			for(int i = 0; i < vehicle.Count; ++i)
			{
				for(int y = 0; y < 3; ++y)
				{
					if(22 - (vehicle[i].posY - cameraHeight) + y > 0 && 20 - (vehicle[i].posY - cameraHeight) + y < 23) //żeby nie właziły na krawędź górną
					{
						for(int x = 0; x < cars[vehicle[i].graphic * 3 + y].Length; ++x)
						{
							if(vehicle[i].left) //rysowanie jadących w lewo
							{
								if(vehicle[i].posX + x > 0 && vehicle[i].posX + x < 63)
								{
									SHGUI.current.SetPixelFront(cars[vehicle[i].graphic * 3 + y][x], vehicle[i].posX + x, 20 - (vehicle[i].posY - cameraHeight) + y, 'z');
								}
							}
							else //rysowanie jadących w prawo
							{
								if(vehicle[i].posX + x > 0 && vehicle[i].posX + x < 63)
								{
									SHGUI.current.SetPixelFront(carsRev[vehicle[i].graphic * 3 + y][x], vehicle[i].posX + x, 20 - (vehicle[i].posY - cameraHeight) + y, 'z');
								}
							}
						}
					}
				}
			}
			//--------------------------koniec rysowania pojazdów----------------------------------
		}
	}
	
	//=======================================sterowanie======================================
	public override void ReactToInputKeyboard(SHGUIinput key)
	{
		if(key == SHGUIinput.enter)
		{
			if(menu)
			{
				if(!lockTab[yourLook]) //startowanie gry
				{
					//postać
					posX			= 30; //pozycja żabki
					posY			= 0;
					
					yourDirection	= 0; //0 - przód | 1 - tył
					
					stepTime		= 0.07f; //ruchu
					
					//kamera
					cameraHeight	= 0;
					cameraJump		= 1f; //czas pomiędzy kolejnym przejściem kamery

					//mapa
					for(int i = 0; i < roadsMap.Length; ++i)
					{
						if(i == 0 || i == 1 || i == 2 || i == 3 || i == 6 || i == 7 || i == 10 || i == 11) roadsMap[i] = 0;
						else roadsMap[i] = 1;
					}

					//mechanika
					for(int i = 0; i < vehicle.Count;) //niszczenie wszystkich aut
					{
						vehicle.RemoveAt(i);
					}
					menu = false;
					lose = false;
					score = 0;

					//przykladowe autka
					crash newCar = new crash(1, true, 5, 6);
					vehicle.Add(newCar);
					newCar = new crash(2, false, 6, 18);
					vehicle.Add(newCar);
					newCar = new crash(3, true, 20, 15);
					vehicle.Add(newCar);
					newCar = new crash(4, false, 32, 18);
					vehicle.Add(newCar);
					newCar = new crash(1, false, 30, 27);
					vehicle.Add(newCar);
					//hue

				}
				else if(piniadze >= 100)
				{
					piniadze -= 100;
					lockTab[yourLook] = false;
				}
			}
			else if(!menu && lose)
			{
				menu = true;
			}
		}
		if (key == SHGUIinput.up)
		{
			if(!menu && !lose && jumpTimer <= 0) //poruszanie w górę
			{
				jumpTimer = 0.3f;

				posY += 3; //przesuwanie do góry

				moveMap(); //losowanie nowego pola
				cameraJump = 1f; //ustawianie czasu do następnego ruchu kamerą
				yourDirection = 1; //obrócenie plecami
				cameraHeight += 2;

				if(posY - 4 > cameraHeight)
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
					jumpTimer = 0.3f;
				}
			}
		}
		if (key == SHGUIinput.right)
		{
			if(!menu && !lose && jumpTimer2 <= 0) //poruszanie w prawo
			{
				if(posX < 56)
				{
					++posX;
					jumpTimer2 = 0.1f;
				}
			}
			else if(menu)
			{
				++yourLook;
				if(yourLook > 4) yourLook = 0;
			}
		}
		if (key == SHGUIinput.left)
		{
			if(!menu && !lose) //poruszanie w lewo
			{
				if(posX > 1 && jumpTimer2 <= 0)
				{
					--posX;
					jumpTimer2 = 0.1f;
				}
			}
			else if(menu)
			{
				--yourLook;
				if(yourLook < 0) yourLook = 4;
			}
		}
		
		if (key == SHGUIinput.esc)
		{
			if(menu) SHGUI.current.PopView();

			if(!menu && !lose)
			{
				menu = true;
				lose = false;
			}

		}
	}
	//==============================koniec sterowania=================================
	
	void randVehicles()
	{
		if(posY < 20) //poziom 1 - 12
		{
			
		}
		else if(posY < 50) //poziom 2 - 20
		{
		
		}
		else if(posY < 150) //poziom 3 - 25
		{
		
		}
		else if(posY < 400) //poziom 4 - 30
		{
		
		}
		else if(posY < 900) //poziom 5 - 36
		{
		
		}
		else if(posY < 2000) //poziom 6 - 44
		{
		
		}
	}

	void moveMap()
	{
		for(int i = 0; i < roadsMap.Length-1; ++i)
		{
			roadsMap[i] = roadsMap[i + 1];
		}
		if(roadsMap[12] == 1 && roadsMap[11] == 1 && roadsMap[10] == 1 && roadsMap[9] == 1) roadsMap[12] = 0;
		else if(roadsMap[10] == 0 && roadsMap[11] == 0) roadsMap[12] = 1;
		else if(roadsMap[10] == 2 && roadsMap[11] == 2) roadsMap[12] = 0;
		else
		{
			roadsMap[12] = Random.Range(0, 2);
		}
	}
}