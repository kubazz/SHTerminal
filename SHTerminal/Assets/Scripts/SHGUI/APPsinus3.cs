
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


public class APPsinus3: SHGUIappbase
{
	
	List<string> columns;
	
	public APPsinus3 (): base("sinus-3-app-by-3.14")
	{	
		columns = new List<string>();

		string pattern = "█▓▒░  ░▒▓██████▓▒░  ░▒▓█";
		for (int i = 0; i < 64; ++i){
			columns.Add(pattern);

		}

		/*
		for (int i = 0; i < 20; ++i){
			columns.Add("█▓▒░ ░▒▓█");
			MultiplyLastColumn();
			columns.Add("██▓▒░ ░▒▓");
			MultiplyLastColumn();
			
			columns.Add("▓██▓▒░ ░▒");
			MultiplyLastColumn();
			
			columns.Add("▒▓██▓▒░ ░");
			MultiplyLastColumn();
			
			columns.Add("░▒▓██▓▒░ ");
			MultiplyLastColumn();
			
			columns.Add(" ░▒▓██▓▒░");
			MultiplyLastColumn();
			
			columns.Add("░ ░▒▓██▓▒");
			MultiplyLastColumn();
			
			columns.Add("▒░ ░▒▓██▓");
			MultiplyLastColumn();
			
			columns.Add("▓▒░ ░▒▓██");			
			MultiplyLastColumn();
		}
		*/
		
		Randomize();
		
		
	}

	void MultiplyLastColumn(int times = 2){
		for (int i = 0; i < times; ++i)
		columns [columns.Count - 1] += columns [columns.Count - 1];

		columns [columns.Count - 1] = "█ " + columns [columns.Count - 1] + " █";
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
		//	return;
		
		base.Redraw (offx, offy);
		XX -= Time.unscaledDeltaTime * 20;
		int X = (int)XX;
		
		//if (X < 0) X += SHGUI.current.resolutionX;
		int Y = 20;
		for (int i = 0; i < columns.Count - 1; ++i){
			for (int j = 0; j < columns[i].Length; ++j){
				//if (columns[i][j] != ' ')
				int startx = mapxcol(X + i);
				if (startx < SHGUI.current.resolutionY - 1)
					SHGUI.current.SetPixelFront(columns[i][j], Y + j + (int)(mysin(time, startx)), startx, 'w');
			}
		}
	}
	
	int mapxcol(int x){
		int A = x;
		while (A < 1){
			A+= SHGUI.current.resolutionX - 1;
		}
		return A;
	}
	
	int speed = 0;
	int height = 0;
	void Randomize(){
		speed = UnityEngine.Random.Range (10, 20);
		height = (int)( UnityEngine.Random.Range (2, 5));
		height = 15;
	}
	
	float mysin(float t, int i){

		return (Mathf.Sin(t * 4) * height * Mathf.Sin(16 * t + (float)(i * Math.PI / 32)));
	}
}


