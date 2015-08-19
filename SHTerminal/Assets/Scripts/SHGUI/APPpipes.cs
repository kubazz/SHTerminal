using System;
using System.Collections.Generic;


public class APPpipes: SHGUIappbase
{
	class snake{
		List<int> x;
		List<int> y;

		public bool dead = false;

		public snake(int X, int Y, int Len){
			x = new List<int>();
			y = new List<int>();

			for (int i = 0; i < Len; ++i){
				x.Add(X);
				y.Add(Y);
			}

			RandomizeDirection(true);
		}

		int dirx = 1;
		int diry = 0;

		public void Update(){

			x.RemoveAt(0);
			y.RemoveAt(0);

			x.Add(x[x.Count - 1] + dirx);
			y.Add(y[y.Count - 1] + diry);

			if (UnityEngine.Random.value > 0.8f){
				RandomizeDirection();
			}

			if ((x[x.Count - 1] < -x.Count) || (x[x.Count - 1] > (SHGUI.current.resolutionX + x.Count)))
				dead = true;

			if ((y[x.Count - 1] < -y.Count) || (y[x.Count - 1] > (SHGUI.current.resolutionY + y.Count)))
				dead = true;

		}

		void RandomizeDirection(bool allowTurnbac = false){
			int prevdirx = dirx;
			int prevdiry = diry;

			if (UnityEngine.Random.value > 0.5f){
				dirx = 1;
				diry = 0;
			}
			else{
				dirx = 0;
				diry = 1;
			}

			if (UnityEngine.Random.value > 0.75f){
				dirx = -dirx;
				diry = -diry;
			}

			if (prevdirx == -dirx && prevdiry == -diry){
				dirx = prevdirx;
				diry = prevdiry;
			}
		}

