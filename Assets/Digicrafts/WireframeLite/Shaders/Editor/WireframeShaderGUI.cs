using System;
using System.IO;
using UnityEngine;
using Digicrafts.WireframePro;

namespace UnityEditor
{
	internal class WireframeShaderGUI : ShaderGUI
	{		
		public static Texture2D logo;
		public static GUIStyle logoStyle;

		public enum AlphaMode
		{
			Normal,
			Alpha,
			AlphaInvert,
			Mask,
			MaskInvert
		}

		private static class Styles
		{
			public static GUIContent wireframeBasicTitleText = new GUIContent("Basic", "");
			public static GUIContent wireframeColorTitleText = new GUIContent("Color/Texture", "");
			public static GUIContent wireframeAnimationTitleText = new GUIContent("Animation", "");

			public static GUIContent wireframeAlphaModeText = new GUIContent("Alpha Mode", "How the wireframe appear");
			public static GUIContent wireframeAlphaCutoffText = new GUIContent("Alpha Cutoff", "How the wireframe alpha cutoff");
			public static GUIContent[] wireframeAlphaModeNames = {new GUIContent("Color"),new GUIContent("Texture alpha"),new GUIContent("Invert texture alpha"),new GUIContent("Mask"),new GUIContent("Invert Mask")};
			public static GUIContent wireframeColorText = new GUIContent("Color(RGB) Trans(A)", "Wireframe Color");
			public static GUIContent wireframeTexText = new GUIContent("Texture", "Wireframe Texture");
			public static GUIContent wireframeMaskTexText = new GUIContent("Mask", "Mask for wireframe");
			public static GUIContent wireframeAAText = new GUIContent("Anti Aliasing", "Enable Anti Aliasing");
			public static GUIContent wireframeSizeText = new GUIContent("Size", "Width of the wire");
			public static GUIContent wireframeDoubleSidedText = new GUIContent("Double-sided", "Enable double-sided");
			public static GUIContent wireframeLightingText = new GUIContent("Color affect by Light", "Wireframe color affect by light/lightmap");
			public static GUIContent wireframeTexAniSpeedXText = new GUIContent("Speed(U direction)", "Speed of animated uv");
			public static GUIContent wireframeTexAniSpeedYText = new GUIContent("Speed(V direction)", "Speed of animated uv");
			public static GUIContent wireframeUVText = new GUIContent("UV Channel", "UV channel for wireframe texture");

			public static GUIContent wireframeVertexColorText = new GUIContent("Vertex Color", "");
			public static GUIContent wireframeEmissionColor = new GUIContent("Emission", "Wireframe Emission");
			public static GUIContent emissiveWarning = new GUIContent ("Emissive value is animated but the material has not been configured to support emissive. Please make sure the material itself has some amount of emissive.");
			public static GUIContent emissiveColorWarning = new GUIContent ("Ensure emissive color is non-black for emission to have effect.");
		}

		MaterialProperty wireframeAlphaMode = null;
		MaterialProperty wireframeAlphaCutoff = null;	
		MaterialProperty wireframeColor = null;
		MaterialProperty wireframeTex = null;
		MaterialProperty wireframeMaskTex = null;
		MaterialProperty wireframeSize = null;
		MaterialProperty wireframeAA = null;	
		MaterialProperty wireframeLighting = null;
		MaterialProperty wireframeDoubleSided = null;
		MaterialProperty wireframeTexAniSpeedX = null;
		MaterialProperty wireframeTexAniSpeedY = null;
		MaterialProperty wireframeUV = null;
		MaterialProperty wireframeEmissionColor = null;
		MaterialProperty wireframeVertexColor = null;

		virtual public void FindProperties (MaterialProperty[] props)
		{			
			wireframeAlphaMode = FindProperty ("_WireframeAlphaMode", props);
			wireframeAlphaCutoff = FindProperty ("_WireframeAlphaCutoff", props);
			wireframeColor = FindProperty ("_WireframeColor", props);
			wireframeTex = FindProperty ("_WireframeTex", props);
			wireframeMaskTex = FindProperty ("_WireframeMaskTex", props);
			wireframeSize = FindProperty ("_WireframeSize", props);
			wireframeAA = FindProperty ("_WireframeAA", props);
			wireframeLighting = FindProperty ("_WireframeLighting", props);
			wireframeDoubleSided = FindProperty ("_WireframeDoubleSided", props);
			wireframeTexAniSpeedX = FindProperty ("_WireframeTexAniSpeedX", props);
			wireframeTexAniSpeedY = FindProperty ("_WireframeTexAniSpeedY", props);
			wireframeUV = FindProperty ("_WireframeUV", props);
			wireframeEmissionColor = FindProperty("_WireframeEmissionColor",props,false);
			wireframeVertexColor = FindProperty("_WireframeVertexColor",props);
		}				

