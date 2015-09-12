using UnityEngine;
using System.Collections;

public class APPFroger : SHGUIappbase {

	private enum CellState : byte
	{
		Empty = 0,
		Train = 1,
	}

	//62x22
	CellState[,] map = new CellState[32, 22]; //mapa;

	int posX = 16; //pozycja żabki
	int posY = 21;

	char frog = '⌂'; //żaba
	char train = '█'; //pociąg
	char wall = '│'; //ściana
	char end = '░'; //końcówka

	int lvl = 0; //aktualny poziom
	int deadCount = 0; //ilosć śmierci
	int moveCount = 0; //ilość ruchów
	float gameTime = 0f; //czas gry

	string lvlString = "1";
	string deadCountString = "0";
	string moveCountString = "0";
	string gameTimeString = "0";

	int lvlBest = 0; //najlepszy poziom
	int deadCountBest = 0; //najmniejsza ilość śmierci
	int moveCountBest = 0; //najmniejsza ilość ruchów
	float gameTimeBest = 0f; //najlepszy czas

	string lvlStringBest = "1";
	string deadCountStringBest = "0";
	string moveCountStringBest = "0";
	string gameTimeStringBest = "0";

	int harder = 0;

	//float trainTimer = 0.2f;

	bool moveTrain = false;

	public APPFroger()
	: base("hot_train-v1.3-by-onionmilk")
	{
		NextLevel();
	}
	
	public override void Update() 
	{
		base.Update();

		gameTime += Time.unscaledDeltaTime;

		//poruszanie pociągami
		//trainTimer -= Time.unscaledDeltaTime; //ruszanie co pewien okres czasu
		//if(trainTimer <= 0f)
		//{
		//	trainTimer = 0.2f;
		if(moveTrain) //poruszanie przy ruchu
		{
			moveTrain = false;

			for(int j = 0; j < 21; ++j)
			{	
				if(j%2 == 0) //ruch pociągów w lewo
				{
					for(int i = 0; i < 32; ++i)
					{
						if(map[i, j] == CellState.Train) 
						{
							map[i, j] = CellState.Empty;
							if(i == 0) map[31, j] = CellState.Train;
							else map[i - 1, j] = CellState.Train;
						}
					}
				}
				else //ruch pociągów w prawo
				{
					for(int i = 31; i >= 0; --i)
					{
						if(map[i, j] == CellState.Train) 
						{
							map[i, j] = CellState.Empty;
							if(i == 30) map[0, j] = CellState.Train;
							else map[i + 1, j] = CellState.Train;
						}
					}
				}
			}

		}
		if(map[posX, posY] == CellState.Train) //powrót na start po dotchnięciu pociągu
		{
			posX = 16;
			posY = 21;
			++deadCount;
		}

	}
	
