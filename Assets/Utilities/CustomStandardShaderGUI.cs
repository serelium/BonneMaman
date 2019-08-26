// Decompiled with JetBrains decompiler
// Type: UnityEditor.StandardShaderGUI
// Assembly: UnityEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 01B28312-B6F5-4E06-90F6-BE297B711E41
// Assembly location: C:\Users\Blake\sandbox\unity\test-project\Library\UnityAssemblies\UnityEditor.dll

using System;
using UnityEngine;

namespace UnityEditor
{
    internal class CustomStandardShaderGUI : ShaderGUI
    {
        private ColorPickerHDRConfig m_ColorPickerHDRConfig = new ColorPickerHDRConfig(0.0f, 99f, 1f / 99f, 3f);
        private bool m_FirstTimeApply = true;
        private MaterialProperty blendMode;
        private MaterialProperty albedoMap;
        private MaterialProperty albedoColor;
        private MaterialProperty alphaCutoff;
        private MaterialProperty specularMap;
        private MaterialProperty specularColor;
        private MaterialProperty metallicMap;
        private MaterialProperty metallic;
        private MaterialProperty smoothness;
        private MaterialProperty bumpScale;
        private MaterialProperty bumpMap;
        private MaterialProperty occlusionStrength;
        private MaterialProperty occlusionMap;
        private MaterialProperty heigtMapScale;
        private MaterialProperty heightMap;
        private MaterialProperty emissionColorForRendering;
        private MaterialProperty emissionMap;
        private MaterialProperty detailMask;
        private MaterialProperty detailAlbedoMap;
        private MaterialProperty detailNormalMapScale;
        private MaterialProperty detailNormalMap;
        private MaterialProperty uvSetSecondary;
        private MaterialProperty outlineColor;
        private MaterialProperty outlineWidth;
        private MaterialProperty intensity;
        private MaterialProperty blurRadius;
        private MaterialProperty distorColor;
        private MaterialProperty bumpAmount;
        private MaterialProperty distortTex;

        private MaterialEditor m_MaterialEditor;
        private CustomStandardShaderGUI.WorkflowMode m_WorkflowMode;

        public void FindProperties(MaterialProperty[] props)
        {
            this.blendMode = ShaderGUI.FindProperty("_Mode", props);
            this.albedoMap = ShaderGUI.FindProperty("_MainTex", props);
            this.albedoColor = ShaderGUI.FindProperty("_Color", props);
            this.alphaCutoff = ShaderGUI.FindProperty("_Cutoff", props);
            this.specularMap = ShaderGUI.FindProperty("_SpecGlossMap", props, false);
            this.specularColor = ShaderGUI.FindProperty("_SpecColor", props, false);
            this.metallicMap = ShaderGUI.FindProperty("_MetallicGlossMap", props, false);
            this.metallic = ShaderGUI.FindProperty("_Metallic", props, false);
            this.m_WorkflowMode = this.specularMap == null || this.specularColor == null ? (this.metallicMap == null || this.metallic == null ? CustomStandardShaderGUI.WorkflowMode.Dielectric : CustomStandardShaderGUI.WorkflowMode.Metallic) : CustomStandardShaderGUI.WorkflowMode.Specular;
            this.smoothness = ShaderGUI.FindProperty("_Glossiness", props);
            this.bumpScale = ShaderGUI.FindProperty("_BumpScale", props);
            this.bumpMap = ShaderGUI.FindProperty("_BumpMap", props);
            this.heigtMapScale = ShaderGUI.FindProperty("_Parallax", props);
            this.heightMap = ShaderGUI.FindProperty("_ParallaxMap", props);
            this.occlusionStrength = ShaderGUI.FindProperty("_OcclusionStrength", props);
            this.occlusionMap = ShaderGUI.FindProperty("_OcclusionMap", props);
            this.emissionColorForRendering = ShaderGUI.FindProperty("_EmissionColor", props);
            this.emissionMap = ShaderGUI.FindProperty("_EmissionMap", props);
            this.detailMask = ShaderGUI.FindProperty("_DetailMask", props);
            this.detailAlbedoMap = ShaderGUI.FindProperty("_DetailAlbedoMap", props);
            this.detailNormalMapScale = ShaderGUI.FindProperty("_DetailNormalMapScale", props);
            this.detailNormalMap = ShaderGUI.FindProperty("_DetailNormalMap", props);
            this.uvSetSecondary = ShaderGUI.FindProperty("_UVSec", props);

            this.outlineWidth = ShaderGUI.FindProperty("_OutlineWidth", props);
            this.outlineColor = ShaderGUI.FindProperty("_OutlineColor", props);
            this.intensity = ShaderGUI.FindProperty("_Intensity", props);
            this.blurRadius = ShaderGUI.FindProperty("_BlurRadius", props);
            this.distorColor = ShaderGUI.FindProperty("_DistorColor", props);
            this.bumpAmount = ShaderGUI.FindProperty("_BumpAmount", props);
            this.distortTex = ShaderGUI.FindProperty("_DistortTex", props);
        }

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
        {
            this.FindProperties(props);
            this.m_MaterialEditor = materialEditor;
            Material target = materialEditor.target as Material;
            if (this.m_FirstTimeApply)
            {
                CustomStandardShaderGUI.SetMaterialKeywords(target, this.m_WorkflowMode);
                this.m_FirstTimeApply = false;
            }
            this.ShaderPropertiesGUI(target);
        }

