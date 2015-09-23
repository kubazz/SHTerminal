
using System;
using UnityEngine;
using System.Collections.Generic;

public class APPtimber: SHGUIappbase
{

	public List<SHGUIsprite> trees;
	SHGUIsprite deadTree;
	public SHGUIsprite dude;

	bool left = true;
	bool alive = true;

	float hitTime = 0;

	public APPtimber (): base("tree-dude-tree-dude-dude-by-piotr")
	{
		//AddSubView (new SHGUIsprite ().AddFramesFromFile ("APPtimberdude", 6));


		trees = new List<SHGUIsprite> ();
		dude = new SHGUIsprite ().AddFramesFromFile("APPtimberdude", 6);
		AddSubView (dude);


		AddTreeOnTop (true);
		AddTreeOnTop (true);
		for (int i = 0; i < 10; ++i) {
			AddTreeOnTop (false);
			AddTreeOnTop (true);
		}
	}

	public override void Update ()
	{
		base.Update ();

		hitTime -= Time.unscaledDeltaTime;
		dude.y = 23-6;
		int margin = 14;
		if (left) {
			dude.x = 23 - margin;
			if (hitTime > 0)
				dude.currentFrame = 1;
			else
				dude.currentFrame = 0;

		} else {
			dude.x = 23 + margin;
			dude.currentFrame = 3;
			if (hitTime > 0)
				dude.currentFrame = 3;
			else
				dude.currentFrame = 2;
		}

		if (!alive) {
			dude.currentFrame = 4;
		}
		UpdateTrees ();

	}

	public override void Redraw (int offx, int offy)
	{
		base.Redraw (offx, offy);

		dude.Redraw (offx, offy);

		APPFRAME.Redraw (offx, offy);
		APPINSTRUCTION.Redraw (offx, offy);
		APPLABEL.Redraw (offx, offy);
		
	}

	void UpdateTrees(){
		if (deadTree != null && !deadTree.remove) {
			if (left){
				deadTree.x++;
			}
			else{
				deadTree.x--;
			}

			if (deadTree.remove){
				deadTree = null;
			}
		}

		if (trees.Count > 0) {
			if (trees[0].y < 23 - 6){
				for (int i = 0; i < trees.Count; ++i){
					trees[i].y++;
				}
			}
		}

		for (int i = 0; i < trees.Count; ++i) {
			if (trees[i].fadingOut){
				trees.RemoveAt(i);
				i--;
			}
		}
	}

	void ForceUpdateTrees(){
		if (trees.Count > 0) {
			while (trees[0].y < 23 - 6){
				for (int i = 0; i < trees.Count; ++i){
					trees[i].y++;
				}
			}
		}
	}

	void AttackTree(){
		if (trees.Count > 1) {
			ForceUpdateTrees();
			bool okay = true;
			if (trees[0].currentFrame == 1 && !left) okay = false; 
			if (trees[0].currentFrame == 2 && left) okay = false;
			
			if (okay){
				if (trees.Count < 10){
					AddTreeOnTop(false);
					AddTreeOnTop (true);
				}
				trees [0].Kill ();
				trees[0].overrideFadeOutSpeed = .5f;
				deadTree = trees[0];
				if (trees[1].currentFrame == 1 && !left) okay = false; 
				if (trees[1].currentFrame == 2 && left) okay = false;
				if (!okay){
					alive = false;
				}
			}
			else{
				alive = false;
			}
		}
	}

	void AddTreeOnTop(bool forceBlank){
		SHGUIsprite s = new SHGUIsprite ().AddFramesFromFile ("APPtimbertrees", 6);
		s.x = 12;
		if (trees.Count > 0) {
			s.y = trees [trees.Count - 1].y - 6;
		} else {
			s.y = 23 - 6;
		}
		if (!forceBlank) {
			s.currentFrame = UnityEngine.Random.Range (0, 5);
			if (s.currentFrame == 3) s.currentFrame = 1;
			if (s.currentFrame == 4) s.currentFrame = 2;
		}
		else
			s.currentFrame = 0;
		AddSubView(s);
		trees.Add (s);
	}
	
	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;

		if (alive) {
			if (key == SHGUIinput.left) {
				hitTime = .1f;
				left = true;

				AttackTree();
			}
			if (key == SHGUIinput.right) {
				hitTime = .1f;
				left = false;

				AttackTree();
			}
		}
		
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView ();
		
		if (key == SHGUIinput.enter)
			SHGUI.current.PopView ();
	}
	
	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll){	
		if (fadingOut)
			return;

	}
}

