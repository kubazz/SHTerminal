using UnityEngine;
using System.Collections;

public class APPHotSlot : SHGUIappbase {
	//Private
	//Wygląd
	private string[]	slotMachine	= new string[] {
"╔══════════════════╦╦═══════════════════╦╦═══════════════════╗",
"║                  ║║                   ║║                   ║",
"║                  ║║                   ║║                   ║",
"║                  ║║                   ║║                   ║",
"║                  ║║                   ║║                   ║",
"╠══════════════════╬╬═══════════════════╬╬═══════════════════╣",
"╠══════════════════╬╬═══════════════════╬╬═══════════════════╣",
"║                  ║║                   ║║                   ║",
"║                  ║║                   ║║                   ║",
"║                  ║║                   ║║                   ║",
"║                  ║║                   ║║                   ║",
"╠══════════════════╬╬═══════════════════╬╬═══════════════════╣",
"╠══════════════════╬╬═══════════════════╬╬═══════════════════╣",
"║                  ║║                   ║║                   ║",
"║                  ║║                   ║║                   ║",
"║                  ║║                   ║║                   ║",
"║                  ║║                   ║║                   ║",
"╠══════════════════╩╩═══════════════════╩╩═══════════════════╣",
"║ Money:           $                                         ║",
"║ Bet:  ←↑         ↓→                                        ║",
"║ ↑↓ Arrows makes +-10 bet        ←→ Arrows makes +-1000 bet ║",
"╚════════════════════════════════════════════════════════════╝"
	};

	private string[]	lostScreen	= new string[] {
" ",
"      \\   /  /---\\  |    |     /---\\  /---\\  /---",
"       \\ /   |   |  |    |     |   |  |   |  |___",
"        |    |   |  |    |     |___|  |---/  |",
"        |    \\---/  \\____/     |   |  |  \\   \\___",
" ",
"      /---\\  |    |  -----      /---\\  /---",
"      |   |  |    |    |        |   |  |",
"      |   |  |    |    |        |   |  |---",
"      \\---/  \\____/    |   █  █ \\---/  |",
"                           █  █",
"                        █████████",
"                       ██  █  █ ██",
"                      ██   █  █  ██",
"                       ██  █  █",
"                        █████████",
"                           █  █ ██",
"                       ██  █  █  ██",
"                        ██ █  █ ██",
"                         ████████",
"                           █  █",
"                           █  █"
	};

	//Symbole w slotach
	private string[]	slot_0		= new string[9 * 4] {
"      __O         ",	//1
"     / / \\        ",
"      / \\         ",
"     /  /         ",
"   ____________   ",
"    \\  _______=   ",	//2
"    / |_/         ",
"   /__/           ",
" SUPER HOT SUPER  ",	//3
" HOT SUPER HOT SU ",
" PER HOT SUPER HO ",
" T SUPER HOT SUPE ",
"                  ",	//1
"     ^      ^     ",
"      ______      ",
"                  ",
" │  │ ┌───┐ ──┬── ",	//10
" │__│ │   │   │   ",
" │  │ │   │   │   ",
" │  │ └───┘   │   ",
"     _______      ",	//8
"     \\__   /      ",
"       /  /       ",
"      /__/        ",
"      ██████      ",	//5
"      █_____      ",
"           █      ",
"      ██████      ",
"      █████       ",	//3
"      █____       ",
"      █   █       ",
"      █████       ",
"     \\     /      ",	//2
"      \\___/       ",
"        |         ",
"       /_\\        "
	};
	private string[]	slot_1		= new string[9 * 4] {
"     \\     /      ",
"      \\___/       ",
"        |         ",
"       /_\\        ",
" │  │ ┌───┐ ──┬── ",
" │__│ │   │   │   ",
" │  │ │   │   │   ",
" │  │ └───┘   │   ",
"   ____________   ",
"    \\  _______=   ",
"    / |_/         ",
"   /__/           ",
"      ██████      ",	//5
"      █_____      ",
"           █      ",
"      ██████      ",
" SUPER HOT SUPER  ",
" HOT SUPER HOT SU ",
" PER HOT SUPER HO ",
" T SUPER HOT SUPE ",
"      █████       ",
"      █____       ",
"      █   █       ",
"      █████       ",
"                  ",
"     ^      ^     ",
"      ______      ",
"                  ",
"     _______      ",
"     \\__   /      ",
"       /  /       ",
"      /__/        ",
"      __O         ",
"     / / \\        ",
"      / \\         ",
"     /  /         "
	};
	private string[]	slot_2		= new string[9 * 4] {
"      __O         ",
"     / / \\        ",
"      / \\         ",
"     /  /         ",
"     \\     /      ",
"      \\___/       ",
"        |         ",
"       /_\\        ",
"   ____________   ",
"    \\  _______=   ",
"    / |_/         ",
"   /__/           ",
" │  │ ┌───┐ ──┬── ",
" │__│ │   │   │   ",
" │  │ │   │   │   ",
" │  │ └───┘   │   ",
"      █████       ",
"      █____       ",
"      █   █       ",
"      █████       ",
" SUPER HOT SUPER  ",
" HOT SUPER HOT SU ",
" PER HOT SUPER HO ",
" T SUPER HOT SUPE ",
"      ██████      ",	//5
"      █_____      ",
"           █      ",
"      ██████      ",
"                  ",
"     ^      ^     ",
"      ______      ",
"                  ",
"     _______      ",
"     \\__   /      ",
"       /  /       ",
"      /__/        "
	};
	char[,]	winBg			= new char[62, 22];

