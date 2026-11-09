using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace ProjectBootstrap
{
    public static class CloudBuildPreparation
    {
        public static void PreExport()
        {
            // Generate CardboardXR's Core,
            // lobby, and demo scenes.
            string[] scenes = StereoBuilder.BuildScenes();

            // Verify the generated scenes exist.
            if (scenes == null || scenes.Length < 2 ||
                scenes.Any(path =>
                    AssetDatabase.LoadAssetAtPath<SceneAsset>(path) == null))
            {
                throw new BuildFailedException(
                    "CardboardXR scene generation failed.");
            }

            // Configure Unity's build scene list.
            EditorBuildSettings.scenes = scenes
                .Select(path =>
                    new EditorBuildSettingsScene(path, true))
                .ToArray();

            PlayerSettings.SetApplicationIdentifier(
                NamedBuildTarget.Android,
                "com.myvr.cardboardxr");

            AssetDatabase.SaveAssets();

            Debug.Log(
                "CardboardXR prepared " +
                scenes.Length + " scenes.");
        }
    }
}
