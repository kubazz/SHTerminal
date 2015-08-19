using UnityEngine;
using System.Collections;

namespace Utilities.CameraEffects
{
    [ExecuteInEditMode]
    public class CameraEffectPositionOffset : MonoBehaviour
    {
        public float OffsetX = 0.0f;
        public float OffsetY = 0.0f;
        public float OffsetZ = 0.0f;

        public Transform CameraTransform = null;

        void Update()
        {
            CameraTransform.localPosition = new Vector3(OffsetX, OffsetY, OffsetZ);
        }
    }
}