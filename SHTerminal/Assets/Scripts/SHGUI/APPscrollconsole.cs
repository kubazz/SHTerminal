using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using InControl;

class scrollmessage{
	public SHGUIview view;
	public int height;
	public float delay;
    public bool overrideLast;
    public float baseFade;

	public string controlCommand = "";

	public scrollmessage (SHGUIview View, int Height, float Delay, bool overrideLast = false, float baseFade = 0.0f, string controlCommand = ""){
		view = View;
		height = Height;
		delay = Delay;
	    this.overrideLast = overrideLast;
	    this.baseFade = baseFade;
		this.controlCommand = controlCommand;
	}
}

public class APPscrollconsole: SHGUIview
{
	public int lines = 0;
	public int maxlines = 22;
	public int desiredFrameWidth = 35;
	public int frameOffset = 0;
	private int chatMargin = 3;

	private List<SHGUIview> messages;
	private List<scrollmessage> queue;
	
	protected float delay = 0;
	private int startQueueCount = -100000;

	private SHGUItext instructions;	
	private SHGUItext chatQuitInstructions;		
	public SHGUIview appname;
	public SHGUIview clock;
	public SHGUIview frame;

	private float noPropmterInteractionTime = 0;
	private int instructionsAnchorX;
	private int instructionsAnchorY;

	private const string typeInstruction = "TYPE TO HACK";
	private const string enterInstruction = "ENTER TO EXECUTE";

	public bool skippable = true;
	public bool customSkip = false;
	public List<String> customSkipMessages;
	public float customSkipTimeout = 1f;
	private bool skipping = false;

	public bool killOnEmptyQueue = true;
	public bool showFadeForChatMessages = false;
	public string leaveChatLocalizedString = "LEAVE_CHAT_INPUT";
	public Action defaultConsoleCallback = null;
	public bool dontDisplaySender = true;

	public APPscrollconsole (){
		//AddSubView (new SHGUIframe (0, 0, SHGUI.current.resolutionX - 1, SHGUI.current.resolutionY - 1, 'z'));

		messages = new List<SHGUIview>();
		queue = new List<scrollmessage>();
		maxlines = SHGUI.current.resolutionY - 2;

		instructionsAnchorX = SHGUI.current.resolutionX - 2;
		instructionsAnchorY = SHGUI.current.resolutionY - 1;
		instructions = new SHGUItext("", instructionsAnchorX, instructionsAnchorY, 'z');
		AddSubView(instructions);

		allowCursorDraw = false;
	}

	public void ShowChatFrames(){
		frame = AddSubView (new SHGUIframe (0, 0, SHGUI.current.resolutionX - 1, SHGUI.current.resolutionY - 1, 'z'));
		appname = AddSubView (new SHGUItext ("guruCHAT", 3, 0, 'w'));
		chatQuitInstructions = AddSubView (new SHGUItext ("", SHGUI.current.resolutionX - 22, SHGUI.current.resolutionY - 1, 'z')) as SHGUItext;
		clock = AddSubView(new SHGUIclock(77, 0, 'w'));

		frameOffset = 1;

		lines = 1;
	}

	public void HideChatLabels(){
		//if (frame != null) frame.Kill ();
		if (appname != null) appname.Kill ();
		if (instructions != null) instructions.Kill ();		
		if (chatQuitInstructions != null) chatQuitInstructions.Kill ();
		if (clock != null) clock.Kill ();
	}

	public void HideChatFrames(){
		if (frame != null) frame.Kill ();
		HideChatLabels ();

		frameOffset = 0;
	}

	private void UpdateInstructions(string newtext){
		if (instructions.text != newtext){
			instructions.PunchIn(0f);
		}
		instructions.text = newtext;
		instructions.x = instructionsAnchorX;
		instructions.y = lines + 1;
		instructions.GoFromRight();
	}

	private void SetChatQuitInstructions(string newtext){
		if (chatQuitInstructions == null)
			return;

		chatQuitInstructions.color = 'z';

		if (chatQuitInstructions.text != newtext){
			chatQuitInstructions.PunchIn(0f);
		}

		chatQuitInstructions.text = newtext;
		chatQuitInstructions.x = SHGUI.current.resolutionX - 5;
		chatQuitInstructions.GoFromRight ();	
	}

