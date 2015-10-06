using System.Linq;
using InControl;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Xml.Linq;
using Utilities.CameraEffects;

public enum SHGUIinput {none, up, down, left, right, enter, esc, any, scrollUp, scrollDown}
public enum SHGUIInputDevice { Keyboard, Gamepad}

public class SHGUI : MonoBehaviour {

	public static SHGUI current;
	public static int id = 0;

	public int resolutionX = 80;
	public int resolutionY = 24;

	char[] display;
	char[] color;
	bool displayDirty = false;
	char[] background;
	char[] backgroundColor;
	bool backgroundDirty = false;

    public Image Background;
	public Text forgroundText;
	public Text backgroundText;

	public List<SHGUIview> views = new List<SHGUIview>();
	public List<SHGUIview> viewQueue = new List<SHGUIview>();

    SHGUIInputDevice currentInputDevice = SHGUIInputDevice.Keyboard;

	bool cursorActive;
	int cursorX;
	int cursorY;
	float cursorTimer = 0;
	float cursorAnimatorTimer = 0f;

	int cursorAnimator = 0;
	string cursorAnimation = "████▓▓░░▓▓░░▓▓";
	bool cursorVirgin = true;
	
	SHGUIinput lastKey;
	string lastInputString = "";
	float keyTimer = 0;

	public bool finished = false;

    float mouseX = 0f;
    float mouseY = 0f;

    public Image LevelThumbnail;
    public bool OnScreen = false;
	public bool RestrictedAccess = false;
	public int RestrictedAccessCounter = 0;

	// Use this for initialization
	void Start () {
		//Application.targetFrameRate = 60;
	    //TextManager.DisplaySUPERHOTBatch();
		current = this;

		display = new char[resolutionX * resolutionY];
		color = new char[resolutionX * resolutionY];
		background = new char[resolutionX * resolutionY];
		backgroundColor = new char[resolutionX * resolutionY];

		Clear ();

		//InitSounds();

        Shader.WarmupAllShaders();
	}

	public float GetScaleFactor(){
		return this.gameObject.GetComponent<CanvasScaler>().scaleFactor;
	}

	[HideInInspector]
	//public TerminalSounds sounds;

	private void InitSounds(){
		var s = GameObject.Instantiate (Resources.Load ("Sounds/TerminalSoundsPrefab")) as GameObject;
		//sounds = s.GetComponent<TerminalSounds> ();
	}

	public void PlaySound(SHGUIsound sound){
//
//			//			beepping.Play ();
//			sounds.PlayPing ();
//		else if (sound == SHGUIsound.pong)
//			//			beeppong.Play();
//			sounds.PlayPong ();
//		else if (sound == SHGUIsound.redping)
//			//			beepping.Play();
//			sounds.PlayRedPing ();
//		else if (sound == SHGUIsound.redpong)
//			//			beeppong.Play();
//			sounds.PlayRedPong ();
//		
//		else if (sound == SHGUIsound.tick)
//			sounds.PlayCursor();
//		//			sounds.PlayTick ();
//		else if (sound == SHGUIsound.redtick)
//			sounds.PlayRedtick ();
//		else if (sound == SHGUIsound.confirm)
//			sounds.PlayNewLine ();
//		else if (sound == SHGUIsound.wrong)
//			sounds.PlayWrong ();
//		else if (sound == SHGUIsound.download)
//			//			beepping.Play();
//			sounds.PlayDownload ();
//		else if (sound == SHGUIsound.downloaded)
//			//			beepping.Play();
//			sounds.PlayDownloaded ();
//		else if (sound == SHGUIsound.driveloading)
//			sounds.PlayDriveLoading ();
//		else if (sound == SHGUIsound.incomingmessage)
//			//			beepping.Play ();
//			sounds.PlayIncomingMessage ();
//		else if (sound == SHGUIsound.finalscramble)
//			//			beepping.Play ();
//			sounds.PlayFinalScramble ();
//		else if (sound == SHGUIsound.restrictedpopup)
//			//			beepping.Play ();
//			sounds.PlayRestrictedPopup ();
//		else if (sound == SHGUIsound.messageswitch)
//			//			beepping.Play ();
//			sounds.PlayMessageSwitch ();
//		else if (sound == SHGUIsound.noescape)
//			//			beepping.Play ();
//			sounds.PlayNoEscape ();
	}