        public void ShaderPropertiesGUI(Material material)
        {
            EditorGUIUtility.labelWidth = 0.0f;
            EditorGUI.BeginChangeCheck();
            this.BlendModePopup();
            GUILayout.Label(CustomStandardShaderGUI.Styles.primaryMapsText, EditorStyles.boldLabel, new GUILayoutOption[0]);
            this.DoAlbedoArea(material);
            this.DoSpecularMetallicArea();
            this.m_MaterialEditor.TexturePropertySingleLine(CustomStandardShaderGUI.Styles.normalMapText, this.bumpMap, !((UnityEngine.Object)this.bumpMap.textureValue != (UnityEngine.Object)null) ? (MaterialProperty)null : this.bumpScale);
            this.m_MaterialEditor.TexturePropertySingleLine(CustomStandardShaderGUI.Styles.heightMapText, this.heightMap, !((UnityEngine.Object)this.heightMap.textureValue != (UnityEngine.Object)null) ? (MaterialProperty)null : this.heigtMapScale);
            this.m_MaterialEditor.TexturePropertySingleLine(CustomStandardShaderGUI.Styles.occlusionText, this.occlusionMap, !((UnityEngine.Object)this.occlusionMap.textureValue != (UnityEngine.Object)null) ? (MaterialProperty)null : this.occlusionStrength);
            this.DoEmissionArea(material);
            this.m_MaterialEditor.TexturePropertySingleLine(CustomStandardShaderGUI.Styles.detailMaskText, this.detailMask);
            EditorGUI.BeginChangeCheck();
            this.m_MaterialEditor.TextureScaleOffsetProperty(this.albedoMap);
            if (EditorGUI.EndChangeCheck())
                this.emissionMap.textureScaleAndOffset = this.albedoMap.textureScaleAndOffset;
            EditorGUILayout.Space();
            GUILayout.Label(CustomStandardShaderGUI.Styles.secondaryMapsText, EditorStyles.boldLabel, new GUILayoutOption[0]);
            this.m_MaterialEditor.TexturePropertySingleLine(CustomStandardShaderGUI.Styles.detailAlbedoText, this.detailAlbedoMap);
            this.m_MaterialEditor.TexturePropertySingleLine(CustomStandardShaderGUI.Styles.detailNormalMapText, this.detailNormalMap, this.detailNormalMapScale);
            this.m_MaterialEditor.TextureScaleOffsetProperty(this.detailAlbedoMap);
            this.m_MaterialEditor.ShaderProperty(this.uvSetSecondary, CustomStandardShaderGUI.Styles.uvSetLabel.text);
            // Outline properties --------------------------------------------------- //
            GUILayout.Label("Outline Properties", EditorStyles.boldLabel);
            m_MaterialEditor.ColorProperty(outlineColor, "Outline Color");
            m_MaterialEditor.FloatProperty(outlineWidth, "Outline Width");
            m_MaterialEditor.FloatProperty(intensity, "Blur Intensity");
            m_MaterialEditor.FloatProperty(blurRadius, "Blur Radius");
            m_MaterialEditor.ColorProperty(distorColor, "Distortion Color");
            m_MaterialEditor.FloatProperty(bumpAmount, "Bump Amount");
            m_MaterialEditor.TextureProperty(distortTex, "Distortion Texture");

            if (!EditorGUI.EndChangeCheck())
                return;
            foreach (Material target in this.blendMode.targets)
                CustomStandardShaderGUI.MaterialChanged(target, this.m_WorkflowMode);
        }