	//Komunikaty
	string[]	winMsg		= new string[] {
		"Amount:     ",
		"You won some money!",
		"You won some hot money!",
		"You won some super hot money!",
		"You won some SUPER FCKIN' HOT money!"
	};
	string		continueMsg	= "[Enter] to continue";
	
	private int			currentBet		= 10;				//Obecny zakład
	private int			currentMoney	= 100;				//Obecny stan pieniędzy
	private float		baseRollSpeed	= 0.075f;			//Podstawowa prędkoś obracania slotów
	
	private int			currentSlot		= 0;				//Obecnie zatrzymywany slot
	private bool 		roll			= false;			//Czy w trakcie obracania slotami
	private bool[]		slotGo			= new bool[3] {		//obecny stan kręcenia się slotów
		true, true, 	true
	};	
	private int[]		slotOffset		= new int[3] {		//Obecne przesunięcie slotów
		0, 0, 0	
	};	
	
	private bool		finish			= false;			//Etap zpisywania wyników
	private int			win				= 0;				//Mnożnik wygranej (jeżeli ujemny to ABS)
	private bool		lost			= false;			//Przegrana
	
	private float		msgFlashTimer	= 0f;				//Licznik mrygania komunikatu o Enterze
	private float[]		slotRollTimer	= new float[3] {	//Licznik od przesówania slotów
		0f, 0f, 0f
	};
	private float		finishTimer		= 0f;

	//Public
	public APPHotSlot()
		:	base("hotslot-v23.6.6.6-by-onionmilk")
	{
		//Init
		slotOffset[0]	= slotOffset[1]	= slotOffset[2]	= 0;

		//Restart
		slotGo[0]		= slotGo[1]		= slotGo[2]		= false;
		roll			= false;
		currentSlot		= 0;
		finish			= false;
		win				= 0;

	}

