using System;
using UnityEngine;

public class SHGUIoutflow: SHGUIview
{
	int[] cells1;
	int[] cells2;
	bool flip = true;
	int width;
	int height;
	
	public SHGUIoutflow ()
	{
		width = SHGUI.current.resolutionX - 2;
		height = SHGUI.current.resolutionY - 2;
		
		ClearCells(ref cells1);
		ClearCells(ref cells2);
		
		allowCursorDraw = true;	
		
		GenerateLevel();
	}

	int sourcex = 32;
	int sourcey = 8;

	SHGUIview debugFrame;
	private void GenerateLevel(){
		DrawFrame (-1 + 27, -1 + 9, 12 + 27, 7 + 7);
	}

	void DrawFrame(int startx, int starty, int endx, int endy){
		sourcex = endx - 1;
		sourcey = endy - 1;
		
		//debugFrame = this.AddSubView(new SHGUIframe (startx, starty, endx, endy, 'r'));
		
		DrawRect (startx, starty, endx, endy, 3);
		DrawRect (startx + 1, starty + 1, endx - 1, endy - 1, 0);

	}	
	
	void DrawRect(int startx, int starty, int endx, int endy, int cell){
		for (int X = startx; X <= endx; X++) {
			for (int Y = starty; Y <= endy; Y++) {
				SetCell(ref cells1, X, Y, cell); 
				SetCell(ref cells2, X, Y, cell);
			}
		}
	}
	
	private void ClearCells(ref int[] C){
		C = new int[width * height];
		for (int x = 0; x < width; ++x){
			for (int y = 0; y < height; ++y){
				C[x + y * width] = -1;
			}
		}
	}
	
	private void SetCell(ref int[] C, int x, int y, int value){
		if (x < 0) return;
		else if (x >= width) return;
		
		if (y < 0) return;
		else if (y >= height) return;
		
		C[x + y * width] = value;
	}
	
	private void SetCellOverflow(ref int[] C, int x, int y, int value){
		if (x < 0) return;
		else if (x >= width) return;
		
		if (y < 0) return;
		else if (y >= height) {
			//y -= height;
			//SetCell(ref C, x, y, value);
			return;
		}
		
		C[x + y * width] = value;
	}
	
	private int GetCell(int[] C, int x, int y){
		if (x < 0) return 0;
		else if (x >= width) return 0;
		
		if (y < 0) return 0;
		else if (y >= height) return 0;
		
		return C[x + y * width];
	}
	
	int sandtimer = 0;
	bool clicker = false;

	public void AddSeeds(){
		SetCell(ref cells1, sourcex, sourcey, 2);
		SetCell(ref cells2, sourcex, sourcey, 2);
	}

	public void DestroyBarrier(){
		SetCell(ref cells1, 26, 11, 0); 
		SetCell(ref cells2, 26, 11, 0);
	}

	public void AddLimit(){
		SetCell(ref cells2, sourcex, sourcey, 4);
		SetCell(ref cells2, sourcex, sourcey, 4);
	}

	public override void Update(){
		base.Update();
		
		if (fade < .99f)
			return;

		
		if (!FixedUpdater(.064f))
			return;



		for (int i = 0; i < 1; ++i) {
			UpdateCells();
		}



	}

