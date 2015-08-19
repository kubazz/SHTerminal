
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;


public class SHGUIview
{
	public int x = 0;
	public int y = 0;
	public char color = 'w';

	public List<SHGUIview> children;
	public SHGUIview parent = null;

	public float fade = 0f;
	public bool fadingIn = false;
	public bool fadingOut = false;

	public bool hidden = false;
	public bool interactable = true;
	public bool dontDrawViewsBelow = true;
	public bool allowCursorDraw = true;

	public bool remove = false;

	public int id;

	float timer = 0;

	public float overrideFadeInSpeed = 1f;
	public float overrideFadeOutSpeed = 1f;

	public float forcedFadeSpeedRegardless = 0f;

	public SHGUIview ()
	{
		Init ();
	}

	protected void Init(){
		id = SHGUI.GetId ();

		children = new List<SHGUIview> ();
		
		fadingIn = true;
	}

    public virtual void OnEnter() { }
    public virtual void OnExit() { }

	public virtual void Update(){
        SHGUI.current.OnScreen = true;
		for (int i = 0; i < children.Count; ++i){
			children[i].Update();
			if (children[i].fade < 0.6f && fadingIn) break;

			if (children[i].remove){
				children.RemoveAt(i);
				i--;
			}
		}
	}

	bool hack = false;
	public void FadeUpdate(float inSpeedMulti = 1f, float outSpeedMulti = 1f){
		if (!hack && forcedFadeSpeedRegardless != 0) {
			hack = true;
			fade = 0.5f;
		}

		if (fadingIn) {
			if (forcedFadeSpeedRegardless == 0)
				fade += 0.2f * Time.unscaledDeltaTime * 30 * inSpeedMulti * overrideFadeInSpeed;
			else
				fade += forcedFadeSpeedRegardless;

			if (fade > 1f){
				fade = 1f;
				fadingIn = false;
			}
		}
		
		if (fadingOut) {
			if (forcedFadeSpeedRegardless == 0)
				fade -= 0.3f * Time.unscaledDeltaTime * 30 * outSpeedMulti * overrideFadeOutSpeed;
			else
				fade -= forcedFadeSpeedRegardless;

			if (fade < 0){
				fade = 0f;
				fadingOut = false;
				
				remove = true;
			}
		}

		for (int i = 0; i < children.Count; ++i) {
			children [i].FadeUpdate (inSpeedMulti * overrideFadeInSpeed, outSpeedMulti * overrideFadeOutSpeed);
		}
	}

    public void ForceFadeRecursive(float fade) {
        this.fade = fade;
        foreach (var child in children) {
            child.ForceFadeRecursive(fade);
        }
    }

	public virtual void Redraw(int offx, int offy){
		if (hidden)
			return;

		for (int i = 0; i < children.Count; ++i){
			children[i].Redraw(offx + x, offy + y);
		}
	}

	public SHGUIview AddSubView(SHGUIview view){
		children.Add (view);
		view.parent = this;
		return view;
	}

	public SHGUIview AddSubViewBottom(SHGUIview view){
		children.Insert (0, view);
		view.parent = this;
		return view;
	}

	public void RemoveView(SHGUIview v){
		for (int i = 0; i < children.Count; ++i){
			if (children[i].id == v.id){
				children.RemoveAt(i);
				break;
			}
		}
	}


	public virtual void Kill(){
		fadingIn = false;
		fadingOut = true;

		for (int i = 0; i < children.Count; ++i) {
			children[i].Kill();
		}	
	}

	public void KillChildren(){
		for (int i = 0; i < children.Count; ++i) {
			children[i].Kill();
		}	
	}

	public void KillInstant(){
		remove = true;
	}

	public void KillChildrenInstant(){
		children = new List<SHGUIview> ();
	}

	public virtual void ReactToInputKeyboard(SHGUIinput key){
		for (int i = 0; i < children.Count; ++i) {
			children[i].ReactToInputKeyboard(key);
		}
	}

	public virtual void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll){
		for (int i = 0; i < children.Count; ++i) {
			children[i].ReactToInputMouse(x, y, clicked, scroll);
		}
	}

	public void PunchIn(float startFade = 0f){
		if (fadingOut)
			return;

		fade = startFade;
		fadingIn = true;

		for (int i = 0; i < children.Count; ++i){
			children[i].PunchIn(startFade);
		}
	}

	public void ForcedSoftFadeIn(){
		fade = -1f;
		fadingIn = true;
		
		for (int i = 0; i < children.Count; ++i){
			children[i].ForcedSoftFadeIn();
		}
	}

	public void SpeedUpFadeIn(){
		if (!fadingIn)
			return;

		fade += 0.4f;

		for (int i = 0; i < children.Count; ++i){
			children[i].SpeedUpFadeIn();
		}

	}

	public bool FixedUpdater(float delay = .05f){
		timer -= Time.unscaledDeltaTime;
		
		if (timer < 0) {
			timer = delay;
			return true;
		}
		return false;
	}

	public SHGUIview SetColor(char c){
		color = c;
		return this;
	}

	public SHGUIview SetColorRecursive(char c){
		color = c;

		for (int i = 0; i < children.Count; ++i){
			children[i].SetColor(color);
		}

		return this;
	}
}


