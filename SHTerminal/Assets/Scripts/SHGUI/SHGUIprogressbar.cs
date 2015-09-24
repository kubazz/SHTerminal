public class SHGUIprogressbar: SHGUIview
{
	public float currentProgress = 0;
	int length = 0;

	//progress char - its color - background char - its color - progress front char - its color 
	public string style = "r█w░r█";

	public SHGUIview frame;
	public SHGUIview rect;

	public SHGUIview labelView;
	public SHGUIview subtitleView;

	public SHGUIprogressbar (int X, int Y, int Length, string label, string sub)
	{
		Init ();
		x = X;
		y = Y;
		rect = AddSubView(new SHGUIrect(0, 0, Length + 2, 2));
		frame = AddSubView(new SHGUIframe(0, 0, Length + 2, 2, 'z'));
		labelView = AddSubView(new SHGUItext(label, (int)((Length  + 2)/ 2 - label.Length / 2) + 1, 0, 'w'));
		subtitleView = AddSubView(new SHGUItext(sub, (int)((Length  + 2)/ 2 - sub.Length / 2) + 1, 2, 'w'));
		length = Length;
	}

	public SHGUIprogressbar SetBlinkingLabel(string label, float blinkTime){

		if (labelView != null) {
			labelView.KillInstant();
		}

		SHGUIview top = AddSubView (new SHGUIview ());
		SHGUIview b = top.AddSubView (new SHGUIblinkview (blinkTime));
		b.AddSubView(new SHGUItext(label, (int)((length  + 2)/ 2 - label.Length / 2) + 1, 0, 'w'));

		labelView = top;

		return this;
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

		if (hidden)
			return;

		base.Redraw(offx, offy);

		for (int i = 0 + (int)(length * (1-fade)); i <= length * fade; ++ i){
			SHGUI.current.SetPixelFront(style[3], 1+ x + offx + i, 1 + y + offy, style[2]);
		}
		
		int p = (int)(currentProgress * length * fade);
		bool maxed = (currentProgress >= 1f);
		for (int i = 0 + (int)(length * (1-fade)); i <= p; ++ i){
			SHGUI.current.SetPixelFront(style[1], 1 + x + offx + i, 1 + y + offy, (maxed?'w':style[0]));

			if (i == p){
				SHGUI.current.SetPixelFront(style[5], 1 + x + offx + i, 1 + y + offy, (maxed?'w':style[4]));
			}
		}
	}
			 
}


