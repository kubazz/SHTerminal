using Assets.Scripts.Utilities;
using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using System.Collections.Generic;
using Utilities.CameraEffects;

public class TextAnimation : MonoBehaviour {

	TextManager	Manager;
	protected float durationScale;
	public bool doTextFading = true;

    public static string TextInterpolator = "TextPunch";
    public static string TextSciemPunchInterpolator = "SciemPunch";
	public static string TextClickThroughSegwayInInterpolator = "";

	void Start () {
	
	}

	public float colorFade = 0f;
	protected List<GameObject> textGameObjects;

	protected OverlayWord word;
	public void Setup(OverlayWord word){
		this.word = word;
		this.doTextFading = word.doFading;
		durationScale = word.durationScale;

		textGameObjects = new List<GameObject> ();

		float marginFractionX;
		float marginFractionY;

		string[] words = word.txt.Split (';');

		//single words behave a bit differently. we don't want them to fill all the screen (by just a bit).
		//multiple words need to be scaled upwards.
		//also: don't fill the whole screen with a word. Always leave some margins.
		if (words.Length == 1)
		{
		    marginFractionX = 0.9f;
		    marginFractionY = marginFractionX * 1.5f;
		}  
        else
		{
		    marginFractionX = 0.9f;
			marginFractionY = marginFractionX * 1.4f;	
		}

		marginFractionX *= word.sizeScale;
		marginFractionY *= word.sizeScale;

		foreach (string w in words) {
				int forcefont = -1;
				word.txt = w;
				float sizeScaleAdditional = 1f;

				if (w.Length > 0) {
						if (w [0] == '\\') {
								word.txt = w.Substring (1);
								forcefont = (int)TextOverlayFonts.RobotoThin;
						} else if (w [0] == '/') {
								word.txt = w.Substring (1);
								forcefont = (int)TextOverlayFonts.RobotoBold;
						}
						else if (w [0] == '#') {
							word.txt = w.Substring (1);
							sizeScaleAdditional = 2f;
						}
			}

                textGameObjects.Add(AddTextObject(word, (int)(TextManager.THIS.ScreenSize.x * marginFractionX * sizeScaleAdditional), (int)(TextManager.THIS.ScreenSize.y * marginFractionY * sizeScaleAdditional), forcefont));
		}


		//magical parameter that represents distance between anchor of a guiText and upper/lower edge of a glyph. 
		//I figured it should be 0.5. It isn;t
		float magic = 0.35f;


		//laying out the texts one below the other.
		float lasty = 0;
		float heightsum = 0;
		float maxwidth = 0;
		for (int i = 0; i < textGameObjects.Count; ++i)
		{
		    var screenRect = textGameObjects[i].GetComponent<GUIText>().GetScreenRect();

            heightsum += screenRect.height;
            if (screenRect.width > maxwidth)
                maxwidth = screenRect.width;

			if (i != 0) {
                lasty -= (textGameObjects[i - 1].GetComponent<GUIText>().GetScreenRect().height / TextManager.THIS.ScreenSize.y * magic);
                lasty -= (screenRect.height / TextManager.THIS.ScreenSize.y * magic);
			}
			textGameObjects[i].transform.localPosition = new Vector3(textGameObjects[i].transform.localPosition.x,
			                                           lasty,
			                                           textGameObjects[i].transform.localPosition.z);


		}

		//scaling so text fill the desired space
        float targetScaleWidth = (TextManager.THIS.ScreenSize.x * marginFractionX) / (maxwidth);
        float targetScaleHeight = (TextManager.THIS.ScreenSize.y * marginFractionY) / (heightsum);

		float targetScale = Mathf.Min (targetScaleHeight, targetScaleWidth);
		if (!float.IsNaN (targetScale) && !float.IsInfinity (targetScale)) {
			ScaleTextGameObjects (textGameObjects, targetScale);
			heightsum *= targetScale;
		}

		float off = 0;

		//centering.
        off = -textGameObjects[0].GetComponent<GUIText>().GetScreenRect().height / TextManager.THIS.ScreenSize.y * magic;
        off += heightsum / TextManager.THIS.ScreenSize.y * magic;

		//anchor of a first text is not at the top of a text but in its' center. we offset for that and additional bit.
		this.transform.localPosition = new Vector3 (this.transform.localPosition.x,
													    this.transform.localPosition.y + off + 0.02f,
													    this.transform.localPosition.z);



		/*
		float maxw = 0;
		float totalh = 0;
		Debug.Log ("----------------------");
		for (int i = 0; i < g.Count; ++i) {
			GUIText z = g[i].GetComponent<GUIText>();
			Debug.Log (z.text + ": " + z.GetScreenRect().width +", " + z.GetScreenRect().height);
			if (z.GetScreenRect().width > maxw) maxw = z.GetScreenRect().width;
			totalh += z.GetScreenRect().height;
			Debug.Log (" line spacing " + z.lineSpacing);
		}
		Debug.Log (" screen.width: " + Screen.width + " screen.height: " + Screen.height);
		Debug.Log (" max.width: " + maxw + " total.height: " + totalh);
		*/
		Manager = TextManager.THIS;		
	}

	bool restoreOldPositionAndRotation = false;
	Vector3 oldPosition;
	Quaternion oldRotation;
	Transform cameraParent;

	protected void TrySetBackground(Color color){;
		GameObject cam = GameObject.Find("TextCamera");

		if (cam != null) cam.GetComponent<Camera>().backgroundColor = color;
	}

	protected void ScaleTextGameObjects(List<GameObject> g, float scale){
		for (int i = 0; i < g.Count; ++i) {
			g[i].transform.localPosition = new Vector3(g[i].transform.localPosition.x,
			                                           g[i].transform.localPosition.y * scale,
			                                           g[i].transform.localPosition.z);

			g[i].GetComponent<GUIText>().fontSize = (int)(g[i].GetComponent<GUIText>().fontSize * scale);
		}
		
	}