	public override void Redraw(int offx, int offy)
	{
		base.Redraw(offx, offy);

		for(int i = 0; i < 32; ++i)
		{
			SHGUI.current.SetPixelBack(end, i + 15, 1, 'z'); //rysowanie pociągów
		}
		for(int i = 0; i < 22; ++i)
		{
			SHGUI.current.SetPixelFront(wall, 14, i + 1, 'w'); //rysowanie pociągów
			SHGUI.current.SetPixelFront(wall, 47, i + 1, 'w'); //rysowanie pociągów
		}

		for(int i = 0; i < 32; ++i)
		{
			for(int j = 0; j < 22; ++j)
			{
				if(map[i, j] == CellState.Train)
				{
					SHGUI.current.SetPixelFront(train, i + 15, j + 1, 'r'); //rysowanie pociągów
				}
			}
		}
		SHGUI.current.SetPixelFront(frog, posX + 15, posY + 1, 'w'); //rysowanie żaby

		lvlString = lvl.ToString();
		moveCountString = moveCount.ToString();
		gameTimeString = gameTime.ToString("F0");
		deadCountString = deadCount.ToString();

		//LEVEL
		SHGUI.current.SetPixelFront('L', 53, 4, 'w');
		SHGUI.current.SetPixelFront('E', 54, 4, 'w');
		SHGUI.current.SetPixelFront('V', 55, 4, 'w');
		SHGUI.current.SetPixelFront('E', 56, 4, 'w');
		SHGUI.current.SetPixelFront('L', 57, 4, 'w');
		for(int u = 0; u < lvlString.Length; ++u) SHGUI.current.SetPixelFront(lvlString[u], 55 - (lvlString.Length / 2) + u, 5, 'w');
		//TIME
		SHGUI.current.SetPixelFront('T', 53, 16, 'w');
		SHGUI.current.SetPixelFront('I', 54, 16, 'w');
		SHGUI.current.SetPixelFront('M', 55, 16, 'w');
		SHGUI.current.SetPixelFront('E', 56, 16, 'w');
		for(int u = 0; u < gameTimeString.Length; ++u) SHGUI.current.SetPixelFront(gameTimeString[u], 55 - (gameTimeString.Length / 2) + u, 17, 'w');
		//MOVE
		SHGUI.current.SetPixelFront('M', 53, 12, 'w');
		SHGUI.current.SetPixelFront('O', 54, 12, 'w');
		SHGUI.current.SetPixelFront('V', 55, 12, 'w');
		SHGUI.current.SetPixelFront('E', 56, 12, 'w');
		for(int u = 0; u < moveCountString.Length; ++u) SHGUI.current.SetPixelFront(moveCountString[u], 55 - (moveCountString.Length / 2) + u, 13, 'w');
		//DEAD
		SHGUI.current.SetPixelFront('D', 53, 8, 'w');
		SHGUI.current.SetPixelFront('E', 54, 8, 'w');
		SHGUI.current.SetPixelFront('A', 55, 8, 'w');
		SHGUI.current.SetPixelFront('D', 56, 8, 'w');
		for(int u = 0; u < deadCountString.Length; ++u) SHGUI.current.SetPixelFront(deadCountString[u], 55 - (deadCountString.Length / 2) + u, 9, 'w');
	

		//Besty
		lvlStringBest = lvlBest.ToString();
		moveCountStringBest = moveCountBest.ToString();
		gameTimeStringBest = gameTimeBest.ToString("F0");
		deadCountStringBest = deadCountBest.ToString();

		SHGUI.current.SetPixelFront('B', 2, 2, 'w');
		SHGUI.current.SetPixelFront('E', 3, 2, 'w');
		SHGUI.current.SetPixelFront('S', 4, 2, 'w');
		SHGUI.current.SetPixelFront('T', 5, 2, 'w');
		SHGUI.current.SetPixelFront('S', 7, 2, 'w');
		SHGUI.current.SetPixelFront('C', 8, 2, 'w');
		SHGUI.current.SetPixelFront('O', 9, 2, 'w');
		SHGUI.current.SetPixelFront('R', 10, 2, 'w');
		SHGUI.current.SetPixelFront('E', 11, 2, 'w');

		//LEVEL
		SHGUI.current.SetPixelFront('L', 4, 4, 'w');
		SHGUI.current.SetPixelFront('E', 5, 4, 'w');
		SHGUI.current.SetPixelFront('V', 6, 4, 'w');
		SHGUI.current.SetPixelFront('E', 7, 4, 'w');
		SHGUI.current.SetPixelFront('L', 8, 4, 'w');
		for(int u = 0; u < lvlStringBest.Length; ++u) SHGUI.current.SetPixelFront(lvlStringBest[u], 6 - (lvlStringBest.Length / 2) + u, 5, 'w');
		//TIME
		SHGUI.current.SetPixelFront('T', 4, 16, 'w');
		SHGUI.current.SetPixelFront('I', 5, 16, 'w');
		SHGUI.current.SetPixelFront('M', 6, 16, 'w');
		SHGUI.current.SetPixelFront('E', 7, 16, 'w');
		for(int u = 0; u < gameTimeStringBest.Length; ++u) SHGUI.current.SetPixelFront(gameTimeStringBest[u], 6 - (gameTimeStringBest.Length / 2) + u, 17, 'w');
		//MOVE
		SHGUI.current.SetPixelFront('M', 4, 12, 'w');
		SHGUI.current.SetPixelFront('O', 5, 12, 'w');
		SHGUI.current.SetPixelFront('V', 6, 12, 'w');
		SHGUI.current.SetPixelFront('E', 7, 12, 'w');
		for(int u = 0; u < moveCountStringBest.Length; ++u) SHGUI.current.SetPixelFront(moveCountStringBest[u], 6 - (moveCountStringBest.Length / 2) + u, 13, 'w');
		//DEAD
		SHGUI.current.SetPixelFront('D', 4, 8, 'w');
		SHGUI.current.SetPixelFront('E', 5, 8, 'w');
		SHGUI.current.SetPixelFront('A', 6, 8, 'w');
		SHGUI.current.SetPixelFront('D', 7, 8, 'w');
		for(int u = 0; u < deadCountStringBest.Length; ++u) SHGUI.current.SetPixelFront(deadCountStringBest[u], 6 - (deadCountStringBest.Length / 2) + u, 9, 'w');
	}
	