	public void StopNoiseLoop(){
//		sounds.TurnOffLoopNoise ();
	}

	public void TurnDownNoiseLoop(){
//		sounds.LowerLoopNoise ();
	}

	public static int GetId(){
		return id++;
	}

	public bool IsBackgroundOn(){
		if (Background == null)
			return false;
		return Background.enabled;
	}
	
    public void TurnBackgroundOn(bool doReset = true) {
        if (Background != null)
            Background.enabled = true;
		
		if(doReset)
        	CameraEffectsManager.Instance.ResetAll();
        //UnityEngine.Cursor.lockState = CursorLockMode.None;
        OnScreen = true;
    }

    public void TurnBackgroundOff() {
        if (Background != null)
            Background.enabled = false;
		
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        OnScreen = false;
    }

	public void AddViewOnTop(SHGUIview view){
        if (views.Count > 0 && views.Last() != null)
            views.Last().OnExit();

		SHGUIview v = GetInteractableView();
		if (v == null)
			views.Add (view);
		else{
			int i = GetInteractableViewIndex() + 1;

			views.Insert(i, view);
	
		}
        views.Last().OnEnter();
	}

	public void AddViewToQueue(SHGUIview view){
		viewQueue.Add(view);
	}

	public void AddViewToQueueFront(SHGUIview view){
		viewQueue.Insert(0, view);
	}

	public void PopView(){
        if (views.Count > 0 && views.Last() != null)
            views.Last().OnExit();

		bool seethrough = false;
		SHGUIview interactable = GetInteractableView ();

		if (interactable != null) {
			seethrough = !interactable.dontDrawViewsBelow;	
			interactable.Kill ();
		}

		//SHGUI.current.PlayConfirm();

		if (views.Count == 0) {
			PopViewFromQueue();
		}

		interactable = GetInteractableView ();
		if (interactable != null) {
			if (!seethrough) 
				GetInteractableView ().PunchIn ();
			else
				GetInteractableView ().PunchIn (.9f);
		}
	}

	public SHGUIview PopViewFromQueue(){
		if (viewQueue.Count > 0) {
			SHGUIview q = viewQueue [0];
			AddViewOnTop(q);
			viewQueue.RemoveAt (0);

			return q;
		} else {
			//Debug.Log("finished");
			finished = true;
			return null;
		}

	}

	public void KillAll(){
		for (int i = 0; i < views.Count; ++i) {
			views[i].Kill();
		}
	}

	public SHGUIview LaunchAppByName(string name){
		System.Type t = System.Type.GetType (name);
		if (t != null) {
			SHGUIview view = System.Activator.CreateInstance(t) as SHGUIview;
			AddViewOnTop(view);
			return view;
		} else {
			Debug.Log("app: " + name + " not found");
			return null;
		}
	}

    public void LaunchLuaAppByScriptName(string fileName)
    {
        AddViewOnTop(new APPlua(fileName));
    }

    public void ShowVideo(string videoname){
		//AddViewOnTop (new APPvideo (videoname));
	}

	public void ShowWiresSchem(string wirname){
		AddViewOnTop (new APPwireworld (wirname));
	}

	public void ShowReadmeFile(string label, string content){
		//Debug.Log ("showing readme: " + label + " " + content);
		AddViewOnTop (new APPreadme (label, content));
	}

//	public void LaunchLevelViaApp(string levelname){
//		AddViewOnTop (new APPlaunchlevel (LevelSetup.GetLevelInfoByName(levelname)));
//	}
//
//    public void LaunchLevelViaApp(LevelInfo level){
//        AddViewOnTop(new APPlaunchlevel(level));
//    }

	public void ShowArtFile(string label, string artname, bool centred){
		//Debug.Log ("showing readme: " + label + " " + content);
		AddViewOnTop (new APPascii (label, artname, centred));
	}

