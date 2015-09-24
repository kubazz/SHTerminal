
using System;
using UnityEngine;
using System.Collections.Generic;

public class APPtreedude: SHGUIappbase
{

	public List<SHGUIsprite> trees;
	SHGUIsprite deadTree;
	public SHGUIsprite dude;

	bool left = true;
	bool alive = true;

	float hitTime = 0;

	int score = 0;
	int lastdisplayedscore = -1;

	SHGUIprogressbar timeToDieBar;
	float timeToDieMax = 10f;
	float timeToDie = 0f;
	float timeToDieMultiplier = .25f;

	int currentSpeedIndex = 0;
	List<float> speeds;
	List<int> chopsPerSpeed;
	int chopsThisSpeed = 0;

	SHGUIview tutorialView;	
	SHGUIview timeTutorialView;

	public APPtreedude (): base("TREE-DUDE-TREE-DUDE-DUDE-by-piotr")
	{
		//AddSubView (new SHGUItext (SHGUI.current.GetASCIIartByName ("APPtreedudeback"), 0, 0, 'z'));


		timeToDie = timeToDieMax * .5f;
		GenerateDifficulty ();

		trees = new List<SHGUIsprite> ();
		dude = new SHGUIsprite ().AddFramesFromFile("APPtreedudedude", 6);
		AddSubView (dude);


		AddTreeOnTop (true);
		AddTreeOnTop (true);
		AddTreeOnTop (true);
		
		for (int i = 0; i < 10; ++i) {
			AddTreeOnTop (false);
			AddTreeOnTop (true);
		}

		timeToDieBar = new SHGUIprogressbar (20, 3, 21, "", "").SetBlinkingLabel("WATCH-TIME!", .2f);
		timeToDieBar.SetStyle ("z█z░z█");
		AddSubView (timeToDieBar);

		tutorialView = new SHGUIview ();
		AddSubView (tutorialView);

		SHGUIblinkview b1 = new SHGUIblinkview (.75f).SetFlipped();
		int tutorialy = 20;
		b1.AddSubView(new SHGUItext ("CHOP-LEFT-->", 13, tutorialy, 'w'));
		tutorialView.AddSubView (b1);

		SHGUIblinkview b2 = new SHGUIblinkview (.75f).SetFlipped();
		b2.AddSubView(new SHGUItext ("<--CHOP-RIGHT", 40, tutorialy, 'w'));
		tutorialView.AddSubView (b2);
	}

	void GenerateDifficulty(){
		int num = 20;
		speeds = new List<float> ();
		speeds.Add (1f);
		float maxAllowedSpeed = 3.5f;
		for (int i = 0; i < num; ++i) {
			speeds.Add (Mathf.Clamp(Mathf.Sqrt(i * 1.4f), 1f, maxAllowedSpeed));
		}

		chopsPerSpeed = new List<int> ();
		for (int i = 0; i < num; ++i) {
			chopsPerSpeed.Add ((i + 1) * 10);
		}

		/*
		int sum = 0;
		for (int i = 0; i < num; ++i) {
			sum += chopsPerSpeed[i];
			Debug.Log("speed: " + speeds[i] + " after " + sum + " chopped trees");
		}
		*/
	}

	public override void Update ()
	{
		base.Update ();

		hitTime -= Time.unscaledDeltaTime;
		dude.y = 17;
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
			if (hitTime < -.5f){
				dude.color = 'z';
			}
			dude.currentFrame = 4;
		}
		UpdateTrees ();
		DrawScore ();
		UpdateTimeToDie();
		UpdateTutorial ();

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

	void UpdateTutorial(){
		if (!alive) {
			tutorialView.Kill();
		}
	}

	void Die(){
		alive = false;
		for (int i = 0; i < trees.Count; ++i) {
			trees[i].color = 'z';
		}
	}

