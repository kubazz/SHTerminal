
using System;
using System.Collections.Generic;
using UnityEngine;

public class SHRLentity{

	public APPshrl app;
	public int x;
	public int y;

	public bool del = false;

	public int id;

	public SHRLentity(APPshrl app, int x, int y){
		this.app = app;
		this.x = x;
		this.y = y;

		id = app.GetId ();
	}

	public virtual void Update(int ticks){
	}

	public virtual void Draw(){
	
	}
}

public class SHRLgun: SHRLitem{

	public int shootDelay = 2;
	public int ammoMin = 3;
	public int ammoMax = 5;
	public int ammoCount = 4;
	
	public SHRLgun(APPshrl app, int x, int y):base(app, x, y){
		pickable = true;
		color = 'z';
	}

	public SHRLitem SetPistol(){
		shootDelay = 2;
		ammoMin = 3;
		ammoMax = 5;

		killsOnContact = false;
		stunsOnContact = false;

		pickable = true;

		gfx = 'g';
		gfx2 = 'G';

		ammoCount = UnityEngine.Random.Range (ammoMin, ammoMax);
		return this;
	}

	public bool Shoot(int originx, int originy, int dirx, int diry, int shooterId, bool ignoreAmmo){
		if (ammoCount > 0 || ignoreAmmo) {
			var item = new SHRLitem (app, originx, originy);
			item.SetBullet ();
			item.Launch(shooterId, dirx, diry, 1);
			app.AddEntity(item);
			if (!ignoreAmmo) ammoCount--;

			app.pendingTicks += 1;

			return true;

		} else {
			if (ammoCount == 0){
				ammoCount--;
				app.Popup("no ammo", originx, originy);

				return true;
			}
			else{
				return false;
			}
		}
	}
}

public class SHRLitem: SHRLentity{
	public int trailLength = 0;
	public int maxTrailLength = 3;

	public int flySpeed = 1;
	public int flySpeedTimer = 1;

	public int dirx;
	public int diry;
	int traillenght = 0;

	public bool fading = false;
	public bool flying = false;
	public bool pickable = false;
	
	public int ownerid;

	public bool killsOnContact = false;
	public bool stunsOnContact = false;
	public char gfx;
	public char gfx2;

	public bool isBullet = false;
	public bool isThrowable = false;

	public char color = 'r';

	public SHRLitem(APPshrl app, int x, int y):base(app, x, y){
	}

	public SHRLitem SetBullet(){
		killsOnContact = true;
		stunsOnContact = false;
		gfx = '*';

		isBullet = true;

		return this;
	}

	public SHRLitem SetThrowable(){
		killsOnContact = false;
		stunsOnContact = false;
		pickable = true;
		gfx = '#';
		gfx2 = '#';
		color = 'z';
		isThrowable = true;

		return this;
	}
	
	public SHRLitem Launch(int ownerid, int dirx, int diry, int flySpeed){
		this.ownerid = ownerid;
		this.dirx = dirx;
		this.diry = diry;

		this.flySpeed = flySpeed;
		this.flySpeedTimer = flySpeed;

		flying = true;
		fading = false;
		del = false;
		
		return this;
	}

	public override void Update(int ticks){
		
		SHRLentity e = app.GetEntity (x, y);
		if (e != null && !fading) {
			SHRLenemy E = (e as SHRLenemy);
			if (E != null && E.id != ownerid){
				if (stunsOnContact || killsOnContact){
					E.Kill();
					Stop ();
				}
			}
		}

		if (flying && !fading) {
			SHRLitem I = app.GetShatterableItem (x, y, id);

			if (I != null){
				I.Stop();
				this.Stop();
			}
			else{
				if (app.GetTile(x, y) == 3){
					this.Stop();
					app.SetTile(0, x, y);
				}
			}
		}
		
		if (!fading) {
			flySpeedTimer--;
			if (flying && flySpeedTimer <= 0){
				x += dirx;
				y += diry;
				
				traillenght++;
				if (traillenght > 3)
					traillenght = 3;
				
				if (app.GetTile (x, y) == 1) {
					Stop ();
				}

				flySpeedTimer = flySpeed;
			}
		} else {
			traillenght--;
			if (traillenght < 0){
				del = true;
			}
		}
	}
	