	protected GameObject AddTextObject(OverlayWord word, int availableWidth, int availableHeight, int forceFont = -1){

		GameObject go = new GameObject ();
		go.transform.parent = this.transform;
		go.name = "Text";
		go.AddComponent<GUIText> ().alignment = TextAlignment.Center;
		go.GetComponent<GUIText> ().anchor = TextAnchor.MiddleCenter;
		go.layer = LayerMask.NameToLayer ("Text");
		go.AddComponent<MeshRenderer> ().material = TextManager.fonts [(int)word.font].material;
		go.transform.localPosition = new Vector3 (0, 0f, 0f);

		GUIText t = go.GetComponent<GUIText>();

        t.color = new Color(word.color.r, word.color.g, word.color.b, TextManager.THIS.currentAlpha);
		t.text = word.txt;

		Font font;
		font = TextManager.fonts [(int)word.font];

		if (forceFont != -1) font = TextManager.fonts [forceFont];
			

		t.text = word.txt.ToUpper ();
		t.font = font;
		t.GetComponent<Renderer>().material = font.material;

		t.fontSize = GetFittingFontSize (t.text, word.font, (int)(availableWidth), (int)(availableHeight));
	
		//additional adjustment of a font size - so it's actual height matches the available width or height
		if (!string.IsNullOrEmpty (t.text))
		{
		    var screenRect = t.GetScreenRect();
            if ((screenRect.width / availableWidth) > (screenRect.height / availableHeight))
                t.fontSize = (int)(t.fontSize * availableWidth / screenRect.width);
			else
                t.fontSize = (int)(t.fontSize * availableHeight / screenRect.height);
		}

		return go;
	}

	//TODO: do for other font sizes too!
	protected int GetFittingFontSize(string text, TextOverlayFonts font, int availableWidth, int availableHeight){
		if (font == TextOverlayFonts.RobotoBold) {
			return (int)((availableWidth * 1.3f)/ text.Length);
		}
		else
			return (int)((availableWidth * 1.3f)/ text.Length);
	}

	public void Launch(){

		StartAnimation ();
	}

	// Update is called once per frame
	void Update () {

		float f = doTextFading?colorFade:1f;
		if (colorFade < .5f) f = 0.90f;
		else f = 1f;

		if (!doTextFading) f = 1f;
		//f = 1f;
		foreach (var T in textGameObjects){
			T.GetComponent<GUIText>().color = new Color(word.color.r * f*1.2f, word.color.g * f, word.color.b * f, TextManager.THIS.currentAlpha);
		}	
	}

	protected virtual void StartAnimation(){

		DefaultStart();
	    //DelayedInvokeMarshal.Instance.Enqueue(FinishAnimation, 0.5f*durationScale);
        ////HACK: does nothing except calling OnComplete;
        HOTween.To(this.transform, .5f * durationScale, new TweenParms()
                         .Prop("localPosition", this.transform.localPosition)
                         .UpdateType(UpdateType.TimeScaleIndependentUpdate)
                         .Ease(EaseType.Linear)
                         .OnComplete(FinishAnimation)
                         .AutoKill(true));

	}

	protected void DefaultStart(){
		Manager.LastTextAnimation = this;

		PlaySound ();
		//TrySetBackground(word.background);
		//GlowHitter.HitSoftly ();
		
		if (!word.silent) {
			if (CameraEffectsManager.Instance != null) {
				if (CameraEffectsManager.Instance[TextSciemPunchInterpolator] && 
				    (CameraEffectsManager.Instance[TextClickThroughSegwayInInterpolator] == null || CameraEffectsManager.Instance[TextClickThroughSegwayInInterpolator].State != InterpolatorState.Playing))
                {
                    float newEffectTimescale = 1.5f;

                    newEffectTimescale *= 0.4f / (word.durationScale * 0.5f);

					if(newEffectTimescale > 1.5f)
						newEffectTimescale = 1.5f;

                    //Debug.Log("new timescale: " + newEffectTimescale);
                    //CameraEffectsManager.Instance[TextSciemPunchInterpolator].DefaultTimeScale = newEffectTimescale;
                   // CameraEffectsManager.Instance[TextSciemPunchInterpolator].Play();
                }

                //if (CameraEffectsManager.Instance[TextInterpolator])
                //{
                //    CameraEffectsManager.Instance[TextInterpolator].Play();
                //}
                //if (CameraEffectsManager.Instance["DarkHitEffectReplayCam"])
                //{
                //    CameraEffectsManager.Instance["DarkHitEffectReplayCam"].Play();
                //}
			}
		}
		
		/*
		GameObject cam = GameObject.Find("WeaponCamera");
		cam.GetComponent<Blur>().enabled = true;
		*/
		
		HOTween.To (this, .5f * durationScale / 4f, new TweenParms ()
		            .Prop ("colorFade", 1f)
		            .UpdateType (UpdateType.TimeScaleIndependentUpdate)
		            .Ease (EaseType.Linear)
		            .AutoKill(true));
	}

	protected virtual void FinishAnimation(){


		/*
		GameObject cam = GameObject.Find("WeaponCamera");
		cam.GetComponent<Blur>().enabled = true;
		*/
		ShowNext ();
		Kill ();
	}


	protected virtual void PlaySound(){

	}

	protected void ShowNext(){
		Manager.ShowNextTextAnimation ();
	}

	public void Kill(){
		GameObject.Destroy (this.gameObject);
	}

	public void RemoveTweens(){
		HOTween.Kill (this.transform);
	}
}
