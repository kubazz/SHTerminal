using LuaInterface;
using UnityEngine;

public class SHGUIluasandbox
{
    private Lua luaVirtualMachine;
    SHGUIview parent;

    public SHGUIluasandbox(SHGUIview parent, string script)
    {
        this.parent = parent;

        luaVirtualMachine = new Lua();
        luaVirtualMachine["SHGUI"] = this;
        luaVirtualMachine.DoString(script);

        boundRedrawFunction = luaVirtualMachine.GetFunction("Redraw");
        boundInitFunction = luaVirtualMachine.GetFunction("Init");
        boundUpdateFunction = luaVirtualMachine.GetFunction("Update");
        boundInputFunction = luaVirtualMachine.GetFunction("Input");
    }

    public LuaFunction boundInitFunction;
    public LuaFunction boundUpdateFunction;
    public LuaFunction boundRedrawFunction;
    public LuaFunction boundInputFunction;

    public void DestroyLua()
    {
        if (luaVirtualMachine != null)
        {
            luaVirtualMachine.Dispose();
            luaVirtualMachine = null;

            boundInitFunction = null;
            boundUpdateFunction = null;
            boundRedrawFunction = null;
            boundInputFunction = null;
        }
    }

    //FUNCTIONS

    public void Log(string msg)
    {
        SHGUItempview t = new SHGUItempview(1f);
        t.PunchIn(1f);
        t.overrideFadeOutSpeed = 100000f;
        t.AddSubView(new SHGUItext(msg, 0, 0, 'z'));
        parent.AddSubView(t);
    }

    public void SetPixel(string c, int x, int y, string col)
    {
        SHGUI.current.SetPixelFront(c[0], x, y, col[0]);
    }

    public void Kill()
    {
        parent.Kill();
    }
}


