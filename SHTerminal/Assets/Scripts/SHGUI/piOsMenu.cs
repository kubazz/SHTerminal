using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using System.Text;
using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;
using Random = UnityEngine.Random;

public class piOsMenu : MonoBehaviour {

	private const bool E3 = false;
	public bool AddToQueueMode = false;

	private bool started = false;

	public bool dontShowPlayLast = false;
	public bool dontShowExploit = true;

	//public bool isBarebones = false;

	private SHGUIcommanderview createdView;

	public int downloadProgress = int.MaxValue;
	int elementCounter = 0;
	bool displayedThisCounter = false;
	// Use this for initialization
	void Start () {

		//ForceStart ();
	}

	public SHGUIcommanderview ForceStart(bool addToQueue = false){
		if (started)
			return null;

		AddToQueueMode = addToQueue;
		started = true;

		CreateDirectoryStructure();


        UnityEngine.Cursor.lockState = CursorLockMode.Confined;

		return createdView;
	}

	public XDocument DATA;
    public XDocument GameData;
	private void CreateDirectoryStructure() {
		string data = Resources.Load (E3 ? "FolderStructureE3" : "FolderStructure").ToString ();
		DATA = XDocument.Parse(data);

        TextAsset xmlLevels = Resources.Load<TextAsset>("GameData");
        GameData = XDocument.Parse(xmlLevels.text);

		CreateViewFromNode ();
    }


