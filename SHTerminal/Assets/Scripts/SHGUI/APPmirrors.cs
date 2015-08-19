
using System.Collections.Generic;
using UnityEngine;

class laser{
	public laser(int X, int Y, int DirX, int DirY, char Col){
		x = X;
		y = Y;
		dirx = DirX;
		diry = DirY;
		col = Col;
		remove = false;
	}

	public int x;
	public int y;
	public int dirx;
	public int diry;
	public char col;
	
	public bool remove;

	public void update(){
		x = x + dirx;
		y = y + diry;

		if (x < 1)
			remove = true;
		if (y < 1)
			remove = true;
		if (x >= SHGUI.current.resolutionX - 1)
			remove = true;
		if (y >= SHGUI.current.resolutionY - 1)
			remove = true;
	}

	public void collide(mirror m){
		if ((m.x == this.x) && (m.y == this.y)) {
			if (m.c == '/'){
				if (dirx != 0){
					diry = -dirx; dirx = 0; 
				}
				else if (diry != 0){
					dirx = -diry; diry = 0;
				}

				//m.col = this.col;
				m.col = 'w';
				m.flip();

				SHGUI.current.PlaySound(SHGUIsound.ping);
			}
			else if (m.c == '\\'){
				if (dirx != 0){
					diry = dirx; dirx = 0;
				}
				else if (diry != 0){
					dirx = diry; diry = 0;
				}

				//m.col = this.col;
				m.col = 'w';
				m.flip();

				SHGUI.current.PlaySound(SHGUIsound.pong);		
			}
		}
	}

	public void draw(){
		if ((dirx == 1)&&(diry == 0))
			SHGUI.current.SetPixelFront('→', x, y, col);
		else if ((dirx == -1)&&(diry == 0))
			SHGUI.current.SetPixelFront('←', x, y, col);
		else if ((dirx == 0)&&(diry == 1))
			SHGUI.current.SetPixelFront('↓', x, y, col);
		else if ((dirx == 0)&&(diry == -1))
			SHGUI.current.SetPixelFront('↑', x, y, col);
		
		//SHGUI.current.SetPixelBack ('█', x, y, col);
	}
}

class mirror{
	public mirror(int X, int Y, char C, char Col){
		x = X;
		y = Y;
		c = C;
		col = Col;
	}

	public int x;
	public int y;
	public char c;
	public char col;
	public bool remove = false;
	

	public void flip(){
		if (Random.value > 0.25f) return;
		if (c == '/')
			c = '\\';
		else if (c == '\\')
			c = '/';
	}

	public void draw(){
		SHGUI.current.SetPixelFront(c, x, y, col);
	}
}

public class APPmirrors: SHGUIappbase
{
	private List<laser> lasers;
	private List<mirror> mirrors;

	public APPmirrors (): base("mirrors-app-by-3.14")
	{
		//allowCursorDraw = true;
		lasers = new List<laser> ();
		mirrors = new List<mirror> ();


		for (int i = 0; i < 200; ++i) {
			mirrors.Add(new mirror((int)Random.Range(10, SHGUI.current.resolutionX - 10),
			                       (int)Random.Range(2, SHGUI.current.resolutionY - 2), '/', 'z'));
		}

		//remove doubles
		for (int i = 0; i < mirrors.Count; ++i){
			for (int j=i; j < mirrors.Count; ++j){
				if (mirrors[i].x == mirrors[j].x && mirrors[i].y == mirrors[j].y){
					if (i != j){
						mirrors[i].c = '#';	
						mirrors[i].remove = true;
					}
				}
			}
		}

		//ClearRoadFor (5);
		ClearRoadFor (11);
		//ClearRoadFor (15);
	}

	void ClearRoadFor(int y){
		for (int i = 0; i < mirrors.Count; ++i){
			if (mirrors[i].x < 20){
				if (mirrors[i].y == y){
					mirrors.RemoveAt(i);
					i--;
				}
			}
		}
	}

	int waiter = 0;
	public override void Update(){
		base.Update ();

		if (!FixedUpdater ())
			return;

		for (int i = 0; i < lasers.Count; ++i){
			lasers[i].update();

			for (int j=0; j < mirrors.Count; ++j){
				lasers[i].collide(mirrors[j]);
			}
		}

		for (int i = 0; i < lasers.Count; ++i){
			
			if (lasers[i].remove){
				lasers.RemoveAt(i);
				i--;
			}
		}

		for (int i=0; i < mirrors.Count; ++i){
			if (mirrors[i].remove) {
				mirrors.RemoveAt(i);
				i--;
			}
		}

		waiter--;
		if (waiter < 0){
			//lasers.Add (new laser (1, 5, 1, 0, 'r'));
			lasers.Add (new laser (1, 11, 1, 0, 'r'));
			//lasers.Add (new laser (1, 15, 1, 0, 'b'));
			
			waiter = 10;
		}
		
	}
	
	public override void Redraw(int offx, int offy){
		
		base.Redraw (offx, offy);

		
		for (int i = 0; i < lasers.Count; ++i){
			lasers[i].draw();
		}

		for (int i = 0; i < mirrors.Count; ++i){
			mirrors[i].draw();
		}



	}
}


