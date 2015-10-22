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

	public crash(int type, bool dir, int posy, int posx)
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
		else if(type == 4) //łudka
		{
			width = 6;
		}
		else if(type == 5) //ciężarówka
		{
			width = 13;
		}
		else if(type == 6) //pociąg
		{
			width = 10;
		}
		else if(type == 7) //wagon
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
		"/0---0--",
		" ____   ",
		"|+ [][\\ ",
		"'o---o-'",
		" ____ ",
		"/    |",
		"\\____|",
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

	//----------------------------------------------------------

	int			posX = 30; //pozycja żabki
	int			posY = 0;
	int			offSetY = 0;

	char		frog = '⌂'; //żaba
	float		stepTime = 0.07f; //ruchu

	public		List<crash>		vehicle = new List<crash>();

	public		bool			lose = false;

	//---------------------------------------------------------

	public APPFroger() : base("hot_train-v5.5.5-by-onionmilk")
	{
		crash newCar = new crash(1, true, 6, 5);
		vehicle.Add(newCar);
		newCar = new crash(2, true, 10, 6);
		vehicle.Add(newCar);
		newCar = new crash(3, true, 16, 20);
		vehicle.Add(newCar);
		newCar = new crash(4, true, 16, 30);
		vehicle.Add(newCar);
	}
	
	public override void Update() 
	{
		base.Update();

		stepTime -= Time.unscaledDeltaTime;

		if(!lose)
		{
			if(stepTime <= 0f)
			{
				stepTime = 0.07f;

				for(int i = 0; i < vehicle.Count;) //jeżeli minąłeś już jakiś auto i wyleciało poza obszar renderu to je wywalamy
				{
					if(vehicle[i].posY < (posY - 4))
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
			/*
			for(int i = 0; i < vehicle.Count; ++i) //kolizja
			{
				if(vehicle[i].posY <= posY && vehicle[i].posY > posY - 3) //jeśli jest na wysokości pojazdu
				{
					if(vehicle[i].left) //dla jadącego w lewo
					{
						if(vehicle[i].posX <= posX && vehicle[i].posX > posX - vehicle[i].width)
						{
							lose = true;
						}
					}
					else
					{
						if(vehicle[i].posX >= posX && vehicle[i].posX < posX - vehicle[i].width)
						{
							lose = true;
						}
					}
				}
			}*/
		}
	}
	
	public override void Redraw(int offx, int offy)
	{
		base.Redraw(offx, offy);

		SHGUI.current.SetPixelFront(frog, posX, 22 - offSetY, 'w'); //rysowanie żaby
		
		//rysowanie pojazdów
		for(int i = 0; i < vehicle.Count; ++i)
		{
			for(int y = 0; y < 3; ++y)
			{
				if(22 - (vehicle[i].posY - posY) + y > 0 && 22 - (vehicle[i].posY - posY) + y < 23) //żeby nie właziły na krawędź górną
				{
					for(int x = 0; x < cars[vehicle[i].graphic * 3 + y].Length; ++x)
					{
						if(vehicle[i].left)
						{
							if(vehicle[i].posX + x > 0 && vehicle[i].posX + x < 63)
							{
								SHGUI.current.SetPixelFront(cars[vehicle[i].graphic * 3 + y][x], vehicle[i].posX + x, 22 - (vehicle[i].posY - posY) + y, 'z');
							}
						}
						else
						{
							if(vehicle[i].posX - x > 0 && vehicle[i].posX - x < 63)
							{
								SHGUI.current.SetPixelFront(cars[vehicle[i].graphic * 3 + y][x], vehicle[i].posX - x, 22 - (vehicle[i].posY - posY) + y, 'z');
							}
						}
					}
				}
			}
		}
	}
	
	public override void ReactToInputKeyboard(SHGUIinput key)
	{
		//sterowanie
		if (key == SHGUIinput.up)
		{
			++posY;
			if(offSetY < 4) ++offSetY;
			else //tworzenie nowych autek
			{
				if(posY % 3 == 0) randVehicles();
			}
		}
		if (key == SHGUIinput.down)
		{
			if(offSetY > 0)
			{
				--posY;
				--offSetY;
			}
		}
		if (key == SHGUIinput.right)
		{
			if(posX < 62) ++posX;
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