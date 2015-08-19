using System;


namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class Xbox360MacProfile : UnityInputDeviceProfile
	{
		public Xbox360MacProfile()
		{
			Name = "XBox 360 Controller";
			Meta = "XBox 360 Controller on Mac";
			
			SupportedPlatforms = new[] {
				"OS X"
			};
			
			JoystickNames = new[] {
				"", // Yes, really.
				"Microsoft Wireless 360 Controller",
				"Mad Catz, Inc. Mad Catz FPS Pro GamePad",
				"Mad Catz, Inc. MadCatz Call of Duty GamePad",
				"\u00A9Microsoft Corporation Controller",
				"\u00A9Microsoft Corporation Xbox Original Wired Controller"
			};
			
			LastResortRegex = "360";
			
			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "A",
					Target = InputControlType.Action1,
					Source = Button11
				},
				new InputControlMapping {
					Handle = "B",
					Target = InputControlType.Action2,
					Source = Button12
				},
				new InputControlMapping {
					Handle = "X",
					Target = InputControlType.Action3,
					Source = Button13
				},
				new InputControlMapping {
					Handle = "Y",
					Target = InputControlType.Action4,
					Source = Button14
				},
				new InputControlMapping {
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Button0
				},
				new InputControlMapping {
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Button1
				},
				new InputControlMapping {
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Button3
				},
				new InputControlMapping {
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Button2
				},
				new InputControlMapping {
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button8
				},
				new InputControlMapping {
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button9
				},
				new InputControlMapping {
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button17
				},
				new InputControlMapping {
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button18
				},
				new InputControlMapping {
					Handle = "Back",
					Target = InputControlType.Back,
					Source = Button5
				},
				new InputControlMapping {
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button4
				}
			};


			AnalogMappings = new[] {
				LeftStickLeftMapping( Analog0 ),
				LeftStickRightMapping( Analog0 ),
				LeftStickUpMapping( Analog1 ),
				LeftStickDownMapping( Analog1 ),
				
				RightStickLeftMapping( Analog2 ),
				RightStickRightMapping( Analog2 ),
				RightStickUpMapping( Analog3 ),
				RightStickDownMapping( Analog3 ),
				
				LeftTriggerMapping( Analog4 ),
				RightTriggerMapping( Analog5 ),
			};
		}
	}
	// @endcond
}

