using UnityEngine;
using System.Collections;

public class APPSnakeGame {
	//Public
		//Grafika
	public char[]	objList			= {
		' ', 	//0	- pustka
		'▀',	//1	- sciana
		'▄',
		'█',	
		'⁰',	//2	- jedzonko
		'₀',
		'8',
		//==================//
		'\\', '/',
		'/', '\\'
	};

	public char[]	snakeGfx = {
		'▀', '▄', '█'
	};
	public char[]	snakeInGameGfx = {
		'▒',	//Tail
		'▓',	//Body
		'█'		//Head
	};

	public char	snakeColor		= 'r';
	
		//Mapa
	//62x22
	//public int[,]	map				= new int[62, 44];
	//31x11
	public int[,]	map				= new int[30, 10];

		//Snake
	public Vector2	snakePos		= Vector2.zero;
	public int[,]	snakeTailMap	= new int[30, 10];
	public int		snakeTailLength	= 5;
	public int		snakeDirecton	= 0; /*
			0
		3		1
			2
	*/
	public float	snakeSpeedTimer	= 0;
	public float	snakeSpeed		= 0.3f;
	public bool		snakeDead		= false;

	public int		foodAmount		= 0;

	public APPSnakeGame() {
		restart();
	}

	public void restart() {
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
		
		snakeTailLength	= 5;
		snakeDirecton	= 0;
		snakeSpeed		= 0.3f;
		snakeDead		= false;
		snakePos		= new Vector2(
			Mathf.FloorToInt(map.GetLength(0) / 2),
			Mathf.FloorToInt(map.GetLength(1) / 2)
		);
		for(int y = (int)snakePos.y; y < Mathf.FloorToInt(snakePos.y + 5); ++y) {
			snakeTailMap[(int)snakePos.x, y]	= snakeTailLength + (((int)snakePos.y) - y);
		}

		generateFood();
	}

	public void Update() {
		if (snakeDead)	{
			return;
		}

		//Snake Movement
		snakeSpeedTimer	+= Time.unscaledDeltaTime;
		if (snakeSpeedTimer > snakeSpeed) {
			snakeMove();
		}
	}

	public bool InputUpdate(SHGUIinput key) {
		if (snakeDead)	return false;

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
		
		//Food Debug
		//if (key == SHGUIinput.enter)
		//	generateFood();
		//--

		if (key == SHGUIinput.esc)
			return false;
		//--

		return true;
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
			--foodAmount;
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
		if (foodAmount > 3 && Random.value > 0.1f) {
			--foodAmount;
			return;
		}

		int	x	= Mathf.FloorToInt(Random.value * map.GetLength(0));
		int	y	= Mathf.FloorToInt(Random.value * map.GetLength(1));

		if (snakeTailMap[x, y] != 0 || map[x, y] != 0) {
			generateFood();
			foodAmount	+= 1;
		} else {
			snakeSpeed	= 0.95f * snakeSpeed;
			map[x, y]	= 2;
			if (Random.value < 0.15f) {
				generateFood();
				foodAmount	+= 1;
				if (Random.value < 0.0001f && foodAmount <= 2) {
					generateFood();
					generateFood();
					generateFood();
					foodAmount	+= 3;
				}
			}
		}
		
		return;
	}
}
