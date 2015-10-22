using UnityEngine;
using System.Collections;

public class pipe
{
	public	float	posX;
	public	float	border;
	public	bool	points;

	public pipe(float pX, float bor)
	{
		posX = pX;
		border = bor;
		points = false;
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

	//liczby
	private string[]	Number0			= new string[] {
		" ___ ",
		"|   |",
		"| | |",
		"|   |",
		" ¯¯¯ "
	};
	private string[]	Number1			= new string[] {
		"  _  ",
		" | | ",
		" | | ",
		" | | ",
		"  ¯  "
	};
	private string[]	Number2			= new string[] {
		" ___ ",
		"|__ |",
		"| __|",
		"|   |",
		" ¯¯¯ "
	};
	private string[]	Number3			= new string[] {
		" ___ ",
		"|__ |",
		"|__ |",
		"|   |",
		" ¯¯¯ "
	};
	private string[]	Number4			= new string[] {
		" _ _ ",
		"| | |",
		"|__ |",
		"  | |",
		"   ¯ "
	};
	private string[]	Number5			= new string[] {
		" ___ ",
		"| __|",
		"|__ |",
		"|   |",
		" ¯¯¯ "
	};
	private string[]	Number6			= new string[] {
		" ___ ",
		"| __|",
		"| . |",
		"|   |",
		" ¯¯¯ "
	};
	private string[]	Number7			= new string[] {
		" ___ ",
		"|__ |",
		"  | |",
		"  | |",
		"   ¯ "
	};
	private string[]	Number8			= new string[] {
		" ___ ",
		"| . |",
		"| , |",
		"|   |",
		" ¯¯¯ "
	};
	private string[]	Number9			= new string[] {
		" ___ ",
		"| . |",
		"|__ |",
		"|   |",
		" ¯¯¯ "
	};


	private string[]	Title			= new string[] {
	" ___ _ _ ___ ___ ___    ___ _ ___             ",
	"| __| | | . |  _| , |  |   | |_  |___________ ",
	"|__ | | | __|  _|   |  |  ¯| | . | . | . | | |",
	"|   |   | | |   | | |  | |¯| |   | __| __|__ |",
	" ¯¯¯ ¯¯¯ ¯   ¯¯¯ ¯ ¯    ¯   ¯ ¯¯¯| | | |  |  |",
	"                                  ¯   ¯    ¯¯ "
	};

	private string[]	Best			= new string[] {
		" ___ ___ ___ _   ",
		"| , |  _| __| |  ",
		"| , |  _|__ |  _|",
		"|   |   |   |   |",
		" ¯¯¯ ¯¯¯ ¯¯¯ ¯¯¯ "
	};

	//wygląd
	private string[]	LookBird			= new string[] {
		" ___",
		"/__o\\_",
		"\\___/-'",
		" ___",
		"/\\\\o\\_",
		"\\___/-'"
	};
	private string[]	DeadBird			= new string[] {
		" ___",
		"/__X\\_",
		"\\___/-'"
	};

	private string[]	LookPipe			= new string[] {
		" ______ ",
		"|      |",
		"|______|"
	};

	private string[]	LookPipeUp			= new string[] {
		"|¯¯¯¯¯¯|",
		"|      |",
		" ¯¯¯¯¯¯ "
	};

	private string[]	LookWall			= new string[] {
		" |    | "
	};

	private string[]	LookWallOnGround	= new string[] {
		" |____| "
	};

	private string[]	Ground				= new string[] {
		"_",
		"/",
		"="
	};

	//mechanika
	private	float		Height				= 10f; //wysokość na której jest ptak
	private float		LastHeight			= 10f; //wysokość jaką miał podczas wyskoku
	
	private	float		Fall				= 10f; //hymm w sumie obecnie nic
	private	float		FlyTime				= 0f; //czas wznoszenia się (jeśli jest na minusie leci do góry gdy jest >= 0 spada)
	
	private	float		HeightFactor		= 5f; //wartość wzlotu podczas skoku

	//napisy
	private	int			Score				= 0; //wynik w obecnej rozgrywce
	private	int			BestScore			= 0; //najlepszy wynik
	private	string		BestScoreString		= "";
	private	string		ScoreString			= "";
	
	private	string		menuHint			= "[=PRESS ENTER TO START=]";
	private	string		newBSString			= "NEW PERSONAL BEST";
	private	string		BSString			= "PERSONAL BEST";

	//przeszkody
	private pipe[]		Wall				= new pipe[4]; //przeszkody


	//rozgrywka
	private bool		PlayGame			= false; //czy gra jest odpalona
	private	bool		Lose				= false; //czy przegraliśmy


	//animacje
	private	float		birdAnimTime		= 0f;
	private	float		endFleshTimer		= 0.1f;
	private	float		endFall				= 0.1f;
	private	int			distanceGroud		= 0;
	private	int			moduloMem			= 0;


	public APPFlappy() : base("super_flappy-v1.2.3-by-onionmilk")
	{
		for(int i = 0; i < Wall.Length; ++i) Wall[i] = new pipe(70 + 30f * i, (float)Random.Range(11, 15)); //ustawianie rur
	}
	
	public override void Update()
	{
		base.Update();

		if(PlayGame)
		{
			if((int)Wall[0].posX % 2 != moduloMem)
			{
				--distanceGroud;
				if(distanceGroud < 0) distanceGroud = 2;
			}
			moduloMem = (int)Wall[0].posX % 2;

			for(int i = 0; i < Wall.Length; ++i)
			{
				Wall[i].posX -= 12 * Time.unscaledDeltaTime;

				if(Wall[i].posX < - 10)
				{
					Wall[i].posX += 120;
					Wall[i].border = Random.Range(11, 15);
					Wall[i].points = false;
				}
			}

			FlyTime += 2.5f * Time.unscaledDeltaTime;
			Height = LastHeight + CalcHeight(FlyTime);
		
			//kolizja
			for(int i = 0; i < Wall.Length; ++i)
			{
				if(Wall[i].posX > -2 && Wall[i].posX < 12) //jesli prak znajdzie się na szerokości platformy
				{
					if(Height + 1 > Wall[i].border) //kolizja z dolnymi rurami
					{
						if(BestScore < Score) BestScore = Score;
						PlayGame = false;
						Lose = true;
					}
					if(Height < Wall[i].border - 7) //kolizja z górnymi rurami
					{
						if(BestScore < Score) BestScore = Score;
						PlayGame = false;
						Lose = true;
					}
				}

				if(Wall[i].posX < 6 && !Wall[i].points) //dodawanie punktu
				{
					Wall[i].points = true;
					++Score;
				}

			}
			if(Height < 1)
			{
				if(BestScore < Score) BestScore = Score;
				PlayGame = false;
				Lose = true;
			}
			if(Height > 18)
			{
				if(BestScore < Score) BestScore = Score;
				PlayGame = false;
				Lose = true;
			}
		}
		if(Lose)
		{
			if(endFall > 0f && Height < 18)
			{
				endFall -= Time.deltaTime;
			}
			else if(Height < 18)
			{
				endFall = 0.1f;
				++Height;
			}
		}

	}

	public override void Redraw(int offx, int offy)
	{
		base.Redraw(offx, offy);

		if(Lose && endFleshTimer > 0f)
		{
			endFleshTimer -= Time.deltaTime;
			for(int i = 1; i < 64; ++i) //rysowanie ziemi
			{
				for(int j = 1; j < 24; ++j) //rysowanie ziemi
				{
					SHGUI.current.SetPixelBack('█', i, j, 'w');
				}
			}
		}
		else
		{
			for(int i = 0; i < 62; ++i) //rysowanie ziemi
			{
				SHGUI.current.SetPixelBack(Ground[0][0], i + 1, 20, 'z');
				if(i % 3 != 0 + distanceGroud) SHGUI.current.SetPixelBack(Ground[1][0], i + 1, 21, 'z');
				SHGUI.current.SetPixelBack(Ground[2][0], i + 1, 22, 'z');
			}

			for(int k = 0; k < Wall.Length; ++k) //rysowanie ścian
			{
				//dolna część
				for(int i = 0; i < 3; ++i) //góra
				{
					for(int j = 0; j < LookPipe[i].Length; ++j)
					{
						if((int)Wall[k].posX + j > 0 && (int)Wall[k].posX + j < 63)
						{
							SHGUI.current.SetPixelBack(LookPipe[i][j], (int)Wall[k].posX + j, (int)Wall[k].border + i, 'z');
						}
					}
				}
				for(int m = (int)Wall[k].border + 3; m < 20; ++m) //rura
				{
					for(int j = 0; j < LookWall[0].Length; ++j)
					{
						if((int)Wall[k].posX + j > 0 && (int)Wall[k].posX + j < 63)
						{
							SHGUI.current.SetPixelBack(LookWall[0][j], (int)Wall[k].posX + j, m, 'z');
						}
					}
				}
				for(int j = 0; j < LookWallOnGround[0].Length; ++j)//podłoże
				{
					if((int)Wall[k].posX + j > 0 && (int)Wall[k].posX + j < 63)
					{
						SHGUI.current.SetPixelBack(LookWallOnGround[0][j], (int)Wall[k].posX + j, 20, 'z');
					}
				}
				//górna część
				for(int i = 0; i < 3; ++i) //góra
				{
					for(int j = 0; j < LookPipeUp[i].Length; ++j)
					{
						if((int)Wall[k].posX + j > 0 && (int)Wall[k].posX + j < 63)
						{
							SHGUI.current.SetPixelBack(LookPipeUp[i][j], (int)Wall[k].posX + j, (int)Wall[k].border - 9 + i, 'z');
						}
					}
				}
				for(int m = (int)Wall[k].border - 10; m > 0; --m) //rura
				{
					for(int j = 0; j < LookWall[0].Length; ++j)
					{
						if((int)Wall[k].posX + j > 0 && (int)Wall[k].posX + j < 63)
						{
							SHGUI.current.SetPixelBack(LookWall[0][j], (int)Wall[k].posX + j, m, 'z');
						}
					}
				}
				//rysowanie ptaka :)
				if(birdAnimTime > 0f)
				{
					for(int i = 3; i < 6; ++i) 
					{
						for(int j = 0; j < LookBird[i].Length; ++j)
						{
							if((int)Height + i > 0)
							{
								if(Lose) SHGUI.current.SetPixelFront(DeadBird[i-3][j], 6 + j, (int)Height + i - 3, 'z');
								else SHGUI.current.SetPixelFront(LookBird[i][j], 6 + j, (int)Height + i - 3, 'z');
							}
						}
					}
					birdAnimTime -= Time.deltaTime;
				}
				else
				{
					for(int i = 0; i < 3; ++i) 
					{
						for(int j = 0; j < LookBird[i].Length; ++j)
						{
							if((int)Height + i > 0)
							{
								if(Lose) SHGUI.current.SetPixelFront(DeadBird[i][j], 6 + j, (int)Height + i, 'z');
								else SHGUI.current.SetPixelFront(LookBird[i][j], 6 + j, (int)Height + i, 'z');
								//--
							}
						}
					}
				}
			}


		}

		//napis w menu
		if(!PlayGame && !Lose)
		{
			for(int i = 0; i < 6; ++i) //rysowanie napisu w menu
			{
				for(int j = 0; j < Title[i].Length; ++j)
				{
					SHGUI.current.SetPixelFront(Title[i][j], 8 + j, 3 + i, 'w');
				}
			}
			for(int j = 0; j < menuHint.Length; ++j)
			{
				SHGUI.current.SetPixelFront(menuHint[j], 32 - menuHint.Length/2 + j, 11, 'w');
			}
			for(int j = 0; j < BSString.Length; ++j)
			{
				SHGUI.current.SetPixelFront(BSString[j], 32 - BSString.Length/2 + j, 15, 'w');
			}
			BestScoreString = "" + BestScore; 
			for(int j = 0; j < BestScoreString.Length; ++j)
			{
				SHGUI.current.SetPixelFront(BestScoreString[j], 32 - BestScoreString.Length/2 + j, 16, 'w');
			}

		}
		if(Lose && endFleshTimer <= 0f) //rysowanie napisów po przegranej
		{
			menuHint = "PRESS ENTER TO RESTART";

			for(int j = 0; j < menuHint.Length; ++j) //press to start
			{
				SHGUI.current.SetPixelFront(menuHint[j], 32 - menuHint.Length/2 + j, 11, 'r');
			}

			if(Score > BestScore)
			{
				BSString = "NEW PERSONAL BEST SCORE";
				BestScore = Score;
			}
			else BSString = "PERSONAL BEST SCORE";

			for(int j = 0; j < BSString.Length; ++j)
			{
				SHGUI.current.SetPixelFront(BSString[j], 32 - BSString.Length/2 + j, 15, 'r');
			}
			BestScoreString = "" + BestScore; 
			for(int j = 0; j < BestScoreString.Length; ++j)
			{
				SHGUI.current.SetPixelFront(BestScoreString[j], 32 - BestScoreString.Length/2 + j, 16, 'r');
			}
		}

		if(PlayGame || Lose) //rysowanie wyniku podczas rozgrywki
		{

			ScoreString = "" + Score;
			int tempSize = 6 * ScoreString.Length;
			int	leftPadding	= (62 - 6 * ScoreString.Length) / 2;
			
			for(int k = 0; k < ScoreString.Length; ++k) //zapisywanie najlepszego wyniku
			{
				for(int i = 0; i < 5; ++i)
				{
					for(int j = 0; j < 5; ++j) //rysowanie wyniku
					{
						if(Lose)
						{
							if(ScoreString[k] == '0') SHGUI.current.SetPixelFront(Number0[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'r');
							else if(ScoreString[k] == '1') SHGUI.current.SetPixelFront(Number1[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'r');
							else if(ScoreString[k] == '2') SHGUI.current.SetPixelFront(Number2[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'r');
							else if(ScoreString[k] == '3') SHGUI.current.SetPixelFront(Number3[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'r');
							else if(ScoreString[k] == '4') SHGUI.current.SetPixelFront(Number4[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'r');
							else if(ScoreString[k] == '5') SHGUI.current.SetPixelFront(Number5[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'r');
							else if(ScoreString[k] == '6') SHGUI.current.SetPixelFront(Number6[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'r');
							else if(ScoreString[k] == '7') SHGUI.current.SetPixelFront(Number7[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'r');
							else if(ScoreString[k] == '8') SHGUI.current.SetPixelFront(Number8[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'r');
							else if(ScoreString[k] == '9') SHGUI.current.SetPixelFront(Number9[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'r');
						}
						else
						{
							if(ScoreString[k] == '0') SHGUI.current.SetPixelFront(Number0[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'w');
							else if(ScoreString[k] == '1') SHGUI.current.SetPixelFront(Number1[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'w');
							else if(ScoreString[k] == '2') SHGUI.current.SetPixelFront(Number2[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'w');
							else if(ScoreString[k] == '3') SHGUI.current.SetPixelFront(Number3[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'w');
							else if(ScoreString[k] == '4') SHGUI.current.SetPixelFront(Number4[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'w');
							else if(ScoreString[k] == '5') SHGUI.current.SetPixelFront(Number5[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'w');
							else if(ScoreString[k] == '6') SHGUI.current.SetPixelFront(Number6[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'w');
							else if(ScoreString[k] == '7') SHGUI.current.SetPixelFront(Number7[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'w');
							else if(ScoreString[k] == '8') SHGUI.current.SetPixelFront(Number8[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'w');
							else if(ScoreString[k] == '9') SHGUI.current.SetPixelFront(Number9[i][j], leftPadding + j + k * 6 + 1, 1 + i, 'w');
						}
					}
				}
			}
		}


	}

	public override void ReactToInputKeyboard(SHGUIinput key) {
		//sterowanie
		if (key == SHGUIinput.up || key == SHGUIinput.enter)
		{
			if(PlayGame)
			{
				birdAnimTime = 0.4f;
				LastHeight 	= Height;
				FlyTime		= -Mathf.Sqrt(HeightFactor);
			}
			else
			{
				//Start/Reset Gry
				for(int i = 0; i < Wall.Length; ++i) Wall[i] = new pipe(70 + 30f * i, (float)Random.Range(11, 15));
				Height				= 15f;
				LastHeight			= 15f;
				endFleshTimer		= 0.1f;
				endFall				= 0.1f;
				
				Fall				= 15f;
				FlyTime				= 0f;
				Score				= 0;
				PlayGame			= true;
				Lose				= false;
			}

		}
		
		if (key == SHGUIinput.esc) SHGUI.current.PopView();
	}

	float CalcHeight(float x) //wyliczanie wysokości ptaka (do małej poprawki)
	{	
		return Mathf.Pow(x, 2) - HeightFactor;
	}

}
