
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;


public class APPAnt : SHGUIappbase
{
    enum Direction
    {
        North, East, West, South
    }

    private int width = 62;
    private int height = 22;


    private SHGUItext text;

    public readonly bool[,] IsBlack;
    private Vector2 _origin;
    private Vector2 _antPosition = new Vector2(0, 0);
    public bool OutOfBounds { get; set; }

    // I don't see any mention of what direction the ant is supposed to start out in
    private Direction _antDirection = Direction.East;

    private readonly Direction[] _leftTurn = new[] { Direction.West, Direction.North, Direction.South, Direction.East };
    private readonly Direction[] _rightTurn = new[] { Direction.East, Direction.South, Direction.North, Direction.West };
    private readonly int[] _xInc = new[] { 0, 1, -1, 0 };
    private readonly int[] _yInc = new[] { -1, 0, 0, 1 };

    public APPAnt()
        : base("langdon-ant-ported-by-kubazz")
    {
        text = AddSubView(new SHGUItext("", 1, 1, 'w')) as SHGUItext;

        _origin = new Vector2((int)(Random.value * width), (int)(Random.value * height));
        IsBlack = new bool[width, height];
        OutOfBounds = false;
    }

    private void MoveAnt()
    {
        _antPosition.x += _xInc[(int)_antDirection];
        _antPosition.y += _yInc[(int)_antDirection];
    }

    public Vector2 Step()
    {
        if (OutOfBounds) {
            return Vector2.zero;
        }
        Vector2 ptCur = new Vector2(_antPosition.x + _origin.x, _antPosition.y + _origin.y);
        bool leftTurn = IsBlack[(int)ptCur.x, (int)ptCur.y];
        int iDirection = (int)_antDirection;
        _antDirection = leftTurn ? _leftTurn[iDirection] : _rightTurn[iDirection];
        IsBlack[(int)ptCur.x, (int)ptCur.y] = !IsBlack[(int)ptCur.x, (int)ptCur.y];
        MoveAnt();
        ptCur = new Vector2(_antPosition.x + _origin.x, _antPosition.y + _origin.y);
        OutOfBounds =
            ptCur.x < 0 ||
            ptCur.x >= IsBlack.GetUpperBound(0) ||
            ptCur.y < 0 ||
            ptCur.y >= IsBlack.GetUpperBound(1);
        return _antPosition;
    }

    public override void Update()
    {
        base.Update();

        var builder = new StringBuilder();

        if (FixedUpdater(0.02f)) {
            Step();

            for (int iRow = 0; iRow < height; iRow++)
            {
                for (int iCol = 0; iCol < width; iCol++)
                {
                    char c = IsBlack[iCol, iRow] ? '▒' : ' ';

                    // Each cell is two characters wide.
                    builder.Append(c);
                }
                builder.Append('\n');
            }
            text.text = builder.ToString();
        }
            
    }

    public override void Redraw(int offx, int offy)
    {

        base.Redraw(offx, offy);

        //drawBoard();


        if (fade < 0.99f)
            return;

    }
}