        internal void DetermineWorkflow(MaterialProperty[] props)
        {
            if (ShaderGUI.FindProperty("_SpecGlossMap", props, false) != null && ShaderGUI.FindProperty("_SpecColor", props, false) != null)
                this.m_WorkflowMode = CustomStandardShaderGUI.WorkflowMode.Specular;
            else if (ShaderGUI.FindProperty("_MetallicGlossMap", props, false) != null && ShaderGUI.FindProperty("_Metallic", props, false) != null)
                this.m_WorkflowMode = CustomStandardShaderGUI.WorkflowMode.Metallic;
            else
                this.m_WorkflowMode = CustomStandardShaderGUI.WorkflowMode.Dielectric;
        }

        public override void AssignNewShaderToMaterial(Material material, Shader oldShader, Shader newShader)
        {
            if (material.HasProperty("_Emission"))
                material.SetColor("_EmissionColor", material.GetColor("_Emission"));
            base.AssignNewShaderToMaterial(material, oldShader, newShader);
            if ((UnityEngine.Object)oldShader == (UnityEngine.Object)null || !oldShader.name.Contains("Legacy Shaders/"))
                return;
            CustomStandardShaderGUI.BlendMode blendMode = CustomStandardShaderGUI.BlendMode.Opaque;
            if (oldShader.name.Contains("/Transparent/Cutout/"))
                blendMode = CustomStandardShaderGUI.BlendMode.Cutout;
            else if (oldShader.name.Contains("/Transparent/"))
                blendMode = CustomStandardShaderGUI.BlendMode.Fade;
            material.SetFloat("_Mode", (float)blendMode);
            this.DetermineWorkflow(MaterialEditor.GetMaterialProperties((UnityEngine.Object[])new Material[1] { material }));
            CustomStandardShaderGUI.MaterialChanged(material, this.m_WorkflowMode);
        }

        private void BlendModePopup()
        {
            EditorGUI.showMixedValue = this.blendMode.hasMixedValue;
            CustomStandardShaderGUI.BlendMode floatValue = (CustomStandardShaderGUI.BlendMode)this.blendMode.floatValue;
            EditorGUI.BeginChangeCheck();
            CustomStandardShaderGUI.BlendMode blendMode = (CustomStandardShaderGUI.BlendMode)EditorGUILayout.Popup(CustomStandardShaderGUI.Styles.renderingMode, (int)floatValue, CustomStandardShaderGUI.Styles.blendNames, new GUILayoutOption[0]);
            if (EditorGUI.EndChangeCheck())
            {
                this.m_MaterialEditor.RegisterPropertyChangeUndo("Rendering Mode");
                this.blendMode.floatValue = (float)blendMode;
            }
            EditorGUI.showMixedValue = false;
        }

        private void DoAlbedoArea(Material material)
        {
            this.m_MaterialEditor.TexturePropertySingleLine(CustomStandardShaderGUI.Styles.albedoText, this.albedoMap, this.albedoColor);
            if ((int)material.GetFloat("_Mode") != 1)
                return;
            this.m_MaterialEditor.ShaderProperty(this.alphaCutoff, CustomStandardShaderGUI.Styles.alphaCutoffText.text, 3);
        }

        private void DoEmissionArea(Material material)
        {
            float maxColorComponent = this.emissionColorForRendering.colorValue.maxColorComponent;
            bool flag1 = !this.HasValidEmissiveKeyword(material);
            bool flag2 = (double)maxColorComponent > 0.0;
            bool flag3 = (UnityEngine.Object)this.emissionMap.textureValue != (UnityEngine.Object)null;
            this.m_MaterialEditor.TexturePropertyWithHDRColor(CustomStandardShaderGUI.Styles.emissionText, this.emissionMap, this.emissionColorForRendering, this.m_ColorPickerHDRConfig, false);
            if ((UnityEngine.Object)this.emissionMap.textureValue != (UnityEngine.Object)null && !flag3 && (double)maxColorComponent <= 0.0)
                this.emissionColorForRendering.colorValue = Color.white;
            if (flag2)
            {
                EditorGUI.BeginDisabledGroup(!CustomStandardShaderGUI.ShouldEmissionBeEnabled(this.emissionColorForRendering.colorValue));
                this.m_MaterialEditor.LightmapEmissionProperty(3);
                EditorGUI.EndDisabledGroup();
            }
            if (!flag1)
                return;
            EditorGUILayout.HelpBox(CustomStandardShaderGUI.Styles.emissiveWarning.text, MessageType.Warning);
        }

