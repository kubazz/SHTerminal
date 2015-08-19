using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class APPsegway : SHGUIview {

	private List<SHGUItext> text;
	private List<string> queue;
	private float thisLineDelay = 1f;
	private float endingDelay = 1f;
	
	public APPsegway (string content)
	{
		queue = new List<string>();
		text = new List<SHGUItext>();
		string[] lines = content.Split('|');

		foreach (var l in lines){
			AddToQueue(l.ToUpper(), false);
		}

		allowCursorDraw = false;

	}

	void AddToQueue(string line, bool spacing = true){
		queue.Add(line);
	}

	void AddText(string line){
		var lines = line.Split(';');
		string firstline = lines[0];

		thisLineDelay = .8f;

		if (firstline.Contains(">")){
			int index = firstline.IndexOf('>');
			var cmd = firstline.Substring(0, index);
			lines[0] = firstline.Substring(index + 1);
			
			for (int i = 0; i < cmd.Length; ++i){
				if (cmd[i] == 'D'){
					this.thisLineDelay *= 1.2f;				
				}			
			}
		}

		foreach (var L in text){
			L.Kill();
			L.remove = true;
		}

		int height = 0;
		foreach (var l in lines){
			var t = new SHGUItext(SHGUI.current.GetASCIIartFromFont(l), 0, 0, 'w');
			
			t.x =  (int)(SHGUI.current.resolutionX / 2) - (int)(t.GetLineLength() / 2); 
			t.y =  (int)(SHGUI.current.resolutionY / 2) - (int)(t.CountLines() / 2) + height; 
			AddSubView(t);
			height += (int)(SHGUI.current.resolutionY / 2) - (int)(t.CountLines() / 2) - 1;
			t.PunchIn(1f);

			text.Add(t);
		}

		foreach (var t in text){
			t.y -= (int)(height / 4);
		}

		PlaySound();

	}

	void PlaySound(){
		AudioClip sound;
			sound = null;
//			AudioManager.SetTimeScaleIndependent(src);
	}	

	public override void Update(){

		thisLineDelay -= Time.unscaledDeltaTime;
		if (thisLineDelay < 0 && queue.Count > 0){
			AddText(queue[0]);
			queue.RemoveAt(0);
		}

		if (queue.Count == 0){
			endingDelay -= Time.unscaledDeltaTime;
			if (endingDelay < 0){
				Kill ();
			}
		}

		base.Update();
	}
	
}
