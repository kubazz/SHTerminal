/* TextManager.cs
 * 
 * Responsible for displaying full-screen messages to the player during gameplay using the 2D and 3D systems.
 * */

using UnityEngine;
using System.Collections.Generic;

// A list containing all fonts used in text overlay
using InControl;
using System;
using System.Text;
using Utilities.CameraEffects;
using PlayMode = Utilities.CameraEffects.PlayMode;

public enum TextOverlayAnimationTypes {
	NoEffect,
	Pulsating,
	ScrollRight,
	ScrollLeft,
	ScrollUp,
	ScrollDown
};

[AddComponentMenu("SUPERHOT/Omni/Text Manager")]

// TODO: consider if the GUIText should be dropped altogether
    
// TODO: text shaking
// TODO: color alteration
// TODO: soft glow
// TODO: film grain
// TODO: advanced movement

public class TextManager : MonoBehaviour {
	#region Variables
	public static TextManager 				THIS;
    public static Font[]           			fonts;

	public static AudioClip defaultWordSound;
	public static AudioClip defaultLastWordSound;

	public GameObject						emptyTextAnimation;

    private List<OverlayWord> wordStack = new List<OverlayWord>();
	private Transform TextContainer;

	public String 	  currentTutorialRequirement;
	public float 	  currentTutorialTextTimeout = 5f;
	public String     subtitleText;

	public GameObject Subtitle1;
	public GameObject Subtitle2;
	public GameObject Uptitle;
	public float 	  textFade = 1f;
	private	bool	  pulsating = true;
	private bool 	  uptitlePulsating = false;
	private bool	  uptitleFading = false;
	private float     uptitleAlpha = 1f;
    private Color     uptitleColor;

    public bool       LoopSubtitles = false;
    public bool       LoopUptitles  = false;
    public bool       IsReady = true;

    private float     srcAlpha                  = 1.0f;
    private float     destAlpha                 = 1.0f;
    public float      currentAlpha              = 1.0f;
    private float     alphaTransitionTime       = 0.0f;
    private float     alphaTransitionProgress   = 0.0f;

	private List<string> UptitleQueue;
    private float 	  uptitleTimer              = 1f;

    private List<string> subtitlesQueue         = new List<string>();
    private float subtitleTimer                 = 1.0f;
	
	public TextAnimation LastTextAnimation      = null;

	public delegate void OnCompleteFunction();
	private OnCompleteFunction OnComplete;
    private bool _killCurrentTextAnimationOnNextUpdate;

	public bool AllowTextDisplay = true;
    public bool RenderedToTexture = false;
    public Vector2 ScreenSize = new Vector2(1920.0f, 1080.0f);
    public bool Active = true;
    #endregion

    private float subtitleTimerOld = -1.0f;

	#region Monobehaviour Methods
	void Awake () {
		THIS = this;
        // load all fonts
		LoadFonts();
		TextContainer = this.transform.FindChild ("TextContainer");
	    Active = true;

        Vector2 menuTextureSize;

        //var menuRTQuality = GlobalShaderValueControl.GetMenuRTQuality();

        ScreenSize = RenderedToTexture ? new Vector2(ScreenSize.x, ScreenSize.y) : 
                                         new Vector2(Screen.width, Screen.height);

        Subtitle1.GetComponent<GUIText>().fontSize = (int)(ScreenSize.y / 20f);
        Subtitle2.GetComponent<GUIText>().fontSize = (int)(ScreenSize.y / 20f);

		if (Uptitle != null){
            Uptitle.GetComponent<GUIText>().fontSize = (int)(ScreenSize.y / 20f);
			UptitleQueue = new List<string>();
		}

        subtitleTimerOld = -1.0f;
	}

	void Start () {
		// initial display setup
		//FIXME: add oculus support;
		//ChangeDisplayMethod();

		//DelayedCall.CancelLast ();
	}

