
using System;
using System.Collections.Generic;
using UnityEngine;


public class SHGUIrenderingtestbase: SHGUIappbase
{
	protected GameObject a;

	public SHGUIrenderingtestbase (string name): base(name)
	{
		//AddSubView(new SHGUItext("CUBE", (int)(SHGUI.current.resolutionX / 2 - 2), (int)(SHGUI.current.resolutionY / 2), 'w'));

	}
	
	float scale = 0;
	protected string label = "CUBE";
	public override void Redraw(int offx, int offy){
		
		base.Redraw (offx, offy);
		if (fade < 0.99f)
			return;
		a.gameObject.GetComponent<Renderer> ().enabled = true;
		a.gameObject.GetComponent<Renderer> ().material.color = Color.red;
	
		float rotoffx = 0;
		float rotoffy = Time.unscaledDeltaTime * 60f;
		float rotoffz = Time.unscaledDeltaTime * 20f;

		a.transform.rotation = Quaternion.Euler (a.transform.rotation.eulerAngles.x + rotoffx,
		                                         a.transform.rotation.eulerAngles.y + rotoffy,
		                                         a.transform.rotation.eulerAngles.z + rotoffz);


		scale += Time.unscaledDeltaTime * 3.1f;
		if (scale > 50.0f)
			scale = 50.0f;
		a.transform.localScale = new Vector3 (scale, scale, scale);
	
		//if (!FixedUpdater (.2f))
		//	return;


		//string glitch = " ░ ▒ ▓";
		string glitch = "▒";
		int i = 0;
		int marginx = 26;
		int marginy = 8;

		for (int Y = 1 + marginy; Y < SHGUI.current.resolutionY - 1 - marginy; ++Y) {
			for (int X = 1 + marginx; X < SHGUI.current.resolutionX - 1 - marginx; ++X) {
				if (UnityEngine.Random.value < scale * 2.5f){
					SHGUI.current.SetPixelFront(label[i], X, Y, 'w');
				}
				i++;
				if (i > label.Length - 1) i = 0;
			}
		}

	}

	public override void Kill(){
		base.Kill ();
		GameObject.Destroy (a);
	}
}


