using UnityEngine;
using System.Collections;

public class APPOnionMan : SHGUIappbase {
	//Private
		//"Grafika"
	private string[]	mapOrg	= new string[22] {
"╔════════════════════════════════════════════════════════════╗",
"║°°°°°°°°°°°°█°°°°°°°█°°°°°°°°°██°°°°°°°°°█°███°█°█°███°█°°°█║",
"║°████████°█°█°█████°█°██°████°██°████°██°█°°°█°█°°°°°°°█°█°█║",
"║°°°██°°°█°█°█°█°°°█°█°██°████°██°████°██°█°█°█°█°█████°█°█°█║",
"║°█°°°°█°°°█°°°°°█°°°█°°°°°°°°°°°°°°°°°°°°█°█°°°█°°°°°°°°°█°█║",
"║°██°███████°█°█°█°█°█°██°█°████████°█°██°█°███°█°█°██°█°°█°█║",
"║°°°°°°█°°°█°█°█°█°█°█°°°°█°°°°██°°°°█°°°°█°°°°°°°█°██°██°█°°║",
"║█████°█°█°█°█°█°█°█°████°████°██°████°███°°█°█████°°°°██°██°║",
"║█°°°█°█°█°°°°°°°█°°°°°°█°█°°°°°°°°°°█°█°°°°█°█°°°°°██°█°°°°°║",
"║█°█°█°█°█°█°██°°█°█°████°█°███░░███°█°████°█°█°█°███°°███°█°║",
"║°°█°°°█°°°█°°°█°°°█°°°°°°°°█      █°°°°°°°°°°°°█°°°°°████°█°║",
"║█°█°█°█°█°█°█°°°███°████°█°████████°█°████°█°█°█°███°°███°█°║",
"║█°°°█°█°█°°°█°█°°°°°°°°█°█°°°° °°°°°█°█°°°°█°█°°°°°██°█°°°°°║",
"║█████°█°█°█°█°█°█°█°████°█°████████°█°████°█°█████°°°°██°██°║",
"║°°°°°°█°°°█°█°█°█°█°█°°°°°°°°°██°°°°°°°°°█°°°°°°°█°██°██°█°°║",
"║°██°███████°█°█°█°█°█°██°████°██°████°██°█°███°█°█°██°█°°█°█║",
"║°█°°°°█°°°█°█°°°°°█°█°°█°°°°°°°°°°°°°°█°°█°█°°°█°°°°°°°°°█°█║",
"║°°°██°°°█°█°°°█°█°█°██°██°█°██████°█°██°██°█°█°█°█████°█°█°█║",
"║°████°███°█████°█°█°█°°°°°█°°°██°°°█°°°°°█°°°█°█°°°°°°°█°█°█║",
"║°°█°°°█°█°█°°°█°█°█°█°███████°██°███████°█°███°█°█°███°█°█°█║",
"║█°°°█°°°█°°°█°°°█°°°█°°°°°°°°°°°°°°°°°°°°█°°°°°█°█°███°█°°°█║",
"╚════Points:      ═══════════════════════════════════════════╝"
	};
	private char[,]	map				= new char[62, 22];
	private char[]	man				= new char[2] {'☺', '☻'};
	private char	enemy			= '☼';
	private char	coin			= '°';

		//Pozycje i dane
	private int[]	manPos			= new int[2] {32, 13};
	private int		manDir			= 1;
	/*	Directions:
			0
		3		1
			2
	*/
	private int[]	enemyDir		= new int[6] {
		0, 0, 0, 0, 0, 0
	};
	private int[]	enemiesPos		= new int[12] {
		31, 11,
		32, 11,
		33, 11,
		34, 11,
		35, 11,
		30, 11
	};
	private float[]	enemyWalkDelay	= new float[6] {
		0.25f, 0.4f, 0.1f, 0.3f, 0.1f, 0.3f
	};

		//Liczniki
	private float	manAnimTimer	= 0f;
	private float	manWalkTimer	= 0f;
	private float[]	enemyWalkTimer	= new float[6] {
		0f, 0f, 0f, 0f, 0f, 0f
	};

		//Inne
	private int		points			= 0;
	private bool	lose			= false;

	//Public
	public APPOnionMan()
		:	base("onionman-v1.3.3.7-by-onionmilk")
	{
		restart();
	}

