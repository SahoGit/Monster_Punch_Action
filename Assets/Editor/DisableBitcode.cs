using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using UnityEngine;
public class DisableBitcode
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            string projPath = PBXProject.GetPBXProjectPath(path);
            PBXProject proj = new PBXProject();
            proj.ReadFromFile(projPath);

            string mainTarget = proj.GetUnityMainTargetGuid();
            string frameworkTarget = proj.GetUnityFrameworkTargetGuid();

            // Set ENABLE_BITCODE to NO for the main target
            proj.SetBuildProperty(mainTarget, "ENABLE_BITCODE", "NO");

            // Set ENABLE_BITCODE to NO for the framework target
            proj.SetBuildProperty(frameworkTarget, "ENABLE_BITCODE", "NO");


            // Save the changes
            proj.WriteToFile(projPath);

            Debug.Log("ENABLE_BITCODE set to NO for all targets.");





            // Get the path to the Info.plist file
            string plistPath = Path.Combine(path, "Info.plist");

            // Read the Info.plist file
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            // Get the root dictionary
            PlistElementDict rootDict = plist.root;

            // Set the value for the "App Uses Non-Exempt Encryption" key
            rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);
            // Set the value for the "Privacy - Location Always and When In Use Usage Description" key
            rootDict.SetString("NSLocationAlwaysAndWhenInUseUsageDescription", "This identifier will be used to deliver personalised ads to you.");
            // Set the value for the "GOOGLE_ANALYTICS_REGISTRATION_WITH_AD_NETWORK_ENABLED" key
           // rootDict.SetBoolean("GOOGLE_ANALYTICS_REGISTRATION_WITH_AD_NETWORK_ENABLED", false);
            // Add "Advertising attribution report endpoint URL" key
          //  rootDict.SetString("NSAdvertisingAttributionReportEndpoint", "https://adjust-skadnetwork.com/");



            plist.WriteToFile(plistPath);

            Debug.Log("Added multiple keys to Info.plist.");
        }
    }
}
