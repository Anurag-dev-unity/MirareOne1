using UnityEditor;

public class CreateAssetBundles : Editor
{
    [MenuItem("Assets/Build Asset Bundles")]
    private static void BuildABs()
    {
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
        buildMap[0].assetBundleName = "secondchairIos";

        string str = "Assets/Prefabs/";
        string[] assets =
        {
            str + "secondchair.prefab"
        };

        buildMap[0].assetNames = assets;
        BuildPipeline.BuildAssetBundles("Assets/Abs", buildMap, BuildAssetBundleOptions.None, BuildTarget.iOS);
    }
}