	public override void Update() {
		base.Update();

		msgFlashTimer	+= Time.unscaledDeltaTime;

		if (roll) {
			for(int i = 0; i < 3; ++i)
				slotRollTimer[i]	+= Time.unscaledDeltaTime;
			//--
			for(int i = 0; i < 3; ++i)
				if (slotRollTimer[i] > (baseRollSpeed / (i + 1))) {
					if (slotGo[i]) {
						slotOffset[i]	-= 1;
						if (slotOffset[i] < 0)
							slotOffset[i] = 35;
						//--
					} else if (slotGo[i] == false && slotOffset[i]%4 != 0) {
						slotOffset[i]	-= 1;
						if (slotOffset[i] < 0)
							slotOffset[i] = 35;
						//--
					}
					slotRollTimer[i]	= 0f;
				}
			//--
		} else
			for(int i = 0; i < 3; ++i)
				if (slotGo[i] == false && slotOffset[i]%4 != 0) {
					slotOffset[i]	-= 1;
					if (slotOffset[i] < 0)
						slotOffset[i] = 35;
					//--
				}
			//--
		//--

		if (finish) {
/*	Wygrane (ID - 1):
	1:	0 8 0	//1
	2:	1 2 2	//2
	3:	2 4 5	//3
	4:	3 6 7	//1
	5:	4 1 3	//10
	6:	5 7 8	//8
	7:	6 3 6	//5
	8:	7 5 4	//3
	9:	8 0 1	//2
*/
			if (checkSlotSymbolID(8, 7, 8)) {
				win			= 1;
			} else if (checkSlotSymbolID(0, 1, 1)) {
				win			= 2;
			} else if (checkSlotSymbolID(1, 3, 4)) {
				win			= 3;
			} else if (checkSlotSymbolID(2, 5, 6)) {
				win			= 1;
			} else if (checkSlotSymbolID(3, 0, 2)) {
				win			= 10;
			} else if (checkSlotSymbolID(4, 6, 7)) {
				win			= 8;
			} else if (checkSlotSymbolID(5, 2, 5)) {
				win			= 5;
			} else if (checkSlotSymbolID(6, 4, 3)) {
				win			= 3;
			} else if (checkSlotSymbolID(7, 8, 0)) {
				win			= 2;
			} else {
				win			= 0;
			}

			if (win > 0 && finishTimer == 0f) {
				currentMoney	+= currentBet * win;
				win				 = -win;
			}

			finishTimer	+= Time.unscaledDeltaTime;
		}
	}	

