using UnityEngine;
using System.Collections;

public class platform
{
	public int posX;
	public int posY;
	public bool hole;

	public platform(int pX, int pY)
	{
		posX = pX;
		posY = 20 - pY;
		hole = false;
	}

	public void SetY(int pY) //ustawienie wysokości
	{
		posY = 20 - pY;
	}

	public void TranslatePlatform(int oldY, bool ifHole) //jeśli platforma wyjdze z obszaru gry przenosimy ją na początek
	{
		if(ifHole) hole = false;
		else
		{
			if(Random.value > 0.95) hole = true;
		}

		//wyznaczenie wysokości
		oldY = 20 - oldY;
		int pY = Random.Range(oldY - 1, oldY + 2);
		if(pY < 0) pY = 0;
		else if(pY > 10) pY = 10;

		posY = 20 - pY;

		posX = -3;
	}
};

public class APPRunner : SHGUIappbase {

	//postać
	bool jump = false;
	bool doubleJump = false;
	bool air = false;
	int posX = 58;
	int posY = 15;
	int jumpPos = 7; 

	//platformy
	public platform[] map = new platform[18];

	//wygląd
	char character = '«';
	char enemy = '☼';
	char bullet = '-';
	char ground = '░';
	char grass = '▓';

	//mechanika
	float runTimer = 0.1f;
	float flyTimer = 0.05f;
	bool move = true;

	//inne
	bool lose = false;
	int score = 0;
	string scoreString = "";

	public APPRunner()
		: base("Reverse_Runner_ASCII-v0.1-by-onionmilk")
	{
		BeginMap();
	}

	public override void Update()
	{
		base.Update();

		if(!lose)
		{
			runTimer -= Time.unscaledDeltaTime;
			flyTimer -= Time.unscaledDeltaTime;

			if(flyTimer <= 0f)
			{
				flyTimer = 0.08f;

				for(int i = 0; i < 18; ++i) //spadanie
				{
					if(map[i].posX >= 54 && map[i].posX <= 58 && posY < (map[i].posY - 1) && !map[i].hole) //jeśli znajduje się nad ziemią (spadanie)
					{
						if(jumpPos == 0)
						{
							++posY;
							if(map[i].posX >= 54 && map[i].posX <= 58 && posY > (map[i].posY)) posY = map[i].posY-1;
							break;
						}
					}
					else if(map[i].posX >= 54 && map[i].posX <= 58 && posY == map[i].posY - 1 && jumpPos == 0 && !map[i].hole) //ustawianie, że można ponownie skoczyć
					{
						jump = false;
						doubleJump = false;
					}
					else if(map[i].posX >= 54 && map[i].posX <= 58 && map[i].hole)
					{
						++posY;
						break;
					}
				}
				if(jumpPos > 0) //wznoszenie się po skoku
				{
					--posY;
					--jumpPos;
				}

				if(posY > 20) lose = true;
			}

			if(runTimer <= 0f)
			{
				runTimer = 0.1f;

				for(int i = 0; i < 18; ++i) //blokada by nie właził w ściany
				{
					if(map[i].posX + 4 == 58 && map[i].hole)
					{
						move = true;
						break;
					}
					if(map[i].posX + 4 == 58 && map[i].posY <= posY)
					{
						move = false;
						break;
					}
					else move = true;
				}

				if(move) //przemieszczanie się platform
				{
					for(int i = 0; i < 18; ++i)
					{

						++map[i].posX; //przemieszczanie platform
						if(map[i].posX > 68)
						{
							if(i < 17) map[i].TranslatePlatform(map[i+1].posY, map[i+1].hole);
							else map[i].TranslatePlatform(map[0].posY, map[0].hole);
						}
					}
				}
			}
		}
	}

	public override void Redraw(int offx, int offy)
	{
		base.Redraw(offx, offy);

		for(int i = 0; i < 18; ++i)
		{
			if(map[i].posX > 0 && map[i].posX < 63 && !map[i].hole)
			{
				SHGUI.current.SetPixelBack(grass, map[i].posX, map[i].posY, 'z');
				for(int j = map[i].posY + 1; j < 23; ++j)
				{
					SHGUI.current.SetPixelBack(ground, map[i].posX, j, 'z');
				}
			}
			if(map[i].posX + 1 > 0 && map[i].posX + 1 < 63 && !map[i].hole)
			{
				SHGUI.current.SetPixelBack(grass, map[i].posX + 1, map[i].posY, 'z');
				for(int j = map[i].posY + 1; j < 23; ++j)
				{
					SHGUI.current.SetPixelBack(ground, map[i].posX + 1, j, 'z');
				}
			}
			if(map[i].posX + 2 > 0 && map[i].posX + 2 < 63 && !map[i].hole)
			{
				SHGUI.current.SetPixelBack(grass, map[i].posX + 2, map[i].posY, 'z');
				for(int j = map[i].posY + 1; j < 23; ++j)
				{
					SHGUI.current.SetPixelBack(ground, map[i].posX + 2, j, 'z');
				}
			}
			if(map[i].posX + 3 > 0 && map[i].posX + 3 < 63 && !map[i].hole)
			{
				SHGUI.current.SetPixelBack(grass, map[i].posX + 3, map[i].posY, 'z');
				for(int j = map[i].posY + 1; j < 23; ++j)
				{
					SHGUI.current.SetPixelBack(ground, map[i].posX + 3, j, 'z');
				}
			}
		}

		SHGUI.current.SetPixelFront(character, posX, posY, 'r');
		SHGUI.current.SetPixelFront(character, posX, posY+1, 'r');

	}

	public override void ReactToInputKeyboard(SHGUIinput key)
	{
		//sterowanie
		if (key == SHGUIinput.esc) SHGUI.current.PopView();

		if (key == SHGUIinput.enter)
		{
			if(!jump)
			{
				jump = true;
				jumpPos = 5;
			}
			else if(!doubleJump && jump)
			{
				doubleJump = true;
				jumpPos = 5;
			}

			if(lose)
			{
				lose = false;
				BeginMap();
			}
		}
	}

	void BeginMap()
	{

		for(int i = 0; i < 18; ++i)
		{
			map[i] = new platform(i*4, 2);
		}
		posY = 15;
	}
}
