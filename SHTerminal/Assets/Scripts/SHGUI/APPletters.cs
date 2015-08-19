using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class APPletters: SHGUIappbase
{
	private List<SHGUIview> letters1;
	private List<SHGUIview> letters2;
	private List<SHGUIview> letters3;	

	public APPletters ():base("letters-standard-rendering-test-by-3.14")
	{
		Init ();
		letters1 = new List<SHGUIview> ();
		letters2 = new List<SHGUIview> ();	
		letters3 = new List<SHGUIview> ();		
		

		StringBuilder str;
		/*
		str = new StringBuilder();
		str.Append(" _____ \n");
		str.Append("|   __|\n");
		str.Append("|__   |\n");
		str.Append("|_____|\n");
		AddLetter(str.ToString());

		str = new StringBuilder();
		str.Append(" _____ \n");
		str.Append("|  |  |\n");
		str.Append("|  |  |\n");
		str.Append("|_____|\n");
		AddLetter(str.ToString());

		str = new StringBuilder();
		str.Append(" _____ \n");
		str.Append("|  _  |\n");
		str.Append("|   __|\n");
		str.Append("|__|   \n");
		AddLetter(str.ToString());

		str = new StringBuilder();
		str.Append(" _____ \n");
		str.Append("|   __|\n");
		str.Append("|   __|\n");
		str.Append("|_____|\n");
		AddLetter(str.ToString());

		str = new StringBuilder();
		str.Append(" _____ \n");
		str.Append("| __  |\n");
		str.Append("|    -|\n");
		str.Append("|__|__|\n");
		AddLetter(str.ToString());

		str = new StringBuilder();
		str.Append(" __ __ \n");
		str.Append("|  |  |\n");
		str.Append("|     |\n");
		str.Append("|__|__|\n");
		AddLetter(str.ToString());

		str = new StringBuilder();
		str.Append(" _____ \n");
		str.Append("|     |\n");
		str.Append("|  |  |\n");
		str.Append("|_____|\n");
		AddLetter(str.ToString());

		str = new StringBuilder();
		str.Append(" _____ \n");
		str.Append("|_   _|\n");
		str.Append("  | |  \n");
		str.Append("  |_|  \n");
		AddLetter(str.ToString());
		*/

		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█      \n");
		str.Append("███████\n");
		str.Append("      █\n");
		str.Append("███████\n");
		AddLetter1(str.ToString());
		
		str = new StringBuilder();
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("███████\n");
		AddLetter1(str.ToString());
		
		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█     █\n");
		str.Append("███████\n");
		str.Append("█      \n");
		str.Append("█      \n");
		AddLetter1(str.ToString());
		
		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█      \n");
		str.Append("███████\n");
		str.Append("█      \n");
		str.Append("███████\n");
		AddLetter1(str.ToString());
		
		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█     █\n");
		str.Append("███████\n");
		str.Append("█    █ \n");
		str.Append("█     █\n");
		
		AddLetter1(str.ToString());
		
		str = new StringBuilder();
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("███████\n");
		str.Append("█     █\n");
		str.Append("█     █\n");
		AddLetter1(str.ToString());
		
		
		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("███████\n");
		AddLetter1(str.ToString());
		

		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("   █   \n");
		str.Append("   █   \n");
		str.Append("   █   \n");
		str.Append("   █   \n");
		AddLetter1(str.ToString());

		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█      \n");
		str.Append("███████\n");
		str.Append("      █\n");
		str.Append("███████\n");
		AddLetter2(str.ToString());
		
		str = new StringBuilder();
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("███████\n");
		AddLetter2(str.ToString());
		
		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█     █\n");
		str.Append("███████\n");
		str.Append("█      \n");
		str.Append("█      \n");
		AddLetter2(str.ToString());
		
		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█      \n");
		str.Append("███████\n");
		str.Append("█      \n");
		str.Append("███████\n");
		AddLetter2(str.ToString());
		
		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█     █\n");
		str.Append("███████\n");
		str.Append("█    █ \n");
		str.Append("█     █\n");
		
		AddLetter2(str.ToString());
		
		str = new StringBuilder();
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("███████\n");
		str.Append("█     █\n");
		str.Append("█     █\n");
		AddLetter2(str.ToString());
		
		
		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("███████\n");
		AddLetter2(str.ToString());
		
		
		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("   █   \n");
		str.Append("   █   \n");
		str.Append("   █   \n");
		str.Append("   █   \n");
		AddLetter2(str.ToString());

		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█      \n");
		str.Append("███████\n");
		str.Append("      █\n");
		str.Append("███████\n");
		AddLetter3(str.ToString());
		
		str = new StringBuilder();
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("███████\n");
		AddLetter3(str.ToString());
		
		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█     █\n");
		str.Append("███████\n");
		str.Append("█      \n");
		str.Append("█      \n");
		AddLetter3(str.ToString());
		
		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█      \n");
		str.Append("███████\n");
		str.Append("█      \n");
		str.Append("███████\n");
		AddLetter3(str.ToString());
		
		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█     █\n");
		str.Append("███████\n");
		str.Append("█    █ \n");
		str.Append("█     █\n");
		
		AddLetter3(str.ToString());
		
		str = new StringBuilder();
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("███████\n");
		str.Append("█     █\n");
		str.Append("█     █\n");
		AddLetter3(str.ToString());
		
		
		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("█     █\n");
		str.Append("███████\n");
		AddLetter3(str.ToString());
		
		
		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("   █   \n");
		str.Append("   █   \n");
		str.Append("   █   \n");
		str.Append("   █   \n");
		AddLetter3(str.ToString());

		/*
		 * 		str = new StringBuilder();
		str.Append("███████ \n");
		str.Append("█░░░░░░░\n");
		str.Append("███████░\n");
		str.Append(" ░░░░░█░\n");
		str.Append("███████░\n");
		str.Append(" ░░░░░░░\n");
		
		AddLetter(str.ToString());
		
		str = new StringBuilder();
		str.Append("█     █ \n");
		str.Append("█░    █░\n");
		str.Append("█░    █░\n");
		str.Append("█░    █░\n");
		str.Append("███████░\n");
		str.Append(" ░░░░░░░\n");
		
		AddLetter(str.ToString());
		
		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█░░░░░█░\n");
		str.Append("███████░\n");
		str.Append("█░░░░░░░\n");
		str.Append("█░      \n");
		str.Append(" ░      \n");
		
		
		AddLetter(str.ToString());
		
		str = new StringBuilder();
		str.Append("███████ \n");
		str.Append("█░░░░░░░\n");
		str.Append("███████ \n");
		str.Append("█░░░░░░░\n");
		str.Append("███████ \n");
		str.Append(" ░░░░░░░\n");
		
		AddLetter(str.ToString());
		
		str = new StringBuilder();
		str.Append("███████ \n");
		str.Append("█░░░░░█░\n");
		str.Append("███████░\n");
		str.Append("█░░░░█░░\n");
		str.Append("█░   ░█\n");
		str.Append(" ░    ░░ \n");
		
		AddLetter(str.ToString());
		
		str = new StringBuilder();
		str.Append("█     █\n");
		str.Append("█░    █░\n");
		str.Append("███████░\n");
		str.Append("█░░░░░█░\n");
		str.Append("█░    █░\n");
		str.Append(" ░     ░\n");
		
		AddLetter(str.ToString());
		
		
		str = new StringBuilder();
		str.Append("███████\n");
		str.Append("█░░░░░█░\n");
		str.Append("█░    █░\n");
		str.Append("█░    █░\n");
		str.Append("███████░\n");
		str.Append(" ░░░░░░░\n");
		
		AddLetter(str.ToString());
		

		str = new StringBuilder();
		str.Append("███████\n");
		str.Append(" ░░█░░░░\n");
		str.Append("   █░  \n");
		str.Append("   █░  \n");
		str.Append("   █░  \n");
		str.Append("    ░  \n");
		 */

		//AddSubView (new SHGUIrect (0, 0, SHGUI.current.resolutionX - 1, SHGUI.current.resolutionY - 1));
		APPFRAME = AddSubView (new SHGUIframe (0, 0, SHGUI.current.resolutionX - 1, SHGUI.current.resolutionY - 1, 'z')) as SHGUIframe;
		APPLABEL = AddSubView (new SHGUItext ("letters-standard-rendering-test-by-3.14", 3, 0, 'w')) as SHGUItext;
		APPINSTRUCTION = AddSubView (new SHGUItext (LocalizationManager.Instance.GetLocalized("QUIT_APP_INPUT"), SHGUI.current.resolutionX - 5, SHGUI.current.resolutionY - 1, 'w').GoFromRight()) as SHGUItext;
	}

	void AddLetter1(string content){
		//letters1.Add(AddSubView(new SHGUItext(content, 9 + letters1.Count * 8, 5, 'z')));
	}

	
	void AddLetter2(string content){
		letters2.Add(AddSubView(new SHGUItext(content, 1 + letters2.Count * 8, 5, 'r')));
	}
	
	void AddLetter3(string content){
		letters3.Add(AddSubView(new SHGUItext(content, 1 + letters3.Count * 8, 5, 'w')));
	}

	public override void Update(){
		for (int i = 0; i < letters3.Count; ++i){
			//letters1[i].hidden = true;
			//letters1[i].y = 9 + (int)(Mathf.Sin(Time.realtimeSinceStartup * 4f + i * Mathf.PI / 5) * 5);
			letters2[i].y = 9 + (int)(Mathf.Sin(Time.realtimeSinceStartup * 4f + Mathf.PI + i * Mathf.PI / 5) * 4);
			letters3[i].y = 9 + (int)(Mathf.Sin(Time.realtimeSinceStartup * 4f + i * Mathf.PI / 5) * 4);

			letters2[i].x = i * 8 + (int)(Mathf.Sin(Time.realtimeSinceStartup * 4f + Mathf.PI + i * Mathf.PI / 5) * 4);
			letters3[i].x = i * 8 + (int)(Mathf.Sin(Time.realtimeSinceStartup * 4f + i * Mathf.PI / 5) * 4);
			
			
		}

		if (UnityEngine.Random.value > 0.98f) {
			letters3[(int)UnityEngine.Random.Range(0, letters3.Count)].PunchIn(.3f);
			letters2[(int)UnityEngine.Random.Range(0, letters2.Count)].PunchIn(.3f);
		}

		base.Update ();
	}
}

