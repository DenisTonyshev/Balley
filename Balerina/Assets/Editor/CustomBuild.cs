// C# example.

using System;
using System.Diagnostics;
using NUnit.Framework;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

namespace Editor
{
    public static class CustomBuild 
    {
        [MenuItem("MyBuild/Android Build With Postprocess")]
        public static void BuildGame ()
        {
           
            var buildVersion = DateTime.Now.ToString("yy-MM-dd.HHmmss");
                    
            string[] scenes = {"Assets/mainScene.unity"};
            var buildPath = "../bin/apk/balerina_" + buildVersion + ".apk";
            var versionFile = "./Assets/scripts/App.cs";
            
            var fileLines = new List<string>(System.IO.File.ReadAllLines(versionFile));
            for (var i = 0; i < fileLines.Count; i++)
            {
                if (!fileLines[i].Contains("/*version_key*/")) continue;
                fileLines[i] = "/*version_key*/		_version = \""+buildVersion+"\";";
                break;
            }
            System.IO.File.WriteAllLines(versionFile, fileLines.ToArray());

            BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.Android, BuildOptions.None);
        
          //  var proc = new Process {StartInfo = {FileName = buildPath}};
          //  proc.Start();
        }
    }
}
