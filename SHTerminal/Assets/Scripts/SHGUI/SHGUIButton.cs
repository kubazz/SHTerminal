using System;
using UnityEngine;
using System.Collections;

public class SHGUIButton : SHGUItext {
    public Action OnActivate;
    public SHGUIinput ActionKey;
    private int xGlobal = 0;
    private int yGlobal = 0;
    public SHGUIButton(string Text, int X, int Y, char Col) 
        : base(Text, X, Y, Col) {
        
    }

    public override void Redraw(int offx, int offy)
    {
        if (hidden)
            return;

        xGlobal = offx + x;
        yGlobal = offy + y;

        SHGUI.current.DrawText(text, x + offx, y + offy, color, fade, backColor);
        base.Redraw(offx, offy);

    }

    public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
    {
        base.ReactToInputMouse(x,y,clicked,scroll);

        if (x >= xGlobal && x <= xGlobal + this.GetLongestLineLength()
            && y >= yGlobal && y < yGlobal + this.CountLines())
        {
                for (int X = xGlobal; X <= xGlobal + GetLongestLineLength(); X++)
                {
                    for (int Y = yGlobal; Y < yGlobal + this.CountLines(); Y++)
                    {
                    SHGUI.current.SetPixelBack('█', X, Y, 'r');
                        if(clicked && OnActivate != null)
                            OnActivate.Invoke();
                }
            }

            if (clicked && OnActivate != null)
                OnActivate.Invoke();
        } else {
            for (int X = xGlobal; X <= xGlobal + GetLongestLineLength(); X++)
            {
                for (int Y = yGlobal; Y < yGlobal + this.CountLines(); Y++)
                {
                    SHGUI.current.SetPixelBack(' ', X, Y, 'w');
                }
            }
        }
    }

    public override void ReactToInputKeyboard(SHGUIinput key) {
        base.ReactToInputKeyboard(key);
        if(key == ActionKey && OnActivate != null)
            OnActivate.Invoke();
    }

    public SHGUIButton SetActionKey(SHGUIinput key) {
        ActionKey = key;
        return this;
    }

    public SHGUIButton SetOnActivate(Action func) {
        OnActivate = func;
        return this;
    }
}
