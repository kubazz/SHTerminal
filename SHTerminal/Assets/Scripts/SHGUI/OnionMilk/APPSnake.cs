using UnityEngine;
using System.Collections;

public class APPSnake : SHGUIappbase {
	//Private
		//Grafika
	char[]	objList			= {
		' ', 	//0	- pustka
		'▀',	//1	- sciana
		'▄',
		'█',	
		'⁰',	//2	- jedzonko
		'₀',
		'8'
	};

	char[]	snakeGfx = {
		'▀', '▄', '▒'
	};
	char	snakeColor		= 'r';
	
		//Mapa
	//62x22
	int[,]	map				= new int[62, 44];

		//Snake
	Vector2	snakePos		= Vector2.zero;
	int[,]	snakeTailMap	= new int[62, 44];
	int		snakeTailLength	= 5;
	int		snakeDirecton	= 0; /*
			0
		3		1
			2
	*/
	float	snakeSpeedTimer	= 0;
	float	snakeSpeed		= 0.3f;
	bool	snakeDead		= false;

	//Public
	public APPSnake()
		:	base("snake-vS.S.S.BOOM-by-onionmilk")
	{
		for(int y = 0; y < map.GetLength(1); ++y)
			for(int x = 0; x < map.GetLength(0); ++x) {
				if (x == 0 || x == (map.GetLength(0) - 1) || y == 0 || y == (map.GetLength(1) - 1))
					map[x, y]		= 1;
				else
					map[x, y]		= 0;
				//--
				snakeTailMap[x, y]	= 0;
			}
		//--
		
		snakePos	= new Vector2(31, 10);
		for(int y = (int)snakePos.y; y < Mathf.FloorToInt(snakePos.y + 5); ++y) {
			snakeTailMap[(int)snakePos.x, y]	= snakeTailLength + (((int)snakePos.y) - y);
		}

		generateFood();
	} 

	public override void Update() {
		base.Update();
		if (snakeDead)	return;

		//Snake Movement
		snakeSpeedTimer	+= Time.unscaledDeltaTime;
		if (snakeSpeedTimer > snakeSpeed) {
			snakeMove();
		}
	}

	public override void Redraw(int offx, int offy) {
		base.Redraw(offx, offy);
		if (snakeDead)	return;

		//SHGUI.current.SetPixelFront(char character, int posX, int posY, char color);
		//Map
		for(int y = 0; y < 22; ++y)
			for(int x = 0; x < map.GetLength(0); ++x) {
				if (map[x, (y * 2)] == 1 && map[x, (y * 2) + 1] == 1) {
					SHGUI.current.SetPixelFront(objList[3], 1 + x, 1 + y, 'w');
				} else if (map[x, (y * 2)] == 1 && map[x, (y * 2) + 1] != 1) {
					SHGUI.current.SetPixelFront(objList[1], 1 + x, 1 + y, 'w');
				} else if (map[x, (y * 2)] != 1 && map[x, (y * 2) + 1] == 1) {
					SHGUI.current.SetPixelFront(objList[2], 1 + x, 1 + y, 'w');
				}
				
				if (map[x, (y * 2)] == 2 && map[x, (y * 2) + 1] == 2) {
					SHGUI.current.SetPixelBack(objList[6], 1 + x, 1 + y, 'r');
				} else if (map[x, (y * 2)] == 2 && map[x, (y * 2) + 1] != 2) {
					SHGUI.current.SetPixelBack(objList[4], 1 + x, 1 + y, 'r');
				} else if (map[x, (y * 2)] != 2 && map[x, (y * 2) + 1] == 2) {
					SHGUI.current.SetPixelBack(objList[5], 1 + x, 1 + y, 'r');
				}
			}
		//--
		//Snake
		for(int y = 0; y < 22; ++y)
			for(int x = 0; x < map.GetLength(0); ++x)
				if (snakeTailMap[x, (y * 2)] > 0 && snakeTailMap[x, (y * 2) + 1] > 0) {
					SHGUI.current.SetPixelFront(snakeGfx[2], 1 + x, 1 + y, snakeColor);
				} else if (snakeTailMap[x, (y * 2)] > 0 && snakeTailMap[x, (y * 2) + 1] <= 0) {
					SHGUI.current.SetPixelFront(snakeGfx[0], 1 + x, 1 + y, snakeColor);
				} else if (snakeTailMap[x, (y * 2)] <= 0 && snakeTailMap[x, (y * 2) + 1] > 0) {
					SHGUI.current.SetPixelFront(snakeGfx[1], 1 + x, 1 + y, snakeColor);
				}
			//--
		//--
	}

