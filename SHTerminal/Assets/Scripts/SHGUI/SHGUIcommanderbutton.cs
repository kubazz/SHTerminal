
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//using UnityEditor;
using UnityEngine;
using System;
using UnityEngine.UI;
using Object = UnityEngine.Object;



public class SHGUIcommanderbutton: SHGUIview {
    public string Text;

    public string data = "";
	public SHGUIcommanderview listLink;
	//public SHGUIview highlight;

	public bool highlighted = false;
	public bool highlightedLastFrame = false;
	public bool activated = false;

	public string panelContent = "";
	
	public Action OnActivate;
	public Action OnHighlight;
    public Action OnDeHighlight;
    public Action OnUpdate;

    private SHGUItext suffixView = null;

    public string prefix = "";
    public string suffix = "";

    private string fullText = "";
    private int textStartIndex = 0;
    private float scrollTextTimer = 1.0f;

    public FieldInfo AssignedModifier = null;
    public bool Active = false;
    public bool IsLocked = false;
    public string Url;
    public string LevelToBeLoaded = "";
    public string AdditionalComponent = "";

    public SHAlign ContentAlign = SHAlign.Left;

	public bool IsQuitButton = false;
	public bool constantScramble = false;
	private float constantScrambleTimer = 0f;
	public float constantScrambleSpeed = .03f;

	public SHGUIcommanderbutton (String text, char textcolor, Action func = null)
	{
		Init ();

		Text = text;
		SetColor(textcolor);
		OnActivate = func;

		for (int i = 0; i < UnityEngine.Random.Range(3, 10); ++i){
			data += data;
		}

	    fullText = text;

	    RefeshText();
	    
	    //highlight = AddSubView (new SHGUIrect (0, 0, 20, 0, 'r', '█', 1));
	    //highlight.hidden = true;
	}

	private string orgPrefix;
	private string orgSuffix;

	public override void Update(){
		base.Update ();

		if (constantScramble) {
			if (orgPrefix == null) orgPrefix = prefix;
			if (orgSuffix == null) orgSuffix = suffix;

			constantScrambleTimer -= Time.unscaledDeltaTime;
			if (constantScrambleTimer < 0){
				constantScrambleTimer = constantScrambleSpeed;
				prefix = StringScrambler.GetScrambledString(orgPrefix, .9f, "▀ ▄ █ ▌ ▐░ ▒ ▓ ■▪          ");
				suffixView.text = StringScrambler.GetScrambledString(orgSuffix, .9f, "▀ ▄ █ ▌ ▐░ ▒ ▓ ■▪           ");
			}
		}

		highlightedLastFrame = highlighted;

		if (highlighted && fadingIn) {
			//fade += 1.5f * Time.deltaTime * 30;
			//SpeedUpFadeIn();
		}

		if (highlighted) {
			if (listLink != null && listLink.currentPanelContentId != this.id){
				listLink.currentPanelContentId = this.id;
				listLink.rightPanel.text = data;
				listLink.rightPanel.BreakTextForLineLength(listLink.rightPanelContentWidth).CutTextForMaxLines(19);

			    if (ContentAlign == SHAlign.Center)
                    listLink.rightPanel.CenterTextForLineLength(listLink.rightPanelContentWidth);

				listLink.rightPanel.ForcedSoftFadeIn();
			}
		}

        if (OnUpdate != null)
        {
            OnUpdate.Invoke();
        }
	}
	public override void Redraw (int offx, int offy)
	{
		if (hidden)
			return;

		//SHGUI.current.DrawText ("" + fade, x + offx, y + offy, TextColor);
        SHGUI.current.DrawText(prefix + "│", x + offx, y + offy, color, fade);
		//
		//if (activated)
			//SHGUI.current.DrawText (Text, x + offx, y + offy, 'g');



		if (highlighted) {// && fade > .99f)
			//SHGUI.current.DrawRectBack (x + offx, y + offy, x + offx + 21, y + offy + 1, 'r', fade * 2);
			char c = 'r';
			if (constantScramble == true && color == 'r')
				c = 'x';
			for (int X = x + offx; X <= x + offx + 20; X++) {
				for (int Y = y + offy; Y <= y + offy; Y++) {

					SHGUI.current.SetPixelBack('█', X, Y, c);
					
				}
			}
		}

		base.Redraw (offx, offy);
	}

