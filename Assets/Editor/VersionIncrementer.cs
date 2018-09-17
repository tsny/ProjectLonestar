// Minor adjustments by Arshd and then tsny
// Version Incrementor Script for Unity by Francesco Forno (Fornetto Games)
// Inspired by http://forum.unity3d.com/threads/automatic-version-increment-script.144917/

using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

[InitializeOnLoad]
public class VersionIncrementor : IPreprocessBuildWithReport
{
    public int callbackOrder
    {
        get
        {
            return 1;
        }
    }

    [MenuItem("Build/Build Current")]
    public static void BuildCurrent()
    {
        IncreaseRevision();
        BuildPlayerWindow.ShowBuildPlayerWindow();
    }

    static void IncrementVersion(int majorIncr, int minorIncr, int buildIncr)
    {
        string[] lines = PlayerSettings.bundleVersion.Split('.');

        int MajorVersion = int.Parse(lines[0]) + majorIncr;
        int MinorVersion = int.Parse(lines[1]) + minorIncr;

        int Build = 0;

        if (lines.Length > 2)
        {
            Build = int.Parse(lines[2]) + buildIncr;
        }

        PlayerSettings.bundleVersion = MajorVersion.ToString("0") + "." +
                                        MinorVersion.ToString("0") + "." +
                                        Build.ToString("0");

        var buildVersionPath = @"buildversion.txt";

        File.WriteAllText(buildVersionPath, PlayerSettings.bundleVersion);
    }

    public static string GetLocalVersion()
    {
        return PlayerSettings.bundleVersion.ToString();
    }

    [MenuItem("Build/Increase Minor Version")]
    private static void IncreaseMinor()
    {
        IncrementVersion(0, 1, 0);
    }

    [MenuItem("Build/Increase Major Version")]
    private static void IncreaseMajor()
    {
        IncrementVersion(1, 0, 0);
    }

    [MenuItem("Build/Increase Revision")]
    private static void IncreaseRevision()
    {
        IncrementVersion(0, 0, 1);
    }

    public void OnPreprocessBuild(BuildReport report)
    {
        IncreaseRevision();
    }
}