	public void Stop(){
		
		fading = true;
		app.AddEntity(new SHRLparticle(app, x, y, "#%$*", "rrrr"));

		del = true;
		
	}
	
	

	float blinkTime;
	bool show = true;
	public override void Draw(){
		if (flying) {
			blinkTime -= Time.unscaledDeltaTime;
			if (blinkTime < 0){
				blinkTime = .1f;
				//show = !show;
			}
			if (!show) return;
		}

		if (!fading)
			app.DrawCharFaded (gfx, x, y, color, true);
		
		if (dirx != 0) {
			for (int i = 1; i < traillenght; ++i) {
				app.DrawCharFaded ('-', x - dirx * i, y - diry * i, color, true);
			}
		}
		
		if (diry != 0) {
			for (int i = 1; i < traillenght; ++i) {
				app.DrawCharFaded ('|', x - dirx * i, y - diry * i, color, true);
			}
		}
		
	}
}

public class SHRLenemy: SHRLentity{

	public char color = 'r';
	int walkDelay = 0;
	public bool isPlayer = false;

	bool horizontalFirst = true;

	public SHRLitem item;

	public int dirx = 0;
	public int diry = 0;

	public SHRLenemy(APPshrl app, int x, int y):base(app, x, y){
		horizontalFirst = true;
	}

	public SHRLenemy SetHorizontal(bool v){
		horizontalFirst = v;
		return this;
	}

	public override void Update(int ticks){

		if (isPlayer) {
			return;
		}

		walkDelay -= 1;
		if (walkDelay <= 0) {

			if (CanSeePlayer() && item != null && (item is SHRLgun)){
				int dirx = 0;
				int diry = 0;
				if (x < app.player.x){
					dirx = 1;
				}
				else if (x > app.player.x){
					dirx = -1;
				}

				if (y < app.player.y){
					diry = 1;
				}
				else if (y > app.player.y){
					diry = -1;
				}

				if (dirx != 0) diry = 0;

				(item as SHRLgun).Shoot (x, y, dirx, diry, id, true);
				this.dirx = dirx;
				this.diry = diry;
				walkDelay = 6;
				
			}
			else{
				if (item == null){
					walkDelay = 4;
				}
				else
					walkDelay = 6;
				
				if (horizontalFirst){
					if (!WalkMatchPlayerHorizontal()){
						WalkMatchPlayerVertical();
					}
				}
				else{
					if (!WalkMatchPlayerVertical()){
						WalkMatchPlayerHorizontal();
					}
				}
			}
		}
	}

	bool CanSeePlayer(){
		if (CanSeePlayer (1, 0))
			return true;
		if (CanSeePlayer (-1, 0))
			return true;
		if (CanSeePlayer (0, -1))
			return true;
		if (CanSeePlayer (0, 1))
			return true;

		return false;
	}

	bool CanSeePlayer(int dirx, int diry){

		for (int i = 1; i < 30; ++i) {
			int X = x + i * dirx;
			int Y = y + i * diry;
			if (app.GetTile(X, Y) == 1){
				return false;
			}

			//if ((X == app.player.x || X == app.player.x - 1 || X == app.player.x + 1)
			//	&& (Y == app.player.y || Y == app.player.y - 1 || Y == app.player.y + 1))
			if (X == app.player.x && Y == app.player.y)
				return true;

			if (app.GetEntity(X, Y) != null && (app.GetEntity(X, Y) is SHRLenemy))
				return false;
		}

		return false;
	}

	bool WalkMatchPlayerHorizontal(){
		if (app.player.x < x)
			return Walk (-1, 0);
		else if (app.player.x > x)
			return Walk (1, 0);

		return false;
	}

	bool WalkMatchPlayerVertical(){
		if (app.player.y < y)
			return Walk (0, -1);
		else if (app.player.y > y)
			return Walk (0, 1);
		
		return false;
	}
	