		public void WireframePropertiesGUI(MaterialEditor materialEditor, Material material)
		{	
			AlphaMode alphaMode = (AlphaMode)wireframeAlphaMode.floatValue;

			EditorGUILayout.Space();
			if(WireframeShaderGUI.logo==null) {				
				WireframeShaderGUI.logo=AssetDatabase.LoadAssetAtPath<Texture2D>(WireframeAssetController.editorPath+"logo.png");
			}
			if(WireframeShaderGUI.logoStyle==null) {
				WireframeShaderGUI.logoStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
				WireframeShaderGUI.logoStyle.alignment = TextAnchor.UpperCenter;
				WireframeShaderGUI.logoStyle.normal.background=WireframeShaderGUI.logo;
			}		
			GUILayout.Label("",WireframeShaderGUI.logoStyle,GUILayout.Height(50));
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("Wireframe Settings",MessageType.None);
			EditorGUILayout.Space();
			GUILayout.Label(Styles.wireframeBasicTitleText,EditorStyles.boldLabel);
			materialEditor.ShaderProperty(wireframeSize,Styles.wireframeSizeText.text);
			materialEditor.ShaderProperty(wireframeDoubleSided,Styles.wireframeDoubleSidedText.text);
			materialEditor.ShaderProperty(wireframeAA,Styles.wireframeAAText.text);
			materialEditor.ShaderProperty(wireframeLighting,Styles.wireframeLightingText.text);
			materialEditor.ShaderProperty(wireframeVertexColor,Styles.wireframeVertexColorText.text);
			EditorGUILayout.Space();
			GUILayout.Label(Styles.wireframeColorTitleText,EditorStyles.boldLabel);
			materialEditor.TexturePropertyWithHDRColor(Styles.wireframeTexText, wireframeTex, wireframeColor,new ColorPickerHDRConfig(0,2,0,2),true);
			if(alphaMode==AlphaMode.Mask||alphaMode==AlphaMode.MaskInvert){
				materialEditor.TexturePropertySingleLine(Styles.wireframeMaskTexText, wireframeMaskTex);
			}
			WireframeAlphaModePopup(materialEditor, material);
			materialEditor.ShaderProperty(wireframeAlphaCutoff,Styles.wireframeAlphaCutoffText.text);
			if(wireframeEmissionColor!=null){
				//				WireframeEmissionArea(materialEditor, material);
				materialEditor.ShaderProperty(wireframeEmissionColor,Styles.wireframeEmissionColor.text);
			}
			EditorGUILayout.Space();
			materialEditor.TextureScaleOffsetProperty(wireframeTex);
			materialEditor.ShaderProperty(wireframeUV, Styles.wireframeUVText.text);
			EditorGUILayout.Space();
			GUILayout.Label(Styles.wireframeAnimationTitleText,EditorStyles.boldLabel);
			materialEditor.ShaderProperty(wireframeTexAniSpeedX, Styles.wireframeTexAniSpeedXText.text);
			materialEditor.ShaderProperty(wireframeTexAniSpeedY, Styles.wireframeTexAniSpeedYText.text);

			// Double Sided
			if(wireframeDoubleSided.floatValue>0)
				material.SetInt("_WireframeCull",(int)UnityEngine.Rendering.CullMode.Off);
			else
				material.SetInt("_WireframeCull",(int)UnityEngine.Rendering.CullMode.Back);

		}

		void WireframeAlphaModePopup(MaterialEditor materialEditor, Material material)
		{
			EditorGUI.showMixedValue = wireframeAlphaMode.hasMixedValue;
			var mode = (AlphaMode)wireframeAlphaMode.floatValue;

			EditorGUI.BeginChangeCheck();
			mode = (AlphaMode)EditorGUILayout.Popup(Styles.wireframeAlphaModeText, (int)mode, Styles.wireframeAlphaModeNames);
			if (EditorGUI.EndChangeCheck())
			{
				materialEditor.RegisterPropertyChangeUndo("Alpha Mode");
				wireframeAlphaMode.floatValue = (float)mode;

				switch (mode)
				{
				case AlphaMode.Normal:				
					material.EnableKeyword("_WIREFRAME_ALPHA_NORMAL");
					material.DisableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA");
					material.DisableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA_INVERT");
					material.DisableKeyword("_WIREFRAME_ALPHA_MASK");
					material.DisableKeyword("_WIREFRAME_ALPHA_MASK_INVERT");
					break;
				case AlphaMode.Alpha:
					material.DisableKeyword("_WIREFRAME_ALPHA_NORMAL");
					material.EnableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA");
					material.DisableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA_INVERT");
					material.DisableKeyword("_WIREFRAME_ALPHA_MASK");
					material.DisableKeyword("_WIREFRAME_ALPHA_MASK_INVERT");
					break;
				case AlphaMode.AlphaInvert:
					material.DisableKeyword("_WIREFRAME_ALPHA_NORMAL");
					material.DisableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA");
					material.EnableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA_INVERT");
					material.DisableKeyword("_WIREFRAME_ALPHA_MASK");
					material.DisableKeyword("_WIREFRAME_ALPHA_MASK_INVERT");
					break;
				case AlphaMode.Mask:
					material.DisableKeyword("_WIREFRAME_ALPHA_NORMAL");
					material.DisableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA");
					material.DisableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA_INVERT");
					material.EnableKeyword("_WIREFRAME_ALPHA_MASK");
					material.DisableKeyword("_WIREFRAME_ALPHA_MASK_INVERT");
					break;
				case AlphaMode.MaskInvert:
					material.DisableKeyword("_WIREFRAME_ALPHA_NORMAL");
					material.DisableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA");
					material.DisableKeyword("_WIREFRAME_ALPHA_TEX_ALPHA_INVERT");
					material.DisableKeyword("_WIREFRAME_ALPHA_MASK");
					material.EnableKeyword("_WIREFRAME_ALPHA_MASK_INVERT");
					break;
				}
			}
			EditorGUI.showMixedValue = false;
		}			

		static public bool ShouldEmissionBeEnabled (Color color)
		{
			return color.maxColorComponent > (0.1f / 255.0f);
		}

		static public void SetKeyword(Material m, string keyword, bool state)
		{
			if (state)
				m.EnableKeyword (keyword);
			else
				m.DisableKeyword (keyword);
		}
	}

} // namespace UnityEditor
