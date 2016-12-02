using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class BuildAssetBundles
{
	// const
	public const string AssetBundlesOutputPath = "AssetBundles";

	[MenuItem ("AssetBundles/Build AssetBundles")]
	private static void Create ()
	{
		string outputPath = Path.Combine (AssetBundlesOutputPath, GetPlatformFolderForAssetBundles (EditorUserBuildSettings.activeBuildTarget));
		if (!Directory.Exists (outputPath)) {
			Directory.CreateDirectory (outputPath);
		}

		BuildPipeline.BuildAssetBundles (outputPath, 0, EditorUserBuildSettings.activeBuildTarget);
	}
	
#if UNITY_EDITOR
	public static string GetPlatformFolderForAssetBundles (BuildTarget target)
	{
		switch(target)
		{
		case BuildTarget.Android:
			return "android";
		case BuildTarget.iOS:
			return "ios";
		default:
			return null;
		}
	}
#endif
}
