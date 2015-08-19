using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class SegwayBase : MonoBehaviour {

	protected bool canSkip = false;
	protected bool nexted = false;
	protected float endingTime = 0f;
	protected float timeFromLastInteraction = 0f;

	protected bool didDisplayPhrases = false;
	protected List<string> phrases = new List<string>();
	protected bool displayHints = true;

	protected float timer = 0f;
	// Use this for initialization

	void Start () {
		
		//EXAMPLE USAGE COMMENTED OUT BELOW
		//Just create a script inheriting from SegwayBase and write your text scripts in Start()
		//Always end the script with MarkEnding() !!!

		/*
		for (int i = 0; i < 10; ++i){
			TextManager.DisplayQuick("CwSBx^^>\\SUPER|CwSBx^^>/HOT", .1f * (12 - i));
		}
		for (int i = 0; i < 10; ++i){
			TextManager.DisplayQuick("CwSBx^^>\\SUPER|CwSBx^^>/HOT", .2f);
			TextManager.DisplayQuick("CwSBr>grab;the;gun", .1f);
		}

		for (int i = 0; i < 10; ++i){
			TextManager.DisplayQuick("CwSBx^^>" + i, .1f * (12 - i));
		}

		MarkEnding();
		*/
	}

	protected virtual void Init(){
		
	}
	

	protected void MarkEnding(){
		Debug.Log ("marked ending: " + Time.realtimeSinceStartup);
		//end a bit before the actual ending of queue - so the last text hangs during loading of next level
		displayHints = false;
		endingTime = Time.realtimeSinceStartup;
	}

	bool first = true;
	bool inited = false;
	// Update is called once per frame
	void Update () {
		timer += Time.unscaledDeltaTime;
		if (timer > .3f){
			if (!inited && Time.unscaledDeltaTime < 0.1f){
				inited = true;
				Init ();
			}
		}
		else{
			return;			
		}

		timeFromLastInteraction += Time.unscaledDeltaTime;
		if (Time.realtimeSinceStartup > endingTime && endingTime != 0f){
			Next();
		}

		if (InputManager.ActiveDevice.RightBumper.WasPressed || first) {

			first = false;
			NextPhrase();

			timeFromLastInteraction = 0;
			//TextManager.THIS.ShowSubtitle("");

		}

		if (timeFromLastInteraction > 5.5f && displayHints){
			//TextManager.THIS.ShowSubtitle("click to continue");
		}

		
	}

	protected void AddPhrase(string phrase){
		string[] p = phrase.Split('|');
		for (int i = 0; i < p.Length; ++i){
			phrases.Add(p[i]);
		}
	}

	protected void NextPhrase(){
		if (phrases.Count > 0){
			didDisplayPhrases = true;
			//TextManager.THIS.Clear();
			//TextManager.DisplayQuick(phrases[0], 10000);
			phrases.RemoveAt(0);
		}
		else{
			if (didDisplayPhrases) Next();
		}
	}

	protected virtual void Next(){
		if (nexted) return;

		nexted = true;
		//TextManager.THIS.ShowSubtitle("");
		//LevelSetup.UnlockNextLevel();
		//LevelSetup.LoadNextLevel();	
	}
}