	private static string[] endingTexts = new string[]{
		"^M2YOU SEE?",
		"^M2DIDN'T WE JUST TALK ABOUT IT?",
		"^M2I TOLD YOU. IT'S THE END OF YOUR STORY. WE DON'T WANT RANDOM KIDS IN OUR SYSTEM.",
		"^M0ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. ENOUGH. JUST QUIT.",
		"^M2DON'T TRY. GO AWAY.",
		"^M2NO. HOW MANY TIMES MUST I REPEAT?",
		"^M2NO. HOW MANY TIMES MUST I REPEAT?",
		"^M2THERE IS NO OTHER WAY FOR YOU.",
		"^M2QUIT."};

	public void Activate(){
		if (SHGUI.current.RestrictedAccess && !IsQuitButton) {
			if (!constantScramble){
				SHGUI.current.PlaySound(SHGUIsound.finalscramble);
				if (SHGUI.current.RestrictedAccessCounter >= 0){
					int popupCount = (int)Mathf.Clamp((SHGUI.current.RestrictedAccessCounter - 1) * 15, 0, 35);
					if (popupCount == 35) popupCount = 5;
					SHGUI.current.AddViewOnTop (new APPpopupflood (popupCount, endingTexts[SHGUI.current.RestrictedAccessCounter]));
				}
				SHGUI.current.RestrictedAccessCounter++;
				if (SHGUI.current.RestrictedAccessCounter >= endingTexts.Length){
					SHGUI.current.RestrictedAccessCounter = endingTexts.Length - 1;
				}
			}
			constantScramble = true;
			return;
		}

		activated = true;
		highlighted = false;

		fade = 0;
		fadingIn = true;
		bool playedSound = false;

        if(IsLocked == true){
			SHGUI.current.PlaySound(SHGUIsound.wrong);
			playedSound = true;
			return;
		}

        if (String.IsNullOrEmpty(Url) == false)
        {
                Application.OpenURL(Url);
        }

        if (AssignedModifier != null && IsLocked == false) {
			if (Active){
				playedSound = true;
				SHGUI.current.PlaySound(SHGUIsound.pong);
			}
			else{
				playedSound = true;
				SHGUI.current.PlaySound(SHGUIsound.ping);
			}
            SetActive(!Active);     
        }

		if (OnActivate != null){
			if (!playedSound) SHGUI.current.PlaySound(SHGUIsound.confirm);
			OnActivate.Invoke ();
		}
	}

    public void SetActive(bool active) {
        Active = active;

        suffixView.text = Active ? ">ACTIVE<" : ">------<";
        suffixView.fade = 0.0f;
        suffixView.fadingIn = true;
    }

	public SHGUIcommanderbutton SetListLink(SHGUIcommanderview l){
		listLink = l;
		return this;
	}

	public SHGUIcommanderbutton SetData(String Data){
		data = Data;
		return this;
	}

	public SHGUIcommanderbutton SetOnActivate(Action a){
		OnActivate = a;
		return this;
	}

	public SHGUIcommanderbutton SetOnHighlight(Action a){
		OnHighlight = a;
		return this;
	}

    public SHGUIcommanderbutton SetOnDeHighlight(Action a)
    {
        OnDeHighlight = a;
        return this;
    }

    public SHGUIcommanderbutton SetOnUpdate(Action a) {
        OnUpdate = a;
        return this;
    }

    public void RefeshText() {
 
        try
        {
            prefix = Text.Substring(0, Text.IndexOf('│'));
            suffix = Text.Substring(Text.IndexOf('│') + 1, Text.Length - Text.IndexOf('│') - 1);
            if (suffixView == null) {
                suffixView = new SHGUItext(suffix, 13, 0, 'w');
                AddSubView(suffixView);
            } else {
                suffixView.text = suffix;
            }
            
        }
        catch (Exception ex)
        {

        }
    }
}


