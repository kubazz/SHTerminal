using System.Collections.Generic;
using UnityEngine;


public class APPmindcopy: SHGUIappbase
{
	string brain;
	string cypherBrain1;
	string cypherBrain0;
	
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
	int skipToPhase = 0;

	float speedMulti = 1f;

	public APPmindcopy(): base("copy-by-SUPERHOT")
	{
		brain = SHGUI.current.GetASCIIartByName ("Brain");
		cypherBrain1 = SHGUI.current.GetASCIIartByName ("CypherBrain1");
		cypherBrain0 = SHGUI.current.GetASCIIartByName ("CypherBrain0");
		
		brainView = SHGUI.current.GetCenteredAsciiArt ("Brain");
		brainView.x = brainViewSideX;
		brainView.y = brainViewSideY;
		//AddSubView (brainView);

		cypherBrainView = SHGUI.current.GetCenteredAsciiArt ("CypherBrain01");

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
		
		

		phase = 0;
		progress = 0;

		APPFRAME.hidden = true;
		APPINSTRUCTION.hidden = true;
		APPLABEL.hidden = true;

		Debug.Log ("brainView: " + brainView.x + ", " + brainView.y);
		Debug.Log ("cypherBrainView: " + cypherBrainView.x + ", " + cypherBrainView.y);
		
	}

	float time;

	float progress;
	public override void Update(){
		bool skipping = false;
		if (phase < skipToPhase) {
			skipping = true;
			speedMulti = 100000000f;
		} else {
			speedMulti = 2f;
		}
		time += Time.unscaledDeltaTime;

		if (fade < .99f && !skipping) {
			return;
		}

		memoryFrame.x = cypherBrainView.x;
		memoryFrame.y = cypherBrainView.y;
		UpdateCurrentPhase ();


	}

	void NextPhase(){
		phase++;
		progress = 0;
	}

