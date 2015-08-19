
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class APPcredits: SHGUIappbase
{
	string super;
	string hot;

	List<string> columns;

	APPscrollconsole console;
	

	public APPcredits (): base("")
	{
		super = SHGUI.current.GetASCIIartByName("supersmall");
		hot = SHGUI.current.GetASCIIartByName("hotsmall");

		columns = new List<string>();

		GetColumnsFrom(super);
		columns.Add("");
		columns.Add("");
		
		GetColumnsFrom(hot);

		APPFRAME.hidden = true;

		Randomize();

		console = new APPscrollconsole ();
		console.y = 16;
		console.maxlines = 6;

		AddSubView (console);

		console.AddWait (2f);

		AddCreditedLine ("game director", "Piotr Iwanicki");
		console.AddWait (1f);
		AddCreditedLine ("art director", "Marcin Surma");
		AddCreditedLine ("programming", "Krzysztof Tracz");
		AddCreditedLine("programming", "Jakub Ziembiński");
		AddCreditedLine("story", "Cezary Skorupka");
		AddCreditedLine("business", "Tomasz Kaczmarczyk");
		AddCreditedLine("PR", "Marek Bączyński");		
		AddCreditedLine("level design", "Panos Rriska");
		AddCreditedLine("3d art", "Tomasz Bolek");
		AddCreditedLine("3d art", "Piotr Kosmala");
		AddCreditedLine("sound", "Artur Walaszczyk");
		AddCreditedLine("additional animations", "fragOut Studio");
		AddCreditedLine("additinal credits", "Łukasz Spierewka|Dawid Adamkiewicz|Konrad Kacperczyk|Maciej Nabiałczyk|Rafał Romanowicz|||||");
		//AddCreditedLine("^Cz(those are just basic credits", "^Czmade quickly for the beta)");

		console.AddEmptyLine (.01f);
		console.AddEmptyLine (.01f);
		console.AddEmptyLine (.01f);
		console.AddEmptyLine (.01f);
		console.AddEmptyLine (.01f);
		console.AddEmptyLine (.01f);
		console.AddEmptyLine (.01f);
		console.AddEmptyLine (.01f);
		console.AddEmptyLine (.01f);

		console.AddPrompterToQueue ("^Cw" + "Huge thanks to all you guys, our beta backers :)!", .1f, true);		

		console.AddEmptyLine (.1f);
		console.AddEmptyLine (.1f);
		console.AddEmptyLine (.1f);
		console.AddWait (3f);
						
		AddBackers (Resources.Load ("backerlist").ToString());

		console.AddEmptyLine (.5f);
		console.AddEmptyLine (.5f);
		console.AddEmptyLine (.5f);
		console.AddEmptyLine (.5f);
		console.AddEmptyLine (.5f);
		
	
		console.AddPrompterToQueue ("THANKS FOR YOUR SUPPORT. YOU ARE AWESOME.", .1f, true);
		console.AddPrompterToQueue ("THIS GAME WOULDN'T BE POSSIBLE WITHOUT YOU.", .1f, true);
		console.AddPrompterToQueue ("SUPERHOT TEAM SALUTES YOU.", .1f, true);
		
		
		console.AddEmptyLine (.1f);
		console.AddEmptyLine (.1f);
		console.AddWait (1000000f);

		this.AddSubView (new SHGUIrect (0, 0, SHGUI.current.resolutionX, console.y - 1, '0', ' '));
		this.AddSubView (new SHGUIrect (0, 0, SHGUI.current.resolutionX, console.y - 2, 'r', '░'));
	
	}

	void AddCreditedLine(string thing, string person){
		console.AddPrompterToQueue ("^Cw" + thing, .1f, true);

		string[] persons = person.Split ('|');
		int i = 0;
		for (; i < persons.Length; ++i) {
			console.AddPrompterToQueue ("^Cr" + persons[i], (persons.Length <= 1)?.1f:.75f, true);
		}
		if (i % 2 == 0) {
			console.AddEmptyLine (.1f);
		}
		console.AddEmptyLine (2f);
	}

	void AddCreditedLine(string person, float delay){
		console.AddPrompterToQueue ("^Cw" + person, delay, true);
	}

	void AddBackers(string backers){
		string[] b = backers.Split ('\n');
		string currentline = "";
		int maxlinelength = 54;
		for (int i = 0; i < b.Length; ++i){
			if (b[i].Length < maxlinelength){
				console.AddPrompterToQueueFaster("^Cw" + b[i], 0.75f, true);
			}
			else{
				string[] ziom = b[i].Split(' ');
				string line = " ";

				for (int j = 0; j < ziom.Length; ++j){
					if (line.Length + ziom[j].Length > maxlinelength){
						console.AddPrompterToQueueFaster("^Cw" + line, 1.55f, true);
						line = ziom[j];
					}
					else{
						line += " " + ziom[j];						
					}
				}

				console.AddPrompterToQueueFaster("^Cw" + line, 1.55f, true);
			}
			
			/*
			if (currentline.Length + b[i].Length > SHGUI.current.resolutionX - 10){
				AddCreditedLine(currentline + ",", 1.5f);
				currentline = b[i];				
			}
			else{
				if (i == 0)
					currentline += b[i];
				else
					currentline += ", " + b[i];
			}
			*/
		}		
	}

	void GetColumnsFrom(string source){
		int i = 0;
		while (GetColumnnFromString(i, source) != ""){
			columns.Add(GetColumnnFromString(i, source));
			i++;
		}
	}

	string GetColumnnFromString(int column, string source){
		string str = "";
		int col = 0;
		for (int i = 0; i < source.Length; ++i){
			if (col == column){
				char c = source[i];
				if ((int)c == 10) c = ' ';
				if (c != '\n')
					str += c;
			}

			col++;
			if (source[i] == '\n'){
				col = 0;
			}
		}

		return str;
	}

			float time = 0;

	float XX = -1;
	public override void Redraw(int offx, int offy){

		console.ClearMessagesAbove (0);

		time += Time.unscaledDeltaTime;
		//if (fade < 0.99f)
	//		return;

		base.Redraw (offx, offy);

		XX -= Time.unscaledDeltaTime * 30;
		int X = (int)XX;

		if (X < 0) X += SHGUI.current.resolutionX;

		int Y = 4;
		for (int i = 0; i < columns.Count - 1; ++i){
			for (int j = 0; j < columns[i].Length; ++j){
				if (columns[i][j] != ' ');
				int startx = mapxcol(X + i);
				if (startx < SHGUI.current.resolutionX - 1)
					if (fade > 0.5f)
						SHGUI.current.SetPixelFront(StringScrambler.GetScrambledString(columns[i][j] + "",1-fade)[0], startx, Y + j + (int)(mysin(time, startx)), 'w');
			}
		}

		//APPFRAME.Redraw (offx, offy);
	}

	int mapxcol(int x){
		int A = x;
		while (A < 1){
			A+= 80 - 1;
		}
		return A;
	}

	int speed = 0;
	int height = 0;
	void Randomize(){
		speed = UnityEngine.Random.Range (10, 20);
		height = (int)( UnityEngine.Random.Range (2, 5));
		height = 4;
	}

	float mysin(float t, int i){
		return (height * Mathf.Sin(8 * t + (float)(i * Math.PI / 32)));
	}
}


