﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class APPrecruit : SHGUIview {

	List<SHGUIview> queue;
	List<float> timers;

	SHGUIview currentView;

	int currentIndex = 0;
	float currentTimer = 0;

	SHGUIview background;

	public APPrecruit ()
	{
		Init ();

		queue = new List<SHGUIview> ();
		timers = new List<float> ();

		AddSubView (new SHGUIrect (0, 0, SHGUI.current.resolutionX - 1, SHGUI.current.resolutionY - 1));
		
		allowCursorDraw = false;

		
		overrideFadeInSpeed = .35f;
		overrideFadeOutSpeed = .5f;

		StringBuilder s = new StringBuilder ();
		for (int X = 0; X < 24; ++X) {
			for (int Y = 0; Y < 64; ++Y) {
				if (Random.value > .5f)
					s.Append('0');
				else
					s.Append('1');
			}
			s.Append('\n');
		}

		//background = AddSubView (new SHGUItext (s.ToString (), 0, 0, 'z'));
		background = AddSubView (new SHGUIview());
		
		Version3 ();
	}

	void Version3(){
		AddViewToQueue (new SHGUIview(), 1f);		
		
		//AddChatToQueue ("^Fr^Cr^M3ONE OF US.^W3\nONE OF US.^W4");
		AddChatToQueue ("^Fr^Cr^M3ONE OF US.^W4");
		AddViewToQueue (new SHGUIview(), .5f);		

		SHGUIview view = new SHGUIview ();

		int marginx = 7;
		int marginy = 5;
		
		for (int i = 0; i < 10; ++i) {
			SHGUIguruchatwindow chat2 = new SHGUIguruchatwindow ();
			chat2.overrideFadeInSpeed = 1f;
			chat2.PunchIn(Random.Range(.5f, 1f));
			chat2.SetContent ("^Fr^Cr^M3ONE OF US.");
			chat2.desiredWidth = 25;
			chat2.showInstructions = false;

			Random.seed = 13274 + i;
			chat2.x = Random.Range(0 + marginx, 64 - 11 - marginx) - 6;
			chat2.y = Random.Range(1 + marginy, 24 - 3 - marginy);

			view.AddSubView(chat2);
		}

		SHGUIguruchatwindow chat = new SHGUIguruchatwindow ();
		chat.overrideFadeInSpeed = 1f;
		chat.PunchIn(Random.Range(.5f, 1f));
		chat.SetContent ("^Fr^Cr^M3ONE OF US.");
		chat.desiredWidth = 25;
		chat.showInstructions = false;

		chat.x = 32 - 6;
		chat.y = 10;
		view.AddSubView(chat);
			
		AddViewToQueue (view, 3.5f);

		AddViewToQueue (new SHGUIview(), 1.5f);
		AddMultiChat (new string[]{"^Fr^Cr^M3WE NEED MORE.^W3", "^Fr^Cr^M3^W1GIVE US YOUR FRIENDS.^W3", "^Fr^Cr^M3^W2GIVE US NAMES.^W3"}, false);		
		AddViewToQueue (new SHGUIview(), .5f);
		AddChatToQueue ("Tomasz Kaczmarczyk", true);
		AddViewToQueue (new SHGUIview(), .5f);
		AddChatToQueue ("Krzysztof Tracz", true);
		AddViewToQueue (new SHGUIview(), .5f);
		AddChatToQueue ("Marcin Surma", true);
		AddViewToQueue (new SHGUIview(), .5f);
		
		
		AddViewToQueue (new SHGUIview(), 1f);
		AddChatToQueue ("^Fr^Cr^M3CONTACT THEM.^W3 USE THIS PHRASE.^W3");
		AddViewToQueue (new SHGUIview(), .5f);
		AddChatToQueue ("^Fr^Cr^M3'IT'S THE MOST INNOVATIVE SHOOTER I'VE PLAYED IN YEARS'.^M3^W3");
		
		AddViewToQueue (new SHGUIview(), 1f);
		AddMultiChat (new string[]{"Hi Marcin Surma, you have to play this game. It's the most\ninnovative shooter I've played in years.",
			"Hi Tomasz Kaczmarczyk, you have to play this game. It's the most\ninnovative shooter I've played in years.",
			"Hi Krzysztof Tracz, you have to play this game. It's the most\ninnovative shooter I've played in years."}, true);	

		AddViewToQueue (new SHGUIview(), 2f);
		
		AddChatToQueue ("^Fr^Cr^M3KEEP THE SECRET.^W3");
		AddViewToQueue (new SHGUIview(), 1f);
		AddChatToQueue ("^Fr^Cr^M3SPREAD THE SYSTEM.^W3");
		AddViewToQueue (new SHGUIview(), 1f);
		AddChatToQueue ("^Fr^Cr^M3RECRUIT.^W3");
		AddViewToQueue (new SHGUIview(), 1f);
		
		AddChatToQueue ("^Fr^Cr^M3GIVE THEM THIS CODE.^W4 IT'S A DISCOUNT.^W4\n          JUST FOR FRIENDS.^W4\n\n         JHJK-WZTH-TYK2-JIN3^W4^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9");
		AddViewToQueue (new SHGUIview(), 1f);	
	}

	void Version2(){
		AddViewToQueue (new SHGUIview(), 1f);		
		
		AddChatToQueue ("^Fr^Cr^M3ONE OF US.^W3");
		AddViewToQueue (new SHGUIview(), 1.5f);
		AddMultiChat (new string[]{"^Fr^Cr^M3ONE OF US.^W3", "^Fr^Cr^M3^W1ONE OF US.^W3", "^Fr^Cr^M3^W2ONE OF US.^W3"}, false);		
		AddViewToQueue (new SHGUIview(), .5f);
		AddMultiChat (new string[]{"^Fr^Cr^M3WE NEED MORE.^W3", "^Fr^Cr^M3^W1GIVE US YOUR FRIENDS.^W3", "^Fr^Cr^M3^W2GIVE US NAMES.^W3"}, false);		
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
		AddChatToQueue ("^Fr^Cr^M3CONTACT THEM.^W3 USE THESE PHRASES.^W3");
		
		AddViewToQueue (new SHGUIview(), 1f);
		AddMultiChat (new string[]{"^Fr^Cr^M3CHECK OUT THIS GAME. SOMETHING FRESH, FINALLY.",
			"^Fr^Cr^M3'YOU HAVE TO PLAY THIS GAME. IT'S THE MOST INNOVATIVE SHOOTER I'VE PLAYED IN YEARS'.^M3^W3",
			"^Fr^Cr^M3THIS GAME IS SO ADDICTIVE. CHECK IT OUT."}, false);		
		
		AddViewToQueue (new SHGUIview(), 1f);
		
		AddMultiChat (new string[]{"Hi Piotr Iwanicki, you have to play this game. It's the most\ninnovative shooter I've played in years.",
			"Hi Tomasz Kaczmarczyk, you have to play this game. It's the most\ninnovative shooter I've played in years.",
			"Hi Krzysztof Tracz, you have to play this game. It's the most\ninnovative shooter I've played in years."}, true);	
		
		AddMultiChat (new string[]{"^Fr^Cr^M3KEEP THE SECRET.^W3",
			"^Fr^Cr^M3SPREAD THE GAME.^W3",
			"^Fr^Cr^M3RECRUIT.^W3"}, false);
		
		AddChatToQueue ("^Fr^Cr^M3GIVE THEM THIS CODE.^W4 IT'S A DISCOUNT.^W4\n          JUST FOR FRIENDS.^W4\n\n         JHJK-WZTH-TYK2-JIN3^W4^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9^W9");
		AddViewToQueue (new SHGUIview(), 1f);	
	}

	void Version1(){
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

	void AddMultiChat(string[] contents, bool defaultInteractive, bool[] overridesForDefaults = null){

		SHGUImultichat multi = new SHGUImultichat ();
		multi.x = 32;
		multi.y = 11;



		AddViewToQueue (multi, 0);
		
		for (int i = 0; i < contents.Length; ++i) {
			bool interactive = defaultInteractive;
			if (overridesForDefaults != null && i < overridesForDefaults.Length){
				interactive = overridesForDefaults[i];
			}
			multi.AddChat (contents[i], interactive);
		}

	}

	void AddChatToQueue(string content, bool interactive = false){
		SHGUIguruchatwindow chat = new SHGUIguruchatwindow ();
		chat.SetContent (content);
		chat.desiredWidth = 25;
		//chat.GetPrompter ().maxSmartBreakOffset = 9;
		if (interactive)
			chat.SetInteractive ();
		else {
			chat.showInstructions = false;
		}
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

		if (Random.value > .97f) {
	
			for (int i = 0; i < 1; ++i){
				SHGUItempview t = new SHGUItempview(.5f);
				string number = "0";
				if (Random.value > .5f) number = "1";
				t.AddSubView(new SHGUItext(number, Random.Range(0, 64), Random.Range(0, 24), 'w'));

				background.AddSubView(t);
			}

		}

		background.hidden = true;

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

		if (currentView != null && currentView is SHGUImultichat) {
			if (!(currentView as SHGUImultichat).finished){
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