	void UpdateCurrentPhase(){
		if (phase == 0) {
			progress += Mathf.Abs (Mathf.Cos (time)) * .0025f * speedMulti;
			
			if (progress > 1.05f) {
				AddSubView (brainView);
				NextPhase();
			}
		} else if (phase == 1) {
			progress += Time.unscaledDeltaTime * 0.5f * speedMulti;

			if (progress > 1f) {
				AddSubView(memoryFrame);
				
				memoryFrame.x = cypherBrainView.x;
				memoryFrame.y = cypherBrainView.y;
				
				NextPhase();
			}
		} else if (phase == 2) {
			progress += Time.unscaledDeltaTime * 0.5f * speedMulti;
			
			if (progress > 1f) {
				memoryFrame.PunchIn(.8f);
				NextPhase();
			}
		} else if (phase == 3) {
			progress += Time.unscaledDeltaTime * 5f * speedMulti;
			
			if (progress > 1f) {
				NextPhase();
			}
		} else if (phase == 4) {
			progress += Time.unscaledDeltaTime * 0.2f * speedMulti;
			float p = Mathf.Clamp01(progress);
			
			brainView.x = (int)Mathf.Lerp(brainViewSideX, brainViewCenterX, p);
			cypherBrainView.x = (int)Mathf.Lerp(cypherBrainViewSideX, cypherBrainViewCenterX, p);
			memoryFrame.x = cypherBrainView.x;			
			
			if (progress > 1f) {
				NextPhase();
			}
		}
		else if (phase == 5) {
			progress += Time.unscaledDeltaTime * 0.05f * speedMulti;
			
			if (progress > 1.1f) {
				cypherBrainView.text = SHGUI.current.GetASCIIartByName("CypherBrain01");
				brainView.AddSubView(brainCover);
				AddSubView(cypherBrainView);
				NextPhase();
			}
		}
		else if (phase == 6) {
			progress += Time.unscaledDeltaTime * 0.75f * speedMulti;

			if (progress > 1.2f) {
				NextPhase();
			}
		}
		else if (phase == 7) {
			progress += Time.unscaledDeltaTime * 0.2f * speedMulti;
			float p = Mathf.Clamp01(progress);
			
			brainView.x = (int)Mathf.Lerp(brainViewCenterX, brainViewSideX, p);
			cypherBrainView.x = (int)Mathf.Lerp(cypherBrainViewCenterX, cypherBrainViewSideX, p);
			memoryFrame.x = cypherBrainView.x;
			brainCoverItself.SetChar (GetPulsatingChar ());
			
			if (progress > 1f) {
				NextPhase();
			}
		}
		else if (phase == 8) {
			progress += Time.unscaledDeltaTime * .15f * speedMulti;
			memoryFrameGlow.SetChar (GetPulsatingChar ());
			brainCoverItself.SetChar (GetPulsatingChar ());
			if (progress > 1f) {
				NextPhase();
			}
		}
		else if (phase == 9) {
			progress += Time.unscaledDeltaTime * .6f * speedMulti;
			memoryFrameGlow.SetChar (GetPulsatingChar (10f));
			brainCoverItself.SetChar (GetPulsatingChar (10f));
			if (progress > 1f) {
				NextPhase();
			}
		}
		else if (phase == 10) {
			progress += Time.unscaledDeltaTime * .3f * speedMulti;
			memoryFrameGlow.SetChar (' ');
			brainCoverItself.SetChar (' ');
			if (progress > 1f) {
				NextPhase();
			}
		}
		else if (phase == 11) {
			progress += Time.unscaledDeltaTime * .3f * speedMulti;
			if (progress > 1f) {
				brainView.Kill();
				brainView.overrideFadeOutSpeed = .3f;
				NextPhase();
			}
		}
		else if (phase == 12) {

	
			
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
			DrawTextProgress (brain, brainView.x, brainView.y, 'z', progress, 6);
		} else if (phase == 5) {
			DrawTextProgress (cypherBrain1, cypherBrainView.x, cypherBrainView.y, 'w', progress, 2);
			DrawTextProgress (cypherBrain0, cypherBrainView.x, cypherBrainView.y, 'w', progress - 0.045f, 2);
			
		} else if (phase == 7) {

			DrawCypherConnection ();
			
		} else if (phase == 8) {
			//	memoryFrameGlow.hidden = false;

			DrawCypherConnection ();
		} else if (phase == 9) {
			DrawCypherConnection (10f);
		}
		else if (phase == 10) {
		}


	}

	char GetPulsatingChar(float multiplier = 1f){
		return scramble[(int)(time * 6f * multiplier) % scramble.Length ];
	}

	public void DrawCypherConnection(float pulseMulti = 1f){
		for (int X = brainView.x + 8; X < cypherBrainView.x + 5; ++X) {
			int Y = 11 + (int)(Mathf.Sin(time * 10 - X / 3) * 2);
			char C = SHGUI.current.GetPixelFront(X,Y);
			if (C == ' ' || C == ',' || C == '}' || C =='{' || C == ')' || C == '_' || C == '\'' || C == '`')
				SHGUI.current.SetPixelFront(GetPulsatingChar(pulseMulti), X, Y, 'z');
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

	void DrawTextProgress(string content, int X, int Y, char color, float progress, int progressEdgeLength = 6){

		int intProgress = (int)(Mathf.Clamp01 (progress) * content.Length);

		bool drawEdge = true;
		if (progress > 1)
			drawEdge = false;

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
				if (Mathf.Abs(i - intProgress) < progressEdgeLength){
					if (drawEdge && zzz != ' ' && Random.value < .5f){
						zzz = StringScrambler.GetGlitchChar();
						ccc = 'w';
					}
				}

				if (zzz != ' ')
					SHGUI.current.SetPixelFront(zzz, X + X1, Y + Y1, ccc);
				
				X1++;

			}
		}


	}

	public override void ReactToInputKeyboard(SHGUIinput key)
	{
		if (key == SHGUIinput.enter){
			skipToPhase = phase + 1;

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