	public void LaunchUserLevel(string bundle, string scene){
		var bundleLong = "file://" + Application.dataPath + "/" + bundle;
		APPuserlevel app = new APPuserlevel (bundleLong, scene);

		Debug.Log (bundleLong + " |||||||||||||||| " + bundle);

		SHGUI.current.AddViewOnTop (app);
	}

	public void ShowSegway(string content){
		AddViewOnTop (new APPsegway(content));
	}

	public string GetASCIIartByName(string artname){
		return (Resources.Load ("ASCII/"+artname).ToString());
	}

	public string GetASCIIartFromFont(string text){
		var figlet = new Figlet("Fonts/Alphabet");
		string content = figlet.ToAsciiArt(text.Replace('|', '\n'));
		
		content = content.Replace('A', '█');
		content = content.Replace('B', '█');
		content = content.Replace('C', '█');
		content = content.Replace('D', '█');
		content = content.Replace('E', '█');
		content = content.Replace('F', '█');
		content = content.Replace('G', '█');
		content = content.Replace('H', '█');
		content = content.Replace('I', '█');
		content = content.Replace('J', '█');
		content = content.Replace('K', '█');
		content = content.Replace('L', '█');
		content = content.Replace('M', '█');
		content = content.Replace('N', '█');
		content = content.Replace('O', '█');
		content = content.Replace('P', '█');
		content = content.Replace('Q', '█');
		content = content.Replace('R', '█');
		content = content.Replace('S', '█');
		content = content.Replace('T', '█');
		content = content.Replace('U', '█');
		content = content.Replace('V', '█');
		content = content.Replace('W', '█');
		content = content.Replace('Z', '█');
		content = content.Replace('X', '█');
		content = content.Replace('Y', '█');
		content = content.Replace('a', '█');
		content = content.Replace('b', '█');
		content = content.Replace('c', '█');
		content = content.Replace('d', '█');
		content = content.Replace('e', '█');
		content = content.Replace('f', '█');
		content = content.Replace('g', '█');
		content = content.Replace('h', '█');
		content = content.Replace('i', '█');
		content = content.Replace('j', '█');
		content = content.Replace('k', '█');
		content = content.Replace('l', '█');
		content = content.Replace('m', '█');
		content = content.Replace('n', '█');
		content = content.Replace('o', '█');
		content = content.Replace('p', '█');
		content = content.Replace('q', '█');
		content = content.Replace('r', '█');
		content = content.Replace('s', '█');
		content = content.Replace('t', '█');
		content = content.Replace('u', '█');
		content = content.Replace('v', '█');
		content = content.Replace('w', '█');
		content = content.Replace('z', '█');
		content = content.Replace('x', '█');
		content = content.Replace('y', '█');

		content = content.Replace('0', '█');
		content = content.Replace('1', '█');
		content = content.Replace('2', '█');
		content = content.Replace('3', '█');
		content = content.Replace('4', '█');
		content = content.Replace('5', '█');
		content = content.Replace('6', '█');
		content = content.Replace('7', '█');
		content = content.Replace('8', '█');
		content = content.Replace('9', '█');

		content = content.Replace('.', '█');
		content = content.Replace(',', '█');
		content = content.Replace(':', '█');
		content = content.Replace(';', '█');
		
		
		content = content.Replace('\'', '█');

		return content;
	}

	public SHGUItext GetCenteredAsciiArt(string artname, int screenPosX = 32, int screenPosY = 12 ){
		SHGUItext t = new SHGUItext (GetASCIIartByName(artname), 1, 1, 'w') as SHGUItext;
		t.x =  screenPosX - (int)(t.GetLineLength() / 2); 
		t.y =  screenPosY - (int)(t.CountLines() / 2); 

		return t;
	}
	
	public void Clear(){

		for (int i = 0; i < resolutionX * resolutionY; ++i) {
			display[i] = ' ';
			color[i] = 'g';
			background[i] = ' ';
			backgroundColor[i] = ' ';
		}
	}

	public void SetPixelFront(char C, int x, int y, char col){
		//int pos = x + resolutuionX * y;
		if (x < 0 || x >= resolutionX)
			return;
		if (y < 0 || y >= resolutionY)
			return;

		//this char breakes everything!
		if (C == (char)13 || C == (char)9) C = ' ';

		int pos = x + resolutionX * y;
		display [pos] = C;
		color [pos] = col;
	}

