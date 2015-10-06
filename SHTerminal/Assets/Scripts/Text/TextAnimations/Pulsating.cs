using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Pulsating : TextAnimation {

	// Use this for initialization
	void Start () {
		TextManager.THIS.AllowTextDisplay = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		float f = doTextFading?colorFade:1f;
		if (colorFade < .5f) f = 0.90f;
		else f = 1f;
		
		if (!doTextFading) f = 1f;
		//f = 1f;
		foreach (var T in textGameObjects){
			T.GetComponent<GUIText>().color = new Color(word.color.r * f*1.2f, word.color.g * f, word.color.b * f, TextManager.THIS.currentAlpha * (0.5f + 0.5f * Mathf.Sin(Time.realtimeSinceStartup * 15f)));
		}
	}

	protected override void StartAnimation(){
		
		DefaultStart();

		TextManager.THIS.AllowTextDisplay = false;

		//HACK: does nothing except calling OnComplete;
		/*
		HOTween.To (this.transform, .5f * durationScale, new TweenParms ()
		            .Prop ("localPosition", this.transform.localPosition)
		            .UpdateType (UpdateType.TimeScaleIndependentUpdate)
		            .Ease (EaseType.Linear)
		            .OnComplete(FinishAnimation)
		            .AutoKill(true));
		            */
		
	}
}
