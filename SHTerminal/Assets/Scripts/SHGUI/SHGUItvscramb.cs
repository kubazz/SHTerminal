using System;
using System.Collections.Generic;
using System.Text;


public class SHGUItvscramb: SHGUIview
{
	int midx;
	int midy;
	int halfwidth;
	int halfheight;

	int currentwidth = 0;
	int currentheight = 0;

	//string scrabmleSource = "█▀▄█▌░▒▓■▪";
	string scramble = "█▓▒░▒▓";
	
	int scrambleIndex = 0;

	public bool reverse = false;
	public bool onlypulse = false;

	//mode is 0 for front, 1 for back, 2 for both
	public SHGUItvscramb (int midx, int midy, int halfwidth, int halfheight)
	{
		Init ();

		this.midx = midx;
		this.midy = midy;
		this.halfwidth = halfwidth;
		this.halfheight = halfheight;

		color = 'z';
	}

	public SHGUItvscramb SetReverse(){
		reverse = true;

		currentwidth = halfwidth;
		currentheight = halfheight;

		SetDelay (.2f);

		return this;
	}

	public SHGUItvscramb SetOnlyPulse(){
		SetReverse ();
		onlypulse = true;
		return this;
	}

	public SHGUItvscramb SetDelay(float delay){
		tickTimer = delay;
		return this;
	}

	float tickTimer = 0;
	float animTimer = 0;
	public override void Update(){
		if (!onlypulse) tickTimer -= UnityEngine.Time.unscaledDeltaTime;
		animTimer -= UnityEngine.Time.unscaledDeltaTime;
		

		if (tickTimer < 0) {
			tickTimer = .005f;

			if (!reverse){
				if (currentwidth < halfwidth){
					currentwidth++;
				}
				else{
					if (currentheight < halfheight){
						currentheight++;
					}
					else{
						tickTimer = -1;

						if (animTimer < 0){
							animTimer = .1f;
							scrambleIndex++;
							if (scrambleIndex > scramble.Length - 1){
								scrambleIndex = 0;
							}
						}
						
					}
				}
			}
			else{

				if (currentheight > 0){
					currentheight--;
				}
				else{
					if (currentwidth > 0){
						currentwidth--;
					}
					else{
				
						
					}
				}
				
			}

		}

		if (animTimer < 0 && reverse){
			animTimer = .1f;
			scrambleIndex++;
			if (scrambleIndex > scramble.Length - 1){
				scrambleIndex = 0;
			}
		}

	}

	public override void Redraw (int offx, int offy)
	{
		if (hidden)
			return;
		
		base.Redraw (this.x, this.y);

		int startx = midx - currentwidth;
		int starty = midy - currentheight;
		int endx = midx + currentwidth;
		int endy = midy + currentheight;

		if (starty == endy)
			endy++;
		
		for (int x = startx; x < endx; ++x) {
			for (int y = starty; y < endy; ++y){
				SHGUI.current.SetPixelFront(scramble[scrambleIndex], this.x + x + offx, this.y + y + offy, color); 
			}
		}
	}
	
}


