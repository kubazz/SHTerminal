using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SupportMap
{
	public	int		posX;
	public	int		posY;
	public	bool	visited;
	public	int		father;
	public	int		liveTime;
	
	public	SupportMap(int pX, int pY)
	{
		posX = pX;
		posY = pY;
		visited = false;
		father = 4;
		liveTime = 0;
	}
}

public class RogueMonster {

	public	RogueMonsters	rogMonsters;

	//wygląd i zachowanie
	public	int				typeAI = 3;
	public	int				ID = 0;
	public	char 			symbol = 'M';
	public	char			color = 'z';

	public	string			name = " ";

	public	int				posX = 25;
	public	int				posY = 15;

	public	int				targetPosX = 0;
	public	int				targetPosY = 0;

	//specyfikacja
	public	int				health = 0;
	public	int				attack = 0;
	public	int				vision = 10;

	//modyfikatory ruchu
	public	List<int>		path = new List<int>();
	public	int				moveState = 0;
	public	int				moveFaze = 0;
	public	bool			canMove = true;

	public RogueMonster(int id, RogueMonsters rogMons)
	{
		rogMonsters = rogMons;
		ID = id;
		rogMonsters.SetParameters(ID, this);
	}

	public RogueMonster(int id, RogueMonsters rogMons, int pX, int pY)
	{
		rogMonsters = rogMons;
		ID = id;
		posX = pX;
		posY = pY;
		rogMonsters.SetParameters(ID, this);
	}

