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
	
	private string[]			cars	= new string[]
	{
		"  o ",
		" /# ",
		"o--o",
		" ",
		" _,--._ ",
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
	};
	//-------Kolizje i rysowanie podlega zasadzie lewogo górnego rogu-------

	//postać
	int			posX			= 30; //pozycja żabki
	int			posY			= 0;

	int			yourDirection	= 1; //0 - przód | 1 - tył
	int			yourLook		= 3; //0 - królik | 1 - kot | 2 - ptak | 3 - świnka

	float		stepTime		= 0.07f; //ruchu

	//kamera
	int			cameraHeight	= 0;
	float		cameraJump		= 1f; //czas pomiędzy kolejnym przejściem kamery
	
	//pojazdy
	public		List<crash>		vehicle = new List<crash>();

	//mechanika
	public		bool			lose = false;

	//---------------------------------------------------------------------

	public APPFroger() : base("hot_train-v5.5.5-by-onionmilk")
	{
		crash newCar = new crash(1, true, 5, 6);
		vehicle.Add(newCar);
		newCar = new crash(2, true, 6, 12);
		vehicle.Add(newCar);
		newCar = new crash(3, true, 20, 15);
		vehicle.Add(newCar);
		newCar = new crash(4, true, 30, 18);
		vehicle.Add(newCar);
	}
	
	public override void Update() 
	{
		base.Update();

		stepTime -= Time.unscaledDeltaTime;

		if(!lose)
		{
			cameraJump -= Time.unscaledDeltaTime;
			if(cameraJump <= 0f)
			{
				++cameraHeight;
				cameraJump = 1f;
			}

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
						if(vehicle[i].posX < (0 - vehicle[i].width)) vehicle[i].posX = 64;
					}
					else
					{
						++vehicle[i].posX;
						if(vehicle[i].posX > (64 + vehicle[i].width)) vehicle[i].posX = 0;
					}
				}
			}

		}
	}
	
	public override void Redraw(int offx, int offy)
	{
		base.Redraw(offx, offy);

		//rysowanie postaci
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
		//koniec rysowania postaci

		//rysowanie pojazdów
		for(int i = 0; i < vehicle.Count; ++i)
		{
			for(int y = 0; y < 3; ++y)
			{
				if(22 - (vehicle[i].posY - cameraHeight) + y > 0 && 20 - (vehicle[i].posY - cameraHeight) + y < 23) //żeby nie właziły na krawędź górną
				{
					for(int x = 0; x < cars[vehicle[i].graphic * 3 + y].Length; ++x)
					{
						if(vehicle[i].left)
						{
							if(vehicle[i].posX + x > 0 && vehicle[i].posX + x < 63)
							{
								SHGUI.current.SetPixelFront(cars[vehicle[i].graphic * 3 + y][x], vehicle[i].posX + x, 20 - (vehicle[i].posY - cameraHeight) + y, 'z');
							}
						}
						else
						{
							if(vehicle[i].posX - x > 0 && vehicle[i].posX - x < 63)
							{
								SHGUI.current.SetPixelFront(cars[vehicle[i].graphic * 3 + y][x], vehicle[i].posX - x, 20 - (vehicle[i].posY - cameraHeight) + y, 'z');
							}
						}
					}
				}
			}
		}
		//koniec rysowania pojazdów
	}
	
	public override void ReactToInputKeyboard(SHGUIinput key)
	{
		//sterowanie
		if (key == SHGUIinput.up)
		{
			cameraJump = 1f;
			posY += 3;
			yourDirection = 1;
			cameraHeight += 2;
			if(posY - 4 > cameraHeight) cameraHeight += 1;

		}
		if (key == SHGUIinput.down)
		{
			if(!(posY - 3 < cameraHeight)) //sprawdzanie, żeby postać nie wyszła poza obszar kamery
			{
				posY -= 3;
				yourDirection = 0;
			}
		}
		if (key == SHGUIinput.right)
		{
			if(posX < 56) ++posX;
		}
		if (key == SHGUIinput.left)
		{
			if(posX > 1) --posX;
		}
		
		if (key == SHGUIinput.esc)
		{
			SHGUI.current.PopView();
		}

	}
	
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
}