	void UpdateCells(){
		int[] C1;
		int[] C2;
		
		if (flip){
			cells2 = new int[width * height];
			C1 = cells1;
			C2 = cells2;
		}
		else{
			cells1 = new int[width * height];
			C2 = cells1;
			C1 = cells2;
		}
		
		for (int x = 0; x < width; ++x){
			for (int y = 0; y < height; ++y){
				
				int cel = GetCell(C1, x, y);
				if (cel == 1 || cel == 2){
					SetCell(ref C1, x, y, cel);
					
					if (GetCell(C1, x + 1, y) != 3 && GetCell(C1, x + 1, y) != 4 && GetCell(C1, x + 1, y) != 2 && GetCell(C1, x + 1, y) != 1 && UnityEngine.Random.value > .25f) SetCell(ref C2, x + 1, y, (UnityEngine.Random.value > .5f)?(2):(1));
					if (GetCell(C1, x - 1, y) != 3 && GetCell(C1, x - 1, y) != 4 && GetCell(C1, x - 1, y) != 2 && GetCell(C1, x - 1, y) != 1 && UnityEngine.Random.value > .25f) SetCell(ref C2, x - 1, y, (UnityEngine.Random.value > .5f)?(2):(1));
					if (GetCell(C1, x, y + 1) != 3 && GetCell(C1, x, y + 1) != 4 && GetCell(C1, x, y + 1) != 2 && GetCell(C1, x, y + 1) != 1 && UnityEngine.Random.value > .25f) SetCell(ref C2, x, y + 1, (UnityEngine.Random.value > .5f)?(2):(1));
					if (GetCell(C1, x, y - 1) != 3 && GetCell(C1, x, y - 1) != 4 && GetCell(C1, x, y - 1) != 2 && GetCell(C1, x, y - 1) != 1 && UnityEngine.Random.value > .25f) SetCell(ref C2, x, y - 1, (UnityEngine.Random.value > .5f)?(2):(1));
					
					SetCell(ref C2, x, y, cel);
				}
				else if (GetCell(C1, x, y) == 3){
					SetCell(ref C2, x, y, 3);
				}
			}
		}
		
		for (int x = 0; x < width; ++x){
			for (int y = 0; y < height; ++y){
				
				int cel = GetCell(C1, x, y);
				if (cel == 4){
					SetCell(ref C1, x, y, cel);
					
					if (GetCell(C1, x + 1, y) != 3 && UnityEngine.Random.value > .25f) SetCell(ref C2, x + 1, y, 4);
					if (GetCell(C1, x - 1, y) != 3 && UnityEngine.Random.value > .25f) SetCell(ref C2, x - 1, y, 4);
					if (GetCell(C1, x, y + 1) != 3 && UnityEngine.Random.value > .25f) SetCell(ref C2, x, y + 1, 4);
					if (GetCell(C1, x, y - 1) != 3 && UnityEngine.Random.value > .25f) SetCell(ref C2, x, y - 1, 4);
					
					SetCell(ref C2, x, y, cel);
				}
			}
		}
		
		flip = !flip;
	}
	
	public override void Redraw(int offx, int offy){
		base.Redraw(offx, offy);
		
		DrawCells(cells1);
		DrawCells(cells2);
		
		if (!flip){
			DrawCells(cells1);
		}
		else{
			DrawCells(cells2);
		}

		if (debugFrame != null)
			debugFrame.Redraw (offx, offy);
	}
	
	private void DrawCells(int[] C){
		for (int x = 0; x < width; ++x){
			for (int y = 0; y < height; ++y){
				char ch = ' ';
				char col = 'w';
				int cel = GetCell(C, x, y);
				if (cel == 1){
					ch = '0';
					col = 'r';
				}
				else if (cel == 2){
					ch = '1';
					col = 'r';
				}
				else if (cel == 3){
					//ch = '#';
					//col = 'z';
				}
				
				if (ch != ' ')
					SHGUI.current.SetPixelFront(ch, x, y, col);
			}
		}
	}
	
	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;
		
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView ();
		
		if (key == SHGUIinput.enter){
			ClearMovingCells();
		}
	}
	
	private void ClearMovingCells(){
		for (int x = 0; x < width; ++x){
			for (int y = 0; y < height; ++y){
				if (GetCell(cells1, x, y) == 1){
					SetCell(ref cells1, x, y, 0);
				}
				if (GetCell(cells2, x, y) == 2){
					SetCell(ref cells2, x, y, 0);
				}
			}
		}
	}
}

