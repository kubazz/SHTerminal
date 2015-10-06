using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class FlyOut : TextAnimation {

	/*
	//TODO: rework for the GUIText animation
	override protected void StartAnimation(){
		this.transform.localPosition = new Vector3 (0f, 0f, 0f);
		
		HOTween.To (this.transform, 0f * durationScale, new TweenParms ()
		            .Prop ("localScale", new Vector3(1f, 1f, 1f))
		            .UpdateType (UpdateType.TimeScaleIndependentUpdate)
		            .Ease (EaseType.Linear)
		            .OnComplete(FinishAnimation)
		            .AutoKill(true));
		
	}
	
	override protected void FinishAnimation(){
		GlowHitter.HitVerySoftly ();
		ShowNext ();
		HOTween.To (this.transform, 1f * durationScale, new TweenParms ()
		            .Prop ("localScale", new Vector3(10f, 10f, 10f))
		            .Delay(0.5f * durationScale)
		            .UpdateType (UpdateType.TimeScaleIndependentUpdate)
		            .Ease (EaseType.Linear)
		            .OnComplete(Kill)
		            .AutoKill(true));
	}
	*/
}
