using UnityEngine;
using System;

public class APPfire : SHGUIappbase
{
    float time = 0;

    const int SCR_SHIFT_X = 1;
    const int SCR_SHIFT_Y = 1;

    const int SCR_WIDTH = 62;
    const int SCR_HEIGHT = 22;

    int currFrame = 0;
    int[,,] m_fireBuffer = new int[2,SCR_WIDTH, SCR_HEIGHT];


    public APPfire()
        : base("Fire!")
	{
	}

    void Clear()
    {
        for (int i = 0; i < SCR_HEIGHT; ++i)
        {
            for (int j = 0; j < SCR_WIDTH; ++j)
            {
                m_fireBuffer[0, j, i] = 0;
                m_fireBuffer[1, j, i] = 0;
            }
        }
    }

    void GenerateSparkles( int buffer )
    {
        for (int i = 0; i < SCR_WIDTH; ++i)
        {
            m_fireBuffer[buffer, i,SCR_HEIGHT - 1] = (int)UnityEngine.Random.Range(0, 255);
        }
    }

    void Step( int buffer )
    {
        int outBuffer = 1 - buffer;
        for (int i = 1; i < SCR_HEIGHT - 1; ++i)
        {
            for (int j = 1; j < SCR_WIDTH - 1; ++j)
            {
                int average = m_fireBuffer[buffer, j, i - 1] +
                              m_fireBuffer[buffer, j, i] +
                              m_fireBuffer[buffer, j - 1, i + 1] +
                              m_fireBuffer[buffer, j, i + 1] +
                              m_fireBuffer[buffer, j + 1, i + 1];

                average /= 5;

                for (int k = 0; k < 2; ++k)
                {
                    if (average > 0)
                    {
                        --average;
                    }
                }

                m_fireBuffer[outBuffer, j, i] = average;
            }
        }
    }

    void ValueToChar(int val, ref char outChar, ref char outCol)
    {
        if (val > 228)
        {
            outChar = '█';
            outCol = 'w';
        }
        else if (val > 192)
        {
            outChar = '▓';
            outCol = 'w';
        }
        else if (val > 160)
        {
            outChar = '▓';
            outCol = 'z';
        }
        else if (val > 128)
        {
            outChar = '▒';
            outCol = 'w';
        }
        else if (val > 96)
        {
            outChar = '▒';
            outCol = 'z';
        }
        else if (val > 64)
        {
            outChar = '░';
            outCol = 'w';
        }
        else if (val > 32)
        {
            outChar = '░';
            outCol = 'z';
        }
        else
        {
            outChar = ' ';
            outCol = (char)0;
        }
    }

    public override void Update()
    {
        base.Update();

        if (!FixedUpdater(.03f))
            return;

        GenerateSparkles(currFrame);
        Step(currFrame);

        currFrame = 1 - currFrame;
    }

    public override void Redraw(int offx, int offy)
    {
		base.Redraw (offx, offy);

        for (int i = 0; i < SCR_HEIGHT; ++i)
        {
            for (int j = 0; j < SCR_WIDTH; ++j)
            {
                char pixelChar = (char)0;
                char pixelCol = (char)0;

                ValueToChar( m_fireBuffer[currFrame, j,i], ref pixelChar, ref pixelCol);

                SHGUI.current.SetPixelFront(pixelChar, j + SCR_SHIFT_X, i +SCR_SHIFT_Y, pixelCol);
            }
        }
	}
}