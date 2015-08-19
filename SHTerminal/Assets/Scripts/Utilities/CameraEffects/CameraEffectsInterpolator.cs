using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Utilities.CameraEffects
{
    public enum InterpolatorState
    {
        None,

        Playing,
        Paused,
        Stopped,
    }

    public enum PlayMode
    {
        Normal,
        Reversed,
        PingPong,
        PingPongReversed
    }

    public class CameraEffectsInterpolator : MonoBehaviour
    {
        // DO NOT TOUCH! It's set as a public only because of an unity serialization policy.
        public String                Id       = String.Empty;
        public float                 Duration = 1.0f;
        public List<PostprocessData> Elements = new List<PostprocessData>();
        //

        public PlayMode DefaultPlayMode     = PlayMode.Normal;
        public float    DefaultTimeScale    = 1.0f;
        public int      DefaultLoops        = 1;

        public bool     UnscaledTime        = true;

        public int               Loops          { get; set; }
        public bool              PingPong       { get; private set; }
        public bool              Reversed       { get; private set; }
        public float             Time           { get; set; }
        public float             TimeScale      { get; set; }
        public InterpolatorState State          { get; private set; }

        public float StartTime { get; private set; }
        public float EndTime   { get; private set; }

        public bool ActivateComponentsOnStart  = true;
        public bool DeactivateComponentsOnStop = true;

        public void Awake()
        {
            CameraEffectsManager.Instance.RegisterInterpolator(this);
        }

        public void Reset()
        {
            if (State == InterpolatorState.Playing || State == InterpolatorState.Paused)
            {
                if ((!Reversed && DeactivateComponentsOnStop) ||
                    (Reversed && ActivateComponentsOnStart))
                {
                    foreach (var postprocessData in Elements) postprocessData.OnStop();
                }
            }

            State      = InterpolatorState.None;
            Loops      = 0;
            Reversed   = false;
            PingPong   = false;
            StartTime = 0.0f;
            
            Time      = 0.0f;
            TimeScale = 1.0f;

            ResetChilds();
        }

        public void Play()
        {
            Play(DefaultPlayMode, DefaultTimeScale, DefaultLoops);
        }

        public void Play(PlayMode playMode)
        {
            Play(playMode, DefaultTimeScale, DefaultLoops);
        }

        public void Play(float timeScale)
        {
            Play(DefaultPlayMode, timeScale, DefaultLoops);
        }

        public void Play(int loops)
        {
            Play(DefaultPlayMode, DefaultTimeScale, loops);
        }

        public void Play(PlayMode playMode, float timeScale)
        {
            Play(playMode, timeScale, DefaultLoops);
        }

        public void Play(PlayMode playMode, int loops)
        {
            Play(playMode, DefaultTimeScale, loops);
        }

        public void Play(float timeScale, int loops)
        {
            Play(DefaultPlayMode, timeScale, loops);
        }

        public void Play(PlayMode playMode, float timeScale, int loops)
        {
            Reset();
            SetPlayModeFlags(playMode, loops, timeScale);
            State = InterpolatorState.Playing;

            if ((Reversed && DeactivateComponentsOnStop) || 
                (!Reversed && ActivateComponentsOnStart))
            {
                foreach (var postprocessData in Elements) postprocessData.OnPlay();
            }

            UpdateChilds();
        }

        protected void SetPlayModeFlags(PlayMode playMode, int loops, float timeScale, float startTime = 0.0f)
        {
            Reversed = false;
            PingPong = false;

            switch (playMode)
            {
                case PlayMode.Reversed:
                    Reversed = true;
                    break;
                case PlayMode.PingPongReversed:
                    Reversed = true;
                    PingPong = true;
                    break;
                case PlayMode.PingPong:
                    PingPong = true;
                    break;
            }

            TimeScale = Math.Abs(timeScale);

            if (!Reversed)
            {
                StartTime = 0.0f;
                EndTime = Duration;
                Time = Math.Abs(startTime);
            }
            else
            {
                StartTime = Duration;
                EndTime = 0.0f;
                Time = Duration - Math.Abs(startTime);
            }

            Loops = (PingPong ? loops * 2 : loops);
        }

        public void InterpolatorUpdate(float dt)
        {
            if (State == InterpolatorState.Playing)
            {                
                bool finished = false;
              
                if (StartTime < EndTime)
                {
                    Time += dt * TimeScale;
                    if (Time > EndTime)
                    {
                        while (Time > Duration) Time -= Duration;
                        finished = true;
                    }
                }
                else
                {
                    Time -= dt * TimeScale;
                    if (Time < EndTime)
                    {
                        while (Time < 0.0f) Time += Duration;
                        finished = true;
                    }
                }

                if (finished)
                {
                    if (Loops > 0)
                    {
                        if (--Loops == 0)
                        {
                            Stop();
                            return;
                        }
                    }

                    if (PingPong)
                    {
                        float tmp = StartTime;
                        StartTime = EndTime;
                        EndTime = tmp;

                        if (StartTime > EndTime) Time = Math.Abs(StartTime - Time);
                        else                     Time = Math.Abs(EndTime   - Time);
                    }                    
                }
                
                UpdateChilds();
            }
        }

        public void Pause()
        {
            State = InterpolatorState.Paused;            
        }

        public void Resume()
        {
            if(State == InterpolatorState.Paused) State = InterpolatorState.Playing;
        }

        public void Stop()
        {
            Time = EndTime;
            State = InterpolatorState.Stopped;
            UpdateChilds();

            if ((!Reversed && DeactivateComponentsOnStop) ||
                (Reversed && ActivateComponentsOnStart))
            {
                foreach (var postprocessData in Elements) postprocessData.OnStop();
            }
        }
        
        public void Setup()
        {
            foreach (var postprocessData in Elements)
            {
                postprocessData.Setup(this);
            }
        }

        private void UpdateChilds()
        {
            foreach (var postprocessData in Elements) postprocessData.Update(Time);
        }

        public void ResetChilds()
        {
            foreach (var postprocessData in Elements)
            {
                if(postprocessData.Active) postprocessData.ResetValues();
            }
        }

        public void SetTimeAndUpdate(float time)
        {
            Time = time;
            UpdateChilds();
        }

#region UNITY_EDITOR
#if UNITY_EDITOR 
        public void GetEffectsInfo(StringBuilder sb)
        {
            foreach (var postprocessData in Elements)
            {
                if (postprocessData.Postprocess != null)
                {
                    sb.AppendLine(" " + postprocessData.Postprocess.ToString());

                    FieldInfo[] fieldInfos = postprocessData.Postprocess.GetType().GetFields();
                    foreach (var fieldInfo in fieldInfos)
                    {
                        if (fieldInfo.FieldType.Equals(typeof (float)))
                        {
                            sb.AppendLine(" - " + fieldInfo.Name);
                        }
                    }
                    sb.AppendLine();
                }
            }
        }
#endif
#endregion
    }

    [Serializable]
    public class PostprocessData
    {
        public Component Postprocess = null;
        public List<InterpolatorDataWrapper> Values = new List<InterpolatorDataWrapper>();
        public bool Active = true;

        private CameraEffectsInterpolator parent = null;
        private bool prevEnabledValue = false;

        public PostprocessData()
        {
            if (TypeToInterpolatorMap == null) 
                InitTypeToInterpolatorMap();
        }

        public void Update(float time)
        {
            if (!Active) return;
            foreach (var interpolatorData in Values) 
                interpolatorData.Get().UpdateValue(time);
        }

        public void OnPlay()
        {
            if (Active)
            {
                var component = Postprocess as MonoBehaviour;
                if (CameraEffectsManager.Instance != null)
                {
                    CameraEffectsManager.Instance.SetActive(component, true, parent.Id);
                }
                else
                {
                    prevEnabledValue = component.enabled;
                    component.enabled = true;
                }
            }
        }

        public void OnStop()
        {
            if (Active)
            {
                var component = Postprocess as MonoBehaviour;
                if (CameraEffectsManager.Instance != null)
                {
                    CameraEffectsManager.Instance.SetActive(component, false, parent.Id);
                }
                else
                {
                    component.enabled = prevEnabledValue;
                }
            }
        }

        public void Setup(CameraEffectsInterpolator parent)
        {
            this.parent = parent;
            List<InterpolatorDataWrapper> dataToRemove = new List<InterpolatorDataWrapper>();
            foreach (var interpolatorData in Values)
            {
                var i = interpolatorData.Get();
                if (i != null) i.UpdateFieldInfo(Postprocess);
                else
                {
                    dataToRemove.Add(interpolatorData);
                }
            }

            foreach (var interpolatorData in dataToRemove)
            {
                Values.Remove(interpolatorData);
            }
        }

        public void ResetValues()
        {
            foreach (var interpolatorData in Values)
                interpolatorData.Get().ResetValue();
        }

#region UNITY_EDITOR
#if UNITY_EDITOR
        public bool showInInspector = true;
        public int selectedValueIndex = 0;

        public String[] GetAvailableValueNames()
        {
            if (Postprocess == null) return new String[0];

            var availableValueName = new List<String>();

            FieldInfo[] fieldInfos = Postprocess.GetType().GetFields();
            foreach (var fieldInfo in fieldInfos)
            {
                if (TypeToInterpolatorMap.ContainsKey(fieldInfo.FieldType) && !IsValueAlreadyInUse(fieldInfo.Name))
                {
                    availableValueName.Add(fieldInfo.Name);
                }
            }

            PropertyInfo[] PropertyInfos = Postprocess.GetType().GetProperties();
            foreach (var propertyInfo in PropertyInfos)
            {
                if (propertyInfo.CanRead && propertyInfo.CanWrite)
                {
                    if (TypeToInterpolatorMap.ContainsKey(propertyInfo.PropertyType) &&
                        !IsValueAlreadyInUse(propertyInfo.Name))
                    {
                        availableValueName.Add(propertyInfo.Name);
                    }
                }
            }
            
            //CustomMaterialOnScreen
            CustomMaterialOnScreen customMaterialOnScreen = null;
            try { customMaterialOnScreen = Postprocess as CustomMaterialOnScreen; }
            catch (Exception) {}

            if (customMaterialOnScreen != null)
            {
                foreach (var materialProperty in customMaterialOnScreen.MaterialProperties)
                {
                    if (!IsValueAlreadyInUse(materialProperty.Get().Name) &&
                        materialProperty.Get().GetValueType() != typeof (Texture))
                    {
                        availableValueName.Add(materialProperty.Get().Name);
                    }
                }
            }

            return availableValueName.ToArray();
        }

        private bool IsValueAlreadyInUse(String name)
        {
            bool result = false;
            foreach (var interpolatorData in Values)
            {
                if (interpolatorData.Get() != null && interpolatorData.Get().ValueName.Equals(name))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void AddInterpolatorFor(String valueName)
        {
            FieldInfoWrapper fieldInfo = GetFieldInfo(Postprocess, valueName);
                
            if (fieldInfo != null)
            {
                Values.Add(
                    new InterpolatorDataWrapper(
                        (InterpolatorData)
                            TypeToInterpolatorMap[fieldInfo.GetFieldType()].GetConstructor(new Type[] {typeof (String)})
                                .Invoke(new object[] {valueName})));
            }     
        }
#endif
#endregion
        
#region STATIC
        private static Dictionary<Type, Type> TypeToInterpolatorMap = null;

        private static void InitTypeToInterpolatorMap()
        {
            TypeToInterpolatorMap = new Dictionary<Type, Type>();

            TypeToInterpolatorMap[typeof (float)] = typeof (FloatData);
            TypeToInterpolatorMap[typeof (int)] = typeof (IntData);
            TypeToInterpolatorMap[typeof (Color)] = typeof (ColorData);
            TypeToInterpolatorMap[typeof (Vector2)] = typeof (Vector2Data);
            TypeToInterpolatorMap[typeof (Vector3)] = typeof (Vector3Data);
            TypeToInterpolatorMap[typeof (Vector4)] = typeof (Vector4Data);
        }

        public static FieldInfoWrapper GetFieldInfo(Component component, String valueName)
        {
            FieldInfoWrapper fieldInfo = null;
            CustomMaterialOnScreen customMaterialOnScreen = null;

            try { customMaterialOnScreen = component as CustomMaterialOnScreen; }
            catch (Exception) { }

            if (customMaterialOnScreen != null)
            {
                var materialProperty = customMaterialOnScreen.GetMaterialProperty(valueName);
                if (materialProperty != null)
                {
                    fieldInfo = new CustomfieldInfo(materialProperty);
                }
            }
            else
            {
                FieldInfo fi = component.GetType().GetField(valueName);
                if (fi != null)
                {
                    fieldInfo = new DefaultFieldInfo(fi);    
                }
                
                PropertyInfo pi = component.GetType().GetProperty(valueName);
                if(pi != null)
                {
                    fieldInfo = new CustomPropertyInfo(pi);
                }
            }

            return fieldInfo;
        }
#endregion
    }

    [Serializable]
    public class AnimationCurveWrapper
    {
        public AnimationCurve Curve = new AnimationCurve();

        public String name = null;
        public Color color = Color.green;
        public bool advancedMode = false;
        
        public float startTime  = 0.0f;
        public float stopTime   = 1.0f;
        public float startValue = 0.0f;
        public float stopValue  = 1.0f;

        public Interpolate.EaseType easeType = Interpolate.EaseType.Linear;

        public AnimationCurveWrapper(String name, Color color)
        {
            this.name = name;
            this.color = color;
        }

        public float Evaluate(float time)
        {
            if (advancedMode) return Curve.Evaluate(time);
            else return Interpolate.Ease(easeType)(startValue, stopValue - startValue, time, stopTime - startTime);
        }

#region UNITY_EDITOR
#if UNITY_EDITOR
        public int    toggleIndex     = -1;
        public int    prevToggleIndex = -1;

        public float tempTime  = 0.0f;
        public float tempValue = 1.0f;

        public float yMin = 0.0f;
        public float yMax = 1.0f;

        public void DrawInspector(float duration)
        {
            if (!String.IsNullOrEmpty(name))
            {
                Color defaultColor = GUI.color;
                GUI.color = color;
                EditorGUILayout.LabelField(name);
                GUI.color = defaultColor;
            }

            if (EditorGUILayout.BeginFadeGroup(advancedMode ? 0.0f : 1.0f))
            {
                easeType = (Interpolate.EaseType)EditorGUILayout.EnumPopup("Ease Type", easeType);

                EditorGUILayout.BeginHorizontal();
                startTime = EditorGUILayout.FloatField("Start Time", startTime);
                startValue = EditorGUILayout.FloatField("Start Value", startValue);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                stopTime = EditorGUILayout.FloatField("Stop Time", stopTime);
                stopValue = EditorGUILayout.FloatField("Stop Value", stopValue);
                EditorGUILayout.EndHorizontal();
            }           
            
            if(EditorGUILayout.BeginFadeGroup(advancedMode ? 1.0f : 0.0f))
            {
                EditorGUILayout.BeginHorizontal();
                Curve = EditorGUILayout.CurveField(Curve, color, new Rect(0, yMin, duration, yMax - yMin), null);
                yMin = EditorGUILayout.FloatField(yMin);
                yMax = EditorGUILayout.FloatField(yMax);
                EditorGUILayout.EndHorizontal();                
            }
            EditorGUILayout.EndFadeGroup();

            if (GUILayout.Button(advancedMode ? "Simple" : "Advanced"))
            {
                advancedMode = !advancedMode;
            }
        }
#endif
#endregion
    }

    [Serializable]
    public class InterpolatorDataWrapper
    {
        public FloatData                  floatData                  = null;
        public IntData                    intData                    = null;
        public ColorData                  colorData                  = null;
        public Vector2Data                vector2Data                = null;
        public Vector3Data                vector3Data                = null;
        public Vector4Data                vector4Data                = null;

        public int typeIndex = -1;

        public InterpolatorDataWrapper(InterpolatorData data)
        {
            if (data.GetType().Equals(typeof (FloatData)))
            {
                typeIndex = 0;
                floatData = (FloatData) data;
            }
            else if (data.GetType().Equals(typeof (IntData)))
            {
                typeIndex = 1;
                intData = (IntData) data;
            }
            else if (data.GetType().Equals(typeof (ColorData)))
            {
                typeIndex = 2;
                colorData = (ColorData) data;
            }
            else if (data.GetType().Equals(typeof (Vector2Data)))
            {
                typeIndex = 3;
                vector2Data = (Vector2Data) data;
            }
            else if (data.GetType().Equals(typeof (Vector3Data)))
            {
                typeIndex = 4;
                vector3Data = (Vector3Data) data;
            }
            else if (data.GetType().Equals(typeof (Vector4Data)))
            {
                typeIndex = 5;
                vector4Data = (Vector4Data) data;
            }
        }

        public InterpolatorData Get()
        {
            switch (typeIndex)
            {
                case 0:
                    return floatData;
                case 1:
                    return intData;
                case 2:
                    return colorData;
                case 3:
                    return vector2Data;
                case 4:
                    return vector3Data;
                case 5:
                    return vector4Data;
                default:
                    return null;
            }
        }

    }

    public abstract class FieldInfoWrapper
    {
        public abstract String GetName();
        public abstract object GetValue(Component component);
        public abstract void   SetValue(Component component, object value);
        public abstract Type   GetFieldType();
    }

    public class DefaultFieldInfo : FieldInfoWrapper
    {
        public DefaultFieldInfo(FieldInfo fieldInfo)
        {
            this.fieldInfo = fieldInfo;
        }

        public FieldInfo fieldInfo = null;

        public override string GetName()
        {
            return fieldInfo != null ? fieldInfo.Name : null;
        }

        public override object GetValue(Component component)
        {
            return fieldInfo != null ? fieldInfo.GetValue(component) : null;
        }

        public override void SetValue(Component component, object value)
        {
            if (fieldInfo != null) fieldInfo.SetValue(component, value);
        }

        public override Type GetFieldType()
        {
            return fieldInfo != null ? fieldInfo.FieldType : typeof (object);
        }
    }

    public class CustomPropertyInfo : FieldInfoWrapper
    {
        public CustomPropertyInfo(PropertyInfo propertyInfo)
        {
            this.propertyInfo = propertyInfo;
        }

        public PropertyInfo propertyInfo = null;

        public override string GetName()
        {
            return propertyInfo != null ? propertyInfo.Name : null;
        }

        public override object GetValue(Component component)
        {
            return propertyInfo != null ? propertyInfo.GetValue(component,null) : null;
        }

        public override void SetValue(Component component, object value)
        {
            if (propertyInfo != null) propertyInfo.SetValue(component, value, null);
        }

        public override Type GetFieldType()
        {
            return propertyInfo != null ? propertyInfo.PropertyType : typeof(object);
        }
    }

    public class CustomfieldInfo : FieldInfoWrapper
    {
        public CustomfieldInfo(CustomMaterialOnScreen.MaterialProperty materialProperty)
        {
            this.materialProperty = materialProperty;
        }

        public CustomMaterialOnScreen.MaterialProperty materialProperty = null;

        public override string GetName()
        {
            return materialProperty != null ? materialProperty.Name : null;
        }

        public override object GetValue(Component component)
        {
            return materialProperty != null ? materialProperty.GetValue() : null;
        }

        public override void SetValue(Component component, object value)
        {
            if (materialProperty != null) materialProperty.SetValue(value);
        }

        public override Type GetFieldType()
        {
            return materialProperty != null ? materialProperty.GetValueType() : typeof(object);
        }
    }

    [Serializable]
    public abstract class InterpolatorData
    {
        public String ValueName = String.Empty;

        protected FieldInfoWrapper _fieldInfo = null;
        protected Component        _component = null;
        
        public InterpolatorData(String valueName)
        {
            ValueName = valueName;
        }

        public virtual void UpdateFieldInfo(Component component)
        {
            try
            {
                bool updateFieldInfo = false;

                _component = component;

                if (_fieldInfo == null) updateFieldInfo = true;
                else if (_fieldInfo.GetName() != ValueName) updateFieldInfo = true;


                if (updateFieldInfo)
                {
                    _fieldInfo = PostprocessData.GetFieldInfo(_component,ValueName);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(":(");
            }
        }

        public abstract void UpdateValue(float time);
        public abstract void ResetValue();

#region UNITY_EDITOR
#if UNITY_EDITOR        
        public abstract void DrawInspector(float duration);
#endif
#endregion
    }

    [Serializable]
    public abstract class InterpolatorDataGeneric<T> : InterpolatorData
    {
        protected T _prevAnimValue;

        public InterpolatorDataGeneric(String valueName)
            : base(valueName)
        {
            _prevAnimValue = GetEmptyAnimValue();
        }

        protected abstract T GetEmptyAnimValue();
        protected abstract T Evaluate(float time);
        protected abstract T GetValue(T originalValue, T currentValue, T prevValue);

        public override void UpdateValue(float time)
        {   
            if(_fieldInfo == null) return;
            
            T currentAnimValue = Evaluate(time);
            T originalValue = (T)_fieldInfo.GetValue(_component);
            _fieldInfo.SetValue(_component, GetValue(originalValue, currentAnimValue, _prevAnimValue));
            _prevAnimValue = currentAnimValue;
        }

        public override void ResetValue()
        {
            if (_fieldInfo == null) return;

            T originalValue = (T)_fieldInfo.GetValue(_component);
            _fieldInfo.SetValue(_component, GetValue(originalValue, GetEmptyAnimValue(), _prevAnimValue));
            _prevAnimValue = GetEmptyAnimValue();
        }
    }

    [Serializable]
    public class FloatData : InterpolatorDataGeneric<float>
    {
        public AnimationCurveWrapper Curve = new AnimationCurveWrapper("",Color.magenta);

        public FloatData(String valueName)
            : base(valueName) { }
        
#region UNITY_EDITOR
#if UNITY_EDITOR
        public override void DrawInspector(float duration)
        {
            Curve.DrawInspector(duration);
        }
#endif
#endregion

        protected override float GetEmptyAnimValue()
        {
            return 0.0f;
        }

        protected override float Evaluate(float time)
        {
            return Curve.Evaluate(time);
        }

        protected override float GetValue(float originalValue, float currentValue, float prevValue)
        {
            return originalValue + currentValue - prevValue;
        }
    }

    [Serializable]
    public class ColorData : InterpolatorDataGeneric<Color>
    {
        public AnimationCurveWrapper CurveR = new AnimationCurveWrapper("R:", Color.red);
        public AnimationCurveWrapper CurveG = new AnimationCurveWrapper("G:", Color.green);
        public AnimationCurveWrapper CurveB = new AnimationCurveWrapper("B:", Color.blue);
        public AnimationCurveWrapper CurveA = new AnimationCurveWrapper("A:", Color.grey);

        public ColorData(String valueName)
            : base(valueName) { }

#region UNITY_EDITOR
#if UNITY_EDITOR
        public override void DrawInspector(float duration)
        {
            CurveR.DrawInspector(duration);
            CurveG.DrawInspector(duration);
            CurveB.DrawInspector(duration);
            CurveA.DrawInspector(duration);
        }
#endif
#endregion

        protected override Color GetEmptyAnimValue()
        {
            return new Color(0.0f, 0.0f, 0.0f, 0.0f);
        }

        protected override Color Evaluate(float time)
        {
            return new Color(CurveR.Evaluate(time), CurveG.Evaluate(time), CurveB.Evaluate(time), CurveA.Evaluate(time));
        }

        protected override Color GetValue(Color originalValue, Color currentValue, Color prevValue)
        {
            return originalValue + currentValue - prevValue;
        }
    }

    [Serializable]
    public class IntData : InterpolatorDataGeneric<int>
    {
        public AnimationCurveWrapper Curve = new AnimationCurveWrapper("",Color.yellow);

        public IntData(String valueName)
            : base(valueName) { }               

#region UNITY_EDITOR
#if UNITY_EDITOR
        public override void DrawInspector(float duration)
        {
            Curve.DrawInspector(duration);
        }
#endif
#endregion

        protected override int GetEmptyAnimValue()
        {
            return 0;
        }

        protected override int Evaluate(float time)
        {
            return (int)Curve.Evaluate(time);
        }

        protected override int GetValue(int originalValue, int currentValue, int prevValue)
        {
            return originalValue + currentValue - prevValue;
        }
    }

    [Serializable]
    public class Vector2Data : InterpolatorDataGeneric<Vector2>
    {
        public AnimationCurveWrapper CurveX = new AnimationCurveWrapper("X:",Color.red);
        public AnimationCurveWrapper CurveY = new AnimationCurveWrapper("Y:",Color.green);

        public Vector2Data(String valueName)
            : base(valueName) { }

#region UNITY_EDITOR
#if UNITY_EDITOR
        public override void DrawInspector(float duration)
        {
            CurveX.DrawInspector(duration);
            CurveY.DrawInspector(duration);
        }
#endif
#endregion

        protected override Vector2 GetEmptyAnimValue()
        {
            return new Vector2(0.0f,0.0f);
        }

        protected override Vector2 Evaluate(float time)
        {
            return new Vector2(CurveX.Evaluate(time), 
                               CurveY.Evaluate(time));
        }

        protected override Vector2 GetValue(Vector2 originalValue, Vector2 currentValue, Vector2 prevValue)
        {
            return originalValue + currentValue - prevValue;
        }
    }

    [Serializable]
    public class Vector3Data : InterpolatorDataGeneric<Vector3>
    {
        public AnimationCurveWrapper CurveX = new AnimationCurveWrapper("X:",Color.green);
        public AnimationCurveWrapper CurveY = new AnimationCurveWrapper("Y:",Color.red);
        public AnimationCurveWrapper CurveZ = new AnimationCurveWrapper("Z:",Color.blue);

        public Vector3Data(String valueName)
            : base(valueName) { }

#region UNITY_EDITOR
#if UNITY_EDITOR
        public override void DrawInspector(float duration)
        {
            CurveX.DrawInspector(duration);
            CurveY.DrawInspector(duration);
            CurveZ.DrawInspector(duration);
        }
#endif
#endregion

        protected override Vector3 GetEmptyAnimValue()
        {
            return new Vector3(0.0f, 0.0f, 0.0f);
        }

        protected override Vector3 Evaluate(float time)
        {
            return new Vector3(CurveX.Evaluate(time), CurveY.Evaluate(time), CurveZ.Evaluate(time));
        }

        protected override Vector3 GetValue(Vector3 originalValue, Vector3 currentValue, Vector3 prevValue)
        {
            return originalValue + currentValue - prevValue;
        }
    }

    [Serializable]
    public class Vector4Data : InterpolatorDataGeneric<Vector4>
    {
        public AnimationCurveWrapper CurveX = new AnimationCurveWrapper("X:",Color.red);
        public AnimationCurveWrapper CurveY = new AnimationCurveWrapper("Y:",Color.green);
        public AnimationCurveWrapper CurveZ = new AnimationCurveWrapper("Z:",Color.blue);
        public AnimationCurveWrapper CurveW = new AnimationCurveWrapper("W:",Color.grey);

        public Vector4Data(String valueName)
            : base(valueName) { }

#region UNITY_EDITOR
#if UNITY_EDITOR
        public override void DrawInspector(float duration)
        {
            CurveX.DrawInspector(duration);
            CurveY.DrawInspector(duration);
            CurveZ.DrawInspector(duration);
            CurveW.DrawInspector(duration);
        }
#endif
#endregion

        protected override Vector4 GetEmptyAnimValue()
        {
            return new Vector4(0.0f, 0.0f, 0.0f, 0.0f);
        }

        protected override Vector4 Evaluate(float time)
        {
            return new Vector4(CurveX.Evaluate(time),
                               CurveY.Evaluate(time),
                               CurveZ.Evaluate(time),
                               CurveW.Evaluate(time));
        }

        protected override Vector4 GetValue(Vector4 originalValue, Vector4 currentValue, Vector4 prevValue)
        {
            return originalValue + currentValue - prevValue;
        }
    }
}