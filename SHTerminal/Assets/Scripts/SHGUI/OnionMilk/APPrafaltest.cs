using UnityEngine;
using System.Collections;

public class APPrafaltest : SHGUIappbase {

	public APPrafaltest(): base("tytuł-tutajalkjsfl;kjasfl;kjas;ldfkjask;jlfl;kjasdfl;kjasdflkjsl;dkf"){
	}

	int animIndex = 0;
	float animTimer = 0;
	public override void Update(){
		base.Update ();

		string[] teksty = new string[]{"|", "/", "-", "\\"};

		animTimer -= Time.unscaledDeltaTime;
		if (animTimer < 0) {
			animIndex++;
			if (animIndex > teksty.Length - 1){
				animIndex = 0;
			}

			APPLABEL.text = "ramka animacji labela: " + animIndex + " animacja: " + teksty[animIndex];
			animTimer = .2f;

			//APPLABEL.PunchIn(.5f);
		}
	}

	int posx = 10;
	int posy = 10;

	public override void Redraw(int offx, int offy){
		base.Redraw (offx, offy);

		SHGUI.current.SetPixelFront ('☺', posx, posy, '0');
		SHGUI.current.SetPixelBack ('█', posx, posy, 'w');

		APPFRAME.Redraw (offx, offy);
		APPLABEL.Redraw (offx, offy);
		APPINSTRUCTION.Redraw (offx, offy);
	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;

		if (key == SHGUIinput.left){
			SHGUI.current.PlaySound(SHGUIsound.ping);
			posx -= 1;
		}
		else if (key == SHGUIinput.right){
			SHGUI.current.PlaySound(SHGUIsound.pong);
			
			posx += 1;
		}
		else if (key == SHGUIinput.up){
			SHGUI.current.PlaySound(SHGUIsound.ping);
			
			posy -= 1;
		}
		else if (key == SHGUIinput.down){
			SHGUI.current.PlaySound(SHGUIsound.pong);
			
			posy += 1;
		}

		if (key == SHGUIinput.esc)
			SHGUI.current.PopView ();
		
		if (key == SHGUIinput.enter) {
			MojaKustomFunkcja();
		}
	}

	SHGUIview ostatniDodanyWidok;

	void MojaKustomFunkcja(){
		if (ostatniDodanyWidok != null) {
			ostatniDodanyWidok.Kill();
			ostatniDodanyWidok = null;
		}

		var kill = new SHGUItempview (4f);
		var frame = new SHGUIframe (posx + 1, posy - 1, posx + "blam blam blam".Length + 4, posy + 1, 'z');
		var black = new SHGUIrect (posx + 1, posy - 1, posx + "blam blam blam".Length + 4, posy + 1, 'z', ' ', 2);
		kill.AddSubView (black);
		kill.AddSubView (frame);
		//kill.AddSubView (new SHGUItext ("blam blam blam", posx + 3, posy, 'r'));
		var promp = new SHGUIprompter (posx + 3, posy, 'r');
		promp.SetInput ("blam^W6 blam^W3 blam");
		kill.AddSubView (promp);

		/*
		var chat = new SHGUIguruchatwindow ();
		chat.SetContent("blam blam blam");
		chat.x = posx + 1;
		chat.y = posy;
		kill.AddSubView (chat);
		*/
		
		ostatniDodanyWidok = AddSubView (kill);

	}
}
