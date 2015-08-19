using System.Runtime.Remoting.Messaging;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class APPwireworld : SHGUIappbase {
	private enum CellState : byte {
		Empty = 0,
		Wire = 1,
		Head = 2,
		Tail = 3

	}

	struct source{		
		public int x;
		public int y;

		public source(int X, int Y){
			x = X;
			y = Y;
		}
	}

	private List<source> sources;
	
	private string text;
	
	private int height = 22;
	private int width = 78;
	
	char headChar = '█';
	char tailChar = '▒';
	char wireChar = '░';	
	
	private int generation = 0;
	
	private CellState[,] state;
	
	public APPwireworld(string wirename)
	: base("wireworld-ported-by-3.14 (click to place/remove wires)") {
		//text = AddSubView(new SHGUItext("", 1, 1, 'w')) as SHGUItext;
		if (wirename == "") wirename = "schemEmpty";
		InitWithSchem(wirename);
	}

	public void InitWithSchem(string wirename){
		state = InitializeWires(height, width);
		allowCursorDraw = true;
		sources = new List<source>();
		
		loadSchematicsFromASCII(wirename);
	}

	private void loadSchematicsFromASCII(string wirename){

		string schem = SHGUI.current.GetASCIIartByName("wires/" + wirename);
		//Debug.Log(schem);

		if (schem == null)
			return;

		int x = 0;
		int y = 0;
		for (int i = 0; i < schem.Length; ++i){
			if (schem[i] == 'X'){
				sources.Add(new source(x, y));
				state[y, x] = CellState.Wire;
			}
			else if (schem[i] == '#'){
				state[y, x] = CellState.Wire;
			}

			x++;
			if (schem[i] == '\n' || schem[i] == 10){
				x = 0;
				y++;
			}

		}	
	}

	private CellState[,] InitializeWires(int height, int width) {
		// Create our state array, initialize all indices as Empty, and return it.
		var state = new CellState[height, width];
		state.Initialize();
		return state;
	}

	int sourceDelay = 0;
	public override void Update() {
		base.Update();
		
		if (!FixedUpdater(0.1f)) {
			return;
		}
		state = StepWires(state);
		
		var builder = new StringBuilder();
		char c = ' ';
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				switch (state[y, x]) {
				case CellState.Empty:
					c = ' ';
					builder.Append(c);
					break;
				case CellState.Head:
					c = headChar;
					builder.Append(c);
					break;
				case CellState.Tail:
					c = tailChar;
					builder.Append(c);
					break;
				case CellState.Wire:
					c = wireChar;
					builder.Append(c);
					break;
				}
			}
			builder.Append('\n');
		}
		text = builder.ToString();
	}
	
	private CellState[,] StepWires(CellState[,] state) {
		var newState = (CellState[,]) state.Clone();
		
		int height = state.GetLength(0);
		int width = state.GetLength(1);
		
		for (int i = 1; i < height - 1; i++) {
			for (int o = 1; o < width - 1; o++) {
				switch (state[i, o]) {
				case CellState.Wire:
					int count = CountNeighbor(state, i, o, CellState.Head);
					if (count == 1 || count == 2){
						newState[i, o] = CellState.Head;
					}
					break;
				case CellState.Head:
						newState[i, o] = CellState.Tail;
					break;
				case CellState.Tail:
					newState[i, o] = CellState.Wire;
					break;
				}
			}
		}


		sourceDelay--;
		if (sourceDelay < 0){
			for (int i = 0; i < sources.Count; ++i){
				newState[sources[i].y, sources[i].x] = CellState.Head;
			}

			sourceDelay = int.MaxValue;
		}
		
		return newState;
	}
	
	private int CountNeighbor(CellState[,] state, int x, int y, CellState value) {
		int count = 0;
		for (int i = -1; i <= 1; i++) {
            for (int o = -1; o <= 1; o++) {
                if (i == 0 && o == 0) {
                    continue;
                }

                if (state[x + i, y + o] == value) {
					count++;
                }
            }
        }

		return count;
	}
	
	public override void Redraw(int offx, int offy) {

		base.Redraw(offx, offy);

		if (fade < 0.7f) {
			return;
		}		

		if (text != null)
		DrawText(text, 1, 1, 'w', fade);

		for (int i = 0; i < sources.Count; ++i){
			SHGUI.current.SetPixelFront('X', sources[i].x + 1, sources[i].y + 1, 'w');
		}
		

	}
	
	public void DrawText(string text, int x, int y, char col, float fade = 1)
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
					if (text[i] == headChar)
						col = 'r';
					else if (text[i] == tailChar)
						col = 'r';
					else if (text[i] == wireChar)
						col = 'z';

					SHGUI.current.SetPixelBack(text[i], xoff, y + yoff, col);
					xoff++;
				}
			}
		}
	}

	public override void ReactToInputKeyboard(SHGUIinput key){
		if (fadingOut)
			return;

		if (key == SHGUIinput.enter){
			ClearAllImpulses();
			sourceDelay = 0;
		}
		
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView ();
	}

	public override void ReactToInputMouse(int x, int y, bool clicked, SHGUIinput scroll)
	{	
		if (fadingOut)
			return;
		
		if (clicked){
			x -= 1;
			y -= 1;

			if (x < 0) return;
			if (x >= width) return;
			if (y < 0) return;
			if (y >= height) return;

			ClearAllImpulses();

			if (state[y,x] == CellState.Empty)
				state[y, x] = CellState.Wire;
			else
				state[y,x] = CellState.Empty;


			sourceDelay = 0;
		}
	}

	void ClearAllImpulses(){
		for (int i = 1; i < height - 1; i++) {
			for (int o = 1; o < width - 1; o++) {
				if (state[i, o] == CellState.Head)
					state[i, o] = CellState.Wire;
			}
		}
	}
}