	public void Move()
	{
		if(typeAI == 0) //stanie w miejscu
		{
			return;
		}
		else if(typeAI == 1) //ruszanie się po ścieżce
		{

			if(path[moveState] == 0)
			{
				if(rogMonsters.map[posX, posY - 1] == 0)
				{
					for(int i = 0; i < rogMonsters.monsterList.Count; ++i)
					{
						//sprawdzanie czy nie ma innego potwora na drodze
						if(rogMonsters.monsterList[i].posX == posX && rogMonsters.monsterList[i].posY == posY - 1)
						{
							canMove = false;
							break;
						}
						else canMove = true;
					}
					if(canMove) --posY;
				}
				++moveState;
				if(moveState == path.Count) moveState = 0;
			}
			else if(path[moveState] == 1)
			{
				if(rogMonsters.map[posX + 1, posY] == 0)
				{
					for(int i = 0; i < rogMonsters.monsterList.Count; ++i)
					{
						//sprawdzanie czy nie ma innego potwora na drodze
						if(rogMonsters.monsterList[i].posX == posX + 1 && rogMonsters.monsterList[i].posY == posY)
						{
							canMove = false;
							break;
						}
						else canMove = true;
					}
					if(canMove) ++posX;
				}
				++moveState;
				if(moveState == path.Count) moveState = 0;
			}
			else if(path[moveState] == 2)
			{
				if(rogMonsters.map[posX, posY + 1] == 0)
				{
					for(int i = 0; i < rogMonsters.monsterList.Count; ++i)
					{
						//sprawdzanie czy nie ma innego potwora na drodze
						if(rogMonsters.monsterList[i].posX == posX && rogMonsters.monsterList[i].posY + 1 == posY)
						{
							canMove = false;
							break;
						}
						else canMove = true;
					}
					if(canMove) ++posY;
				}
				++moveState;
				if(moveState == path.Count) moveState = 0;
			}
			else if(path[moveState] == 3)
			{
				if(rogMonsters.map[posX - 1, posY] == 0)
				{
					for(int i = 0; i < rogMonsters.monsterList.Count; ++i)
					{
						//sprawdzanie czy nie ma innego potwora na drodze
						if(rogMonsters.monsterList[i].posX - 1 == posX && rogMonsters.monsterList[i].posY == posY)
						{
							canMove = false;
							break;
						}
						else canMove = true;
					}
					if(canMove) --posX;
				}
				++moveState;
				if(moveState == path.Count) moveState = 0;
			}
			else
			{
				++moveState;
				if(moveState == path.Count) moveState = 0;
			}

			return;
		}
		else if(typeAI == 2 || typeAI == 3) //podchodzenie || podchodzenie z czekaniem
		{
			if(typeAI == 3)
			{
				if(moveState == 0) moveState = 1;
				else
				{
					moveState = 0;
					return;
				}
			}
			//sprawdzenie czy gracz stoi zaraz obok
			if(rogMonsters.player.position[0] == posX && rogMonsters.player.position[1] == posY - 1) //nad
			{
				HitPlayer();
				return;
			}
			if(rogMonsters.player.position[0] == posX + 1 && rogMonsters.player.position[1] == posY) //po prawo
			{
				HitPlayer();
				return;
			}
			if(rogMonsters.player.position[0] == posX && rogMonsters.player.position[1] == posY + 1) //pod
			{
				HitPlayer();
				return;
			}
			if(rogMonsters.player.position[0] == posX - 1 && rogMonsters.player.position[1] == posY) //po lewo
			{
				HitPlayer();
				return;
			}
			//sprawdzenie czy stór widzi(jest wystarczająco blisko) postać
			if(Mathf.Sqrt(Mathf.Pow(rogMonsters.player.position[0] - posX, 2f) + Mathf.Pow(rogMonsters.player.position[1] - posY, 2f)) <= vision)
			{
				SupportMap[,] visitedMap = new SupportMap[rogMonsters.map.GetLength(0), rogMonsters.map.GetLength(1)];

				for(int i = 0; i < visitedMap.GetLength(0); ++i) //wypełnienie pozycji na pomocniczej mapie
				{
					for(int j = 0; j < visitedMap.GetLength(1); ++j)
					{
						visitedMap[i, j] = new SupportMap(i, j);
					}
				}

				//dużucenie pierwszego elementu do listy
				Queue<SupportMap> que = new Queue<SupportMap>();
				que.Enqueue(visitedMap[posX, posY]);
				que.Peek().visited = true;
				que.Peek().father = 4;
				que.Peek().liveTime = 0;

				for(;;) //wyznaczanie trasy do gracza
				{
					if(que.Peek().posX != 0) //lewa krawędź
					{
						if(rogMonsters.map[que.Peek().posX - 1, que.Peek().posY] == 0) //sprawdzenie czy jest to zykłe podłoże a nie ściana czy coś
						{
							if(CheckFild(3))
							{
								if(visitedMap[que.Peek().posX - 1, que.Peek().posY].visited == false) //sprawdzenie czy nie został odwiedzony
								{
									visitedMap[que.Peek().posX - 1, que.Peek().posY].visited = true; //zaznaczanie, że się odwiedziło daną pozycje
									visitedMap[que.Peek().posX - 1, que.Peek().posY].liveTime = que.Peek().liveTime + 1; //zmiana czasu życia
									visitedMap[que.Peek().posX - 1, que.Peek().posY].father = 1;
									que.Enqueue(visitedMap[que.Peek().posX - 1, que.Peek().posY]);
								}
							}
						}
					}
					if(que.Peek().posX < rogMonsters.map.GetLength(0)) //prawa krawędź
					{
						if(rogMonsters.map[que.Peek().posX + 1, que.Peek().posY] == 0) //sprawdzenie czy jest to zykłe podłoże a nie ściana czy coś
						{
							if(CheckFild(1))
							{
								if(visitedMap[que.Peek().posX + 1, que.Peek().posY].visited == false) //sprawdzenie czy nie został odwiedzony
								{
									visitedMap[que.Peek().posX + 1, que.Peek().posY].visited = true; //zaznaczanie, że się odwiedziło daną pozycje
									visitedMap[que.Peek().posX + 1, que.Peek().posY].liveTime = que.Peek().liveTime + 1; //zmiana czasu życia
									visitedMap[que.Peek().posX + 1, que.Peek().posY].father = 3;
									que.Enqueue(visitedMap[que.Peek().posX + 1, que.Peek().posY]);
								}
							}
						}
					}
					if(que.Peek().posY != 0) //górna krawędź
					{
						if(rogMonsters.map[que.Peek().posX, que.Peek().posY - 1] == 0) //sprawdzenie czy jest to zykłe podłoże a nie ściana czy coś
						{
							if(CheckFild(0))
							{
								if(visitedMap[que.Peek().posX, que.Peek().posY - 1].visited == false) //sprawdzenie czy nie został odwiedzony
								{
									visitedMap[que.Peek().posX, que.Peek().posY - 1].visited = true; //zaznaczanie, że się odwiedziło daną pozycje
									visitedMap[que.Peek().posX, que.Peek().posY - 1].liveTime = que.Peek().liveTime + 1; //zmiana czasu życia
									visitedMap[que.Peek().posX, que.Peek().posY - 1].father = 2;
									que.Enqueue(visitedMap[que.Peek().posX, que.Peek().posY - 1]);
								}
							}
						}
					}
					if(que.Peek().posY < rogMonsters.map.GetLength(1)) //dolna krawędź
					{
						if(rogMonsters.map[que.Peek().posX, que.Peek().posY + 1] == 0) //sprawdzenie czy jest to zykłe podłoże a nie ściana czy coś
						{
							if(CheckFild(2))
							{
								if(visitedMap[que.Peek().posX, que.Peek().posY + 1].visited == false) //sprawdzenie czy nie został odwiedzony
								{
									visitedMap[que.Peek().posX, que.Peek().posY + 1].visited = true; //zaznaczanie, że się odwiedziło daną pozycje
									visitedMap[que.Peek().posX, que.Peek().posY + 1].liveTime = que.Peek().liveTime + 1; //zmiana czasu życia
									visitedMap[que.Peek().posX, que.Peek().posY + 1].father = 0;
									que.Enqueue(visitedMap[que.Peek().posX, que.Peek().posY + 1]);
								}
							}
						}
					}

					if(que.Peek().posX == rogMonsters.player.position[0] && que.Peek().posY == rogMonsters.player.position[1]) //gdy znajdziemy gracza
					{
						targetPosX = rogMonsters.player.position[0];
						targetPosY = rogMonsters.player.position[1];
					}

					if(que.Peek().liveTime > vision) break; //jeżeli doszlo do granicy widoku i nie znaleziono gracza przerywamy

					que.Dequeue();
					if(que.Count == 0) //jeśli kolejka jest pusta wychodzimy z pętli
					{
						break;
					}
				}
				int lastFather = visitedMap[targetPosX, targetPosY].father;
				//wyznaczanie ścieżki
				for(int i = 0;; ++i)
				{
					if(visitedMap[targetPosX, targetPosY].father == 0)
					{
						targetPosY -= 1;
						lastFather = 0;
					}
					else if(visitedMap[targetPosX, targetPosY].father == 1)
					{
						targetPosX += 1;
						lastFather = 1;
					}
					else if(visitedMap[targetPosX, targetPosY].father == 2)
					{
						targetPosY += 1;
						lastFather = 2;
					}
					else if(visitedMap[targetPosX, targetPosY].father == 3)
					{
						targetPosX -= 1;
						lastFather = 3;
					}
					else if(visitedMap[targetPosX, targetPosY].father == 4)
					{
						if(lastFather == 0) posY += 1;
						else if(lastFather == 1) posX -= 1;
						else if(lastFather == 2) posY -= 1;
						else if(lastFather == 3) posX += 1;
						break;
					}
				}

			}
			return;
		}
		else if(typeAI == 4) //lustrzane sterowanie
		{
			return;
		}
	}

	public void HitPlayer() //walnięcie w gracza
	{

	}

	public bool CheckFild(int tempDirect) //sprawdzenie czy na polu na które chcemy sie ruszyć nie stoi jakiś inny potworek
	{

		return true;
	}
}