	public char GetPixelFront(int x, int y){
		//int pos = x + resolutuionX * y;
		if (x < 0 || x >= resolutionX)
			return ' ';
		if (y < 0 || y >= resolutionY)
			return ' ';
		
		int pos = x + resolutionX * y;
		return display [pos];	
	}

	public void SetPixelBack(char C, int x, int y, char col){
		//int pos = x + resolutuionX * y;
		if (x < 0 || x >= resolutionX)
			return;
		if (y < 0 || y >= resolutionY)
			return;

		if (C == (char)13 || C == (char)9) C = ' ';
		
		int pos = x + resolutionX * y;
		background [pos] = C;
		backgroundColor [pos] = col;	
	}

	public void DrawLine(string style, int startpos, int endpos, int colpos, bool horizontal, char col, float fade = 1){
		char c;

		fade += 0.5f;
		fade = Mathf.Clamp01 (fade);
		int newstartpos = (int)( startpos + (endpos - startpos) * (1 - fade));
		int newendpos = (int)( endpos + (startpos - endpos) * (1 - fade));

		if (newstartpos == newendpos) {
			newendpos-=1;
			//newstartpos -=1;
			style ="+++";
		}

		if (horizontal) {

			for (int i = newstartpos; i <= newendpos; ++i){
				if (i == newstartpos) c = style[0];
				else if (i == newendpos) c = style[2];
				else c = style[1];

				if (Random.value < fade)
					SetPixelFront(c, i, colpos, col);
			}
		} else {
			for (int i = newstartpos; i <= newendpos; ++i){
				if (i == newstartpos) c = style[0];
				else if (i == newendpos) c = style[2];
				else c = style[1];
				if (Random.value < fade)
					SetPixelFront(c, colpos, i, col);
			}
		}
	}

	void ClearRect(int startx, int starty, int endx, int endy, float fade = 1){
		for (int x = (int)(startx * fade); x < (int)(endx * fade); ++x) {
			for (int y = (int)(starty * fade); y < (int)(endy * fade); ++y) {
				if (UnityEngine.Random.value < fade)
					SetPixelFront(' ', x, y, ' ');
				else
					SetPixelFront(StringScrambler.GetGlitchChar(), x, y, ' ');
			}
		}
	}

	public void DrawText(string text, int x, int y, char col, float fade = 1, char backColor = ' '){
		int xoff = x;
		int yoff = 0;
		text = StringScrambler.GetScrambledString (text, 1 - fade);
		for (int i = 0; i < text.Length; ++i) {
			if (text[i] == '\n' || (int)text[i] == 10){
				xoff = x;
				yoff++;
			}
			else{
				if (Random.value < fade){
					if (text[i] != ' '){
						SetPixelFront(text[i], xoff, y + yoff, col);
						if (backColor != ' '){
							SetPixelBack('█', xoff, y + yoff, backColor);
						}
					}
				}
				xoff++;
			}
		}

		//DrawRectBack (x, y, x + text.Length, y + 1, 'g');
	}

	public void DrawTextSkipSpaces(string text, int x, int y, char col, float fade = 1, char backColor = ' '){
		int xoff = x;
		int yoff = 0;
		text = StringScrambler.GetScrambledString (text, 1 - fade);
		for (int i = 0; i < text.Length; ++i) {
			if (text[i] == '\n' || (int)text[i] == 10){
				xoff = x;
				yoff++;
			}
			else{
				if (Random.value < fade){
					if (text[i] != ' '){
						SetPixelFront(text[i], xoff, y + yoff, col);
						if (backColor != ' '){
							SetPixelBack('█', xoff, y + yoff, backColor);
						}
					}
				}
				xoff++;
				
			}
		}
		
		//DrawRectBack (x, y, x + text.Length, y + 1, 'g');
	}

	public void DrawBlack(string text, int x, int y){
		int xoff = x;
		int yoff = 0;
		text = StringScrambler.GetScrambledString (text, 1 - 1);
		for (int i = 0; i < text.Length; ++i) {
			if (text[i] == '\n' || (int)text[i] == 10){
				xoff = x;
				yoff++;
			}
			else{
				if (Random.value < 1){
					if (text[i] != ' '){
						SetPixelFront(' ', xoff, y + yoff, 'x');
					}
				}
				xoff++;
				
			}
		}
		
		//DrawRectBack (x, y, x + text.Length, y + 1, 'g');
	}
	
