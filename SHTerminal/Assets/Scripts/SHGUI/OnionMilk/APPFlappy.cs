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

public class decoration
{
	public int posX;
	public int posY;

	public decoration(int pX, int pY)
	{
		posX = pX;
		posY = pY;
	}
}

public class APPFlappy : SHGUIappbase {
	
	//62x22
	//wygląd
	char	ground = '░';
	char	wallChar = '█';
	char	bird = '»';
	char	cloudChar = '░';

	//mechanika
	int		posY = 10;

	float	collapseTimer = 0.1f;
	float	wallTimer = 0.1f;
	float	decorationTimer = 0.05f;

	bool	jump = false;
	int		jumpLvl = 5;

	bool	lose = true;
	bool	begin = true;
	bool	air = false;

	//przeszkody
	pipe[]	wall = new pipe[5];
	decoration[] cloud = null;

	//informacje
	int		score = 0;
	string	scoreString = "0";
	string	loseString = "You Lose";

	int		bestScore = 0;
	string	bestScoreString = "0";
	string	bestString = "Best Score";
	

	public APPFlappy()
	: base("super_flappy-v1.2.3-by-onionmilk") {
		for(int i = 0; i < 5; ++i)
		{
			wall[i] = new pipe(63 + (17 * i), Random.Range(5, 13));
		}
		cloud = new decoration[Random.Range(8, 15)];
		for(int i = 0; i < cloud.Length; ++i)
		{
			cloud[i] = new decoration(Random.Range(64, 125), Random.Range(4, 18));
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
					if(!air)
					{
						collapseTimer = 0.1f;
						++posY;
					}
					else
					{
						collapseTimer = 0.1f;
						air = false;
					}
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
						jumpLvl = 5;
						jump = false;
						air = true;
					}
				}
				if(posY == 1)
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
				for(int i = 0; i < 5; ++i) //przesuwanie rur
				{
					--wall[i].posX;
					if(wall[i].posX == -1)
					{
						wall[i].posX = 84;
						wall[i].border = Random.Range(5, 13);
					}

					if(wall[i].posX == 4) //sprawdzenie czy ptak się zmieścił
					{
						if(posY > wall[i].border || posY < (wall[i].border + 6))
						{
							++score;
							//SHGUI.current.PlaySound(SHGUIsound.ping);
						}
					}
				}
			}

			for(int i = 0; i < 5; ++i)
			{
				if(wall[i].posX == 5 || wall[i].posX == 4) //sprawdzenie czy trafił w ptaka :)
				{
					if(posY <= wall[i].border || posY >= (wall[i].border + 6))
					{
						lose = true;
						collapseTimer = 1f;
					}
				}
			}
		}
		else //czas między przegraną a możliwością resetu (zabezpieczenie przed przypadkowym restartem)
		{
			collapseTimer -= Time.unscaledDeltaTime;
		}

		//poruszanie się chmurek
		decorationTimer -= Time.unscaledDeltaTime;
		if(decorationTimer <= 0f)
		{
			decorationTimer = 0.07f;
			for(int i = 0; i < cloud.Length; ++i)
			{
				--cloud[i].posX;
				if(cloud[i].posX <= -7)
				{
					cloud[i].posX = Random.Range(65, 125);
					cloud[i].posY = Random.Range(4, 18);
				}
			}
		}

	}

	public override void Redraw(int offx, int offy) {
		base.Redraw(offx, offy);

		for(int i = 0; i < cloud.Length; ++i) //rysowanie chmurek
		{
			if(cloud[i].posX < 63 && cloud[i].posX > 0) SHGUI.current.SetPixelBack(cloudChar, cloud[i].posX, cloud[i].posY, 'z');
			if(cloud[i].posX < 63 + 1 && cloud[i].posX > - 1) SHGUI.current.SetPixelBack(cloudChar, cloud[i].posX + 1, cloud[i].posY, 'z');
			if(cloud[i].posX < 63 + 2 && cloud[i].posX > - 2) SHGUI.current.SetPixelBack(cloudChar, cloud[i].posX + 2, cloud[i].posY, 'z');
			if(cloud[i].posX < 63 + 3 && cloud[i].posX > - 3) SHGUI.current.SetPixelBack(cloudChar, cloud[i].posX + 3, cloud[i].posY, 'z');
			if(cloud[i].posX < 63 + 4 && cloud[i].posX > - 4) SHGUI.current.SetPixelBack(cloudChar, cloud[i].posX + 4, cloud[i].posY, 'z');
			if(cloud[i].posX < 63 + 5 && cloud[i].posX > - 5) SHGUI.current.SetPixelBack(cloudChar, cloud[i].posX + 5, cloud[i].posY, 'z');
			if(cloud[i].posX < 63 + 1 && cloud[i].posX > - 1) SHGUI.current.SetPixelBack(cloudChar, cloud[i].posX + 1, cloud[i].posY - 1, 'z');
			if(cloud[i].posX < 63 + 2 && cloud[i].posX > - 2) SHGUI.current.SetPixelBack(cloudChar, cloud[i].posX + 2, cloud[i].posY - 1, 'z');
			if(cloud[i].posX < 63 + 3 && cloud[i].posX > - 3) SHGUI.current.SetPixelBack(cloudChar, cloud[i].posX + 3, cloud[i].posY - 1, 'z');
		}

		for(int i = 1; i <= 62; ++i) //rysowanie krawędzi
		{
			SHGUI.current.SetPixelFront(ground, i, 22, 'w');
			SHGUI.current.SetPixelFront(ground, i, 1, 'w');
		}

		for(int i = 0; i < 5; ++i) //rysowanie scian
		{
			for(int j = 2; j <= wall[i].border; ++j)
			{
				if(wall[i].posX < 63 && wall[i].posX > 0) SHGUI.current.SetPixelBack(wallChar, wall[i].posX, j, 'w');
				if(wall[i].posX < 63 - 1  && wall[i].posX > -1) SHGUI.current.SetPixelBack(wallChar, wall[i].posX+1, j, 'w');
			}
			for(int j = 21; j >= wall[i].border + 6; --j)
			{
				if(wall[i].posX < 63  && wall[i].posX > 0) SHGUI.current.SetPixelBack(wallChar, wall[i].posX, j, 'w');
				if(wall[i].posX < 63 - 1  && wall[i].posX > -1) SHGUI.current.SetPixelBack(wallChar, wall[i].posX+1, j, 'w');
			}
		}

		SHGUI.current.SetPixelFront(bird, 5, posY, 'w'); //rysowanie ptaka
		if(!begin && lose) SHGUI.current.SetPixelBack(wallChar, 5, posY, 'r');


		//rysowanie wyniku
		if(!lose)
		{
			scoreString = score.ToString();
			for(int i = 0; i < scoreString.Length; ++i)
			{
				SHGUI.current.SetPixelFront(scoreString[i], 32 - scoreString.Length/2 + i, 5, 'r');
			}
		}

		//rysowanie napisów
		if(lose)
		{
			if(begin) loseString = "Press To Start";
			else
			{
				loseString = "Your Score";
				for(int i = 0; i < scoreString.Length; ++i) //wypisanie wyniku
				{
					SHGUI.current.SetPixelFront(scoreString[i], 32 + i - scoreString.Length/2, 11, 'r');
				}

				for(int i = 0; i < bestString.Length; ++i) //napis Best Score
				{
					SHGUI.current.SetPixelFront(bestString[i], 32 + i - bestString.Length/2, 13, 'r');
				}
				bestScoreString = bestScore.ToString();
				for(int i = 0; i < bestScoreString.Length; ++i) //wypisanie najlepszego wyniku
				{
					SHGUI.current.SetPixelFront(bestScoreString[i], 32 + i - bestScoreString.Length/2, 14, 'r');
				}
			}

			for(int i = 0; i < loseString.Length; ++i) //napis Press To Start lub Your Score
			{
				SHGUI.current.SetPixelFront(loseString[i], 32 + i - loseString.Length/2, 10, 'r');
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
