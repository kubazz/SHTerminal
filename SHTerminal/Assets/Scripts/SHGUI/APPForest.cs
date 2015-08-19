using System.Runtime.Remoting.Messaging;
using System.Text;
using UnityEngine;
using System.Collections;

public class APPForest : SHGUIappbase {
    private enum CellState : byte {
        Empty = 0,
        Tree = 1,
        Burning = 2
    }

    private string text;

    private int height = 22;
    private int width = 62;
    private int fireChance = 10000;
    private int treeChance = 20000;
    private int nearbyTreeChance = 100;

	char treeChar = 'T';
	char fireChar = '&';


    private int generation = 0;

    private CellState[,] state;

    public APPForest()
        : base("forrest-fire-ported-by-kubazz") {
        //text = AddSubView(new SHGUItext("", 1, 1, 'w')) as SHGUItext;
        state = InitializeForestFire(height, width);
    }

    private CellState[,] InitializeForestFire(int height, int width) {
        // Create our state array, initialize all indices as Empty, and return it.
        var state = new CellState[height, width];
        state.Initialize();
        return state;
    }

    public override void Update() {
        base.Update();

        if (!FixedUpdater(0.05f)) {
            return;
        }
        state = StepForestFire(state, fireChance, treeChance);


        //Console.SetCursorPosition(0, 0);
        //Console.ResetColor();
        //Console.WriteLine("Generation " + ++generation);

        var builder = new StringBuilder();
        char c = ' ';
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                switch (state[y, x]) {
                    case CellState.Empty:
                        c = ' ';
                        builder.Append(c);
                        break;
                    case CellState.Tree:
						c = treeChar;
                        builder.Append(c);
                        break;
                    case CellState.Burning:
                        c = fireChar;
                        builder.Append(c);
                        break;
                }
            }
            builder.Append('\n');
        }
        text = builder.ToString();


        //var builder = new StringBuilder();

        //if (FixedUpdater(0.02f))
        //{
        //    Step();

        //    for (int iRow = 0; iRow < height; iRow++)
        //    {
        //        for (int iCol = 0; iCol < width; iCol++)
        //        {
        //            char c = IsBlack[iCol, iRow] ? '▒' : ' ';

        //            // Each cell is two characters wide.
        //            builder.Append(c);
        //        }
        //        builder.Append('\n');
        //    }
        //    text.text = builder.ToString();
        //}
    }

    private CellState[,] StepForestFire(CellState[,] state, int f, int p) {
        /* Clone our old state, so we can write to our new state
         * without changing any values in the old state. */
        var newState = (CellState[,]) state.Clone();

        int height = state.GetLength(0);
        int width = state.GetLength(1);

		bool playtick = false;
		bool playwrong = false;

        for (int i = 1; i < height - 1; i++) {
            for (int o = 1; o < width - 1; o++) {
                /* 
                 * Check the current cell.
                 * 
                 * If it's empty, give it a 1/p chance of becoming a tree.
                 * 
                 * If it's a tree, check to see if any neighbors are burning.
                 * If so, set the cell's state to burning, otherwise give it
                 * a 1/f chance of combusting.
                 * 
                 * If it's burning, set it to empty.
                 */
                switch (state[i, o]) {
                    case CellState.Empty:
                        if (Random.Range(0, p) == 0 ) {
                            newState[i, o] = CellState.Tree;
							playwrong = true;
                        }

                        if (IsNeighbor(state, i, o, CellState.Tree) && Random.Range(0, nearbyTreeChance) == 0)
                        {
							newState[i, o] = CellState.Tree;
							playwrong = true;
						}
                        break;
                    case CellState.Tree:
                        if (IsNeighbor(state, i, o, CellState.Burning) ||
                            Random.Range(0, f) == 0) {
                            newState[i, o] = CellState.Burning;
							playtick = true;
                        }
                        break;
                    case CellState.Burning:
                        newState[i, o] = CellState.Empty;
                        break;
                }
            }
        }

		if (playtick) SHGUI.current.PlaySound(SHGUIsound.tick);
		else if (playwrong) SHGUI.current.PlaySound(SHGUIsound.wrong);
		

        return newState;
    }

    private bool IsNeighbor(CellState[,] state, int x, int y, CellState value) {
        // Check each cell within a 1 cell radius for the specified value.
		if (state[x - 1, y] == value) return true;
		if (state[x + 1, y] == value) return true;
		if (state[x, y - 1] == value) return true;
		if (state[x, y + 1] == value) return true;
		

		return false;
		
		/*
		for (int i = -1; i <= 1; i++) {
            for (int o = -1; o <= 1; o++) {
                if (i == 0 && o == 0) {
                    continue;
                }

                if (state[x + i, y + o] == value) {
                    return true;
                }
            }
        }

        return false;
        */
    }

    public override void Redraw(int offx, int offy) {
        
        //text.text = "";

        base.Redraw(offx, offy);
        DrawText(text, 1, 1, 'w', fade);

        if (fade < 0.99f) {
            return;
        }
    }

    public void DrawText(string text, int x, int y, char col, float fade = 1, char backColor = ' ')
    {
        int xoff = x;
        int yoff = 0;
        text = StringScrambler.GetScrambledString(text, 1 - fade);
        for (int i = 0; i < text.Length; ++i)
        {
            if (text[i] == '\n' || (int)text[i] == 10)
            {
                xoff = x;
                yoff++;
            }
            else
            {
                if (Random.value < fade) {
					if (text[i] == treeChar)
                        col = 'w';
                    else if (text[i] == fireChar)
                        col = 'r';
                    SHGUI.current.SetPixelFront(text[i], xoff, y + yoff, col);
                    if (backColor != ' ')
                    {
                        SHGUI.current.SetPixelBack('█', xoff, y + yoff, backColor);
                    }
                    xoff++;
                }
            }
        }

        //DrawRectBack (x, y, x + text.Length, y + 1, 'g');
    }
}
