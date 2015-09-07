using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RogueItemType {
	Other	= 0,
	Weapon	= 1,
	Helmet	= 2,
	Armor	= 3,
	Legs	= 4,
	Ring	= 5,
	Amulet	= 6,
	Drinks	= 7,
	Key		= 8,
	Food	= 9,
	Gold	= 10
}

public class RogueItemData {
	public string			name	= "UnknownItem";	//Nazwa przedmiotu
	public char				symbol	= '?';	//Wygląd przedmiotu
	public RogueItemType	type	= RogueItemType.Other;	//Typ przedmiotu
	public int				value	= 0;	//wartość wewnętrzna przedmiotu (np.: Miecz ma Atak)
	public int				cost	= 0;	//Wartość pieniężna
}