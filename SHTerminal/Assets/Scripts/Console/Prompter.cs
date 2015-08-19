using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;
using Utilities.CameraEffects;

public class Prompter : MonoBehaviour
{
	public static Prompter CURRENT;

	public float timer = 0.01f;
	private float currentTimer = 0f;
	private int currentChar = 0;
	public string textToDisplay = "";
	private bool carriageVisible = false;
	private string outputBuffer = "";
	float waitMulti = 1f;
	float waitMultiPersistent = 1f;
	private string color = "#ffffff";
	private bool colorChanged = true;
	public int baseSpeed = 1;

	GameObject beep4go;
	AudioSource beep4;

	public int CharsInLine = 40;
	int currentCharsInLine = 0;

	public bool waitForInput= false;
	public bool waitForActualInput = false;
	public bool waitForEnter = false;
	public bool currentInput=false;
	private bool distortedBeep = false;
	public bool silent = false;

	public delegate void ConsoleCallback();
	public ConsoleCallback thisConsoleCallback;

	public delegate void ExecuteCallback(string command);
	public ExecuteCallback thisExecuteCallback;

	public delegate void ExecuteCallbackNoParms();
	public ExecuteCallbackNoParms OnActivateCallback;
	public ExecuteCallbackNoParms OnDeactivateCallback;

	private float topVolume;

	public bool decay = true;
	int skippedChars = 0;
	float decayWaiter;
	float defaultDecayWaiter = .5f;

	private bool effectActivated = false;

	//private TerminalSounds sounds;

	private bool waitChecked = false;
	public float noInputWhileWaitingForInputTime = 0f;
	public float noInputWhileWaitingForEnterTime = 0f;

	// Use this for initialization
	void Awake (){
		SetStandardActivationEffects();
		CURRENT = this;

		this.gameObject.GetComponent<Text> ().text = "";
		
		beep4go = new GameObject ();
		beep4 = beep4go.AddComponent<AudioSource> ();
		beep4.clip = Resources.Load ("Sounds/Source/prompter_character_sound") as AudioClip;
		beep4.panStereo = 0f;

		topVolume = 0.05f;

		decayWaiter = defaultDecayWaiter;

		var s = GameObject.Instantiate (Resources.Load ("Sounds/TerminalSoundsPrefab")) as GameObject;
		//sounds = s.GetComponent<TerminalSounds> ();
	}

	public void DisplayText (string str, bool clear = true)
	{
		if (clear) ClearInstant();
		textToDisplay += str;
	}

	public void DisplayText(ConsoleString str, bool clear = true){
		if (clear) ClearInstant();
		textToDisplay += str.GetText();
	}
	
	int inputBuffer = 0;
	string lastKey;

	public bool gainingSpeed = false;
	float wainter = 0;

	public void Update ()
	{
		waitChecked = false;
		actualInputCheckedThisFrame = false;
		wainter -= Time.unscaledDeltaTime;

		if (outputBuffer.Length > skippedChars){
			if (OnActivateCallback != null){
				if (!effectActivated)
					OnActivateCallback();

				effectActivated = true;
			}
		}
		else{
			if (OnDeactivateCallback != null){
				if (effectActivated)
					OnDeactivateCallback();
				effectActivated = false;
			}
		}

		if (wainter < 0){
			if (gainingSpeed) baseSpeed++;
			wainter = 0.1f;
		}

		for (int i = 0; i < baseSpeed; ++i){
			UpdateConsole();
		}

		//Debug.Log("isnextcharescape: " + IsNextCharEscapeOrInterpunction());

	}

	public void ShowInstant(){
		float t = currentTimer;

		while (currentChar < textToDisplay.Length){
			currentTimer = -1;
			UpdateConsole();
		}

		currentTimer = t;

	}

	public void Clear(){
		textToDisplay = "";
		decay = true;		
	}

	public void ClearInstant(){
		currentChar = 0;
		skippedChars = 0;
		currentCharsInLine = 0;
		textToDisplay = "";
		outputBuffer = "";

		colorChanged = true;
		waitForInput = false;
		waitForEnter = false;
		waitForActualInput = false;

		carriageVisible = false;
	}

