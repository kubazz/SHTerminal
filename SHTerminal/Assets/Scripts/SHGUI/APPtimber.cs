
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

	int score = 0;
	int lastdisplayedscore = -1;

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

		timerBar = new SHGUIprogressbar (20, 3, 21, "", "");
		timerBar.SetStyle ("w█w░z█");
		AddSubView (timerBar);
	}

	public override void Update ()
	{
		base.Update ();
		hitTime -= Time.unscaledDeltaTime;
		dude.y = 23-6;
		int margin = 14;
		if (left) {
			dude.x = 23 - margin;
			if (hitTime > 0){
				dude.currentFrame = 1;
				dude.y -= 1;
			}
			else
				dude.currentFrame = 0;

		} else {
			dude.x = 23 + margin;
			dude.currentFrame = 3;
			if (hitTime > 0){
				dude.y -= 1;
				dude.currentFrame = 3;
			}
			else
				dude.currentFrame = 2;
		}

		if (!alive) {
			dude.currentFrame = 4;
		}
		UpdateTrees ();
		DrawScore ();
		DrawProgressBar ();

		guiHidingTimer -= Time.unscaledDeltaTime;
		if (guiHidingTimer < 0) {
			guiHidingTimer = .0f;

			int diff = desiredGuiHideOffset - guiHideOffset;
			diff = Mathf.Clamp(diff, -1, 1);

			guiHideOffset += diff;
		}

		if (!alive) {
			desiredGuiHideOffset = 40;
		} else {
			desiredGuiHideOffset = 0;
		}

	}

	public override void Redraw (int offx, int offy)
	{
		base.Redraw (offx, offy);

		if (!alive) {
			dude.currentFrame = 5;
			dude.RedrawBlack(offx, offy);
			dude.currentFrame = 4;
		}
		dude.Redraw (offx, offy);
		if (timerBar != null) {
			timerBar.Redraw(offx, offy);
		}
		if (smallScore != null) {
			smallScore.Redraw(offx, offy);
		}
		if (bigScore != null) {
			bigScore.Redraw(offx, offy);
		}
		
		APPFRAME.Redraw (offx, offy);
		APPINSTRUCTION.Redraw (offx, offy);
		APPLABEL.Redraw (offx, offy);
		
	}

	void UpdateTrees(){
		if (deadTree != null && !deadTree.remove) {
			if (left){
				deadTree.x++;
				deadTree.x++;
				
			}
			else{
				deadTree.x--;
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

				if (okay){
					score++;
					if (left)
						AddTextParticle("CHOP!", 36 - 6, 20 + UnityEngine.Random.Range(-2, 2), 'w');
					else
						AddTextParticle("CHOP!", 24 + 6, 20 + UnityEngine.Random.Range(-2, 2), 'w');
				}
			}
			else{
				alive = false;
			}
		}
	}

	SHGUIview smallScore;
	SHGUItext smallScoreContent;
	SHGUIview bigScore;

	float guiHidingTimer = 0;
	int guiHideOffset = 7;
	int desiredGuiHideOffset = 0;
	
	void DrawScore(){
		if (smallScore != null) {
			smallScore.y = 6 - guiHideOffset;
		}

		if (alive) {
			if (score == lastdisplayedscore)
				return;

			if (smallScore != null){
				smallScore.KillInstant();
			}

			smallScore = null;
			smallScore = new SHGUIview();
			smallScore.overrideFadeInSpeed = 4f;
			string s = (score )+ "";
			if (s.Length == 2){
				s = s[0] + " " + s[1];
			}
			if (s.Length == 4){
				s = s[0] + "" + s[1] + " " + s[2] + "" + s[3];
			}
			smallScoreContent = new SHGUItext(s, 0 - (int)(s.Length / 2), 0, 'w');
			smallScore.x = 32;
			smallScore.y = 6 - guiHideOffset;
			smallScore.AddSubView(new SHGUIrect(-(int)(s.Length / 2) - 2, -1, (int)(s.Length / 2) + 2, 1, 'z'));
			smallScore.AddSubView(new SHGUIframe(-(int)(s.Length / 2) - 2, -1, (int)(s.Length / 2) + 2, 1, 'z'));
			
			smallScore.AddSubView(smallScoreContent);

			AddSubView(smallScore);

			lastdisplayedscore = score;
		} else {
			if (bigScore == null && guiHideOffset == 40){
				bigScore = new SHGUIview();
				AddSubView(bigScore);

				//bigScore.AddSubView(new SHGUItext(SHGUI.current.GetASCIIartFromFont(score + ""), 10, 11, 'x'));
				bigScore.AddSubView(new SHGUItext(SHGUI.current.GetASCIIartFromFont("YOU DEAD"), 10, 3, 'w'));
				bigScore.AddSubView(new SHGUItext(SHGUI.current.GetASCIIartFromFont("SCORED:"), 10, 9, 'w'));
				bigScore.AddSubView(new SHGUItext(SHGUI.current.GetASCIIartFromFont(score + ""), 10, 15, 'w'));
				
			}
		
		}
	
	}

	SHGUIprogressbar timerBar;

	void DrawProgressBar(){
		timerBar.y = 3 - guiHideOffset;
		timerBar.currentProgress = .5f;
	}

	void AddTextParticle(string text, int x, int y, char color){
		SHGUIview temp = new SHGUItempview (.1f);

		temp.fade = 1f;
		SHGUItext t = new SHGUItext (text, x, y, color);
		temp.AddSubView (t);
		t.fade = 1f;
		AddSubView (temp);
	}

	void AddTreeOnTop(bool forceBlank){
		SHGUIsprite s = new SHGUIsprite ().AddFramesFromFile ("APPtimbertrees", 6);
		s.x = 11;
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