	void Update () {
	    if (_killCurrentTextAnimationOnNextUpdate)
	    {
            //This has been moved here from DisplayInternal to allow for using Display methods from other threads.
	        _killCurrentTextAnimationOnNextUpdate = false;
            if (LastTextAnimation != null)
            {
                LastTextAnimation.RemoveTweens();
                LastTextAnimation.Kill();
            }
	    }
        
        subtitleTimer -= Time.unscaledDeltaTime;
		if (subtitleTimer < 0) {
			if (subtitlesQueue.Count > 0) {
				SetSubtitle(subtitlesQueue[0]);
			    if (LoopSubtitles) {
                    subtitlesQueue.Add(subtitlesQueue[0]);
			    }
				subtitlesQueue.RemoveAt(0);
				subtitleTimer = 1f;
            }
            else if (subtitleTimerOld >= 0.0f) {
                SetSubtitle("");
            }
		}
        subtitleTimerOld = subtitleTimer;

        float alpha;
        if (pulsating && LoopSubtitles)
            alpha = 0.3f + (1.0f - Mathf.Abs(1.0f - 2.0f * subtitleTimer)) * 0.7f;
        else if (pulsating)
            alpha = 0.8f + (Mathf.Sin(Time.realtimeSinceStartup * 4.0f)) * 0.2f;
        else
            alpha = 1f;

        if (LastTextAnimation == null && wordStack.Count > 0 && Time.unscaledDeltaTime < 0.1f && Active == true)
        {
            ShowNextTextAnimation();
        }

        Subtitle1.GetComponent<GUIText>().color = new Color(Subtitle1.GetComponent<GUIText>().color.r,
                                            Subtitle1.GetComponent<GUIText>().color.g,
                                            Subtitle1.GetComponent<GUIText>().color.b,
                                            alpha * textFade);

        Subtitle2.GetComponent<GUIText>().color = new Color(Subtitle2.GetComponent<GUIText>().color.r,
                                            Subtitle2.GetComponent<GUIText>().color.g,
                                            Subtitle2.GetComponent<GUIText>().color.b,
                                            textFade);

		if (Uptitle != null && Active){
			float alpha2;
			if (uptitlePulsating)
				alpha2 = 0.7f + Mathf.Sin (Time.realtimeSinceStartup * 10f) * 0.3f;
			else
				alpha2 = 0.7f;

			if (uptitleFading){
				uptitleAlpha -= Time.unscaledDeltaTime;
			}

			Uptitle.GetComponent<GUIText>().color = new Color(Uptitle.GetComponent<GUIText>().color.r,
                                                Uptitle.GetComponent<GUIText>().color.g,
                                                Uptitle.GetComponent<GUIText>().color.b,
			                                    alpha2 * textFade * (uptitleFading?uptitleAlpha:1));

			uptitleTimer -= Time.unscaledDeltaTime;
			if (uptitleTimer < 0){
			    if (UptitleQueue.Count > 0) {
			        SetUptitle(UptitleQueue[0]);
			        if (LoopUptitles) {
                        UptitleQueue.Add(UptitleQueue[0]);
			        }
			        UptitleQueue.RemoveAt(0);
                    if (UptitleQueue.Count == 0)
                        AddUptitleToQueue("");
			        uptitleTimer = 1f;
			    } 
                //else {
                //    SetUptitle("");
                //}
			}
		}

       
		alphaTransitionProgress += Time.unscaledDeltaTime;
		alphaTransitionProgress = 1f;
        //currentAlpha = Mathf.Lerp(srcAlpha, destAlpha, alphaTransitionProgress / alphaTransitionTime);

        foreach (var text in TextContainer.GetComponentsInChildren<GUIText>()) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, currentAlpha);
        }

		// HACK: was previously:
