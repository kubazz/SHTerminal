using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class wirzcholekGrafu
{
	public int posX;
	public int posY;
	public bool visited;

	public wirzcholekGrafu(int pX, int pY)
	{
		posX = pX;
		posY = pY;
		visited = false;
	}
};


public class APPMaze : SHGUIappbase {

	bool[,] map = new bool[62, 22];

	int posX = 0;
	int posY = 0;

	int posEndX = 0;
	int posEndY = 0;

	bool play = false;

	char character = '*';
	char wall = '█';

	bool changeGenerator = true;

	int currMaze = 0;

	public APPMaze()
	: base("simple_maze-v1.23.1-by-onionmilk") {
		MakeInMaze();
	}

	public override void Update() {
		base.Update();

		APPLABEL.text = "simple_maze-v1.23.1-by-onionmilk  maze: "+currMaze;

		if(posX == posEndX && posY == posEndY)
		{
			play = false;
			MakeInMaze();
		}

	}

	public override void Redraw(int offx, int offy) {
		base.Redraw(offx, offy);

		for(int i = 0; i < 62; ++i) //ściany
		{
			for(int j = 0; j < 22; ++j)
			{
				if(map[i, j] == true) SHGUI.current.SetPixelBack(wall, 1 + i, 1 + j, 'z');
			}
		}

		SHGUI.current.SetPixelFront(character, posX + 1, posY + 1, 'w'); //postać
		SHGUI.current.SetPixelFront(wall, posEndX + 1, posEndY + 1, 'r'); //koniec mapy
	}

	public override void ReactToInputKeyboard(SHGUIinput key) {
		//sterowanie
		if (key == SHGUIinput.up) {
			
			if(posY > 0 && play)
			{
				if(map[posX, posY - 1] == false) --posY;
			}
		}
		if (key == SHGUIinput.down && play) {
			if(posY < 21)
			{
				if(map[posX, posY + 1] == false) ++posY;
			}
		}
		if (key == SHGUIinput.right && play) {
			if(posX < 61)
			{
				if(map[posX + 1, posY] == false) ++posX;
			}
		}
		if (key == SHGUIinput.left && play) {
			if(posX > 0)
			{
				if(map[posX - 1, posY] == false) --posX;
			}
		}
		
		
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView();
		//--
	}

	private void MakeInMaze()
	{
		int tempDir = Random.Range(0, 4);
		if(tempDir == 0)
		{
			posX = Random.Range(0, 62);
			posY = 0;
		}
		else if(tempDir == 1)
		{
			posX = Random.Range(0, 62);
			posY = 21;
		}
		else if(tempDir == 2)
		{
			posX = 61;
			posY = Random.Range(0, 22);
		}
		else if(tempDir == 3)
		{
			posX = 0;
			posY = Random.Range(0, 22);
		}


		++currMaze;
		//tworzenie labiryntu
		for(int i = 0; i < 62; ++i) for(int j = 0; j < 22; ++j) map[i, j] = false;

		for(int y = 0; y < 22; ++y)
		{
			if (y%2 == 1)
			{
				for(int x = 0; x < 62; ++x)
				{
	 				if (x%2 == 1)
					{
						map[x, y]	= true;
					}
					if (x%2 == 0)
					{
						if(Random.value > 0.8f) map[x, y] = true;
					}
				}
			}
			if (y%2 == 0)
			{
				for(int x = 0; x < 62; ++x)
				{
					if (x%2 == 1)
					{
						if(Random.value > 0.8f) map[x, y] = true;
					}
				}
			}
		}
		for(int i = 1; i < 61; ++i)
		{
			for(int j = 1; j < 21; ++j)
			{
				if(map[i + 1, j] == false && map[i - 1, j] == false && map[i, j + 1] == false && map[i, j - 1] == false)
				{
					int randomTemp = Random.Range(0, 4);
					if(randomTemp == 0) map[i, j + 1] = true;
					else if(randomTemp == 1) map[i, j - 1] = true;
					else if(randomTemp == 2) map[i - 1, j] = true;
					else if(randomTemp == 3) map[i + 1, j] = true;
				}
			}
		}
		map[posX, posY] = false;

		FindPath();
	}

	private void FindPath()
	{
		//wyszukiwanie najdluższej drogi by postawić na niej koniec labiryntu
		wirzcholekGrafu[,] visitedMap = new wirzcholekGrafu[62, 22];
		for(int i = 0; i < 62; ++i) for(int j = 0; j < 22; ++j) visitedMap[i, j] = new wirzcholekGrafu(i, j);

		Queue<wirzcholekGrafu> kolejka = new Queue<wirzcholekGrafu>();

		kolejka.Enqueue(visitedMap[posX, posY]);
		kolejka.Peek().visited = true;

		for(;;)
		{
			if(kolejka.Peek().posX != 0) //lewa krawędź
			{
				if(map[kolejka.Peek().posX - 1, kolejka.Peek().posY] == false) //sprawdzenie czy nie jest czasem ścianą
				{
					if(visitedMap[kolejka.Peek().posX - 1, kolejka.Peek().posY].visited == false) //sprawdzenie czy nie został odwiedzony
					{
						kolejka.Enqueue(visitedMap[kolejka.Peek().posX - 1, kolejka.Peek().posY]);
						visitedMap[kolejka.Peek().posX - 1, kolejka.Peek().posY].visited = true;
					}
				}
			}
			if(kolejka.Peek().posX != 61) //prawa krawędź
			{
				if(map[kolejka.Peek().posX + 1, kolejka.Peek().posY] == false) //sprawdzenie czy nie jest czasem ścianą
				{
					if(visitedMap[kolejka.Peek().posX + 1, kolejka.Peek().posY].visited == false) //sprawdzenie czy nie został odwiedzony
					{
						kolejka.Enqueue(visitedMap[kolejka.Peek().posX + 1, kolejka.Peek().posY]);
						visitedMap[kolejka.Peek().posX + 1, kolejka.Peek().posY].visited = true;
					}
				}
			}
			if(kolejka.Peek().posY != 0) //górna krawędź
			{
				if(map[kolejka.Peek().posX, kolejka.Peek().posY - 1] == false) //sprawdzenie czy nie jest czasem ścianą
				{
					if(visitedMap[kolejka.Peek().posX, kolejka.Peek().posY - 1].visited == false) //sprawdzenie czy nie został odwiedzony
					{
						kolejka.Enqueue(visitedMap[kolejka.Peek().posX, kolejka.Peek().posY - 1]);
						visitedMap[kolejka.Peek().posX, kolejka.Peek().posY - 1].visited = true;
					}
				}
			}
			if(kolejka.Peek().posY != 21) //dolna krawędź
			{
				if(map[kolejka.Peek().posX, kolejka.Peek().posY + 1] == false) //sprawdzenie czy nie jest czasem ścianą
				{
					if(visitedMap[kolejka.Peek().posX, kolejka.Peek().posY + 1].visited == false) //sprawdzenie czy nie został odwiedzony
					{
						kolejka.Enqueue(visitedMap[kolejka.Peek().posX, kolejka.Peek().posY + 1]);
						visitedMap[kolejka.Peek().posX, kolejka.Peek().posY + 1].visited = true;
					}
				}
			}

			//kolejka.Peek().visited = true;
			posEndX = kolejka.Peek().posX;
			posEndY = kolejka.Peek().posY;

			kolejka.Dequeue();

			if(kolejka.Count == 0)
			{
				play = true;
				return;
			}
		}
	}
}
