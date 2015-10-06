using UnityEngine;
using System;

public class APProtozoomer : SHGUIappbase
{
    const int SCR_SHIFT_X = 1;
    const int SCR_SHIFT_Y = 1;

    const int SCR_WIDTH = 62;
    const int SCR_HEIGHT = 22;

    const int IMAGE_WIDTH = 16;
    const int IMAGE_HEIGHT = 8;

    const float ASPECT_RATIO = 1.8f;

    byte[,] s_image = new byte[IMAGE_HEIGHT, IMAGE_WIDTH]
                        { { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                          { 0x00, 0x00, 0x00, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0x00, 0x00, 0x00 },
                          { 0x00, 0x00, 0xA8, 0xA8, 0x00, 0x00, 0xA8, 0xA8, 0xA8, 0xA8, 0x00, 0x00, 0xA8, 0xA8, 0x00, 0x00 },
                          { 0x00, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0x00 },
                          { 0x00, 0xA8, 0xA8, 0xA8, 0x00, 0x00, 0xA8, 0xA8, 0xA8, 0xA8, 0x00, 0x00, 0xA8, 0xA8, 0xA8, 0x00 },
                          { 0x00, 0x00, 0xA8, 0xA8, 0xA8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xA8, 0xA8, 0xA8, 0x00, 0x00 },
                          { 0x00, 0x00, 0x00, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0xA8, 0x00, 0x00, 0x00 },
                          { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 } };

    private float m_time = 0.0f;
    private float m_currAngle = 0.0f;
    private float m_currScale = 1.0f;
    private float m_currPosX = 0.0f;
    private float m_currPosY = 0.0f;
     
    public APProtozoomer()
        : base("Zoom & Rotate!")
    {
    }

    public override void Update()
    {
        base.Update();

        if (!FixedUpdater(.03f))
            return;

        m_time += 0.1f;
        m_currAngle += 0.05f;
        m_currScale = 1.0f / ( 2.2f + 1.8f * Mathf.Sin(m_time * 0.4f) );
        m_currPosX = 30.0f * Mathf.Sin(m_time * 0.17f);
        m_currPosY = 20.0f * Mathf.Cos(m_time * 0.12f);
    }

    void ValueToChar(byte val, ref char outChar, ref char outCol)
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

    byte SampleImage(float u, float v)
    {
        float uBase = Mathf.Floor(u / IMAGE_WIDTH) * IMAGE_WIDTH;
        float vBase = Mathf.Floor(v / IMAGE_HEIGHT) * IMAGE_HEIGHT;

        float uLocal = u - uBase;
        float vLocal = v - vBase;

        int x0 = (int)Mathf.Floor(uLocal - 0.5f);
        int y0 = (int)Mathf.Floor(vLocal - 0.5f);

        if (x0 < 0)
        {
            x0 += IMAGE_WIDTH;
        }

        if (y0 < 0)
        {
            y0 += IMAGE_HEIGHT;
        }

        return s_image[y0, x0];
    }


    public override void Redraw(int offx, int offy)
    {
        base.Redraw(offx, offy);

        float angleSin = Mathf.Sin(m_currAngle);
        float angleCos = Mathf.Cos(m_currAngle);

        for (int i = 0; i < SCR_HEIGHT; ++i)
        {
            for (int j = 0; j < SCR_WIDTH; ++j)
            {
                float x = ( j - SCR_WIDTH / 2 ) + 0.5f;
                float y = ( i - SCR_HEIGHT / 2 ) + 0.5f;

                x /= ASPECT_RATIO;

                float xTrans = ( x * angleCos - y * angleSin ) * m_currScale + m_currPosX;
                float yTrans = ( x * angleSin + y * angleCos ) * m_currScale + m_currPosY;

                byte imageSample = SampleImage(xTrans, yTrans);

                char pixelChar = (char)0;
                char pixelCol = (char)0;

                ValueToChar(imageSample, ref pixelChar, ref pixelCol);               

                SHGUI.current.SetPixelFront(pixelChar, j + SCR_SHIFT_X, i + SCR_SHIFT_Y, pixelCol);
            }
        }
    }
}