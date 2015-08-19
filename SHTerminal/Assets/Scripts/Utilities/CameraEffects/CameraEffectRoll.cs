using UnityEngine;
using System.Collections;

namespace Utilities.CameraEffects
{
    [ExecuteInEditMode]
    public class CameraEffectRoll : MonoBehaviour
    {
        public float Value   = 0.0f;
        public Vector2 Range = new Vector2(0.0f,1.0f);
        public Transform CameraTransform = null;

        void Update()
        {
            CameraTransform.localRotation = Quaternion.Euler(CameraTransform.localEulerAngles.x,
                                                             CameraTransform.localEulerAngles.y,
                                                             (( ((Value + 1.0f)*0.5f) * (Range.y - Range.x)) + Range.x) * 180.0f);
        }
    }
}