	private void CreateViewFromNode(XElement e = null){
		bool isRoot = false;
		if (e == null) {
			isRoot = true;
			e = DATA.Root;
		}
		
		string currentPath = "";
		SHGUIcommanderview l = new SHGUIcommanderview ();
		createdView = l;
		l.isRoot = isRoot;
		SHGUIcommanderbutton b = null;
		string description = "";

		if (!isRoot) {
			b = new SHGUIcommanderbutton ("/..         │<UP-FOL>", 'w', () => {
				SHGUI.current.PopView ();}).SetListLink (l).SetData ("directory: GO UP");
			l.AddButtonView (b);
			SHGUIcommanderview L = SHGUI.current.GetInteractableView() as SHGUIcommanderview;
			if (L != null)
				l.path = L.path + e.Name.ToString().ToUpper() + "\\";
		}
		else{
			l.path = "C:\\";
			//AnalProbe.PushEnvironmentSurvey();
		}


		foreach (XElement E in e.Elements()) {
			XElement node = E;
			string name = "";

			if (!(node.Attribute("showInBarebones") != null && node.Attribute("showInBarebones").Value.ToString() == "true")){
				elementCounter++;
				displayedThisCounter = false;
			}
			else{
				
			}
			

			if (elementCounter >= downloadProgress){
				if (!(node.Attribute("showInBarebones") != null && node.Attribute("showInBarebones").Value.ToString() == "true")){
					continue;
				}
				else{
				}
			}

            if (E.Name.ToString() != "item"){
				name = E.Name.ToString().ToUpper();
				while (name.Length < 12){
					name += " ";
				}
				name = name + "│>FOLDER<";
				description = "directory: " + name;
				b = new SHGUIcommanderbutton (name, 'w', ()=> CreateViewFromNode(node)).SetListLink(l).SetData(description);
				l.AddButtonView (b);
			}
			else{
				string type = node.Attribute("type").Value.ToString();//.ToLower();
				
				name = node.Attribute("name").Value.ToString();
				while (name.Length < 12){
					name += " ";
				}
				
				if (type == "readme"){
					name = name + "│--ITEM->";
					description = StringScrambler.GetScrambledSimply(node.Value.ToString());
					b = new SHGUIcommanderbutton (name, 'w', null).SetListLink(l).SetData(description);//.SetOnActivate(()=>{SHGUI.current.ShowReadmeFile(node.Attribute("name").Value.ToString(),node.Value.ToString());});
				    if (node.Attribute("align") != null) {
				        if (node.Attribute("align").Value == "left") {
                            b.ContentAlign = SHAlign.Left;
                        } else if (node.Attribute("align").Value == "center") {
                            b.ContentAlign = SHAlign.Center;
                        } if (node.Attribute("align").Value == "right") {
                            b.ContentAlign = SHAlign.Right;
                        }
				    }
					l.AddButtonView (b);
				}
				else if (type == "separator"){
					name = "------------│--------";
					description = "";
					b = new SHGUIcommanderbutton (name, 'w', ()=> {}).SetListLink(l).SetData(description);
					l.AddButtonView (b);
				}
				///NOT USED
				else if (type == "level"){
					name = name + "│--ITEM->";
					description = "level: " + name + " " + Random.Range(0, 3000);
					b = new SHGUIcommanderbutton (name, 'w', null).SetListLink(l).SetData(description);
					l.AddButtonView (b);
				}
				else if (type == "userlevel"){
					name = name + "│--ITEM->";
					description = "userlevel: " + name + " " + Random.Range(0, 3000);
					b = new SHGUIcommanderbutton (name, 'w', ()=>{SHGUI.current.LaunchUserLevel(node.Attribute("bundle").Value.ToString(), node.Attribute("scene").Value.ToString());}).SetListLink(l).SetData("launches user level: " + node.Attribute("bundle").Value.ToString());
					l.AddButtonView (b);
				}
				else if (type == "mod"){
					name = name + "│--ITEM->";
					
					description = StringScrambler.GetScrambledSimply(StringScrambler.longfiller + StringScrambler.longfiller + "\n\n" + node.Value.ToString() + StringScrambler.longfiller + StringScrambler.longfiller);
					//description = node.Attribute("description").Value.ToString();
					b = new SHGUIcommanderbutton (name, 'w', null).SetListLink(l).SetData(description);
					l.AddButtonView (b);
				}
				else if (type == "app"){
					name = name + "│--ITEM->";
					description = "app: " + name + " " + Random.Range(0, 3000);
					b = new SHGUIcommanderbutton (name, 'w', ()=>{SHGUI.current.LaunchAppByName(node.Attribute("appclass").Value.ToString());}).SetListLink(l).SetData(description);
					l.AddButtonView (b);

					if (node.Attribute("appclass").Value.ToString() == "APPquit"){
						b.IsQuitButton = true;
					}
				}
				else if (type == "exploit" && !dontShowExploit){
					name = name + "│--ITEM->";
					description = "app: " + name + " " + Random.Range(0, 3000);
					b = new SHGUIcommanderbutton (name, 'w', ()=> SHGUI.current.LaunchAppByName("APPkill")).SetListLink(l).SetData(description);
					l.AddButtonView (b);
				}
                else if (type == "reboot")
                {
                    name = name + "│--ITEM->";
                    description = "app " + name + " " + Random.Range(0, 3000);
                    b = new SHGUIcommanderbutton(name, 'w',
                        () => {

                        }).SetListLink(l).SetData(description);
                    l.AddButtonView(b);
                }
				else if (type == "endless")
				{
					name = name + "│--ITEM->";
					description = "app " + name + " " + Random.Range(0, 3000);
					b = new SHGUIcommanderbutton(name, 'w',
					                             () => {

					}).SetListLink(l).SetData(description);
					l.AddButtonView(b);
				}
				else if (type == "last" && !dontShowPlayLast){

				}
				else if (type == "vid"){
					name = name + "│--ITEM->";
					description = "vid: " + name + " " + Random.Range(0, 3000);
					b = new SHGUIcommanderbutton (name, 'w', ()=>{SHGUI.current.ShowVideo(node.Attribute("vidname").Value.ToString());}).SetListLink(l).SetData(description);
					l.AddButtonView (b);
				}
				else if (type == "wir"){
					name = name + "│--ITEM->";
					description = "wir: " + name + " " + Random.Range(0, 3000);
					b = new SHGUIcommanderbutton (name, 'w', ()=>{SHGUI.current.ShowWiresSchem(node.Attribute("wirname").Value.ToString());}).SetListLink(l).SetData(description);
					l.AddButtonView (b);
				}
				else if (type == "art"){
					name = name + "│--ITEM->";
					description = "app: " + name + " " + Random.Range(0, 3000);
					b = new SHGUIcommanderbutton (name, 'w', ()=>{SHGUI.current.ShowArtFile(node.Attribute("name").Value.ToString(), node.Attribute("artname").Value.ToString(), false);}).SetListLink(l).SetData(description);
					l.AddButtonView (b);
				}
				else if (type == "artcent"){
					name = name + "│--ITEM->";
					description = "app: " + name + " " + Random.Range(0, 3000);
					b = new SHGUIcommanderbutton (name, 'w', ()=>{SHGUI.current.ShowArtFile(node.Attribute("name").Value.ToString(), node.Attribute("artname").Value.ToString(), true);}).SetListLink(l).SetData(description);
					l.AddButtonView (b);
				}
                else if (type == "storylevels")
                {
					AppendGameDataListFromNode(l, GameData.Descendants("Story").First());
                }
                else if (type == "endlesslevels")
                {
                    AppendGameDataListFromNode(l, GameData.Descendants("Endless").First());
                    CheckEndlessAvaliability();
                }
                else if (type == "modslist")
                {
					AppendGameDataListFromNode(l, GameData.Descendants("Mods").First());
                }
                if (type == "deactivate")
                {
                    name = name + "│--ITEM->";
                    description = StringScrambler.GetScrambledSimply(node.Value);
                    b = new SHGUIcommanderbutton(name, 'w', null).SetListLink(l).SetData(description).SetOnActivate(
                        () => {
                            var modsButtons = SHGUI.current.GetInteractableView().children.Where(
                                c =>
                                    (c is SHGUIcommanderbutton)  &&
                                    (c as SHGUIcommanderbutton).AssignedModifier != null);


                            b.data = node.Value;
                            b.highlighted = true;
                        }).SetOnHighlight(
                            () => {
                                var activeMods = SHGUI.current.GetInteractableView().children.Where(
                                c =>
                                    (c is SHGUIcommanderbutton) &&
                                    (c as SHGUIcommanderbutton).AssignedModifier != null &&
                                    (c as SHGUIcommanderbutton).Active);

                                foreach (SHGUIcommanderbutton mod in activeMods) {
                                    b.data += mod.Text.Substring(0, 12) + "\n";
                                }
                            }).SetOnDeHighlight(
                            () => {
                                b.data = node.Value;
                            });
                    l.AddButtonView(b);
                }
                if (type == "web")
                {
                    name = name + "│--LINK->";
                    description = StringScrambler.GetScrambledSimply(node.Value);
                    b = new SHGUIcommanderbutton(name, 'w', null).SetListLink(l).SetData(description);//.SetOnActivate(()=>{SHGUI.current.ShowReadmeFile(node.Attribute("name").Value.ToString(),node.Value.ToString());});
                    b.Url = node.Attribute("url").Value;
                    l.AddButtonView(b);
                }
                if (type == "settingInvert")
                {
                    name = name + "│>------<";
                    description = StringScrambler.GetScrambledSimply(node.Value);
                    b = new SHGUIcommanderbutton(name, 'w', null).SetListLink(l).SetData(description);

                    var b1 = b;
                    b.SetOnActivate(
                        () =>
                        {

							//AnalProbe.PushEnvironmentSurvey();
                        });
                    l.AddButtonView(b);
                }
                if (type == "settingReset")
                {
                    name = name + "│--ITEM->";
                    description = StringScrambler.GetScrambledSimply(node.Value);
                    b = new SHGUIcommanderbutton(name, 'w', () => SHGUI.current.LaunchAppByName("APPResetStory")).SetListLink(l).SetData(description);

                    l.AddButtonView(b);
                }
				if (type == "settingUnlock")
				{
					name = name + "│--ITEM->";
					description = StringScrambler.GetScrambledSimply(node.Value);
					b = new SHGUIcommanderbutton(name, 'w', () => SHGUI.current.LaunchAppByName("APPUnlockEverything")).SetListLink(l).SetData(description);
					
					l.AddButtonView(b);
				}
				if (type == "settingAttract")
				{
					name = name + "│>------<";
					description = StringScrambler.GetScrambledSimply(node.Value);
					b = new SHGUIcommanderbutton(name, 'w', null).SetListLink(l).SetData(description);
					
					var b1 = b;
					b.SetOnActivate(
						() =>
						{

					});

					l.AddButtonView(b);
				}

                if (b != null && b.data.Contains("[RANDOM_TIME_PUN]")) {
                    b.data = b.data.Replace("[RANDOM_TIME_PUN]", GetRandomTimepun());
                }
			}

			if (b != null){
				if (Mathf.Abs(downloadProgress - elementCounter) <= 20 && !displayedThisCounter){
					if ((node.Attribute("type") != null && node.Attribute("type").Value.ToString() == "separator") ||
					    (node.Attribute("showInBarebones") != null && node.Attribute("showInBarebones").Value.ToString() == "true")){
						displayedThisCounter = true;
					}
					else{
						displayedThisCounter = true;
						
						b.SetColorRecursive('r');
						b.constantScramble = true;
						b.constantScrambleSpeed =.01f;
						b.color = 'r';
					}
				}
				else{
				}
			}
		}
        if (l.buttons[l.currentButton].OnHighlight != null && AddToQueueMode == false)
            l.buttons[l.currentButton].OnHighlight.Invoke();

		if (!AddToQueueMode) {
			SHGUI.current.AddViewOnTop (l);
		} else {
			SHGUI.current.AddViewToQueue (l);
			AddToQueueMode = false;		
		}
	}

