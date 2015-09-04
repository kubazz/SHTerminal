using System;
using UnityEngine;
using System.Collections.Generic;


class square{
	public APPsquare appref;

	public int X1;
	public int Y1;
	public int width;
	public int height;
	public float rotation;

	public string chars;
	public string colors;

	public int index;

	public float timer;
	public bool del;

	public bool ticker = false;

	public square(APPsquare appref, int X1, int Y1, int width, int height, float rotation, string chars, string colors){
		this.appref = appref;

		this.X1 = X1;
		this.Y1 = Y1;
		this.width = width;
		this.height = height;
		this.chars = chars;
		this.colors = colors;

		this.rotation = rotation;

		this.del = false;
		this.index = 0;
		this.timer = .2f;

		ticker = UnityEngine.Random.value > .5f;
	}

	public void Update(){
		timer -= Time.unscaledDeltaTime;
		if (timer < 0) {
			timer = .05f;

			//width++;
			
			//height++;

			rotation += .03f;

			index++;

			if ((index > (chars.Length - 1)) || (index > (colors.Length - 1))){
				//del = true;
				index = 0;
			}
		}
	}

	public void Draw(){
		if (!del) {

			float W = width;
			float H = height;

			var A = getRotatedVertices(X1, Y1, (int)W, (int)H, rotation);
			var B = getRotatedVertices(X1, Y1, (int)W, (int)-H, rotation);
			var C = getRotatedVertices(X1, Y1, (int)-W, (int)-H, rotation);
			var D = getRotatedVertices(X1, Y1, (int)-W, (int)H, rotation);

			drawLine(A,B);
			drawLine(B,C);
			drawLine(C,D);
			drawLine(D,A);
		}
	}

	void drawLine(Vector2 a, Vector2 b){
		appref.DrawLine((int)a.x, (int)a.y, (int)b.x, (int)b.y, chars[index], colors[index]);
	}

	private Vector2 getRotatedVertices(int centerx, int centery, int offx, int offy, float angle){
		float X = offx * Mathf.Cos (angle) - offy * Mathf.Sin (angle);
		float Y = offx * Mathf.Sin (angle) + offy * Mathf.Cos (angle);

		X += centerx;
		Y += centery;

		return new Vector2 (X, Y);
	}
	
}


public class APPsquare: SHGUIappbase
{
	List<square> squares;

	public APPsquare():base("square-app-by-3.14")
	{
		squares = new List<square> ();
	}

	float timer = 0;
	float rot = 0;
	public override void Update(){
		base.Update();

		timer -= Time.unscaledDeltaTime;
		if (squares.Count < 1 && timer < 0) {
			timer = .5f;

			int X1 = UnityEngine.Random.Range (0, SHGUI.current.resolutionX);
			int Y1 = UnityEngine.Random.Range (0, SHGUI.current.resolutionY);
			int X2 = UnityEngine.Random.Range (0, SHGUI.current.resolutionX);
			int Y2 = UnityEngine.Random.Range (0, SHGUI.current.resolutionY);

			squares.Add(new square(this, 30, 12, 5, 5, rot, "░░░▒▒▒▓▓▓███▓▓▓▒▒▒░░░", "wwwwwwwwwwwwwwwwwwwwwwwwwwww"));
			rot += 0.01f * Mathf.PI;
		}

		for (int i = 0; i < squares.Count; ++i) {
			squares[i].Update();
			if (squares[i].del == true){
				squares.RemoveAt(i);
				i--;
			}
		}
	}

	public override void Redraw(int offx, int offy){

		base.Redraw (offx, offy);


		for (int i = 0; i < squares.Count; ++i) {
			squares[i].Draw();
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


