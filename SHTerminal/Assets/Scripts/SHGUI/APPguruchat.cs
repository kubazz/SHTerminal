using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

class gurumessage{
	public string sender;
	public string message;
	public bool leftright;
	public bool interactive;
	public bool isQuit;
	public bool isPoor;

	public gurumessage (String Sender, String Message, bool LeftRight, bool Interactive, bool IsQuit, bool IsPoor){
		sender = Sender;
		message = Message;
		leftright = LeftRight;
		interactive = Interactive;
		isQuit = IsQuit;
		isPoor = IsPoor;
	}
}

public class APPguruchat: SHGUIview
{
	SHGUIguruchatwindow lastChat;
	SHGUItext instructions;
	SHGUIview appname;
	SHGUIview clock;
	SHGUIview frame;
	
	List<SHGUIguruchatwindow> chats;
	int totalOff = 0;

	List<gurumessage> messageQueue;

	int lines = 1;
	bool quiting = false;
	public bool skippable = true;

	public APPguruchat ()
	{
		Init ();
		allowCursorDraw = false;
		chats = new List<SHGUIguruchatwindow> ();

		frame = AddSubView (new SHGUIframe (0, 0, SHGUI.current.resolutionX - 1, SHGUI.current.resolutionY - 1, 'z'));
		appname = AddSubView (new SHGUItext ("guruCHAT", 3, 0, 'w'));
		instructions = AddSubView (new SHGUItext ("", SHGUI.current.resolutionX - 22, SHGUI.current.resolutionY - 1, 'w')) as SHGUItext;
		clock = AddSubView(new SHGUIclock(77, 0, 'w'));

		messageQueue = new List<gurumessage> ();
		//AddChatMessage ("asdfasdf", "", true);

		/*
		SetInstructions ("WAIT-FOR-YOUR-PARTNER");
		SetInstructions ("TYPE-TO-SPEAK");
		SetInstructions ("PRESS-ENTER-TO-END-YOUR-TURN");
		*/
	}

	public void AddMyMessage(string sender, string message){
		messageQueue.Add (new gurumessage (sender, message, true, true, false, false));
	}

	public void AddOtherMessage(string sender, string message){
		messageQueue.Add (new gurumessage (sender, message, false, false, false, false));
	}

	public void AddMySystemMessage(string sender, string message){
		messageQueue.Add (new gurumessage (sender, message, true, false, false, true));
	}

	public void AddOtherSystemMessage(string sender, string message){
		messageQueue.Add (new gurumessage (sender, message, false, false, false, false));
	}

	public void AddMyQuit(){
		messageQueue.Add (new gurumessage ("User56755548", "^CrUSER LEFT CHAT^W8^W8", true, false, true, true));
	}
	
	public override void Update(){
		base.Update ();
		if (fadingOut)
			return;
	
		if ((lastChat == null || lastChat.finished)) {
			if (messageQueue.Count > 0){
				gurumessage m = messageQueue[0];
				messageQueue.RemoveAt(0);
				AddChatMessage(m.sender, m.message, m.leftright, m.interactive, m.isPoor);

				if (m.isQuit)
					quiting = true;
			}
			else{
				Kill ();
				return;
			}
		}

		if (quiting) {
			instructions.Kill();
			appname.Kill();
			clock.Kill();
			//frame.Kill();

			/*
			for (int i = 1; i < children.Count; ++i){
				children[i].Kill();
			}
			*/
		}

		if (lastChat == null)
			return;

		if (FixedUpdater (.01f)) {
			//lastChat.SetContent(lastChat.message + StringScrambler.GetGlitchChar());
			//chat.ForcedFadeIn(1);
			int margin = 14;
			lastChat.x = margin;
			if (!lastChat.leftright) {
				lastChat.x = SHGUI.current.resolutionX - margin - lastChat.width - 1;
			}
		}

		if (lines + lastChat.height - totalOff > SHGUI.current.resolutionY - 1) {
			for (int i = 0; i < chats.Count; ++i){
				chats[i].y -= 1;

				if (chats[i].y < -chats[i].height) chats[i].remove = true;
			}
			totalOff++;
		}
	}

	public void SetInstructions(string text){

		instructions.text = text;
		instructions.x = SHGUI.current.resolutionX - 5;
		instructions.GoFromRight ();
		//instructions.ForcedFadeIn (.7f);
	}

	public void AddChatMessage(string sender, string message, bool leftright, bool interactive, bool poor){
		sender = "";
		if (lastChat!=null){
			if (lastChat.height < 3)
				lines += 3;
			else 
				lines += lastChat.height;
		}

		lastChat = new SHGUIguruchatwindow ();
		lastChat.SetLeftRight(leftright);
		if (interactive)
			lastChat.SetInteractive ();
		if (poor)
			lastChat.poorMode = true;

		lastChat.SetWidth (35);
		lastChat.SetContent (message);
		lastChat.SetLabel (sender);
		chats.Add(AddSubViewBottom (lastChat) as SHGUIguruchatwindow);
		lastChat.y = lines - totalOff;

		int margin = 14;
		lastChat.x = margin;
		if (!leftright) {
			lastChat.x = 80 - margin - lastChat.width - 1;
		}

		if (skippable) {
			if (leftright) {
				SetInstructions ("press-ESC-to-leave-chat");
				//SetInstructions ("TYPE-TO-SPEAK-|-ESC-TO-GO-AWAY");
			} else {
				SetInstructions ("press-ESC-to-leave-chat");
				//SetInstructions ("WAIT-FOR-YOUR-PARTNER-|-ESC-TO-GO-AWAY");
			}

		} else {
			SetInstructions("");
		}

		if (leftright && interactive && !poor) {
			lastChat.PunchIn (.7f);
		}

		if (!poor) {
			SHGUI.current.PlaySound(SHGUIsound.confirm);
		} else {
			SHGUI.current.PlaySound(SHGUIsound.wrong);
		}

		/*
		a.Add(AddSubView (new SHGUIframe (0, 0, SHGUI.current.resolutionX - 1, 3, 'z')));
		a.Add(AddSubView (new SHGUItext (message, 2, 1, 'w').BreakCut((int)(SHGUI.current.resolutionX / 2), 2)));
		if (!leftright) {
			(a [a.Count - 1] as SHGUItext).x = SHGUI.current.resolutionX - 3;
			(a [a.Count - 1] as SHGUItext).GoFromRight ();
		}

		a.Add(AddSubView (new SHGUItext (sender, 2, 0, 'z')));
		if (!leftright) {
			(a [a.Count - 1] as SHGUItext).x = SHGUI.current.resolutionX - 3;
			(a [a.Count - 1] as SHGUItext).GoFromRight ();
		}
		*/
	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;

		if (key == SHGUIinput.esc) {
			QuitChat ();

		}
		base.ReactToInputKeyboard (key);
	}

	private void QuitChat(){
		if (lastChat != null && !quiting && skippable) {
			lastChat.Stop();
			//lastChat = null;
			quiting = true;
			lastChat.PunchIn(.7f);
			
			messageQueue = new List<gurumessage>();
			AddMyQuit();		
		}
	}

    public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
    {	
		if (fadingOut)
			return;

		if (quiting){
//			SHGUI.current.PopView();
		}

		base.ReactToInputMouse (x, y, clicked, scroll);
	}
}