	public override void ReactToInputKeyboard(SHGUIinput key) {
		if (snakeDirecton == 1 || snakeDirecton == 3) {
			if (key == SHGUIinput.up)
				snakeDirecton	= 0;
			else if (key == SHGUIinput.down)
				snakeDirecton	= 2;
			//--
		} else if (snakeDirecton == 0 || snakeDirecton == 2) {
			if (key == SHGUIinput.left)
				snakeDirecton	= 3;
			else if (key == SHGUIinput.right)
				snakeDirecton	= 1;
			//--
		}
		
		if (key == SHGUIinput.enter)
			generateFood();
		//--

		if (key == SHGUIinput.esc)
			SHGUI.current.PopView();
		//--
	}

	void snakeMove() {
		bool	move	= true;
		switch(snakeDirecton) {
			case(0): {
				snakePos.y	-= 1;

				break;
			}
			case(1): {
				snakePos.x	+= 1;

				break;
			}
			case(2): {
				snakePos.y	+= 1;

				break;
			}
			case(3): {
				snakePos.x	-= 1;

				break;
			}
		}

		if (snakePos.x >= snakeTailMap.GetLength(0) || snakePos.x < -1)
			snakePos.x	= snakePos.x % snakeTailMap.GetLength(0);
		//--
		if (snakePos.y >= snakeTailMap.GetLength(1) || snakePos.y < -1)
			snakePos.y	= snakePos.y % snakeTailMap.GetLength(1);
		//--
		
		if (snakeTailMap[(int)snakePos.x, (int)snakePos.y] > 0
		||	map[(int)snakePos.x, (int)snakePos.y] == 1
		) {
			switch(snakeDirecton) {
				case(0): {
					snakePos.y	+= 1;

					break;
				}
				case(1): {
					snakePos.x	-= 1;

					break;
				}
				case(2): {
					snakePos.y	-= 1;

					break;
				}
				case(3): {
					snakePos.x	+= 1;

					break;
				}
			}

			snakeDead	= true;
			move	= false;
		} else if (map[(int)snakePos.x, (int)snakePos.y] == 2) {
			map[(int)snakePos.x, (int)snakePos.y]	=	0;
			snakeTailLength							+=	1;
			generateFood();
		} else {
			//NICZ
		}

		if (move) {
			snakeTailMap[(int)snakePos.x, (int)snakePos.y]	= snakeTailLength + 1;

			for(int y = 0; y < map.GetLength(1); ++y)
				for(int x = 0; x < map.GetLength(0); ++x)
					if (snakeTailMap[x, y] > 0)
						snakeTailMap[x, y]	-= 1;
					//--
				//--
			//--
		}

		snakeSpeedTimer		= 0f;
	}

	void generateFood() {
		int	x	= Mathf.FloorToInt(Random.value * map.GetLength(0));
		int	y	= Mathf.FloorToInt(Random.value * map.GetLength(1));

		if (snakeTailMap[x, y] != 0 || map[x, y] != 0)
			generateFood();
		else {
			snakeSpeed	= 0.95f * snakeSpeed;
			map[x, y]	= 2;
		}
		
		return;
	}
}
