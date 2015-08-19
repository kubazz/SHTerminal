using System.Text;
using System.Linq;
using System.Reflection;
using System;
using System.Linq.Expressions;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/XULM/Custom Material On Screen")]
public class CustomMaterialOnScreen : MonoBehaviour
{

    public string Name = String.Empty;
    public Shader Shader = null;

    private Material material = null;
    public Material Material
    {
        get
        {
            if (material == null && Shader != null)
            {
                material = new Material(Shader);
                material.hideFlags = HideFlags.HideAndDontSave;
            }
            return material;
        }
    }

    public List<MaterialPropertyWrapper> MaterialProperties = new List<MaterialPropertyWrapper>();

    public MaterialProperty GetMaterialProperty(String propertyName)
    {
        return (from materialProperty in MaterialProperties where materialProperty.Get().Name.Equals(propertyName) select materialProperty.Get()).FirstOrDefault();
    }

    private void RefreshPropertyIds()
    {
        foreach (var materialProperty in MaterialProperties)
        {
            materialProperty.Get().RefreshPropertyId();
        } 
    }

    void Awake()
    {
        RefreshPropertyIds();
    }

    // Called by the camera to apply the image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination ){
        if (Material == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        foreach (var materialProperty in MaterialProperties)
	    {
	        materialProperty.Get().SetMaterialValue(Material);
	    }
		
        Graphics.Blit(source,destination,Material);
	}

#if UNITY_EDITOR
    public void SetupMaterialInfo()
    {

		material = new Material(Shader);
        int propertyCount = ShaderUtil.GetPropertyCount(Shader);

        List<MaterialPropertyWrapper> propertiesToAdd = new List<MaterialPropertyWrapper>();

        for (int i = 0; i < propertyCount; i++)
        {
            String propertyName = ShaderUtil.GetPropertyName(Shader, i);
            ShaderUtil.ShaderPropertyType propertyType = ShaderUtil.GetPropertyType(Shader, i);
            bool addAsNew = true;

            foreach (var materialProperty in MaterialProperties)
            {
                if (materialProperty.Get().Name.Equals(propertyName))
                {
                    if (MaterialPropertyTypeToShaderPropertyType(materialProperty.Get().GetType()).Equals(propertyType))
                    {
                        propertiesToAdd.Add(materialProperty);
                        addAsNew = false;
                    }
                    break;
                }
            }

            if (addAsNew)
            {
                switch (propertyType)
                {
                    case ShaderUtil.ShaderPropertyType.Float:
                        propertiesToAdd.Add(new MaterialPropertyWrapper(new MaterialFloatProperty(propertyName, 0.0f)));
                        break;
                    case ShaderUtil.ShaderPropertyType.Color:
                        propertiesToAdd.Add(new MaterialPropertyWrapper(new MaterialColorProperty(propertyName, Color.black)));
                        break;
                    case ShaderUtil.ShaderPropertyType.Vector:
                        propertiesToAdd.Add(new MaterialPropertyWrapper(new MaterialVectorProperty(propertyName, Vector4.zero)));
                        break;
                    case ShaderUtil.ShaderPropertyType.TexEnv:
                        propertiesToAdd.Add(new MaterialPropertyWrapper(new MaterialTextureProperty(propertyName, null)));
                        break;
                }
            }
        }

        MaterialProperties = propertiesToAdd;

        RefreshPropertyIds();
    }

    private static ShaderUtil.ShaderPropertyType MaterialPropertyTypeToShaderPropertyType(Type materialPropertyType)
    {
        if (materialPropertyType.Equals(typeof(MaterialFloatProperty))) return ShaderUtil.ShaderPropertyType.Float;
        if (materialPropertyType.Equals(typeof(MaterialColorProperty))) return ShaderUtil.ShaderPropertyType.Color;
        if (materialPropertyType.Equals(typeof(MaterialVectorProperty))) return ShaderUtil.ShaderPropertyType.Vector;
        if (materialPropertyType.Equals(typeof(MaterialTextureProperty))) return ShaderUtil.ShaderPropertyType.TexEnv;
        return ShaderUtil.ShaderPropertyType.Float;
    }
#endif
    
