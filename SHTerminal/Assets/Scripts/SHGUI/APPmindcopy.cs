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
	SHGUIoutflow outflowView;

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

	SHGUIview popup;

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

		//Debug.Log ("brainView: " + brainView.x + ", " + brainView.y);
		//Debug.Log ("cypherBrainView: " + cypherBrainView.x + ", " + cypherBrainView.y);
		
	}

	float time;

	float progress;
	public override void Update(){
		base.Update ();

		bool skipping = false;
		if (phase < skipToPhase) {
			skipping = true;
			speedMulti = 100000000f;
		} else {
			speedMulti = 1f;
		}
		time += Time.unscaledDeltaTime;

		if (fade < .99f && !skipping) {
			return;
		}

		memoryFrame.x = cypherBrainView.x;
		memoryFrame.y = cypherBrainView.y;

		if (!IsTherePopup ()) {
			UpdateCurrentPhase ();
		}
	}

	void NextPhase(){
		phase++;
		progress = 0;
		currentWaiter = 0;
		progressThisFrame = 0;
		popupShownThisPhase = false;
	}

	bool popupShownThisPhase = false;
	float currentWaiter = 0;
	bool Wait(float t){
		if (phase < skipToPhase)
			return true;

		currentWaiter += Time.unscaledDeltaTime * speedMulti;
		if (currentWaiter > t) {
			return true;
		}
		return false;
	}

	float progressThisFrame = 0;
	bool Progress(float t){
		if (phase < skipToPhase) {
			NextPhase();
			return true;
		}
		
		progressThisFrame += Time.unscaledDeltaTime * speedMulti;
		progress = progressThisFrame / t;
		if (progressThisFrame > t) {
			NextPhase();
			return true;
		}
		return false;
	}

	void UpdateCurrentPhase(){
		if (phase == 0) { //brain gets scanned in Redraw(...)
			if (Progress (10f)) {
				AddSubView (brainView);
			}
		} else if (phase == 1) { //memory is prepared
			if (Progress (1f)) {
				AddSubView (memoryFrame);
				
				memoryFrame.x = cypherBrainView.x;
				memoryFrame.y = cypherBrainView.y;
			}
		} else if (phase == 2) { //brain and memory converge
			if (!Wait (1.5f))
				return;
			ShowPopup (" Are you sure? ");

			brainView.x = (int)Mathf.Lerp (brainViewSideX, brainViewCenterX, progress);
			cypherBrainView.x = (int)Mathf.Lerp (cypherBrainViewSideX, cypherBrainViewCenterX, progress);
			memoryFrame.x = cypherBrainView.x;			
			if (Progress (2f)) {
				brainView.x = brainViewCenterX;
				cypherBrainView.x = cypherBrainViewCenterX;
				memoryFrame.x = cypherBrainView.x;	
				SetSubtitle (" you may experience\n slight discomfort ", 'z');
			}
		} else if (phase == 3) { //brain dump in Redraw(...)
			if (!Wait (0.5f))
				return;
			Progress (4f);
		} else if (phase == 4) { //brain dump in Redraw(...)
			if (!Wait (0.5f))
				return;
			Progress (4f);
		} else if (phase == 5) { //brain dump in Redraw(...)
			if (!Wait (0.5f))
				return;
			Progress (4f);
		} else if (phase == 6) { //brain dump in Redraw(...)
			if (!Wait (0.5f))
				return;
			if (Progress (4f)) {
				SetSubtitle ("", 'w');
				cypherBrainView.text = SHGUI.current.GetASCIIartByName ("CypherBrain01");
				AddSubView (brainCover);

				brainCover.x = brainView.x;
				brainCover.y = brainView.y;
				
				AddSubView (cypherBrainView);
			}
		} else if (phase == 7) { // brain and memory diverge
			if (!Wait (2f))
				return;
				
			brainView.x = (int)Mathf.Lerp (brainViewCenterX, brainViewSideX, progress);
			cypherBrainView.x = (int)Mathf.Lerp (cypherBrainViewCenterX, cypherBrainViewSideX, progress);
			memoryFrame.x = cypherBrainView.x;	
			brainCoverItself.SetChar (GetPulsatingChar ());

			brainCover.x = brainView.x;
			brainCover.y = brainView.y;
			
			if (Progress (2.5f)) {
				brainView.x = brainViewSideX;
				cypherBrainView.x = cypherBrainViewSideX;
				memoryFrame.x = cypherBrainView.x;	
			}
		} else if (phase == 8) { //connection persists
			brainCover.x = brainView.x;
			brainCover.y = brainView.y;
			memoryFrameGlow.SetChar (GetPulsatingChar ());
			brainCoverItself.SetChar (GetPulsatingChar ());

			Progress (2f);
		} else if (phase == 9) { //connection blinks fast for a moment

			memoryFrameGlow.SetChar (GetPulsatingChar (10f));
			brainCoverItself.SetChar (GetPulsatingChar (10f));

			Progress (.5f);
		} else if (phase == 10) { //connection disappears brain half-empty; brain dies

			memoryFrameGlow.SetChar (' ');
			brainCoverItself.SetChar (' ');

			if (Progress (2f)) {
				brainView.Kill ();
				brainView.overrideFadeOutSpeed = .15f;
			}
		} else if (phase == 11) {
			if (!Wait (2.5f))
				return;
			
			cypherBrainView.x = (int)Mathf.Lerp (cypherBrainViewSideX, cypherBrainViewCenterX, progress);
			memoryFrame.x = cypherBrainView.x;	
			
			if (Progress (2.5f)) {
				if (brainCover != null) brainCover.Kill();
				cypherBrainView.x = cypherBrainViewCenterX;
				memoryFrame.x = cypherBrainView.x;
			}
		} else if (phase == 12) {			
			if (Progress (2f)) {
				memoryFrame.SetColorRecursive('r');
				outflowView = AddSubView(new SHGUIoutflow()) as SHGUIoutflow;
			}
		} else if (phase == 13) {
			if (!Wait (.3f))
				return;

			outflowView.AddSeeds();

			if (Progress (2f)) {
				memoryFrame.AddSubView(new SHGUIrect(-1, 2, -1, 2));
				outflowView.DestroyBarrier();
			}
		} else if (phase == 14) {
			outflowView.DestroyBarrier();
			if (cypherBrainView != null && !cypherBrainView.fadingOut){
				cypherBrainView.Kill();
			}
			
			if (Progress(1.2f)){
				outflowView.AddLimit();
			}
		} else if (phase == 15) {
			outflowView.AddLimit();//dla pewności
			if (!Wait (1.5f))
				return;

			if (memoryFrame != null) 
				memoryFrame.Kill ();

			if (Progress(4.2f)){
				outflowView.Kill();
				Kill ();
				SHGUI.current.AddViewOnTop(new APPrecruit());
			}
		}
		else if (phase == 16) {
			if (Progress (10f)){

			}

		}
		else if (phase == 60) { //only copy exists
			if (Progress (5)) {
				//Kill ();
			}
		} else { // skip empty phases
			Progress(-1f); 
		}
	}


	string scramble = "█▓▒░▒▓";

	void DrawCurrentPhase(){		

		if (phase == 0) {
			DrawTextProgress (brain, brainView.x, brainView.y, 'z', progress, 6);
		} else if (phase == 3) {
			DrawBrain(progress, 0, 4);
		} else if (phase == 4) {
			DrawBrain(progress, 1, 4);
		} else if (phase == 5) {
			DrawBrain(progress, 2, 4);
		} else if (phase == 6) {
			DrawBrain(progress, 3, 4);
		} else if (phase == 7) {
			DrawCypherConnection ();
		} else if (phase == 8) {
			DrawCypherConnection ();
		} else if (phase == 9) {
			DrawCypherConnection (10f);
		}
		else if (phase == 10) {
		}
	}

	void DrawBrain(float prog, int subPhase, int subPhasesCount){
		float baseProg = ((float)subPhase / (float)subPhasesCount);
		float onePhaseProg = 1f / (float)subPhasesCount;
		//Debug.Log (prog +", " + baseProg + onePhaseProg * prog);
		DrawTextProgress (cypherBrain1, cypherBrainView.x, cypherBrainView.y, 'w', baseProg + onePhaseProg * prog, 2);
		DrawTextProgress (cypherBrain0, cypherBrainView.x, cypherBrainView.y, 'w', baseProg + onePhaseProg * prog - 0.045f, 2);
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
				SHGUI.current.SetPixelFront('▌', X, Y, 'w');
			}
		}
	}

	public void SetSubtitle(string content, char color){
		if (subtitle != null) {
			subtitle.Kill();
		}

		int currentLine = 0;
		int longestLine = 0;
		for (int i = 0; i < content.Length; ++i) {
			if (currentLine > longestLine){
				longestLine = currentLine;
			}

			if (content[i] == '\n'){
				currentLine = 0;
			}
			else{
				currentLine++;
			}
		}

		subtitle = AddSubView (new SHGUItext (content, 32 - (int)(longestLine / 2), 19, color)) as SHGUItext;
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
			if (IsTherePopup()){
				popup.Kill();
			}else{
				skipToPhase = phase + 1;
			}
		}


		if (key == SHGUIinput.esc)
			SHGUI.current.PopView ();
	}
	
	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
	{
	    if (fadingOut)
	        return;
	}

	bool IsTherePopup(){
		return !(popup == null || popup.fadingOut || popup.remove);
	}

	void ShowPopup(string phrase){

		if (phase < skipToPhase)
			return;
		if (popupShownThisPhase)
			return;

		if (popup != null)
			popup.Kill ();

		popupShownThisPhase = true;

		popup = new SHGUIview ();
		popup.x = 32 - (int)(phrase.Length / 2);
		popup.y = 11;
		AddSubView (popup);

		popup.AddSubView(new SHGUIrect(-1, -1, phrase.Length, 1));
		popup.AddSubView(new SHGUIframe(-1, -1, phrase.Length, 1, 'r'));
		popup.AddSubView (new SHGUItext (phrase, 0, 0, 'r'));
	}
}