	public void DrawRectBack(int startx, int starty, int endx, int endy, char col, float fade = 1){

		for (int x = startx; x < endx; ++x) {
			for (int y = starty; y < endy; ++y){
				if (Random.value < fade) 
					SetPixelBack('█', x, y, col); 
				else 
					SetPixelBack(' ', x, y, col); 
			}
		
		}
	}

	int waiter = 0;
	int scriptsLoaded = 5;
	// Update is called once per frame
	void Update () {
		if (Time.unscaledDeltaTime < .3f) {
			scriptsLoaded--;
		}

		if (scriptsLoaded > 0)
			return;

		if (views.Count == 0) {
			PopViewFromQueue ();
			//return;
		}

        
		waiter--;
		if (waiter > 0)
			return;
		waiter = 0;

		Clear ();

		int startIndex = 0;
		for (int i = views.Count -1; i >= 0; --i) {
			if (views[i].dontDrawViewsBelow){
				startIndex = i;
				break;
			}
		}

		for (int i = startIndex; i < views.Count; i++) {
			views[i].Update();
			views[i].FadeUpdate();
			views[i].Redraw(0, 0);
		}

		ReactToInputKeyboard ();
		ReactToInputMouse ();

		for (int i = views.Count -1; i >= 0; --i) {
			if (views[i].remove) {
				views.RemoveAt(i);
				break;
			}
		}

        if (OnScreen == false)
            return;

		SHGUIview interactable = GetInteractableView ();

        int YLineOld = cursorY;
        int XLineOld = cursorX;

	    int mouseLocalX = (int)((Input.mousePosition.x / Screen.width) * 80.0f);
        int mouseLocalY = (int)(((Screen.height - Input.mousePosition.y) / Screen.height) * 24.0f);

	    cursorX = mouseLocalX;
        cursorY = mouseLocalY;

        mouseX += Input.GetAxis("mouse x");
        mouseY -= Input.GetAxis("mouse y");

       // UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = false;

	    cursorX = Mathf.Clamp(cursorX, 0, 79);
        cursorY = Mathf.Clamp(cursorY, 0, 23);

        if (YLineOld != cursorY || XLineOld != cursorX)
        {
			cursorActive = true;
			//cursorAnimator = 0;
			cursorTimer = 5f;

			if (!cursorVirgin){
				if (interactable != null) interactable.SpeedUpFadeIn();
			}
			cursorVirgin = false;
		}

		cursorTimer -= Time.unscaledDeltaTime;
		if (cursorTimer < 0) {
			cursorActive = false;
		}

		if (cursorActive && interactable != null && interactable.allowCursorDraw) {
			cursorAnimatorTimer -= Time.unscaledDeltaTime;

			if (cursorAnimatorTimer < 0){
				cursorAnimatorTimer = .02f;
				cursorAnimator++;
				if (cursorAnimator > cursorAnimation.Length - 1)
					cursorAnimator = 0;
				
			}

			if (stringContainsChar(GetPixelFront(cursorX, cursorY), cursorFrontingChars))
          		SHGUI.current.SetPixelFront(cursorAnimation[cursorAnimator], cursorX, cursorY, 'r');
			else
          		SHGUI.current.SetPixelBack(cursorAnimation[cursorAnimator], cursorX, cursorY, 'r');
			//SHGUI.current.SetPixelFront (cursorAnimator.ToString()[0], XLine, YLine, 'r');
		}

        

		PrintBackground ();
		PrintForground ();
	}
	
	const string cursorFrontingChars = "▀▄█▌░▒▓■▪";
	bool stringContainsChar(char c, string str){
		for (int i = 0; i < str.Length; ++i){
			if (str[i] == c)
				return true;
		}
		return false;
	}			 

