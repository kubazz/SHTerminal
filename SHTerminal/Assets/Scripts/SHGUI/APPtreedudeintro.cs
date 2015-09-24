
using System;
using UnityEngine;
using System.Collections.Generic;

public class APPtreedudeintro: SHGUIappbase
{
	SHGUIview loading;
	SHGUIview prompters;
	SHGUIview finalAnimation;
	SHGUIview instruction;
	SHGUIview dude;

	int lastShownStep = -1;
	float timer = 0;

	public APPtreedudeintro (): base("TREE-DUDE-TREE-DUDE-DUDE-by-piotr")
	{
		APPFRAME.Kill ();
		APPLABEL.Kill ();
		APPINSTRUCTION.Kill ();

		PrepareFinalAnimation ();
		PrepareInstruction ();
		PrepareLoading ();
		PreparePrompters ();
		PrepareDudeJumping ();
	}

	void PreparePrompters(){
		prompters = new SHGUIview ();
		prompters.hidden = true;

		SHGUIprompter p1 = new SHGUIprompter (32 - 10, 6, 'w');
		p1.SetInput ("^M2HIS NAME IS TREEDUDE");
		prompters.AddSubView (p1);

		SHGUIprompter p2 = new SHGUIprompter (32 - 9, 15, 'w');
		p2.SetInput ("^M2^W9AND HIS SONG IS^M1^W2.^W2.^W2.^W7");
		prompters.AddSubView (p2);


		//AddSubView (prompters);
		
	}

	void PrepareDudeJumping(){
		dude = new SHGUIview ();
		SHGUIsprite s = new SHGUIsprite ();
		s.AddSpecyficFrameFromFile("APPtreedudedude", 6, 0);
		s.frames [s.frames.Count - 1] = "\n" + s.frames [s.frames.Count - 1];		
		s.AddSpecyficFrameFromFile("APPtreedudedude", 6, 1);
		s.AddSpecyficFrameFromFile("APPtreedudedude", 6, 2);
		s.frames [s.frames.Count - 1] = "\n" + s.frames [s.frames.Count - 1];		
		
		s.loops = false;
		s.killOnAnimationComplete = false;
		s.animationSpeed = .2f;
		
		s.x = 32 - 10;
		s.y = 7;

		dude.AddSubView (s);
	}

	void PrepareLoading(){
		loading = new SHGUIview ();
		SHGUIsprite s = new SHGUIsprite ();
		s.AddFrame ("Loading |");
		s.AddFrame ("Loading /");
		s.AddFrame ("Loading -");
		s.AddFrame ("Loading \\");

		s.loops = true;
		s.animationSpeed = .1f;

		s.x = 32 - 5;
		s.y = 11;

		loading.AddSubView (s);

	//	loading.hidden = true;

		//AddSubView (loading);
	}

	void PrepareFinalAnimation(){
		finalAnimation = new SHGUIview ();
		
		SHGUIsprite s = new SHGUIsprite ();
		s.x = 32 - 11;
		s.y = 8;

		TreeDudeSequence (s, "TT|| D|0| TT|| D| D|");
		TreeDudeSequence (s, "TT|| D|2| TT|| D| D|");
		
		s.animationSpeed = .3f;
		
		s.loops = true;
		
		finalAnimation.AddSubView (s);
	}

	public static void TreeDudeSequence(SHGUIsprite s, string sequence){
		for (int i = 0; i < sequence.Length; ++i) {
			if (sequence[i] == '|'){
				s.AddFrame ("");
			}
			else if (sequence[i] == 'T'){
				s.AddFrame ("\n" + SHGUI.current.GetASCIIartFromFont ("TREE"));
			}
			else if (sequence[i] == 'D'){
				s.AddFrame ("\n" + SHGUI.current.GetASCIIartFromFont ("DUDE"));
			}
			else if (sequence[i] == '%'){
				s.AddFrame ("\n" + SHGUI.current.GetASCIIartFromFont ("DEAD"));
			}
			else if (sequence[i] == '0'){
				s.AddSpecyficFrameFromFile("APPtreedudedude", 6, 0);
			}
			else if (sequence[i] == '1'){
				s.AddSpecyficFrameFromFile("APPtreedudedude", 6, 1);
			}
			else if (sequence[i] == '2'){
				s.AddSpecyficFrameFromFile("APPtreedudedude", 6, 2);
			}
			else if (sequence[i] == '3'){
				s.AddSpecyficFrameFromFile("APPtreedudedude", 6, 3);
			}
		}
	}

	void PrepareInstruction(){
		instruction = new SHGUIview ();

		SHGUIview v2 = new SHGUIview();
		string ttt2 = "[PRESS ENTER TO START]";
		v2.AddSubView(new SHGUItext(ttt2, 31 - (int)(ttt2.Length / 2), 17, 'w'));
		instruction.AddSubView(v2);
	}

	public override void Update ()
	{
		base.Update ();

		timer += Time.unscaledDeltaTime;
		if (fade < .5f)
			return;

		if (timer > 0f)
			ShowAnimationStep(0);

		if (timer > 0f)
			ShowAnimationStep (1);

		if (timer > 2f)
			ShowAnimationStep (2);

		if (timer > 8.6f)
			ShowAnimationStep (3);

		if (timer > 13.76f)
			ShowAnimationStep (4);
	}
	
	void ShowAnimationStep(int step){
		if (step <= lastShownStep) {
			return;
		}
		
		if (step == 0) {
			//AddSubView(loading);
		} else if (step == 1) {
			loading.Kill ();
			prompters.hidden = false;
			AddSubView(prompters);
		} else if (step == 2) {
			AddSubView(dude);
		} else if (step == 3) {
			dude.Kill();
			prompters.Kill();
			AddSubView (finalAnimation);
		}
		else if (step == 4) {
			AddSubView(instruction);
		}
		
		lastShownStep = step;
	}

	public override void Redraw (int offx, int offy)
	{
		base.Redraw (offx, offy);
	
		APPFRAME.Redraw (offx, offy);
		APPINSTRUCTION.Redraw (offx, offy);
		APPLABEL.Redraw (offx, offy);
		
	}

	void LaunchGame(){
		Kill ();
		SHGUI.current.LaunchAppByName ("APPtreedude");
	}
	
	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;

		if (key == SHGUIinput.esc)
			LaunchGame();
		
		if (key == SHGUIinput.enter) {
			LaunchGame();
		}
	}
	
	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll){	
		if (fadingOut)
			return;

		if (clicked) {
			LaunchGame();
		}

	}
}

