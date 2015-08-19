using System;
using System.Collections;
using UnityEngine;
using InControl;


namespace CustomProfileExample
{
	// This custom profile is enabled by adding it to the Custom Profiles list
	// on the InControlManager component, or you can attach it yourself like so:
	// InputManager.AttachDevice( new UnityInputDevice( new KeyboardAndMouseProfile() ) );
	//
	public class KeyboardAndMouseProfile : CustomInputDeviceProfile
	{
		public KeyboardAndMouseProfile()
		{
			Name = "Keyboard/Mouse";
			Meta = "A keyboard and mouse combination profile appropriate for FPS.";

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "Fire",
					Target = InputControlType.RightBumper,
					Source = MouseButton0
				},
				new InputControlMapping {
					Handle = "Throw",
					Target = InputControlType.Action2,
					Source = MouseButton1
				},
				//new InputControlMapping {
				//	Handle = "Throw",
				//	Target = InputControlType.Action2,
				//	Source = KeyCodeButton(KeyCode.E)
				//},
				new InputControlMapping {
					Handle = "Jump",
					Target = InputControlType.Action3,
					Source = KeyCodeButton( KeyCode.Space )
				},
				new InputControlMapping {
					Handle = "Esc",
					Target = InputControlType.Start,
					Source = KeyCodeButton( KeyCode.Escape )
				},
				new InputControlMapping {
					Handle = "Reset",
					Target = InputControlType.Action4,
					Source = KeyCodeButton( KeyCode.R )
				}
			};

			Sensitivity = 1f;
			LowerDeadZone = 0f;
			UpperDeadZone = 1f;

			AnalogMappings = new[] {
				new InputControlMapping {
					Handle = "Move Up",
					Target = InputControlType.LeftStickUp,
					Source = KeyCodeButton( KeyCode.W, KeyCode.UpArrow )
				},
				new InputControlMapping {
					Handle = "Move Down",
					Target = InputControlType.LeftStickDown,
					Source = KeyCodeButton( KeyCode.S, KeyCode.DownArrow )
				},
				new InputControlMapping {
					Handle = "Move Left",
					Target = InputControlType.LeftStickLeft,
					Source = KeyCodeButton( KeyCode.A, KeyCode.LeftArrow )
				},
				new InputControlMapping {
					Handle = "Move Right",
					Target = InputControlType.LeftStickRight,
					Source = KeyCodeButton( KeyCode.D, KeyCode.RightArrow )
				},
				new InputControlMapping {
					Handle = "Look Up",
					Target = InputControlType.RightStickUp,
					Source = MouseYAxis,
					SourceRange = InputRange.ZeroToPositiveInfinity,
					Raw = true,
					Scale = 0.04f
				},
				new InputControlMapping {
					Handle = "Look Down",
					Target = InputControlType.RightStickDown,
					Source = MouseYAxis,
					SourceRange = InputRange.ZeroToNegativeInfinity,
					Raw = true,
					Scale = 0.04f,
					Invert = true
				},
				new InputControlMapping {
					Handle = "Look Left",
					Target = InputControlType.RightStickLeft,
					Source = MouseXAxis,
					SourceRange = InputRange.ZeroToNegativeInfinity,
					Raw = true,
					Scale = 0.04f,
					Invert = true
				},
				new InputControlMapping {
					Handle = "Look Right",
					Target = InputControlType.RightStickRight,
					Source = MouseXAxis,
					SourceRange = InputRange.ZeroToPositiveInfinity,
					Raw = true,
					Scale = 0.04f
				},
			};
		}
	}
}

