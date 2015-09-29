using System.Collections.Generic;
using UnityEngine;


public class APPxcopy: SHGUIappbase
{
	private float mainPhase;
	private float startTime;
	private float curTime;
	private string[] stringList = {"allocating memory","baking neurons","duplicating mind","postprocessing"}; 
	private SHGUItext statusText;

	private string asciiBrain = null;
	private string asciiCypherBrain = null;

	private int waveIndex = 0;
	private int waveLength = 9;
	private float waveTime = 0.0f;
	private float waveTotalTime = 3.0f;

	private bool popupShowed = false;

	public APPxcopy(): base("xcopy")
	{
		//stars = new List<Vector3> ();
		mainPhase = 0;
		startTime = 10;
		curTime = startTime;
		statusText = new SHGUItext ("xxxxxxxx", 22, 18, 'w');
		AddSubView(statusText);
			
		asciiBrain = SHGUI.current.GetASCIIartByName ("Brain");
		asciiCypherBrain = SHGUI.current.GetASCIIartByName ("CypherBrain");

		int width = 30;
	    int textoff = 4;
	}
	
	private void drawFloppy(float phase, int pivotX, int pivotY, char onColor='0', char offColor='0'){


		int width = 16;
		int height = 8;

		float cur = 0;
		float bufferLen = width * height;

		char curZnak = 'X';

		for(int y=pivotY; y<pivotY+height; y++){
			for(int x=pivotX; x<pivotX+width; x++){


				if(Mathf.Abs(phase - cur/bufferLen)<0.05f){

				}else if(phase > cur/bufferLen){
					SHGUI.current.SetPixelFront(curZnak, x, y, onColor);
				}else{
					SHGUI.current.SetPixelFront(curZnak, x, y, offColor);
				}

				cur++;
			}
		}


	}
	private void drawBrain(float phase, int pivotX, int pivotY, char onColor='0', char offColor='0'){
		
		int x = pivotX;
		int y = pivotY;
		
		for (int i=0; i<asciiBrain.Length; i++) 
		{
			if(asciiBrain[i] == '\r')
			{
				i++;
				x = pivotX;
				y++;
			}
			else
			{


				if( Mathf.Abs(phase - (float)i/(float)asciiBrain.Length)<0.05f ){
					SHGUI.current.SetPixelFront(asciiBrain[i], x, y, 'w');
				}else if(phase > (float)i/(float)asciiBrain.Length){
					SHGUI.current.SetPixelFront(asciiBrain[i], x, y, onColor);
				}else{
					SHGUI.current.SetPixelFront(asciiBrain[i], x, y, offColor);
				}
				x++;
			}		
		}

	}






	protected void drawProgressBar(float phase, float length, float xOffset, float yOffset){

		for (float i = 0; i < length; ++i) {
			char progressChar = '-';
			
			if(phase > (i/length - 1.0f/length) && phase < i/length ){
				SHGUI.current.SetPixelFront(')', (int)(xOffset+i), (int)yOffset, 'r');
			}
			else if(phase<i/length){
				SHGUI.current.SetPixelFront('_', (int)(xOffset+i), (int)yOffset, 'w');
			}else{
				SHGUI.current.SetPixelFront('#', (int)(xOffset+i), (int)yOffset, 'r');
			}
		}
	}


	protected float getNormalizedSubPhase(float sourcePhase, float newPhaseStart, float newPhaseEnd){

		if (sourcePhase < newPhaseStart || sourcePhase > newPhaseEnd) {
			return -1;
		}

		return (sourcePhase - newPhaseStart) / (newPhaseEnd - newPhaseStart);

	}




/// <summary>
/// R///////////////////////////////////////////////////////////////////////////// </summary>
/// <param name="offx">Offx.</param>
/// <param name="offy">Offy.</param>