	public override void Redraw(int offx, int offy) {
		base.Redraw(offx, offy);

		if (lost) {
			for(int y = 0; y < 22; ++y)
				for(int x = 0; x < lostScreen[y].Length; ++x) {
					SHGUI.current.SetPixelFront(lostScreen[y][x], 1 + x, 1 + y, 'r');
					SHGUI.current.SetPixelBack(' ', 1 + x, 1 + y, 'w');
				}
			//--
			return;
		}

		if (finish && finishTimer > 0.5f) {														//Moment wygania
			if (win != 0) {
				//Tło
				if (msgFlashTimer > 0.25f) {
					for(int y = 0; y < 22; ++y)
						for(int x = 0; x < 62; ++x)
							winBg[x, y]	= (Random.value < 0.25f? '$': ' ');
						//--
					//--
					msgFlashTimer	= 0f;
				}
				for(int y = 0; y < 22; ++y)
					for(int x = 0; x < 62; ++x)
						if (y > 6 && y < 11)
							continue;
						else
							SHGUI.current.SetPixelBack('█', 1 + x, 1 + y, 'w');
						//--
					//--
				//--
				for(int y = 0; y < 22; ++y)
					for(int x = 0; x < 62; ++x)
						if (y > 6 && y < 11)
							continue;
						else
							SHGUI.current.SetPixelFront(winBg[x, y], 1 + x, 1 + y, 'r');
						//--
					//--
				//--
				
				//Symbole w tle
				for(int y = 0; y < 4; ++y)
					for(int x = 0; x < 18; ++x) {
						SHGUI.current.SetPixelFront(slot_0[(y + 4 + slotOffset[0])%36][x], 2 + x, 8 + y, 'w');
						SHGUI.current.SetPixelBack('█', 2 + x, 8 + y, 'r');
					}
				//--
				for(int y = 0; y < 4; ++y)
					for(int x = 0; x < 18; ++x) {
						SHGUI.current.SetPixelFront(slot_1[(y + 4 + slotOffset[1])%36][x], 23 + x, 8 + y, 'w');
						SHGUI.current.SetPixelBack('█', 23 + x, 8 + y, 'r');
					}
				//--
				for(int y = 0; y < 4; ++y)
					for(int x = 0; x < 18; ++x) {
						SHGUI.current.SetPixelFront(slot_2[(y + 4 + slotOffset[2])%36][x], 44 + x, 8 + y, 'w');
						SHGUI.current.SetPixelBack('█', 44 + x, 8 + y, 'r');
					}
				//--

				//Teksty
				int	msgID	= 1;
				if (win == 8)
					msgID	= 3;
				else if (win == 10)
					msgID	= 4;
				else if (win >= 3)
					msgID	= 2;
				//--

				for(int y = 0; y < 3; ++y)
					for(int x = 0; x < (winMsg[msgID].Length + 2); ++x)
						SHGUI.current.SetPixelBack('█', 30 - (winMsg[msgID].Length / 2) + x, 19 + y, 'r');
					//--
				//--
				
				for(int x = 0; x < winMsg[msgID].Length; ++x)
					SHGUI.current.SetPixelFront(winMsg[msgID][x], 31 - (winMsg[msgID].Length / 2) + x, 19, 'w');
				//--
				for(int x = 0; x < winMsg[0].Length; ++x)
					SHGUI.current.SetPixelFront(winMsg[0][x], 22 + x, 20, 'w');
				//--
				for(int x = 0; x < continueMsg.Length; ++x)
					SHGUI.current.SetPixelFront(continueMsg[x], 21 + x, 21, 'w');
				//--
				
				string	wonMoney	= (win * currentBet).ToString();
				for(int x = 0; x < wonMoney.Length; ++x)
					SHGUI.current.SetPixelFront(wonMoney[x], 20 + winMsg[0].Length + x, 20, 'w');
				//--
				SHGUI.current.SetPixelFront('$', 20 + winMsg[0].Length + wonMoney.Length, 20, 'w');
			} else {
				finish	= false;
			}
		} else {																						//Reszta gry
			//Maszyna
			//SHGUI.current.SetPixelFront(char character, int posX, int posY, char color);
			for(int y = 0; y < slotMachine.Length; ++y)
				for(int x = 0; x < slotMachine[y].Length; ++x)
					SHGUI.current.SetPixelFront(slotMachine[y][x], 1 + x, 1 + y, 'w');
				//--
			//--
			
			//Symbole
				//Środkowy rząd
			for(int y = 0; y < 6; ++y)
				for(int x = 0; x < 60; ++x)
					SHGUI.current.SetPixelBack('█', 2 + x, 7 + y, 'r');
				//--
			//--

				//Slot 0
			for(int y = 0; y < 4; ++y)
				for(int x = 0; x < 18; ++x)
					SHGUI.current.SetPixelFront(slot_0[(y + slotOffset[0])%36][x], 2 + x, 2 + y, 'w');
				//--
			//--
			for(int y = 0; y < 4; ++y)
				for(int x = 0; x < 18; ++x)
					SHGUI.current.SetPixelFront(slot_0[(y + 4 + slotOffset[0])%36][x], 2 + x, 8 + y, 'w');
				//--
			//--
			for(int y = 0; y < 4; ++y)
				for(int x = 0; x < 18; ++x)
					SHGUI.current.SetPixelFront(slot_0[(y + 8 + slotOffset[0])%36][x], 2 + x, 14 + y, 'w');
				//--
			//--
				//Slot 1
			for(int y = 0; y < 4; ++y)
				for(int x = 0; x < 18; ++x)
					SHGUI.current.SetPixelFront(slot_1[(y + slotOffset[1])%36][x], 22 + x, 2 + y, 'w');
				//--
			//--
			for(int y = 0; y < 4; ++y)
				for(int x = 0; x < 18; ++x)
					SHGUI.current.SetPixelFront(slot_1[(y + 4 + slotOffset[1])%36][x], 22 + x, 8 + y, 'w');
				//--
			//--
			for(int y = 0; y < 4; ++y)
				for(int x = 0; x < 18; ++x)
					SHGUI.current.SetPixelFront(slot_1[(y + 8 + slotOffset[1])%36][x], 22 + x, 14 + y, 'w');
				//--
			//--
				//Slot 2
			for(int y = 0; y < 4; ++y)
				for(int x = 0; x < 18; ++x)
					SHGUI.current.SetPixelFront(slot_2[(y + slotOffset[2])%36][x], 43 + x, 2 + y, 'w');
				//--
			//--
			for(int y = 0; y < 4; ++y)
				for(int x = 0; x < 18; ++x)
					SHGUI.current.SetPixelFront(slot_2[(y + 4 + slotOffset[2])%36][x], 43 + x, 8 + y, 'w');
				//--
			//--
			for(int y = 0; y < 4; ++y)
				for(int x = 0; x < 18; ++x)
					SHGUI.current.SetPixelFront(slot_2[(y + 8 + slotOffset[2])%36][x], 43 + x, 14 + y, 'w');
				//--
			//--

			//Pieniądze i zakład
			string	tempMoney	= currentMoney.ToString();
			if (tempMoney.Length > 9)
				tempMoney	= "SUPER HOT";
			//--

			string	tempBet		= currentBet.ToString();

			for(int x = 0; x < 9; ++x)
				SHGUI.current.SetPixelBack('█', 11 + x, 19, 'w');
			//--
			for(int x = 0; x < 9; ++x)
				SHGUI.current.SetPixelBack('█', 11 + x, 20, 'z');
			//--
			for(int x = 0; x < tempMoney.Length; ++x)
				SHGUI.current.SetPixelFront(tempMoney[x], 20 - tempMoney.Length + x, 19, 'r');
			//--
			for(int x = 0; x < tempBet.Length; ++x)
				SHGUI.current.SetPixelFront(tempBet[x], 20 - tempBet.Length + x, 20, 'w');
			//--
			
			//Komunikat sterowania
			//Press Enter to Roll!
			string	msg	 = "Press Enter to ";
			if (roll)
				msg		+= "Stop!";
			else
				msg		+= "Roll!";
			//--

			if (msgFlashTimer > 0.5f) {
				for(int x = 0; x < msg.Length; ++x)
					SHGUI.current.SetPixelFront(msg[x], 61 - msg.Length + x, 19, 'r');
				//--
				if (msgFlashTimer > 1f)
					msgFlashTimer	= 0f;
				//--
			}
		}
	}

