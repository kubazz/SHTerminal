
using System;
using System.Collections.Generic;
using UnityEngine;


public class APPcube: SHGUIrenderingtestbase
{
	public APPcube (): base("cube-VR-test-by-3.14")
	{
		a = GameObject.CreatePrimitive (PrimitiveType.Cube);
		a.transform.rotation = Quaternion.Euler (UnityEngine.Random.insideUnitSphere * 360);
		a.gameObject.GetComponent<Renderer> ().enabled = false;
		a.transform.position = new Vector3 (0f, 0f, 100f);

		label = "CUBEVIRTUALREALITY";
	}
}


