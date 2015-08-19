using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class SHGUIprompter : SHGUItext
{
	public string input = "";
	public StringBuilder output;
	
	public float baseCharDelay = 0.02f;
	public bool drawCarriage = false;

	float currentCharDelay = 0f;
	float waitMulti = 1f;
	float waitMultiPersistent = 1f;	
	int currentChar = 0;
	int baseSpeed = 1;
	bool ignoreDefaultPunctuationWaits = false;
	public int charBuffer = int.MaxValue;
	
	public bool manualUpdate = false;
	private bool confirmed = false;

	public int maxLineLength = 30;
	public int maxSmartBreakOffset = 7;

	public float initDelay = 0.3f;

	public delegate void ConsoleCallback();
	public ConsoleCallback thisConsoleCallback;

	public float noInteractionTimer = 0;
	
	public SHGUIprompter(int X, int Y, char Col):base("", X, Y, Col) {
		output = new StringBuilder ();
	}

	public void SetInput (string str, bool clear = true)
	{
		if (clear) Clear();
		input += str;

		if (str.Length > 2){
			if ((str[0] == '^') && (str[1] == 'F')){
				if (parent != null){
					if (parent as SHGUIguruchatwindow != null){
						(parent as SHGUIguruchatwindow).SetFrameColor(str[2]);
					}
				}
			}
		}
	}

	/*
	public void SetInput(ConsoleString str, bool clear = true){
		if (clear) Clear();
		input += str.GetText();
	}
	*/

	public void Stop(){
		baseSpeed = 0;
		input = "";
		manualUpdate = false;
		drawCarriage = false;
		currentCharDelay = -1;

		UpdateConsole (true);
		currentCharDelay = -1;
		
	}

	public void AddPrefix(string prefix){
		output.Append(prefix);
	}
	
	public override void Update ()
	{
		base.Update ();

		noInteractionTimer += Time.unscaledDeltaTime;

		initDelay -= Time.unscaledDeltaTime;
		if (initDelay > 0)
			return;

		if (fade < .99f)
			return;
		for (int i = 0; i < baseSpeed; ++i) {
			UpdateConsole ();
		}

		if (fadingIn)
			fade = 1f;
	}

	public void ShowInstant(){
		float t = currentCharDelay;
		
		while (currentChar < input.Length){
			currentCharDelay = -1;
			UpdateConsole(true);
		}
		
		currentCharDelay = t;
	}
	
	public void Clear(){
		currentChar = 0;
		input = "";
		output.Length = 0;
		output.Capacity = 0;
	}

	void UpdateConsole(bool force = false) {
		currentCharDelay -= Time.unscaledDeltaTime;
		
		if ((((!manualUpdate && currentCharDelay < 0) || (manualUpdate && charBuffer > 0)) && !IsFinished())||(force)) {
			waitMulti = 1f;
			
			if (currentChar < input.Length) {
				char parsedChar = GetNextChar();
				
				if (parsedChar == '^'){
					char commandChar = GetNextChar();

					if (commandChar == 'O'){
						if (thisConsoleCallback != null) thisConsoleCallback();
					}
					else if (commandChar == 'C'){
						color = GetNextChar();
					}
					else if (commandChar == 'I'){
						ignoreDefaultPunctuationWaits = !ignoreDefaultPunctuationWaits;
					}
					else if (commandChar == 'w'){
						waitMulti = int.Parse (GetNextChar().ToString()) * 1;
					}
					else if (commandChar == 'W'){
						waitMulti = int.Parse (GetNextChar().ToString()) * 10;
					}
					else if (commandChar == 'M'){
						waitMultiPersistent = int.Parse (GetNextChar().ToString()) * 1;
					}
					else if (commandChar == 'm'){
						waitMultiPersistent = 1 / int.Parse (GetNextChar().ToString()) * 1;
					}
					else if (commandChar == 'F'){
						if (parent != null){
							if (parent as SHGUIguruchatwindow != null){
								(parent as SHGUIguruchatwindow).SetFrameColor(GetNextChar());
							}
						}
						
					}
				}
				else {
					charBuffer--;
					output.Append(parsedChar);

					if (parsedChar == ' ') {
						if (!ignoreDefaultPunctuationWaits)
							waitMulti *= 3f;
					}
					else if (parsedChar == '.' || parsedChar == '!' || parsedChar == '?'){
						if (!ignoreDefaultPunctuationWaits)
							waitMulti *= 5f;

						if (color != 'r')
							SHGUI.current.PlaySound(SHGUIsound.tick);
						else
							SHGUI.current.PlaySound(SHGUIsound.redtick);

					}
					else if (parsedChar == ','){
						if (!ignoreDefaultPunctuationWaits)
							waitMulti *= 5f;

						if (color != 'r')
							SHGUI.current.PlaySound(SHGUIsound.tick);
						else
							SHGUI.current.PlaySound(SHGUIsound.redtick);
						
					}
					else {
						if (color != 'r')
							SHGUI.current.PlaySound(SHGUIsound.tick);
						else
							SHGUI.current.PlaySound(SHGUIsound.redtick);
						
					}
				}
			}
			else{
				charBuffer--;
			}
			
			currentCharDelay = baseCharDelay;
			currentCharDelay *= waitMulti * waitMultiPersistent;
		}

		text = output.ToString();

		SmartBreak(maxLineLength, maxSmartBreakOffset);
		
		if (drawCarriage && !IsFinished()) {
			if ((Time.unscaledTime * 4) % 2 > 1) {
				text += '█';
			}
			else{
				text += ' ';
			}
		}

		//TO BEDZIE MUUUUULIĆ
	}

	private char GetNextChar(){
		return input [currentChar++];
	}

	public bool IsFinished(){
		return (currentChar >= input.Length && (manualUpdate?confirmed:true) && (currentCharDelay < 0));
	}

	public bool IsAlmostFinished(){
		return (currentChar >= input.Length);
	}

	public int GetFirstLineLengthWithoutSpecialSigns(){
		int addChars = 0;
		int chars = 0;

		for (int i = 0; i < input.Length; ++i){
			if (input[i] == '^'){
				chars--;
				chars--;
			}
			else{
				chars++;
			}

			if (input[i] == '\n')
				break;
		}

		return chars;
	}

	public void SwitchToManualInputMode(){
		charBuffer = 0;
		baseCharDelay = -1;
		baseSpeed = 2;
		drawCarriage = true;

		manualUpdate = true;
	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;
		
		base.ReactToInputKeyboard (key);
		
		if (key == SHGUIinput.enter) {
			ReactionEnter();
		}
		
		if (key == SHGUIinput.any && charBuffer < 60){
			ReactionTyping();
		}
	}
	
	int lastXpos = 0;
	int lastYpos = 0;
	bool mouseVirgin = true;
    public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
    {	
		if (fadingOut)
			return;
		
		bool moved = false;
		if ((lastXpos != x || lastYpos != y)&&(!mouseVirgin)){
			moved = true;
		}
		mouseVirgin = false;
		
		lastXpos = x;
		lastYpos = y;
		
		if (!clicked){
			if (moved){ 

				//temporarily locked to bettter support the gamepad for E3
				//ReactionTyping(true);
			}
		}
		else if (clicked){
			if (IsAlmostFinished()){
				ReactionEnter();
			}
		}
		
		base.ReactToInputMouse (x, y, clicked, scroll);
	}

	private void ReactionEnter(){
		if (IsAlmostFinished()){
			confirmed = true;
		}
		noInteractionTimer = 0;

		SHGUI.current.PlaySound(SHGUIsound.confirm);
	}

	public void SetConfirmed(){
		confirmed = true;
	}

	private void ReactionTyping(bool slow = false){
		if (charBuffer < 60){
			if (!slow)
				charBuffer += 4;
			else
				charBuffer += 1;

			if (!IsAlmostFinished())
				noInteractionTimer = 0;
		}
	}
}