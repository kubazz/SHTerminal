using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class APPrecruit : SHGUIview {

	List<SHGUIview> queue;
	List<float> timers;

	SHGUIview currentView;

	int currentIndex = 0;
	float currentTimer = 0;

	public APPrecruit ()
	{
		Init ();

		queue = new List<SHGUIview> ();
		timers = new List<float> ();

		AddSubView (new SHGUIrect (0, 0, SHGUI.current.resolutionX - 1, SHGUI.current.resolutionY - 1));
		
		allowCursorDraw = false;

		
		overrideFadeInSpeed = .35f;
		overrideFadeOutSpeed = .5f;


		
		AddViewToQueue (new SHGUIview(), 1f);		

		AddChatToQueue ("^Fr^Cr^M3ONE OF US.^W3\nONE OF US.^W4");
		AddViewToQueue (new SHGUIview(), 1.5f);
		AddChatToQueue ("^Fr^Cr^M3WE NEED MORE.^W3");
		AddViewToQueue (new SHGUIview(), .5f);
		AddChatToQueue ("^Fr^Cr^M3GIVE US NAMES.^W3");
		AddViewToQueue (new SHGUIview(), .5f);
		AddChatToQueue ("^Fr^Cr^M3GIVE US YOUR FRIENDS.^W3");
		AddViewToQueue (new SHGUIview(), .5f);
		AddChatToQueue ("Jakub Ziembiński", true);
		AddViewToQueue (new SHGUIview(), .5f);
		AddChatToQueue ("Tomasz Kaczmarczyk", true);
		AddViewToQueue (new SHGUIview(), .5f);
		AddChatToQueue ("Krzysztof Tracz", true);
		AddViewToQueue (new SHGUIview(), .5f);
		AddChatToQueue ("Piotr Iwanicki", true);
		AddViewToQueue (new SHGUIview(), .5f);
		
		
		AddViewToQueue (new SHGUIview(), 1f);
		AddChatToQueue ("^Fr^Cr^M3CONTACT THEM.^W3");
		AddViewToQueue (new SHGUIview(), 1f);
		AddChatToQueue ("^Fr^Cr^M3USE PHRASES LIKE 'YOU HAVE TO PLAY THIS GAME. IT'S THE MOST INNOVATIVE SHOOTER I'VE PLAYED IN YEARS'.^M3^W3");
		
		AddViewToQueue (new SHGUIview(), 1f);

		AddChatToQueue ("Hi Piotr Iwanicki, you have to play this game. It's the most\ninnovative shooter I've played in years.", true);
		AddViewToQueue (new SHGUIview(), 3f);
		AddChatToQueue ("No, no, it's not like other games. This thing really draws you in.", true);
		AddViewToQueue (new SHGUIview(), 3f);
		AddChatToQueue ("Just try it, it's really addicting.", true);
		AddViewToQueue (new SHGUIview(), 3f);
		
		/*
		AddViewToQueue (new SHGUIview(), 1f);
		AddChatToQueue ("^Fr^Cr^M3SEND THE FILES.^W3");
		AddViewToQueue (new SHGUIview(), 1f);
		AddChatToQueue ("I'm sending you the files right now.", true);
		AddViewToQueue (new SHGUIview(), 1f);
		*/

		AddChatToQueue ("^Fr^Cr^M3KEEP THE SECRET.^W3");
		AddViewToQueue (new SHGUIview(), 1f);
		AddChatToQueue ("^Fr^Cr^M3SPREAD THE GAME.^W3");
		AddViewToQueue (new SHGUIview(), 1f);
		AddChatToQueue ("^Fr^Cr^M3RECRUIT.^W3");
		AddViewToQueue (new SHGUIview(), 1f);
		AddChatToQueue ("^Fr^Cr^M3USE THIS CODE.^W4 IT'S A DISCOUNT.^W4\n       JUST FOR FRIENDS.^W4\n\n      JHJK-WZTH-TYK2-JIN3^W4^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9");
		AddViewToQueue (new SHGUIview(), 1f);		
	}

	void AddChatToQueue(string content, bool interactive = false){
		SHGUIguruchatwindow chat = new SHGUIguruchatwindow ();
		chat.SetContent (content);
		chat.desiredWidth = 25;
		//chat.GetPrompter ().maxSmartBreakOffset = 9;
		if (interactive)
			chat.SetInteractive ();

		chat.y = 10;
	
		AddViewToQueue (chat, 0f);		
	}

	public void AddViewToQueue(SHGUIview view, float timer){
		queue.Add (view);
		timers.Add (timer);
	}

	override public void Update(){
		base.Update ();

		if (fade < .99f) {
			return;
		}

		if (currentView != null && currentView is SHGUIguruchatwindow) {
			(currentView as SHGUIguruchatwindow).x = 32 - (int)((currentView as SHGUIguruchatwindow).width / 2);
			(currentView as SHGUIguruchatwindow).y = 11 - (int)((currentView as SHGUIguruchatwindow).height / 2);
		}

		ShowNextItemWhenReady ();
	}

	void ShowNextItemWhenReady(){
		currentTimer -= Time.unscaledDeltaTime;

		bool ready = (currentTimer < 0);

		if (currentView != null && currentView is SHGUIguruchatwindow) {
			if (!(currentView as SHGUIguruchatwindow).finished){
				ready = false;
				currentTimer += Time.unscaledDeltaTime;
			}
		}

		if (ready) {
			if (timers.Count > 0){
				if (currentView != null){
					currentView.Kill();
				}

				currentView = queue[0];
				currentTimer = timers[0];

				queue.RemoveAt(0);
				timers.RemoveAt(0);

				AddSubView(currentView);
			}
			else{
				Kill();
			}
		}
	
	}
	
	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;

		base.ReactToInputKeyboard (key);
	}
	
	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
	{	
		if (fadingOut)
			return;

		base.ReactToInputMouse (x, y, clicked, scroll);
	}
}