	public override void ReactToInputKeyboard(SHGUIinput key) {
		//Wyjście
		if (key == SHGUIinput.esc)
			SHGUI.current.PopView();
		//--

		//Gry gracz stracil wszystkie pieniądze
		if (lost) {
			if (key == SHGUIinput.enter)
				SHGUI.current.PopView();
			//--
			return;
		}

		//Reszta
		if (!finish) {
			if (!roll) {
				//Kontrola Stawki
				if (key == SHGUIinput.up)
					currentBet	+= 10;
				//--
				//--
				if (key == SHGUIinput.down)
					currentBet	-= 10;
				//--
				if (key == SHGUIinput.left)
					currentBet	+= 1000;
				//--
				if (key == SHGUIinput.right)
					currentBet	-= 1000;
				//--
				
				if (currentBet > 999999999)
					currentBet	= 999999999;
				//--
				if (currentBet > currentMoney)
					currentBet	= currentMoney;
				//--
				if (currentBet < 10)
					currentBet	= 10;
				//--
			}

			if (key == SHGUIinput.enter) {
				if (!roll) {
					slotGo[0]		= slotGo[1]	= slotGo[2]	= true;
					currentSlot		= 0;
					roll			= true;
					currentMoney	-= currentBet;
					if (currentMoney < 0)
						lost		= true;
					//--
				} else {
					slotGo[currentSlot++]		= false;
					if (currentSlot > 2) {
						finish		= true;
						roll		= false;
					}
				}
			}
		} else
			if (key == SHGUIinput.enter) {
				finish		= false;
				finishTimer	= 0f;
			}
		//--
	}

	bool checkSlotSymbolID(int left, int middle, int right) {
		int leftID		= Mathf.FloorToInt((float)(slotOffset[0]) / 4f);
		int middleID	= Mathf.FloorToInt((float)(slotOffset[1]) / 4f);
		int rightID		= Mathf.FloorToInt((float)(slotOffset[2]) / 4f);

		if (left == leftID && right == rightID && middle == middleID)
			return true;
		//--
		return false;
	}
}
