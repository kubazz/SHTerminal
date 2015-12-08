using UnityEngine;
using System.Collections;

public class APPSnake : SHGUIappbase {
	//Private
	APPSnakeGame	gameHandle	= null;
	APPSnakeMenu	menuHandle	= null;
	
	bool			menuPhase	= true;
	bool			gamePhase	= false;
	//Public
	public APPSnake()
		:	base("snake-vS.S.S.BOOM-by-onionmilk")
	{
		gameHandle	= new APPSnakeGame();
		menuHandle	= new APPSnakeMenu();
	} 

	public override void Update() {
		base.Update();
		if (gamePhase)
			gameHandle.Update();
		else if (menuPhase)
			menuHandle.Update();
		//--
	}

	public override void Redraw(int offx, int offy) {
		base.Redraw(offx, offy);
		//SHGUI.current.SetPixelFront(char character, int posX, int posY, char color);

		if (menuPhase) {
			//Menu
			int	offset	= 0;

			//Logo
			for(int y = 0; y < menuHandle.menuLogo.Length; ++y)
				for(int x = 0; x < menuHandle.menuLogo[y].Length; ++x) {
					SHGUI.current.SetPixelBack(
						menuHandle.menuLogo[y][x],
						1 + x,
						3 + y,
						'w'
					);	
				}
			//--

			//Snake
			offset	= 14;
			for(int x = 0; x < menuHandle.map.GetLength(0); ++x) {
				SHGUI.current.SetPixelBack('=', 1 + x, 13, 'w');
			}
			for(int y = 0; y < (menuHandle.map.GetLength(1) / 2); ++y)
				for(int x = 0; x < menuHandle.map.GetLength(0); ++x) {
					//Snake
					if (menuHandle.map[x, (y * 2)] > 0 && menuHandle.map[x, (y * 2) + 1] > 0) {
						SHGUI.current.SetPixelBack(gameHandle.snakeGfx[2], 1 + x, offset + y, gameHandle.snakeColor);
					} else if (menuHandle.map[x, (y * 2)] > 0 && menuHandle.map[x, (y * 2) + 1] <= 0) {
						SHGUI.current.SetPixelBack(gameHandle.snakeGfx[0], 1 + x, offset + y, gameHandle.snakeColor);
					} else if (menuHandle.map[x, (y * 2)] <= 0 && menuHandle.map[x, (y * 2) + 1] > 0) {
						SHGUI.current.SetPixelBack(gameHandle.snakeGfx[1], 1 + x, offset + y, gameHandle.snakeColor);
					}
				}
			//--
			for(int x = 0; x < menuHandle.map.GetLength(0); ++x) {
				SHGUI.current.SetPixelBack('=', 1 + x, offset + (menuHandle.map.GetLength(1) / 2), 'w');
			}

			//Menu bar
			for(int x = 0; x < menuHandle.menuOptionsBar.Length; ++x) {
				SHGUI.current.SetPixelFront(
					(((Time.unscaledTime * 1000)%1200 > 400)? menuHandle.menuOptionsBar[x]: ' '),
					18 + x,
					11,
					'w'
				);
			}
		} else if (gamePhase) {
			if (gameHandle.snakeDead)	return;

			//Map
			for(int y = 0; y < (gameHandle.map.GetLength(1) / 2); ++y)
				for(int x = 0; x < gameHandle.map.GetLength(0); ++x) {
					//Ściana
					if (gameHandle.map[x, (y * 2)] == 1 && gameHandle.map[x, (y * 2) + 1] == 1) {
						SHGUI.current.SetPixelBack(gameHandle.objList[3], 1 + x, 1 + y, 'w');
					} else if (gameHandle.map[x, (y * 2)] == 1 && gameHandle.map[x, (y * 2) + 1] != 1) {
						SHGUI.current.SetPixelBack(gameHandle.objList[1], 1 + x, 1 + y, 'w');
					} else if (gameHandle.map[x, (y * 2)] != 1 && gameHandle.map[x, (y * 2) + 1] == 1) {
						SHGUI.current.SetPixelBack(gameHandle.objList[2], 1 + x, 1 + y, 'w');
					}
					
					//Jedzenie
					if (gameHandle.map[x, (y * 2)] == 2 && gameHandle.map[x, (y * 2) + 1] == 2) {
						SHGUI.current.SetPixelBack(gameHandle.objList[6], 1 + x, 1 + y, 'r');
					} else if (gameHandle.map[x, (y * 2)] == 2 && gameHandle.map[x, (y * 2) + 1] != 2) {
						SHGUI.current.SetPixelBack(gameHandle.objList[4], 1 + x, 1 + y, 'r');
					} else if (gameHandle.map[x, (y * 2)] != 2 && gameHandle.map[x, (y * 2) + 1] == 2) {
						SHGUI.current.SetPixelBack(gameHandle.objList[5], 1 + x, 1 + y, 'r');
					}
				}
			//--
			//Snake
			for(int y = 0; y < 22; ++y)
				for(int x = 0; x < gameHandle.map.GetLength(0); ++x)
					if (gameHandle.snakeTailMap[x, (y * 2)] > 0 && gameHandle.snakeTailMap[x, (y * 2) + 1] > 0) {
						SHGUI.current.SetPixelFront(gameHandle.snakeGfx[2], 1 + x, 1 + y, gameHandle.snakeColor);
					} else if (gameHandle.snakeTailMap[x, (y * 2)] > 0 && gameHandle.snakeTailMap[x, (y * 2) + 1] <= 0) {
						SHGUI.current.SetPixelFront(gameHandle.snakeGfx[0], 1 + x, 1 + y, gameHandle.snakeColor);
					} else if (gameHandle.snakeTailMap[x, (y * 2)] <= 0 && gameHandle.snakeTailMap[x, (y * 2) + 1] > 0) {
						SHGUI.current.SetPixelFront(gameHandle.snakeGfx[1], 1 + x, 1 + y, gameHandle.snakeColor);
					}
				//--
			//--
		}
	}

	public override void ReactToInputKeyboard(SHGUIinput key) {
		if (gamePhase) {
			if (gameHandle.InputUpdate(key) == false) {
				gamePhase	= false;
				menuPhase	= true;
			}
		} else if (menuPhase) {
			switch (menuHandle.InputUpdate(key)) {
				case(0): {
					SHGUI.current.PopView();
					break;
				}
				case(2): {
					gamePhase	= true;
					menuPhase	= false;
					gameHandle.restart();
					break;
				}
			}
		}
	}

	
}
