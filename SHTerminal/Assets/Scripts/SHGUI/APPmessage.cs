
using System;
using UnityEngine;
using System.Collections.Generic;
using InControl;

public class APPmessage: SHGUIview
{
	float timeOut = 0f;
	bool doTimeOut = false;

	SHGUIview instructions1;
	SHGUIview instructions2;
	SHGUIview frame;
	SHGUIview text;
	SHGUIview back;

	float delay = 0f;
	int queueSkips = 1;

	public List<string> restrictedInstructions;
	public bool restrictDismissing;

	SHGUIview restrictInstruction;
	int instructionX = 0;
	int instructionY = 0;

	SHGUIblinkview blink;
	SHGUIblinkview blink2;
	

	public APPmessage ()
	{
		string message = "INCOMING MESSAGE";
		char frameColor = 'z';
		char textColor = 'w';

		x = (int)(SHGUI.current.resolutionX / 2) - (int)(message.Length / 2);
		y = 12 - 3;

		//AddSubView (new SHGUIrect (-3, -1, message.Length + 4, 6, 'z', '░'));
		back = AddSubView (new SHGUIrect (-3, -1, message.Length + 4, 3, '0', '▒'));
		//back.AddSubView (new SHGUIframe (-3, -2, message.Length + 4, 6, 'z'));
		
		allowCursorDraw = false;
		frame = AddSubView(new SHGUIframe(-2, -1, message.Length + 3, 3, frameColor));
		//frame = AddSubView(new SHGUIframe(-2, -1, message.Length + 3, 3, frameColor));

		float blinkTime = .6f;
		blink = new SHGUIblinkview (blinkTime);
		AddSubView (blink);
		text = blink.AddSubView(new SHGUItext(message, 1, 1, textColor));

		blink2 = new SHGUIblinkview (blinkTime).SetFlipped();
		AddSubView (blink2);
		instructions1 = blink2.AddSubView(new SHGUItext(LocalizationManager.Instance.GetLocalized("INCOMING_MESSAGE_REPLY_INPUT"), 1, 1, textColor));
		if (InputManager.ActiveDevice.Name != "Keyboard/Mouse") {
			instructions1.x -= 1;
		}
		instructions2 = AddSubView (new SHGUItext (LocalizationManager.Instance.GetLocalized("INCOMING_MESSAGE_DISMISS_INPUT"), message.Length, 3, 'z').GoFromRight ());

		instructionX = message.Length;
		instructionY = instructions2.y;

		dontDrawViewsBelow = false;

		restrictedInstructions = new List<string> ();

		AddSubView (new SHGUIplaysound (SHGUIsound.incomingmessage));
	}

	public APPmessage SetTimeOut(float time){
		timeOut = time;
		doTimeOut = true;

		blink.blinkTime = .3f;

		return this;
	}
	
	public APPmessage SetRed(){
		frame.SetColorRecursive ('r');
		text.SetColorRecursive ('r');
		instructions1.SetColorRecursive ('r');
		instructions2.SetColorRecursive ('r');
		
		color = 'r';
		return this;
	}

	public APPmessage SetDelay(float delay){
		this.delay = delay;
		return this;
	}

	public APPmessage SetQueueSkips(int skips){
		this.queueSkips = skips;
		return this;
	}

	public APPmessage AddRestrictInstruction(string i){
		restrictedInstructions.Add (i);
		restrictDismissing = true;
		return this;
	}
	
	public override void Update(){
		base.Update ();

		delay -= Time.unscaledDeltaTime;
		if (delay > 0)
			this.ForceFadeRecursive (-5f);

		if (restrictInstruction != null) {
			instructions2.hidden = true;
		}

		if (doTimeOut) {
			instructions1.hidden = true;
			instructions2.hidden = true;

			back.hidden = true;
		}

		if (fade < 0.99f)
			return;

		if (doTimeOut) {
			timeOut -= Time.unscaledDeltaTime;
			if (timeOut < 0){
				Kill ();
				doTimeOut = false;
			}
		}

	}

	public void Dismiss(){

		if (!restrictDismissing) {
			Kill ();
			for (int i= 0; i < queueSkips; ++i) {
				SHGUIview v = SHGUI.current.PopViewFromQueue ();
				if (v != null) {
					v.KillInstant ();
				}
			}
		}
		else {
			char c = 'w';
			if (restrictInstruction != null){
				restrictInstruction.KillInstant();
				c = 'w';
			}

			if (color == 'r')
				c = 'r';
			restrictInstruction = null;

			string text = restrictedInstructions[UnityEngine.Random.Range(0,restrictedInstructions.Count)];
			restrictInstruction = new SHGUItempview(1f);

			restrictInstruction.AddSubView(new SHGUItext(text, instructionX, instructionY, c).GoFromRight());

			AddSubView(restrictInstruction);

			PunchIn(.8f);

			restrictInstruction.PunchIn(0f);

			SHGUI.current.PlaySound(SHGUIsound.noescape);
		}
	}

	public void Read(){
		SHGUI.current.AddViewOnTop (new APPkill ());
	}

	public override void ReactToInputKeyboard (SHGUIinput key)
	{
		base.ReactToInputKeyboard (key);

		if (key == SHGUIinput.enter) {
			Read ();
		} else if (key == SHGUIinput.esc) {
			Dismiss();		
		}
	}

	public override void ReactToInputMouse (int x, int y, bool clicked, SHGUIinput scroll)
	{
		base.ReactToInputMouse (x, y, clicked, scroll);

		if (clicked) {
			Read ();
		}
	}
}