	bool actualInputCheckedThisFrame = false;
	private float randomGlitchTimer = 0f;
	private string currentInputString = "";
	void UpdateConsole(bool force = false) {
	    bool repeat = false;
		currentCharsInLine = 0;//////HACK

		currentTimer -= Time.unscaledDeltaTime;
		
		if (InputManager.ActiveDevice.RightTrigger || (Input.anyKey && Input.inputString != "" && Input.inputString != lastKey && waitForInput)){
			lastKey = Input.inputString;
			if (inputBuffer <= 0)
				inputBuffer = 2;
			else
				inputBuffer += inputBuffer * inputBuffer;
		}
		
		//Debug.Log("last key: " + lastKey);
		
		if (!waitForInput){
			inputBuffer = 0;
		}

        bool enterPressed = (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || InputManager.ActiveDevice.RightBumper);
		bool inputAllow = (waitForInput==false || (waitForInput==true && inputBuffer > 0));
		if (waitForEnter && !enterPressed) {
			inputAllow = false; inputBuffer = 0;
			if (Input.inputString != ""){

				//WRONG KEY PRESSED WHEN WAITING FOR ENTER SOUND
				//sounds.PlayCursorError();	
			}
		} 
        //Debug.Log(waitForEnter);
		if (!waitForEnter && enterPressed) {inputAllow = false; inputBuffer = 0;}
	    if (waitForEnter && enterPressed) {
	        inputAllow = true;
	        inputBuffer = 4;
	    }

		if (!waitChecked){
			waitChecked = true;

			if (waitForEnter && !inputAllow)
				noInputWhileWaitingForEnterTime += Time.unscaledDeltaTime;
			else
				noInputWhileWaitingForEnterTime = 0f;

			if (waitForInput && !inputAllow)
				noInputWhileWaitingForInputTime += Time.unscaledDeltaTime;
			else
				noInputWhileWaitingForInputTime = 0f;
		}

		if ((currentTimer < 0 && inputAllow)||(force)) {
			waitMulti = 1f;
			
			if (currentChar < textToDisplay.Length) {
				string parsedChar = GetNextChar();
				
				if (parsedChar == "^"){
					string commandChar = GetNextChar();
					
					if (commandChar == "C"){
						string colorChar = GetNextChar();
						if (colorChar == "w"){
							color = "#ffffff";
						}
						else if (colorChar == "r"){
							color = "#ff0000";
						}
						else if (colorChar == "g"){
							color = "#00ff00";
						}
						else if (colorChar == "b"){
							color = "#999999";
						}
						else if (colorChar == "x"){
							color = "#000000";
						}
						colorChanged = true;
					}
					else if (commandChar == "I"){
						if (waitForInput) waitForEnter = true;
						if (!carriageVisible) carriageVisible = true;
						waitForInput = !waitForInput;

						if (waitForInput) waitMultiPersistent = 0;
						else waitMultiPersistent = 1;
					}
					else if (commandChar == "O"){
						if (thisConsoleCallback != null) thisConsoleCallback();
					}
					else if (commandChar == "w"){
						waitMulti = int.Parse (GetNextChar()) * 1;
					}
					else if (commandChar == "W"){
						waitMulti = int.Parse (GetNextChar()) * 10;
					}
					else if (commandChar == "M"){
						waitMultiPersistent = int.Parse (GetNextChar()) * 1;
					}
					else if (commandChar == "m"){
						waitMultiPersistent = 1 / int.Parse (GetNextChar()) * 1;
					}
					else if (commandChar == "B"){

						if (!silent) {
							//beep2.Play();
						}
					}
					else if (commandChar == "b"){
						distortedBeep = !distortedBeep;
					}
					else if (commandChar == "A"){
						waitForActualInput = !waitForActualInput;
					}
				}
				else {
					bool useBeep2 = false;
					if (waitForInput) inputBuffer--;
					if (waitForEnter) {
						inputBuffer = 0;
						lastKey = "";
						useBeep2 = true;
						waitMulti = 0f;

						carriageVisible = false;
					    repeat = true;

					}

					waitForEnter = false;

					if (parsedChar == "\n") currentCharsInLine = 0;
					currentCharsInLine++;
					string endline = (currentCharsInLine >= CharsInLine)?"\n":"";
					if (currentCharsInLine >= CharsInLine){
						currentCharsInLine = 0;
					}

					if (colorChanged){
						outputBuffer += "<color=" + color + ">" + parsedChar + endline + "</color>";
						colorChanged = false;
					}
					else{
						outputBuffer = outputBuffer.Substring(0, outputBuffer.Length - 8);
						outputBuffer += parsedChar + endline + "</color>";
					}
					
					if (parsedChar == " ") {
						waitMulti *= 5f;
					} else {
						if (!useBeep2){
							if (!distortedBeep)
							if (!silent) {
								//sounds.PlayCursor();
							}
									
								//beep.PlayMutated (.005f, 5);
							else if (!silent) {
								//beep3.Play();
							}
											
								//beep3.PlayMutated (.005f, 5);
						}
						else if (!silent) {
							//sounds.PlayNewLine();
						}
								//
							//beep2.PlayMutated (.005f, 5);
					}

					decayWaiter = defaultDecayWaiter;
				}
			}
			else{
				decayWaiter -= Time.unscaledDeltaTime;
				if (decay && decayWaiter < 0){
					waitMulti = 2;
					if (outputBuffer.Length > skippedChars){
						string first = GetFirstCharFromOutPut();
						if (first == "<"){
							while(first != ">"){
								skippedChars++;
								first = GetFirstCharFromOutPut();
							}
							skippedChars++;
						}

						if ((outputBuffer.Length - 1 - skippedChars) > 0)
							outputBuffer = outputBuffer.Substring(0, skippedChars) + outputBuffer.Substring(skippedChars + 1, outputBuffer.Length - 1 - skippedChars);
						else{
							outputBuffer = "";
							skippedChars = 0;
						}
						if (!silent) {
							//beep1.Play();
						}
							
					}

				}
			}

			currentTimer = timer;
			currentTimer *= waitMulti * waitMultiPersistent;
		}

		/*
		string optinallyGlitchedBuffer = outputBuffer;

		if (true){
			randomGlitchTimer -= Time.unscaledDeltaTime;

			if (randomGlitchTimer < 0){
				this.gameObject.GetComponent<Text> ().text = optinallyGlitchedBuffer;
				randomGlitchTimer = 0.2f;
			}
		}
		*/

		
		this.gameObject.GetComponent<Text> ().text = outputBuffer;
		if (carriageVisible) {
			if ((Time.unscaledTime * 4) % 2 > 1) {
				this.gameObject.GetComponent<Text> ().text += "█";
			}
			else{
				this.gameObject.GetComponent<Text> ().text += "<color=#000000>█</color>";
			}
		}

		Text T = this.gameObject.GetComponent<Text>();

		if (waitForInput && IsNextCharEnterOrInterpunction() || repeat)UpdateConsole(true);

		/*
		if (this.gameObject.GetComponent<Text>().preferredHeight - offset > 430){
			Vector3 p = this.gameObject.GetComponent<RectTransform>().position;
			//CharacterInfo info = new CharacterInfo();
			//T.font.GetCharacterInfo('A', out info);
			float offadd = 31.7f;
			counter ++;
			counter = 1;
			this.gameObject.GetComponent<RectTransform>().position = p + Vector3.up * offadd;
			offset += offadd * counter;
		}
		*/

	}

