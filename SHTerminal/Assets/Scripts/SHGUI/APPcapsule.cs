
using System;
using System.Collections.Generic;
using UnityEngine;


public class APPcapsule: SHGUIrenderingtestbase
{
	public APPcapsule (): base("capsule-VR-test-by-3.14")
	{
		a = GameObject.CreatePrimitive (PrimitiveType.Capsule);
		a.transform.rotation = Quaternion.Euler (UnityEngine.Random.insideUnitSphere * 360);
		a.gameObject.GetComponent<Renderer> ().enabled = false;
        a.transform.position = new Vector3(0f, 0f, 100f);

		label = "CAPSULEVIRTUALREALITY";
	}
}


