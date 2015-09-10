using System;
using UnityEngine;

public class SHGUIguruchatwindow: SHGUIview
{
	public string message = "";

	private SHGUIprompter textElement;
	private SHGUItext labelElement;
	private SHGUItext subElement;
	private SHGUIframe frameElement;
	private SHGUIrect background;

	public int height = 1;

	public int width = 30;
	public int desiredWidth = 30;

	public bool leftright = true;

	int currentIndex = 0;
	public bool finished = false;
	private bool confirmed = false;

	public bool showInstructions = true;

	public bool poorMode = false;

	public SHGUIguruchatwindow ()
	{
		background = AddSubView (new SHGUIrect (0, 0, 0, 0, 'z')) as SHGUIrect;
		frameElement = AddSubView (new SHGUIframe (0, 0, 0, 0, 'z')) as SHGUIframe;
		labelElement = AddSubView (new SHGUItext ("", 1, 0, 'z')) as SHGUItext;
		subElement = AddSubView (new SHGUItext ("", 1, 0, 'z')) as SHGUItext;
		textElement = AddSubView (new SHGUIprompter(1, 1, 'w')) as SHGUIprompter;

		SetContent ("");
	}

	public SHGUIguruchatwindow SetWidth(int w){
		desiredWidth = w;
		textElement.maxLineLength = w - 2;
		return this;
	}

	public SHGUIguruchatwindow SetCallback(Action a){
		textElement.thisConsoleCallback = a;
		return this;
	}

	public SHGUIguruchatwindow SetContent(string text){
		message = text;
		textElement.SetInput (text, true);

		return this;
	}

	public void SetFrameColor(char color){
		frameElement.color = color;
		subElement.color = color;
	}
	
	public void Fit(bool force = false){
		if (textElement.IsFinished () && !force)
			return;
		int oldheight = height;

		width = textElement.longestLineAfterSmartBreak + 2;
		//if (width > desiredWidth)
		//	width = desiredWidth;
		if (width < labelElement.text.Length + 1)
			width = labelElement.text.Length + 1;
	
		if (width < subElement.text.Length + 1)
			width = subElement.text.Length + 1;
		height = textElement.CountLines () + 1;
		//textElement.fade = 1;
		
		RefreshFrames();
		height += 1;

		if (height != oldheight && height > 3)
			frameElement.PunchIn (.4f);
		
		//return this;
	}

	public int GetHeightOfCompleteTextWithFrameVERYSLOWandMOODY(){
		SHGUIprompter p = new SHGUIprompter(0, 0, 'w');
		SHGUIprompter old = textElement as SHGUIprompter;

		p.SetInput(old.input, true);
		p.maxLineLength = old.maxLineLength;
		p.ShowInstant();

		return p.CountLines() + 2;
	}

	private void RefreshFrames(){
		frameElement.remove = true;
		frameElement.hidden = true;
		float oldfade = frameElement.fade;
	
		frameElement = AddSubViewBottom (new SHGUIframe (-1, 0, width, height, frameElement.color)) as SHGUIframe;
		frameElement.PunchIn(oldfade);

		
		background.remove = true;
		background.hidden = true;
		float oldfade2 = background.fade;
		background = AddSubViewBottom (new SHGUIrect (-1, 0, width, height)) as SHGUIrect;
		background.PunchIn(oldfade2);	
	}

	bool playedSound = false;
	public override void Update(){
		base.Update ();

		//if (fade < 0.99f)
		//	return;

		if (!playedSound) {
			playedSound = true;
			if (leftright) {
				if (frameElement.color != 'r')
					SHGUI.current.PlaySound(SHGUIsound.pong);
				else
					SHGUI.current.PlaySound(SHGUIsound.redpong);
					
			}
			else {
				if (frameElement.color != 'r')
					SHGUI.current.PlaySound(SHGUIsound.ping);
				else
					SHGUI.current.PlaySound(SHGUIsound.redping);
			}
		}

		Fit ();
		UpdateInstructions ();

		if (poorMode) {
			//frameElement.SetColorRecursive('r');
			//labelElement.SetColorRecursive('r');
			//subElement.SetColorRecursive('r');
			//labelElement.hidden = true;
			//frameElement.hidden = true;
			//subElement.hidden = true;

		}

		finished = textElement.IsFinished ();
	}

	public SHGUIguruchatwindow SetLeftRight(bool LeftRight){
		leftright = LeftRight;
		return this;
	}

	public SHGUIguruchatwindow SetInteractive(){
		textElement.drawCarriage = true;
			textElement.SwitchToManualInputMode();
		return this;
	}

	public void ShowInstantPunchIn(){
		textElement.ShowInstant ();
		textElement.PunchIn (.2f);
		textElement.SetConfirmed ();
	}
	
	public SHGUIguruchatwindow SetLabel(string text){
		labelElement.text = text;
		return this;
	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;
		
		base.ReactToInputKeyboard (key);
		
		if (key == SHGUIinput.any){
			if (subElement != null && subElement.text != LocalizationManager.Instance.GetLocalized("TYPE_TO_HACK_INPUT")){
				subElement.PunchIn(.6f);
			}
		}
		
	}
	
	private void UpdateInstructions(){
		if (subElement == null)
			return;

		string lastInstructions = subElement.text;
		if (showInstructions && !textElement.IsFinished()) {
			if (textElement.manualUpdate){
				if (textElement.IsAlmostFinished()){
					subElement.text = LocalizationManager.Instance.GetLocalized("ENTER_TO_SEND_INPUT");
				}else{
					subElement.text = LocalizationManager.Instance.GetLocalized("TYPE_TO_HACK_INPUT");
				}
			}
			else{
				//if (!leftright)
					subElement.text = "WAIT";
			}
			subElement.x = width - 2;
			subElement.y = height - 1;
			subElement.GoFromRight ();

			if (lastInstructions != subElement.text){
				subElement.PunchIn(.6f);
			}
		} else {
			//this could backfire and null maybe? 
			subElement.Kill();
		}

		if (textElement.IsFinished ()) {
			subElement.Kill();
			
		}


	}

	public void Stop(){
		showInstructions = false;
		textElement.Stop ();
	}
}