	void Backspace(){
		if (waitForActualInput && currentInputString.Length > 0){
			actualInputCheckedThisFrame = true;
			outputBuffer = outputBuffer.Remove(outputBuffer.Length - 9, 1);
			currentInputString = currentInputString.Remove(currentInputString.Length - 1, 1);
		}
	}

	
	float counter = 0;

	float offset = 0;

	private string GetNextChar(){
		string next = textToDisplay.Substring(currentChar, 1);
		//Debug.Log("getnextchar: " + next);
		currentChar++;
		return next;
	}

	private bool IsNextCharEnterOrInterpunction(){
		if (currentChar >= textToDisplay.Length) return false;
		string next = textToDisplay.Substring(currentChar, 1);
		//if (next == "^") next += textToDisplay.Substring(currentChar + 1, 1);
        return (next == "^E"  || next == "^" || next == "." || next == "!" || next == "?" || next == "," || next == " " || next == "\n");
	}

	private string GetFirstCharFromOutPut(){
		string first = outputBuffer.Substring(skippedChars, 1);
		//Debug.Log("getnextchar: " + next);
		return first;
	}

	public float tweenedVolume = 1f;
	public void SetVolume(float volume, float time){
		if (time <= 0.00001f){

		}
		else{

			
		}
	}

	private void standardActivateEffect(){
		Debug.Log("Activation Effect");
		if (CameraEffectsManager.Instance["LoFiEffect"] != null)
			CameraEffectsManager.Instance["LoFiEffect"].Play(0.25f);
	
	}

