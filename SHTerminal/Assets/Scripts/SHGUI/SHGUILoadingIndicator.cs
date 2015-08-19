using System;
using UnityEngine;
using System.Collections;

public class SHGUILoadingIndicator : SHGUIview
{
    string loadingChars = @"│/─\";
    private string currentChar = " ";
    public char backColor = ' ';
    private float rotationSpeed = 0.15f;
    private float timeToRotate = 0.15f;
    private int indicatorIndex = 0;

    public SHGUILoadingIndicator(int X, int Y, float delay = 0.15f, char col = 'w') {
		Init ();
        rotationSpeed = delay;
        timeToRotate = rotationSpeed;
        currentChar = loadingChars[indicatorIndex].ToString();
		x = X;
		y = Y;

		SetColor(col);
	}

    public override void Redraw(int offx, int offy)
    {
        if (hidden)
            return;
        SHGUI.current.DrawText(currentChar, x + offx, y + offy, color, fade, backColor);
        base.Redraw(offx, offy);

    }

    
    public override void Update()
    {
        base.Update();
        if (timeToRotate <= 0.0f) {
            indicatorIndex++;
            indicatorIndex %= loadingChars.Length;
            currentChar = loadingChars[indicatorIndex].ToString();
            timeToRotate = rotationSpeed;
        } else {
            timeToRotate -= Time.unscaledDeltaTime;
        }
    }
}