	public override void Draw(){
		SHRLitem b = app.GetKillingOrStunningItem (x, y);
		if (b != null && b.ownerid != id) {
			if (b.killsOnContact) Kill ();
			b.Stop();
		}

		char g = '@';
		if (item != null)
			g = item.gfx2;
		if (app.shooting && isPlayer) {
			app.DrawCharFaded (g, x, y, (Time.realtimeSinceStartup % .2f > .1f)?('r'):('w'), true);
		} else {
			app.DrawCharFaded (g, x, y, color, true);
		}

		/*
		if (item != null) {
			item.x = this.x + dirx;
			item.y = this.y + diry;
			
			item.x = this.x;
			item.y = this.y;
			
			if (Time.realtimeSinceStartup % .4f < .1f)
				item.Draw(offx, offy);
		}
		*/
	}

	public void Kill(){
		del = true;
		Drop ();
		app.AddEntity(new SHRLparticle(app, x, y, "#%$*", "rrrr"));
	}

	public void Drop(){
		if (item != null) {
			app.AddEntity(item);
			item.x = this.x;
			item.y = this.y;
		}
	}

	public bool Walk(int dirx, int diry){
		int tile = app.GetTile (x + dirx, y + diry);
		if (tile != 1 && tile != 2 && tile != 3) {
			SHRLentity e = app.GetEntity(x + dirx, y + diry);
			if (e != null){
				if (e is SHRLenemy){
					SHRLenemy E = e as SHRLenemy;
					if (isPlayer){
						E.Kill();
						return false;
					}
					else{
						if (E.isPlayer){
							E.Kill();
						}
						return false;
					}
				}
			}

			x += dirx;
			y += diry;

			this.dirx = dirx;
			this.diry = diry;
			
			return true;

		} else {
			app.AddEntity(new SHRLparticle(app, x + dirx, y + diry, "▓▒░▒▓", "rrrrr"));
			return false;
		}
	}
}

public class SHRLparticle: SHRLentity{
	string animation;
	string colors;
	float timer;
	int index = 0;
	public SHRLparticle(APPshrl app, int x, int y, string animation, string colors):base(app, x, y){
		this.animation = animation;
		this.colors = colors;
	}

	public override void Draw(){
			
		app.DrawCharFaded (animation[index], x, y, colors[index], true);
		
		timer -= Time.unscaledDeltaTime;

		if (timer < 0) {
			timer = .05f;
			index++;

			if (index > animation.Length - 1 || index > colors.Length - 1){
				del = true;
				index = 0;
			}
		}
	}

}

public class APPshrl: SHGUIview
{
	public enum SHRLstate {Gameplay, SHSH, Dead};

	public SHRLstate state;

	int[] level;
	int levelWidth = 64;
	int levelHeight = 24;

	int camerax = 0;
	int cameray = 0;

	int id = 0;

	public int pendingTicks = 0;
	float tickTimer = 0;

	public SHRLenemy player;
	public bool shooting = false;
	
	public List<SHRLentity> entities;

	public string LevelName;

	public APPshrl (string levelName)
	{
		SetLevel (levelName);
	}

	public APPshrl SetLevel(string levelName){
		this.LevelName = levelName;
		RestartLevel ();

		return this;
	}

	void RestartLevel(){

		if (SHSHquit != null) {
			SHSHquit.Kill();
		}
		SHSHquit = null;
		this.SHSHtimer = 0;

		state = SHRLstate.Gameplay;
		entities = new List<SHRLentity> ();
		
		LoadLevel ("shrlTESTLEVEL");
		
		overrideFadeInSpeed = .1f;
		overrideFadeOutSpeed = .1f;
	}

	public int GetId(){
		return id++;
	}

