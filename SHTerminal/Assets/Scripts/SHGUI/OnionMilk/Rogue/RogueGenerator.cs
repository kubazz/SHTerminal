using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RogueGenerator {
	//Private
	private	int[,]		miniMap		= null;
	/*	ID elementów Mini Mapy
			0:	Nic
			1:	Pokój
			2:	
			3:	Droga
			4:	Pokój z NPC'em
			5:	Boss Room
	*/
	public int[,]		miniRoomMap	= null;
	private List<int[]>	roomDoor	= null;

	private	int[,]		map			= null;
	/*	ID elemtów Mapy
			<= -1000:	Itemy w krzyni
			-1:			Wolna przestrzeń (której nie da się odwiedzić)
			0:			Wolna przestrzeń
			1:			Kamienna Ściana (blok: █)
			2:			Kamienna Ściana (blok: ▌)
			3:			Kamienna Ściana (blok: ▐)
			4:			Kamienna Ściana (blok: ▄)
			5:			Kamienna Ściana (blok: ▀)
			6:			Drzwi (blok: #)
			>= 1000:	Itemy
	*/

	private float	frequency	= 0.1f;
	private int		maxRoomSize	= 3;

	//Public
	public RogueGenerator(float newFrequency, int newMaxRoomSize) {
		frequency	=	newFrequency;
		maxRoomSize	=	1 + newMaxRoomSize;
	}

	public void generate(int sizeX, int sizeY) {
		miniMap		= new int[sizeX, sizeY];
		miniRoomMap	= new int[sizeX, sizeY];
		roomDoor	= new List<int[]>();
		map			= new int[sizeX * 5, sizeY * 5];
		clear(sizeX, sizeY);

		do {
			for(int y = 1; y < sizeY; ++y)
				for(int x = 1; x < sizeX; ++x)
					if (Random.value < frequency) {
						roomDoor.Add(new int[2] {x, y});
						putRoom(roomDoor.Count, x, y, Mathf.FloorToInt(Random.value * maxRoomSize));
					}
				//--
			//--
			if (mapIsEmpty() == false)
				break;
			//--
		} while(true);
		
		connectRooms();
		translate();
	}

	void clear(int sizeX, int sizeY, int fromX = 0, int fromY = 0) {
		for(int x = fromX; x < sizeX; ++x)
			for(int y = fromY; y < sizeY; ++y)
				miniMap[x, y]	= 0;
			//--
		//--
		
		for(int x = (fromX * 5); x < (sizeX * 5); ++x)
			for(int y = (fromY * 5); y < (sizeY * 5); ++y)
				map[x, y]	= -1;
			//--
		//--
	}

	bool mapIsEmpty() {
		int	filler	= 0;
		for(int y = 0; y < miniMap.GetLength(1); ++y)
			for(int x = 0; x < miniMap.GetLength(0); ++x) {
				if (miniMap[x, y] != 0)
					filler	+= 1;
				//--
				if (filler > 5)
					return false;
				//--
			}
		//--
		return true;
	}

	void putRoom(int roomID, int x, int y, int size) {
		if (x < 0 || y < 0 || x >= miniMap.GetLength(0) || y >= miniMap.GetLength(1) || size <= 0)
			return;
		//--

		miniMap[x, y]		= 1;
		miniRoomMap[x, y]	= roomID;

		--size;
		if (Random.value < 0.5f) {	//< 0.5 w osi X
			putRoom(roomID, x - 1, y, size);
			putRoom(roomID, x + 1, y, size);
		} else {	//> 0.5 w osi Y
			putRoom(roomID, x, y - 1, size);
			putRoom(roomID, x, y + 1, size);
		}
	}

	void connectRooms() {
		//Łączenie drzwi pokojów
		for(int i = 0; i < roomDoor.Count; ++i) {
			int mod		= 0;
			int next	= (i + 1)%roomDoor.Count;
			int	x 		= 0;
			int y		= 0;

			if ((roomDoor[next])[0] < (roomDoor[i])[0])
				mod		= -1;
			else if ((roomDoor[next])[0] > (roomDoor[i])[0])
				mod		= 1;
			//--
			x	= (roomDoor[i])[0];
			for(; x != (roomDoor[next])[0]; x += mod)
				if (miniMap[x, (roomDoor[i])[1]] == 0)	//Zabezpieczenie przed "tłumami" tuneli pod rząd w osi X
					if (((roomDoor[i])[1] - 1) >= 0 && miniMap[x, (roomDoor[i])[1] - 1] == 3) {
						miniMap[x, (roomDoor[i])[1] - 1]	= 3;
					} else if (((roomDoor[i])[1] + 1) < miniMap.GetLength(1) &&	miniMap[x, (roomDoor[i])[1] + 1] == 3) {
						miniMap[x, (roomDoor[i])[1] + 1]	= 3;
					} else {
						miniMap[x, (roomDoor[i])[1]]	= 3;
					}
				//--
			//--
			
			if ((roomDoor[next])[1] < (roomDoor[i])[1])
				mod		= -1;
			else if ((roomDoor[next])[1] > (roomDoor[i])[1])
				mod		= 1;
			//--
			y = (roomDoor[i])[1];
			for(; y != (roomDoor[next])[1]; y += mod)
				if (miniMap[x, y] == 0)
					miniMap[x, y]	= 3;
				//--
			//--
		}
	}

	void translate() {
		for(int y = 0; y < map.GetLength(1); ++y)
			for(int x = 0; x < map.GetLength(0); ++x)
				map[x, y]	= -1;
			//--
		//--

		//Stawianie pokoi i dróg
		for(int y = 0; y < miniMap.GetLength(1); ++y)
			for(int x = 0; x < miniMap.GetLength(0); ++x)
				switch(miniMap[x, y]) {
					case(1): {
						for(int _y = 0; _y < 5; ++_y)
							for(int _x = 0; _x < 5; ++_x)
								if (_y == 0 || _y == 4 || _x == 0 || _x == 4)
									map[(x * 5) + _x, (y * 5) + _y]	= 1;
								else
									map[(x * 5) + _x, (y * 5) + _y]	= 0;
								//--
							//--
						//--
						break;
					}
					case(3): {
						int	around	= 0;
						/*	Sąsiedzi w około:
								1
							8		2
								4
						*/
						if (x > 0)
							if (miniMap[x - 1, y] > 0)
								around	+= 8;
							//--
						//--
						if ((x + 1) < miniMap.GetLength(0))
							if (miniMap[x + 1, y] > 0)
								around	+= 2;
							//--
						//--
						if (y > 0)
							if (miniMap[x, y - 1] > 0)
								around	+= 1;
							//--
						//--
						if ((y + 1) < miniMap.GetLength(1))
							if (miniMap[x, y + 1] > 0)
								around	+= 4;
							//--
						//--
						
						//Środek drogi
						for(int _y = 1; _y < 4; ++_y)
							for(int _x = 1; _x < 4; ++_x)
								map[(x * 5) + _x, (y * 5) + _y]	= 1;
							//--
						//--
						map[(x * 5) + 2, (y * 5) + 2]	= 0;

						//Odłam w górę
						int	what = 0;
						if ((around&1) == 1) {
							for(int _x = 1; _x < 4; ++_x) {
								if (_x == 2)
									what	= 0;
								else if (_x == 1)
									what	= 3;
								else
									what	= 2;
								//--
								map[(x * 5) + _x, (y * 5) + 0]	= what;
								map[(x * 5) + _x, (y * 5) + 1]	= what;
							}
						}
						if ((around&4) == 4) {
							for(int _x = 1; _x < 4; ++_x) {
								if (_x == 2)
									what	= 0;
								else if (_x == 1)
									what	= 3;
								else
									what	= 2;
								//--
								map[(x * 5) + _x, (y * 5) + 3]	= what;
								map[(x * 5) + _x, (y * 5) + 4]	= what;
							}
						}
						if ((around&2) == 2) {
							for(int _y = 1; _y < 4; ++_y) {
								if (_y == 2)
									what	= 0;
								else if (_y == 1)
									what	= 4;
								else
									what	= 5;
								//--
								map[(x * 5) + 3, (y * 5) + _y]	= what;
								map[(x * 5) + 4, (y * 5) + _y]	= what;
							}
						}
						if ((around&8) == 8) {
							for(int _y = 1; _y < 4; ++_y) {
								if (_y == 2)
									what	= 0;
								else if (_y == 1)
									what	= 4;
								else
									what	= 5;
								//--
								map[(x * 5) + 0, (y * 5) + _y]	= what;
								map[(x * 5) + 1, (y * 5) + _y]	= what;
							}
						}

						//Środek drogi (poprawka)
						map[(x * 5) + 1, (y * 5) + 1]	= 1;
						map[(x * 5) + 1, (y * 5) + 3]	= 1;
						map[(x * 5) + 3, (y * 5) + 3]	= 1;
						map[(x * 5) + 3, (y * 5) + 1]	= 1;
						map[(x * 5) + 2, (y * 5) + 2]	= 0;

						break;
					}
					default: {
						//Nic
						break;
					}
				}
			//--
		//--

		//Pozbywanie się ścian pomiędzy sąsiadującymi pokojami
		for(int y = 0; y < miniMap.GetLength(1); ++y)
			for(int x = 0; x < miniMap.GetLength(0); ++x)
				if (miniMap[x, y] == 1) {
					if ((x + 1) < miniMap.GetLength(0))
						if (miniMap[x + 1, y] == 1)
							for(int _x = 4; _x < 6; ++_x)
								for(int _y = 1; _y < 4; ++_y)
									map[(x * 5) + _x, (y * 5) + _y]	= 0;
								//--
							//--
						//--
					//--
					if ((y + 1) < miniMap.GetLength(1))
						if (miniMap[x, y + 1] == 1)
							for(int _y = 4; _y < 6; ++_y)
								for(int _x = 1; _x < 4; ++_x)
									map[(x * 5) + _x, (y * 5) + _y]	= 0;
								//--
							//--
						//--
					//--
				}
			//--
		//--
		for(int y = 0; y < (map.GetLength(1) - 1); ++y)
			for(int x = 0; x < (map.GetLength(0) - 1); ++x) {
				if (map[x, y] == 1
				&&	map[x + 1, y] == 1
				&&	map[x + 1, y + 1] == 1
				&&	map[x, y + 1] == 1
				&&	(x%5 != 0 && x%5 != 4)
				&&	(y%5 != 0 && y%5 != 4)
				) {
					map[x, y]			= 0;
					map[x + 1, y]		= 0;
					map[x + 1, y + 1]	= 0;
					map[x, y + 1]		= 0;
				}
			}
		//--
		//Pozbywanie się ścian między korytażami a pokojami
		for(int y = 0; y < miniMap.GetLength(1); ++y)
			for(int x = 0; x < miniMap.GetLength(0); ++x) {
				if ((x - 1) > 0)
					if (miniMap[x, y] == 1 && miniMap[x - 1, y] == 3)
						map[(x * 5), (y * 5) + 2]	= 6;
					//--
				//--
				if ((x + 1) < miniMap.GetLength(0))
					if (miniMap[x, y] == 1 && miniMap[x + 1, y] == 3)
						map[(x * 5) + 4, (y * 5) + 2]	= 6;
					//--
				//--
				if ((y - 1) > 0)
					if (miniMap[x, y] == 1 && miniMap[x, y - 1] == 3)
						map[(x * 5) + 2, (y * 5)]	= 6;
					//--
				//--
				if ((y + 1) < miniMap.GetLength(1))
					if (miniMap[x, y] == 1 && miniMap[x, y + 1] == 3)
						map[(x * 5) + 2, (y * 5) + 4]	= 6;
					//--
				//--
			}
		//--
	}

	public int[] getSpawn() {
		return new int[2] {
			((roomDoor[0])[0] * 5) + 2,
			((roomDoor[0])[1] * 5) + 2
		};
	}
	public int[,] getMap() {
		return map;
	}
}
