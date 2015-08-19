using UnityEngine;
using System.Collections;

namespace Utilities.CameraEffects
{
    [ExecuteInEditMode]
    public class CameraEffectRotation : MonoBehaviour
    {
        public float AngleX = 0.0f;
        public float AngleY = 0.0f;
        public float AngleZ = 0.0f;

        public Transform CameraTransform = null;

        void Update()
        {
            CameraTransform.localRotation = Quaternion.Euler(AngleX * 180.0f, AngleY * 180.0f, AngleZ * 180.0f);
        }
    }
}