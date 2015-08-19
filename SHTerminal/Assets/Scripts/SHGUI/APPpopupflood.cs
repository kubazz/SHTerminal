
using System;
using System.Collections.Generic;
using System.Text;


public class APPpopupflood: SHGUIview
{
	float timer;
	int counter = 0;
	string endingText = "";

	public bool skippable = true;

	public APPpopupflood(int counter, string endingText){
		Init ();

		this.counter = counter;
		this.endingText = endingText;

		if (counter > 0)
			AddRandomTextFrame ();
		else {
			AddEndingPrompter();
		}
		
		dontDrawViewsBelow = false;
		//interactable = false;


	}

	void AddRandomTextFrame(){
		string[] words = new string[] {
			"I TOLD YOU NOT TO MESS WITH US.",
			"DON'T.",
			"NO.",
			"THIS IS NOT A GAME.",
			"GET OUT OF THE SYSTEM.",
			"DON'T EVER COME BACK.",
			"YOU SHOULD QUIT.",
			"NOTHING FOR YOU HERE.",
			"WE'RE IN CONTROL.",
			"TOTAL CONTROL.",
			"GIVE UP.",
		};
		
		string word = words [UnityEngine.Random.Range (0, words.Length)];
		StringBuilder c = new StringBuilder ();
		for (int i = 0; i < 60; ++i) {
			c.Append (word);
		}
		
		int w = 17 + (int)(UnityEngine.Random.Range (0, 6));
		int h = 4 + (int)(UnityEngine.Random.Range (0, 6));
		AddSubView (new SHGUIrect (0, 0, w, h));
		AddSubView (new SHGUIframe (0, 0, w, h, 'z'));
		//AddSubView (new SHGUItext (StringScrambler.GetScrambledString(c.ToString (), 1f), 1, 1, 'z').BreakTextForLineLength (w - 1).CutTextForMaxLines (h - 2));
		AddSubView (new SHGUItext (c.ToString (), 1, 1, 'z').BreakTextForLineLength (w - 1).CutTextForMaxLines (h - 2));
		AddSubView( new SHGUIplaysound(SHGUIsound.restrictedpopup));

		x = UnityEngine.Random.Range (0, SHGUI.current.resolutionX - w);
		y = UnityEngine.Random.Range (0, SHGUI.current.resolutionY - h);
	}

	SHGUIguruchatwindow endingChat;
	void AddEndingPrompter(){
		endingChat = new SHGUIguruchatwindow ();
		endingChat.desiredWidth = 35;
		endingChat.SetFrameColor ('r');
		this.AddSubView (endingChat);
		endingChat.SetContent("^Cr" + endingText);
		endingChat.showInstructions = false;
	}

	bool launched = false;
	public override void Update ()
	{
		base.Update ();

		//if (SHGUI.current.GetInteractableView ().id == this.id)
		if (endingChat == null) {
			timer += UnityEngine.Time.unscaledDeltaTime;

			if (timer > 0.05f && !launched) {
				if (counter > 0)
					LaunchNext();
				launched = true;
			}

			if (timer > -1f && launched) {
				if (SHGUI.current.GetInteractableView ().id == this.id)
					Kill ();			
			}
		}
		else{
			endingChat.x = (int)(SHGUI.current.resolutionX / 2) - (int)(endingChat.width / 2);
			endingChat.y = (int)(SHGUI.current.resolutionY / 2) - (int)(endingChat.height / 2) - 2;

			if (endingChat.finished) {
				timer += UnityEngine.Time.unscaledDeltaTime;

				if (timer > 2f && SHGUI.current.GetInteractableView ().id == this.id)
					Kill ();		
			}
		}		
	}

	void LaunchNext(){
		var flood = new APPpopupflood (counter - 1, endingText);
		if (skippedOnce)
			flood.timer += 1.5f;
		SHGUI.current.AddViewOnTop (flood);
	}

	bool skippedOnce = false;
	private void Skip(){
		if (!skippable)
			return;

		if (endingChat != null) {
			if (skippedOnce) {
				Kill ();
			} else{
				skippedOnce = true;
				endingChat.ShowInstantPunchIn ();
				endingChat.finished = true;
			}
		} else {
			skippedOnce = true;
			timer += 1.5f;
		}
	}


	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;

		if (key != SHGUIinput.none && endingChat != null) {
			Skip();
		}
	}

    public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
    {	
		if (fadingOut)
			return;

		if (clicked) {
			Skip ();
		}
	}
	
}