        private void DoSpecularMetallicArea()
        {
            if (this.m_WorkflowMode == CustomStandardShaderGUI.WorkflowMode.Specular)
            {
                if ((UnityEngine.Object)this.specularMap.textureValue == (UnityEngine.Object)null)
                    this.m_MaterialEditor.TexturePropertyTwoLines(CustomStandardShaderGUI.Styles.specularMapText, this.specularMap, this.specularColor, CustomStandardShaderGUI.Styles.smoothnessText, this.smoothness);
                else
                    this.m_MaterialEditor.TexturePropertySingleLine(CustomStandardShaderGUI.Styles.specularMapText, this.specularMap);
            }
            else
            {
                if (this.m_WorkflowMode != CustomStandardShaderGUI.WorkflowMode.Metallic)
                    return;
                if ((UnityEngine.Object)this.metallicMap.textureValue == (UnityEngine.Object)null)
                    this.m_MaterialEditor.TexturePropertyTwoLines(CustomStandardShaderGUI.Styles.metallicMapText, this.metallicMap, this.metallic, CustomStandardShaderGUI.Styles.smoothnessText, this.smoothness);
                else
                    this.m_MaterialEditor.TexturePropertySingleLine(CustomStandardShaderGUI.Styles.metallicMapText, this.metallicMap);
            }
        }

        public static void SetupMaterialWithBlendMode(Material material, CustomStandardShaderGUI.BlendMode blendMode)
        {
            switch (blendMode)
            {
                case CustomStandardShaderGUI.BlendMode.Opaque:
                    material.SetOverrideTag("RenderType", string.Empty);
                    material.SetInt("_SrcBlend", 1);
                    material.SetInt("_DstBlend", 0);
                    material.SetInt("_ZWrite", 1);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = -1;
                    break;
                case CustomStandardShaderGUI.BlendMode.Cutout:
                    material.SetOverrideTag("RenderType", "TransparentCutout");
                    material.SetInt("_SrcBlend", 1);
                    material.SetInt("_DstBlend", 0);
                    material.SetInt("_ZWrite", 1);
                    material.EnableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 2450;
                    break;
                case CustomStandardShaderGUI.BlendMode.Fade:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", 5);
                    material.SetInt("_DstBlend", 10);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    break;
                case CustomStandardShaderGUI.BlendMode.Transparent:
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.SetInt("_SrcBlend", 1);
                    material.SetInt("_DstBlend", 10);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    break;
            }
        }

        private static bool ShouldEmissionBeEnabled(Color color)
        {
            return (double)color.maxColorComponent > 0.000392156856833026;
        }

        private static void SetMaterialKeywords(Material material, CustomStandardShaderGUI.WorkflowMode workflowMode)
        {
            CustomStandardShaderGUI.SetKeyword(material, "_NORMALMAP", (bool)((UnityEngine.Object)material.GetTexture("_BumpMap")) || (bool)((UnityEngine.Object)material.GetTexture("_DetailNormalMap")));
            if (workflowMode == CustomStandardShaderGUI.WorkflowMode.Specular)
                CustomStandardShaderGUI.SetKeyword(material, "_SPECGLOSSMAP", (bool)((UnityEngine.Object)material.GetTexture("_SpecGlossMap")));
            else if (workflowMode == CustomStandardShaderGUI.WorkflowMode.Metallic)
                CustomStandardShaderGUI.SetKeyword(material, "_METALLICGLOSSMAP", (bool)((UnityEngine.Object)material.GetTexture("_MetallicGlossMap")));
            CustomStandardShaderGUI.SetKeyword(material, "_PARALLAXMAP", (bool)((UnityEngine.Object)material.GetTexture("_ParallaxMap")));
            CustomStandardShaderGUI.SetKeyword(material, "_DETAIL_MULX2", (bool)((UnityEngine.Object)material.GetTexture("_DetailAlbedoMap")) || (bool)((UnityEngine.Object)material.GetTexture("_DetailNormalMap")));
            bool state = CustomStandardShaderGUI.ShouldEmissionBeEnabled(material.GetColor("_EmissionColor"));
            CustomStandardShaderGUI.SetKeyword(material, "_EMISSION", state);
            MaterialGlobalIlluminationFlags illuminationFlags1 = material.globalIlluminationFlags;
            if ((illuminationFlags1 & (MaterialGlobalIlluminationFlags.RealtimeEmissive | MaterialGlobalIlluminationFlags.BakedEmissive)) == MaterialGlobalIlluminationFlags.None)
                return;
            MaterialGlobalIlluminationFlags illuminationFlags2 = illuminationFlags1 & ~MaterialGlobalIlluminationFlags.EmissiveIsBlack;
            if (!state)
                illuminationFlags2 |= MaterialGlobalIlluminationFlags.EmissiveIsBlack;
            material.globalIlluminationFlags = illuminationFlags2;
        }

