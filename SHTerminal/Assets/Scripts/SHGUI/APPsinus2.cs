
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class APPsinus2: SHGUIappbase
{
	string super;
	string hot;

	List<string> columns;

	public APPsinus2 (): base("sinus-2-app-by-3.14")
	{
		super = SHGUI.current.GetASCIIartByName("supersmall");
		hot = SHGUI.current.GetASCIIartByName("hotsmall");

		columns = new List<string>();

		GetColumnsFrom(super);
		columns.Add("");
		columns.Add("");
		
		GetColumnsFrom(hot);

		

		Randomize();
	
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
		
		time += Time.unscaledDeltaTime;
		//if (fade < 0.99f)
	//		return;

		base.Redraw (offx, offy);

		XX -= Time.unscaledDeltaTime * 30;
		int X = (int)XX;

		if (X < 0) X += SHGUI.current.resolutionX;

		int Y = 8;
		for (int i = 0; i < columns.Count - 1; ++i){
			for (int j = 0; j < columns[i].Length; ++j){
				if (columns[i][j] != ' ');
				int startx = mapxcol(X + i);
				if (startx < 80 - 1)
					SHGUI.current.SetPixelFront(columns[i][j], startx, Y + j + (int)(mysin(time, startx)), 'w');
			}
		}
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


