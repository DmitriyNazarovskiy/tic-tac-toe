using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BundleBuilder : EditorWindow
{
	private string _bundleTitle;
	private Sprite _background, _x, _o;

	[MenuItem("Window/Bundle builder")]
	public static void ShowWindow() => GetWindow(typeof(BundleBuilder));

	void OnGUI()
	{
		GUILayout.Label("Asset bundle builder", EditorStyles.boldLabel);

		EditorGUILayout.BeginHorizontal();
		_bundleTitle = EditorGUILayout.TextField("Bundle name", _bundleTitle);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Background image:");
		_background = (Sprite) EditorGUILayout.ObjectField(_background, typeof(Sprite), false);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("X image:");
		_x = (Sprite) EditorGUILayout.ObjectField(_x, typeof(Sprite), false);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("O image:");
		_o = (Sprite) EditorGUILayout.ObjectField(_o, typeof(Sprite), false);
		EditorGUILayout.EndHorizontal();

		if (GUILayout.Button("Build"))
		{
			if (string.IsNullOrEmpty(_bundleTitle) || _background == null || _x == null || _o == null)
			{
				Debug.LogError("Please, fill all fields");

				return;
			}

			CreateBundle();
		}
	}

	private void CreateBundle()
	{
		var buildMaps = new AssetBundleBuild[1];

		buildMaps[0].assetNames = GetAssetsToPack();

		CreateAssetsBundle(buildMaps);
	}

	private string[] GetAssetsToPack()
	{
		var assetNames = new[]
		{
			AssetDatabase.GetAssetPath(_background),
			AssetDatabase.GetAssetPath(_x),
			AssetDatabase.GetAssetPath(_o)
		};

		return assetNames;
	}

	private void CreateAssetsBundle(AssetBundleBuild[] buildMaps)
	{
		try
		{
			CreateAssetsBundleByPlatform(EditorUserBuildSettings.activeBuildTarget, buildMaps);
		}
		catch (Exception e)
		{
			Debug.LogError(e.Message);
		}
	}

	private void CreateAssetsBundleByPlatform(BuildTarget buildTarget, AssetBundleBuild[] buildMap)
	{
		var folderPath = Application.streamingAssetsPath;

		if (!Directory.Exists(folderPath))
			Directory.CreateDirectory(folderPath);

		buildMap[0].assetBundleName = _bundleTitle;
		buildMap[0].assetBundleVariant = "unity3d";

		var bundleManifest = BuildPipeline.BuildAssetBundles(folderPath, buildMap,
			BuildAssetBundleOptions.DisableLoadAssetByFileName | BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension,
			buildTarget);

		if (bundleManifest == null)
		{
			Debug.LogError("There is no Asset Bundles to build");

			return;
		}

		var allBundles = bundleManifest.GetAllAssetBundlesWithVariant();

		foreach (var bundle in allBundles)
		{
			Debug.LogFormat("Bundle build {0}: {1}", buildTarget, bundle);
		}
	}
}
