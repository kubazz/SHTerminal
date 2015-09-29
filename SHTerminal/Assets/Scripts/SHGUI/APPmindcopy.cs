using System.Collections.Generic;
using UnityEngine;


public class APPmindcopy: SHGUIappbase
{
	string brain;
	string cypherBrain;
	SHGUItext brainView;
	SHGUItext cypherBrainView;
	SHGUIview memoryFrame;
	SHGUIframe memoryFrameItself;
	SHGUIview brainCover;
	SHGUIrect brainCoverItself;
	SHGUIrect memoryFrameGlow;

	SHGUItext subtitle;

	int brainViewCenterX = 20;
	int brainViewCenterY = 7 - 1;
	int cypherBrainViewCenterX = 27;
	int cypherBrainViewCenterY = 7;

	int brainViewSideX = 20 - 15;
	int brainViewSideY = 7;
	int cypherBrainViewSideX = 27 + 15;
	int cypherBrainViewSideY = 8 + 1;

	int phase;
	int skipToPhase = 5;

	float speedMulti = 1f;

	public APPmindcopy(): base("copy-by-SUPERHOT")
	{
		brain = SHGUI.current.GetASCIIartByName ("Brain");
		cypherBrain = SHGUI.current.GetASCIIartByName ("CypherBrain");
		
		brainView = SHGUI.current.GetCenteredAsciiArt ("Brain");
		brainView.x = brainViewSideX;
		brainView.y = brainViewSideY;
		//AddSubView (brainView);

		cypherBrainView = SHGUI.current.GetCenteredAsciiArt ("CypherBrain");

		cypherBrainView.x = cypherBrainViewSideX;
		cypherBrainView.y = cypherBrainViewSideY;
		
		//AddSubView (cypherBrainView);

		memoryFrame = new SHGUIview ();
		memoryFrameGlow = new SHGUIrect (-2, -2, 13, 6, 'z', ' ', 0);
		memoryFrameGlow.hidden = true;
		memoryFrame.AddSubView (memoryFrameGlow);
		memoryFrameItself = new SHGUIframe (-1, -1, 12, 5, 'z');
		memoryFrame.AddSubView(memoryFrameItself);

		brainCover = new SHGUIview ();
		brainCoverItself = new SHGUIrect (7, 2, 18, 6, 'z', ' ', 0);
		//brainCoverItself = new SHGUIrect (6, 1, 19, 7, 'z', ' ', 0);
		brainCover.AddSubView(brainCoverItself);
		//brainCover.AddSubView (new SHGUIframe (6, 1, 19, 7, 'z'));
		
		

		phase = -1;

		SetupNewPhase (0);

		APPFRAME.hidden = true;
		APPINSTRUCTION.hidden = true;
		APPLABEL.hidden = true;

		Debug.Log ("brainView: " + brainView.x + ", " + brainView.y);
		Debug.Log ("cypherBrainView: " + cypherBrainView.x + ", " + cypherBrainView.y);
		
	}

	float time;

	float progress;
	public override void Update(){
		if (phase < skipToPhase) {
			speedMulti = 100000000f;
		} else {
			speedMulti = 1f;
		}
		time += Time.unscaledDeltaTime;

		memoryFrame.x = cypherBrainView.x;
		memoryFrame.y = cypherBrainView.y;
		UpdateCurrentPhase ();


	}

	void SetupNewPhase(int p){


		if (phase != p) {
			phase = p;

			if (phase == 0){
				progress = 0;
				SetSubtitle("scanning...", 'z');
			}
			else if (phase == 1){
				SetSubtitle("scanning complete!", 'w');
				progress = 0;
				
			}
			else if (phase == 2){
				SetSubtitle("allocating memory...", 'z');
				AddSubView(memoryFrame);

				memoryFrame.x = cypherBrainView.x;
				memoryFrame.y = cypherBrainView.y;
				
				progress = 0;
				
			}
			else if (phase == 3){
				//memoryFrameItself.SetColor('r');
				memoryFrame.PunchIn(.7f);

				progress = 0;
			}
			else if (phase == 4){
				
				progress = 0;
			}
			else if (phase == 5){
				progress = 0;
			}
			else if (phase == 6){
				brainView.AddSubView(brainCover);
				AddSubView(cypherBrainView);
				progress = 0;
			}
			else if (phase == 7){
				progress = 0;
			}
			else if (phase == 8){
				progress = 0;
			}
			else if (phase == 9){
				progress = 0;
			}
			else if (phase == 10){
				progress = 0;
			}
		}
	}

