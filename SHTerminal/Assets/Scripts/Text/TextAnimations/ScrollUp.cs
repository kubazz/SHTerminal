﻿using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class ScrollUp : TextAnimation {

	//TODO: rework for the GUIText animation
	override protected void StartAnimation(){
		DefaultStart();
		Vector3 asd = this.transform.localPosition;

		this.transform.localPosition = new Vector3(asd.x, asd.y + 1f, asd.z);

		PlaySound ();
		
		HOTween.To (this.transform, .5f * durationScale, new TweenParms ()
		            .Prop ("localPosition", asd - Vector3.up * 2f)
		            .UpdateType (UpdateType.TimeScaleIndependentUpdate)
		            .Ease (EaseType.Linear)
		            .OnComplete(FinishAnimation)
		            .AutoKill(true));
		
	}

	override protected void FinishAnimation(){
		
		
		ShowNext ();
		Kill ();
		
	}
}