	public override void ReactToInputKeyboard(SHGUIinput key)
	{
		//sterowanie
		if (key == SHGUIinput.up)
		{

			if(posY > 0) --posY;
			else
			{
				posY = 0;
				if(lvl < 1000) NextLevel();
			}
			++moveCount;
			moveTrain = true;
		}
		if (key == SHGUIinput.down)
		{
			if(posY < 21) ++posY;
			++moveCount;
			moveTrain = true;
		}
		if (key == SHGUIinput.right)
		{
			if(posX < 30) ++posX;
			++moveCount;
			moveTrain = true;
		}
		if (key == SHGUIinput.left)
		{
			if(posX > 0) --posX;
			++moveCount;
			moveTrain = true;
		}
		
		if (key == SHGUIinput.esc)
		{
			if(lvl > lvlBest) //zapisanie wyniku jeśli jest lepszy
			{
				SetBestScore();
			}
			else if(lvl == lvlBest)
			{
				if(deadCount < deadCountBest)
				{
					SetBestScore();
				}
				else if(deadCount == deadCountBest)
				{
					if(moveCount < moveCountBest)
					{
						SetBestScore();
					}
					else if(moveCount == moveCountBest)
					{
						if(gameTimeBest < gameTimeBest)
						{
							SetBestScore();
						}
					}
				}
			}

			SHGUI.current.PopView();
		}

	}

	void SetBestScore()
	{
		lvlBest = lvl;
		deadCountBest = deadCount;
		moveCountBest = moveCount;
		gameTimeBest = gameTime;
	}

	private void NextLevel()
	{
		Random.seed = lvl;
		++lvl;

		posX = 16;
		posY = 21;

		//zwiększanie poziomu trudności
		if(lvl > 33) harder = 11;
		else if(lvl > 30) harder = 10;
		else if(lvl > 27) harder = 9;
		else if(lvl > 24) harder = 8;
		else if(lvl > 21) harder = 7;
		else if(lvl > 18) harder = 6;
		else if(lvl > 15) harder = 5;
		else if(lvl > 12) harder = 4;
		else if(lvl > 9) harder = 3;
		else if(lvl > 6) harder = 2;
		else if(lvl > 3) harder = 1;


		for(int i = 0; i < 32; ++i) for(int j = 0; j < 22; ++j) map[i, j] = CellState.Empty; //czyszczenie mapy

		int temp = 10;
		int temp2 = 0;

		if(lvl < 1000) //ustawianie początkowej pozycji pociągu
		{
			for(int j = 1; j < 21; ++j)
			{
				if(j == 20 && harder < 1) continue;
				if(j == 17 && harder < 2) continue;
				if(j == 11 && harder < 9) continue;
				if(j == 7 && harder < 4) continue;
				if(j == 5 && harder < 7) continue;

				temp = Random.Range(1 + harder, 7 + harder);

				for(int i = 0; i < temp; ++i)
				{
					temp2 = Random.Range(0, 31);
					if(map[temp2, j] == CellState.Train)
					{
						for(;; ++temp2)
						{
							if(temp2 == 31) temp2 = 0;
							if(map[temp2, j] == CellState.Empty)
							{
								map[temp2, j] = CellState.Train;
								break;
							}
						}
					}
					else map[temp2, j] = CellState.Train;
				}
			}
		}
	}
}