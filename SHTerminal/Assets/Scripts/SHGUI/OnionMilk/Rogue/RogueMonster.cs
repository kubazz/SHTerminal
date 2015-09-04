using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SupportMap
{
	public	int		posX;
	public	int		posY;
	public	bool	visited;
	public	int		father;
	public	int		indexX;
	public	int		indexY;
	
	public	SupportMap(int pX, int pY, int iX, int iY)
	{
		posX = pX;
		posY = pY;
		indexX = iX;
		indexY = iY;
		visited = false;
		father = 4;
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
	}

	public RogueMonster(int id, RogueMonsters rogMons, int pX, int pY)
	{
		rogMonsters = rogMons;
		ID = id;
		posX = pX;
		posY = pY;
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
			//sprawdzenie czy stór widzi(jest wystarczająco blisko) postać
			if(Mathf.Sqrt(Mathf.Pow(rogMonsters.posPlayer[0] - posX, 2f) + Mathf.Pow(rogMonsters.posPlayer[1] - posY, 2f)) <= vision)
			{
				SupportMap[,] visitedMap = new SupportMap[vision * 2 + 1, vision * 2 + 1];

				//wypełnianie mapy odwiedzin odpowiednimi pozycjami
				for(int i = 0; i < vision * 2 + 1; ++i)
					for(int j = 0; j < vision * 2 + 1; ++j)
						visitedMap[i, j] = new SupportMap(posX - vision + i, posY - vision + j, i, j); 


				Queue<SupportMap> que = new Queue<SupportMap>();
				que.Enqueue(visitedMap[vision + 1, vision + 1]);
				que.Peek().visited = true;
				que.Peek().father = 4;

				for(int m = 0;; ++m)
				{

					if(m > 1000) return;

					if(que.Peek().indexY > 0) //sprawdzenie czy nie wyjdzie za górną część tablicy
					{
						if(rogMonsters.map[que.Peek().posX, que.Peek().posY - 1] == 0) //czy nie ma ściany
						{
							if(visitedMap[que.Peek().indexX, que.Peek().indexY - 1].visited == false)
							{
								que.Enqueue(visitedMap[que.Peek().indexX, que.Peek().indexY - 1]);
								visitedMap[que.Peek().indexX, que.Peek().indexY - 1].visited = true;
								visitedMap[que.Peek().indexX, que.Peek().indexY - 1].father = 2;
							}
						}
					}
					if(que.Peek().indexX < vision * 2) //sprawdzenie czy nie wyjdzie za prawą część tablicy
					{
						if(rogMonsters.map[que.Peek().posX + 1, que.Peek().posY] == 0) //czy nie ma ściany
						{
							if(visitedMap[que.Peek().indexX + 1, que.Peek().indexY].visited == false)
							{
								que.Enqueue(visitedMap[que.Peek().indexX + 1, que.Peek().indexY]);
								visitedMap[que.Peek().indexX + 1, que.Peek().indexY].visited = true;
								visitedMap[que.Peek().indexX + 1, que.Peek().indexY].father = 3;
							}
						}
					}
					if(que.Peek().indexY < vision * 2) //sprawdzenie czy nie wyjdzie za dolną część tablicy
					{
						if(rogMonsters.map[que.Peek().posX, que.Peek().posY + 1] == 0) //czy nie ma ściany
						{
							if(visitedMap[que.Peek().indexX, que.Peek().indexY + 1].visited == false)
							{
								que.Enqueue(visitedMap[que.Peek().indexX, que.Peek().indexY + 1]);
								visitedMap[que.Peek().indexX, que.Peek().indexY + 1].visited = true;
								visitedMap[que.Peek().indexX, que.Peek().indexY + 1].father = 0;
							}
						}
					}
					if(que.Peek().indexX > 0) //sprawdzenie czy nie wyjdzie za lewą część tablicy
					{
						if(rogMonsters.map[que.Peek().posX - 1, que.Peek().posY] == 0) //czy nie ma ściany
						{
							if(visitedMap[que.Peek().indexX - 1, que.Peek().indexY].visited == false)
							{
								que.Enqueue(visitedMap[que.Peek().indexX - 1, que.Peek().indexY]);
								visitedMap[que.Peek().indexX - 1, que.Peek().indexY].visited = true;
								visitedMap[que.Peek().indexX - 1, que.Peek().indexY].father = 1;
							}
						}
					}

					//kiedy znaleziono gracza w zasięgu wzroku
					if(que.Peek().posX == rogMonsters.posPlayer[0] && que.Peek().posY == rogMonsters.posPlayer[1])
					{
						//ustawianie kierunku w który będzie się poruszał potwór
						SupportMap actualStep = que.Peek();
						int lastFather = que.Peek().father;

						for(int k = 0;; ++k)
						{
							if(k > 500) return;

							lastFather = actualStep.father;

							if(actualStep.father == 0) //jeżeli jesteśmy nad graczem
							{
								if(rogMonsters.map[actualStep.posX, actualStep.posY + 1] == 0)
									if(actualStep.indexY + 1 < vision * 2)
										actualStep = visitedMap[actualStep.indexX, actualStep.indexY + 1];
								else return;
							}
							else if(actualStep.father == 1) //jeżeli jesteśmy na lewo od gracza graczem
							{
								if(rogMonsters.map[actualStep.posX - 1, actualStep.posY] == 0)
									if(actualStep.indexX - 1 > 0)
										actualStep = visitedMap[actualStep.indexX - 1, actualStep.indexY];
								else return;
							}
							else if(actualStep.father == 2) //jeżeli jesteśmy pod graczem
							{
								if(rogMonsters.map[actualStep.posX, actualStep.posY - 1] == 0)
									if(actualStep.indexY - 1 > 0)
										actualStep = visitedMap[actualStep.indexX, actualStep.indexY - 1];
								else return;
							}
							else if(actualStep.father == 3) //jeżeli jesteśmy na prawo od gracza graczem
							{
								if(rogMonsters.map[actualStep.posX + 1, actualStep.posY] == 0)
									if(actualStep.indexX + 1 < vision * 2)
										actualStep = visitedMap[actualStep.indexX + 1, actualStep.indexY];
								else return;
							}
							if(actualStep.father == 4)
							{
								if(lastFather == 0) //jeżeli jesteśmy nad graczem
								{
									if(rogMonsters.map[actualStep.posX, actualStep.posY + 1] == 0) ++posY;
								}
								else if(lastFather == 1) //jeżeli jesteśmy na lewo od gracza graczem
								{
									if(rogMonsters.map[actualStep.posX - 1, actualStep.posY] == 0) --posX;
								}
								else if(lastFather == 2) //jeżeli jesteśmy pod graczem
								{
									if(rogMonsters.map[actualStep.posX, actualStep.posY - 1] == 0) --posY;
								}
								else if(lastFather == 3) //jeżeli jesteśmy na prawo od gracza graczem
								{
									if(rogMonsters.map[actualStep.posX + 1, actualStep.posY] == 0) ++posX;
								}
								return;
							}
						}
					}

					que.Dequeue();

					if(que.Count == 0) //nie znaleziono gracza
					{
						return;
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
}
