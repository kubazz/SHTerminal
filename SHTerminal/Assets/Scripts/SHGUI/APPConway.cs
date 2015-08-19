
using System.Collections.Generic;
using System.Text;
using UnityEngine;



public class APPConway : SHGUIappbase
{
    public List<Vector3> stars;

    private int width = 62;
    private int height = 22;

    private bool loopEdges = true;

    // Holds the current state of the board.
    private bool[,] board;

    private SHGUItext text;

    public APPConway()
        : base("conway's-game-of-life-ported-by-kubazz")
    {
        //stars = new List<Vector3> ();
        text = AddSubView(new SHGUItext("", 1, 1, 'z')) as SHGUItext;
        initializeRandomBoard();
    }

    public override void Update()
    {
        base.Update();

        if(FixedUpdater(0.1f))
            updateBoard();
    }

    public override void Redraw(int offx, int offy)
    {

        base.Redraw(offx, offy);

        drawBoard();
        

        if (fade < 0.99f)
            return;

    }

    // Creates the initial board with a random state.
        private void initializeRandomBoard()
        {
            var random = new Random();

            board = new bool[width, height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    // Equal probability of being true or false.
                    board[x, y] = Random.value > 0.5f;
                }
            }
        }

        private void drawBoard()
        {
            // One Console.Write call is much faster than writing each cell individually.
            var builder = new StringBuilder();

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    char c = board[x, y] ? '█' : ' ';

                    // Each cell is two characters wide.
                    builder.Append(c);
                    
                }
                builder.Append('\n');
            }
            text.text = builder.ToString();
            // Write the string to the console.
        }

        // Moves the board to the next state based on Conway's rules.
        private void updateBoard()
        {
            // A temp variable to hold the next state while it's being calculated.
            bool[,] newBoard = new bool[width, height];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var n = countLiveNeighbors(x, y);
                    var c = board[x, y];

                    // A live cell dies unless it has exactly 2 or 3 live neighbors.
                    // A dead cell remains dead unless it has exactly 3 live neighbors.
                    newBoard[x, y] = c && (n == 2 || n == 3) || !c && n == 3;
                }
            }

            // Set the board to its new state.
            board = newBoard;
        }

        // Returns the number of live neighbors around the cell at position (x,y).
        private int countLiveNeighbors(int x, int y)
        {
            // The number of live neighbors.
            int value = 0;

            // This nested loop enumerates the 9 cells in the specified cells neighborhood.
            for (var j = -1; j <= 1; j++)
            {
                // If loopEdges is set to false and y+j is off the board, continue.
                if (!loopEdges && y + j < 0 || y + j >= height)
                {
                    continue;
                }

                // Loop around the edges if y+j is off the board.
                int k = (y + j + height) % height;

                for (var i = -1; i <= 1; i++)
                {
                    // If loopEdges is set to false and x+i is off the board, continue.
                    if (!loopEdges && x + i < 0 || x + i >= width)
                    {
                        continue;
                    }

                    // Loop around the edges if x+i is off the board.
                    int h = (x + i + width) % width;

                    // Count the neighbor cell at (h,k) if it is alive.
                    value += board[h, k] ? 1 : 0;
                }
            }

            // Subtract 1 if (x,y) is alive since we counted it as a neighbor.
            return value - (board[x, y] ? 1 : 0);
        }
}