	bool popupOnScreen = false;
	public override void Redraw(int offx, int offy){
		
		base.Redraw (offx, offy);
		if (fade < 0.99f)
			return;

		//kontrola fazy
		mainPhase = Mathf.Clamp01 (1.0f - (curTime / startTime));
		curTime -= Time.unscaledDeltaTime;

		float copyBrainPhase = getNormalizedSubPhase(mainPhase,0.0f,0.8f);
		float flipBrainPhase = getNormalizedSubPhase(mainPhase,0.8f,0.9f);
		float flippedBrainPhase = getNormalizedSubPhase(mainPhase,0.9f,1.0f);

		int brainPivotX = 3;
		int brainPivotY = 3;

		if (copyBrainPhase >= 0.0f) {
			drawBrain (copyBrainPhase, brainPivotX,    brainPivotY,  'w','5');
			drawBrain (copyBrainPhase, 30+brainPivotX, brainPivotY,'2','0');
		}

		if (flipBrainPhase >= 0.0f) {
			drawBrain (1.0f-flipBrainPhase, brainPivotX,    brainPivotY,  'w','2');
			drawBrain (flipBrainPhase,      30+brainPivotX, brainPivotY,'r','2');
		}

		if (flippedBrainPhase >= 0.0f) {
			drawBrain (1.0f, brainPivotX,    brainPivotY,'w','2');
			drawBrain (1.0f, 30+brainPivotX, brainPivotY,  'r','2');
		}

		if(popupOnScreen)
			base.Redraw (offx, offy);

		//progressbar
		float progressBarLength = 40;
		float progressBarPivotX = 13;
		float progressBarPivotY = 14;
		drawProgressBar (mainPhase, progressBarLength, progressBarPivotX, progressBarPivotY);
		drawProgressBar (copyBrainPhase, progressBarLength, progressBarPivotX, progressBarPivotY+1);
		drawProgressBar (flipBrainPhase, progressBarLength, progressBarPivotX, progressBarPivotY+2);

		//statusText.x = 10;
		//statusText.y = 10;
		if (mainPhase != 1.0f) {

			statusText.text = stringList [(int)Mathf.Floor (mainPhase * (stringList.Length))];
			statusText.text = mainPhase + " " + copyBrainPhase + " " + flipBrainPhase;


		} else {
			if(Mathf.Sin(Time.realtimeSinceStartup * 10.0f) > 0.0f)
			{
				dontDrawViewsBelow = false;
				statusText.text = "mózg skopiowany";
				statusText.color = 'r';
				allowCursorDraw = true;
				if(popupOnScreen == false){
					popupOnScreen = true;
					int width = 30;
					int textoff = 4;
					
					SHGUIview container = AddSubView(new SHGUIview());
					container.x = (int)(SHGUI.current.resolutionX / 2) - (int)(width / 2);
					container.y = (int)(SHGUI.current.resolutionY / 2) - 4;
					
					container.AddSubView(new SHGUIrect(0, 0, width, 4));
					container.AddSubView(new SHGUIframe(0, 0, width, 4, 'w')); 
					container.AddSubView(new SHGUItext("MÓZG SKOPIOWANY?????", 1 + textoff, 2, 'w'));
					
					container.AddSubView(new SHGUIButton("YES", width - 6, 4, 'w').SetActionKey(SHGUIinput.enter).SetOnActivate(
						() => {
						if (fadingOut)
							return;
						//SaveManager.Instance.ResetStory();
						//SHGUI.current.PopView();
					}).GoFromRight());
					container.AddSubView(new SHGUIButton("NO─", 5, 4, 'w').SetActionKey(SHGUIinput.esc).SetOnActivate(() => {
						if (fadingOut)
							return;
						//SHGUI.current.PopView();
					}));
				}

			}
			else
			{
				statusText.text = "";
			}
		}

	}

	public override void ReactToInputKeyboard(SHGUIinput key)
	{
		if (key == SHGUIinput.enter){

		}


		if (key == SHGUIinput.esc)
			SHGUI.current.PopView ();
	}
	
	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
	{
	    if (fadingOut)
	        return;

		if (clicked){
			x -= 1;
			y -= 1;
			

		}
	}
}


