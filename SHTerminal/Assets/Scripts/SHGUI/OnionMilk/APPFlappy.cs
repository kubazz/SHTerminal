using UnityEngine;
using System.Collections;

public class pipe
{
	public int posX;
	public int border;

	public pipe(int pX, int bor)
	{
		posX = pX;
		border = bor;
	}
}

public class APPFlappy : SHGUIappbase {
	
	//62x22
	//wygląd
	char	ground = '░';
	char	wallChar = '█';
	char	bird = '»';

	//mechanika
	int		posY = 10;

	float	collapseTimer = 0.1f;
	float	wallTimer = 0.1f;

	bool	jump = false;
	int		jumpLvl = 4;

	bool	lose = true;
	bool	begin = true;

	//przeszkody
	pipe[]	wall = new pipe[5];

	//informacje
	int		score = 0;
	string	scoreString = "0";
	string	loseString = "You Lose";
	

	public APPFlappy()
	: base("super_flappy-v1.2.3-by-onionmilk") {
		for(int i = 0; i < 5; ++i)
		{
			wall[i] = new pipe(63 + (17 * i), Random.Range(5, 13));
		}
	}
	
	public override void Update() {
		base.Update();

		if(!lose)
		{
			//postać
			collapseTimer -= Time.unscaledDeltaTime;
			if(!jump) //upadanie
			{
				if(collapseTimer <= 0f)
				{
					collapseTimer = 0.1f;
					++posY;
				}
				if(posY == 22)
				{
					lose = true;
					collapseTimer = 1f;
				}
			}
			else //wznoszenie
			{
				if(collapseTimer <= 0f)
				{
					collapseTimer = 0.07f;
					--posY;
					--jumpLvl;
					if(jumpLvl <= 0)
					{
						jumpLvl = 4;
						jump = false;
					}
				}
				if(posY == 2)
				{
					lose = true;
					collapseTimer = 1f;
				}
			}

			//przeszkody
			wallTimer -= Time.unscaledDeltaTime;
			if(wallTimer <= 0f)
			{
				wallTimer = 0.1f;
				for(int i = 0; i < 5; ++i)
				{
					--wall[i].posX;
					if(wall[i].posX == 0)
					{
						wall[i].posX = 85;
						wall[i].border = Random.Range(5, 13);
					}

					if(wall[i].posX == 5) //sprawdzenie czy trafił w ptaka :)
					{
						if(posY > wall[i].border || posY < (wall[i].border + 5))
						{
							++score;
							//SHGUI.current.PlaySound(SHGUIsound.ping);
						}
					}
				}
			}

			for(int i = 0; i < 5; ++i)
			{
				if(wall[i].posX == 5) //sprawdzenie czy trafił w ptaka :)
				{
					if(posY <= wall[i].border || posY >= (wall[i].border + 5))
					{
						lose = true;
						collapseTimer = 1f;
					}
				}
			}
		}
		else
		{
			collapseTimer -= Time.unscaledDeltaTime;
		}

	}

	public override void Redraw(int offx, int offy) {
		base.Redraw(offx, offy);


		for(int i = 1; i <= 62; ++i) //rysowanie krawędzi
		{
			SHGUI.current.SetPixelFront(ground, i, 22, 'w');
			SHGUI.current.SetPixelFront(ground, i, 2, 'w');
		}

		for(int i = 0; i < 5; ++i) //rysowanie scian
		{
			for(int j = 3; j <= wall[i].border; ++j)
			{
				if(wall[i].posX < 63) SHGUI.current.SetPixelFront(wallChar, wall[i].posX, j, 'w');
			}
			for(int j = 21; j >= wall[i].border + 5; --j)
			{
				if(wall[i].posX < 63) SHGUI.current.SetPixelFront(wallChar, wall[i].posX, j, 'w');
			}
		}

		SHGUI.current.SetPixelFront(bird, 5, posY, 'w'); //rysowanie ptaka
		if(!begin && lose) SHGUI.current.SetPixelBack(wallChar, 5, posY, 'r');

		//rysowanie napisu
		scoreString = score.ToString();
		for(int i = 0; i < scoreString.Length; ++i)
		{
			SHGUI.current.SetPixelFront(scoreString[i], 31 - scoreString.Length + i, 1, 'w');
		}

		if(lose)
		{
			if(begin) loseString = "Press To Start";
			else loseString = "You Lose";

			for(int i = 0; i < loseString.Length; ++i)
			{
				SHGUI.current.SetPixelFront(loseString[i], 26 + i, 10, 'r');
			}
		}
	}

	public override void ReactToInputKeyboard(SHGUIinput key) {
		//sterowanie
		if (key == SHGUIinput.up || key == SHGUIinput.enter) {

			if(lose)
			{
				begin = false;
				if(collapseTimer <= 0f)
				{
					lose = false;
					posY = 10;
					score = 0;
					for(int i = 0; i < 5; ++ i) wall[i] = new pipe(63 + (17 * i), Random.Range(5, 13));
				}
			}
			else
			{
				jump = true;
			}
		}
		
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView();
		//--
	}
}
