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
		monsterList.Add(new RogueMonster(7, this, 18, 8));
		monsterList.Add(new RogueMonster(29, this, 20, 7));

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
			//1 - 2
			if(monsterList[ID].ID == 0) //Goblin
			{
				monsterList[ID].typeAI	= 1;
				monsterList[ID].symbol	= 'g';
				monsterList[ID].name	= "Goblin";
				monsterList[ID].health	= 23;
				monsterList[ID].attack	= 10;
				AddPath(ID, new int[4] {0, 4, 2, 4});
				
			}
			else if(monsterList[ID].ID == 1) //Worm
			{
				monsterList[ID].typeAI	= 0;
				monsterList[ID].symbol	= '.';
				monsterList[ID].name	= "Worm";
				monsterList[ID].health	= 15;
				monsterList[ID].attack	= 0;
			}
			else if(monsterList[ID].ID == 2) //Cockroach
			{
				monsterList[ID].typeAI	= 1;
				monsterList[ID].symbol	= ',';
				monsterList[ID].name	= "Cockroach";
				monsterList[ID].health	= 10;
				monsterList[ID].attack	= 5;
				AddPath(ID, new int[4] {1, 4, 3, 4});
			}
			//3 - 4
			else if(monsterList[ID].ID == 3) //Dwarf
			{
				monsterList[ID].typeAI = 1;
				monsterList[ID].symbol = 'd';
				monsterList[ID].name = "Dwarf";
				monsterList[ID].health = 135;
				monsterList[ID].attack = 42;
				AddPath(ID, new int[4] {0, 4, 2, 4});
			}
			else if(monsterList[ID].ID == 4) //Goblin Scavanger
			{
				monsterList[ID].typeAI	= 1;
				monsterList[ID].symbol	= 'G';
				monsterList[ID].name	= "Goblin Scavenger";
				monsterList[ID].health	= 90;
				monsterList[ID].attack	= 50;
				AddPath(ID, new int[4] {3, 4, 1, 4});
			}		
			else if(monsterList[ID].ID == 5) //Spider
			{
				monsterList[ID].typeAI	= 1;
				monsterList[ID].symbol	= '¤';
				monsterList[ID].name	= "Spider";
				monsterList[ID].health	= 78;
				monsterList[ID].attack	= 45;
				AddPath(ID, new int[16] {0, 4, 0, 4, 1, 4, 1, 4, 2, 4, 2, 4, 3, 4, 3, 4});
			}
			//5 - 6
			else if(monsterList[ID].ID == 6) //Goblin Slinger
			{
				monsterList[ID].typeAI	= 1;
				monsterList[ID].symbol	= 'ŕ';
				monsterList[ID].name	= "Goblin Slinger";
				monsterList[ID].health	= 317;
				monsterList[ID].attack	= 140;
				AddPath(ID, new int[8] {1, 4, 2, 4, 0, 4, 3, 4});
			}
			else if(monsterList[ID].ID == 7) //Skeleton
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 's';
				monsterList[ID].name	= "Skeleton";
				monsterList[ID].health	= 500;
				monsterList[ID].attack	= 270;
				monsterList[ID].vision	= 7;
			}
			else if(monsterList[ID].ID == 8) //Zombie
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'z';
				monsterList[ID].name	= "Zombie";
				monsterList[ID].health	= 800;
				monsterList[ID].attack	= 45;
				monsterList[ID].vision	= 7;
			}
			else if(monsterList[ID].ID == 9) //Poison Spider
			{
				monsterList[ID].typeAI	= 1;
				monsterList[ID].symbol	= '*';
				monsterList[ID].name	= "Poison Spider";
				monsterList[ID].health	= 300;
				monsterList[ID].attack	= 105;
				AddPath(ID, new int[16] {4, 1, 4, 1, 4, 1, 4, 2, 4, 3, 4, 3, 4, 3, 4, 0});
			}
			//7 - 8
			else if(monsterList[ID].ID == 10) //Vampire
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'v';
				monsterList[ID].name	= "Vampire";
				monsterList[ID].health	= 1590;
				monsterList[ID].attack	= 258;
				monsterList[ID].vision	= 11;
			}
			else if(monsterList[ID].ID == 11) //Dwarf Miner
			{
				monsterList[ID].typeAI	= 1;
				monsterList[ID].symbol	= 'D';
				monsterList[ID].name	= "Dwarf Miner";
				monsterList[ID].health	= 960;
				monsterList[ID].attack	= 298;
				AddPath(ID, new int[12] {1, 4, 0, 4, 1, 4, 3, 4, 2, 4, 3, 4});
			}
			else if(monsterList[ID].ID == 12) //Scarab
			{
				monsterList[ID].typeAI	= 1;
				monsterList[ID].symbol	= 'ˇ';
				monsterList[ID].name	= "Scarab";
				monsterList[ID].health	= 1179;
				monsterList[ID].attack	= 395;
				AddPath(ID, new int[16] {3, 4, 3, 4, 3, 4, 2, 4, 1, 4, 1, 4, 1, 4, 0, 4});
			}
			else if(monsterList[ID].ID == 13) //Scorpion
			{
				monsterList[ID].typeAI	= 1;
				monsterList[ID].symbol	= '|';
				monsterList[ID].name	= "Scorpion";
				monsterList[ID].health	= 629;
				monsterList[ID].attack	= 1091;
				AddPath(ID, new int[8] {1, 1, 1, 0, 3, 3, 3, 2});
			}
			else if(monsterList[ID].ID == 14) //Skeleton Archer
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'S';
				monsterList[ID].name	= "Skeleton Archer";
				monsterList[ID].health	= 2704;
				monsterList[ID].attack	= 1000;
				monsterList[ID].vision	= 16;
			}
			//9 - 10
			else if(monsterList[ID].ID == 15) //Lizard
			{
				monsterList[ID].typeAI	= 1;
				monsterList[ID].symbol	= 'l';
				monsterList[ID].name	= "Lizard";
				monsterList[ID].health	= 3893;
				monsterList[ID].attack	= 1764;
				AddPath(ID, new int[6] {0, 0, 1, 2, 2, 3});
			}
			else if(monsterList[ID].ID == 16) //Small Troll
			{
				monsterList[ID].typeAI	= 1;
				monsterList[ID].symbol	= 't';
				monsterList[ID].name	= "Small Troll";
				monsterList[ID].health	= 4162;
				monsterList[ID].attack	= 1593;
				AddPath(ID, new int[16] {0, 4, 0, 4, 0, 4, 0, 4, 2, 4, 2, 4, 2, 4, 2, 4});
			}
			else if(monsterList[ID].ID == 17) //Orc
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'o';
				monsterList[ID].name	= "Orc";
				monsterList[ID].health	= 3586;
				monsterList[ID].attack	= 1890;
				monsterList[ID].vision	= 15;
			}
			else if(monsterList[ID].ID == 18) //Dwarf Solider
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'đ';
				monsterList[ID].name	= "Dwarf Solider";
				monsterList[ID].health	= 3129;
				monsterList[ID].attack	= 1190;
				monsterList[ID].vision	= 15;
			}
			else if(monsterList[ID].ID == 19) //Skeleton Warrior
			{
				monsterList[ID].typeAI	= 1;
				monsterList[ID].symbol	= 'ś';
				monsterList[ID].name	= "Skeleton Warrior";
				monsterList[ID].health	= 3786;
				monsterList[ID].attack	= 1111;
				AddPath(ID, new int[8] {3, 3, 3, 3, 1, 1, 1, 1});
			}
			//11 - 12
			else if(monsterList[ID].ID == 20) //Troll
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'T';
				monsterList[ID].name	= "Troll";
				monsterList[ID].health	= 8540;
				monsterList[ID].attack	= 2897;
				monsterList[ID].vision	= 12;
			}
			else if(monsterList[ID].ID == 21) //Orc Spearman
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'O';
				monsterList[ID].name	= "Orc Spearman";
				monsterList[ID].health	= 4982;
				monsterList[ID].attack	= 1996;
				monsterList[ID].vision	= 15;
			}
			else if(monsterList[ID].ID == 22) //Goblin Assasin
			{
				monsterList[ID].typeAI	= 2;
				monsterList[ID].symbol	= 'A';
				monsterList[ID].name	= "Goblin Assasin";
				monsterList[ID].health	= 4000;
				monsterList[ID].attack	= 4145;
				monsterList[ID].vision	= 20;
			}
			else if(monsterList[ID].ID == 23) //Ghost
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= '▲';
				monsterList[ID].name	= "Ghost";
				monsterList[ID].health	= 7541;
				monsterList[ID].attack	= 2159;
				monsterList[ID].vision	= 20;
			}
			else if(monsterList[ID].ID == 24) //Ghoul
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'Ů';
				monsterList[ID].name	= "Ghoul";
				monsterList[ID].health	= 6892;
				monsterList[ID].attack	= 2638;
				monsterList[ID].vision	= 18;
			}
			//13 - 14
			else if(monsterList[ID].ID == 25) //Skeleton Pirate
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'Ś';
				monsterList[ID].name	= "Skeleton Pirate";
				monsterList[ID].health	= 12100;
				monsterList[ID].attack	= 4582;
				monsterList[ID].vision	= 16;
			}
			else if(monsterList[ID].ID == 26) //Pirate Ghost
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'p';
				monsterList[ID].name	= "Pirate Ghost";
				monsterList[ID].health	= 14012;
				monsterList[ID].attack	= 3582;
				monsterList[ID].vision	= 20;
			}
			else if(monsterList[ID].ID == 27) //Orc Warrior
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'ő';
				monsterList[ID].name	= "Orc Warrior";
				monsterList[ID].health	= 11931;
				monsterList[ID].attack	= 5780;
				monsterList[ID].vision	= 15;
			}
			//15 -  16
			else if(monsterList[ID].ID == 28) //Giant Spider
			{
				monsterList[ID].typeAI	= 2;
				monsterList[ID].symbol	= 'x';
				monsterList[ID].name	= "Giant Spider";
				monsterList[ID].health	= 15019;
				monsterList[ID].attack	= 8462;
				monsterList[ID].vision	= 18;
			}
			else if(monsterList[ID].ID == 29) //Elf
			{
				monsterList[ID].typeAI	= 2;
				monsterList[ID].symbol	= 'e';
				monsterList[ID].name	= "Elf";
				monsterList[ID].health	= 17671;
				monsterList[ID].attack	= 7340;
				monsterList[ID].vision	= 25;
			}
			else if(monsterList[ID].ID == 30) //Lizard Sentitel
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'L';
				monsterList[ID].name	= "Lizard Sentitel";
				monsterList[ID].health	= 18834;
				monsterList[ID].attack	= 6245;
				monsterList[ID].vision	= 21;
			}
			//17 - 18
			else if(monsterList[ID].ID == 31) //Dwarf Guard
			{
				monsterList[ID].typeAI	= 2;
				monsterList[ID].symbol	= 'ď';
				monsterList[ID].name	= "Dwarf Guard";
				monsterList[ID].health	= 28721;
				monsterList[ID].attack	= 9012;
				monsterList[ID].vision	= 20;
			}
			else if(monsterList[ID].ID == 32) //Orc Rider
			{
				monsterList[ID].typeAI	= 2;
				monsterList[ID].symbol	= 'Ő';
				monsterList[ID].name	= "Orc Rider";
				monsterList[ID].health	= 25012;
				monsterList[ID].attack	= 7284;
				monsterList[ID].vision	= 25;
			}
			else if(monsterList[ID].ID == 33) //Ancient Scarab
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'š';
				monsterList[ID].name	= "Ancient Scarab";
				monsterList[ID].health	= 30102;
				monsterList[ID].attack	= 5233;
				monsterList[ID].vision	= 26;
			}
			else if(monsterList[ID].ID == 34) //Mummy
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'm';
				monsterList[ID].name	= "Mummy";
				monsterList[ID].health	= 29032;
				monsterList[ID].attack	= 6341;
				monsterList[ID].vision	= 15;
			}
			//19 - 20
			else if(monsterList[ID].ID == 35) //Swamp Troll
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'ť';
				monsterList[ID].name	= "Swamp Troll";
				monsterList[ID].health	= 38901;
				monsterList[ID].attack	= 15241;
				monsterList[ID].vision	= 18;
			}
			else if(monsterList[ID].ID == 36) //Lizard Legionnaire
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'ł';
				monsterList[ID].name	= "Lizard Legionnaire";
				monsterList[ID].health	= 34901;
				monsterList[ID].attack	= 14891;
				monsterList[ID].vision	= 23;
			}
			else if(monsterList[ID].ID == 37) //Minotaur
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'n';
				monsterList[ID].name	= "Minotaur";
				monsterList[ID].health	= 40123;
				monsterList[ID].attack	= 14002;
				monsterList[ID].vision	= 22;
			}
			else if(monsterList[ID].ID == 38) //Cristal Spider
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'č';
				monsterList[ID].name	= "Cristal Spider";
				monsterList[ID].health	= 31023;
				monsterList[ID].attack	= 18023;
				monsterList[ID].vision	= 21;
			}
			else if(monsterList[ID].ID == 39) //Elf Scout
			{
				monsterList[ID].typeAI	= 2;
				monsterList[ID].symbol	= 'E';
				monsterList[ID].name	= "Elf Scout";
				monsterList[ID].health	= 32562;
				monsterList[ID].attack	= 15023;
				monsterList[ID].vision	= 35;
			}
			//21 - 22
			else if(monsterList[ID].ID == 40) //Dwarf Geomancer
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'Ď';
				monsterList[ID].name	= "Dwarf Geomancer";
				monsterList[ID].health	= 47013;
				monsterList[ID].attack	= 24134;
				monsterList[ID].vision	= 30;
			}
			else if(monsterList[ID].ID == 41) //Centipide
			{
				monsterList[ID].typeAI	= 2;
				monsterList[ID].symbol	= 'C';
				monsterList[ID].name	= "Centipide";
				monsterList[ID].health	= 57184;
				monsterList[ID].attack	= 19934;
				monsterList[ID].vision	= 21;
			}
			else if(monsterList[ID].ID == 42) //Dragon Hatchling
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'h';
				monsterList[ID].name	= "Dragon Hatchling";
				monsterList[ID].health	= 62314;
				monsterList[ID].attack	= 21512;
				monsterList[ID].vision	= 20;
			}
			else if(monsterList[ID].ID == 43) //Water Elemental
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'w';
				monsterList[ID].name	= "Water Elemental";
				monsterList[ID].health	= 52391;
				monsterList[ID].attack	= 18236;
				monsterList[ID].vision	= 25;
			}
			//22 - 23
			else if(monsterList[ID].ID == 44) //Troll Guard
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'ţ';
				monsterList[ID].name	= "Troll Guard";
				monsterList[ID].health	= 82130;
				monsterList[ID].attack	= 39912;
				monsterList[ID].vision	= 30;
			}
			else if(monsterList[ID].ID == 45) //Crypt Shambler
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'c';
				monsterList[ID].name	= "Crypt Shambler";
				monsterList[ID].health	= 75023;
				monsterList[ID].attack	= 35091;
				monsterList[ID].vision	= 31;
			}
			else if(monsterList[ID].ID == 46) //Minotaur Archer
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'N';
				monsterList[ID].name	= "Minotaur Archer";
				monsterList[ID].health	= 78671;
				monsterList[ID].attack	= 38123;
				monsterList[ID].vision	= 27;
			}
			else if(monsterList[ID].ID == 47) //Lizard Warrior
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'Ł';
				monsterList[ID].name	= "Lizard Warrior";
				monsterList[ID].health	= 77123;
				monsterList[ID].attack	= 36127;
				monsterList[ID].vision	= 30;
			}
			//25 - 26
			else if(monsterList[ID].ID == 48) //Orc Szaman
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'ö';
				monsterList[ID].name	= "Orc Szaman";
				monsterList[ID].health	= 93145;
				monsterList[ID].attack	= 58243;
				monsterList[ID].vision	= 34;
			}
			else if(monsterList[ID].ID == 49) //Skeleton Mage
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'ş';
				monsterList[ID].name	= "Skeleton Mage";
				monsterList[ID].health	= 102451;
				monsterList[ID].attack	= 61345;
				monsterList[ID].vision	= 34;
			}
			else if(monsterList[ID].ID == 50) //Elf Assasin
			{
				monsterList[ID].typeAI	= 2;
				monsterList[ID].symbol	= 'é';
				monsterList[ID].name	= "Elf Assasin";
				monsterList[ID].health	= 123456;
				monsterList[ID].attack	= 64562;
				monsterList[ID].vision	= 40;
			}
			else if(monsterList[ID].ID == 51) //Insektoid
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'I';
				monsterList[ID].name	= "Insektoid";
				monsterList[ID].health	= 135239;
				monsterList[ID].attack	= 49124;
				monsterList[ID].vision	= 31;
			}
			//27 - 28
			else if(monsterList[ID].ID == 52) //Orc Berseker
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'Ö';
				monsterList[ID].name	= "Orc Berseker";
				monsterList[ID].health	= 163414;
				monsterList[ID].attack	= 89142;
				monsterList[ID].vision	= 32;
			}
			else if(monsterList[ID].ID == 53) //Frost Dragon Hatchling
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'f';
				monsterList[ID].name	= "Frost Dragon Hatchling";
				monsterList[ID].health	= 182451;
				monsterList[ID].attack	= 82412;
				monsterList[ID].vision	= 33;
			}
			else if(monsterList[ID].ID == 54) //Nature Elemental
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= '♣';
				monsterList[ID].name	= "Nature Elemental";
				monsterList[ID].health	= 170973;
				monsterList[ID].attack	= 72142;
				monsterList[ID].vision	= 31;
			}
			//29 - 30
			else if(monsterList[ID].ID == 55) //Frost Troll
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'Ţ';
				monsterList[ID].name	= "Frost Troll";
				monsterList[ID].health	= 321445;
				monsterList[ID].attack	= 102931;
				monsterList[ID].vision	= 32;
			}
			else if(monsterList[ID].ID == 56) //Demon Skeleton
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'Ş';
				monsterList[ID].name	= "Demon Skeleton";
				monsterList[ID].health	= 280912;
				monsterList[ID].attack	= 191345;
				monsterList[ID].vision	= 35;
			}
			else if(monsterList[ID].ID == 57) //Banshee
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'b';
				monsterList[ID].name	= "Banshee";
				monsterList[ID].health	= 254300;
				monsterList[ID].attack	= 102930;
				monsterList[ID].vision	= 40;
			}
			else if(monsterList[ID].ID == 58) //Minotaur Guard
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'M';
				monsterList[ID].name	= "Minotaur Guard";
				monsterList[ID].health	= 312501;
				monsterList[ID].attack	= 129510;
				monsterList[ID].vision	= 33;
			}
			//31 - 32
			else if(monsterList[ID].ID == 59) //Crawler
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'Č';
				monsterList[ID].name	= "Crawler";
				monsterList[ID].health	= 531245;
				monsterList[ID].attack	= 214556;
				monsterList[ID].vision	= 32;
			}
			else if(monsterList[ID].ID == 60) //Lizard Templar
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'L';
				monsterList[ID].name	= "Lizard Templar";
				monsterList[ID].health	= 491450;
				monsterList[ID].attack	= 229549;
				monsterList[ID].vision	= 31;
			}
			else if(monsterList[ID].ID == 61) //Red Dragon Hatchling
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'r';
				monsterList[ID].name	= "Red Dragon Hatchling";
				monsterList[ID].health	= 571308;
				monsterList[ID].attack	= 246728;
				monsterList[ID].vision	= 35;
			}
			//33 - 34
			else if(monsterList[ID].ID == 62) //Dragon
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'Đ';
				monsterList[ID].name	= "Dragon";
				monsterList[ID].health	= 813542;
				monsterList[ID].attack	= 372348;
				monsterList[ID].vision	= 32;
			}
			else if(monsterList[ID].ID == 63) //Fire Elemental
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= '♥';
				monsterList[ID].name	= "Fire Elemental";
				monsterList[ID].health	= 801424;
				monsterList[ID].attack	= 311062;
				monsterList[ID].vision	= 33;
			}
			else if(monsterList[ID].ID == 64) //Cyclop
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= '÷';
				monsterList[ID].name	= "Cyclop";
				monsterList[ID].health	= 795281;
				monsterList[ID].attack	= 362369;
				monsterList[ID].vision	= 31;
			}
			else if(monsterList[ID].ID == 65) //Orc Maruder
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'ô';
				monsterList[ID].name	= "Orc Maruder";
				monsterList[ID].health	= 752465;
				monsterList[ID].attack	= 298675;
				monsterList[ID].vision	= 34;
			}
			//35 - 36
			else if(monsterList[ID].ID == 66) //Grave Troll
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'Ť';
				monsterList[ID].name	= "Grave Troll";
				monsterList[ID].health	= 1012093;
				monsterList[ID].attack	= 416723;
				monsterList[ID].vision	= 36;
			}
			else if(monsterList[ID].ID == 67) //Spectre
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= '~';
				monsterList[ID].name	= "Spectre";
				monsterList[ID].health	= 9612421;
				monsterList[ID].attack	= 382601;
				monsterList[ID].vision	= 40;
			}
			else if(monsterList[ID].ID == 68) //Undead Priest
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= ':';
				monsterList[ID].name	= "Undead Priest";
				monsterList[ID].health	= 913560;
				monsterList[ID].attack	= 40953;
				monsterList[ID].vision	= 32;
			}
			//37 - 38
			else if(monsterList[ID].ID == 69) //Minotaur Mage
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'Ň';
				monsterList[ID].name	= "Minotaur Mage";
				monsterList[ID].health	= 1309623;
				monsterList[ID].attack	= 793462;
				monsterList[ID].vision	= 35;
			}
			else if(monsterList[ID].ID == 70) //Giant Mantis
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'X';
				monsterList[ID].name	= "Giant Mantis";
				monsterList[ID].health	= 1709308;
				monsterList[ID].attack	= 582394;
				monsterList[ID].vision	= 38;
			}
			else if(monsterList[ID].ID == 71) //Elf Arcanist
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'É';
				monsterList[ID].name	= "Elf Arcanist";
				monsterList[ID].health	= 1538923;
				monsterList[ID].attack	= 625782;
				monsterList[ID].vision	= 40;
			}
			else if(monsterList[ID].ID == 72) //Lizard Snakecharmer
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= '╚';
				monsterList[ID].name	= "Lizard Snakecharmer";
				monsterList[ID].health	= 1675447;
				monsterList[ID].attack	= 592436;
				monsterList[ID].vision	= 38;
			}
			//39 - 40
			else if(monsterList[ID].ID == 73) //Frost Dragon
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'F';
				monsterList[ID].name	= "Frost Dragon";
				monsterList[ID].health	= 2007259;
				monsterList[ID].attack	= 745270;
				monsterList[ID].vision	= 36;
			}
			else if(monsterList[ID].ID == 74) //Ice Elemental
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= '♦';
				monsterList[ID].name	= "Ice Elemental";
				monsterList[ID].health	= 1825984;
				monsterList[ID].attack	= 612340;
				monsterList[ID].vision	= 35;
			}
			else if(monsterList[ID].ID == 75) //Cyclop Worker
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'Î';
				monsterList[ID].name	= "Cyclop Worker";
				monsterList[ID].health	= 1594138;
				monsterList[ID].attack	= 783856;
				monsterList[ID].vision	= 36;
			}
			//41 - 42
			else if(monsterList[ID].ID == 76) //Troll Warrior
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= '↑';
				monsterList[ID].name	= "Troll Warrior";
				monsterList[ID].health	= 2213173;
				monsterList[ID].attack	= 809358;
				monsterList[ID].vision	= 39;
			}
			else if(monsterList[ID].ID == 77) //Orc Leader
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'Ô';
				monsterList[ID].name	= "Orc Leader";
				monsterList[ID].health	= 2235633;
				monsterList[ID].attack	= 901246;
				monsterList[ID].vision	= 39;
			}
			else if(monsterList[ID].ID == 78) //Lich
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'Ľ';
				monsterList[ID].name	= "Lich";
				monsterList[ID].health	= 2182451;
				monsterList[ID].attack	= 991234;
				monsterList[ID].vision	= 39;
			}
			//43 - 44
			else if(monsterList[ID].ID == 79) //Elder Elf
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'Ę';
				monsterList[ID].name	= "Elder Elf";
				monsterList[ID].health	= 2501447;
				monsterList[ID].attack	= 1023501;
				monsterList[ID].vision	= 49;
			}
			else if(monsterList[ID].ID == 80) //Lizard Dragon Priest
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'ľ';
				monsterList[ID].name	= "Lizard Dragon Priest";
				monsterList[ID].health	= 2498712;
				monsterList[ID].attack	= 1001534;
				monsterList[ID].vision	= 40;
			}
			else if(monsterList[ID].ID == 81) //Red Dragon
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'R';
				monsterList[ID].name	= "Red Dragon";
				monsterList[ID].health	= 2601345;
				monsterList[ID].attack	= 1118730;
				monsterList[ID].vision	= 40;
			}
			else if(monsterList[ID].ID == 82) //Stone Elemental
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= '♠';
				monsterList[ID].name	= "Stone Elemental";
				monsterList[ID].health	= 2889340;
				monsterList[ID].attack	= 8021470;
				monsterList[ID].vision	= 41;
			}
			//45 - 46
			else if(monsterList[ID].ID == 83) //Cyclop Smith
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= '‼';
				monsterList[ID].name	= "Cyclop Smith";
				monsterList[ID].health	= 2814925;
				monsterList[ID].attack	= 1602831;
				monsterList[ID].vision	= 42;
			}
			else if(monsterList[ID].ID == 84) //Orc Warlord
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= '0';
				monsterList[ID].name	= "Orc Warlord";
				monsterList[ID].health	= 2701539;
				monsterList[ID].attack	= 1802931;
				monsterList[ID].vision	= 42;
			}
			else if(monsterList[ID].ID == 85) //Undead Gladiator
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'U';
				monsterList[ID].name	= "Undead Gladiator";
				monsterList[ID].health	= 2579342;
				monsterList[ID].attack	= 1614235;
				monsterList[ID].vision	= 42;
			}
			else if(monsterList[ID].ID == 86) //Lizard High Guard
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'ĺ';
				monsterList[ID].name	= "Lizard High Guard";
				monsterList[ID].health	= 2901804;
				monsterList[ID].attack	= 1312523;
				monsterList[ID].vision	= 45;
			}
			//47 - 48
			else if(monsterList[ID].ID == 87) //Hydra
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'H';
				monsterList[ID].name	= "Hydra";
				monsterList[ID].health	= 3020809;
				monsterList[ID].attack	= 1748230;
				monsterList[ID].vision	= 50;
			}
			else if(monsterList[ID].ID == 88) //Energy Elemental
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= '♀';
				monsterList[ID].name	= "Energy Elemental";
				monsterList[ID].health	= 2989010;
				monsterList[ID].attack	= 1692540;
				monsterList[ID].vision	= 42;
			}
			else if(monsterList[ID].ID == 89) //Cyclop Warrior
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'W';
				monsterList[ID].name	= "Cyclop Warrior";
				monsterList[ID].health	= 2171730;
				monsterList[ID].attack	= 2002380;
				monsterList[ID].vision	= 41;
			}
			//49 - 51
			else if(monsterList[ID].ID == 90) //Troll Champion
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= '▼';
				monsterList[ID].name	= "Troll Champion";
				monsterList[ID].health	= 3503850;
				monsterList[ID].attack	= 2018741;
				monsterList[ID].vision	= 45;
			}
			else if(monsterList[ID].ID == 91) //Lizard Choosen
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'Ĺ';
				monsterList[ID].name	= "Lizard Choosen";
				monsterList[ID].health	= 3254100;
				monsterList[ID].attack	= 1913540;
				monsterList[ID].vision	= 46;
			}
			else if(monsterList[ID].ID == 92) //Cyklop Commander
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= 'ß';
				monsterList[ID].name	= "Cyklop Commander";
				monsterList[ID].health	= 3456100;
				monsterList[ID].attack	= 1998642;
				monsterList[ID].vision	= 47;
			}
			//47 - 48
			else if(monsterList[ID].ID == 93) //Undead Dragon
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= '§';
				monsterList[ID].name	= "Undead Dragon";
				monsterList[ID].health	= 4012398;
				monsterList[ID].attack	= 2017241;
				monsterList[ID].vision	= 50;
			}
			else if(monsterList[ID].ID == 94) //Grim Reaper
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= '%';
				monsterList[ID].name	= "Grim Reaper";
				monsterList[ID].health	= 3790200;
				monsterList[ID].attack	= 2408917;
				monsterList[ID].vision	= 50;
			}
			else if(monsterList[ID].ID == 95) //Lava Elemental
			{
				monsterList[ID].typeAI	= 3;
				monsterList[ID].symbol	= '♂';
				monsterList[ID].name	= "Lava Elemental";
				monsterList[ID].health	= 4092984;
				monsterList[ID].attack	= 1992390;
				monsterList[ID].vision	= 50;
			}
		}
	}


	public void AddPath(int monsterID, int[] path) {
		for(int i = 0; i < path.Length; ++i) {
			monsterList[monsterID].path.Add(path[i]);
		}
	}

}	