	void UpdateCurrentPhase(){
		if (phase == 0) {
			progress += Mathf.Abs (Mathf.Cos (time)) * .0005f * speedMulti;
			
			if (progress > 1.05f) {
				SetupNewPhase (1);
				AddSubView (brainView);
			}
		} else if (phase == 1) {
			progress += Time.unscaledDeltaTime * 0.5f * speedMulti;

			if (progress > 1f) {
				SetupNewPhase (2);
			}
		} else if (phase == 2) {
			progress += Time.unscaledDeltaTime * 0.5f * speedMulti;
			
			if (progress > 1f) {
				memoryFrame.PunchIn(.8f);
				SetupNewPhase (3);
			}
		} else if (phase == 3) {
			progress += Time.unscaledDeltaTime * 5f * speedMulti;
			
			if (progress > 1f) {
				SetupNewPhase (4);
			}
		} else if (phase == 4) {
			progress += Time.unscaledDeltaTime * 0.2f * speedMulti;
			float p = Mathf.Clamp01(progress);
			
			brainView.x = (int)Mathf.Lerp(brainViewSideX, brainViewCenterX, p);
			cypherBrainView.x = (int)Mathf.Lerp(cypherBrainViewSideX, cypherBrainViewCenterX, p);
			memoryFrame.x = cypherBrainView.x;			
			
			if (progress > 1f) {
				SetupNewPhase (5);
			}
		}
		else if (phase == 5) {
			progress += Time.unscaledDeltaTime * 0.05f * speedMulti;
			
			if (progress > 1f) {
				SetupNewPhase (6);
			}
		}
		else if (phase == 6) {
			progress += Time.unscaledDeltaTime * 0.75f * speedMulti;

			if (progress > 1.2f) {
				SetupNewPhase (7);
			}
		}
		else if (phase == 7) {
			progress += Time.unscaledDeltaTime * 0.2f * speedMulti;
			float p = Mathf.Clamp01(progress);
			
			brainView.x = (int)Mathf.Lerp(brainViewCenterX, brainViewSideX, p);
			cypherBrainView.x = (int)Mathf.Lerp(cypherBrainViewCenterX, cypherBrainViewSideX, p);
			memoryFrame.x = cypherBrainView.x;	
			if (progress > 1f) {
				SetupNewPhase (8);
			}
		}
		else if (phase == 8) {
			progress += Time.unscaledDeltaTime * .1f * speedMulti;
			if (progress > 1f) {
				brainView.Kill();
				brainCover.Kill();
				SetupNewPhase (9);
			}
		}
		else if (phase == 9) {
			progress += Time.unscaledDeltaTime * 1f * speedMulti;
			if (progress > 1f) {
				SetupNewPhase (10);
			}
		}
		else if (phase == 10) {
			/*
			progress += Time.unscaledDeltaTime * .05f * speedMulti;
			float p = Mathf.Clamp01(progress);

			brainView.x = (int)Mathf.Lerp(brainViewSideX, brainViewCenterX, p);
			cypherBrainView.x = (int)Mathf.Lerp(cypherBrainViewSideX, cypherBrainViewCenterX, p);
			memoryFrame.x = cypherBrainView.x;	

			if (progress > 1f) {
				SetupNewPhase (11);
			}
			*/
		}
	}


	string scramble = "█▓▒░▒▓";

	void DrawCurrentPhase(){
		if (phase == 0) {
			DrawTextProgress (brain, brainView.x, brainView.y, (progress > 1f) ? ('z') : ('z'), progress);
		} else if (phase == 5) {
			DrawTextProgress (cypherBrain, cypherBrainView.x, cypherBrainView.y, (progress > 1f) ? ('w') : ('w'), progress);
		} else if (phase == 7) {

			brainCoverItself.SetChar(GetPulsatingChar());
			DrawCypherConnection ();
			
		} else if (phase == 8) {
		//	memoryFrameGlow.hidden = false;
			memoryFrameGlow.SetChar(GetPulsatingChar());
			brainCoverItself.SetChar(GetPulsatingChar());
			DrawCypherConnection ();
			
		}


	}

	char GetPulsatingChar(){
		return scramble[(int)(time * 6f) % scramble.Length ];
	}

	public void DrawCypherConnection(){
		for (int X = brainView.x + 8; X < cypherBrainView.x + 5; ++X) {
			int Y = 11 + (int)(Mathf.Sin(time * 10 - X / 3) * 2);
			char C = SHGUI.current.GetPixelFront(X,Y);
			if (C == ' ' || C == ',' || C == '}' || C =='{' || C == ')' || C == '_' || C == '\'' || C == '`')
				SHGUI.current.SetPixelFront(GetPulsatingChar(), X, Y, 'z');
			if (C == '│'){
				//SHGUI.current.SetPixelBack(GetPulsatingChar(), X, Y, 'w');
				
				SHGUI.current.SetPixelFront('▌', X, Y, 'w');
			}
		}
	}

	public void SetSubtitle(string content, char color){
		if (subtitle != null) {
			subtitle.Kill();
		}

		//subtitle = AddSubView (new SHGUItext (content, 32 - (int)(content.Length / 2), 18, color)) as SHGUItext;
	}

	public override void Redraw(int offx, int offy){
		
		base.Redraw (offx, offy);
		if (fade < 0.99f)
			return;

		DrawCurrentPhase ();

		//DrawTextProgress (cypherBrain, cypherBrainView.x, cypherBrainView.y, (progress > 1) ?('r'):('w'), progress);

	}

	void DrawTextProgress(string content, int X, int Y, char color, float progress){

		int intProgress = (int)(Mathf.Clamp01 (progress) * content.Length);

		int X1 = 0;
		int Y1 = 0;
		for (int i = 0; i < intProgress; ++i){
			if (content[i] == '\n'){
				X1 = 0;
				Y1++;
			}
			else{
				char zzz = content[i];
				char ccc = color;
				if (Mathf.Abs(i - intProgress) < 6f){
					if (zzz != ' ' && Random.value < .5f){
						zzz = StringScrambler.GetGlitchChar();
						ccc = 'w';
					}
				}
				SHGUI.current.SetPixelFront(zzz, X + X1, Y + Y1, ccc);
				
				X1++;

			}
		}


	}

	public override void ReactToInputKeyboard(SHGUIinput key)
	{
		if (key == SHGUIinput.enter){

		}


		if (key == SHGUIinput.esc)
			SHGUI.current.PopView ();
	}
	
	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
	{
	    if (fadingOut)
	        return;
	}
}