//		else
//			alpha = 0.9f;
//		
//		
//		Subtitle1.guiText.color = new Color(Subtitle1.guiText.color.r,
//		                                    Subtitle1.guiText.color.g,
//		                                    Subtitle1.guiText.color.b,
//		                                    alpha * textFade);
//		
//		Subtitle2.guiText.color = new Color(Subtitle2.guiText.color.r,
//		                                    Subtitle2.guiText.color.g,
//		                                    Subtitle2.guiText.color.b,
//		                                    (1 - alpha) * textFade);

	}

    public void FadeInMainText(float time) {
        srcAlpha = 0.0f;
        destAlpha = 1.0f;
        alphaTransitionTime = time;
        alphaTransitionProgress = 0.0f;
    }

    public void FadeOutMainText(float time, bool instant = false) {
        srcAlpha = instant ? 0.0f : 1.0f;
        destAlpha = 0.0f;
        currentAlpha = 0.0f;
        alphaTransitionTime = time;
        alphaTransitionProgress = 0.0f;
    }

    public void ShowSubtitle(string text) {
        SetSubtitle(LocalizationManager.Instance.GetLocalized(text));
    }

    private static bool SubtitleBackgroundVisible = false;
	public static void SetSubtitle(String text, bool pulsating = true) {
	    if (THIS.Active == false)
	        return;

        if (string.IsNullOrEmpty(text) == false)
        {
			if (SubtitleBackgroundVisible == false && CameraEffectsManager.Instance["ShowSubtitle"] != null) {
                CameraEffectsManager.Instance["ShowSubtitle"].Play();
                SubtitleBackgroundVisible = true;
            }
            
        }
        else
        {
			if (SubtitleBackgroundVisible == true && CameraEffectsManager.Instance["ShowSubtitle"] != null) {
                CameraEffectsManager.Instance["ShowSubtitle"].Play(PlayMode.Reversed);
                SubtitleBackgroundVisible = false;
            }
        }

		THIS.pulsating = pulsating;

		THIS.Subtitle1.GetComponent<GUIText>().text = text;
		THIS.Subtitle2.GetComponent<GUIText>().text = text;
	}

	public static String GetCurrentSubtitle(){
		return THIS.Subtitle1.GetComponent<GUIText>().text;
	}
	
	public static void SetSubtitle2(String text, bool pulsating = true){
		THIS.pulsating = pulsating;
		
		THIS.Subtitle1.GetComponent<GUIText>().text = text;
		THIS.Subtitle2.GetComponent<GUIText>().text = text;
	}

    public static void SetSubtitle1(String text, Vector3 position, bool pulsating = true)
    {
        THIS.pulsating = pulsating;
        THIS.Subtitle1.transform.localPosition = position;
        THIS.Subtitle1.GetComponent<GUIText>().text = text;
    }

	public static void SetSubtitle2(String text, Vector3 position, bool pulsating = true){
		THIS.pulsating = pulsating;
		THIS.Subtitle2.transform.localPosition = position;
		THIS.Subtitle2.GetComponent<GUIText>().text = text;
	}

    private static bool UptitleBackgroundVisible = false;
    public static void SetUptitle(String text, bool pulsating = true, bool fading = false)
    {
        if (THIS.Active == false)
            return;
        if (string.IsNullOrEmpty(text) == false)
        {
            if (UptitleBackgroundVisible == false && CameraEffectsManager.Instance["ShowUptitle"] != null) {
                CameraEffectsManager.Instance["ShowUptitle"].Play();
                UptitleBackgroundVisible = true;
            }
        }
        else
        {
            if (UptitleBackgroundVisible == true && CameraEffectsManager.Instance["ShowUptitle"] != null) {
                CameraEffectsManager.Instance["ShowUptitle"].Play(PlayMode.Reversed);
                UptitleBackgroundVisible = false;
            }
        }

		if (THIS.Uptitle == null) 
            return;
		THIS.uptitlePulsating = pulsating;

		//Debug.Log("setting uptitlte: " + text);
		THIS.uptitleAlpha = 5f;
		THIS.uptitleFading = fading;
		THIS.Uptitle.GetComponent<GUIText>().text = text;
        THIS.Uptitle.GetComponent<GUIText>().color = Color.white;
	}

    public static void SetUptitle(String text, Color color, bool pulsating = true, bool fading = false)
    {
        if (THIS.Uptitle == null) return;
        THIS.uptitlePulsating = pulsating;

        Debug.Log("setting uptitlte: " + text);
        THIS.uptitleAlpha = 5f;
        THIS.uptitleFading = fading;
        THIS.Uptitle.GetComponent<GUIText>().text = text;
        THIS.Uptitle.GetComponent<GUIText>().color = color;
    }

	public static void AddUptitleToQueue(String text){
		THIS.UptitleQueue.Add(text);
	}

    public static void SetUptitleColor(Color color) {
        THIS.Uptitle.GetComponent<GUIText>().color = color;
    }

    public static void ClearSubtitleQueue() {
        THIS.subtitlesQueue.Clear();
        THIS.LoopSubtitles = false;
    }

    public static void AddSubtitleToQueue(string text) {
        THIS.subtitlesQueue.Add(text);
    }
	#endregion

    public static int SubtitleQueueLength()
    {
        return THIS.subtitlesQueue.Count;
    }

	#region Methods

	public static void Display(OverlayWord[] words, OnCompleteFunction onComplete = null){
		if (!THIS.AllowTextDisplay) return;
		THIS.OnComplete = onComplete;
		THIS.DisplayInternal (words);
	}

	public static void DisplayQuick(string word, float delay = 0.4f){
		string[] T = word.Split(new [] {'|', '\n'});
		TextManager.DisplayQuick(T, false, delay);
	}

	public static void DisplayQuickSilent(string word, float delay = 0.4f){
        string[] T = word.Split(new[] { '|', '\n' });
		TextManager.DisplayQuick(T, true, delay);
	}

	public static void DisplayQuickPulsating(string word){
		var lol = new List<OverlayWord>();

		OverlayWord W = new OverlayWord(word)
			.SetAnimationType (TextOverlayAnimationTypes.Pulsating, 100000f)
			.SetFont (TextOverlayFonts.RobotoRegular)
			.SetSizeScale(0.7f)
			.ParseCommands();
			
		W.MakeSilent();
			
		lol.Add(W); 
				
		TextManager.Display (lol.ToArray());
	}

	public static void DisplayQuickSilentSmaller(string word, float durationScale = 0.2f, bool blackFading = false){
        string[] T = word.Split(new[] { '|', '\n' });
		TextManager.DisplayQuickSmaller(T, true, durationScale);
	}

	public static void DisplayQuick(string[] words, bool silent = false, float delay = 0.4f){

		var lol = new List<OverlayWord>();

		foreach (string w in words){
			OverlayWord W = new OverlayWord(w)
				.SetAnimationType (TextOverlayAnimationTypes.NoEffect, delay * 2f)
				.SetFont (TextOverlayFonts.RobotoRegular)
				.SetSizeScale(0.7f)
				.SetAudio(defaultWordSound)
				.ParseCommands();

			if (silent) W.MakeSilent();

			lol.Add(W); 
		}

		if(lol.Count>1)
			lol [lol.Count-1].SetAudio(defaultLastWordSound);

		TextManager.Display (lol.ToArray());
	}

	public static void DisplayQuickSmaller(string[] words, bool silent = false, float durationScale = 0.2f, bool blackFading = false){
		var lol = new List<OverlayWord>();
		
		foreach (string w in words) {
		    OverlayWord W = new OverlayWord(w)
				.SetAnimationType(TextOverlayAnimationTypes.NoEffect, 1f)
		        .SetFont(TextOverlayFonts.RobotoRegular)
		        .SetSizeScale(0.7f)
		        .SetDurationScale(durationScale)
		        .SetBlackFading(blackFading)
				.ParseCommands();
			
			if (silent) W.MakeSilent();
			
			lol.Add(W); 
		}
		
		TextManager.Display (lol.ToArray());
	}

	public void DisplayInternal(OverlayWord[] words)
	{
	    _killCurrentTextAnimationOnNextUpdate = true;

        for (int i = 0; i < words.Length; i++)
        {
            wordStack.Add(words[i]);
		}
	}

	public void Clear(){
        bool wasActive = Active;
        Active = true;
		wordStack.Clear();
		if (LastTextAnimation != null)
		{
			LastTextAnimation.RemoveTweens();
			LastTextAnimation.Kill();
		}
        ClearSubtitleQueue();
        SetUptitle("");
        SetSubtitle("");
        Active = wasActive;
        
        //CameraEffectsManager.Instance[TextAnimation.TextSciemPunchInterpolator].Stop();
		//CameraEffectsManager.Instance[TextAnimation.TextSciemPunchInterpolator].Reset();
	}

	public void ClearOnlyMainText(){
		wordStack.Clear ();
		if (LastTextAnimation != null)
		{
			LastTextAnimation.RemoveTweens();
			LastTextAnimation.Kill();
		}
	}

	public float GetQueueLength(){

		float time = 0;
		for (int i = 0; i < wordStack.Count; i++)
		{
			//Debug.Log(i + " : " + wordStack[i].txt);
			time += wordStack[i].durationScale * .5f;
		}
		//Debug.Log("wordstack: " + wordStack.Count +", duration: " + time);
		return time;	
	}

	public bool IsQueueEmpty(){
		return wordStack.Count == 0;
	}

	public bool IsQueueEmptyAndNoTextHanging(){
		return (wordStack.Count == 0 && LastTextAnimation == null);
	}

	public void ShowNextTextAnimation(){
		if (wordStack.Count > 0) {
			OverlayWord next = wordStack[0];
			wordStack.RemoveAt(0);
			//if (!SHGUI.current.IsBackgroundOn())
				DisplaySingleWord (next);
		} else {
			if (OnComplete != null){
				OnComplete();
			}
		}
	}

	private void DisplaySingleWord(OverlayWord word){
        GameObject wordObject;
        wordObject = GameObject.Instantiate(emptyTextAnimation) as GameObject;

        wordObject.transform.parent = TextContainer;
        wordObject.transform.localRotation = Quaternion.identity;
        wordObject.transform.localPosition = Vector3.zero;

        //Enum effect names are converted to strings and used to instantiate a proper class of effect
        string t1 = ((TextOverlayAnimationTypes)word.type).ToString();

		TextAnimation a;
		if (word.type == TextOverlayAnimationTypes.ScrollDown)
			a = wordObject.AddComponent<ScrollDown>();
		else if (word.type == TextOverlayAnimationTypes.ScrollLeft)
			a = wordObject.AddComponent<ScrollLeft>();
		else if (word.type == TextOverlayAnimationTypes.ScrollRight)
			a = wordObject.AddComponent<ScrollRight>();
		else if (word.type == TextOverlayAnimationTypes.ScrollUp)
			a = wordObject.AddComponent<ScrollUp>();
		else
			a = wordObject.AddComponent<NoEffect>();
		
        a.Setup(word);
        a.Launch();
	}

    // Manually load appropriate fonts
	// FIXME: find a way to load the fonts less manually
	private void LoadFonts() {
        // initialize the fonts array
        fonts = new Font[System.Enum.GetNames(typeof(TextOverlayFonts)).Length];

        // load fonts from resources
		fonts[(int)TextOverlayFonts.RobotoBlack] = Resources.Load("Fonts/Roboto-Black") as Font;
		fonts[(int)TextOverlayFonts.RobotoBold] = Resources.Load("Fonts/Roboto-Bold") as Font;
		fonts[(int)TextOverlayFonts.RobotoLight] = Resources.Load("Fonts/Roboto-Light") as Font;
		fonts[(int)TextOverlayFonts.RobotoMedium] = Resources.Load("Fonts/Roboto-Medium") as Font;
		fonts[(int)TextOverlayFonts.RobotoRegular] = Resources.Load("Fonts/Roboto-Regular") as Font;
		fonts[(int)TextOverlayFonts.RobotoThin] = Resources.Load("Fonts/Roboto-Thin") as Font;
		fonts[(int)TextOverlayFonts.ComicSans] = Resources.Load("Fonts/comicsans") as Font;
	}

	static float SUPERHOTbatchtime = 1f;
	public static void DisplaySUPERHOTBatch(float time = 1f){
		SUPERHOTbatchtime = time;
		DisplaySUPERHOTBatchInternal();
	}

    private static void DisplaySUPERHOTBatchInternal()
    {
        var lol = new List<OverlayWord>();

        //AudioClip SUPER = AudioManager.AudioClips["Sounds/Source/super2"];
        //AudioClip HOT = AudioManager.AudioClips["Sounds/Source/hot2"];

        for (int i = 0; i < 1; ++i)
        {
            lol.Add(new OverlayWord("SUPER")
			        .SetAnimationType(TextOverlayAnimationTypes.NoEffect, SUPERHOTbatchtime * 2)
                     .SetFont(TextOverlayFonts.RobotoThin)
                     //.SetAudio(SUPER)
                     .SetSizeScale(0.9f)
                     .SetColor(Color.white)
			         .SetBlackFading(false));
            lol.Add(new OverlayWord("HOT")
			        .SetAnimationType(TextOverlayAnimationTypes.NoEffect, SUPERHOTbatchtime * 2)
	                .SetFont(TextOverlayFonts.RobotoBold)
	                //.SetAudio(HOT)
	                .SetColor(Color.white)
				    .SetBlackFading(false));
        }

		TextManager.Display(lol.ToArray(), DisplaySUPERHOTBatchInternal);
    }
	#endregion
}


