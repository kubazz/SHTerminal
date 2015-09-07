using UnityEngine;
using System.Collections;

public class APPRogue : SHGUIappbase {
	//Private
		//Mapa
	RogueGenerator	generator		= null;	//Obiekt generatora
	RogueMap		world			= null;	//Mapa

		//Wyświetlanie
	bool			mapView			= false;	//Tryb podglądu mapy
	int[]			displayOffset	= new int[2] {0, 0};	//Offset wyświetlania mapy
	RogueStatusBar	status			= new RogueStatusBar();	//Pasek informacyjny w grze

		//Gracz
	RoguePlayer		player			= null;	//Obiekt gracza

	bool updateLogic	= true;

	//Public
	public APPRogue()
		:	base("roguelike-v23.0.0.23-by-onionmilk")
	{
		generator							= new RogueGenerator();
		RogueMapParameters	mapParameters	= new RogueMapParameters();
		mapParameters.size					= new int[2] {20, 20};
		mapParameters.roomFrequency			= 0.2f;
		mapParameters.roomMaxSize			= 3;
		mapParameters.chestMaxAmount		= 5;
		generator.generate(mapParameters);
		world								= generator.getMap();

		player			= new RoguePlayer(world.map);
		player.position	= generator.getSpawn();
	}

	public override void Update() {
		base.Update();

		if (updateLogic) {
			updateLogic	= false;
		}
	}

	public override void Redraw(int offx, int offy) {
		base.Redraw(offx, offy);
		
		if (mapView == false)
			displayOffset	= new int[2] {
				player.position[0] - 30,
				player.position[1] - 10
			};
		//--
		
		for(int y = 0; y < 22; ++y) {
			for(int x = 0; x < 62; ++x) {
				//Wyświetlanie eteru (obszar poza mapą)
				if ((displayOffset[0] + x) >= world.map.GetLength(0) || (displayOffset[0] + x) < 0
				||	(displayOffset[1] + y) >= world.map.GetLength(1) || (displayOffset[1] + y) < 0
				) {
					SHGUI.current.SetPixelBack('░', 1 + x, 1 + y, 'z');
					continue;
				}
				//Wyświetlanie właściwych bloków
				switch(world.map[displayOffset[0] + x, displayOffset[1] + y]) {
					case(0): {
						SHGUI.current.SetPixelBack(' ', 1 + x, 1 + y, 'z');
						break;
					}
					case(1): {
						SHGUI.current.SetPixelBack('█', 1 + x, 1 + y, 'z');
						break;
					}
					case(2): {
						SHGUI.current.SetPixelBack('░', 1 + x, 1 + y, 'z');
						SHGUI.current.SetPixelFront('▌', 1 + x, 1 + y, 'z');
						break;
					}
					case(3): {
						SHGUI.current.SetPixelBack('░', 1 + x, 1 + y, 'z');
						SHGUI.current.SetPixelFront('▐', 1 + x, 1 + y, 'z');
						break;
					}
					case(4): {
						SHGUI.current.SetPixelBack('░', 1 + x, 1 + y, 'z');
						SHGUI.current.SetPixelFront('▄', 1 + x, 1 + y, 'z');
						break;
					}
					case(5): {
						SHGUI.current.SetPixelBack('░', 1 + x, 1 + y, 'z');
						SHGUI.current.SetPixelFront('▀', 1 + x, 1 + y, 'z');
						break;
					}
					case(6): {
						SHGUI.current.SetPixelBack('#', 1 + x, 1 + y, 'z');
						break;
					}

					default: {
						SHGUI.current.SetPixelBack('░', 1 + x, 1 + y, 'z');
						break;
					}
				}
			}
		}

		//Wyświetlanie gracza
		if (mapView == false)
			SHGUI.current.SetPixelFront('@', 31, 11, 'w');
		//--
		

		//Pasek statusu
		char[,]	statusMap	= status.screenMap();
		for(int y = 0; y < statusMap.GetLength(1); ++y)
			for(int x = 0; x < statusMap.GetLength(0); ++x) {
				SHGUI.current.SetPixelBack('█', 1 + x, 23 + y - statusMap.GetLength(1), '0');
				SHGUI.current.SetPixelFront(statusMap[x, y], 1 + x, 23 + y - statusMap.GetLength(1), 'w');
			}
		//--
	}

	public override void ReactToInputKeyboard(SHGUIinput key) {
		if (mapView) {	//Tryb podglądu mapy
			if (key == SHGUIinput.up)
				displayOffset[1]	-= 1;
			if (key == SHGUIinput.down)
				displayOffset[1]	+= 1;
			if (key == SHGUIinput.left)
				displayOffset[0]	-= 1;
			if (key == SHGUIinput.right)
				displayOffset[0]	+= 1;
			//--
		} else if (	key == SHGUIinput.up
				||	key == SHGUIinput.down
				||	key == SHGUIinput.left
				||	key == SHGUIinput.right
		) {	//Czy się próbowano ruszyć
			bool 	moved	= false;
			if (key == SHGUIinput.up)
				moved	= player.moveBy(0, -1);
			//--
			if (key == SHGUIinput.down)
				moved	= player.moveBy(0, +1);
			//--
			if (key == SHGUIinput.left)
				moved	= player.moveBy(-1, 0);
			//--
			if (key == SHGUIinput.right)
				moved	= player.moveBy(+1, 0);
			//--
			if (moved) {
				updateLogic	= true;
			} else {
				status.setMessage("Cannot move there!");
			}
		}
		
		
		if (key == SHGUIinput.enter) {
			RogueMapParameters	mapParameters	= new RogueMapParameters();
			mapParameters.size				= new int[2] {20, 20};
			mapParameters.roomFrequency		= 0.3f;
			mapParameters.roomMaxSize		= 5;
			mapParameters.chestMaxAmount	= 5;
			generator.generate(mapParameters);
			world			= generator.getMap();

			player			= new RoguePlayer(world.map);
			player.position	= generator.getSpawn();
		}
		

		if (key == SHGUIinput.esc)
			SHGUI.current.PopView();
		//--
	}
}
