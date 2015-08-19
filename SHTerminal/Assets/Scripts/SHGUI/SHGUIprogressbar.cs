public class SHGUIprogressbar: SHGUIview
{
	public float currentProgress = 0;
	int length = 0;

	//progress char - its color - background char - its color - progress front char - its color 
	public string style = "r█w░r█";

	public SHGUIview frame;
	public SHGUIview rect;

	public SHGUIprogressbar (int X, int Y, int Length, string label, string sub)
	{
		Init ();
		x = X;
		y = Y;
		rect = AddSubView(new SHGUIrect(0, 0, Length + 2, 2));
		frame = AddSubView(new SHGUIframe(0, 0, Length + 2, 2, 'z'));
		AddSubView(new SHGUItext(label, (int)((Length  + 2)/ 2 - label.Length / 2), 0, 'w'));
		AddSubView(new SHGUItext(sub, (int)((Length  + 2)/ 2 - sub.Length / 2), 2, 'w'));
		length = Length;
	}

	public SHGUIprogressbar SetStyle(string Style){
		style = Style;
		return this;
	}

	public SHGUIprogressbar SetFrameHidden(bool v){
		frame.hidden = v;
		rect.hidden = v;
		return this;
	}
	
	public override void Redraw(int offx, int offy){

		base.Redraw(offx, offy);

		if (fade < 0.999f)
			return;


		for (int i = 0; i <= length; ++ i){
			SHGUI.current.SetPixelFront(style[3], 1+ x + offx + i, 1 + y + offy, style[2]);
		}
		
		int p = (int)(currentProgress * length);
		bool maxed = (currentProgress >= 1f);
		for (int i = 0; i <= p; ++ i){
			SHGUI.current.SetPixelFront(style[1], 1 + x + offx + i, 1 + y + offy, (maxed?'w':style[0]));

			if (i == p){
				SHGUI.current.SetPixelFront(style[5], 1 + x + offx + i, 1 + y + offy, (maxed?'w':style[4]));
			}
		}
	}
			 
}