	private float SHSHtimer = 0;
	private SHGUIview SHSHquit;
	public override void Update(){
		base.Update ();

		if (fade < .99f) {
			return;
		}

		if (state == SHRLstate.Gameplay) {

			overrideFadeInSpeed = 1f;
			overrideFadeOutSpeed = 1f;

			RefreshEntities ();

			tickTimer -= Time.unscaledDeltaTime;
			if (tickTimer < 0 && pendingTicks > 0) {
				tickTimer = .05f;
				TickEntities (1);
				pendingTicks--;
			}

			if (GetEnemyCount() <= 0){
				state = SHRLstate.SHSH;
			}

			if (player.del){
				state = SHRLstate.Dead;
			}
		} else if (state == SHRLstate.SHSH) {
			overrideFadeInSpeed = .5f;
			overrideFadeOutSpeed = .5f;

			for (int i = 0; i < 5; ++i){
				string sh = (UnityEngine.Random.value > .5f)?("SUPER"):("HOT");
				Popup(sh, UnityEngine.Random.Range(0, SHGUI.current.resolutionX), UnityEngine.Random.Range(0, SHGUI.current.resolutionY), false);
			}	

			SHSHtimer += Time.unscaledDeltaTime;

			if (SHSHtimer > 1.5f && SHSHquit == null){

				SHSHquit = new SHGUIblinkview(.5f);

				string s = "HAND OVER THE CONTROL";

				SHSHquit.AddSubView(new SHGUIframe(0, 0, s.Length + 2, 2, 'z'));
				SHSHquit.AddSubView(new SHGUItext(s, 1, 1, 'r'));

				SHSHquit.x = (int)(SHGUI.current.resolutionX / 2) - (int)(s.Length / 2);
				SHSHquit.y = (int)(SHGUI.current.resolutionY / 2) - 2;

				AddSubView(SHSHquit);
			}

		} else if (state == SHRLstate.Dead) {
			overrideFadeInSpeed = .5f;
			overrideFadeOutSpeed = .5f;
			
			SHSHtimer += Time.unscaledDeltaTime;
			
			if (SHSHtimer > 0.2f && SHSHquit == null){
				
				SHSHquit = new SHGUIblinkview(.5f);
				
				string s = "AGAIN";
				
				SHSHquit.AddSubView(new SHGUIframe(0, 0, s.Length + 2, 2, 'z'));
				SHSHquit.AddSubView(new SHGUItext(s, 1, 1, 'r'));
				
				SHSHquit.x = (int)(SHGUI.current.resolutionX / 2) - (int)(s.Length / 2);
				SHSHquit.y = (int)(SHGUI.current.resolutionY / 2) - 2;
				
				AddSubView(SHSHquit);
			}
			
		}
	}

	public override void Redraw(int offx, int offy){
		base.Redraw (offx, offy);

		DrawLevel ();
		DrawEntities ();

		if (lastPopup != null) {
			lastPopup.Redraw(offx, offy);
		}

	}
	
	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;

		if (pendingTicks > 0) {
			//tickTimer = 0;
			return;
		}

		if (key == SHGUIinput.esc)
			Kill ();

