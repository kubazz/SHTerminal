
using System;
using System.Collections.Generic;
using System.Text;

public enum SHAlign { Left, Right, Center }

public class SHGUItext: SHGUIview
{
	public string text;

	public int longestLineAfterSmartBreak = 0;

	public char backColor = ' ';
	
	public SHGUItext (String Text, int X, int Y, char Col)
	{
		Init ();

		text = Text;
		x = X;
		y = Y;

		SetColor (Col);
	}
	
	public override void Redraw (int offx, int offy)
	{
		if (hidden)
			return;

		SHGUI.current.DrawText (text, x + offx, y + offy, color, fade, backColor);
		base.Redraw (offx, offy);

	}

	public SHGUItext SetBackColor(char c){
		backColor = c;
		return this;
	}

	public SHGUItext GoFromRight(){
		x -= GetLineLength () - 1;
		return this;
	}

	public SHGUItext BreakTextForLineLength(int lenght){
		int curr = 0;
		for (int i = 0; i < text.Length; ++i) {
			curr++;

			if (text[i] == '\n'){
				curr = 0;
			}

			if (curr > lenght){
				text = text.Insert(i, "\n");
				i++;
				curr = 1;
			}
		}

		return this;
	}

    public SHGUItext CenterTextForLineLength(int length) {
        string newText = "";
        int curr = 0;

        string line = "";
        for (int i = 0; i < text.Length; ++i)
        {
            curr++;
            line += text[i];

            if (text[i] == '\n') {
                int offset = (length - line.Length)/2;
                for (int j = 0; j < offset; j++) {
                    newText += " ";
                }
                newText += line;
                line = "";

                curr = 0;
            }
        }

        text = newText;

        return this;
    }

	public SHGUItext SmartBreak(int lenght, int maxoffset = 7){
		int curr = 0;
		int off = 0;
		longestLineAfterSmartBreak = 0;

		for (int i = 0; i < text.Length; ++i) {
			curr++;
			
			if (text[i] == '\n'){
				curr = 0;
			}
			
			if (curr > lenght){
				if (text[i] == ' ' || off >= maxoffset){

					if (text[i] == ' ') {
						text = text.Insert(i + 1, "\n");
						text = text.Remove(i, 1);
					}
					else
						text = text.Insert(i, "\n");
					
					i++;
					curr = 1;
					off = 0;
				}
				else{
					off++;
				}
			}

			if(curr > longestLineAfterSmartBreak)
				longestLineAfterSmartBreak = curr;
		}
		
		return this;
	}

	public int GetLineLength(){
		int i = 0;
		for (i = 0; i < text.Length; ++i) {
			
			if (text[i] == '\n'){
				return i - 1;
			}
		}
		
		return i - 1;
	}

    public int GetLongestLineLength() {
        int longestLineLength = 0;
        int currentLineLength = 0;
        for (int i = 0; i < text.Length; ++i) {
            
            if (text[i] == '\n' || i == text.Length - 1) {
                if (currentLineLength > longestLineLength)
                    longestLineLength = currentLineLength;
                currentLineLength = 0;
                continue;
            }
            currentLineLength++;
        }
        return longestLineLength;
    }

	public SHGUItext CutTextForLineLength(int lenght){
		StringBuilder str = new StringBuilder();
		int curr = 1;
		for (int i = 0; i < text.Length; ++i) {
			if (text[i] == '\n'){
				curr = 0;
			}

			if (curr <= lenght){
				str.Append(text[i]);
			}
			else{
				
			}
			curr++;
		}
		text = str.ToString();
		
		return this;
	}

	public SHGUItext CutTextForMaxLines(int lines){
		int curr = 0;
		int chars = 0;
		for (int i = 0; i < text.Length; ++i) {
			if (text[i] == '\n'){
				curr++;
			}

			if (curr > lines){
				text = text.Substring(0, i);
				return this;
			}
		}
		
		return this;
	}

	public SHGUItext BreakCut(int lenght, int height){
		return BreakTextForLineLength (lenght).CutTextForMaxLines (height);
	}

	public int CountLines(){
		int lines = 1;
		for (int i = 0; i < text.Length; ++i) {
			if (text[i] == '\n'){
				lines++;
			}
		}
		return lines;
	}
	
}