	private void DisplayScrollMessage(scrollmessage m){
		if (!String.IsNullOrEmpty (m.controlCommand)) {
			ParseCommand(m.controlCommand);
			return;
		}

		if (skipping == true) {
			return;
		}

	    if (m.overrideLast) {
			SHGUI.current.PlaySound(SHGUIsound.messageswitch);
	        var lastMessage = messages.Last();

            lastMessage.KillInstant();
			//Debug.Log("killing last");
	        if (lastMessage is SHGUItext)
	            lines -= (lastMessage as SHGUItext).CountLines();
			else if (lastMessage is SHGUIguruchatwindow){
				lines -= (lastMessage as SHGUIguruchatwindow).height;
			}
	    }

		messages.Add(m.view);
		AddSubViewBottom(m.view);
		m.view.overrideFadeInSpeed = .75f;

		if (showFadeForChatMessages) {
			m.view.ForceFadeRecursive(0f);
			m.view.overrideFadeInSpeed = .45f;
		}

		m.view.y = lines;
	    m.view.fade = m.baseFade;
		delay = m.delay;

		lines += m.height;

		noPropmterInteractionTime = 0;

		if (m.view is SHGUIguruchatwindow && m.overrideLast) {
			(m.view as SHGUIguruchatwindow).ShowInstantPunchIn();
		}

		if (!(m.view is SHGUIprompter || m.view is SHGUIguruchatwindow)){
			SHGUI.current.PlaySound(SHGUIsound.tick);			
		}
	}

	private void ParseCommand(string command){
		if (command == "skiphere"){
			skippable = false;
			skipping = false;
		}
		
		if (command == "nolabels"){
			HideChatLabels();
		}
		
		if (command == "noframes"){
			HideChatFrames();
		}
	}
	
	protected void DisplayEmptyMessage(){
		scrollmessage m = new scrollmessage(new SHGUIview(), 1, .2f);
		DisplayScrollMessage(m);
	}

	protected void KillMessages(){
		for (int i =0; i < messages.Count; ++i){
			messages[i].Kill();
		}
	}
	
	public void AddTextToQueue(string Text, float Delay, char color = 'z', int offset = 0){

		SHGUItext TextView = new SHGUItext(Text, frameOffset + offset, 0, color);
		queue.Add(new scrollmessage(TextView, TextView.CountLines(), Delay, false, .5f));
	}

	public void AddTextToQueueBreakLines(string Text, float Delay, char color = 'z', int offset = 0){
		
		SHGUItext TextView = new SHGUItext(Text, frameOffset + offset, 0, color);
		TextView.BreakCut (SHGUI.current.resolutionX - frameOffset, 100);
		queue.Add(new scrollmessage(TextView, TextView.CountLines(), Delay, false, .5f));
	}

	public void AddTextToQueueCentered(string Text, float Delay, char color = 'z'){
		SHGUItext TextView = new SHGUItext(Text, 0, 0, color);
		TextView.x = frameOffset + (int)(SHGUI.current.resolutionX / 2) - (int)(TextView.GetLineLength () / 2);
		queue.Add(new scrollmessage(TextView, TextView.CountLines(), Delay, false, .5f));
	}

	public void AddEmptyLine(float Delay){
		AddTextToQueue ("", Delay);
	}

	public void AddWait(float Delay){
		queue.Add(new scrollmessage(new SHGUIview(), 0, Delay));
	}

	public void AddPrompterToQueue(string Text, float Delay, bool centered = false){
		SHGUItext TextView = new SHGUItext(Text, frameOffset, 0, 'z');
		TextView.BreakCut(SHGUI.current.resolutionX - 2 - frameOffset, 100);
		int width = TextView.GetLongestLineLength ();

		SHGUIprompter promp = new SHGUIprompter(frameOffset, 0, 'w');

		promp.SetInput(Text);
		if (centered) {
			promp.x = (int)(SHGUI.current.resolutionX / 2) - (int)(promp.GetFirstLineLengthWithoutSpecialSigns() / 2);
		}
		promp.maxLineLength = SHGUI.current.resolutionX - 2;
		promp.maxSmartBreakOffset = 0;
		queue.Add(new scrollmessage(promp, TextView.CountLines(), Delay));
	}

	public void AddPrompterToQueueFaster(string Text, float Delay, bool centered = false){
		SHGUItext TextView = new SHGUItext(Text, frameOffset, 0, 'z');
		TextView.BreakCut(SHGUI.current.resolutionX - 2 - frameOffset, 100);
		int width = TextView.GetLongestLineLength ();
		
		SHGUIprompter promp = new SHGUIprompter(frameOffset, 0, 'w');
		
		promp.SetInput(Text);
		if (centered) {
			promp.x = (int)(SHGUI.current.resolutionX / 2) - (int)(promp.GetFirstLineLengthWithoutSpecialSigns() / 2);
		}
		promp.maxLineLength = SHGUI.current.resolutionX - 2;
		promp.maxSmartBreakOffset = 0;
		promp.baseCharDelay /= 2;
		queue.Add(new scrollmessage(promp, TextView.CountLines(), Delay));
	}