public enum TextScreenMode { Size, Stretch }

public enum TextOverlayFonts
{
    RobotoRegular,
    RobotoThin,
    RobotoLight,
    RobotoMedium,
    RobotoBold,
    RobotoBlack,
    ComicSans
};

[Serializable]
public class OverlayWord
{
    #region Variables
    public string txt;
    public float durationScale = 0.2f;
    public float sizeScale = 1f;
    public TextOverlayFonts font = TextOverlayFonts.RobotoRegular;
    public TextOverlayAnimationTypes type = TextOverlayAnimationTypes.NoEffect;
    public AudioClip audio = null;
    public bool silent = false;
    public Color color = Color.white;
    public bool doFading = true;
    public Color background = Color.clear;
    public Transform cameraToSwitch = null;
    #endregion

    #region Constructor
    public OverlayWord(string word)
    {
        txt = word;
        this.durationScale = 1f;
        this.sizeScale = 1f;
        this.font = TextOverlayFonts.RobotoThin;
        this.type = TextOverlayAnimationTypes.NoEffect;
        this.audio = null;
        this.silent = false;
        this.color = new Color(1.0f, 1.0f, 1.0f, TextManager.THIS.currentAlpha);
    }

    public OverlayWord SetFont(TextOverlayFonts font)
    {
        this.font = font;
        return this;
    }