	public override void Update() {
		base.Update();

		manAnimTimer	+= Time.unscaledDeltaTime;
		if (manAnimTimer >= 1.0f)
			manAnimTimer	= 0f;
		//--
		manWalkTimer	+= Time.unscaledDeltaTime;
		if (manWalkTimer >= 0.25f) {
			manMove();
			manWalkTimer	= 0f;
		}

		for(int i = 0; i < enemyWalkTimer.Length; ++i) {
			enemyWalkTimer[i]	+= Time.unscaledDeltaTime;
			if (enemyWalkTimer[i] >= enemyWalkDelay[i]) {
				enemyMoveUpdate(i);
				enemyWalkTimer[i]	= 0f;
			}
		}

		if (lose == true)
			restart();
		//--
	}

	public override void Redraw(int offx, int offy) {
		base.Redraw(offx, offy);

		for(int y = 0; y < mapOrg.Length; ++y)
			for(int x = 0; x < mapOrg[y].Length; ++x)
				if (map[x, y] == coin)
					SHGUI.current.SetPixelBack(map[x, y], 1 + x, 1 + y, 'z');
				else
					SHGUI.current.SetPixelFront(map[x, y], 1 + x, 1 + y, 'w');
				//--
			//--
		//--
		SHGUI.current.SetPixelFront(man[(manAnimTimer < 0.5f? 0: 1)], manPos[0], manPos[1], 'w');

		for(int i = 0; i < enemyDir.Length; ++i)
			SHGUI.current.SetPixelFront(enemy, enemiesPos[(i * 2) + 0], enemiesPos[(i * 2) + 1], 'r');
		//--
		
		string	tempPoints	= points.ToString();
		for(int i = 0; i < tempPoints.Length; ++i)
			SHGUI.current.SetPixelFront(tempPoints[i], 19 - tempPoints.Length + i, 22, 'w');
		//--
	}

	public override void ReactToInputKeyboard(SHGUIinput key) {
		if (key == SHGUIinput.up)
			manDir	= 0;
		//--
		if (key == SHGUIinput.down)
			manDir	= 2;
		//--
		if (key == SHGUIinput.left)
			manDir	= 3;
		//--
		if (key == SHGUIinput.right)
			manDir	= 1;
		//--
		
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView();
		//--
	}

	void restart() {
		for(int y = 0; y < mapOrg.Length; ++y)
			for(int x = 0; x < mapOrg[y].Length; ++x)
				map[x, y]	= mapOrg[y][x];
			//--
		//--
		
		points	= 0;
		manPos			= new int[2] {32, 13};
		manDir			= 1;
		enemiesPos		= new int[12] {
			31, 11,
			32, 11,
			33, 11,
			34, 11,
			35, 11,
			30, 11
		};
		enemyWalkDelay	= new float[6] {
			0.25f, 0.4f, 0.1f, 0.3f, 0.1f, 0.3f
		};
		lose	 		= false;
	}

	bool manMove() {
		int[]	lastPos	= new int[2] {
			manPos[0], manPos[1]
		};

		if (manDir == 0)
			manPos[1]	-= 1;
		//--
		if (manDir == 2)
			manPos[1]	+= 1;
		//--
		if (manDir == 1)
			manPos[0]	+= 1;
		//--
		if (manDir == 3)
			manPos[0]	-= 1;
		//--
		
		if (map[manPos[0] - 1, manPos[1] - 1] == '█'
		||	map[manPos[0] - 1, manPos[1] - 1] == '║'
		||	map[manPos[0] - 1, manPos[1] - 1] == '═'
		||	map[manPos[0] - 1, manPos[1] - 1] == '░'
		) {
			manPos[0]	= lastPos[0];
			manPos[1]	= lastPos[1];
			return false;
		}

		if (map[manPos[0] - 1, manPos[1] - 1] == coin) {
			map[manPos[0] - 1, manPos[1] - 1]	= ' ';
			++points;
		}

		if (manPos[0] < 2)
			manPos[0]	= 2;
		else if (manPos[0] > 61)
			manPos[0]	= 61;
		//--
		if (manPos[1] < 2)
			manPos[1]	= 2;
		else if (manPos[1] > 21)
			manPos[1]	= 21;
		//--
		
		for(int i = 0; i < enemyDir.Length; ++i)
			if (manPos[0] == enemiesPos[(i * 2) + 0] && manPos[1] == enemiesPos[(i * 2) + 1]) {
				lose	= true;
				break;
			}
		//--

		return true;
	}

