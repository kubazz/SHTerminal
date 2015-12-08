using UnityEngine;
using System.Collections;

public class APPSnakeMenu {
	//Public
	
	public string	menuOptionsBar		= "[ PRESS ANYTHING TO START ]";
	public int		currentOption		= 0;
	public int		optionsOffset		= 0;
	public float	optionsOffsetTarget	= 0;
	public float	optionsAnimTimer	= 0;

public string[]		menuLogo = new string[5] {
	"        █████ ██████ ██████ ██    █ █████ █  █ █████            ",
	"        █     █      █      █ █   █ █   █ █ █  █                ",
	"        █████ ██████ ██████ █  █  █ █   █ ██   ████             ",
	"            █      █      █ █   █ █ █████ █ █  █                ",
	"        █████ ██████ ██████ █    ██ █   █ █  █ █████            "
};

	//Animacja węża
	public int[,]	map				= new int[62, 16];
	public int		snakePosX		= 31;
	public int		snakePosY		= 5;
	public int		foodPosX		= 0;
	public int		foodPosY		= 0;
	public int		snakeLength		= 10;
	public float	snakeMoveTimer	= 0f;
	public float	snakeDirTimer	= 0f;
	public float	snakeGrowTimer	= 0f;

	public APPSnakeMenu() {
		for(int y = 0; y < map.GetLength(1); ++y)
			for(int x = 0; x < map.GetLength(0); ++x)
				map[x, y]	= 0;
			//--
		//--

		map[snakePosX, snakePosY]	= snakeLength;
		generateFood();
	}

	public void Update() {
		optionsAnimTimer	+= Time.unscaledDeltaTime;
		if (optionsOffsetTarget != 0
		&&	optionsAnimTimer > 0.0012f
		) {
			if (optionsOffsetTarget < 0) {
				++optionsOffsetTarget;
				optionsOffset	-= 1;
			} else if (optionsOffsetTarget > 0) {
				--optionsOffsetTarget;
				optionsOffset	+= 1;
			}
			optionsOffset	%= menuOptionsBar.Length;
			
			optionsAnimTimer	= 0;
		}

		snakeMoveTimer	+= Time.unscaledDeltaTime;
		snakeDirTimer	+= Time.unscaledDeltaTime;
		snakeGrowTimer	+= Time.unscaledDeltaTime;
		if (snakeMoveTimer >= 0.1f) {
			if (snakeDirTimer >= 0.5f) {
				snakePosY		= snakePosY + (Mathf.FloorToInt(Random.value * 3) - 1);
				snakeDirTimer	= 0;
			} else {
				snakePosX += 1;
				snakePosX %= map.GetLength(0);
			}

			snakePosY	%= map.GetLength(1);
			if (snakePosY < 0) {
				snakePosY	= map.GetLength(1) - 1;
			}
			map[snakePosX, snakePosY]	= snakeLength;

			for(int y = 0; y < map.GetLength(1); ++y)
				for(int x = 0; x < map.GetLength(0); ++x)
					if (map[x, y] > 0)
						--map[x, y];
					//--
				//--
			//--
			
			if (snakeGrowTimer > 1f) {
				++snakeLength;
				if (snakeLength > 62)
					snakeLength	= -62;
				//--
			}

			snakeMoveTimer	= 0f;
		}
	}

	public int InputUpdate(SHGUIinput key) {
		if (optionsOffsetTarget == 0) {
			if (key == SHGUIinput.right || key == SHGUIinput.down) {
				optionsOffsetTarget	= 20;
				currentOption		+= 1;
			}
			if (key == SHGUIinput.left || key == SHGUIinput.up) {
				optionsOffsetTarget	= -20;
				currentOption		-= 1;
			}
			if (currentOption < 0)
				currentOption	= 3 + currentOption;
			//--
			currentOption	%= 3;
		}

		if (key == SHGUIinput.enter) {
			return 2;
		}

		if (key == SHGUIinput.esc)
			return 0;
		//--
		
		return 1;
	}

	void generateFood() {
		int	x	= Mathf.FloorToInt(Random.value * map.GetLength(0));
		int	y	= Mathf.FloorToInt(Random.value * map.GetLength(1));

		if (map[x, y] != 0)
			generateFood();
		else {
			map[x, y]	= -1;
			foodPosX	= x;
			foodPosY	= y;
		}
		
		return;
	}
}