	void ReactToInputKeyboard(){
		if (GetInteractableView () == null)
			return;
		string currentInputString = Input.inputString;

	    float vertical = 0.0f;
	    float horizontal = 0.0f;

        foreach (var device in InputManager.Devices)
        {
            horizontal += device.Name == "Keyboard/Mouse" ? device.LeftStickX : Mathf.Pow(Mathf.Abs(device.LeftStickX), 2f) * Mathf.Sign(device.LeftStickX);
            vertical += device.Name == "Keyboard/Mouse" ? device.LeftStickY : Mathf.Pow(Mathf.Abs(device.LeftStickY), 2f) * Mathf.Sign(device.LeftStickY);
        }

	    horizontal += InputManager.ActiveDevice.DPadLeft.IsPressed ? -1.0f : 0.0f;
        horizontal += InputManager.ActiveDevice.DPadRight.IsPressed ? 1.0f : 0.0f;

        vertical += InputManager.ActiveDevice.DPadUp.IsPressed ? 1.0f : 0.0f;
        vertical += InputManager.ActiveDevice.DPadDown.IsPressed ? -1.0f : 0.0f;

		SHGUIinput key = SHGUIinput.none;
        //float vertical = Input.GetAxisRaw ("Vertical");
        //float horizontal = Input.GetAxisRaw ("Horizontal");
        bool enter = Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || (InputManager.ActiveDevice.Action1.WasReleased && Time.unscaledDeltaTime < 0.1f);
		bool esc = Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape) || (InputManager.ActiveDevice.Action2.WasPressed && InputManager.ActiveDevice.Name!="Keyboard/Mouse") || InputManager.ActiveDevice.GetControl(InputControlType.Start).WasPressed;


		if (vertical < -0.5f) 
			key = SHGUIinput.down;
		if (vertical > 0.5f)
			key = SHGUIinput.up;

		if (horizontal > 0.5f) 
			key = SHGUIinput.right;
		if (horizontal < -0.5f)
			key = SHGUIinput.left;
		
		if (enter) 
			key = SHGUIinput.enter;

		if (esc)
			key = SHGUIinput.esc;

		if (key != SHGUIinput.none) {
			cursorActive = false;
		}

		if (!enter && !esc && currentInputString != "" && currentInputString != lastInputString ) {
			key = SHGUIinput.any;
		}

		//check for random gamepad mashing

		if (key == SHGUIinput.none && InputManager.ActiveDevice.Name!="Keyboard/Mouse") {
			if(InputManager.ActiveDevice.LeftStick.HasChanged || InputManager.ActiveDevice.RightStick.HasChanged ||
			   InputManager.ActiveDevice.LeftTrigger.HasChanged || InputManager.ActiveDevice.RightTrigger.HasChanged ||
			   InputManager.ActiveDevice.LeftBumper.WasPressed || InputManager.ActiveDevice.RightBumper.WasPressed ||
			   InputManager.ActiveDevice.AnyButton.WasPressed)
				key = SHGUIinput.any;

		}

		SHGUIinput k = key;
		if (lastKey == key) {
			keyTimer += Time.unscaledDeltaTime;
			if (keyTimer > .3f){
				keyTimer = .275f;

			}
			else{
				key = SHGUIinput.none;
			}
		} else {
			keyTimer = 0;
		}

		lastKey = k;

