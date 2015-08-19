using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Utilities.CameraEffects
{
    [ExecuteInEditMode]
    internal class CameraEffectsManager : MonoBehaviour
    {
        public static CameraEffectsManager Instance { get; private set; }
        private bool DisplayDebug = true;

        private Dictionary<String, CameraEffectsInterpolator> interpolators =
            new Dictionary<string, CameraEffectsInterpolator>();

		private CameraEffectsInterpolator dummyEffect;

        private Dictionary<MonoBehaviour, Pair<string, int>> componentsActivationDictionary = new Dictionary<MonoBehaviour, Pair<string, int>>();

        void Awake() 
        {
            Instance = this;
        }

        public void RegisterInterpolator(CameraEffectsInterpolator interpolator)
        {
			if(!interpolators.ContainsKey(interpolator.Id))
           		interpolators.Add(interpolator.Id, interpolator);
        }

        public void Start()
        {
			dummyEffect = gameObject.GetComponent<CameraEffectsInterpolator> ();
			if (dummyEffect == null) {
					dummyEffect = gameObject.AddComponent<CameraEffectsInterpolator> ();
					dummyEffect.Id = "Dummy. Please ignore";
			}
            foreach (var interpolator in interpolators.Values)
            {
                interpolator.Setup();
            }   
        }

        public void ResetAll() {
            foreach (var interpolator in interpolators.Values)
            {
                interpolator.Reset();
            }
        }

        public void Update()
        {
            foreach (var interpolator in interpolators.Values)
            {
                interpolator.InterpolatorUpdate(interpolator.UnscaledTime ? UnityEngine.Time.unscaledDeltaTime : UnityEngine.Time.deltaTime);
            }

            if (DisplayDebug)
            {
                foreach (var component in componentsActivationDictionary.Keys)
                {
                    string effectName = "";
                    CustomMaterialOnScreen customMaterialOnScreen = null;
                    try
                    {
                        customMaterialOnScreen = component as CustomMaterialOnScreen;
                    }
                    catch (Exception)
                    {
                    }

                    if (customMaterialOnScreen != null) effectName = customMaterialOnScreen.Name;
                    else effectName = component.GetType().ToString();

                    //DebugText.WriteLine(">>> [" + (component.enabled ? "+" : "_") + ", " + componentsActivationDictionary[component].Second + "] " + effectName + " <" + componentsActivationDictionary[component].First + ">");
                }
            }
        }

        public CameraEffectsInterpolator this[String interpolatorName]
        {
            get
            {
                if (interpolators.ContainsKey(interpolatorName)) return interpolators[interpolatorName];

                return dummyEffect;
            }
        }

        public void SetActive(MonoBehaviour component, bool active, string interpolator)
        {
            if (!componentsActivationDictionary.ContainsKey(component))
            {
                componentsActivationDictionary.Add(component, new Pair<string,int>(interpolator, component.enabled ? 1 : 0));
            }

            var value = componentsActivationDictionary[component].Second += active ? 1 : -1;
            if (value > 0 && !component.enabled)
            {
                component.enabled = true;
            }
            else if (value <= 0 && component.enabled)
            {
                component.enabled = false;
            }
        }

        public class Pair<T, U>
        {
            public Pair(T t, U u)
            {
                First = t;
                Second = u;
            }

            public T First;
            public U Second;
        }
    }
}
