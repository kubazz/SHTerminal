using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RogueMonsters {

	//mapa
	public	int[,]						map = null;

	//potwory
	public	List<RogueMonster>			monsterList = new List<RogueMonster>();
	public	RoguePlayer					player;

	public RogueMonsters(int[,] mapka, RoguePlayer pl)
	{
		monsterList.Clear();
		map = mapka;
		player = pl;

		//monsterList.Add(new RogueMonster(10, this, 16, 9));
		//								ID	PARENT	X	Y
		monsterList.Add(new RogueMonster(7, this, 18, 8));
		monsterList.Add(new RogueMonster(29, this, 20, 7));

	}

	public void SetParameters(int ID, RogueMonster monster)
	{
		//1 - 2
		if(ID == 0) //Goblin
		{
			monster.typeAI	= 1;
			monster.symbol	= 'g';
			monster.name	= "Goblin";
			monster.health	= 23;
			monster.attack	= 10;
			AddPath(ID, new int[4] {0, 4, 2, 4});
			
		}
		else if(ID == 1) //Worm
		{
			monster.typeAI	= 0;
			monster.symbol	= '.';
			monster.name	= "Worm";
			monster.health	= 15;
			monster.attack	= 0;
		}
		else if(ID == 2) //Cockroach
		{
			monster.typeAI	= 1;
			monster.symbol	= ',';
			monster.name	= "Cockroach";
			monster.health	= 10;
			monster.attack	= 5;
			AddPath(ID, new int[4] {1, 4, 3, 4});
		}
		//3 - 4
		else if(ID == 3) //Dwarf
		{
			monster.typeAI = 1;
			monster.symbol = 'd';
			monster.name = "Dwarf";
			monster.health = 135;
			monster.attack = 42;
			AddPath(ID, new int[4] {0, 4, 2, 4});
		}
		else if(ID == 4) //Goblin Scavanger
		{
			monster.typeAI	= 1;
			monster.symbol	= 'G';
			monster.name	= "Goblin Scavenger";
			monster.health	= 90;
			monster.attack	= 50;
			AddPath(ID, new int[4] {3, 4, 1, 4});
		}		
		else if(ID == 5) //Spider
		{
			monster.typeAI	= 1;
			monster.symbol	= '¤';
			monster.name	= "Spider";
			monster.health	= 78;
			monster.attack	= 45;
			AddPath(ID, new int[16] {0, 4, 0, 4, 1, 4, 1, 4, 2, 4, 2, 4, 3, 4, 3, 4});
		}
		//5 - 6
		else if(ID == 6) //Goblin Slinger
		{
			monster.typeAI	= 1;
			monster.symbol	= 'ŕ';
			monster.name	= "Goblin Slinger";
			monster.health	= 317;
			monster.attack	= 140;
			AddPath(ID, new int[8] {1, 4, 2, 4, 0, 4, 3, 4});
		}
		else if(ID == 7) //Skeleton
		{
			monster.typeAI	= 3;
			monster.symbol	= 's';
			monster.name	= "Skeleton";
			monster.health	= 500;
			monster.attack	= 270;
			monster.vision	= 7;
		}
		else if(ID == 8) //Zombie
		{
			monster.typeAI	= 3;
			monster.symbol	= 'z';
			monster.name	= "Zombie";
			monster.health	= 800;
			monster.attack	= 45;
			monster.vision	= 7;
		}
		else if(ID == 9) //Poison Spider
		{
			monster.typeAI	= 1;
			monster.symbol	= '*';
			monster.name	= "Poison Spider";
			monster.health	= 300;
			monster.attack	= 105;
			AddPath(ID, new int[16] {4, 1, 4, 1, 4, 1, 4, 2, 4, 3, 4, 3, 4, 3, 4, 0});
		}
		//7 - 8
		else if(ID == 10) //Vampire
		{
			monster.typeAI	= 3;
			monster.symbol	= 'v';
			monster.name	= "Vampire";
			monster.health	= 1590;
			monster.attack	= 258;
			monster.vision	= 11;
		}
		else if(ID == 11) //Dwarf Miner
		{
			monster.typeAI	= 1;
			monster.symbol	= 'D';
			monster.name	= "Dwarf Miner";
			monster.health	= 960;
			monster.attack	= 298;
			AddPath(ID, new int[12] {1, 4, 0, 4, 1, 4, 3, 4, 2, 4, 3, 4});
		}
		else if(ID == 12) //Scarab
		{
			monster.typeAI	= 1;
			monster.symbol	= 'ˇ';
			monster.name	= "Scarab";
			monster.health	= 1179;
			monster.attack	= 395;
			AddPath(ID, new int[16] {3, 4, 3, 4, 3, 4, 2, 4, 1, 4, 1, 4, 1, 4, 0, 4});
		}
		else if(ID == 13) //Scorpion
		{
			monster.typeAI	= 1;
			monster.symbol	= '|';
			monster.name	= "Scorpion";
			monster.health	= 629;
			monster.attack	= 1091;
			AddPath(ID, new int[8] {1, 1, 1, 0, 3, 3, 3, 2});
		}
		else if(ID == 14) //Skeleton Archer
		{
			monster.typeAI	= 3;
			monster.symbol	= 'S';
			monster.name	= "Skeleton Archer";
			monster.health	= 2704;
			monster.attack	= 1000;
			monster.vision	= 16;
		}
		//9 - 10
		else if(ID == 15) //Lizard
		{
			monster.typeAI	= 1;
			monster.symbol	= 'l';
			monster.name	= "Lizard";
			monster.health	= 3893;
			monster.attack	= 1764;
			AddPath(ID, new int[6] {0, 0, 1, 2, 2, 3});
		}
		else if(ID == 16) //Small Troll
		{
			monster.typeAI	= 1;
			monster.symbol	= 't';
			monster.name	= "Small Troll";
			monster.health	= 4162;
			monster.attack	= 1593;
			AddPath(ID, new int[16] {0, 4, 0, 4, 0, 4, 0, 4, 2, 4, 2, 4, 2, 4, 2, 4});
		}
		else if(ID == 17) //Orc
		{
			monster.typeAI	= 3;
			monster.symbol	= 'o';
			monster.name	= "Orc";
			monster.health	= 3586;
			monster.attack	= 1890;
			monster.vision	= 15;
		}
		else if(ID == 18) //Dwarf Solider
		{
			monster.typeAI	= 3;
			monster.symbol	= 'đ';
			monster.name	= "Dwarf Solider";
			monster.health	= 3129;
			monster.attack	= 1190;
			monster.vision	= 15;
		}
		else if(ID == 19) //Skeleton Warrior
		{
			monster.typeAI	= 1;
			monster.symbol	= 'ś';
			monster.name	= "Skeleton Warrior";
			monster.health	= 3786;
			monster.attack	= 1111;
			AddPath(ID, new int[8] {3, 3, 3, 3, 1, 1, 1, 1});
		}
		//11 - 12
		else if(ID == 20) //Troll
		{
			monster.typeAI	= 3;
			monster.symbol	= 'T';
			monster.name	= "Troll";
			monster.health	= 8540;
			monster.attack	= 2897;
			monster.vision	= 12;
		}
		else if(ID == 21) //Orc Spearman
		{
			monster.typeAI	= 3;
			monster.symbol	= 'O';
			monster.name	= "Orc Spearman";
			monster.health	= 4982;
			monster.attack	= 1996;
			monster.vision	= 15;
		}
		else if(ID == 22) //Goblin Assasin
		{
			monster.typeAI	= 2;
			monster.symbol	= 'A';
			monster.name	= "Goblin Assasin";
			monster.health	= 4000;
			monster.attack	= 4145;
			monster.vision	= 20;
		}
		else if(ID == 23) //Ghost
		{
			monster.typeAI	= 3;
			monster.symbol	= '▲';
			monster.name	= "Ghost";
			monster.health	= 7541;
			monster.attack	= 2159;
			monster.vision	= 20;
		}
		else if(ID == 24) //Ghoul
		{
			monster.typeAI	= 3;
			monster.symbol	= 'Ů';
			monster.name	= "Ghoul";
			monster.health	= 6892;
			monster.attack	= 2638;
			monster.vision	= 18;
		}
		//13 - 14
		else if(ID == 25) //Skeleton Pirate
		{
			monster.typeAI	= 3;
			monster.symbol	= 'Ś';
			monster.name	= "Skeleton Pirate";
			monster.health	= 12100;
			monster.attack	= 4582;
			monster.vision	= 16;
		}
		else if(ID == 26) //Pirate Ghost
		{
			monster.typeAI	= 3;
			monster.symbol	= 'p';
			monster.name	= "Pirate Ghost";
			monster.health	= 14012;
			monster.attack	= 3582;
			monster.vision	= 20;
		}
		else if(ID == 27) //Orc Warrior
		{
			monster.typeAI	= 3;
			monster.symbol	= 'ő';
			monster.name	= "Orc Warrior";
			monster.health	= 11931;
			monster.attack	= 5780;
			monster.vision	= 15;
		}
		//15 -  16
		else if(ID == 28) //Giant Spider
		{
			monster.typeAI	= 2;
			monster.symbol	= 'x';
			monster.name	= "Giant Spider";
			monster.health	= 15019;
			monster.attack	= 8462;
			monster.vision	= 18;
		}
		else if(ID == 29) //Elf
		{
			monster.typeAI	= 2;
			monster.symbol	= 'e';
			monster.name	= "Elf";
			monster.health	= 17671;
			monster.attack	= 7340;
			monster.vision	= 25;
		}
		else if(ID == 30) //Lizard Sentitel
		{
			monster.typeAI	= 3;
			monster.symbol	= 'L';
			monster.name	= "Lizard Sentitel";
			monster.health	= 18834;
			monster.attack	= 6245;
			monster.vision	= 21;
		}
		//17 - 18
		else if(ID == 31) //Dwarf Guard
		{
			monster.typeAI	= 2;
			monster.symbol	= 'ď';
			monster.name	= "Dwarf Guard";
			monster.health	= 28721;
			monster.attack	= 9012;
			monster.vision	= 20;
		}
		else if(ID == 32) //Orc Rider
		{
			monster.typeAI	= 2;
			monster.symbol	= 'Ő';
			monster.name	= "Orc Rider";
			monster.health	= 25012;
			monster.attack	= 7284;
			monster.vision	= 25;
		}
		else if(ID == 33) //Ancient Scarab
		{
			monster.typeAI	= 3;
			monster.symbol	= 'š';
			monster.name	= "Ancient Scarab";
			monster.health	= 30102;
			monster.attack	= 5233;
			monster.vision	= 26;
		}
		else if(ID == 34) //Mummy
		{
			monster.typeAI	= 3;
			monster.symbol	= 'm';
			monster.name	= "Mummy";
			monster.health	= 29032;
			monster.attack	= 6341;
			monster.vision	= 15;
		}
		//19 - 20
		else if(ID == 35) //Swamp Troll
		{
			monster.typeAI	= 3;
			monster.symbol	= 'ť';
			monster.name	= "Swamp Troll";
			monster.health	= 38901;
			monster.attack	= 15241;
			monster.vision	= 18;
		}
		else if(ID == 36) //Lizard Legionnaire
		{
			monster.typeAI	= 3;
			monster.symbol	= 'ł';
			monster.name	= "Lizard Legionnaire";
			monster.health	= 34901;
			monster.attack	= 14891;
			monster.vision	= 23;
		}
		else if(ID == 37) //Minotaur
		{
			monster.typeAI	= 3;
			monster.symbol	= 'n';
			monster.name	= "Minotaur";
			monster.health	= 40123;
			monster.attack	= 14002;
			monster.vision	= 22;
		}
		else if(ID == 38) //Cristal Spider
		{
			monster.typeAI	= 3;
			monster.symbol	= 'č';
			monster.name	= "Cristal Spider";
			monster.health	= 31023;
			monster.attack	= 18023;
			monster.vision	= 21;
		}
		else if(ID == 39) //Elf Scout
		{
			monster.typeAI	= 2;
			monster.symbol	= 'E';
			monster.name	= "Elf Scout";
			monster.health	= 32562;
			monster.attack	= 15023;
			monster.vision	= 35;
		}
		//21 - 22
		else if(ID == 40) //Dwarf Geomancer
		{
			monster.typeAI	= 3;
			monster.symbol	= 'Ď';
			monster.name	= "Dwarf Geomancer";
			monster.health	= 47013;
			monster.attack	= 24134;
			monster.vision	= 30;
		}
		else if(ID == 41) //Centipide
		{
			monster.typeAI	= 2;
			monster.symbol	= 'C';
			monster.name	= "Centipide";
			monster.health	= 57184;
			monster.attack	= 19934;
			monster.vision	= 21;
		}
		else if(ID == 42) //Dragon Hatchling
		{
			monster.typeAI	= 3;
			monster.symbol	= 'h';
			monster.name	= "Dragon Hatchling";
			monster.health	= 62314;
			monster.attack	= 21512;
			monster.vision	= 20;
		}
		else if(ID == 43) //Water Elemental
		{
			monster.typeAI	= 3;
			monster.symbol	= 'w';
			monster.name	= "Water Elemental";
			monster.health	= 52391;
			monster.attack	= 18236;
			monster.vision	= 25;
		}
		//22 - 23
		else if(ID == 44) //Troll Guard
		{
			monster.typeAI	= 3;
			monster.symbol	= 'ţ';
			monster.name	= "Troll Guard";
			monster.health	= 82130;
			monster.attack	= 39912;
			monster.vision	= 30;
		}
		else if(ID == 45) //Crypt Shambler
		{
			monster.typeAI	= 3;
			monster.symbol	= 'c';
			monster.name	= "Crypt Shambler";
			monster.health	= 75023;
			monster.attack	= 35091;
			monster.vision	= 31;
		}
		else if(ID == 46) //Minotaur Archer
		{
			monster.typeAI	= 3;
			monster.symbol	= 'N';
			monster.name	= "Minotaur Archer";
			monster.health	= 78671;
			monster.attack	= 38123;
			monster.vision	= 27;
		}
		else if(ID == 47) //Lizard Warrior
		{
			monster.typeAI	= 3;
			monster.symbol	= 'Ł';
			monster.name	= "Lizard Warrior";
			monster.health	= 77123;
			monster.attack	= 36127;
			monster.vision	= 30;
		}
		//25 - 26
		else if(ID == 48) //Orc Szaman
		{
			monster.typeAI	= 3;
			monster.symbol	= 'ö';
			monster.name	= "Orc Szaman";
			monster.health	= 93145;
			monster.attack	= 58243;
			monster.vision	= 34;
		}
		else if(ID == 49) //Skeleton Mage
		{
			monster.typeAI	= 3;
			monster.symbol	= 'ş';
			monster.name	= "Skeleton Mage";
			monster.health	= 102451;
			monster.attack	= 61345;
			monster.vision	= 34;
		}
		else if(ID == 50) //Elf Assasin
		{
			monster.typeAI	= 2;
			monster.symbol	= 'é';
			monster.name	= "Elf Assasin";
			monster.health	= 123456;
			monster.attack	= 64562;
			monster.vision	= 40;
		}
		else if(ID == 51) //Insektoid
		{
			monster.typeAI	= 3;
			monster.symbol	= 'I';
			monster.name	= "Insektoid";
			monster.health	= 135239;
			monster.attack	= 49124;
			monster.vision	= 31;
		}
		//27 - 28
		else if(ID == 52) //Orc Berseker
		{
			monster.typeAI	= 3;
			monster.symbol	= 'Ö';
			monster.name	= "Orc Berseker";
			monster.health	= 163414;
			monster.attack	= 89142;
			monster.vision	= 32;
		}
		else if(ID == 53) //Frost Dragon Hatchling
		{
			monster.typeAI	= 3;
			monster.symbol	= 'f';
			monster.name	= "Frost Dragon Hatchling";
			monster.health	= 182451;
			monster.attack	= 82412;
			monster.vision	= 33;
		}
		else if(ID == 54) //Nature Elemental
		{
			monster.typeAI	= 3;
			monster.symbol	= '♣';
			monster.name	= "Nature Elemental";
			monster.health	= 170973;
			monster.attack	= 72142;
			monster.vision	= 31;
		}
		//29 - 30
		else if(ID == 55) //Frost Troll
		{
			monster.typeAI	= 3;
			monster.symbol	= 'Ţ';
			monster.name	= "Frost Troll";
			monster.health	= 321445;
			monster.attack	= 102931;
			monster.vision	= 32;
		}
		else if(ID == 56) //Demon Skeleton
		{
			monster.typeAI	= 3;
			monster.symbol	= 'Ş';
			monster.name	= "Demon Skeleton";
			monster.health	= 280912;
			monster.attack	= 191345;
			monster.vision	= 35;
		}
		else if(ID == 57) //Banshee
		{
			monster.typeAI	= 3;
			monster.symbol	= 'b';
			monster.name	= "Banshee";
			monster.health	= 254300;
			monster.attack	= 102930;
			monster.vision	= 40;
		}
		else if(ID == 58) //Minotaur Guard
		{
			monster.typeAI	= 3;
			monster.symbol	= 'M';
			monster.name	= "Minotaur Guard";
			monster.health	= 312501;
			monster.attack	= 129510;
			monster.vision	= 33;
		}
		//31 - 32
		else if(ID == 59) //Crawler
		{
			monster.typeAI	= 3;
			monster.symbol	= 'Č';
			monster.name	= "Crawler";
			monster.health	= 531245;
			monster.attack	= 214556;
			monster.vision	= 32;
		}
		else if(ID == 60) //Lizard Templar
		{
			monster.typeAI	= 3;
			monster.symbol	= 'L';
			monster.name	= "Lizard Templar";
			monster.health	= 491450;
			monster.attack	= 229549;
			monster.vision	= 31;
		}
		else if(ID == 61) //Red Dragon Hatchling
		{
			monster.typeAI	= 3;
			monster.symbol	= 'r';
			monster.name	= "Red Dragon Hatchling";
			monster.health	= 571308;
			monster.attack	= 246728;
			monster.vision	= 35;
		}
		//33 - 34
		else if(ID == 62) //Dragon
		{
			monster.typeAI	= 3;
			monster.symbol	= 'Đ';
			monster.name	= "Dragon";
			monster.health	= 813542;
			monster.attack	= 372348;
			monster.vision	= 32;
		}
		else if(ID == 63) //Fire Elemental
		{
			monster.typeAI	= 3;
			monster.symbol	= '♥';
			monster.name	= "Fire Elemental";
			monster.health	= 801424;
			monster.attack	= 311062;
			monster.vision	= 33;
		}
		else if(ID == 64) //Cyclop
		{
			monster.typeAI	= 3;
			monster.symbol	= '÷';
			monster.name	= "Cyclop";
			monster.health	= 795281;
			monster.attack	= 362369;
			monster.vision	= 31;
		}
		else if(ID == 65) //Orc Maruder
		{
			monster.typeAI	= 3;
			monster.symbol	= 'ô';
			monster.name	= "Orc Maruder";
			monster.health	= 752465;
			monster.attack	= 298675;
			monster.vision	= 34;
		}
		//35 - 36
		else if(ID == 66) //Grave Troll
		{
			monster.typeAI	= 3;
			monster.symbol	= 'Ť';
			monster.name	= "Grave Troll";
			monster.health	= 1012093;
			monster.attack	= 416723;
			monster.vision	= 36;
		}
		else if(ID == 67) //Spectre
		{
			monster.typeAI	= 3;
			monster.symbol	= '~';
			monster.name	= "Spectre";
			monster.health	= 9612421;
			monster.attack	= 382601;
			monster.vision	= 40;
		}
		else if(ID == 68) //Undead Priest
		{
			monster.typeAI	= 3;
			monster.symbol	= ':';
			monster.name	= "Undead Priest";
			monster.health	= 913560;
			monster.attack	= 40953;
			monster.vision	= 32;
		}
		//37 - 38
		else if(ID == 69) //Minotaur Mage
		{
			monster.typeAI	= 3;
			monster.symbol	= 'Ň';
			monster.name	= "Minotaur Mage";
			monster.health	= 1309623;
			monster.attack	= 793462;
			monster.vision	= 35;
		}
		else if(ID == 70) //Giant Mantis
		{
			monster.typeAI	= 3;
			monster.symbol	= 'X';
			monster.name	= "Giant Mantis";
			monster.health	= 1709308;
			monster.attack	= 582394;
			monster.vision	= 38;
		}
		else if(ID == 71) //Elf Arcanist
		{
			monster.typeAI	= 3;
			monster.symbol	= 'É';
			monster.name	= "Elf Arcanist";
			monster.health	= 1538923;
			monster.attack	= 625782;
			monster.vision	= 40;
		}
		else if(ID == 72) //Lizard Snakecharmer
		{
			monster.typeAI	= 3;
			monster.symbol	= '╚';
			monster.name	= "Lizard Snakecharmer";
			monster.health	= 1675447;
			monster.attack	= 592436;
			monster.vision	= 38;
		}
		//39 - 40
		else if(ID == 73) //Frost Dragon
		{
			monster.typeAI	= 3;
			monster.symbol	= 'F';
			monster.name	= "Frost Dragon";
			monster.health	= 2007259;
			monster.attack	= 745270;
			monster.vision	= 36;
		}
		else if(ID == 74) //Ice Elemental
		{
			monster.typeAI	= 3;
			monster.symbol	= '♦';
			monster.name	= "Ice Elemental";
			monster.health	= 1825984;
			monster.attack	= 612340;
			monster.vision	= 35;
		}
		else if(ID == 75) //Cyclop Worker
		{
			monster.typeAI	= 3;
			monster.symbol	= 'Î';
			monster.name	= "Cyclop Worker";
			monster.health	= 1594138;
			monster.attack	= 783856;
			monster.vision	= 36;
		}
		//41 - 42
		else if(ID == 76) //Troll Warrior
		{
			monster.typeAI	= 3;
			monster.symbol	= '↑';
			monster.name	= "Troll Warrior";
			monster.health	= 2213173;
			monster.attack	= 809358;
			monster.vision	= 39;
		}
		else if(ID == 77) //Orc Leader
		{
			monster.typeAI	= 3;
			monster.symbol	= 'Ô';
			monster.name	= "Orc Leader";
			monster.health	= 2235633;
			monster.attack	= 901246;
			monster.vision	= 39;
		}
		else if(ID == 78) //Lich
		{
			monster.typeAI	= 3;
			monster.symbol	= 'Ľ';
			monster.name	= "Lich";
			monster.health	= 2182451;
			monster.attack	= 991234;
			monster.vision	= 39;
		}
		//43 - 44
		else if(ID == 79) //Elder Elf
		{
			monster.typeAI	= 3;
			monster.symbol	= 'Ę';
			monster.name	= "Elder Elf";
			monster.health	= 2501447;
			monster.attack	= 1023501;
			monster.vision	= 49;
		}
		else if(ID == 80) //Lizard Dragon Priest
		{
			monster.typeAI	= 3;
			monster.symbol	= 'ľ';
			monster.name	= "Lizard Dragon Priest";
			monster.health	= 2498712;
			monster.attack	= 1001534;
			monster.vision	= 40;
		}
		else if(ID == 81) //Red Dragon
		{
			monster.typeAI	= 3;
			monster.symbol	= 'R';
			monster.name	= "Red Dragon";
			monster.health	= 2601345;
			monster.attack	= 1118730;
			monster.vision	= 40;
		}
		else if(ID == 82) //Stone Elemental
		{
			monster.typeAI	= 3;
			monster.symbol	= '♠';
			monster.name	= "Stone Elemental";
			monster.health	= 2889340;
			monster.attack	= 8021470;
			monster.vision	= 41;
		}
		//45 - 46
		else if(ID == 83) //Cyclop Smith
		{
			monster.typeAI	= 3;
			monster.symbol	= '‼';
			monster.name	= "Cyclop Smith";
			monster.health	= 2814925;
			monster.attack	= 1602831;
			monster.vision	= 42;
		}
		else if(ID == 84) //Orc Warlord
		{
			monster.typeAI	= 3;
			monster.symbol	= '0';
			monster.name	= "Orc Warlord";
			monster.health	= 2701539;
			monster.attack	= 1802931;
			monster.vision	= 42;
		}
		else if(ID == 85) //Undead Gladiator
		{
			monster.typeAI	= 3;
			monster.symbol	= 'U';
			monster.name	= "Undead Gladiator";
			monster.health	= 2579342;
			monster.attack	= 1614235;
			monster.vision	= 42;
		}
		else if(ID == 86) //Lizard High Guard
		{
			monster.typeAI	= 3;
			monster.symbol	= 'ĺ';
			monster.name	= "Lizard High Guard";
			monster.health	= 2901804;
			monster.attack	= 1312523;
			monster.vision	= 45;
		}
		//47 - 48
		else if(ID == 87) //Hydra
		{
			monster.typeAI	= 3;
			monster.symbol	= 'H';
			monster.name	= "Hydra";
			monster.health	= 3020809;
			monster.attack	= 1748230;
			monster.vision	= 50;
		}
		else if(ID == 88) //Energy Elemental
		{
			monster.typeAI	= 3;
			monster.symbol	= '♀';
			monster.name	= "Energy Elemental";
			monster.health	= 2989010;
			monster.attack	= 1692540;
			monster.vision	= 42;
		}
		else if(ID == 89) //Cyclop Warrior
		{
			monster.typeAI	= 3;
			monster.symbol	= 'W';
			monster.name	= "Cyclop Warrior";
			monster.health	= 2171730;
			monster.attack	= 2002380;
			monster.vision	= 41;
		}
		//49 - 51
		else if(ID == 90) //Troll Champion
		{
			monster.typeAI	= 3;
			monster.symbol	= '▼';
			monster.name	= "Troll Champion";
			monster.health	= 3503850;
			monster.attack	= 2018741;
			monster.vision	= 45;
		}
		else if(ID == 91) //Lizard Choosen
		{
			monster.typeAI	= 3;
			monster.symbol	= 'Ĺ';
			monster.name	= "Lizard Choosen";
			monster.health	= 3254100;
			monster.attack	= 1913540;
			monster.vision	= 46;
		}
		else if(ID == 92) //Cyklop Commander
		{
			monster.typeAI	= 3;
			monster.symbol	= 'ß';
			monster.name	= "Cyklop Commander";
			monster.health	= 3456100;
			monster.attack	= 1998642;
			monster.vision	= 47;
		}
		//47 - 48
		else if(ID == 93) //Undead Dragon
		{
			monster.typeAI	= 3;
			monster.symbol	= '§';
			monster.name	= "Undead Dragon";
			monster.health	= 4012398;
			monster.attack	= 2017241;
			monster.vision	= 50;
		}
		else if(ID == 94) //Grim Reaper
		{
			monster.typeAI	= 3;
			monster.symbol	= '%';
			monster.name	= "Grim Reaper";
			monster.health	= 3790200;
			monster.attack	= 2408917;
			monster.vision	= 50;
		}
		else if(ID == 95) //Lava Elemental
		{
			monster.typeAI	= 3;
			monster.symbol	= '♂';
			monster.name	= "Lava Elemental";
			monster.health	= 4092984;
			monster.attack	= 1992390;
			monster.vision	= 50;
		}

	}


	public void AddPath(int monsterID, int[] path) {
		for(int i = 0; i < path.Length; ++i) {
			monsterList[monsterID].path.Add(path[i]);
		}
	}

}	
