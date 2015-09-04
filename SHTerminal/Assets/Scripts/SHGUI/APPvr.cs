
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;


public class APPvr : SHGUIappbase
{
	int levelWidth;
	int levelHeight;
	int[] level;
	float tileSize = 1f;

	bool drawDebug = false;

	Vector2 playerPosition;
	float playerRotation;
	float moveSpeed = .32f; 

	public APPvr(): base("vr-app-by-3.14")
    {
		playerPosition = new Vector2 ();
		playerRotation = 0;

		LoadLevel ("raycast_labyrinth");

		//AddSubView (new SHGUIrect (0, 0, SHGUI.current.resolutionX, (int)(SHGUI.current.resolutionY / 2) + 1, '3', '▒', 0));
		//AddSubView (new SHGUIrect (0, (int)(SHGUI.current.resolutionY / 2) + 1, SHGUI.current.resolutionX, (int)(SHGUI.current.resolutionY), '4', '▒', 0));		
		
    }

	public override void Update(){
	}
	
    public override void Redraw(int offx, int offy)
    {
        base.Redraw(offx, offy);

        if (fade < 0.99f)
            return;


		castRays ();

		
		if (drawDebug)
			DrawDebug ();

		/*
		for (int i = 0; i < SHGUI.current.resolutionY; ++i){
			SHGUI.current.SetPixelFront('▒', (int)(SHGUI.current.resolutionX / 2), i, 'r');
		}
		*/

		//RedrawAppGui (offx, offy);

    }

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;

		if (key == SHGUIinput.up) {
			playerPosition.x += Mathf.Cos(playerRotation) * moveSpeed;
			playerPosition.y += Mathf.Sin(playerRotation) * moveSpeed;
		}
		if (key == SHGUIinput.down) {
			playerPosition.x -= Mathf.Cos(playerRotation) * moveSpeed;
			playerPosition.y -= Mathf.Sin(playerRotation) * moveSpeed;
		}
		if (key == SHGUIinput.left) {
			playerRotation -= .1f;
		}
		if (key == SHGUIinput.right) {
			playerRotation += .1f;
		}
		
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView ();
		