    public OverlayWord SetText(string text)
    {
        this.txt = text;
        return this;
    }

    public OverlayWord SetAnimationType(TextOverlayAnimationTypes animationType, float durationScale)
    {
        this.type = animationType;
        this.durationScale = durationScale;
        return this;
    }

    public OverlayWord MakeSilent()
    {
        silent = true;
        return this;
    }

    public OverlayWord SetCameraToSwitch(Transform cam)
    {
        cameraToSwitch = cam;
        return this;
    }

    public OverlayWord SetAudio(AudioClip audio)
    {
        this.silent = false;
        this.audio = audio;
        return this;
    }

    public OverlayWord SetDurationScale(float scale)
    {
        this.durationScale = scale;
        return this;
    }

    public OverlayWord SetColor(Color color)
    {
        this.color = color;
        return this;
    }

    public OverlayWord SetBackground(Color color)
    {
        this.background = color;
        return this;
    }

    public OverlayWord SetSizeScale(float sizeScale)
    {
        this.sizeScale = sizeScale;

        return this;
    }

    public OverlayWord SetBlackFading(bool v)
    {
        this.doFading = v;

        return this;
    }

    public OverlayWord ParseCommands()
    {
        if (!txt.Contains(">")) return this;
        else
        {
            int cmdIndex = txt.IndexOf('>') + 1;
            string commands = txt.Substring(0, cmdIndex);

            float baseDurationScale = durationScale;
            int currentChar = 0;
            while (currentChar < commands.Length)
            {
                if (commands[currentChar] == 'D')
                {
                    durationScale += baseDurationScale;
                }
                if (commands[currentChar] == 'd')
                {
                    durationScale *= .75f;
                }
                if (commands[currentChar] == '^')
                {
                    sizeScale *= 1.2f;
                }
                if (commands[currentChar] == 'v')
                {
                    sizeScale *= 0.7f;
                }

                if (commands[currentChar] == 'C')
                {
                    currentChar++;
                    if (commands[currentChar] == 'r')
                    {
                        SetColor(Color.red);
                    }
                    else if (commands[currentChar] == 'w')
                    {
                        SetColor(Color.white);
                    }
                    else if (commands[currentChar] == 'x')
                    {
                        SetColor(Color.black);
                    }
                    else if (commands[currentChar] == 'z')
                    {
                        SetColor(Color.clear);
                    }
                    else
                        durationScale += baseDurationScale;
                }

                if (commands[currentChar] == 'c')
                {
                    currentChar++;
                    if (commands[currentChar] == '\'')
                    {
                        currentChar++;
                        StringBuilder targetName = new StringBuilder();
                        int failsafe = 30;
                        while (commands[currentChar] != '\'' && failsafe > 0)
                        {
                            targetName.Append(commands[currentChar]);
                            currentChar++;
                            failsafe--;
                        }

                        string targetNameString = targetName.ToString();
                        //Debug.Log("set camera to switch: " + targetNameString);
                        GameObject target = GameObject.Find(targetNameString);
                        cameraToSwitch = target.transform;
                    }
                }

                if (commands[currentChar] == '$')
                {
                    currentChar++;
                    if (commands[currentChar] == '0')
                    {
                        SetAnimationType(TextOverlayAnimationTypes.NoEffect, this.durationScale);
                    }
                    else if (commands[currentChar] == 'l')
                    {
                        SetAnimationType(TextOverlayAnimationTypes.ScrollLeft, this.durationScale);
                    }
                    else if (commands[currentChar] == 'r')
                    {
                        SetAnimationType(TextOverlayAnimationTypes.ScrollRight, this.durationScale);
                    }
                    else if (commands[currentChar] == 'u')
                    {
                        SetAnimationType(TextOverlayAnimationTypes.ScrollUp, this.durationScale);
                    }
                    else if (commands[currentChar] == 'd')
                    {
                        SetAnimationType(TextOverlayAnimationTypes.ScrollDown, this.durationScale);
                    }
                }

                if (commands[currentChar] == 'B')
                {
                    currentChar++;

                    if (commands[currentChar] == 'r')
                    {
                        SetBackground(Color.red);
                    }
                    else if (commands[currentChar] == 'w')
                    {
                        SetBackground(Color.white);
                    }
                    else if (commands[currentChar] == 'x')
                    {
                        SetBackground(Color.black);
                    }
                }

                if (commands[currentChar] == 'S')
                {
                    silent = !silent;
                }

                currentChar++;
            }

            txt = txt.Substring(cmdIndex);
            return this;
        }
    }
    #endregion
}