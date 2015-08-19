using UnityEngine;
using System.Collections;

public class piOsMenuLauncher : MonoBehaviour {

	public static bool firstMenuShowupSinceStartup = true;
	public bool ForceBetaEnding = false;
	public bool ForceFirstLaunch = false;
	public bool ForcePause = false;
	// Use this for initialization
	void Start () {
		Init();
	}

    public void Init() {
        //if (ForcePause)
        //    firstMenuShowupSinceStartup = false;

        
            ShowStandardMenu();
        
    }

	void ShowStandardMenu(){
		var menu = this.gameObject.AddComponent<piOsMenu>();
		menu.dontShowExploit = true;

		menu.ForceStart (true);	
	}

	// Update is called once per frame
	void Update () {



	
	}
}
