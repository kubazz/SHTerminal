using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SHGUIsprite : SHGUIview {

	public List<string> frames;

	public int currentFrame;

	public float animationSpeed = 0;
	public float currentAnimationTimer = 0;

	public bool killOnAnimationComplete = false;
	public bool loops = false;

	// Use this for initialization
	public SHGUIsprite (){
		Init ();
		frames = new List<string> ();
	}

	public SHGUIsprite AddFrame(string frame){
		frames.Add (frame);
		return this;
	}

	public SHGUIsprite AddFramesFromFile(string filename, int rowsPerFrame){
		string art = SHGUI.current.GetASCIIartByName (filename);

		string[] rows = art.Split ('\n');

		int row = 0;
		string str = "";
		for (int i = 0; i < rows.Length; ++i) {
			str += rows[i] + "\n";
			row++;
			if (row > rowsPerFrame - 1){
				row = 0;
				AddFrame(str);
				str = "";
			}
		}

		return this;
	}

	public SHGUIsprite AddSpecyficFrameFromFile(string filename, int rowsPerFrame, int addFrame){
		string art = SHGUI.current.GetASCIIartByName (filename);
		
		string[] rows = art.Split ('\n');
		
		int row = 0;
		string str = "";
		int count = 0;
		for (int i = 0; i < rows.Length; ++i) {
			str += rows[i] + "\n";
			row++;
			if (row > rowsPerFrame - 1){
				row = 0;
				if (addFrame == count) AddFrame(str);
				str = "";

				count++;
			}
		}
		
		return this;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
		
		if (fade < .999f) {
			return;
		}
		currentAnimationTimer += Time.unscaledDeltaTime;
		if (currentAnimationTimer > animationSpeed && (animationSpeed > 0)) {
			currentAnimationTimer -= animationSpeed;
			currentFrame++;
			if (currentFrame >= frames.Count) {
				if (loops)
					currentFrame = 0;
				else
					currentFrame = frames.Count - 1;
			}
		}

	}

	public void RedrawBlack(int offx, int offy){
		SHGUI.current.DrawBlack (frames[currentFrame], x + offx, y + offy);
	}

	public override void Redraw(int offx, int offy){
		base.Redraw (offx, offy);

		if (frames.Count > 0)
			SHGUI.current.DrawTextSkipSpaces (frames[currentFrame], x + offx, y + offy, color, fade, ' ');
	}
}
