using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GlobalShaderValueControl : MonoBehaviour
{
	
    public static GlobalShaderValueControl Instance;

	public Texture				GlobalConcreteGradientMap;
	public Texture				GlobalSkyboxGradientMap;

	public Color				GlobalConcreteAlbedo;

	//public Texture				GlobalScreenSpaceLines;


	public Texture				GlobalStripesTex;
	public Texture				GlobalHotlineGradientMap;

	public Texture				GlobalCloudMap;
	public float				GlobalLoadingProgress;

	public float				GlobalDynamicLightsOnOff;

	public float				GlobalConcreteStripesPower;
	public float				GlobalCrystalStripesPower;

	public float				GlobalSkyboxStripesPower;
    public Texture              GlobalGoldGradientMap;

    public Texture              GlobalConcreteColorTexture;

    public Texture              GlobalCodeTexture;
    public float GlobalVertexScale;

	public bool GlobalFog = true;

    public RenderTexture MenuTexture;
    public RenderTexture GlobalMonitorTexture;

    public Material GoldenBulletMaterial;

	public float globalRegularPassPower = 1.0f;
	public float globalEightiesPassPower = 0.0f;
	public float globalRedvisionPassPower = 0.0f;
	public float globalHotlinePassPower = 0.0f;
	public float globalPrototypePassPower = 0.0f;



    private const float DefaultScreenRatio = 1.77777777778f; //do not change.


    void Awake()
    {
        Instance = this;
        GlobalMonitorTexture = new RenderTexture(256, 256, 0, RenderTextureFormat.Default);

        if (QualitySettings.GetQualityLevel() > 0 || IsMenu()) {
            MenuTexture = new RenderTexture(1920, 1080, 0, RenderTextureFormat.ARGB32);
            FindObjectOfType<CanvasScaler>().scaleFactor = 1.0f;
        } else {
            FindObjectOfType<CanvasScaler>().scaleFactor = 0.5f;
            MenuTexture = new RenderTexture(960, 540, 0, RenderTextureFormat.ARGB32);
        }

        if (QualitySettings.GetQualityLevel() > 0)
            Shader.EnableKeyword("FANCY_MARCIN");




    }

    public static bool IsMenu() {
        var segOmni = GameObject.Find("_SegwayOmni");
        return segOmni != null;
    }

    // Use this for initialization
	void Start () {
		//nadpisanie global dynamic lights on off
		string qualityLevel = QualitySettings.names.GetValue(QualitySettings.GetQualityLevel()).ToString();
		GlobalDynamicLightsOnOff = System.Convert.ToSingle(qualityLevel != "Low");

        
	}
	

	void updateAllTheTime(){

        float ratioRatio = DefaultScreenRatio / ((float)Screen.width / (float)Screen.height);
        Shader.SetGlobalFloat("_DefaultScreenRatioToCurrentScreenRatioRatio", ratioRatio);
        Shader.SetGlobalFloat("_DefaultScreenRatio", DefaultScreenRatio);
        Shader.SetGlobalFloat("_CurrentScreenRatio", ((float)Screen.width / (float)Screen.height));
        Shader.SetGlobalFloat("_GuiXOffset", (((float)Screen.width - ((float)Screen.height * DefaultScreenRatio)) / 2.0f / ratioRatio) / (float)Screen.width);
        Shader.SetGlobalFloat("_GuiYOffset", (((float)Screen.height - ((float)Screen.width / DefaultScreenRatio)) / 2.0f * ratioRatio) / (float)Screen.height);

		Shader.SetGlobalFloat("_Random", Random.Range(0.0f, 1.0f));
		Shader.SetGlobalFloat("_GlobalUnscaledTime", Time.unscaledTime);
		Shader.SetGlobalFloat("_GlobalScaledTime", Time.time);
		Shader.SetGlobalFloat("_GlobalTimeScale", Time.timeScale);

		//GlobalLoadingProgress=Mathf.Abs(Mathf.Sin(Time.unscaledTime));




        float GlobalIsEditor = 0.0f;
        if (!Application.isPlaying) {
            GlobalIsEditor = 1.0f;
        }
      
        //flaga is editor
        Shader.SetGlobalFloat("_GlobalIsEditor", GlobalIsEditor);
	}

	void updateSometimes(){

		Shader.SetGlobalTexture("_GlobalConcreteGradientMap", GlobalConcreteGradientMap);
		Shader.SetGlobalTexture("_GlobalHotlineGradientMap", GlobalHotlineGradientMap);


		Shader.SetGlobalColor("_GlobalConcreteAlbedo", GlobalConcreteAlbedo);
		Shader.SetGlobalTexture("_GlobalSkyboxGradientMap", GlobalSkyboxGradientMap);
		Shader.SetGlobalTexture("_GlobalCloudMap", GlobalCloudMap);
		
		Shader.SetGlobalFloat("_GlobalLoadingProgress", GlobalLoadingProgress);
		
		
		Shader.SetGlobalFloat("_GlobalScreenWidth", Screen.width);
		Shader.SetGlobalFloat("_GlobalScreenHeight", Screen.height);
		
		//Shader.SetGlobalFloat("_GlobalDynamicLightsOnOff", GlobalDynamicLightsOnOff);
		Shader.SetGlobalFloat("_GlobalConcreteStripesPower", GlobalConcreteStripesPower);
		Shader.SetGlobalFloat("_GlobalCrystalStripesPower", GlobalCrystalStripesPower);
		
		
		Shader.SetGlobalFloat("_GlobalSkyboxStripesPower", GlobalSkyboxStripesPower);
		
		Shader.SetGlobalTexture("_GlobalStripesTex", GlobalStripesTex);
        Shader.SetGlobalTexture("_GlobalGoldGradientMap", GlobalGoldGradientMap);
        Shader.SetGlobalTexture("_GlobalMonitorTexture", GlobalMonitorTexture);
        Shader.SetGlobalTexture("_GlobalConcreteColorTexture", GlobalConcreteColorTexture);

        Shader.SetGlobalTexture("_GlobalCodeTexture", GlobalCodeTexture);
       

        Shader.SetGlobalFloat("_GlobalVertexScale", GlobalVertexScale);



		Shader.SetGlobalFloat("_GlobalRegularPassPower", globalRegularPassPower);
		Shader.SetGlobalFloat("_GlobalEightiesPassPower", globalEightiesPassPower);
        Shader.SetGlobalFloat("_GlobalRedvisionPassPower", globalRedvisionPassPower);
		Shader.SetGlobalFloat("_GlobalHotlinePassPower", globalHotlinePassPower);
		Shader.SetGlobalFloat("_GlobalPrototypePassPower", globalPrototypePassPower);




		RenderSettings.fog = GlobalFog;

	}




	// Update is called once per frame
	void Update () {
		
		updateAllTheTime();
		updateSometimes();

		
	}













}
