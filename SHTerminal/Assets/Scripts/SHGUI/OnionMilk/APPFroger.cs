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

public class coin
{
	public	int		posX;
	public	int		posY;

	public coin(int px, int py)
	{
		posX = px;
		posY = py;
	}
}

public class APPFroger : SHGUIappbase {

	//---------------------grafiki------------------------
	//liczby
	private string[]	Number			= new string[] {
		" ███ ",
		"█  ██",
		"█ █ █",
		"██  █",
		" ███ ",
		"  █  ",
		" ██  ",
		"  █  ",
		"  █  ",
		" ███ ",
		"█████",
		"    █",
		"█████",
		"█    ",
		"█████",
		"████ ",
		"    █",
		"████ ",
		"    █",
		"████ ",
		"█    ",
		"█  █ ",
		"█████",
		"   █ ",
		"   █ ",
		"█████",
		"█    ",
		"█████",
		"    █",
		"████ ",
		" ███ ",
		"█    ",
		"████ ",
		"█   █",
		" ███ ",
		"█████",
		"   █ ",
		"  █  ",
		" █   ",
		"█    ",
		" ███ ",
		"█   █",
		" ███ ",
		"█   █",
		" ███ ",
		" ███ ",
		"█   █",
		" ████",
		"    █",
		" ███ "
	};

	private string[]			cars	= new string[]
	{
		"??o?",
		"?/#?",
		"o--o",
		"???__???",
		"?_/0 \\_?",
		"'-o--o-'",
		"???__???",
		"?_/# \\_",
		"'0---0--",
		"?______?",
		"/][]  +|",
		"'-o---o'",
		"?__ _________",
		"|#  |||||||||",
		"\\-0--0--0--0/",
		"?_____<__?",
		"(`===HOT=|",
		"=o-o--o-o^",
		"_____________?",
		"H==EXPRESS==H|",
		"-oo-------oo-^"
	};
	private string[]			carsRev	= new string[]
	{
		"?o??",
		"?#\\?",
		"o--o",
		"???__???",
		"?_/ 0\\_?",
		"'-o--o-'",
		"???__???",
		"?_/ #\\_",
		"--0---0-'",
		" ______ ",
		"|+  [][\\",
		"'o---o-'",
		"_________ __ ",
		"|||||||||  #|",
		"\\0--0--0--0-/",
		"?__>_____?",
		"|=HOT===`)",
		"^o-o--o-o=",
		"?_____________",
		"|H==EXPRESS==H",
		"^-oo-------oo-"
	};

