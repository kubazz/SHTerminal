using UnityEngine;
using System.Collections;
using System.Text;

public class StringScrambler : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	public float fade = 0;
	bool clicker = false;
	public static int seed = 100;
	public static string filler = "^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^";
	public static string longfiller = "^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^";
	// Update is called once per frame
	void Update () {
		
	}
	
	public static string simpleScramble = "▀▄ █ ▌ ░ ▒ ▓ ■▪";
	public static string GetScrambledSimply(string text){
		StringBuilder outputBuilder = new StringBuilder(text.Length);
		foreach (char c in text) {
			if (c=='%') {
				outputBuilder.Append(GetScrambledSimply(filler));
			}
			if (c=='|') {
				outputBuilder.Append("\n");
			}
			else if (c=='^') {
				outputBuilder.Append(simpleScramble[Random.Range(0,simpleScramble.Length)]);
			}
			else outputBuilder.Append(c);
		}
		
		return outputBuilder.ToString();
	}
	
	public static char GetGlitchChar(){
		return simpleScramble [Random.Range (0, simpleScramble.Length)];
	}

    public static string GetGlitchString(int length) {
        StringBuilder newtext = new StringBuilder();
        for (int i = 0; i < length; i++) {
            newtext.Append(GetGlitchChar());
        }
        return newtext.ToString();
    }
	
	public static string GetScrambledString(string text, float amount, string substitute = "▀ ▄ █ ▌ ▐░ ▒ ▓ ■▪")
	{
		StringBuilder newtext = new StringBuilder();
		
		for (int i = 0; i < text.Length; ++i){
			Random.seed++;
			
			float v = Random.value;
			char character = text[i];
			if (v < amount && character != ' ' && character != '/' && character != '\n')
			{
				newtext.Append(substitute[(int)(Random.Range(0, substitute.Length))]);
			}
			else{
				newtext.Append(text[i]);
			}
		}
		
		return newtext.ToString();
		
	}
}
