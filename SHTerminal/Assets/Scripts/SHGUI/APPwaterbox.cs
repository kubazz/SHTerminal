//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;

public class APPwaterbox: SHGUIappbase
{
	int[] cells1;
	int[] cells2;
	bool flip = true;
	int width;
	int height;
	
	public APPwaterbox ():base("waterbox-by-3.14-(click-to-place/remove-rock)")
	{
		width = SHGUI.current.resolutionX - 2;
		height = SHGUI.current.resolutionY - 2;
		
		ClearCells(ref cells1);
		ClearCells(ref cells2);
		
		allowCursorDraw = true;	
		
		GenerateLevel();
	}
	
	private void GenerateLevel(){
		
		int marginx = 20;
		int marginy = 5;
		for (int i = 0; i < 20; ++i){
			PlacePlatform((int)UnityEngine.Random.Range(marginx, width - marginx), (int)UnityEngine.Random.Range(marginy, height - marginy));
		}
		
		PlacePlatform(39, 10);
		
		
	}
	
	private void PlacePlatform(int x, int y){
		SetCell(ref cells1, x, y, 2); 
		SetCell(ref cells1, x + 1, y, 2);
		SetCell(ref cells1, x + 2, y, 2);
		SetCell(ref cells1, x - 1, y, 2);
		SetCell(ref cells1, x - 2, y, 2);
		
		SetCell(ref cells2, x, y, 2); 
		SetCell(ref cells2, x + 1, y, 2);
		SetCell(ref cells2, x + 2, y, 2);
		SetCell(ref cells2, x - 1, y, 2);
		SetCell(ref cells2, x - 2, y, 2);
		
	}
	
	private void ClearCells(ref int[] C){
		C = new int[width * height];
		for (int x = 0; x < width; ++x){
			for (int y = 0; y < height; ++y){
				C[x + y * width] = 0;
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
	public override void Update(){
		base.Update();
		
		if (fade < .99f)
			return;
		
		if (!FixedUpdater(.1f))
			return;
		
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
				
				if (GetCell(C1, x, y) == 1){
					if (GetCell(C1, x, y + 1) == 0 && GetCell(C2, x, y + 1) == 0){
						SetCell(ref C2, x, y + 1, 1);
						SetCell(ref C1, x, y, 0);
						continue;
					}
					if (GetCell(C1, x + 1, y + 1) == 0 && GetCell(C2, x + 1, y + 1) == 0){
						SetCell(ref C2, x + 1, y + 1, 1);
						SetCell(ref C1, x, y, 0);
						continue;
					}
					if (GetCell(C1, x - 1, y + 1) == 0 && GetCell(C2, x - 1, y + 1) == 0){
						SetCell(ref C2, x - 1, y + 1, 1);
						SetCell(ref C1, x, y, 0);
						continue;
					}
					
					if (GetCell(C1, x - 1, y) == 0 && GetCell(C2, x - 1, y) == 0){
						SetCell(ref C2, x - 1, y, 1);
						SetCell(ref C1, x, y, 0);
						continue;
					}
					if (GetCell(C1, x + 1, y) == 0 && GetCell(C2, x + 1, y) == 0){
						SetCell(ref C2, x + 1, y, 1);
						SetCell(ref C1, x, y, 0);
						continue;
					}
					
					
					SetCell(ref C2, x, y, 1);
				}
				else if (GetCell(C1, x, y) == 2){
					SetCell(ref C2, x, y, 2);
				}
			}
		}
		
		sandtimer--;
		if (sandtimer < 0){
			SetCell(ref C2, 39, 1, 1);			
			sandtimer = 2;
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
	}
	
	private void DrawCells(int[] C){
		for (int x = 0; x < width; ++x){
			for (int y = 0; y < height; ++y){
				char ch = ' ';
				char col = 'w';
				int cel = GetCell(C, x, y);
				if (cel == 1){
					ch = '░';
					col = 'r';
				}
				else if (cel == 2){
					ch = '█';
					col = 'w';
				}
				
				if (ch != ' ')
					SHGUI.current.SetPixelFront(ch, x + 1, y + 1, col);
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
				if (GetCell(cells2, x, y) == 1){
					SetCell(ref cells2, x, y, 0);
				}
			}
		}
	}
	
	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
	{	
		if (fadingOut)
			return;
		
		if (clicked){
			int setint = 2;
			if (GetCell(cells1, x - 1, y - 1) == 2) setint = 0;
			
			SetCell(ref cells2, x - 1, y - 1, setint);
			SetCell(ref cells1, x - 1, y - 1, setint);
		}
	}
}

