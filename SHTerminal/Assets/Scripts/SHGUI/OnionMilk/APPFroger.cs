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
		SHGUI.current.SetPixelFront('T', 53, 8, 'w');
		SHGUI.current.SetPixelFront('I', 54, 8, 'w');
		SHGUI.current.SetPixelFront('M', 55, 8, 'w');
		SHGUI.current.SetPixelFront('E', 56, 8, 'w');
		for(int u = 0; u < gameTimeString.Length; ++u) SHGUI.current.SetPixelFront(gameTimeString[u], 55 - (gameTimeString.Length / 2) + u, 9, 'w');
		//MOVE
		SHGUI.current.SetPixelFront('M', 53, 12, 'w');
		SHGUI.current.SetPixelFront('O', 54, 12, 'w');
		SHGUI.current.SetPixelFront('V', 55, 12, 'w');
		SHGUI.current.SetPixelFront('E', 56, 12, 'w');
		for(int u = 0; u < moveCountString.Length; ++u) SHGUI.current.SetPixelFront(moveCountString[u], 55 - (moveCountString.Length / 2) + u, 13, 'w');
		//DEAD
		SHGUI.current.SetPixelFront('D', 53, 16, 'w');
		SHGUI.current.SetPixelFront('E', 54, 16, 'w');
		SHGUI.current.SetPixelFront('A', 55, 16, 'w');
		SHGUI.current.SetPixelFront('D', 56, 16, 'w');
		for(int u = 0; u < deadCountString.Length; ++u) SHGUI.current.SetPixelFront(deadCountString[u], 55 - (deadCountString.Length / 2) + u, 17, 'w');
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
		
		if (key == SHGUIinput.esc) SHGUI.current.PopView();

	}

	private void NextLevel()
	{
		Random.seed = lvl;
		++lvl;

		posX = 16;
		posY = 21;

		//zwiększanie poziomu trudności
		if(lvl > 55) harder = 9;
		else if(lvl > 45) harder = 8;
		else if(lvl > 36) harder = 7;
		else if(lvl > 28) harder = 6;
		else if(lvl > 21) harder = 5;
		else if(lvl > 15) harder = 4;
		else if(lvl > 10) harder = 3;
		else if(lvl > 6) harder = 2;
		else if(lvl > 3) harder = 1;


		for(int i = 0; i < 32; ++i) for(int j = 0; j < 22; ++j) map[i, j] = CellState.Empty; //czyszczenie mapy

		int temp = 10;
		int temp2 = 0;

		if(lvl < 1000) //ustawianie początkowej pozycji pociągu
		{
			for(int j = 1; j < 21; ++j)
			{
				if(j == 17 && harder < 2) continue;
				if(j == 11 && harder < 9) continue;
				if(j == 5 && harder < 4) continue;

				temp = Random.Range(1 + harder, 8 + harder);

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