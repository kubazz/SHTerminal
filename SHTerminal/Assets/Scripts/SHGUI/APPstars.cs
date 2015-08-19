
using System.Collections.Generic;
using UnityEngine;


public class APPstars: SHGUIappbase
{
	public List<Vector3> stars;
	public APPstars (): base("starry-night-app-by-3.14")
	{
		//stars = new List<Vector3> ();

	}
	
	public override void Redraw(int offx, int offy){

		base.Redraw (offx, offy);

		if (fade < 0.99f)
			return;

		if (stars == null) {
			stars = new List<Vector3> ();
		}
		while (stars.Count < 100) {
			stars.Add(new Vector3(Random.Range(1, SHGUI.current.resolutionX - 1), Random.Range(1, SHGUI.current.resolutionY - 1), Random.Range(30, 1000)));
		}

		for (int i = 0; i < stars.Count; ++i){
			stars[i] = new Vector3(stars[i].x, stars[i].y, stars[i].z - Time.unscaledDeltaTime * 5);

			if (stars[i].z > 0){
				SHGUI.current.SetPixelFront('.', (int)stars[i].x, (int)stars[i].y, 'w');
			}
			if (stars[i].z < 0 && stars[i].z > -.5f){
				SHGUI.current.PlaySound(SHGUIsound.ping);
				SHGUI.current.SetPixelFront('*', (int)stars[i].x, (int)stars[i].y, 'w');
				stars[i] = new Vector3(stars[i].x, stars[i].y, stars[i].z - .5f);
			}
			if (stars[i].z < -0.5f){
				SHGUI.current.SetPixelFront('*', (int)stars[i].x, (int)stars[i].y, 'w');
			}
			if (stars[i].z < -1f){
				SHGUI.current.SetPixelFront('o', (int)stars[i].x, (int)stars[i].y, 'w');
			}
			if (stars[i].z < -1.5f){
				SHGUI.current.SetPixelFront('0', (int)stars[i].x, (int)stars[i].y, 'w');
			}
			if (stars[i].z < -2f){
				SHGUI.current.SetPixelFront('O', (int)stars[i].x, (int)stars[i].y, 'w');
			}
			if (stars[i].z < -2.5f){
				SHGUI.current.SetPixelFront('+', (int)stars[i].x, (int)stars[i].y, 'w');
			}
			if (stars[i].z < -3f){
				SHGUI.current.SetPixelFront(' ', (int)stars[i].x, (int)stars[i].y, 'w');
			}

			if (stars[i].z < -4.0f){
				stars.RemoveAt(i);
				i--;
			}
		}


	}
}


