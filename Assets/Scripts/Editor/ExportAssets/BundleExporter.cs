using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
//using UnityDemo;

namespace UnityEditor
{
    public class BundleExporter
    {
        private static readonly string mBundlePath = "Assets/";
        
        private static readonly string mExportPath = Application.streamingAssetsPath;
        private static readonly BuildAssetBundleOptions mOptions =
            BuildAssetBundleOptions.CollectDependencies |
            BuildAssetBundleOptions.CompleteAssets |
            BuildAssetBundleOptions.DeterministicAssetBundle;
      
        private static readonly BuildAssetBundleOptions mDataAssetOptions =
            BuildAssetBundleOptions.CollectDependencies |
            BuildAssetBundleOptions.CompleteAssets |
            BuildAssetBundleOptions.UncompressedAssetBundle |
            BuildAssetBundleOptions.DeterministicAssetBundle;

        private static BuildTarget mCurrentTarget;

        [MenuItem("BunldeExporter/ExportBundles_Android")]
        public static void ExportBundles_Android()
        {
            mCurrentTarget = BuildTarget.Android;
            ExportSelectedBundles();
            UnityEngine.Debug.Log("ExportBundles_Android ok.");
        }


        /// <summary>
        /// 导出选中的Bundles
        /// </summary>
        private static void ExportSelectedBundles()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            var rootPath = mExportPath.Replace(Application.dataPath, "Assets");

            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            var files = (from file in selection
                         let path = AssetDatabase.GetAssetPath(file)
                         where File.Exists(path)
                         select path).ToArray();

            foreach( var file in files)
            {
                ExportBundle(file, rootPath);
            }

            watch.Stop();
        }

        private static void ExportBundle(string assetPath, string rootPath)
        {
            var relativePath = GetRelativePath(assetPath);
            var exportPath = string.Concat(rootPath, "/", relativePath);

            var exportDir = Path.GetDirectoryName(exportPath);
            if (!Directory.Exists(exportDir))
                Directory.CreateDirectory(exportDir);

            exportPath = string.Concat(exportPath, AppConfig.ASSET_FILE_EXTENSION);
            var mainAsset = AssetDatabase.LoadMainAssetAtPath(assetPath);
            BuildPipeline.BuildAssetBundle(mainAsset, null, exportPath, mOptions, mCurrentTarget);
        }

        private static string GetRelativePath(string path)
        {
            string trimPath = Regex.Replace(path, @"\s", "_");
            return trimPath.Substring(mBundlePath.Length);
        }
    }
}
