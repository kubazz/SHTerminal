using System;
using UnityEngine;
using System.Collections.Generic;

class line{
	public APProtolines appref;

	public int X1;
	public int Y1;
	public int X2;
	public int Y2;

	public string chars;
	public string colors;

	public int index;

	public float timer;
	public bool del;

	public bool ticker = false;

	public line(APProtolines appref, int X1, int Y1, int X2, int Y2, string chars, string colors){
		this.appref = appref;

		this.X1 = X1;
		this.Y1 = Y1;
		this.X2 = X2;
		this.Y2 = Y2;
		this.chars = chars;
		this.colors = colors;

		this.del = false;
		this.index = 0;
		this.timer = .2f;

		ticker = UnityEngine.Random.value > .5f;
	}

	public void Update(){
		timer -= Time.unscaledDeltaTime;
		if (timer < 0) {
			timer = .05f;

			X1--;
			X2++;
			Y1--;
			Y2++;

			index++;

			if ((index > (chars.Length - 1)) || (index > (colors.Length - 1))){
				del = true;
			}
		}
	}

	public void Draw(){
		if (!del)
			appref.DrawLine (X1, Y1, X2, Y2, chars [index], colors [index]);
	}
	
}

public class APProtolines: SHGUIappbase
{
	List<line> lines;

	public APProtolines():base("lines-app-by-3.14")
	{
		lines = new List<line> ();
	}

	float timer = 0;
	public override void Update(){
		base.Update();

		timer -= Time.unscaledDeltaTime;
		if (lines.Count < 10 && timer < 0) {
			timer = .2f;

			int X1 = UnityEngine.Random.Range (0, SHGUI.current.resolutionX);
			int Y1 = UnityEngine.Random.Range (0, SHGUI.current.resolutionY);
			int X2 = UnityEngine.Random.Range (0, SHGUI.current.resolutionX);
			int Y2 = UnityEngine.Random.Range (0, SHGUI.current.resolutionY);
			
			lines.Add(new line(this, X1, Y1, X2, Y2, " .,;!l%$#@#$%l!;,. ", "wwwwwwwwwwwwwwwwwwwwwwwwwwww"));
		}

		for (int i = 0; i < lines.Count; ++i) {
			lines[i].Update();
			if (lines[i].del == true){
				lines.RemoveAt(i);
				i--;
			}
		}
	}

	public override void Redraw(int offx, int offy){

		base.Redraw (offx, offy);

		for (int i = 0; i < lines.Count; ++i) {
			lines[i].Draw();
		}

		APPFRAME.Redraw (offx, offy);

	}

	public void DrawLine(int x,int y,int x2, int y2, char c, char color) {
		int w = x2 - x ;
		int h = y2 - y ;
		int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0 ;
		if (w<0) dx1 = -1 ; else if (w>0) dx1 = 1 ;
		if (h<0) dy1 = -1 ; else if (h>0) dy1 = 1 ;
		if (w<0) dx2 = -1 ; else if (w>0) dx2 = 1 ;
		int longest = Mathf.Abs(w) ;
		int shortest = Mathf.Abs(h) ;
		if (!(longest>shortest)) {
			longest = Mathf.Abs(h) ;
			shortest = Mathf.Abs(w) ;
			if (h<0) dy2 = -1 ; else if (h>0) dy2 = 1 ;
			dx2 = 0 ;            
		}
		int numerator = longest >> 1 ;
		for (int i=0;i<=longest;i++) {
			SHGUI.current.SetPixelFront(c, x, y, color);
			numerator += shortest ;
			if (!(numerator<longest)) {
				numerator -= longest ;
				x += dx1 ;
				y += dy1 ;
			} else {
				x += dx2 ;
				y += dy2 ;
			}
		}
	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;
		
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView ();
		
		if (key == SHGUIinput.enter) {
		//	SHGUI.current.PopView ();
		}
	}
	
	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
	{	
		if (fadingOut)
			return;
		
		if (clicked)
			SHGUI.current.PopView ();
	}
}


