using System;
using UnityEngine;
using System.Collections.Generic;

public class SHGUImultichat: SHGUIview
{
	public List<SHGUIguruchatwindow> chats;
	public bool finished = false;

	public SHGUImultichat ()
	{
		chats = new List<SHGUIguruchatwindow>();
	}

	public void AddChat(SHGUIguruchatwindow chat){
		chats.Add (chat);
		AddSubView (chat);
	}

	public void AddChat(string content, bool interactive){
		SHGUIguruchatwindow chat = new SHGUIguruchatwindow ();
		chat.SetContent (content);
		chat.desiredWidth = 25;
		if (interactive)
			chat.SetInteractive ();

		if (!interactive)
			chat.showInstructions = false;

		AddChat (chat);
	}

	public override void Update(){
		base.Update ();

		bool fin = true;
		int Y = 0;
		for (int i = 0; i < chats.Count; ++i) {
			if (!chats[i].finished)
				fin = false;

			chats[i].x = 0 - (int)(chats[i].width / 2);
			chats[i].y = Y;

			Y += Mathf.Clamp(chats[i].height, 3, 10000);
		}

		for (int i = 0; i < chats.Count; ++i) {
			chats[i].y -= (int)(Y / 2);
		}

		finished = fin;
	}

}
