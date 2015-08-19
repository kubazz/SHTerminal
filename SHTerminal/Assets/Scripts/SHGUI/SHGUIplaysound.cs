
using System;
using System.Collections.Generic;

public enum SHGUIsound {tick, redtick, ping, pong, redping, redpong, download, downloaded, confirm, wrong, driveloading, incomingmessage, finalscramble, restrictedpopup, messageswitch, noescape}

public class SHGUIplaysound: SHGUIview
{
	public SHGUIsound sound;

	public SHGUIplaysound (SHGUIsound sound)
	{
		this.sound = sound;
	}

	bool playedSound = false;
	public override void Update ()
	{
		base.Update ();

		if (!playedSound) {
			playedSound = true;
			SHGUI.current.PlaySound(sound);

			KillInstant();
		}
	}

}


