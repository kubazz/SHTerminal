using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;

public class TalkingSceneBase : MonoBehaviour {

	public bool isSkippable = true;
	private float currentAlpha = 1.0f;
	private float destAlpha = 1.0f;
	private float alphaTransitionTime = 0.0f;
	private float alphaTransitionProgress = 0.0f;
	
	private bool skipIntro = false;
	private float prompterFadeoutTime = 4.0f;
	
	private float endWait = 0f;
	
	// Use this for initialization
	void Start () {
		Init();

	}

	protected void Init(){
		Prompter.CURRENT.thisConsoleCallback = endIntro;

		Prompter.CURRENT.decay = false;
	}
	
	float time = 0;
	bool pressed = false;
	
	// Update is called once per frame
	void Update () {
		//Prompter.CURRENT.GetComponent<Text>().material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, 1.0f));
		
		time += Time.unscaledDeltaTime;



		bool tutorial = false;
		if (Prompter.CURRENT.noInputWhileWaitingForInputTime > 2f){


			tutorial = true;
		} 

		if (Prompter.CURRENT.noInputWhileWaitingForEnterTime > 2f){

			tutorial = true;
		}

		if (!tutorial){

		}

		if (Input.GetKeyDown(KeyCode.Escape) && isSkippable){
			Next();
		}

		if (Input.inputString != null && Input.inputString != ""){
			pressed = true;
		}

					
		if (Prompter.CURRENT.IsFinished()){
			endWait += Time.unscaledDeltaTime;

		}
		if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return)){
			if (Prompter.CURRENT.IsFinished()){
				Next();
			}
		}

	}
	
	void Next(){

	}
	
	void endIntro() {

	}
	
	public void ActivateMenu(){

	}
}