		if (state == SHRLstate.Gameplay) {
			if (player.del) {
				return;
			}

			if (key == SHGUIinput.up) {
				ClearPopups ();
				if (!shooting) {
					pendingTicks += 4;
					player.Walk (0, -1);
				} else {
					pendingTicks += 1;
					InteractInDirection (0, -1);
					shooting = false;
				}
			} else if (key == SHGUIinput.down) {
				ClearPopups ();
				if (!shooting) {
					pendingTicks += 4;
					player.Walk (0, 1);
				} else {
					pendingTicks += 1;
					InteractInDirection (0, 1);
					shooting = false;
				}
			} else if (key == SHGUIinput.left) {
				ClearPopups ();
				if (!shooting) {
					pendingTicks += 4;
					player.Walk (-1, 0);
				} else {
					pendingTicks += 1;
					InteractInDirection (-1, 0);
					shooting = false;
				}
			} else if (key == SHGUIinput.right) {
				ClearPopups ();
				if (!shooting) {
					pendingTicks += 4;
					player.Walk (1, 0);
				} else {
					pendingTicks += 1;
					InteractInDirection (1, 0);
					shooting = false;
				}
			}

			if (key == SHGUIinput.enter) {
				ClearPopups ();
				shooting = !shooting;

				if (shooting) {
					if (player.item == null)
						Popup ("pickup");
					else {
						if (player.item is SHRLgun && (player.item as SHRLgun).ammoCount >= 0) {
							Popup ("shoot");
						} else {
							Popup ("throw");
						
						}
					}
				}
			}
		} else if (state == SHRLstate.SHSH) {
			if (key == SHGUIinput.enter && SHSHtimer > .5f) {
				Kill ();
			}
		} else if (state == SHRLstate.Dead) {
			if (key == SHGUIinput.enter && SHSHtimer > .5f) {
				RestartLevel();
			}	
		}
			
	}

	void InteractInDirection(int dirx, int diry){
		if (player.item != null){
			if (player.item is SHRLgun){
				if ((player.item as SHRLgun).Shoot(player.x, player.y, dirx, diry, player.id, false) == false){
					ThrowInDirection(dirx, diry);
					pendingTicks += 1;
					player.dirx = dirx;
					player.diry = diry;
				}else{
					player.dirx = dirx;
					player.diry = diry;
				}
			}
			else{
				ThrowInDirection(dirx, diry);
				pendingTicks += 1;
			}
		}
		else{
			var e = GetPickableItem(player.x + dirx, player.y + diry);
			if (e != null){				
				player.item = e;
				e.del = true;

				pendingTicks += 2;
			}
		}
	}

	void ThrowInDirection(int dirx, int diry){
		if (player.item == null)
			return;

		entities.Add(player.item);
		player.item.x = player.x;
		player.item.y = player.y;
		
		player.item.Launch(player.id, dirx, diry, 2);
		player.item.trailLength++;
		

		player.item.stunsOnContact = true;
		player.item.killsOnContact = true;
		player.item = null;
	}
    public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll){
		//if (fadingOut)
		//	return;
		
		//if (clicked)
		//	SHGUI.current.PopView ();
	}

	#region ENTITIES

	public SHRLentity GetEntity(int x, int y){
		for (int i = 0; i < entities.Count; ++i){
			if (entities[i].x == x && entities[i].y == y){
				return entities[i];
			}
		}
		return null;
	}

	public SHRLitem GetShatterableItem(int x, int y, int ignoreOwner = -10){
		for (int i = 0; i < entities.Count; ++i){
			if (entities[i].x == x && entities[i].y == y && (entities[i] is SHRLitem)){
				var E = (entities[i] as SHRLitem);

				if (E.id == ignoreOwner)
					continue;

				if (E.isThrowable)
					return E;
				else if (!E.isBullet && E.flying)
					return E;
			}
		}
		return null;
	}

	public SHRLitem GetKillingOrStunningItem(int x, int y){
		for (int i = 0; i < entities.Count; ++i){
			if (entities[i].x == x && entities[i].y == y && (entities[i] is SHRLitem) && ((entities[i] as SHRLitem).killsOnContact || (entities[i] as SHRLitem).stunsOnContact)){
				return (entities[i] as SHRLitem);
			}
		}
		return null;
	}

	public SHRLitem GetPickableItem(int x, int y){
		for (int i = 0; i < entities.Count; ++i){
			if (entities[i].x == x && entities[i].y == y && (entities[i] is SHRLitem) && (entities[i] as SHRLitem).pickable){
				return (entities[i] as SHRLitem);
			}
		}
		return null;
	}
	
	public void AddEntity(SHRLentity e){
		entities.Add (e);
	}

	public void RefreshEntities(){
		for (int i = 0; i < entities.Count; ++i) {
			if (entities[i].del == true){
				entities.RemoveAt(i);
				i--;
			}
		}
	}

	void TickEntities(int ticks){
		for (int i = 0; i < entities.Count; ++i) {
			entities [i].Update (ticks);
		}
	}

	void DrawEntities(){
		for (int i = 0; i < entities.Count; ++i) {
			entities [i].Draw();
		}

		for (int i = 0; i < entities.Count; ++i) {
			if (entities [i] is SHRLenemy) entities[i].Draw();
		}
	}

	#endregion


	#region LEVEL

	void LoadLevel(string levelName){
		string levelString = SHGUI.current.GetASCIIartByName (levelName);

		level = new int[levelWidth * levelHeight];

		int x = 0;
		int y = 0;

		for (int i = 0; i < levelString.Length; ++i) {
			if (levelString[i] == '\n'){
				x = 0;
				y++;
				continue;
			}

			if (levelString[i] == '#'){
				SetTile(1, x, y);
			}
			else if (levelString[i] == '.'){
				SetTile(2, x, y);
			}
			else if (levelString[i] == '='){
				SetTile(3, x, y);
			}
			else if (levelString[i] == 'G'){
				var e = new SHRLenemy(this, x, y);
				e.item = new SHRLgun(this, x, y);
				(e.item as SHRLgun).SetPistol();
				entities.Add(e);
				SetTile(0, x, y);
			}
			else if (levelString[i] == 'T'){
				var e = new SHRLenemy(this, x, y);
				e.item = new SHRLgun(this, x, y);
				(e.item as SHRLgun).SetPistol();
				e.SetHorizontal(false);
				entities.Add(e);
				SetTile(0, x, y);
			}
			else if (levelString[i] == 'F'){
				var e = new SHRLenemy(this, x, y);
				entities.Add(e);
				SetTile(0, x, y);
			}
			else if (levelString[i] == 'R'){
				var e = new SHRLenemy(this, x, y);
				entities.Add(e);
				e.SetHorizontal(false);
				SetTile(0, x, y);
			}
			else if (levelString[i] == 'g'){
				SHRLgun g = new SHRLgun(this, x, y);
				g.SetPistol();
				entities.Add(g);
			}
			else if (levelString[i] == '&'){
				SHRLitem g = new SHRLitem(this, x, y);
				g.SetThrowable();
				entities.Add(g);
			}
			else if (levelString[i] == '@'){
				player = new SHRLenemy(this, x, y); 
				player.color = 'w';
				player.isPlayer = true;
				entities.Add(player);
				SetTile(0, x, y);
			}

			x++;
		}
	}

	public int GetTile(int x, int y){
		if (x < 0 || x >= levelWidth || y < 0 || y >= levelHeight)
			return 1;

		return level[y + x * levelHeight];
	}

	public void SetTile(int v, int x, int y){
		if (x < 0 || x >= levelWidth || y < 0 || y >= levelHeight)
			return;
		
		level [y + x * levelHeight] = v;
	}

	void DrawLevel(){
		for (int x = 0; x < levelWidth; ++x) {
			for (int y = 0; y < levelHeight; ++y) {
				int tile = GetTile(x, y);
				if (tile == 1)
					DrawCharFaded('█', x, y, 'z', true);
				else if (tile == 2)
					DrawCharFaded('░', x, y, 'z', true);
				else if (tile == 3)
					DrawCharFaded('█', x, y, 'w', true);
			}
		}
	}

	string substituteChars = "▀▄ █ ▌ ░ ▒ ▓ ■▪                   ";
	string substituteColors = "wwwwwwwwwwwwwwwwwwwzzzzzzzzzzzzrrrrrrrrrr";
	
	public void DrawCharFaded(char c, int x, int y, char col, bool front){
		if (UnityEngine.Random.value > fade) {
			c = substituteChars [UnityEngine.Random.Range (0, substituteChars.Length - 1)];
			col = substituteColors [UnityEngine.Random.Range (0, substituteColors.Length - 1)];
		}

		if (front)
			SHGUI.current.SetPixelFront(c, x + camerax, y + cameray, col);
		else
			SHGUI.current.SetPixelBack(c, x + camerax, y + cameray, col);
	
	}
	#endregion

	SHGUIview lastPopup;

	public void Popup(string msg){
		Popup (msg, player.x, player.y);
	}

	public void Popup(string msg, int originx, int originy, bool overrideLast = true){
		if (lastPopup != null && overrideLast) {
			lastPopup.Kill();
		}

		int X = camerax + originx + 2;
		int Y = cameray + originy;

		SHGUItempview pop = new SHGUItempview (.75f);
		SHGUIview text = new SHGUItext (msg, X, Y, 'w');
		pop.AddSubView (new SHGUIrect (X, Y, X + msg.Length - 1, Y, 'z', ' ', 2));
		pop.AddSubView (text);

		this.AddSubViewBottom (pop);

		lastPopup = pop;
	}

	public void ClearPopups(){
		if (lastPopup != null) {
			lastPopup.Kill();
		}
	}

	public int GetEnemyCount(){
		int count = 0;
		for (int i = 0; i < this.entities.Count; ++i) {
			if (entities[i] is SHRLenemy)
				count++;
		}

		return count - 1;
	}
}


