using System;

public class APPingamepopup: SHGUIview
{
	SHGUIguruchatwindow chat;
	bool finished = false;
	public string content = "";
	public bool moveToTop = false;

	public float waitBeforeDisplay = 0f;
	private bool displayed = false;

	public int positionX = 0;
	public int positionY = 0;
	
	public APPingamepopup (string content = "")
	{
		x += 7;
		y += 0;
		this.content = content;
		interactable = false;
		dontDrawViewsBelow = false;
	}

	public APPingamepopup SetInteractable(bool v){
		interactable = v;
		return this;
	}

	public override void Update(){

		if (!displayed) {
			if ((parent == null)||(parent != null && parent.id == SHGUI.current.GetInteractableView ().id))
				waitBeforeDisplay -= UnityEngine.Time.unscaledDeltaTime;
			if (waitBeforeDisplay < 0){
				AddPopup("", content);
				displayed = true;
			}
			return;
		}

		if (chat.finished){
			Kill ();
		}

		if (parent != null && moveToTop) {
			KillInstant ();
			SHGUI.current.AddViewOnTop(new APPingamepopup(content).SetInteractable(true));
		}

		if (parent != null && parent.id != SHGUI.current.GetInteractableView ().id && (SHGUI.current.GetInteractableView () is SHGUIcommanderview)) {
				Kill ();
		}

		base.Update();

		if (chat != null) {
			int margin = 8;
			chat.x = margin;
			chat.x = SHGUI.current.resolutionX - margin - chat.width - 1;
		}
	}

	public void AddPopup(string sender, string message){

		message += "^W9^W9";
		SHGUI.current.PlaySound (SHGUIsound.confirm);
		
		chat = new SHGUIguruchatwindow ();
		chat.SetLeftRight(false);
		
		chat.SetWidth (35);
		chat.SetContent (message);
		chat.SetLabel (sender);
		AddSubViewBottom (chat);
		chat.y = 0;
	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;

		if (key != SHGUIinput.none) {
			PunchIn(.8f);
		}
	}
	
	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
	{	
		if (fadingOut)
			return;
		
		if (clicked)
			PunchIn(.8f);
	}
}