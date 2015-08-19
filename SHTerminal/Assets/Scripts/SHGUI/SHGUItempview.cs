
using System;
using System.Collections.Generic;
using UnityEngine;


public class SHGUItempview: SHGUIview
{
	protected float timeToDie = 1f;
	protected float startTime = 0f;
	public bool killParent = false;

	private float speedMultiplier = 1f;

	public bool speedUpOnInteraction = false;

	public SHGUItempview (float destroyAfter)
	{
		startTime = destroyAfter;
		timeToDie = destroyAfter;
	}

	public float GetCurrentProgress(){
		return Mathf.Clamp(((startTime - timeToDie) / timeToDie), 0f, 1f);
	}

	public override void Update(){
		if (fade < .99f)
			return;

		timeToDie -= Time.unscaledDeltaTime * speedMultiplier;

		if (timeToDie < 0){
			//PunchIn(0);
			Kill();

			if (killParent){
				if (parent != null){
					parent.Kill();
				}
			}
			
			//remove = true;
			//hidden = true;
		}

		base.Update();
	}

	public override void ReactToInputKeyboard (SHGUIinput key)
	{
		base.ReactToInputKeyboard (key);

		if (!speedUpOnInteraction)
			return;

		if (key != SHGUIinput.none) {
			speedMultiplier = 10000f;
			PunchIn(.9f);
		}
	}

	int lastCursorX = -1;
	int lastCursorY = -1;
	public override void ReactToInputMouse (int x, int y, bool clicked, SHGUIinput scroll)
	{
		base.ReactToInputMouse (x, y, clicked, scroll);

		if (!speedUpOnInteraction)
			return;

		if (clicked) {
			speedMultiplier = 4f;
			PunchIn (.9f);
		} else {
			if (x != lastCursorX || y != lastCursorY){
				if (lastCursorX != -1 && lastCursorY != -1)
					timeToDie *= .9f;
			}
		}

		lastCursorX = x;
		lastCursorY = y;
	}

}


