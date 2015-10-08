using UnityEngine;
using System;
using System.Collections.Generic;

public class APProar : SHGUIappbase
{
    AudioSource sound;
    AudioSource sound2;

    bool clicker = true;

    SHGUIview superView;
    SHGUIview hotView;


    List<float> history;

    public APProar()
        : base("")
	{
        Init();

        sound = SHGUI.current.gameObject.AddComponent<AudioSource>();
        sound.clip = Resources.Load("super2") as AudioClip;

        sound2 = SHGUI.current.gameObject.AddComponent<AudioSource>();
        sound2.clip = Resources.Load("hot2") as AudioClip;


        samples = new float[qSamples];

        history = new List<float>();


    }

    void AddSuperView()
    {
        return;

        if (superView != null) superView.Kill();
        if (hotView != null) hotView.Kill();

        superView = new SHGUItext(SHGUI.current.GetASCIIartByName("supersmall"), 8, 8, '0');
        superView.overrideFadeInSpeed = 2f;
        AddSubView(superView);
    }

    void AddHotView()
    {
        return;
        if (superView != null) superView.Kill();
        if (hotView != null) hotView.Kill();

        hotView = new SHGUItext(SHGUI.current.GetASCIIartByName("hotsmall"), 16, 8, '0');
        hotView.overrideFadeInSpeed = 2f;
        AddSubView(hotView);

    }

    public override void Update()
    {
        base.Update();

        history.Add(GetRMS(0));

        if (!FixedUpdater(.03f))
            return;
    }

    public override void Redraw(int offx, int offy)
    {
		base.Redraw (offx, offy);

        if (!clicker)
        {
            for (int i = 0; i < SHGUI.current.resolutionX; i += 1)
            {
                float div = i * 0.15f;
                if (div < .5f) div = .5f;
                //if (div > 2f) div = 2f;
                DrawColumn(31 + i, 11, (int)(0 + GetHistoryVolume(history.Count - (i)) / div));
                DrawColumn(33 - i, 11, (int)(0 + GetHistoryVolume(history.Count - (i)) / div));

            }
        }
        else
        {
            for (int i = 0; i < SHGUI.current.resolutionY; i += 1)
            {
                float div = i * 0.25f;
                if (div < .5f) div = .5f;
                DrawRow(32, 10 + i, (int)(0 + GetHistoryVolume(history.Count - (i)) / div));
                DrawRow(32, 12 - i, (int)(0 + GetHistoryVolume(history.Count - (i)) / div));

            }
        }

        base.Redraw(offx, offy);

    }

    float GetHistoryVolume(int index)
    {
        if (history.Count == 0)
            return 0;

        if (index < history.Count && index > 0)
        {
            //Debug.Log(history[index]);
            return history[index] * 50f;
        }
        else
            return 0;
    }

    void DrawColumn(int x, int y, int height)
    {
        for (int i = 0; i < height; ++i)
        {
            SHGUI.current.SetPixelFront('█', x, y + i, 'z');
            SHGUI.current.SetPixelFront('█', x, y - i, 'z');
        }
    }

    void DrawRow(int x, int y, int height)
    {
        for (int i = 0; i < height; ++i)
        {
            SHGUI.current.SetPixelFront('█', x + i, y, 'z');
            SHGUI.current.SetPixelFront('█', x - i, y, 'z');
        }
    }

    int qSamples = 4096; // array size (corresponds to about 85mS)
    float[] samples; // audio samples

    float GetRMS(int channel) {
        AudioListener.GetOutputData(samples, channel);
        float sum = 0;
        for (var i=0; i < qSamples; i++){
            sum += samples[i]*samples[i];
        }

        return Mathf.Sqrt(sum/qSamples);
    }

    public override void ReactToInputKeyboard(SHGUIinput key)
    {
        if (key == SHGUIinput.enter)
        {
            if (clicker) {
                AddSuperView();
                sound.Play();
            }
            else {
                AddHotView();
                sound2.Play();
            }
            clicker = !clicker;
        }
        else if (key == SHGUIinput.esc)
        {
            Kill();
        }
    }

    public override void OnExit()
    {
        if (sound != null)
        {
            GameObject.Destroy(sound);
        }
    }
}