	void UpdateTimeToDie(){
		timeToDie -= Time.unscaledDeltaTime * timeToDieMultiplier;

		if (timeToDie > timeToDieMax) {
			timeToDie = timeToDieMax;
		} else {
			if (timeToDie < 0){
				if (alive){
					hitTime = .1f;
					timeToDie = 0f;
				}
				Die();
			}
		}

		timeToDieBar.y = 3 - guiHideOffset;
		timeToDieBar.currentProgress = timeToDie / timeToDieMax;

		if (timeToDieBar.currentProgress < .25f) {
			timeToDieBar.labelView.hidden = false;
		} else {
			timeToDieBar.labelView.hidden = true;
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
		if (timeToDieBar != null) {
			timeToDieBar.Redraw(offx, offy);
		}
		if (smallScore != null) {
			smallScore.Redraw(offx, offy);
		}
		if (bigScore != null) {
			bigScore.Redraw(offx, offy);
		}

		if (lastFigletParticle != null && lastFigletParticle.remove == false) {
			lastFigletParticle.Redraw(offx, offy);
		}

		if (tutorialView != null) {
			tutorialView.Redraw(offx, offy);
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
			tutorialView.Kill();

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
					Die();
				}

				if (okay){
					chopsThisSpeed++;
					if (chopsThisSpeed > chopsPerSpeed[currentSpeedIndex]){
						chopsThisSpeed = 0;
						currentSpeedIndex++;
						if (currentSpeedIndex > speeds.Count - 1){
							currentSpeedIndex = speeds.Count - 1;
						}

						timeToDieMultiplier = speeds[currentSpeedIndex];

						DisplayRandomChopChopMessage();
					}

					timeToDie += .5f;
					score++;
					if (left)
						AddTextParticle("CHOP!", 36 - 6, 20 + UnityEngine.Random.Range(-2, 2), 'w');
					else
						AddTextParticle("CHOP!", 24 + 6, 20 + UnityEngine.Random.Range(-2, 2), 'w');
				}
			}
			else{
				Die();
			}
		}
	}

	string[] chopchopMessages = {"CHOP CHOP", "FASTER", "LEVEL UP", "SPEED UP", "TICK TOCK"};
	void DisplayRandomChopChopMessage(){
		AddFigletTextParticle(chopchopMessages[UnityEngine.Random.Range(0, chopchopMessages.Length)], 32, 2, 'w');
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
				//bigScore.AddSubView(new SHGUItext(SHGUI.current.GetASCIIartFromFont("DUDE DEAD"), 10, 3, 'w'));
				SHGUIsprite s1 = new SHGUIsprite();
				s1.x = 10;
				s1.y = 2;
				s1.animationSpeed = 1.5f;
				s1.loops = true;
				APPtreedudeintro.TreeDudeSequence(s1, "D%T%");
				bigScore.AddSubView(s1);

				SHGUIsprite s2 = new SHGUIsprite();
				s2.x = 35;
				s2.y = 2;
				s2.loops = true;
				s2.animationSpeed = 1.5f;
				APPtreedudeintro.TreeDudeSequence(s2, "%DDD");
				bigScore.AddSubView(s2);
				

				//score += 990;
				int len = (6 + score.ToString().Length) * 6 - 1;
				int off = (((score.ToString ().Length % 2) == 0)?(1):(0));
				bigScore.AddSubView(new SHGUItext(SHGUI.current.GetASCIIartFromFont("SCORE " + score), 34 - (int)(len / 2) + off, 9, 'w'));
				//bigScore.AddSubView(new SHGUItext(SHGUI.current.GetASCIIartFromFont(score + ""), 10, 15, 'w'));

				string ttt = "--PERSONAL-BEST-" + personalBest + "--";
				if (score > personalBest){
					personalBest = score;
					ttt = "--NEW-PERSONAL-BEST!-";
				}

				SHGUIview v = new SHGUIview();

				v.AddSubView(new SHGUItext(ttt, 32 - (int)(ttt.Length / 2) - 1, 15, 'w'));
				bigScore.AddSubView(v);

				SHGUIview v2 = new SHGUIblinkview(.75f);
				string ttt2 = "[PRESS ENTER TO RESTART]";
				v2.AddSubView(new SHGUIrect(32 - (int)(ttt2.Length / 2), 20, 32 + (int)(ttt2.Length / 2) - 1, 20));
				v2.AddSubView(new SHGUItext(ttt2, 32 - (int)(ttt2.Length / 2), 20, 'w'));
				bigScore.AddSubView(v2);
			}
		}
	}

	public static int personalBest = 10;

	

	void AddTextParticle(string text, int x, int y, char color){
		SHGUIview temp = new SHGUItempview (.1f);

		temp.fade = 1f;
		SHGUItext t = new SHGUItext (text, x, y, color);
		temp.AddSubView (t);
		t.fade = 1f;
		AddSubView (temp);
	}

	SHGUIview lastFigletParticle;
	void AddFigletTextParticle(string text, int x, int y, char color){
		//centered X
		SHGUIview temp = new SHGUItempview (.4f);

		int halflen = (text.Length - 1) * 3;
		temp.fade = 1f;
		SHGUItext t = new SHGUItext (SHGUI.current.GetASCIIartFromFont(text), x - halflen, y, color);
		temp.AddSubView (t);
		t.fade = 1f;
		AddSubView (temp);

		lastFigletParticle = temp;
	}


	void AddTreeOnTop(bool forceBlank){
		SHGUIsprite s = new SHGUIsprite ().AddFramesFromFile ("APPtreedudetrees", 6);
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

	void Restart(){
		Kill ();
		SHGUI.current.LaunchAppByName ("APPtreedude");
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
		
		if (key == SHGUIinput.enter) {
			if (!alive){
				Restart();
			}
		}
	}
	
	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll){	
		if (fadingOut)
			return;

	}
}