        private bool HasValidEmissiveKeyword(Material material)
        {
            return material.IsKeywordEnabled("_EMISSION") || !CustomStandardShaderGUI.ShouldEmissionBeEnabled(this.emissionColorForRendering.colorValue);
        }

        private static void MaterialChanged(Material material, CustomStandardShaderGUI.WorkflowMode workflowMode)
        {
            CustomStandardShaderGUI.SetupMaterialWithBlendMode(material, (CustomStandardShaderGUI.BlendMode)material.GetFloat("_Mode"));
            CustomStandardShaderGUI.SetMaterialKeywords(material, workflowMode);
        }

        private static void SetKeyword(Material m, string keyword, bool state)
        {
            if (state)
                m.EnableKeyword(keyword);
            else
                m.DisableKeyword(keyword);
        }

        private enum WorkflowMode
        {
            Specular,
            Metallic,
            Dielectric,
        }

        public enum BlendMode
        {
            Opaque,
            Cutout,
            Fade,
            Transparent,
        }

        private static class Styles
        {
            public static GUIStyle optionsButton = (GUIStyle)"PaneOptions";
            public static GUIContent uvSetLabel = new GUIContent("UV Set");
            public static GUIContent[] uvSetOptions = new GUIContent[2] { new GUIContent("UV channel 0"), new GUIContent("UV channel 1") };
            public static string emptyTootip = string.Empty;
            public static GUIContent albedoText = new GUIContent("Albedo", "Albedo (RGB) and Transparency (A)");
            public static GUIContent alphaCutoffText = new GUIContent("Alpha Cutoff", "Threshold for alpha cutoff");
            public static GUIContent specularMapText = new GUIContent("Specular", "Specular (RGB) and Smoothness (A)");
            public static GUIContent metallicMapText = new GUIContent("Metallic", "Metallic (R) and Smoothness (A)");
            public static GUIContent smoothnessText = new GUIContent("Smoothness", string.Empty);
            public static GUIContent normalMapText = new GUIContent("Normal Map", "Normal Map");
            public static GUIContent heightMapText = new GUIContent("Height Map", "Height Map (G)");
            public static GUIContent occlusionText = new GUIContent("Occlusion", "Occlusion (G)");
            public static GUIContent emissionText = new GUIContent("Emission", "Emission (RGB)");
            public static GUIContent detailMaskText = new GUIContent("Detail Mask", "Mask for Secondary Maps (A)");
            public static GUIContent detailAlbedoText = new GUIContent("Detail Albedo x2", "Albedo (RGB) multiplied by 2");
            public static GUIContent detailNormalMapText = new GUIContent("Normal Map", "Normal Map");
            public static string whiteSpaceString = " ";
            public static string primaryMapsText = "Main Maps";
            public static string secondaryMapsText = "Secondary Maps";
            public static string renderingMode = "Rendering Mode";
            public static GUIContent emissiveWarning = new GUIContent("Emissive value is animated but the material has not been configured to support emissive. Please make sure the material itself has some amount of emissive.");
            public static GUIContent emissiveColorWarning = new GUIContent("Ensure emissive color is non-black for emission to have effect.");
            public static readonly string[] blendNames = Enum.GetNames(typeof(CustomStandardShaderGUI.BlendMode));
        }
    }
}