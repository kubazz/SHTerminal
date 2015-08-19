#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using UnityEngine.Internal;

namespace Utilities.CameraEffects
{  
    [CustomEditor(typeof (CameraEffectsInterpolator)), CanEditMultipleObjects]
    public class CameraEffectsInterpolatorEditor : Editor
    {
        private float lastFrameTime = 0.0f;
        private float deltaTime     = 0.0f;        

        public override void OnInspectorGUI()
        {
            if (lastFrameTime.Equals(0.0f))
            {
                deltaTime = 0.0f;
                lastFrameTime = Time.realtimeSinceStartup;
            }
            else
            {
                deltaTime     = Time.realtimeSinceStartup - lastFrameTime;
                lastFrameTime = Time.realtimeSinceStartup;
            }

            var CameraEffectsInterpolator = (CameraEffectsInterpolator) target;
            
            EditorGUI.BeginDisabledGroup(Application.isPlaying);                      

            EditorGUILayout.BeginHorizontal();

			
			
            if (GUILayout.Button(CameraEffectsInterpolator.State != InterpolatorState.Playing ? "Play" : "Pause"))
            {
                switch (CameraEffectsInterpolator.State)
                {
                    case InterpolatorState.Playing:
                        CameraEffectsInterpolator.Pause();
                        break;
                    case InterpolatorState.Paused:
                        CameraEffectsInterpolator.Resume();
                        break;
                    case InterpolatorState.Stopped:
                    case InterpolatorState.None:
                        CameraEffectsInterpolator.Play();
                        break;
                }
            }

			

            if (GUILayout.Button("Stop"))
            {
                CameraEffectsInterpolator.Reset();
            }

//            if (CameraEffectsInterpolator.State == InterpolatorState.None)
//            {
//                CameraEffectsInterpolator.ResetChilds();
//            }
            
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(CameraEffectsInterpolator.State != InterpolatorState.Paused);
            float newTime = EditorGUILayout.Slider(CameraEffectsInterpolator.Time, 0.0f, CameraEffectsInterpolator.Duration);
            EditorGUI.EndDisabledGroup();

            if (CameraEffectsInterpolator.State == InterpolatorState.Paused)
            {
                CameraEffectsInterpolator.SetTimeAndUpdate(newTime);
            }
            
            EditorGUI.BeginDisabledGroup(CameraEffectsInterpolator.State != InterpolatorState.None);

            ////
            //    Globals
            ////
            _showGlobals = EditorGUILayout.Foldout(_showGlobals, "Globals:");
            if (_showGlobals)
            {
                Color defaultColor = GUI.color;
                GUI.color = Color.green;
                CameraEffectsInterpolator.Id = EditorGUILayout.TextField("Id", CameraEffectsInterpolator.Id);
                GUI.color = defaultColor;

                CameraEffectsInterpolator.Duration= EditorGUILayout.FloatField("Duration(s)",
                    CameraEffectsInterpolator.Duration);

                CameraEffectsInterpolator.DefaultTimeScale = EditorGUILayout.FloatField("Time Scale",
                    CameraEffectsInterpolator.DefaultTimeScale);

                CameraEffectsInterpolator.DefaultPlayMode = (PlayMode)EditorGUILayout.EnumPopup("Play Mode",
                    CameraEffectsInterpolator.DefaultPlayMode);

                CameraEffectsInterpolator.DefaultLoops = EditorGUILayout.IntField("Loops",
                    CameraEffectsInterpolator.DefaultLoops);

                CameraEffectsInterpolator.UnscaledTime = EditorGUILayout.Toggle("Unscaled Time",
                    CameraEffectsInterpolator.UnscaledTime);

                CameraEffectsInterpolator.ActivateComponentsOnStart = EditorGUILayout.Toggle("Activate Components On Start",
                    CameraEffectsInterpolator.ActivateComponentsOnStart);

                CameraEffectsInterpolator.DeactivateComponentsOnStop = EditorGUILayout.Toggle("Deactivate Components On Stop",
                    CameraEffectsInterpolator.DeactivateComponentsOnStop);
            }
          
            ////
            //    Effects
            ////
            var effectsToRemove = new List<PostprocessData>();
            foreach (var postprocessData in CameraEffectsInterpolator.Elements)
            {
                String effectName = "<None>";
                if (postprocessData.Postprocess != null)
                {
                    CustomMaterialOnScreen customMaterialOnScreen = null;
                    try
                    {
                        customMaterialOnScreen = postprocessData.Postprocess as CustomMaterialOnScreen;
                    }
                    catch (Exception) { }

                    if (customMaterialOnScreen != null) effectName = customMaterialOnScreen.Name;
                    else                                effectName = postprocessData.Postprocess.GetType().ToString();

                    effectName += " (" + postprocessData.Postprocess.name + ")";
                }

                Color defaultColor = GUI.color;
               // GUI.backgroundColor = Color.red;


				//generate posprocess color
				//Color currentColor = new Color(0,0,0);
				//currentColor.r = 1.0f / effectName.Length;
				//currentColor.g = 1/(effectName.Length % 3);
				//currentColor.b = 1/(effectName.Length % 3);


				if(postprocessData.Active){
					GUI.color = Color.yellow;
				}else{
					GUI.color = Color.red;
				}
          
                postprocessData.showInInspector = EditorGUILayout.Foldout(postprocessData.showInInspector, effectName);
                GUI.color = defaultColor;

                if (postprocessData.showInInspector)
                {
                    ////
                    //    Component Header
                    ////
                    EditorGUILayout.BeginHorizontal();
                    postprocessData.Active = EditorGUILayout.Toggle(postprocessData.Active);                    
                    postprocessData.Postprocess = (Component)EditorGUILayout.ObjectField(postprocessData.Postprocess, typeof(Component), true);
                    
                    if (GUILayout.Button("Remove")) effectsToRemove.Add(postprocessData);
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.BeginDisabledGroup(!postprocessData.Active);

                    ////
                    //    List of available values
                    ////            
                    EditorGUILayout.BeginHorizontal();
                    String[] availableValueNames = postprocessData.GetAvailableValueNames();
                    if (availableValueNames.Count() > 0)
                    {
                        bool addValue = GUILayout.Button("Animate Value:");

                        postprocessData.selectedValueIndex = EditorGUILayout.Popup(postprocessData.selectedValueIndex,
                            availableValueNames);

                        if (addValue)
                        {
                            postprocessData.AddInterpolatorFor(availableValueNames[postprocessData.selectedValueIndex]);
                            postprocessData.selectedValueIndex = 0;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    
                    ////
                    //    Single value view
                    ////
                    var valuesToRemove = new List<InterpolatorDataWrapper>();
                    foreach (var interpolatorDataWrapper in postprocessData.Values)
                    {
                        InterpolatorData interpolatorData = interpolatorDataWrapper.Get();
                        if (interpolatorData != null)
                        {
                            EditorGUILayout.Space();
                            EditorGUILayout.Space();

                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField(interpolatorData.ValueName, EditorStyles.whiteLabel);
                            if (GUILayout.Button("Remove")) valuesToRemove.Add(interpolatorDataWrapper);
                            EditorGUILayout.EndHorizontal();

                            interpolatorData.DrawInspector(CameraEffectsInterpolator.Duration);
                        }
                    }

                    foreach (var value in valuesToRemove) postprocessData.Values.Remove(value);
                    
                    EditorGUI.EndDisabledGroup();
                }                

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
            }

            foreach (var postprocessData in effectsToRemove) CameraEffectsInterpolator.Elements.Remove(postprocessData);

            if (GUILayout.Button("Add Postprocess Component"))
                CameraEffectsInterpolator.Elements.Add(new PostprocessData());           

            // Update
            CameraEffectsInterpolator.Setup();
            if (!Application.isPlaying)
            {
                CameraEffectsInterpolator.InterpolatorUpdate(deltaTime);
            }
            EditorUtility.SetDirty(target);

            EditorGUI.EndDisabledGroup();
            EditorGUI.EndDisabledGroup();
        }

        void OnDisable()
        {
            var CameraEffectsInterpolator = (CameraEffectsInterpolator)target;
            CameraEffectsInterpolator.ResetChilds();
        }

        public override bool RequiresConstantRepaint()
        {
            return true;
        }

        private bool _showGlobals = true;
    }
}
#endif