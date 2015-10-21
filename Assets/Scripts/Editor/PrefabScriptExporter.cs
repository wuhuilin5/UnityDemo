using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEngine;

namespace UnityEditor
{
    public class PrefabScriptExporter
    {
        private static readonly string mBundlePath = "Assets/";
        private static readonly string mExportPath = Application.streamingAssetsPath + "/PrefabScript/";

        [MenuItem("PrefabScriptExporter/Export_Prefab_Scripts")]
        public static void ExportPrefabScripts()
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            var rootPath = mExportPath.Replace(Application.dataPath, "Assets");

            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            var selection = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets);
            var files = (from file in selection
                         let path = AssetDatabase.GetAssetPath(file)
                         where path.EndsWith(".prefab")
                         select path).ToArray();

            Debug.Log("export files:" + files.Count());

            foreach (var file in files)
            {
                ExportPrefabScript(file, rootPath);
                Debug.Log("file:" + file);
            }

            watch.Stop();
            Debug.Log("time:" + watch.ElapsedMilliseconds / 1000.0);
        }

        private static void ExportPrefabScript(string assetPath, string rootPath)
        {
            //Debug.Log("file:" + assetPath + ",rootPath:" + rootPath);

            var relativePath = GetRelativePath(assetPath);
            var exportPath = string.Concat(rootPath, "/", relativePath, ".xml");

            var exportDir = Path.GetDirectoryName(exportPath);
            if (!Directory.Exists(exportDir))
                Directory.CreateDirectory(exportDir);

            XmlDocument xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement("root");
            xmlDoc.AppendChild(root);

            var mainAsset = AssetDatabase.LoadMainAssetAtPath(assetPath);
          
            var gameObect = GameObject.Instantiate(mainAsset) as GameObject;
            if (gameObect != null)
            {
                MonoBehaviour[] behaviours = gameObect.GetComponents<MonoBehaviour>();
                foreach (var mono in behaviours)
                {
                    AddBehaviour(xmlDoc, mono);
                }
            }
            GameObject.DestroyImmediate(gameObect);

            xmlDoc.Save(exportPath);
        }

        private static void AddBehaviour(XmlDocument xmlDoc, MonoBehaviour component)
        {
            var monoNode = xmlDoc.CreateElement("MonoBehaviour");
            var root = xmlDoc.SelectSingleNode("root");
            root.AppendChild(monoNode);

            monoNode.SetAttribute("name", component.GetType().FullName);
            FieldInfo[] fieldInfos = component.GetType().GetFields();
            foreach (var fieldInfo in fieldInfos)
            {
                var field = xmlDoc.CreateElement("field");
                field.SetAttribute("name", fieldInfo.Name);
                field.SetAttribute("type", GetFieldType(fieldInfo));
                field.SetAttribute("value", GetFieldValue(fieldInfo, component));
             
                monoNode.AppendChild(field);
            }
        }

        private static string GetFieldType(FieldInfo fieldInfo )
        {
            var type = fieldInfo.FieldType.ToString();
            var index = type.LastIndexOf(".");
            if (index > 0)
            {
                type = type.Substring(index + 1);
            }

            return type;
        }

        private static string GetFieldValue(FieldInfo fieldInfo, Component component)
        {
            var value = fieldInfo.GetValue(component);
            return string.Format("{0}", value);
        }

        private static string GetRelativePath(string path)
        {
            string trimPath = Regex.Replace(path, @"\s", "_");
            return trimPath.Substring(mBundlePath.Length);
        } 
    }
}
