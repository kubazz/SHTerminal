using UnityEngine;
using System.Collections;

public class SetMenuTexture : MonoBehaviour {
    public Component ComponentUsingMenuTexture;
	// Use this for initialization
	void Start () {
        if (ComponentUsingMenuTexture is Camera)
            (ComponentUsingMenuTexture as Camera).targetTexture = GlobalShaderValueControl.Instance.MenuTexture;
        else if (ComponentUsingMenuTexture is CustomMaterialOnScreen)
            (ComponentUsingMenuTexture as CustomMaterialOnScreen).GetMaterialProperty("_MenuTex").SetValue(GlobalShaderValueControl.Instance.MenuTexture);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
