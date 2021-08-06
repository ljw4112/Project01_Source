using System;
using System.Collections.Generic;
using UnityEditor;

class MyEditorScript
{
    static string[] SCENES = FindEnabledEditorScenes();

    static string APP_NAME = "InChoice";
    static string TARGET_DIR = "APK_BUILD";

    [MenuItem("Build/Build Android")]
    static void PerformAndroidBuild()
    {
        string target_dir = APP_NAME + ".apk";
        PlayerSettings.keystorePass = "123456";
        PlayerSettings.keyaliasPass = "123456";
        GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTargetGroup.Android, BuildTarget.Android, BuildOptions.None);
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    static void GenericBuild(string[] scenes, string target_dir,BuildTargetGroup build_target_group, BuildTarget build_target, BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(build_target_group, build_target);
        string res = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options).ToString();

        if (res.Length < 0)
        {
            throw new Exception("BuildPlayer failure: " + res);
        }
    }
}