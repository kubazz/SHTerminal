
using System;
using System.Collections.Generic;
using UnityEngine;


public class SHGUIcommanderview: SHGUIview
{
	public List<SHGUIcommanderbutton> buttons;
	public int currentButton = 0;
	public String path=  "C:\\";
	public int currentPanelContentId = -1;

	public SHGUItext rightPanel;
	private SHGUItext pathView;

	private int left;
	private int right;
	private int width;

	private int listLength = 5;
	public int currentListOffset = 0;
	int minListOffset;
	int maxListOffset;

	public int rightPanelContentWidth = 52;

	public bool isRoot = false;
	public bool isDownloading = false;

	public SHGUItext instructions;

	public SHGUIview clock;

	public SHGUIcommanderview ()
	{
		Init ();

		left = 3;
		width = SHGUI.current.resolutionX - left - left - 1;


		buttons = new List<SHGUIcommanderbutton> ();

		AddSubView (new SHGUIline (left, left + width, 0, true, 'z').SetStyle("┌─┐"));
		AddSubView (new SHGUIline (left, left + width, SHGUI.current.resolutionY - 1, true, 'z').SetStyle("└─┘"));
		AddSubView (new SHGUIline (0, SHGUI.current.resolutionY - 1, left, false, 'z').SetStyle("┌│└"));
		AddSubView (new SHGUIline (0, SHGUI.current.resolutionY - 1, left + width, false, 'z').SetStyle("┐│┘"));
		AddSubView (new SHGUIline (left, left + width, SHGUI.current.resolutionY - 3, true, 'z').SetStyle("├─┤"));
		AddSubView(new SHGUIline( 0,SHGUI.current.resolutionY - 3, left + 13, false, 'z').SetStyle("┬│┴"));
		AddSubView(new SHGUIline( 0,SHGUI.current.resolutionY - 3, left + 22, false, 'z').SetStyle("┬│┴"));

		clock = AddSubView(new SHGUIclock(SHGUI.current.resolutionX - 3 - left, 0, 'w'));
		//AddSubView(new SHGUItext("piOS-v2.1", SHGUI.current.resolutionX - 3 - left - 8, 0, 'w').GoFromRight());
		AddSubView(new SHGUItext("0mni-piOS-v2.1.01p", SHGUI.current.resolutionX - 21 - left, SHGUI.current.resolutionY - 1, 'w'));
		pathView = AddSubView(new SHGUItext(path, left + 1, SHGUI.current.resolutionY - 2, 'w')) as SHGUItext;
	
		listLength = SHGUI.current.resolutionY - 4;
		minListOffset = 1;
		maxListOffset = 0;

		/*
		rightPanel = new SHGUIview ();
		rightPanel.x = 25;
		rightPanel.y = 1;
		*/

		/*
		string content = "wlazł-kotek-na-plotek-i-";
		for (int i = 0; i < 10; ++i) {
			content += content;
		}
		*/

		string content = "";
		rightPanelContentWidth = 32;
		rightPanel = AddSubView(new SHGUItext(content, left + 24, 1, 'z').BreakTextForLineLength(rightPanelContentWidth).CutTextForMaxLines(19)) as SHGUItext;
		//AddSubView (rightPanel);
		
	}

	bool pathUpdated = false;

	public override void Update(){

		MoveListOffset (0);
		if (!pathUpdated) {
			pathView.text = path;
			pathView.CutTextForLineLength(width - 2);
			pathUpdated = true;
		}

		if (rightPanel.fadingIn)
			rightPanel.fade += 0.2f;

		base.Update ();

		for (int i = 0; i < buttons.Count; ++i) {
			buttons[i].highlighted = false;	
		}

		buttons [currentButton].highlighted = true;
		buttons [currentButton].SpeedUpFadeIn ();


		if (fade < 0.99f)
			return;


		if (buttons[currentButton].fade > .99f && buttons[currentButton].y == minListOffset && currentListOffset < 0)
			MoveListOffset (1);

		if (buttons[currentButton].fade > .99f && buttons[currentButton].y == listLength)
			MoveListOffset (-1);


		return;
		//LocalizationManager.Instance
		if (instructions == null) {
			string text = LocalizationManager.Instance.GetLocalized ("COMMANDER_TUTORIAL_INPUT");
			instructions = AddSubView (new SHGUItext (text, SHGUI.current.resolutionX - left - 5, SHGUI.current.resolutionY - 1, 'z').GoFromRight ()) as SHGUItext;
		} else {
			string text = LocalizationManager.Instance.GetLocalized ("COMMANDER_TUTORIAL_INPUT");
			if (text != instructions.text){
				instructions.Kill();
				instructions = null;
				instructions = AddSubView (new SHGUItext (text, SHGUI.current.resolutionX - left - 5, SHGUI.current.resolutionY - 1, 'z').GoFromRight ()) as SHGUItext;
			}
		}
	}

