using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Digicrafts.WireframePro
{	
	public class WireframeSettings : ScriptableObject {		
		[SerializeField]
		public string[] modelsListData;
		public List<string> modelsNeedToImport;
	}

	[InitializeOnLoad]
	public class WireframeAssetController {

		public static WireframeSettings settings;
		public static string editorPath="Assets/Digicrafts/WireframePro/Shaders/Editor/";
		public static string settingsPath;

//		#if LITE_VERSION
//		#else
//		public static string editorPath = "Assets/Digicrafts/WireframePro/Shaders/Editor/";
//		#endif
//		public static string settingsPath = "";

//		private static bool prefsLoaded = false;

		static WireframeAssetController()
		{			
			// Get the path of the editor from asset the logo.png path
			string[] assets = AssetDatabase.FindAssets("logo");			
			foreach (string s in assets)
        	{
				string path = AssetDatabase.GUIDToAssetPath(s);
				if(path.Contains("Shaders/Editor/logo.png")){
					editorPath = Path.GetDirectoryName(path)+Path.DirectorySeparatorChar;
					break;
				}
			}
			
//			editorPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)))+Path.DirectorySeparatorChar;
			settingsPath = editorPath+"WireframeEditorSettings.asset";

//			// Load the preferences
//			if (!prefsLoaded)
//			{							
//				string path = EditorPrefs.GetString("EasyWireframeSettingsPath");
//				prefsLoaded= true;
//
//				Debug.Log("Wireframe Setting Path: " + path);	
//
//				if(path==null||path=="")
//					settingsPath=editorPath+"WireframeEditorSettings.asset";
//				else
//					settingsPath=path;
//			}
		}

		// Add preferences section named "My Preferences" to the Preferences Window
//		[PreferenceItem("Easy  Wireframe")]
//
//		public static void PreferencesGUI()
//		{
//			// Load the preferences
//			if (!prefsLoaded)
//			{				
//				string path = EditorPrefs.GetString("EasyWireframeSettingsPath");
//				prefsLoaded= true;
//
//				if(path==null||path=="")
//					settingsPath=editorPath+"WireframeEditorSettings.asset";
//				else
//					settingsPath=path;
//			}
//
//			EditorGUILayout.LabelField("Setting Path");
//			WireframeAssetController.settingsPath = EditorGUILayout.TextField(WireframeAssetController.settingsPath);
//
//			// Save the preferences
//			if (GUI.changed)
//				EditorPrefs.SetString("EasyWireframeSettingsPath", settingsPath);
//		}

		public static void CreateSettings()
		{

			if(settings==null){
				settings = (WireframeSettings)EditorGUIUtility.Load(settingsPath);
//				Debug.Log("[EasyWireframePro] init: " + settings);
				if(settings==null){										
					settings = ScriptableObject.CreateInstance<WireframeSettings>();
					settings.modelsNeedToImport = new List<string>();
				} else {
					if(settings.modelsListData==null)
						settings.modelsNeedToImport = new List<string>();
					else
						settings.modelsNeedToImport = new List<string>(settings.modelsListData);
				}					
//				Debug.Log("settings " + settings.modelsNeedToImport.Count);
			}
		}

		public static void SaveSettings()
		{			
			if(settings!=null){				
				settings.modelsListData=settings.modelsNeedToImport.ToArray();
				EditorUtility.SetDirty(settings);
				if(!AssetDatabase.Contains(settings))
					AssetDatabase.CreateAsset(settings, settingsPath);				
				AssetDatabase.SaveAssets();
			}
		}

		public static void UpdateAsset(Object go)
		{
			if(go == null){

			} else {
//				Debug.Log("UpdateAsset" + go);

				if(go.GetType() == typeof(Mesh)){
					
					Mesh m = (Mesh)go;
					WireframeShaderUtils.updateMesh(m);

				} else if(go.GetType() == typeof(GameObject)){
					
					GameObject g = (GameObject)go;

					// For child Mesh Filter
					MeshFilter[] f = g.GetComponentsInChildren<MeshFilter>();
					for(int i=0; i<f.Length;i++){
						MeshFilter filter = f[i];
						if(filter!=null && filter.sharedMesh != null){		
							WireframeShaderUtils.updateMesh(filter.sharedMesh);
						}
					}

					// For child Skinned Mesh Filter
					SkinnedMeshRenderer[] fs = g.GetComponentsInChildren<SkinnedMeshRenderer>();
					for(int i=0; i<fs.Length;i++){
						SkinnedMeshRenderer filter = fs[i];
						if(filter!=null && filter.sharedMesh != null){
							EditorUtility.SetDirty(filter.sharedMesh);
							WireframeShaderUtils.updateMesh(filter.sharedMesh);
						}
					}

				}
			}

		}

		// Note that we pass the same path, and also pass "true" to the second argument.
		[MenuItem("Assets/Add Wireframe Data",true)]
		private static bool ValidateAddWireframe()
		{			
			bool active = false;
			if(Selection.activeObject!=null&&Selection.objects.Length==1){
				if(Selection.activeObject.GetType() == typeof(Mesh) ||
					Selection.activeObject.GetType() == typeof(GameObject)){
					string hashCode = AssetDatabase.GetAssetPath(Selection.activeObject);
					WireframeAssetController.CreateSettings();
					if(!WireframeAssetController.settings.modelsNeedToImport.Contains(hashCode)){
						active = true;
					}
				}
			}
			return active;
		}
		[MenuItem("Assets/Add Wireframe Data")]
		private static void AddWireframe()
		{
			if(Selection.activeObject!=null&&Selection.objects.Length==1){
				if(Selection.activeObject.GetType() == typeof(Mesh) ||
					Selection.activeObject.GetType() == typeof(GameObject)){								
					string hashCode = AssetDatabase.GetAssetPath(Selection.activeObject);
					if(WireframeAssetController.settings && !WireframeAssetController.settings.modelsNeedToImport.Contains(hashCode)){
						WireframeAssetController.settings.modelsNeedToImport.Add(hashCode);
						WireframeAssetController.SaveSettings();
						AssetDatabase.ImportAsset(hashCode);
					}
				}
			}
		}

		[MenuItem("Assets/Remove Wireframe Data",true)]
		private static bool ValidateRemoveWireframe()
		{			
			bool active = false;
			if(Selection.activeObject!=null&&Selection.objects.Length==1){

				if(Selection.activeObject.GetType() == typeof(Mesh) ||
					Selection.activeObject.GetType() == typeof(GameObject)){
					string hashCode = AssetDatabase.GetAssetPath(Selection.activeObject);
					WireframeAssetController.CreateSettings();
					if(WireframeAssetController.settings.modelsNeedToImport.Contains(hashCode)){
						active = true;
					}
				}
			}
			return active;
		}
		[MenuItem("Assets/Remove Wireframe Data")]
		private static void RemoveWireframe()
		{
			if(Selection.activeObject!=null&&Selection.objects.Length==1){
				if(Selection.activeObject.GetType() == typeof(Mesh) ||
					Selection.activeObject.GetType() == typeof(GameObject)){								
					string hashCode = AssetDatabase.GetAssetPath(Selection.activeObject);
					if(WireframeAssetController.settings && WireframeAssetController.settings.modelsNeedToImport.Contains(hashCode)){
						WireframeAssetController.settings.modelsNeedToImport.Remove(hashCode);
						WireframeAssetController.SaveSettings();
					}
				}
			}
		}

		[MenuItem("Assets/Update All Wireframe Data")]
		private static void UpdateAllWireframe()
		{
			WireframeAssetController.CreateSettings();
			if(WireframeAssetController.settings!=null){						
				// Check if settings is null, means first time import
				foreach(string path in WireframeAssetController.settings.modelsNeedToImport){
					AssetDatabase.ImportAsset(path);
				}
			}
		}
	}

	/// <summary>
	/// Wireframe model post processor.
	/// </summary>
	public class WireframeModelPostProcessor : AssetPostprocessor {

//		static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) 
//		{
//
//			Debug.Log(" >>>> OnPostprocessAllAssets <<<<");
//			WireframeAssetController.CreateSettings();
//			foreach (string str in importedAssets)
//			{				
//				Debug.Log("assets " + str);
//				// Check if processing settings
//				if(str == WireframeAssetController.settingsPath){
//					Debug.Log("processing serttings file " + WireframeAssetController.settings);
//					if(WireframeAssetController.settings==null){						
//						// Check if settings is null, means first time import
//						WireframeAssetController.CreateSettings();
//						foreach(string path in WireframeAssetController.settings.modelsNeedToImport){
//							AssetDatabase.ImportAsset(path);
//						}
//					}
//				}
//			}
//		}

		public void OnPostprocessModel (GameObject go) {	
			
			if(go!=null){				
				string hashCode = assetImporter.assetPath;
//				Debug.Log("[EasyWireframePro] Importing Asssets: " + hashCode);
//				Debug.Log("[EasyWireframePro] settings: " + WireframeAssetController.settings);
				if(WireframeAssetController.settings && WireframeAssetController.settings.modelsNeedToImport.Contains(hashCode)){										
//					Debug.Log("update assets " + hashCode);
					WireframeAssetController.UpdateAsset(go);
				}

			}
		}	
	}
}