	private void standardDeactivateEffect(){
		Debug.Log("Deactivation Effect");
		if (CameraEffectsManager.Instance["LoFiEffect"] != null)
			CameraEffectsManager.Instance["LoFiEffect"].Play(Utilities.CameraEffects.PlayMode.Reversed,0.25f);
	}

	public void ResetActivationEffects(){
		OnActivateCallback = null;
		OnDeactivateCallback = null;
	}

	public void SetStandardActivationEffects(){
		OnActivateCallback += standardActivateEffect;
		OnDeactivateCallback += standardDeactivateEffect;
	}

	public static bool IsInConsoleWritingMode(){
		if ((CURRENT != null) && (CURRENT.gameObject.activeInHierarchy)){
			if (CURRENT.waitForInput || CURRENT.waitForActualInput || CURRENT.waitForEnter)
				return true;
		}

		return false;
	}

	public bool IsFinished(){
		return (currentChar >= textToDisplay.Length);
	}

	public int GetOutputBufferLength(){
		return outputBuffer.Length;
	}
}

public class ConsoleString{

	private string TEXT = "";

	//Commands available
	// ^T Toggle carriage
	// ^I Toggle typing input (also toggles character delay multiplier between 0 and 1) 
	// ^E Require Enter
	// ^Mx Character show delay multiplier set persistenly to x
	// ^mx Character show delay multiplier set persistenly to 1/x
	// ^wx Short wait x times standard timer
	// ^Wx Long wait 10x times standard timer
	// ^Cx Set color to x (r: red, g: green, b: blue, w: white)
	// \n New Line

	public ConsoleString Add(string text){
		TEXT += text;
		return this;
	}

	public ConsoleString AddLine(string text){
		TEXT += text + "\n";
		return this;
	}

	public ConsoleString NewLine(){
		TEXT += "\n";
		return this;
	}

	public ConsoleString TypingInput(){
		TEXT += "^I";
		return this;
	}

	public ConsoleString DelayLongPersistant(int x){
		TEXT += "^M" + x;
		return this;
	}

	public ConsoleString DelayPersistant(int x){
		TEXT += "^m" + x;
		return this;
	}

	public ConsoleString Wait(int x){
		TEXT += "^w" + x;
		return this;
	}

	public ConsoleString WaitLong(int x){
		TEXT += "^W" + x;
		return this;
	}

	public ConsoleString Color(string c){
		TEXT += "^C" + c;
		return this;
	}

	public string GetText(){
		return TEXT;
	}

	public ConsoleString AddDialogueLineOther(string name, string text){
		//TEXT += name + ": " + text + "\n";
		TEXT += "^Cb" + text + "\n";
		return this;
	}

	public ConsoleString AddDialogueLineMine(string name, string text){
		//TEXT += name + ": ^T^I" + text + "^E^T^I\n";
		TEXT += "^Cw^I" + text + "^I\n";

		return this;
	}

	public ConsoleString AddCommandInputLine(string text){
		TEXT += "^Cw>:^I" + text + "^I\n";
		
		return this;
	}


}
