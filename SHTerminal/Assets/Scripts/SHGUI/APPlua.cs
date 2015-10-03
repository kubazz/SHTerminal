
using LuaInterface;
using System;
using System.Collections.Generic;
using UnityEngine;


public class APPlua: SHGUIview
{
    SHGUIluasandbox lua;

    public APPlua(string luaFileName)
	{
        var script = Resources.Load("LUA/" + luaFileName);
        string script2 = "";
        if (script != null) script2 = script.ToString();

        if (!String.IsNullOrEmpty(script2))
            lua = new SHGUIluasandbox(this, script2);
        else
            Kill();

        allowCursorDraw = false;
    }

    public override void Update()
    {
        base.Update();

        if (lua != null && lua.boundUpdateFunction != null)
        {
            lua.boundUpdateFunction.Call();
        }
    }

    public override void Redraw(int offx, int offy)
    {
        base.Redraw(offx, offy);

        if (lua != null && lua.boundRedrawFunction != null)
        {
            lua.boundRedrawFunction.Call();
        }
    }

    public override void ReactToInputKeyboard(SHGUIinput key)
    {
        if (key == SHGUIinput.esc)
        {
            this.Kill();
        }
        base.ReactToInputKeyboard(key);

        int dirx = 0;
        if (key == SHGUIinput.left) dirx = -1;
        else if (key == SHGUIinput.right) dirx = 1;
        int diry = 0;
        if (key == SHGUIinput.up) diry = -1;
        else if (key == SHGUIinput.down) diry = 1;
        int enter = (key == SHGUIinput.enter)?(1):(0);

        if (lua != null && lua.boundInputFunction != null)
        {
            lua.boundInputFunction.Call(dirx, diry, enter);
        }
    }

    public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
    {
        base.ReactToInputMouse(x, y, clicked, scroll);
    }

    public override void Kill()
    {
        base.Kill();
    }

    public override void OnExit()
    {
        if (lua != null)
        {
            lua.DestroyLua();
        }
    }
}