		if (key == SHGUIinput.enter)
			drawDebug = !drawDebug;
	}
	
	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
	{	
		if (fadingOut)
			return;
	}



	#region RAYCASTING
	float viewDist = 20f;
	void castRays() {
		for (int i=0; i < SHGUI.current.resolutionY; i++) {
			// Where on the screen does ray go through?
			float rayScreenPos = ((int)(-SHGUI.current.resolutionY / 2) + i) * 1f;
			
			// The distance from the viewer to the point
			// on the screen, simply Pythagoras.
			float rayViewDist = Mathf.Sqrt(rayScreenPos*rayScreenPos + viewDist*viewDist);
			
			// The angle of the ray, relative to the viewing direction
			// Right triangle: a = sin(A) * c
			float rayAngle = Mathf.Asin(rayScreenPos / rayViewDist);
			rayAngle = 10 / Mathf.PI / 180;
			rayAngle *= i - (int)(SHGUI.current.resolutionY / 2);
			
			castSingleRayVertical(
				// Add the players viewing direction
				// to get the angle in world space
				0 + rayAngle,
				i
				);
		}

		/*
		for (int i=0; i < SHGUI.current.resolutionY; i++) {
			// Where on the screen does ray go through?
			float rayScreenPos = ((int)(-SHGUI.current.resolutionY / 2) + i) * 1f;
			
			// The distance from the viewer to the point
			// on the screen, simply Pythagoras.
			float rayViewDist = Mathf.Sqrt(rayScreenPos*rayScreenPos + viewDist*viewDist);
			
			// The angle of the ray, relative to the viewing direction
			// Right triangle: a = sin(A) * c
			float rayAngle = Mathf.Asin(rayScreenPos / rayViewDist);
			rayAngle = 10 / Mathf.PI / 180;
			rayAngle *= i - (int)(SHGUI.current.resolutionY / 2);
			
			
			castSingleRayVertical2(
				// Add the players viewing direction
				// to get the angle in world space
				playerRotation + rayAngle,
				i
				);
			
		}
		*/

		var stripIdx = 0;
		for (int i=0; i < SHGUI.current.resolutionX; i++) {
			// Where on the screen does ray go through?
			float rayScreenPos = ((int)(-SHGUI.current.resolutionX / 2) + i) * 1f;

			// The distance from the viewer to the point
			// on the screen, simply Pythagoras.
			float rayViewDist = Mathf.Sqrt(rayScreenPos*rayScreenPos + viewDist*viewDist);
			
			// The angle of the ray, relative to the viewing direction
			// Right triangle: a = sin(A) * c
			float rayAngle = Mathf.Asin(rayScreenPos / rayViewDist);
			rayAngle = 10 / Mathf.PI / 180;
			rayAngle *= i - (int)(SHGUI.current.resolutionX / 2);


			castSingleRay(
				// Add the players viewing direction
				// to get the angle in world space
				playerRotation + rayAngle,
				i
				);

		}

	
	}

	//string shading = "`.,-+=*%@#░▒▓█";
	string shading = "░░░░░░░░░▒▒▒▓▓▓███";
	
	//string shading = "░▒▓█▓▒░▒▓█▓▒░▒▓█";
	
	float twoPI = Mathf.PI * 2;
	void castSingleRay(float rayAngle, int column) {
		// Make sure the angle is between 0 and 360 degrees
		rayAngle %= twoPI;
		//if (rayAngle > 0) rayAngle += twoPI;
		
		// Moving right/left? up/down? Determined by
		// which quadrant the angle is in
		bool right = (rayAngle > twoPI * 0.75f || rayAngle < twoPI * 0.25f);
		bool up = (rayAngle < 0 || rayAngle > Math.PI);
		
		float angleSin = Mathf.Sin(rayAngle);
		float angleCos = Mathf.Cos(rayAngle);
		
		// The distance to the block we hit
		float dist = 0;
		// The x-coord on the texture of the block,
		// i.e. what part of the texture are we going to render
		int textureX = 1;
		// The (x,y) map coords of the block
		float wallX;
		float wallY;
		
		// First check against the vertical map/wall lines
		// we do this by moving to the right or left edge
		// of the block we’re standing in and then moving
		// in 1 map unit steps horizontally. The amount we have
		// to move vertically is determined by the slope of
		// the ray, which is simply defined as sin(angle) / cos(angle).
		
		// The slope of the straight line made by the ray
		float slope = angleSin / angleCos;
		// We move either 1 map unit to the left or right
		float dX = right ? 1 : -1;
		// How much to move up or down
		float dY = dX * slope;

		dX /= 40f;
		dY /= 40f;
		
		// Starting horizontal position, at one
		// of the edges of the current map block
		float x = right ? Mathf.Ceil(playerPosition.x) : Mathf.Floor(playerPosition.x);
		// Starting vertical position. We add the small horizontal
		// step we just made, multiplied by the slope
		float y = playerPosition.y + (x - playerPosition.x) * slope;
		
		while (x >= 0 && x < levelWidth && y >= 0 && y < levelHeight) {
			wallX = Mathf.Floor(x + (right ? 0 : -1));
			wallY = Mathf.Floor(y);
			
			// Is this point inside a wall block?
			if (GetLevelCell((int)wallX, (int)wallY) != 0) {
				var distX = x - playerPosition.x;
				var distY = y - playerPosition.y;
				// The distance from the player to this point, squared
				dist = distX*distX + distY*distY;

				int s = shading.Length - 1 - Mathf.Clamp((int)(dist / 3), 0, shading.Length - 1);
				char shade = shading[s];
				s = Mathf.Clamp(s-13, 1, 9);
				char color = (s).ToString()[0];
				int height = (int)(Mathf.Round(viewDist / Mathf.Sqrt(dist)));
				DrawColumn(shade, column, (int)(SHGUI.current.resolutionY / 2), height, color);
				//DrawRow(shade, (int)(SHGUI.current.resolutionX / 2), column, height, color);
				
				//SHGUI.current.SetPixelFront(((int)dist).ToString()[0], column, (int)(SHGUI.current.resolutionY / 2), 'r');
				break;
			}
			x += dX;
			y += dY;
		}
		
		// Horizontal run snipped,
		// basically the same as vertical run

			//if (dist)
			//	drawRay(xHit, yHit);
	}

	void DrawColumn(char c, int x, int y, int height, char color){
		for (int i = 0; i < height; ++i) {
			SHGUI.current.SetPixelFront(c, x, y + i, color);
			SHGUI.current.SetPixelFront(c, x, y - i, color);
			
		}

		//for (int i = 0; i < height * 2; ++i) {
		//}		
	}

	void castSingleRayVertical(float rayAngle, int row) {
		// Make sure the angle is between 0 and 360 degrees
		rayAngle %= twoPI;
		
		float angleSin = Mathf.Sin(rayAngle);
		float angleCos = Mathf.Cos(rayAngle);

		float slope = angleSin / angleCos;

		if (slope != 0) {
			float dist = Mathf.Abs(.5f / slope);
			dist *= dist;

			//int s = (shading.Length - 1) - (int)Mathf.Clamp((shading.Length - 1) * Mathf.Abs(slope) * 4, 0, shading.Length - 1);
			//int s = (int)Mathf.Clamp ((shading.Length - 1) * Mathf.Abs (slope) * 4 + 2, 0, shading.Length - 1);
			int s = shading.Length - 1 - Mathf.Clamp((int)(dist / 3) + 2, 0, shading.Length - 1);

			char shade = shading [s];
			s = Mathf.Clamp (s - 13, 1, 9);
			char color = (s).ToString () [0];
			//Debug.Log (shade);
			DrawRow (shade, (int)(SHGUI.current.resolutionX / 2), row, (int)(SHGUI.current.resolutionX / 2), color);
		}
	}

	void castSingleRayVertical2(float rayAngle, int column) {
		// Make sure the angle is between 0 and 360 degrees
		rayAngle %= twoPI;
		//if (rayAngle > 0) rayAngle += twoPI;
		
		// Moving right/left? up/down? Determined by
		// which quadrant the angle is in
		bool right = (rayAngle > twoPI * 0.75f || rayAngle < twoPI * 0.25f);
		bool up = (rayAngle < 0 || rayAngle > Math.PI);
		
		float angleSin = Mathf.Sin(rayAngle);
		float angleCos = Mathf.Cos(rayAngle);
		
		// The distance to the block we hit
		float dist = 0;
		// The x-coord on the texture of the block,
		// i.e. what part of the texture are we going to render
		int textureX = 1;
		// The (x,y) map coords of the block
		float wallX;
		float wallY;
		
		// First check against the vertical map/wall lines
		// we do this by moving to the right or left edge
		// of the block we’re standing in and then moving
		// in 1 map unit steps horizontally. The amount we have
		// to move vertically is determined by the slope of
		// the ray, which is simply defined as sin(angle) / cos(angle).
		
		// The slope of the straight line made by the ray
		float slope = angleSin / angleCos;
		// We move either 1 map unit to the left or right
		float dX = right ? 1 : -1;
		// How much to move up or down
		float dY = dX * slope;
		
		dX /= 40f;
		dY /= 40f;
		
		// Starting horizontal position, at one
		// of the edges of the current map block
		float x = right ? Mathf.Ceil(playerPosition.x) : Mathf.Floor(playerPosition.x);
		// Starting vertical position. We add the small horizontal
		// step we just made, multiplied by the slope
		float y = playerPosition.y + (x - playerPosition.x) * slope;
		
		while (x >= 0 && x < levelWidth && y >= 0 && y < levelHeight) {
			wallX = Mathf.Floor(x + (right ? 0 : -1));
			wallY = Mathf.Floor(y);
			
			// Is this point inside a wall block?
			if (GetLevelCell((int)wallX, (int)wallY) != 0) {
				var distX = x - playerPosition.x;
				var distY = y - playerPosition.y;
				// The distance from the player to this point, squared
				dist = distX*distX + distY*distY;
				
				int s = shading.Length - 1 - Mathf.Clamp((int)(dist / 3), 0, shading.Length - 1);
				char shade = shading[s];
				s = Mathf.Clamp(s-13, 1, 9);
				char color = (s).ToString()[0];
				int height = (int)(Mathf.Round(viewDist / Mathf.Sqrt(dist)));
				//DrawColumn(shade, column, (int)(SHGUI.current.resolutionY / 2), height, color);
				DrawRow(shade, (int)(SHGUI.current.resolutionX / 2), column, height, color);
				
				//SHGUI.current.SetPixelFront(((int)dist).ToString()[0], column, (int)(SHGUI.current.resolutionY / 2), 'r');
				break;
			}
			x += dX;
			y += dY;
		}
		
		// Horizontal run snipped,
		// basically the same as vertical run
		
		//if (dist)
		//	drawRay(xHit, yHit);
	}
	
	void DrawRow(char c, int x, int y, int width, char color){
		for (int i = 0; i < width; ++i) {
			SHGUI.current.SetPixelFront(c, x + i, y, color);
			SHGUI.current.SetPixelFront(c, x - i, y, color);
			
		}
		
		//for (int i = 0; i < height * 2; ++i) {
		//}		
	}
	#endregion

	#region LEVEL
	void LoadLevel(string levelName){
		string levelString = SHGUI.current.GetASCIIartByName (levelName);

		levelWidth = 120;
		levelHeight = 10;

		level = new int[levelWidth * levelHeight];
		int x = 0;
		int y = 0;
		for (int i = 0; i < levelString.Length; ++i){

			if (levelString[i] == '\n'){
				x = 0;
				y++;
			}
			else{
				if (levelString[i] == 'X'){
					playerPosition.x = x * tileSize + tileSize / 2;
					playerPosition.y = y * tileSize + tileSize / 2;
				}
				else if (levelString[i] != ' '){
					SetLevelCell(1, x, y);
				}
				else{
					SetLevelCell(0, x, y);
				}
			}
			x++;
		}

		//Debug.Log ("levelString: " + levelString);
	}


	void SetLevelCell(int v, int x, int y){
		if (x > (levelWidth - 1) || x < 0 || y > (levelHeight - 1) || y < 0)
			return;
		else {
			level[y + x * levelHeight] = v;
		}
	}
	
	int GetLevelCell(int x, int y){
		if (x > (levelWidth - 1) || x < 0 || y > (levelHeight - 1) || y < 0)
			return -1;
		else {
			return level[y + x * levelHeight];
		}
	}

	void DrawDebug(){
		
		for (int x = 0; x < levelWidth; ++x) {
			for (int y = 0; y < levelHeight; ++y) {
				int cell = GetLevelCell(x, y);
				if (cell != 0){
					SHGUI.current.SetPixelFront('█', x + 10, y + 5, 'z');
				}
				else{
					SHGUI.current.SetPixelFront('░', x + 10, y + 5, 'z');
				}
			}
		}

		SHGUI.current.SetPixelFront('█', (int)((playerPosition.x / tileSize)+ 10), (int)((playerPosition.y / tileSize) + 5), 'r');
	}

	#endregion
}


