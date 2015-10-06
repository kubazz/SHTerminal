using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class FlyIn : TextAnimation {

	/*
	//TODO: rework for the GUIText animation
	override protected void StartAnimation(){
		this.transform.localScale = new Vector3 (0f, 0f, 1f);

		GlowHitter.HitVerySoftly ();
		PlaySound ();
		
		HOTween.To (this.transform, 1f * durationScale, new TweenParms ()
		            .Prop ("localScale", new Vector3(1f, 1f, 1f))
		            .UpdateType (UpdateType.TimeScaleIndependentUpdate)
		            .Ease (EaseType.Linear)
		            .OnComplete(FinishAnimation)
		            .AutoKill(true));
		
	}
	
	override protected void FinishAnimation(){


		ShowNext ();
		Kill ();
	
	}
	*/
}
