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