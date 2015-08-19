
using System;
using System.Collections.Generic;
using UnityEngine;


public class APPsinus: SHGUIappbase
{

	public APPsinus (): base("sinus-app-by-3.14")
	{
		Randomize ();		
	}

	float time = 0;
	float sinusOffset = 0;
	string sinusText = "";
	string sinusSpacer = "              ";
	string sinusFiller = "--------------";

	int a = 0;
	int timer = 10;

	float speed = 10f;
	int height = 2;
	public override void Redraw(int offx, int offy){
		
		base.Redraw (offx, offy);
		//if (fade < 0.99f)
		//	return;
		time += Time.unscaledDeltaTime;

		sinusText = sinusSpacer + sinusSpacer + sinusFiller + "---S----U----P----E----R---";
		sinusText += sinusSpacer + "----H----O----T----" + sinusFiller + sinusSpacer + sinusSpacer;

		sinusOffset += .4f * Time.unscaledDeltaTime * speed;
		if (sinusOffset > sinusText.Length) {
			sinusOffset = 0;
			Randomize();
		}

		for (int i = (int)sinusOffset; i < sinusText.Length; ++i){
			float sss = mysin(time, i);

			char col = 'r';
			char t = sinusText[i];
			if (t == '-'){
				col = 'w';
				if (mysin(time - 0.1f, i) > mysin(time + 0.1f, i)){
					t = '/';
				}
				else{
					t ='\\';
				}
			}
			   
			int p = 13 + i * 2 - (int)sinusOffset * 2;
			if (p > SHGUI.current.resolutionX - 13) break;
			SHGUI.current.SetPixelFront(t, p, (int)(SHGUI.current.resolutionY / 2) - 1 + (int)(sss), col); 
			//SHGUI.current.SetPixelFront(sinusText[i], 10 + i, 10, 'g');



		}
	}

	void Randomize(){
		speed = UnityEngine.Random.Range (10, 20);
		height = (int)( UnityEngine.Random.Range (2, 5));
	}

	float mysin(float t, int i){
		return (height * Mathf.Sin(4 * t + (float)(i * Math.PI / 4)));
	}
}


