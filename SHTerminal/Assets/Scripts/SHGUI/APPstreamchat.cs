
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System.Linq;


public class APPstreamchat: SHGUIview
{
	APPscrollconsole console;
	bool instructionsDisplayed = false;

	public static string[] forumStrings;
	public static int currentForumStringIndex = -1;

	bool quitting = false;
	float quittingDelay = .75f;
	bool allowSecretSystemMessages = false;

	public APPstreamchat ()
	{		
		if (forumStrings == null) {
			LoadForumContent();
		}

		if (currentForumStringIndex == -1) {
			currentForumStringIndex = Random.Range(0, forumStrings.Length - 1);
		}

		//LOAD "IS GAME FINISHED" HERE
		allowSecretSystemMessages = false;

		console = new APPscrollconsole ();
		console.ShowChatFrames ();
		console.killOnEmptyQueue = false;
		console.showFadeForChatMessages = true;
		console.desiredFrameWidth = 41;
		console.dontDisplaySender = false;

		console.defaultConsoleCallback = ()=>{NextMessage();};

		console.AddMySystemMessage ("", "^m7^CzLISTENING TO #HACKING");

		/*
		console.AddTextToQueueCentered ("", .1f, 'w');
		console.AddTextToQueueCentered ("", .1f, 'w');
		AsciiArtLineByLine (console, "cubes", .075f, 'w', true);
		console.AddTextToQueueCentered ("", .1f, 'w');
		console.AddTextToQueueCentered ("WELCOME TO THE FORUM", .1f, 'w');
		*/

		(console.appname as SHGUItext).text = "guruGROUPS";
		console.leaveChatLocalizedString = "LEAVE_GROUP_INPUT";
		this.AddSubView(console);
	}

	void LoadForumContent(){
		TextAsset forumDataText = Resources.Load<TextAsset>("GroupChannelContent");
		forumStrings = forumDataText.text.Split('\n');

		List<List<string>> L = new List<List<string>> ();
		List<string> I = new List<string> ();
		for (int i = 0; i < forumStrings.Length; ++i) {
			if (!string.IsNullOrEmpty(forumStrings[i]) && (forumStrings[i][0] == '-')){
				L.Add(I);
				I = new List<string>();
			}
			else{
				I.Add(forumStrings[i]);
			}
		}
		if (I.Count > 0) {
			L.Add(I);
		}
		Random rng = new Random();
		
		List<List<string>> shuffL = L.OrderBy (a => Random.value).ToList();

		forumStrings = new string[forumStrings.Length];
		int counter = 0;
		for (int i = 0; i < shuffL.Count; ++i) {
			for (int j = 0; j < shuffL[i].Count; ++j){
				forumStrings[counter] = shuffL[i][j];
				counter++;
			}
		}

		/*
		for (int i = 0; i < forumStrings.Length; ++i) {
			Debug.Log(forumStrings[i]);
		}
		*/
	}

	float nonInteractionDelay = 2f;
	public override void Update(){
		base.Update ();

		if (fadingOut)
			return;

		if (console.GetPendingMessagesCount () == 0 && (console.isEmptyAndFinished)) {
			if (!quitting)
				NextMessage();
			else{
				quittingDelay -= Time.unscaledDeltaTime;
				if (quittingDelay < 0){
					SHGUI.current.PopView();
				}
			}

			/*
			nonInteractionDelay -= Time.unscaledDeltaTime;
			if ((!instructionsDisplayed) && (nonInteractionDelay < 0)) {
				console.AddTextToQueueCentered ("", .1f, 'w');			

				console.AddTextToQueueCentered (LocalizationManager.Instance.GetLocalized("BROWSE_FORUM_INPUT"), .1f, 'z');

				console.AddTextToQueueCentered ("", .1f, 'w');			
			
				instructionsDisplayed = true;
			}
			*/
		}

		if (console.remove) {
			Kill ();
		}
	}

	void Quit(){
		if (!quitting){
			quitting = true;
			console.AddMySystemMessage ("", "^m7^CzLEAVING #HACKING");
			console.DisplayNextMessage ();
		}
		else{
			SHGUI.current.PopView ();
		}
	}
	
	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;
		
		if (key == SHGUIinput.esc) {
			Quit();
		}
		
		if (key == SHGUIinput.enter) {
			Quit();
		}
	}

	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
	{	
		if (fadingOut)
			return;
		
		if (clicked)
			Quit();
	}

	void NextMessage(){
		nonInteractionDelay = 2f;
		instructionsDisplayed = false;
		if (console.GetPendingMessagesCount () > 0) {
			console.DisplayNextMessage ();
		} else {
			DisplayNextForumMessage();
			console.DisplayNextMessage ();
		}
	}

	void DisplayNextForumMessage(){
		if (currentForumStringIndex > forumStrings.Length - 1) {
			currentForumStringIndex = 0;
		}

		string r = forumStrings[currentForumStringIndex];

		if (!string.IsNullOrEmpty(r)) {
			if (r [0] == '#') {
				if (r [1] == 'T') {
					console.AddMySystemMessage ("", "^m7^Cz" + r.Substring (3));
				}
				if (r [1] == 'j'){
					console.AddMySystemMessage ("", "^m7^CzUSER " + r.Substring(2) + " JOINED");
				}
				if (r [1] == 'b'){
					console.AddMySystemMessage ("", "^m7^CzUSER " + r.Substring(2) + " BANNED");
				}
				if (r [1] == 'l'){
					console.AddMySystemMessage ("", "^m7^CzUSER " + r.Substring(2) + " LEFT");
				}
				if (r [1] == 'k'){
					console.AddMySystemMessage ("", "^m7^CzUSER " + r.Substring(2) + " KICKED");
				}
			}
			else if (r[0] == '!'){
				if (allowSecretSystemMessages){
					console.AddMySystemMessage ("", "^Fr^M2^Cr" + r.Substring (1).ToUpper());
					currentForumStringIndex++;
					DisplayNextForumMessage();
					console.DisplayNextMessage();
					return;
				}
				else{
					currentForumStringIndex++;
					return;
				}
			}
			else {
				int index = r.IndexOf (':');
				if (index <= 0)
					index = 0;

				string a = r.Substring (0, index);
				string b = r.Substring (index + 1);
				console.AddOtherMessage (a, b);		
			}
		} else {
			console.AddWait(1f);
		}

		currentForumStringIndex++;
	}

	protected void AsciiArtLineByLine(APPscrollconsole console, string artname, float lineDelay, char color, bool centered){
		
		string[] lines = SHGUI.current.GetASCIIartByName(artname).Split ('\n');
		
		int centerOffset = (int)(SHGUI.current.resolutionX / 2) - (int)(lines [0].Length / 2);
		
		for (int i = 0; i < lines.Length; ++i) {
			if (centered)
				console.AddTextToQueue(lines[i], lineDelay, color, centerOffset);
			else
				console.AddTextToQueue(lines[i], lineDelay, color);
		}
	}
}


