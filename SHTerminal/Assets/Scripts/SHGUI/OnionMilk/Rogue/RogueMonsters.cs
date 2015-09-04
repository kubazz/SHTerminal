using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RogueMonsters {

	//mapa
	public	int[,]						map = null;

	//potwory
	public	List<RogueMonster>			monsterList = new List<RogueMonster>();

	public	int[]						posPlayer = new int[2];

	public RogueMonsters(int[,] mapka, int[] posPl)
	{
		monsterList.Clear();
		map = mapka;

		//monsterList.Add(new RogueMonster(10, this, 16, 9));
		//monsterList.Add(new RogueMonster(14, this, 18, 8));
		monsterList.Add(new RogueMonster(34, this, 19, 8));

		SetParameters();

		posPlayer = posPlayer;
	}

	public void UpdatePosPlayer(int pX, int pY)
	{
		posPlayer[0] = pX;
		posPlayer[1] = pY;
	}

	public void SetParameters()
	{
		for(int ID = 0; ID < monsterList.Count; ++ID)
		{
			//1-4
			if(monsterList[ID].ID == 0) //Goblin
			{
				monsterList[ID].typeAI = 1;
				monsterList[ID].symbol = 'g';
				monsterList[ID].name = "Goblin";
				monsterList[ID].health = 1;
				monsterList[ID].attack = 1;
				AddPath(ID, new int[4] {0, 4, 2, 4});

			}
			else if(monsterList[ID].ID == 1) //Goblin Scavanger
			{
				monsterList[ID].typeAI = 1;
				monsterList[ID].symbol = 'g';
				monsterList[ID].color = 'r';
				monsterList[ID].name = "Goblin Scavenger";
				monsterList[ID].health = 2;
				monsterList[ID].attack = 1;
				AddPath(ID, new int[4] {3, 4, 1, 4});
			}
			else if(monsterList[ID].ID == 2) //Dwarf
			{
				monsterList[ID].typeAI = 1;
				monsterList[ID].symbol = 'd';
				monsterList[ID].name = "Dwarf";
				monsterList[ID].health = 3;
				monsterList[ID].attack = 2;
				AddPath(ID, new int[4] {0, 4, 2, 4});
			}
			else if(monsterList[ID].ID == 3) //Worm
			{
				monsterList[ID].typeAI = 0;
				monsterList[ID].symbol = '˙';
				monsterList[ID].name = "Worm";
				monsterList[ID].health = 1;
				monsterList[ID].attack = 0;
			}
			else if(monsterList[ID].ID == 4) //Cockroach
			{
				monsterList[ID].typeAI = 1;
				monsterList[ID].symbol = ',';
				monsterList[ID].name = "Cockroach";
				AddPath(ID, new int[4] {1, 4, 3, 4});
				monsterList[ID].health = 1;
				monsterList[ID].attack = 1;
			}
			else if(monsterList[ID].ID == 5) //Spider
			{
				monsterList[ID].typeAI = 1;
				monsterList[ID].symbol = '.';
				monsterList[ID].name = "Spider";
				monsterList[ID].health = 1;
				monsterList[ID].attack = 1;
				AddPath(ID, new int[16] {0, 4, 0, 4, 1, 4, 1, 4, 2, 4, 2, 4, 3, 4, 3, 4});
			}
			//=================================3-7==========================================
			else if(monsterList[ID].ID == 6) //Small Troll
			{
				monsterList[ID].typeAI = 1;
				monsterList[ID].symbol = 't';
				monsterList[ID].name = "Small Troll";
				monsterList[ID].health = 9;
				monsterList[ID].attack = 4;
				AddPath(ID, new int[16] {0, 4, 0, 4, 0, 4, 0, 4, 2, 4, 2, 4, 2, 4, 2, 4});

			}
			else if(monsterList[ID].ID == 7) //Orc
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'o';
				monsterList[ID].name = "Orc";
				monsterList[ID].health = 7;
				monsterList[ID].attack = 3;
			}
			else if(monsterList[ID].ID == 8) //Goblin Slinger
			{
				monsterList[ID].typeAI = 1;
				monsterList[ID].symbol = 'G';
				monsterList[ID].color = 'r';
				monsterList[ID].name = "Goblin Slinger";
				monsterList[ID].health = 4;
				monsterList[ID].attack = 4;
				AddPath(ID, new int[8] {1, 4, 2, 4, 0, 4, 3, 4});
			}
			else if(monsterList[ID].ID == 9) //Skeleton
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 's';
				monsterList[ID].name = "Skeleton";
				monsterList[ID].health = 6;
				monsterList[ID].attack = 3;
			}
			else if(monsterList[ID].ID == 10) //Zombie
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'z';
				monsterList[ID].vision = 7;
				monsterList[ID].name = "Zombie";
				monsterList[ID].health = 8;
				monsterList[ID].attack = 2;
			}
			else if(monsterList[ID].ID == 11) //Vampire
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'v';
				monsterList[ID].vision = 11;
				monsterList[ID].name = "Vampire";
				monsterList[ID].health = 7;
				monsterList[ID].attack = 3;
			}
			else if(monsterList[ID].ID == 12) //Dwarf Miner
			{
				monsterList[ID].typeAI = 1;
				monsterList[ID].symbol = 'd';
				monsterList[ID].color = 'r';
				monsterList[ID].name = "Dwarf Miner";
				monsterList[ID].health = 9;
				monsterList[ID].attack = 4;
				AddPath(ID, new int[12] {1, 4, 0, 4, 1, 4, 3, 4, 2, 4, 3, 4});
			}
			else if(monsterList[ID].ID == 13) //Poison Spider
			{
				monsterList[ID].typeAI = 1;
				monsterList[ID].symbol = '*';
				monsterList[ID].name = "Poison Spider";
				monsterList[ID].health = 2;
				monsterList[ID].attack = 7;
				AddPath(ID, new int[16] {4, 1, 4, 1, 4, 1, 4, 2, 4, 3, 4, 3, 4, 3, 4, 0});
			}
			else if(monsterList[ID].ID == 14) //Scarab
			{
				monsterList[ID].typeAI = 1;
				monsterList[ID].symbol = '\'';
				monsterList[ID].name = "Scarab";
				monsterList[ID].health = 5;
				monsterList[ID].attack = 3;
				AddPath(ID, new int[16] {3, 4, 3, 4, 3, 4, 2, 4, 1, 4, 1, 4, 1, 4, 0, 4});
			}
			else if(monsterList[ID].ID == 15) //Scorpion
			{
				monsterList[ID].typeAI = 1;
				monsterList[ID].symbol = '|';
				monsterList[ID].name = "Scorpion";
				monsterList[ID].health = 5;
				monsterList[ID].attack = 5;
				AddPath(ID, new int[8] {1, 1, 1, 0, 3, 3, 3, 2});
			}
			else if(monsterList[ID].ID == 16) //Lizard
			{
				monsterList[ID].typeAI = 1;
				monsterList[ID].symbol = 'l';
				monsterList[ID].name = "Lizard";
				monsterList[ID].health = 6;
				monsterList[ID].attack = 3;
				AddPath(ID, new int[6] {0, 0, 1, 2, 2, 3});
			}
			//=================================6-11==========================================
			else if(monsterList[ID].ID == 17) //Troll
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 't';
				monsterList[ID].color = 'r';
				monsterList[ID].vision = 10;
				monsterList[ID].name = "Troll";
				monsterList[ID].health = 20;
				monsterList[ID].attack = 9;
			}
			else if(monsterList[ID].ID == 18) //Orc Spearman
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'o';
				monsterList[ID].color = 'r';
				monsterList[ID].vision = 11;
				monsterList[ID].name = "Orc Spearman";
				monsterList[ID].health = 18;
				monsterList[ID].attack = 8;
			}
			else if(monsterList[ID].ID == 19) //Goblin Assasin
			{
				monsterList[ID].typeAI = 2;
				monsterList[ID].symbol = 'G';
				monsterList[ID].vision = 20;
				monsterList[ID].name = "Goblin Assasin";
				monsterList[ID].health = 11;
				monsterList[ID].attack = 11;
			}
			else if(monsterList[ID].ID == 20) //Ghost
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = '^';
				monsterList[ID].vision = 12;
				monsterList[ID].name = "Ghost";
				monsterList[ID].health = 15;
				monsterList[ID].attack = 7;
			}
			else if(monsterList[ID].ID == 21) //Dwarf Solider
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'D';
				monsterList[ID].vision = 15;
				monsterList[ID].name = "Dwarf Solider";
				monsterList[ID].health = 20;
				monsterList[ID].attack = 7;
			}
			else if(monsterList[ID].ID == 22) //Orc Warrior
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'O';
				monsterList[ID].vision = 15;
				monsterList[ID].name = "Orc Warrior";
				monsterList[ID].health = 20;
				monsterList[ID].attack = 9;
			}
			else if(monsterList[ID].ID == 23) //Skeleton Archer
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 's';
				monsterList[ID].color = 'r';
				monsterList[ID].vision = 16;
				monsterList[ID].name = "Skeleton Archer";
				monsterList[ID].health = 15;
				monsterList[ID].attack = 6;
			}
			else if(monsterList[ID].ID == 24) //Skeleton Warrior
			{
				monsterList[ID].typeAI = 1;
				monsterList[ID].symbol = 'S';
				monsterList[ID].name = "Skeleton Warrior";
				monsterList[ID].health = 15;
				monsterList[ID].attack = 11;
				AddPath(ID, new int[8] {3, 3, 3, 3, 1, 1, 1, 1});
			}
			else if(monsterList[ID].ID == 25) //Skeleton Pirate
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'p';
				monsterList[ID].vision = 16;
				monsterList[ID].name = "Skeleton Pirate";
				monsterList[ID].health = 19;
				monsterList[ID].attack = 11;
			}
			else if(monsterList[ID].ID == 26) //Pirate Ghost
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'P';
				monsterList[ID].vision = 20;
				monsterList[ID].name = "Pirate Ghost";
				monsterList[ID].health = 20;
				monsterList[ID].attack = 7;
			}
			else if(monsterList[ID].ID == 27) //Ghoul
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = '<';
				monsterList[ID].vision = 18;
				monsterList[ID].name = "Ghoul";
				monsterList[ID].health = 25;
				monsterList[ID].attack = 4;
			}
			else if(monsterList[ID].ID == 28) //Giant Spider
			{
				monsterList[ID].typeAI = 2;
				monsterList[ID].symbol = '↔';
				monsterList[ID].vision = 18;
				monsterList[ID].name = "Giant Spider";
				monsterList[ID].health = 15;
				monsterList[ID].attack = 7;
			}
			else if(monsterList[ID].ID == 29) //Elf
			{
				monsterList[ID].typeAI = 2;
				monsterList[ID].symbol = 'e';
				monsterList[ID].vision = 15;
				monsterList[ID].name = "Elf";
				monsterList[ID].health = 14;
				monsterList[ID].attack = 8;
			}
			else if(monsterList[ID].ID == 30) //Lizard Sentitel
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'l';
				monsterList[ID].color = 'r';
				monsterList[ID].vision = 18;
				monsterList[ID].name = "Lizard Sentitel";
				monsterList[ID].health = 20;
				monsterList[ID].attack = 11;
			}
			//=================================10-16==========================================
			else if(monsterList[ID].ID == 31) //Swamp Troll
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'T';
				monsterList[ID].vision = 18;
				monsterList[ID].name = "Swamp Troll";
				monsterList[ID].health = 40;
				monsterList[ID].attack = 16;
			}
			else if(monsterList[ID].ID == 32) //Orc Rider
			{
				monsterList[ID].typeAI = 2;
				monsterList[ID].symbol = 'O';
				monsterList[ID].color = 'r';
				monsterList[ID].vision = 25;
				monsterList[ID].name = "Orc Rider";
				monsterList[ID].health = 30;
				monsterList[ID].attack = 12;
			}
			else if(monsterList[ID].ID == 33) //Mummy
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'm';
				monsterList[ID].vision = 15;
				monsterList[ID].name = "Mummy";
				monsterList[ID].health = 35;
				monsterList[ID].attack = 9;
			}
			else if(monsterList[ID].ID == 34) //Dwarf Guard
			{
				monsterList[ID].typeAI = 2;
				monsterList[ID].symbol = 'Đ';
				monsterList[ID].vision = 20;
				monsterList[ID].name = "Dwarf Guard";
				monsterList[ID].health = 32;
				monsterList[ID].attack = 20;
			}
			else if(monsterList[ID].ID == 35) //Minotaur
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'M';
				monsterList[ID].vision = 22;
				monsterList[ID].name = "Minotaur";
				monsterList[ID].health = 30;
				monsterList[ID].attack = 15;
			}
			else if(monsterList[ID].ID == 36) //Ancient Scarab
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'A';
				monsterList[ID].vision = 26;
				monsterList[ID].name = "Ancient Scarab";
				monsterList[ID].health = 36;
				monsterList[ID].attack = 8;
			}
			else if(monsterList[ID].ID == 37) //Cristal Spider
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'C';
				monsterList[ID].vision = 21;
				monsterList[ID].name = "Cristal Spider";
				monsterList[ID].health = 40;
				monsterList[ID].attack = 5;
			}
			else if(monsterList[ID].ID == 38) //Elf Scout
			{
				monsterList[ID].typeAI = 2;
				monsterList[ID].symbol = 'E';
				monsterList[ID].vision = 35;
				monsterList[ID].name = "Elf Scout";
				monsterList[ID].health = 25;
				monsterList[ID].attack = 10;
			}
			else if(monsterList[ID].ID == 39) //Lizard Legionnaire
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'Ĺ';
				monsterList[ID].vision = 23;
				monsterList[ID].name = "Lizard Legionnaire";
				monsterList[ID].health = 35;
				monsterList[ID].attack = 13;
			}
			else if(monsterList[ID].ID == 40) //Centipide
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'Č';
				monsterList[ID].vision = 20;
				monsterList[ID].name = "Centipide";
				monsterList[ID].health = 45;
				monsterList[ID].attack = 20;
			}
			else if(monsterList[ID].ID == 41) //Dragon Hatchling
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'ď';
				monsterList[ID].vision = 20;
				monsterList[ID].name = "Dragon Hatchling";
				monsterList[ID].health = 40;
				monsterList[ID].attack = 14;
			}
			else if(monsterList[ID].ID == 42) //Water Elemental
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'W';
				monsterList[ID].vision = 25;
				monsterList[ID].name = "Water Elemental";
				monsterList[ID].health = 40;
				monsterList[ID].attack = 5;
			}
			//=================================15-21==========================================
			else if(monsterList[ID].ID == 43) //Troll Guard
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'W';
				monsterList[ID].vision = 30;
				monsterList[ID].name = "Troll Guard";
				monsterList[ID].health = 90;
				monsterList[ID].attack = 40;
			}
			else if(monsterList[ID].ID == 44) //Orc Szaman
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'Ô';
				monsterList[ID].vision = 30;
				monsterList[ID].name = "Orc Szaman";
				monsterList[ID].health = 50;
				monsterList[ID].attack = 60;
			}
			else if(monsterList[ID].ID == 45) //Skeleton Mage
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'š';
				monsterList[ID].vision = 30;
				monsterList[ID].name = "Skeleton Mage";
				monsterList[ID].health = 45;
				monsterList[ID].attack = 55;
			}
			else if(monsterList[ID].ID == 46) //Crypt Shambler
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'ç';
				monsterList[ID].vision = 30;
				monsterList[ID].name = "Crypt Shambler";
				monsterList[ID].health = 78;
				monsterList[ID].attack = 34;
			}
			else if(monsterList[ID].ID == 47) //Dwarf Geomancer
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'Ď';
				monsterList[ID].vision = 30;
				monsterList[ID].name = "Dwarf Geomancer";
				monsterList[ID].health = 37;
				monsterList[ID].attack = 46;
			}
			else if(monsterList[ID].ID == 48) //Minotaur Archer
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'Ą';
				monsterList[ID].vision = 27;
				monsterList[ID].name = "Minotaur Archer";
				monsterList[ID].health = 63;
				monsterList[ID].attack = 29;
			}
			else if(monsterList[ID].ID == 49) //Insektoid
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'I';
				monsterList[ID].vision = 30;
				monsterList[ID].name = "Insektoid";
				monsterList[ID].health = 100;
				monsterList[ID].attack = 30;
			}
			else if(monsterList[ID].ID == 50) //Elf Assasin
			{
				monsterList[ID].typeAI = 2;
				monsterList[ID].symbol = 'Ę';
				monsterList[ID].vision = 40;
				monsterList[ID].name = "Elf Assasin";
				monsterList[ID].health = 35;
				monsterList[ID].attack = 70;
			}
			else if(monsterList[ID].ID == 51) //Lizard Warrior
			{
				monsterList[ID].typeAI = 3;
				monsterList[ID].symbol = 'ľ';
				monsterList[ID].vision = 30;
				monsterList[ID].name = "Lizard Warrior";
				monsterList[ID].health = 60;
				monsterList[ID].attack = 29;
			}
			else if(monsterList[ID].ID == 52) //Frost Dragon Hatchling
			{
				monsterList[ID].typeAI = 2;
				monsterList[ID].symbol = 'f';
				monsterList[ID].vision = 30;
				monsterList[ID].name = "Frost Dragon Hatchling";
				monsterList[ID].health = 81;
				monsterList[ID].attack = 39;
			}
			else if(monsterList[ID].ID == 53) //Nature Elemental
			{
				monsterList[ID].typeAI = 2;
				monsterList[ID].symbol = 'N';
				monsterList[ID].vision = 34;
				monsterList[ID].name = "Nature Elemental";
				monsterList[ID].health = 80;
				monsterList[ID].attack = 40;
			}
			//=================================20-26==========================================
			//=================================25-35==========================================
			//=================================34-46==========================================
			//=================================45-61==========================================
			//=================================60-70==========================================
		}
	}


	public void AddPath(int monsterID, int[] path) {
		for(int i = 0; i < path.Length; ++i) {
			monsterList[monsterID].path.Add(path[i]);
		}
	}

}	
