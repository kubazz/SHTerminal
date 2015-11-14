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
		'8'
	};

	public char[]	snakeGfx = {
		'▀', '▄', '▒'
	};
	public char	snakeColor		= 'r';
	
		//Mapa
	//62x22
	public int[,]	map				= new int[62, 44];

		//Snake
	public Vector2	snakePos		= Vector2.zero;
	public int[,]	snakeTailMap	= new int[62, 44];
	public int		snakeTailLength	= 5;
	public int		snakeDirecton	= 0; /*
			0
		3		1
			2
	*/
	public float	snakeSpeedTimer	= 0;
	public float	snakeSpeed		= 0.3f;
	public bool		snakeDead		= false;

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
		
		snakePos	= new Vector2(31, 10);
		for(int y = (int)snakePos.y; y < Mathf.FloorToInt(snakePos.y + 5); ++y) {
			snakeTailMap[(int)snakePos.x, y]	= snakeTailLength + (((int)snakePos.y) - y);
		}

		generateFood();
	}

	public void Update() {
		if (snakeDead)	return;

		//Snake Movement
		snakeSpeedTimer	+= Time.unscaledDeltaTime;
		if (snakeSpeedTimer > snakeSpeed) {
			snakeMove();
		}
	}

	public bool InputUpdate(SHGUIinput key) {
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
		int	x	= Mathf.FloorToInt(Random.value * (map.GetLength(0) - 4)) + 2;
		int	y	= Mathf.FloorToInt(Random.value * (map.GetLength(1) - 4)) + 2;

		if (snakeTailMap[x, y] != 0 || map[x, y] != 0)
			generateFood();
		else {
			snakeSpeed	= 0.95f * snakeSpeed;
			map[x, y]	= 2;
		}
		
		return;
	}
}