		public void Draw(bool cornermode = false){
			
			for (int i = 0; i < x.Count; ++i){
				if ((x[i] != 0 && x[i] != SHGUI.current.resolutionX - 1)&&
					(y[i] != 0 && y[i] != SHGUI.current.resolutionY - 1)){

					int X = x[i];
					int Y = y[i];
					if (i == 0){
					//	SHGUI.current.SetPixelFront(' ', x[i], y[i], 'z');
					}
					else if (i == x.Count - 1){
					//	SHGUI.current.SetPixelFront(' ', x[i], y[i], 'z');
					}
					else{
						int X0 = x[i - 1];
						int Y0 = y[i - 1];

						int X1 = x[i];
						int Y1 = y[i];

						int X2 = x[i + 1];
						int Y2 = y[i + 1];

						char currentPixel = SHGUI.current.GetPixelFront(x[i], y[i]);

						if (!cornermode){
							if (X0 == X2 && Y0 != Y2){
								if (currentPixel == '═' || currentPixel == '╬' || currentPixel == '╦' || currentPixel == '╩'){
									SHGUI.current.SetPixelFront('╬', x[i], y[i], 'w');
								}
								else if (currentPixel == '╚' || currentPixel == '╠'){
									SHGUI.current.SetPixelFront('╠', x[i], y[i], 'w');
								}
								else if (currentPixel == '╔' || currentPixel == '╠'){
									SHGUI.current.SetPixelFront('╠', x[i], y[i], 'w');
								}
								else if (currentPixel == '╝' || currentPixel == '╣'){
									SHGUI.current.SetPixelFront('╣', x[i], y[i], 'w');
								}
								else if (currentPixel == '╗' || currentPixel == '╣'){
									SHGUI.current.SetPixelFront('╣', x[i], y[i], 'w');
								}
								else{
									SHGUI.current.SetPixelFront('║', x[i], y[i], 'z');
								}
								continue;
							}
							if (Y0 == Y2){
								if (currentPixel == '║' || currentPixel == '╬' || currentPixel == '╠' || currentPixel == '╣'){
									SHGUI.current.SetPixelFront('╬', x[i], y[i], 'w');
								}
								else if (currentPixel == '╚' || currentPixel == '╩'){
									SHGUI.current.SetPixelFront('╩', x[i], y[i], 'w');
								}
								else if (currentPixel == '╔' || currentPixel == '╦'){
									SHGUI.current.SetPixelFront('╦', x[i], y[i], 'w');
								}
								else if (currentPixel == '╝' || currentPixel == '╩'){
									SHGUI.current.SetPixelFront('╩', x[i], y[i], 'w');
								}
								else if (currentPixel == '╗' || currentPixel == '╦'){
									SHGUI.current.SetPixelFront('╦', x[i], y[i], 'w');
								}
								else{
									SHGUI.current.SetPixelFront('═', x[i], y[i], 'z');
								}
								continue;
							}
						}
						else{
							int X2norm = X2 - X1;
							int X0norm = X0 - X1;

							int Y2norm = Y2 - Y1;
							int Y0norm = Y0 - Y1;

							int Xsum = X2norm + X0norm;
							int Ysum = Y2norm + Y0norm;


							if(Xsum==1 && Ysum==1){
								if (currentPixel == '╝')
									SHGUI.current.SetPixelFront('╬', x[i], y[i], 'w');
								else
									SHGUI.current.SetPixelFront('╔', x[i], y[i], 'z');
							}else if(Xsum==1 && Ysum==-1){
								if (currentPixel == '╗')
									SHGUI.current.SetPixelFront('╬', x[i], y[i], 'w');
								else
									SHGUI.current.SetPixelFront('╚', x[i], y[i], 'z');
							}else if(Xsum==-1 && Ysum==1){
								if (currentPixel == '╚')
									SHGUI.current.SetPixelFront('╬', x[i], y[i], 'w');
								else
									SHGUI.current.SetPixelFront('╗', x[i], y[i], 'z');
							}else if(Xsum==-1 && Ysum==-1){
								if (currentPixel == '╔')
									SHGUI.current.SetPixelFront('╬', x[i], y[i], 'w');
								else
									SHGUI.current.SetPixelFront('╝', x[i], y[i], 'z');
							}
						}
					}
				}
			}
		}
	
	}

	List<snake> snakes;

	public APPpipes (): base("pipes-app-by-3.14-&-xu")
	{
		snakes = new List<snake>();
	}

	public override void Update(){
		base.Update();

		if (!FixedUpdater()) return;

		int margin = 10;
		if (snakes.Count < 100){

			float r = UnityEngine.Random.value;
			int len = (int)UnityEngine.Random.Range(16, 48);

			if (r > .75f){
				snakes.Add(new snake((int)UnityEngine.Random.Range(margin, SHGUI.current.resolutionX - margin),
				                     0, len));
			}
			else if (r > .5f){
				snakes.Add(new snake((int)UnityEngine.Random.Range(margin, SHGUI.current.resolutionX - margin),
				                     SHGUI.current.resolutionY - 1, len));
			}
			else if (r > .25f){
				snakes.Add(new snake(SHGUI.current.resolutionX - 1,
					(int)UnityEngine.Random.Range(margin, SHGUI.current.resolutionY - margin),
				     len));
			}
			else if (r > 0f){
				snakes.Add(new snake(0,
				     (int)UnityEngine.Random.Range(margin, SHGUI.current.resolutionY - margin),
				     len));
			}
		}

		for (int i = 0; i < snakes.Count; ++i){
			snakes[i].Update();

			if (snakes[i].dead){
				snakes.RemoveAt(i);
				i--;
			}
		}


	}

	public override void Redraw(int offx, int offy){
		base.Redraw(offx, offy);

		for (int i = 0; i < snakes.Count; ++i){
			snakes[i].Draw(true);
		}

		for (int i = 0; i < snakes.Count; ++i){
			snakes[i].Draw(false);
		}
	
	}
}