	private void AppendGameDataListFromNode(SHGUIcommanderview l, XElement baseNode = null){

		SHGUI.current.views.Add(l);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void PrepareLevelCommanderButtonForXElement(ref SHGUIcommanderbutton b, XElement element, SHGUIcommanderview l, string customName = "") {
        var btnName = customName == "" ? element.Attribute("name").Value : customName;
        while (btnName.Length < 12)
        {
            btnName += " ";
        }

        if (btnName.Length > 12)
            btnName = btnName.Substring(0, 12);

        btnName = btnName + "│--ITEM->";
        string description = PrepareLevelDescription(element.Value);
        XElement element1 = element;

        b = new SHGUIcommanderbutton(btnName, 'w', null).SetListLink(l).SetData(description);

        b.LevelToBeLoaded = element1.Attribute("scene").Value;
        var xAttribute = element1.Attribute("addObject");
        if (xAttribute != null) {
            b.AdditionalComponent = xAttribute.Value;
        }

        SHGUIcommanderbutton icommanderbutton = b;
    }


    string PrepareLevelDescription(string keywords) {
        string desc = StringScrambler.GetGlitchString(288*2); //32 columns x 9 rows

        var dictionary = keywords.Split(',').ToList();

        int iterations = 25;

        for (int i = 0; i < iterations; i++) {
            desc = desc.Insert(Random.Range(0, desc.Length - 1), dictionary[Random.Range(0, dictionary.Count)]);
        }
        return desc;
    }

    void FillLevelsList() {
        TextAsset xmlLevels = Resources.Load<TextAsset>("GameData");

        var levels = XDocument.Parse(xmlLevels.text);
        var LevelsNode = levels.Descendants("Story");
        var LevelsList = LevelsNode.Descendants("Level");
    }

    string GetRandomTimepun() {
        TextAsset gameData = Resources.Load<TextAsset>("GameData");

        var data = XDocument.Parse(gameData.text);
        var punsNode = data.Descendants("TimePuns");
        var punsList = punsNode.Descendants("pun");

        var xElements = punsList as XElement[] ?? punsList.ToArray();
        string pun = xElements[Random.Range(0, xElements.Count())].Value;

        pun = pun.Replace("|", "\n");

        return pun;
    }

    public void CheckEndlessAvaliability()
    {
        
    }

    public void LockUnfinishedLevels() {
        
    }

    public string GetLevelAfterIntermission(string intermissionLevelName)
    {
        
        return "";
    }

    public string GetLevelBeforeIntermission(string intermissionLevelName)
    {
        
        return "";
    }
}