	private string[]			animal = new string[]
	{
		"?()_()?",
		"?(O.O)?",
		"'()”()'",
		"?/\\_/\\?",
		"(=^.^=)",
		"?(”_”)/",
		"?(^v^)?",
		"<(   )>",
		"??^?^??",
		" ^___^ ",
		"( 'o' )",
		"( u u )",
		"(o)_(o)",
		"(     )",
		"(||_||)"
	};
	private string[]			animalBack = new string[]
	{
		"?()_()?",
		"?(   )?",
		"'(_o_)'",
		"?/\\_/\\?",
		"(     )",
		"?(___)/",
		"?(¯¯¯)?",
		"<(   )>",
		"??^?^??",
		"?^___^?",
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

	private string[]			coinLook = new string[]
	{
		"/¯¯¯\\",
		"| $ |",
		"\\___/"
	};

	char grass = '"';

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
	public			List<crash>		vehicle = new List<crash>(); //lista przechowująca pojazdy
	float			stepTime		= 0.07f; //ruchu

	public			List<crash>		train = new List<crash>(); //lista przechowująca pociągi
	float			stepTrainTime	= 0.03f; //ruch pociągu

	//kasa
	public			List<coin>		coinList = new List<coin>(); //lista przechowujące monety
	bool			onMoney			= true;
	float			timeShowMoney	= 0.3f;

	//mechanika
	bool			menu			= true; //czy jesteś w menu
	bool			lose			= false; //czy przegrałeś
	int[]			roadsMap		= new int[13]; //rodzaj podłoża
	int[]			stepCounter		= new int[13]; //za ile ma się zrespić kolejne autko
	bool[]			roadsDirection	= new bool[13]; //w którą stronę po danej ulicy się jeździ
	int				actualStep		= 3; //ilość skoków mapy od początku gry
	bool			backAnimal		= false; //jeśli się cofniesz pilnowanie by trawa się nie rozjeżdżała
	
	//manu
	string			pressText		= "==PRESS ENTER TO START==";
	string			unlockText		= "UNLOCK COST: 100";
	string			bestText		= "PERSONAL TOP: 0";
	string			moneyText		= "YOUR MONEY: 0";

	//end game
	string			restartText		= "==PRESS ENTER TO RESTART==";
	string			endText			= "==PRESS ESCAPE TO BECK MENU==";
	string			endPointsText	= "SCORE ";
	string			endBestText		= "NEW TOP SCORE";
	bool			mryga			= true;
	float			mrygaTimer		= 0.3f;

	//zapis
	bool[]			lockTab			= new bool[] {false, true, true, true , true}; //zablokowane / odblokowane postacie
	int				piniadze		= 100; //ilość pinieniędzy
	int				best			= 0; //najlepszy osobisty wynik
	int				score			= 0; //obecny wynik podczas rozgrywki
	int				oldBest			= 0; //nallepszy wynik jaki był w poprzedniej rundzie
	string			gameMoneyText	= "0$";
	string			gameScoreText	= "Score ";
	string			gameBestText	= "Top ";

	//---------------------------------------------------------------------

	public APPFroger() : base("hot_roads-v5.5.5-by-onionmilk")
	{
		for(int i = 0; i < roadsMap.Length; ++i)
		{
			//ustawianie ulic i trawy
			if(i == 0 || i == 1 || i == 2 || i == 3 || i == 6 || i == 7 || i == 10 || i == 11) roadsMap[i] = 0;
			else roadsMap[i] = 1;

			//losowanie kierunków w których się będą poruszać pojazdy
			if(Random.value > 0.5) roadsDirection[i] = false;
			else roadsDirection[i] = true;

			//za ile ma się zrespić kolejne autko
			stepCounter[i] = Random.Range(8, 15);
		}


	}
	/*
	//PARTIKLE - tekst który się pokazuje na pewien okres czasu
	void Particle(){
		var ziom = new SHGUItext(SHGUI.current.GetASCIIartFromFont("235"), 5, 5, 'w'); //treść napisu miejsce i kolor
		var temp = new SHGUItempview(.3f); //czs po którym się on wyłącza
		temp.overrideFadeInSpeed = .5f; //mnożnik przyśpieszenia wejścia
		temp.overrideFadeOutSpeed = .5f; //mnożnik przyśpieszenia wyjścia
		temp.AddSubView(ziom); //podpięcie napisu pod czas życia
		AddSubView(temp); //dodanie widoku do widocznej sceny
	}*/
	public override void Update() 
	{
		base.Update();

		if(!menu)
		{
			stepTime -= Time.unscaledDeltaTime;
			stepTrainTime -= Time.unscaledDeltaTime;

			timeShowMoney -= Time.unscaledDeltaTime;
			if(timeShowMoney <= 0f)
			{
				if(onMoney)
				{
					onMoney = false;
					timeShowMoney = 0.3f;
				}
				else
				{
					onMoney = true;
					timeShowMoney = 0.6f;
				}
			}

			//-----------------------podczas gry-------------------------------
			if(!lose) 
			{
				if(score < (posY / 3)) score = posY / 3;
				if(best < score) best = score;

				if(cameraHeight - 2 > posY) lose = true; //jeśli za długo stoisz w jednym miejscu

				jumpTimer -= Time.unscaledDeltaTime;
				jumpTimer2 -= Time.unscaledDeltaTime;

				cameraJump -= Time.unscaledDeltaTime;
				if(cameraJump <= 0f)
				{
					++cameraHeight;
					cameraJump = 1f;
				}

				//-----------------------------usówanie zbędnych monet------------------------
				for(int i = 0; i < coinList.Count;) //jeżeli minąłeś już jakąś monete i wyleciała poza obszar renderu to ją wywalamy
				{
					if(coinList[i].posY < (posY - 8))
					{
						coinList.RemoveAt(i);
					}
					else
					{
						++i;
					}
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
						if(train[i].posY < (posY - 8))
						{
							train.RemoveAt(i);
						}
						else
						{
							++i;
						}
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
				for(int i = 0; i < train.Count; ++i)
				{
					if(train[i].posY == posY) //jeśli jest na wysokości pojazdu
					{
						if(posX < train[i].posX) //jeśli zwierzak jest z lewej strony auta
						{
							if(train[i].posX < posX + 7)
							{
								lose = true;
							}
						}
						else if(posX > train[i].posX) //jeśli zwierzak jest z prawej strony auta
						{
							if(train[i].posX + train[i].width > posX)
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
				//---------------------zmieranie piniądzów------------------

				for(int i = 0; i < coinList.Count; ++i)
				{
					if(coinList[i].posY == posY)
					{
						if(posX < coinList[i].posX) //jeśli zwierzak jest z lewej strony monetki
						{
							if(coinList[i].posX < posX + 5)
							{
								coinList.RemoveAt(i);
								++piniadze;
							}
						}
						else if(posX > coinList[i].posX) //jeśli zwierzak jest z prawej strony monetki
						{
							if(coinList[i].posX + 5 > posX)
							{
								coinList.RemoveAt(i);
								++piniadze;
							}
						}
						else //wpadł równo
						{
							coinList.RemoveAt(i);
							++piniadze;
						}
					}

				}
				//-----------------------koniec kolizji-----------------------
			}
			else
			{
				if(best > oldBest) oldBest = best;

				mrygaTimer -= Time.unscaledDeltaTime;
				if(mrygaTimer <= 0)
				{
					if(mryga)
					{
						mryga = false;
						mrygaTimer = 0.3f;
					}
					else
					{
						mryga = true;
						mrygaTimer = 0.6f;
					}
				}

			}
		}
		else //--------------------------uaktualnianie tekstów-----------------
		{
			bestText		= "PERSONAL TOP: " + best;
			moneyText		= "YOUR MONEY: " + piniadze + "$";

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
			//----------------Rysowanie tytułu---------------------------------------------
			for(int j = 0; j < 5; ++j)
			{
				for(int i = 0; i < titleText[j].Length; ++i)
				{
					SHGUI.current.SetPixelFront(titleText[j][i], 31 - (titleText[j].Length / 2) + i, j + 3, 'w');
				}
			}
			//------------------hint czym się włącza grę lub kupuje zwierzaka---------------
			for(int i = 0; i < pressText.Length; ++i) 
			{
				SHGUI.current.SetPixelFront(pressText[i], 31 - (pressText.Length / 2) + i, 10, 'w');
			}
			//---------------------------rysowanie postaci---------------------------------
			for(int j = 0; j < 3; ++j) 
			{
				for(int i = 0; i < 7; ++i)
				{
					if(animal[j + (yourLook * 3)][i] != '?')
					{
						SHGUI.current.SetPixelFront(animal[j + (yourLook * 3)][i], 31 - (animal[j + (yourLook * 3)].Length / 2) + i, 15 + j, 'w');
					}
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
							if((i%2 == 0 && j%2 == 0) || (i%2 == 1 && j%2 == 1))
							{
								if(backAnimal)
								{
									if(20 - (posY + 3 - cameraHeight) + j - (l * 3) + 9 < 23) //jeśli nie wychodzi za dolną krawędz
									{
										if(20 - (posY + 3 - cameraHeight) + j - (l * 3) + 9 > 0) //jesli nie wychodzi za górną krawędz
										{
											SHGUI.current.SetPixelFront(',', i + 1, 20 - (posY + 3 - cameraHeight) + j - (l * 3) + 9, 'z'); //rysowanie trawy
										}
									}
								}
								else
								{
									if(20 - (posY - cameraHeight) + j - (l * 3) + 9 < 23) //jeśli nie wychodzi za dolną krawędz
									{
										if(20 - (posY - cameraHeight) + j - (l * 3) + 9 > 0) //jesli nie wychodzi za górną krawędz
										{
											SHGUI.current.SetPixelFront(',', i + 1, 20 - (posY - cameraHeight) + j - (l * 3) + 9, 'z'); //rysowanie trawy
										}
									}
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
								if(backAnimal)
								{
									if(20 - (posY + 3 - cameraHeight) + j - (l * 3) + 9 < 23) //jeśli nie wychodzi za dolną krawędz
									{
										if(20 - (posY + 3 - cameraHeight) + j - (l * 3) + 9 > 0) //jesli nie wychodzi za górną krawędz
										{
											SHGUI.current.SetPixelFront('_', i + 1, 20 - (posY + 3 - cameraHeight) + j - (l * 3) + 9, 'z'); //rysowanie trawy
										}
									}
								}
								else
								{
									if(20 - (posY - cameraHeight) + j - (l * 3) + 9 < 23) //jeśli nie wychodzi za dolną krawędz
									{
										if(20 - (posY - cameraHeight) + j - (l * 3) + 9 > 0) //jesli nie wychodzi za górną krawędz
										{
											SHGUI.current.SetPixelFront('_', i + 1, 20 - (posY - cameraHeight) + j - (l * 3) + 9, 'z'); //rysowanie trawy
										}
									}
								}
							}
							else if(j == 2 && i%2 == 0)
							{
								if(backAnimal)
								{
									if(20 - (posY + 3 - cameraHeight) + j - (l * 3) + 9 < 23) //jeśli nie wychodzi za dolną krawędz
									{
										if(20 - (posY + 3 - cameraHeight) + j - (l * 3) + 9 > 0) //jesli nie wychodzi za górną krawędz
										{
											SHGUI.current.SetPixelFront('/', i + 1, 20 - (posY + 3 - cameraHeight) + j - (l * 3) + 9, 'z'); //rysowanie trawy
										}
									}
								}
								else
								{
									if(20 - (posY - cameraHeight) + j - (l * 3) + 9 < 23) //jeśli nie wychodzi za dolną krawędz
									{
										if(20 - (posY - cameraHeight) + j - (l * 3) + 9 > 0) //jesli nie wychodzi za górną krawędz
										{
											SHGUI.current.SetPixelFront('/', i + 1, 20 - (posY - cameraHeight) + j - (l * 3) + 9, 'z'); //rysowanie trawy
										}
									}
								}
							}
							else if(j == 2 && i%2 == 1)
							{
								if(backAnimal)
								{
									if(20 - (posY + 3 - cameraHeight) + j - (l * 3) + 9 < 23) //jeśli nie wychodzi za dolną krawędz
									{
										if(20 - (posY + 3 - cameraHeight) + j - (l * 3) + 9 > 0) //jesli nie wychodzi za górną krawędz
										{
											SHGUI.current.SetPixelFront('_', i + 1, 20 - (posY + 3 - cameraHeight) + j - (l * 3) + 9, 'z'); //rysowanie trawy
										}
									}
								}
								else
								{
									if(20 - (posY - cameraHeight) + j - (l * 3) + 9 < 23) //jeśli nie wychodzi za dolną krawędz
									{
										if(20 - (posY - cameraHeight) + j - (l * 3) + 9 > 0) //jesli nie wychodzi za górną krawędz
										{
											SHGUI.current.SetPixelFront('_', i + 1, 20 - (posY - cameraHeight) + j - (l * 3) + 9, 'z'); //rysowanie trawy
										}
									}
								}
							}
						}
					}
				}
			}

			//---------------------------koniec rysowania mapy----------------------------------

			//-------------------------------rysowanie monetek-----------------------------------

			if(onMoney)
			{
				for(int i = 0; i < coinList.Count; ++i)
				{
					for(int y = 0; y < 3; ++y)
					{
						if(22 - (coinList[i].posY - cameraHeight) + y > 0 && 20 - (coinList[i].posY - cameraHeight) + y < 23) //żeby nie właziły na krawędź górną
						{
							for(int x = 0; x < coinLook[y].Length; ++x)
							{
								if((20 - (coinList[i].posY - cameraHeight) + y) > 0 && (20 - (coinList[i].posY - cameraHeight) + y) < 23)
								{
									SHGUI.current.SetPixelFront(coinLook[y][x], coinList[i].posX + x, 20 - (coinList[i].posY - cameraHeight) + y, 'z');
								}
							}
						}
					}
				}
			}

			//---------------------------koniec rysowania monetek--------------------------------

			//------------------------------rysowanie postaci---------------------------------
			for(int j = 0; j < 3; ++j)
			{
				for(int i = 0; i < 7; ++i)
				{
					if(yourDirection == 0)
					{
						if(20 - (posY - cameraHeight) + j < 23)
						{
							if(animal[j + (yourLook * 3)][i] != '?')
							{
								if(lose)
								SHGUI.current.SetPixelFront(animal[j + (yourLook * 3)][i], posX + i, 20 - (posY - cameraHeight) + j, 'r');
								else
								SHGUI.current.SetPixelFront(animal[j + (yourLook * 3)][i], posX + i, 20 - (posY - cameraHeight) + j, 'w');
							}
						}
					}
					else
					{
						if(20 - (posY - cameraHeight) + j < 23)
						{
							if(animalBack[j + (yourLook * 3)][i] != '?')
							{
								if(lose)
								SHGUI.current.SetPixelFront(animalBack[j + (yourLook * 3)][i], posX + i, 20 - (posY - cameraHeight) + j, 'r');
								else
								SHGUI.current.SetPixelFront(animalBack[j + (yourLook * 3)][i], posX + i, 20 - (posY - cameraHeight) + j, 'w');
							}
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
									if((20 - (vehicle[i].posY - cameraHeight) + y) > 0 && (20 - (vehicle[i].posY - cameraHeight) + y) < 23)
									{
										if(cars[vehicle[i].graphic * 3 + y][x] != '?') //sprawdzanie maski przeźroczystości
										{
											SHGUI.current.SetPixelFront(cars[vehicle[i].graphic * 3 + y][x], vehicle[i].posX + x, 20 - (vehicle[i].posY - cameraHeight) + y, 'z');
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
										if(carsRev[vehicle[i].graphic * 3 + y][x] != '?') //sprawdzanie maski przeźroczystości
										{
											SHGUI.current.SetPixelFront(carsRev[vehicle[i].graphic * 3 + y][x], vehicle[i].posX + x, 20 - (vehicle[i].posY - cameraHeight) + y, 'z');
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
						for(int x = 0; x < cars[train[i].graphic * 3 + y].Length; ++x)
						{
							if(train[i].left) //rysowanie jadących w lewo
							{
								if(train[i].posX + x > 0 && train[i].posX + x < 63)
								{
									if((20 - (train[i].posY - cameraHeight) + y) > 0 && (20 - (train[i].posY - cameraHeight) + y) < 23)
									{
										if(cars[train[i].graphic * 3 + y][x] != '?')
										{
											SHGUI.current.SetPixelFront(cars[train[i].graphic * 3 + y][x], train[i].posX + x, 20 - (train[i].posY - cameraHeight) + y, 'z');
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
										if(carsRev[train[i].graphic * 3 + y][x] != '?')
										{
											SHGUI.current.SetPixelFront(carsRev[train[i].graphic * 3 + y][x], train[i].posX + x, 20 - (train[i].posY - cameraHeight) + y, 'z');
										}
									}
								}
							}
						}
					}
				}
			}
			//--------------------------koniec rysowania pojazdów----------------------------------
		
			//----------------------------rysowanie napisów po przegranej-------------------------
			if(lose)
			{
				if(!lockTab[yourLook]) //zablokowany / odblokowany zwierzak
				{
					if(mryga) //klik to restart
					{
						for(int n = 0; n < restartText.Length; ++n)
						{
							SHGUI.current.SetPixelFront(restartText[n], (62 - restartText.Length) / 2 + n, 14, 'r');
						}
					}
				}
				else if(piniadze >= 100) //klik to buy
				{
					for(int i = 0; i < unlockText.Length; ++i)
					{
						SHGUI.current.SetPixelFront(unlockText[i], 31 - (unlockText.Length / 2) + i, 14, 'w');
					}
				}
				else //brak hajsów
				{
					for(int i = 0; i < unlockText.Length; ++i)
					{
						SHGUI.current.SetPixelFront(unlockText[i], 31 - (unlockText.Length / 2) + i, 14, 'w');
					}
				}

				for(int n = 0; n < endText.Length; ++n) //powrót do menu
				{
					SHGUI.current.SetPixelFront(endText[n], (62 - endText.Length) / 2 + n, 21, 'w');
				}

				//rysowanie wyniku
				gameScoreText = "" + score;
				int tempPaddingLeft = 31 - (gameScoreText.Length*3); //wyznaczanie środka
				
				for(int k = 0; k < gameScoreText.Length; ++k)
				{
					for(int i = 0; i < 5; ++i)
					{
						for(int j = 0; j < 5; ++j)
						{
							SHGUI.current.SetPixelFront(Number[i+(int.Parse(gameScoreText[k].ToString()) * 5)][j], j + k * 6 + tempPaddingLeft, 8 + i, 'w');
						}
					}
				}

				if(score == best) //nowy najlepszy wynik
				{
					for(int n = 0; n < endBestText.Length; ++n)
					{
						SHGUI.current.SetPixelFront(endBestText[n], (62 - endBestText.Length) / 2 + n, 6, 'w');
					}
				}
				else //obecny najlepszy
				{
					bestText = "PERSONAL TOP: " + best;

					for(int n = 0; n < bestText.Length; ++n)
					{
						SHGUI.current.SetPixelFront(bestText[n], (62 - bestText.Length) / 2 + n, 6, 'w');
					}
				}
			
				for(int i = 1; i < 62; ++i) 
				{
					for(int j = 15; j < 20; ++j)
					{
						SHGUI.current.SetPixelFront(' ', i, j, 'w'); //zaciemnienie za postacią
					}
				}

				for(int j = 0; j < 3; ++j) //postać
				{
					for(int i = 0; i < 7; ++i)
					{
						if(animal[j + (yourLook * 3)][i] != '?')
						{
							SHGUI.current.SetPixelFront(animal[j + (yourLook * 3)][i], 31 - (animal[j + (yourLook * 3)].Length / 2) + i, 17 + j, 'w');
						}
					}
				}
				
				SHGUI.current.SetPixelFront('>', 37, 18, 'w'); //szczałki
				SHGUI.current.SetPixelFront('<', 25, 18, 'w');
			
				for(int n = 0; n < gameMoneyText.Length; ++n)
				{
					SHGUI.current.SetPixelFront(gameMoneyText[n], 62 - gameMoneyText.Length + 1 + n, 1, 'r');
				}

			}
			//-----------------------koniec rysowania napisów po przegranej---------------------

			//----------------------------rysowanie pieniędzy i punktów-------------------------
			if(!lose)
			{
				gameMoneyText = "" + piniadze + "$";
				for(int n = 0; n < gameMoneyText.Length; ++n)
				{
					SHGUI.current.SetPixelFront(gameMoneyText[n], 62 - gameMoneyText.Length + 1 + n, 1, 'w');
				}

				gameBestText = "Top " + best;
				for(int n = 0; n < gameBestText.Length; ++n)
				{
					SHGUI.current.SetPixelFront(gameBestText[n], n + 1, 7, 'w');
				}

				gameScoreText = "" + score;
				
				for(int k = 0; k < gameScoreText.Length; ++k)
				{
					for(int i = 0; i < 5; ++i)
					{
						for(int j = 0; j < 5; ++j) //rysowanie wyniku
						{
							SHGUI.current.SetPixelFront(Number[i+(int.Parse(gameScoreText[k].ToString()) * 5)][j], j + k * 6 + 1, 1 + i, 'w');
						}
					}
				}
			}
			//----------------------koniec rysowania pieniędzy i punktów-----------------------

			/*for (int i = 1; i < 63; ++i) //pożeranie mapy :v
			{
				for (int j = 0; j < 1; ++j)
				{
					SHGUI.current.SetPixelFront(StringScrambler.GetGlitchChar(), i, 22-j, 'z');
				}
			}*/
		}
	}
	
	//=======================================sterowanie======================================
	public override void ReactToInputKeyboard(SHGUIinput key)
	{
		if(key == SHGUIinput.enter)
		{
			if(menu || lose)
			{
				if(!lockTab[yourLook]) //startowanie gry
				{
					resetGame();

				}
				else if(piniadze >= 100)
				{
					piniadze -= 100;
					lockTab[yourLook] = false;
				}
			}
		}
		if (key == SHGUIinput.up)
		{
			if(!menu && !lose && jumpTimer <= 0) //poruszanie w górę
			{
				jumpTimer = 0.2f;

				posY += 3; //przesuwanie do góry

				if(!backAnimal) moveMap(); //losowanie nowego pola
				cameraJump = 1f; //ustawianie czasu do następnego ruchu kamerą
				yourDirection = 1; //obrócenie plecami
				cameraHeight += 2;

				backAnimal = false;

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
					jumpTimer = 0.2f;
					backAnimal = true;
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
					jumpTimer2 = 0.1f;
				}
			}
			else if(menu || lose)
			{
				++yourLook;
				if(yourLook > 4) yourLook = 0;
			}
		}
		if (key == SHGUIinput.left)
		{
			if(!menu && !lose && jumpTimer2 <= 0) //poruszanie w lewo
			{
				if(posX > 3)
				{
					posX-=3;
					jumpTimer2 = 0.1f;
				}
			}
			else if(menu || lose)
			{
				--yourLook;
				if(yourLook < 0) yourLook = 4;
			}
		}
		
		if (key == SHGUIinput.esc)
		{
			if(menu) SHGUI.current.PopView();
			else if(!menu)
			{
				menu = true;
				lose = false;
			}

		}
	}
	//==============================koniec sterowania=================================
	
	int randVehicles(int road, int type)
	{
		int tempPosY = (road + actualStep - 6) * 3;
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
				tempPosX += 15;
				newTrain = new crash(6, roadsDirection[road], tempPosX, tempPosY);
				train.Add(newTrain);
				tempPosX += 15;
				newTrain = new crash(6, roadsDirection[road], tempPosX, tempPosY);
				train.Add(newTrain);
				tempPosX += 15;
				newTrain = new crash(6, roadsDirection[road], tempPosX, tempPosY);
				train.Add(newTrain);
				tempPosX += 15;
				newTrain = new crash(6, roadsDirection[road], tempPosX, tempPosY);
				train.Add(newTrain);
				tempPosX += 15;
				newTrain = new crash(6, roadsDirection[road], tempPosX, tempPosY);
				train.Add(newTrain);
			}
			else
			{
				tempPosX = -5;
				
				crash newTrain = new crash(5, roadsDirection[road], tempPosX, tempPosY);
				train.Add(newTrain);
				tempPosX -= 11;
				newTrain = new crash(6, roadsDirection[road], tempPosX, tempPosY);
				train.Add(newTrain);
				tempPosX -= 15;
				newTrain = new crash(6, roadsDirection[road], tempPosX, tempPosY);
				train.Add(newTrain);
				tempPosX -= 15;
				newTrain = new crash(6, roadsDirection[road], tempPosX, tempPosY);
				train.Add(newTrain);
				tempPosX -= 15;
				newTrain = new crash(6, roadsDirection[road], tempPosX, tempPosY);
				train.Add(newTrain);
				tempPosX -= 15;
				newTrain = new crash(6, roadsDirection[road], tempPosX, tempPosY);
				train.Add(newTrain);
				tempPosX -= 15;
				newTrain = new crash(6, roadsDirection[road], tempPosX, tempPosY);
				train.Add(newTrain);
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
		
		stepTime		= 0.07f; //ruchu

		actualStep		= 3;

		backAnimal = false;

		//kamera
		cameraHeight	= 0;
		cameraJump		= 1f; //czas pomiędzy kolejnym przejściem kamery
		
		//mapa
		//ustawianie ulic i trawy
		for(int i = 0; i < roadsMap.Length; ++i)
		{
			if(i == 0 || i == 1 || i == 2 || i == 3 || i == 6 || i == 7 || i == 10 || i == 11) roadsMap[i] = 0;
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
		score = 0;
		
		//wstepne autka
		for(int b = 3; b < roadsMap.Length; ++b)
		{
			if(roadsMap[b] == 1)
			{
				crash newCar = new crash(Random.Range(0, 5), roadsDirection[b], Random.Range(20, 40), (actualStep + b - 6) * 3);
				vehicle.Add(newCar);
			}
		}
		//hue
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
		if(roadsMap[12] == 1 && roadsMap[11] == 1 && roadsMap[10] == 1 && roadsMap[9] == 1) roadsMap[12] = 0;
		else if(roadsMap[10] == 0 && roadsMap[11] == 0) roadsMap[12] = 1;
		else if(roadsMap[10] == 2 && roadsMap[11] == 2) roadsMap[12] = 0;
		else
		{
			roadsMap[12] = Random.Range(0, 2);
			if(Random.value > 0.90)
			{
				if(roadsMap[10] != 2 || roadsMap[11] != 2)
				{
					roadsMap[12] = 2;
				}
			}
		}

		//losowanie kierunków w których się będą poruszać pojazdy
		if(roadsDirection[11] == true) roadsDirection[12] = false;
		else roadsDirection[12] = true;

		//za ile ma się zrespić kolejne autko
		stepCounter[12] = Random.Range(4, 10);

		if(roadsMap[12] == 1)
		{
			crash newCar = new crash(Random.Range(0, 5), roadsDirection[12], Random.Range(15, 40), (actualStep + 7) * 3);
			vehicle.Add(newCar);
		}

		if(Random.value > 0.95)
		{
			coin newCoin = new coin(Random.Range(15, 40), (actualStep + 7) * 3);
			coinList.Add(newCoin);
		}
	}
}