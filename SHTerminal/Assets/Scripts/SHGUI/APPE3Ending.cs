using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class APPE3Ending : SHGUIview
{

    private List<SHGUIprompter> prompters;
    private List<string> queue;
    private float endingDelay = 1f;

    public APPE3Ending(string content)
    {
        prompters = new List<SHGUIprompter>();
        queue = new List<string>();

        string[] lines = content.Split('|');

        foreach (string str in lines)
        {
            AddToQueue(str, false);
        }

        allowCursorDraw = false;

    }

    void AddToQueue(string line, bool spacing = true)
    {
        queue.Add(line);
        if (spacing) queue.Add("");
    }

    void AddPrompter(string line)
    {
        SHGUIprompter prom = new SHGUIprompter(0, 0, 'w');
        prom.SetInput(line);
        prom.baseCharDelay *= 1.25f;
        prom.maxLineLength = 60;

        AddSubView(prom);

        prompters.Add(prom);
    }

    public override void Update()
    {
        if (prompters.Count == 0 || (prompters[prompters.Count - 1].IsFinished() && queue.Count > 0))
        {
            AddPrompter(queue[0]);
            queue.RemoveAt(0);
        }

        for (int i = 0; i < prompters.Count; ++i)
        {
            prompters[i].x = (int)(SHGUI.current.resolutionX / 2) - (int)(prompters[i].GetLineLength() / 2);
            prompters[i].y = (int)(SHGUI.current.resolutionY / 2) - (int)(prompters.Count / 2) + i - 1;
        }

        if (prompters[prompters.Count - 1].IsFinished() && queue.Count == 0)
        {
            endingDelay -= Time.unscaledDeltaTime;
            if (endingDelay < 0)
            {
                //Kill();
            }
        }

        base.Update();
    }

}
