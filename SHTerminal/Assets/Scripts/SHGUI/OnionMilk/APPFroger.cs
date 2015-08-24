using UnityEngine;
using System.Collections;

public class APPFroger : SHGUIappbase {

	private enum CellState : byte {
		Empty = 0,
		Train = 1,
	}

	//62x22
	CellState[,] map = new CellState[32, 19]; //mapa;

	int posX = 16; //pozycja żabki
	int posY = 17;

	char frog = '⌂'; //żaba
	char train = '█'; //pociąg
	char border = '│'; //ścianka

	int lvl = 0;

	int harder = 0;

	float trainTimer = 0.2f;


	public APPFroger()
	: base("hot_train-v1.3-by-onionmilk") {
		NextLevel();
	}
	
	public override void Update() {
		base.Update();

		if(lvl >= 1000) //zmiana nazwy apki zależna od lewelu
		{
			APPLABEL.text = "hot_train-v1.3-by-onionmilk LEVEL: OVER 1000";
		}
		else if(lvl >= 50)
		{
			APPLABEL.text = "hot_train-v1.3-by-onionmilk SUPER LEVEL: " + lvl;
		}
		else
		{
			APPLABEL.text = "hot_train-v1.3-by-onionmilk LEVEL: " + lvl;
		}

		//poruszanie pociągami
		trainTimer -= Time.unscaledDeltaTime;
		if(trainTimer <= 0f)
		{
			trainTimer = 0.2f;


			for(int j = 0; j < 18; ++j)
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
							if(i == 31) map[0, j] = CellState.Train;
							else map[i + 1, j] = CellState.Train;
						}
					}
				}
			}

		}

		if(map[posX, posY] == CellState.Train) //powrót na start po dotchnięciu pociągu
		{
			posX = 16;
			posY = 17;
		}

	}
	
	public override void Redraw(int offx, int offy) {
		base.Redraw(offx, offy);

		for(int i = 0; i < 18; ++i) SHGUI.current.SetPixelFront(border, 17, i + 2, 'w'); //ściana I
		for(int i = 0; i < 18; ++i) SHGUI.current.SetPixelFront(border, 50, i + 2, 'w'); //ściana II

		for(int i = 0; i < 32; ++i)
		{
			for(int j = 0; j < 18; ++j)
			{
				SHGUI.current.SetPixelFront(' ', i + 18, j + 2, 'w'); //nicość

				if(map[i, j] == CellState.Train)
				{
					SHGUI.current.SetPixelBack(train, i + 18, j + 2, 'r'); //rysowanie pociągów
				}
			}
		}
		SHGUI.current.SetPixelFront(frog, posX + 18, posY + 2, 'w'); //rysowanie żaby

	}
	
	public override void ReactToInputKeyboard(SHGUIinput key) {
		//sterowanie
		if (key == SHGUIinput.up) {

			if(posY > 1) --posY;
			else
			{
				posY = 0;
				if(lvl < 1000) NextLevel();
			}
		}
		if (key == SHGUIinput.down) {
			if(posY < 17) ++posY;
		}
		if (key == SHGUIinput.right) {
			if(posX < 31) ++posX;
		}
		if (key == SHGUIinput.left) {
			if(posX > 0) --posX;
		}

		
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView();
		//--
	}

	private void NextLevel()
	{
		Random.seed = lvl;
		++lvl;

		posX = 16;
		posY = 17;

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


		for(int i = 0; i < 32; ++i) for(int j = 0; j < 18; ++j) map[i, j] = CellState.Empty; //czyszczenie mapy

		int temp = 10;
		int temp2 = 0;

		if(lvl < 1000) //ustawianie początkowej pozycji pociągu
		{
			for(int j = 1; j < 17; ++j)
			{
				if(j == 13 && harder < 2) continue;
				if(j == 9 && harder < 9) continue;
				if(j == 4 && harder < 4) continue;

				temp = Random.Range(1 + harder, 8 + harder);

				for(int i = 0; i < temp; ++i)
				{
					temp2 = Random.Range(0, 32);
					if(map[temp2, j] == CellState.Train)
					{
						for(;; ++temp2)
						{
							if(temp2 == 32) temp2 = 0;
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