		SHGUIview interactable = GetInteractableView ();
		interactable.ReactToInputKeyboard (key);
		if (!string.IsNullOrEmpty(Input.inputString))
			lastInputString = Input.inputString;
		//views [views.Count - 1].ReactToInput (key);
	}

    void ReactToInputGamepad() {
        if (GetInteractableView () == null)
			return;
    }

	void ReactToInputMouse(){
		if (!cursorActive)
			return;
		if (GetInteractableView () == null)
			return;

	    SHGUIinput scrollInput = SHGUIinput.none;
        if(Input.mouseScrollDelta.y > 0.0f)
            scrollInput = SHGUIinput.scrollUp;
        else if (Input.mouseScrollDelta.y < 0.0f)
            scrollInput = SHGUIinput.scrollDown;

        GetInteractableView().ReactToInputMouse(cursorX, cursorY, Input.GetMouseButtonUp(0), scrollInput);
	}

	public SHGUIview GetInteractableView(){
		if (views.Count == 0)
			return null;
		for (int i = views.Count - 1; i >= 0 ; --i) {
			if (!views[i].fadingOut && !views[i].remove && views[i].interactable) return views[i];
		}
		return views [0];
	}

	public int GetInteractableViewIndex(){
		if (views.Count == 0)
			return 0;
		for (int i = views.Count - 1; i >= 0 ; --i) {
			if (!views[i].fadingOut && !views[i].remove && views[i].interactable) return i;
		}
		return 0;
	}

	public int GetProperViewCount(){
		int count = 0;

		for (int i = 0; i < views.Count; ++i){
			if (!views[i].fadingOut && !views[i].remove){
				count++;
			}
		}

		return count;
	}

	
	void PrintForground(){

		System.Text.StringBuilder str = new System.Text.StringBuilder(resolutionX * resolutionY * 2);
		string finalText = "";
		char lastColor = ' ';
		bool firsttag = true;
		int charinLineCount = 0;
        str.Append('\n');
		for (int i = 0; i < display.Length; ++i) {
			if (lastColor != color[i] && display[i] != ' '){
				if (!firsttag) {
					str.Append("</color>");
					//finalText += "</color>";
				}
				

				str.Append("<color=");
				str.Append(GetColorFromChar(color[i]));
				str.Append(">");
				str.Append(display[i]);
				//finalText += "<color=" + GetColorFromChar(color[i]) + ">" + display[i];
				
				lastColor = color[i];
				firsttag = false;
			}
			else{
				str.Append(display[i]);
				//finalText += display[i];
			}
			charinLineCount++;
			if (charinLineCount >= resolutionX){
				//finalText += "\n";
				str.Append("\n");
				
				charinLineCount = 0;
			}
		}
		
		if (!firsttag) {
			//finalText += "</color>";
			str.Append("</color>");
		}
		forgroundText.text = str.ToString ();
	}

	void PrintBackground(){
		
		System.Text.StringBuilder str = new System.Text.StringBuilder(resolutionX * resolutionY * 2);
		string finalText = "";
		char lastColor = ' ';
		bool firsttag = true;
		int charinLineCount = 0;
	    str.Append('\n');
		for (int i = 0; i < background.Length; ++i) {
			if (lastColor != backgroundColor[i] && background[i] != ' '){
				if (!firsttag) {
					str.Append("</color>");
					//finalText += "</color>";
				}
				
				
				str.Append("<color=");
				str.Append(GetColorFromChar(backgroundColor[i]));
				str.Append(">");
				str.Append(background[i]);
				//finalText += "<color=" + GetColorFromChar(color[i]) + ">" + display[i];
				
				lastColor = backgroundColor[i];
				firsttag = false;
			}
			else{
				str.Append(background[i]);
				//finalText += display[i];
			}
			charinLineCount++;
			if (charinLineCount >= resolutionX){
				//finalText += "\n";
				str.Append("\n");
				
				charinLineCount = 0;
			}
		}
		
		if (!firsttag) {
			//finalText += "</color>";
			str.Append("</color>");
		}
		backgroundText.text = str.ToString ();
	}

	private string GetColorFromChar(char c){
		string colString = "#000000ff";
		//if (c == 'r') colString = "#aa0303";
		if (c == 'r')
			colString = "#ff0000ff";
		else if (c == 'g')
			colString = "#00ff00ff";
		else if (c == 'w')
			colString = "#ffffffff";
		else if (c == 'b')
			colString = "#0000ffff";
		else if (c == 'z')
			colString = "#888888ff";
		else if (c == '0')
			colString = "#000000ff";
		else if (c == '1')
			colString = "#222222ff";
		else if (c == '2')
			colString = "#444444ff";
		else if (c == '3')
			colString = "#66666600";
		else if (c == '4')
			colString = "#888888ff";
		else if (c == '5')
			colString = "#aaaaaaff";
		else if (c == '6')
			colString = "#ccccccff";
		else if (c == '7')
			colString = "#eeeeeeff";
		else if (c == '8')
			colString = "#ffffffff";
		else if (c == '9')
			colString = "#ffffffff";
		else if (c == 't')
			colString = "#ffffff00";
		
		return colString;
	}
}