	public void AddInteractivePrompterToQueue(string Text, string prefix = ""){


		SHGUItext TextView = new SHGUItext(Text, frameOffset, 0, 'w');
		TextView.BreakCut(SHGUI.current.resolutionX - 2, 100);
		
		SHGUIprompter promp = new SHGUIprompter(frameOffset, 0, 'w');
		promp.SetInput(Text);
		promp.SwitchToManualInputMode();
		promp.maxLineLength = SHGUI.current.resolutionX - 2 - frameOffset;
		promp.maxSmartBreakOffset = 0;
		promp.AddPrefix(prefix);
		queue.Add(new scrollmessage(promp, TextView.CountLines(), 0));
	}

	private scrollmessage AddChatMessage(string sender, string message, bool leftright, bool interactive, bool poor, bool overrideLast = false){
		if (dontDisplaySender) 
			sender = "";

		SHGUIguruchatwindow chat;
		chat = new SHGUIguruchatwindow ();
		chat.SetLeftRight(leftright);
		if (interactive)
			chat.SetInteractive ();
		if (poor)
			chat.poorMode = true;
		
		chat.SetWidth (desiredFrameWidth);
		chat.SetContent (message);
		chat.SetLabel (sender);

		chat.x = chatMargin;
		if (!leftright) {
			chat.x = SHGUI.current.resolutionX - chatMargin - chat.width;
		}

		int h = chat.GetHeightOfCompleteTextWithFrameVERYSLOWandMOODY();
		var msg = new scrollmessage (chat, h, 0, false, 0);
		msg.overrideLast = overrideLast;
		queue.Add(msg);

		if (defaultConsoleCallback != null) {
			chat.SetCallback(defaultConsoleCallback);
		}
		//chat.ForceFadeRecursive (1f);
		

		return msg;
	}

	public void AddControlCommand(string command){
		queue.Add(new scrollmessage(new SHGUIview(), 0, 0, false, 1f, command));
	}

	public void AddMyMessage(string sender, string message){
		AddChatMessage(sender, message, true, true, false);
	}

	public void AddMyMessageChatOverride(string sender, string message){
		AddChatMessage (sender, message, true, true, false, true);
	}
	
	public void AddOtherMessage(string sender, string message){
		AddChatMessage(sender, message, false, false, false);
	}
	
	public void AddMySystemMessage(string sender, string message){
		AddChatMessage(sender, message, true, false, true);		
	}
	
	public void AddOtherSystemMessage(string sender, string message){
		AddChatMessage(sender, message, false, false, true);		
	}
	
	public void AddMyQuit(){
		AddControlCommand ("skiphere");
		AddControlCommand ("nolabels");
		string chatEndedString = "---CHAT-ENDED---";
		SHGUItext v = new SHGUItext(chatEndedString, 0, 0, 'w'); 
		v.x = frameOffset + (int)(SHGUI.current.resolutionX / 2) - (int)(v.GetLineLength () / 2) - 1;
		AddMessageToQueue (new SHGUIview(), 1, 0f, false);
		AddMessageToQueue (v, 1, 0f, false);
		AddMessageToQueue (new SHGUIview(), 1, 1.75f, false);
		
	}

	public void AddOtherQuit(){
		AddControlCommand ("skiphere");
		AddControlCommand ("nolabels");
		string chatEndedString = "---CHAT-ENDED---";
		SHGUItext v = new SHGUItext(chatEndedString, 0, 0, 'w'); 
		v.x = frameOffset + (int)(SHGUI.current.resolutionX / 2) - (int)(v.GetLineLength () / 2) - 1;
		AddMessageToQueue (new SHGUIview(), 1, 0f, false);
		AddMessageToQueue (v, 1, 0f, false);
		AddMessageToQueue (new SHGUIview(), 1, 1.75f, false);
		
	}

	public void AddMessageToQueue(SHGUIview View, int Height, float Delay, bool overrideLast = false, float baseFade = 0.0f){
		queue.Add(new scrollmessage(View, Height, Delay, overrideLast, baseFade));
	}

	private void ShiftAllMessages(int offy){
		for (int i = 0; i < messages.Count; ++i){
			messages[i].y += offy;
		}

		for (int i = 0; i < messages.Count; ++i){
			if (messages[i].y < -20){
				messages[i].Kill();
				messages.RemoveAt(i);
				i--;
			}
		}

		lines +=offy;
	}
	
