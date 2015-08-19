public class SHGUItempviewprogressbar: SHGUIview
{
	public SHGUIprogressbar bar;
	public bool force = false;
	public float forceValue = 0f;

	public SHGUItempviewprogressbar (int X, int Y, int length)
	{
		Init ();

		x = X;
		y = Y;

		bar = new SHGUIprogressbar (0, 0, length, "", "");
		bar.SetFrameHidden (true);

		bar.SetStyle ("z█z░z█");

		AddSubView (bar);
	}

	public override void Update ()
	{
		if (parent != null){
			SHGUItempview t = parent as SHGUItempview;

			if (t != null){
				bar.currentProgress = t.GetCurrentProgress();
				if (bar.currentProgress < .1f)
					bar.currentProgress = -1f;
				if (bar.currentProgress > .9f)
					bar.currentProgress = 1f;
			}

			if (force){
				bar.currentProgress = forceValue;
			}
		}
		base.Update ();
	}
			 
}