	bool enemyMoveUpdate(int id) {
		//Zafixowane wyjście z pozycji startowej przeciwników
		if (enemiesPos[(id * 2) + 0] > 31 && enemiesPos[(id * 2) + 0] < 35)
			if (enemiesPos[(id * 2) + 1] > 9 && enemiesPos[(id * 2) + 1] < 12)
				enemyDir[id]		= 0;
			//--
		//--
		if (enemiesPos[(id * 2) + 1] == 11)
			if (enemiesPos[(id * 2) + 0] > 29 && enemiesPos[(id * 2) + 0] < 36)
				if (enemiesPos[(id * 2) + 0] < 32)
					enemyDir[id]	= 1;
				else if (enemiesPos[(id * 2) + 0] > 33)
					enemyDir[id]	= 3;
				//--
			//--
		//--

		int[]	lastPos	= new int[2] {
			enemiesPos[(id * 2) + 0], enemiesPos[(id * 2) + 1]
		};

		if (enemyDir[id] == 0)
			enemiesPos[(id * 2) + 1]	-= 1;
		//--
		if (enemyDir[id] == 2)
			enemiesPos[(id * 2) + 1]	+= 1;
		//--
		if (enemyDir[id] == 1)
			enemiesPos[(id * 2) + 0]	+= 1;
		//--
		if (enemyDir[id] == 3)
			enemiesPos[(id * 2) + 0]	-= 1;
		//--
		
		bool	otherEnemyCollision	= false;
		for(int i = 0; i < enemyDir.Length; ++i) {
			if (i == id)
				continue;
			//--
			if (enemiesPos[(id * 2) + 0] == enemiesPos[(i * 2) + 0]
			&&	enemiesPos[(id * 2) + 1] == enemiesPos[(i * 2) + 1]
			) {
				enemiesPos[(id * 2) + 0]	= lastPos[0];
				enemiesPos[(id * 2) + 1]	= lastPos[1];
				enemyDir[id]				= Mathf.FloorToInt(Random.value * 4f);
				return false;
			}
		}

		if (map[enemiesPos[(id * 2) + 0] - 1, enemiesPos[(id * 2) + 1] - 1] == '█'
		||	map[enemiesPos[(id * 2) + 0] - 1, enemiesPos[(id * 2) + 1] - 1] == '║'
		||	map[enemiesPos[(id * 2) + 0] - 1, enemiesPos[(id * 2) + 1] - 1] == '═'
		) {
			enemiesPos[(id * 2) + 0]	= lastPos[0];
			enemiesPos[(id * 2) + 1]	= lastPos[1];
			enemyDir[id]				= Mathf.FloorToInt(Random.value * 4f);
			return false;
		}

		if (isCrossingOn(enemiesPos[(id * 2) + 0] - 1, enemiesPos[(id * 2) + 1] - 1) == true)
			enemyDir[id]				= Mathf.FloorToInt(Random.value * 4f);
		//--

		if (enemiesPos[(id * 2) + 0] < 2)
			enemiesPos[(id * 2) + 0]	= 2;
		else if (enemiesPos[(id * 2) + 0] > 61)
			enemiesPos[(id * 2) + 0]	= 61;
		//--
		if (enemiesPos[(id * 2) + 1] < 2)
			enemiesPos[(id * 2) + 1]	= 2;
		else if (enemiesPos[(id * 2) + 1] > 21)
			enemiesPos[(id * 2) + 1]	= 21;
		//--
		
		return true;
	}

	bool isCrossingOn(int x, int y) {
		int	ways	= 0;
		if (map[x - 1, y] == ' ' || map[x - 1, y] == coin)
			++ways;
		//--
		if (map[x + 1, y] == ' ' || map[x + 1, y] == coin)
			++ways;
		//--
		if (map[x, y - 1] == ' ' || map[x, y - 1] == coin)
			++ways;
		//--
		if (map[x, y + 1] == ' ' || map[x, y + 1] == coin)
			++ways;
		//--
		
		return ways >= 3;
	}
}