	public override void Update(){
		base.Update();
		
		if (startQueueCount < 0){
			startQueueCount = queue.Count;
		}

		delay -= Time.unscaledDeltaTime;
		if (lines > maxlines){
			ShiftAllMessages(-1);
		}

		if (customSkipTimeout < 0) {
			if (skippable) {
				SetChatQuitInstructions (LocalizationManager.Instance.GetLocalized(leaveChatLocalizedString));
			} else {
				SetChatQuitInstructions ("");
			}
		} else {
			customSkipTimeout -= Time.unscaledDeltaTime;
		}

		bool waitForMessageToEnd = false;

		if (messages.Count > 0){
			if (messages[messages.Count - 1] as SHGUIprompter != null && (messages[messages.Count - 1] as SHGUIprompter).manualUpdate){
				waitForMessageToEnd = true;
				SHGUIprompter prom = messages[messages.Count - 1] as SHGUIprompter;
				if (prom.noInteractionTimer > 1.5f){
					if (prom.IsAlmostFinished()){
						UpdateInstructions(enterInstruction);
					}
					else{
						UpdateInstructions(typeInstruction);
					}
				}
				else{
					UpdateInstructions("");
				}
			}
			if (messages[messages.Count - 1] as SHGUIguruchatwindow != null)
				waitForMessageToEnd = true;
		}	

		if (waitForMessageToEnd){
			if ((messages[messages.Count - 1] as SHGUIprompter != null) && (messages[messages.Count - 1] as SHGUIprompter).IsFinished()){
				DisplayNextMessage();
			}
			else if (messages[messages.Count - 1] as SHGUIguruchatwindow != null){
				SHGUIguruchatwindow g = messages[messages.Count - 1] as SHGUIguruchatwindow;
				if (g.finished == true)
					DisplayNextMessage();
			}
		}
		else{
			UpdateInstructions("");
			if (delay < 0)
				DisplayNextMessage();
		}

		for (int i = 0; i < messages.Count; ++i) {
			SHGUIguruchatwindow g = messages[i] as SHGUIguruchatwindow;

			if (g != null){
				g.x = chatMargin;
				if (!g.leftright) {
					g.x = SHGUI.current.resolutionX - chatMargin - g.width;
				}
			}

		}

	}

	private ulong lastKey=0;

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;

		base.ReactToInputKeyboard (key);

		if (key == SHGUIinput.any){
			if (instructions.text == enterInstruction){
				instructions.PunchIn(.5f);
			}
		}

		if (key == SHGUIinput.esc && InputManager.CurrentTick - lastKey > 25) {
			SkipConsole ();
		}
		if(key!= SHGUIinput.none && key != SHGUIinput.esc)
			lastKey = InputManager.CurrentTick;
	}

	public void AddCustomSkipMessage(string msg){
		customSkip = true;
		if (customSkipMessages == null)
			customSkipMessages = new List<string> ();
		this.customSkipMessages.Add (msg);
	}
	
	private void SkipConsole(){
		if (!skipping && skippable) {
			if (customSkip){
				if (customSkipMessages != null && customSkipMessages.Count > 0)
					SetChatQuitInstructions(customSkipMessages[UnityEngine.Random.Range(0, customSkipMessages.Count)]);
				if (customSkipTimeout > 0)
					chatQuitInstructions.color = 'r';
				else
					chatQuitInstructions.color = 'z';

				customSkipTimeout = 2f;

				SHGUI.current.PlaySound(SHGUIsound.noescape);
				
				return;
			}

			if (messages.Count > 0 && (messages[messages.Count - 1] is SHGUIguruchatwindow)){
				(messages[messages.Count - 1] as SHGUIguruchatwindow).Stop ();
			}
			skipping = true;
		}
	}

	public bool isEmptyAndFinished = false;
	public void DisplayNextMessage(){
		if (queue.Count > 0){
			DisplayScrollMessage(queue[0]);
			
			queue.RemoveAt(0);
			isEmptyAndFinished = false;
		}
		else{
			if (killOnEmptyQueue)
				Kill ();
			isEmptyAndFinished = true;
		}
	}

	public float GetProgress(){
		//Debug.Log("que: " + queue.Count + "/ " + startQueueCount);
		return ((float)queue.Count / (float)startQueueCount);
	}

    public int GetPendingMessagesCount() {
        return queue.Count;
    }

	public void ClearMessagesAbove(int y){
		for (int i = 0; i < messages.Count; ++i){
			if (messages[i].y < y){
				messages[i].Kill();
			}
		}
	}
}
