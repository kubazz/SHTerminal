using System;
using UnityEngine;
using UnityEngine.UI;
using Utilities.CameraEffects;

public class APPuserlevel: SHGUIview
{
	string sceneName;
	string bundleURL;
	WWW downloader;
	public APPuserlevel (string bundleURL, string sceneName)
	{
		this.sceneName = sceneName;
		this.bundleURL = bundleURL;

		AddSubView (new SHGUItext ("launching user level " + bundleURL, 0, 0, 'w').BreakTextForLineLength(SHGUI.current.resolutionX));

		downloader = new WWW (bundleURL);
	}
	
	public override void Update(){
		base.Update();

		if (downloader != null && downloader.assetBundle != null) {
			AssetBundle bundle = downloader.assetBundle;
			Application.LoadLevel(sceneName);
		}
	}
}