	public void AddButtonView(SHGUIcommanderbutton button){
		AddSubView (button);
		buttons.Add (button);

		button.x += left + 1;

		button.y = buttons.Count - 1 + 1;
	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (!interactable)
			return;

		if (fadingOut)
			return;

		int lastButton = currentButton;
		base.ReactToInputKeyboard (key);
		
		if (isDownloading)
			return;

        int oldButton = currentButton;

		if (key == SHGUIinput.up) {
			SpeedUpFadeIn();
			currentButton--;
		} else if (key == SHGUIinput.down) {
			SpeedUpFadeIn();
			currentButton++;
		}

		if (currentButton > buttons.Count - 1)
			currentButton = buttons.Count - 1;
		if (currentButton < 0)
			currentButton = 0;

	    if (oldButton != currentButton) {
            if (buttons[oldButton].OnDeHighlight != null)
	            buttons[oldButton].OnDeHighlight.Invoke();

            if(buttons[currentButton].OnHighlight!= null)
                buttons[currentButton].OnHighlight.Invoke();
	    }

		if (lastButton != currentButton) {
			SHGUI.current.PlaySound(SHGUIsound.tick);
		}

		if (key == SHGUIinput.enter) {
			SpeedUpFadeIn();
			buttons[currentButton].Activate();
			
		}

		if (key == SHGUIinput.esc && !isRoot) {
			SHGUI.current.PopView();
			SHGUI.current.PlaySound(SHGUIsound.confirm);
		}
	}

	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll){
		if (fadingOut)
			return;

		if (!interactable)
			return;
	

		int lastButton = currentButton;

		base.ReactToInputMouse (x, y, clicked, scroll);

		if (isDownloading)
			return;

        if(scroll == SHGUIinput.scrollUp)
            MoveListOffset(1);

        if (scroll == SHGUIinput.scrollDown)
            MoveListOffset(-1);

		for (int i = 0; i < buttons.Count; ++i) {
			if (buttons[i].y == y && x >= buttons[i].x && x < (buttons[i].x + 21) && !buttons[i].hidden){
				currentButton = i;
			}
		}

		for (int i = 0; i < buttons.Count; ++i) {
			if ((buttons[i].y == y || buttons[i].y == y - 1 || buttons[i].y == y + 1)
			    && x >= buttons[i].x && x < (buttons[i].x + 21)){
				if (clicked){
					SpeedUpFadeIn();
					buttons[currentButton].Activate();
					SHGUI.current.PlaySound(SHGUIsound.confirm);
					return;
				}
			}
		}

		if (lastButton != currentButton) {
			SHGUI.current.PlaySound(SHGUIsound.tick);

            if (buttons[lastButton].OnDeHighlight != null)
                buttons[lastButton].OnDeHighlight.Invoke();

            if (buttons[currentButton].OnHighlight != null)
                buttons[currentButton].OnHighlight.Invoke();
		}
	}
	
	public void MoveListOffset(int off){
		if (isDownloading)
			return;

		maxListOffset = 0;
		if (buttons.Count <= listLength)
			return;

		maxListOffset = buttons.Count - listLength;
		int old = currentListOffset;
		currentListOffset += off;

		//Debug.Log (old + ", " + currentListOffset);
		if (currentListOffset > 0)
			currentListOffset = 0;

		if (currentListOffset < -maxListOffset)
			currentListOffset = -maxListOffset;

		for (int i = 0; i < buttons.Count; ++i) {
			buttons[i].y -= old;
			buttons[i].y += currentListOffset;

			if (buttons[i].y < minListOffset || buttons[i].y > listLength){
				buttons[i].hidden = true;
			}
			else{
				buttons[i].hidden = false;
			}
		}

		//currentListOffset += off;
	}

	public override void Redraw(int offx, int offy){
		base.Redraw (offx, offy);
		if (currentListOffset + buttons.Count > listLength) {
			SHGUI.current.DrawText("-MORE-MORE-MORE-MORE-", left + 1, SHGUI.current.resolutionY - 4, 'z', fade);
		}

		if (currentListOffset < 0) {
			SHGUI.current.DrawText("-MORE-MORE-MORE-MORE-", left + 1, 1, 'z', fade);
		}
	}
}


