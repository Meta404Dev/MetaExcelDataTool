using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace XFramework.ExcelData.Editor
{
    public class ExcelImportEditorWindow : EditorWindow
    {
        private static Vector2 windowSize = new Vector2(800, 400);

        private string pathExcelFile;
        private string pathExcelFolder;

        [MenuItem("Tools/Excel导入工具 #&%E", false, 998)]
        static void ShowEditor()
        {
            ExcelImportEditorWindow window = GetWindow<ExcelImportEditorWindow>();
            window.minSize = windowSize;
            window.maxSize = windowSize;
            window.titleContent.text = "Excel导入工具";
        }

        private void OnGUI()
        {
            #region GUIStyle 设置
            Color fontColor = new Color(179f / 255f, 179f / 255f, 179f / 255f, 1f);

            //GUIStyle gl = "Toggle";
            //gl.margin = new RectOffset(0, 100, 0, 0);

            GUIStyle titleStyle = new GUIStyle() { fontSize = 18, alignment = TextAnchor.MiddleCenter };
            titleStyle.normal.textColor = fontColor;

            GUIStyle sonTittleStyle = new GUIStyle() { fontSize = 15, alignment = TextAnchor.MiddleCenter };
            sonTittleStyle.normal.textColor = fontColor;

            GUIStyle leftStyle = new GUIStyle() { fontSize = 15, alignment = TextAnchor.MiddleLeft };
            leftStyle.normal.textColor = fontColor;

            GUIStyle littoleStyle = new GUIStyle() { fontSize = 13, alignment = TextAnchor.MiddleCenter };
            littoleStyle.normal.textColor = fontColor;
            #endregion


            GUILayout.BeginArea(new Rect(0, 0, windowSize.x, windowSize.y));
            GUILayout.BeginVertical();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("单个表格导出", titleStyle, GUILayout.Width(800));
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Excel路径", leftStyle, GUILayout.Width(80));
            pathExcelFile = GUILayout.TextField(pathExcelFile, GUILayout.Width(680));
            if (GUILayout.Button("...", GUILayout.Width(20)))
            {
                string path = string.IsNullOrEmpty(pathExcelFile) ? GetExcelFolder() : pathExcelFile;
                string folder = Path.GetDirectoryName(path);
                pathExcelFile = EditorUtility.OpenFilePanel("Open Excel file", folder, "excel files;*.xls;*.xlsx;*.xlsm");
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("导出"))
            {
                if (string.IsNullOrEmpty(pathExcelFile))
                {
                    Debug.LogError("pathExcelFile is null");
                }
                else
                {
                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    sw.Start();

                    new ExcelExportToClass().Generate(pathExcelFile);
                    new ExcelExportToAsset().Generate(pathExcelFile);

                    sw.Stop();
                    Debug.Log("generate excel complete, total time:" + sw.ElapsedMilliseconds);
                }
            }
            GUILayout.EndHorizontal();



            GUILayout.Space(50);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("文件夹导出", titleStyle, GUILayout.Width(800));
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("文件夹路径", leftStyle, GUILayout.Width(80));
            pathExcelFolder = GUILayout.TextField(pathExcelFolder, GUILayout.Width(680));
            if (GUILayout.Button("...", GUILayout.Width(20)))
            {
                string path = string.IsNullOrEmpty(pathExcelFolder) ? GetExcelFolder() : pathExcelFolder;
                string folder = Path.GetDirectoryName(path);
                pathExcelFolder = EditorUtility.OpenFolderPanel("Open Excel folder", folder, null);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("导出"))
            {
                if (string.IsNullOrEmpty(pathExcelFolder))
                {
                    Debug.LogError("pathExcelFile is null");
                }
                else
                {
                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    sw.Start();

                    new ExcelConvertRequest().GenerateAllClass(pathExcelFolder);
                    new ExcelConvertRequest().GenerateAllAsset(pathExcelFolder);

                    sw.Stop();
                    Debug.Log("generate excel complete, total time:" + sw.ElapsedMilliseconds);
                }
            }
            GUILayout.EndHorizontal();



            GUILayout.Space(50);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("导出所有表格", titleStyle, GUILayout.Width(800));
            GUILayout.EndHorizontal();
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("导出所有表格"))
            {
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                new ExcelConvertRequest().GenerateAllClass(ExcelConvertPathSetting.GetExcelPath());
                new ExcelConvertRequest().GenerateAllAsset(ExcelConvertPathSetting.GetExcelPath());

                sw.Stop();
                Debug.Log("generate excel complete, total time:" + sw.ElapsedMilliseconds);
            }
            GUILayout.EndHorizontal();


            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private string GetExcelFolder()
        {
            return ExcelConvertPathSetting.GetExcelPath();
            return Application.dataPath.Replace("/Assets", "/[TableUtils]/Table-Game/");
        }
    }
}