    [Serializable]
    public class MaterialPropertyWrapper
    {
        public MaterialFloatProperty    floatProperty   = null;
        public MaterialColorProperty    colorProperty   = null;
        public MaterialVectorProperty   vectorProperty  = null;
        public MaterialTextureProperty  textureProperty = null;

        public int typeIndex = -1;

        public MaterialPropertyWrapper(MaterialProperty materialProperty)
        {
            if (materialProperty.GetType().Equals(typeof (MaterialFloatProperty)))
            {
                typeIndex = 0;
                floatProperty = (MaterialFloatProperty)materialProperty;
            }
            else if (materialProperty.GetType().Equals(typeof(MaterialColorProperty)))
            {
                typeIndex = 1;
                colorProperty = (MaterialColorProperty)materialProperty;
            }
            else if (materialProperty.GetType().Equals(typeof(MaterialVectorProperty)))
            {
                typeIndex = 2;
                vectorProperty = (MaterialVectorProperty)materialProperty;
            }
            else if (materialProperty.GetType().Equals(typeof(MaterialTextureProperty)))
            {
                typeIndex = 3;
                textureProperty = (MaterialTextureProperty)materialProperty;
            }
        }

        public MaterialProperty Get()
        {
            switch (typeIndex)
            {
                case 0:
                    return floatProperty;
                case 1:
                    return colorProperty;
                case 2:
                    return vectorProperty;
                case 3:
                    return textureProperty;
                default:
                    return null;
            }
        }
    }

    [Serializable]
    public abstract class MaterialProperty
    {
        public String Name       = String.Empty;
        protected int PropertyId = -1;

        public MaterialProperty(String name)
        {
            Name       = name;
            RefreshPropertyId();
        }

        public void RefreshPropertyId()
        {
            PropertyId = UnityEngine.Shader.PropertyToID(Name);
        }

        public abstract void   SetMaterialValue(Material material);
        public abstract void   SetValue(object value);
        public abstract object GetValue();
        public abstract Type   GetValueType();      
        
#if UNITY_EDITOR
        public abstract void DrawInspector();
#endif
    }

    [Serializable]
    public abstract class MaterialGenericProperty<T> : MaterialProperty
    {
        public MaterialGenericProperty(String name, T value)
            : base(name)
        {
            Value = value;
        }

        public override object GetValue()
        {
            return Value;
        }

        public override void SetValue(object value)
        {
            Value = (T)value;
        }

        public T Value;

        public override Type GetValueType()
        {
            return typeof (T);
        }
    }
    
    [Serializable]
    public class MaterialFloatProperty : MaterialGenericProperty<float>
    {
        public MaterialFloatProperty(String name, float value)
            : base(name, value)
        {
            
        }

        public override void SetMaterialValue(Material material)
        {
            material.SetFloat(PropertyId, Value);
        }

#if UNITY_EDITOR
        public override void DrawInspector()
        {
            Value = EditorGUILayout.FloatField(Name, Value);
        }
#endif
    }

    [Serializable]
    public class MaterialColorProperty : MaterialGenericProperty<Color>
    {
        public MaterialColorProperty(String name, Color value)
            : base(name, value)
        {
            
        }

        public override void SetMaterialValue(Material material)
        {
            material.SetColor(PropertyId, Value);
        }

#if UNITY_EDITOR
        public override void DrawInspector()
        {
            Value = EditorGUILayout.ColorField(Name, Value);
        }
#endif
    }

    [Serializable]
    public class MaterialVectorProperty : MaterialGenericProperty<Vector4>
    {
        public MaterialVectorProperty(String name, Color value)
            : base(name, value)
        {
            
        }

        public override void SetMaterialValue(Material material)
        {
            material.SetVector(PropertyId, Value);
        }

#if UNITY_EDITOR
        public override void DrawInspector()
        {
            Value = EditorGUILayout.Vector4Field(Name, Value);
        }
#endif
    }

    [Serializable]
    public class MaterialTextureProperty : MaterialGenericProperty<Texture>
    {
        public MaterialTextureProperty(String name, Texture value)
            : base(name, value)
        {

        }

        public override void SetMaterialValue(Material material)
        {
            material.SetTexture(PropertyId, Value);
        }

#if UNITY_EDITOR
        public override void DrawInspector()
        {
            Value = (Texture)EditorGUILayout.ObjectField(Name, Value, typeof(Texture),true,null);
        }
#endif
    }
}