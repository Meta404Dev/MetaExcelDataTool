using ExcelDataReader;
using System;
using System.Data;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace XFramework.ExcelData.Editor
{
    public class ExcelExportToClass
    {
        public void Generate(string filePath)
        {
            try
            {
                using (FileStream file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (IExcelDataReader excelData = ExcelReaderFactory.CreateOpenXmlReader(file))
                    {
                        DataSet dataSet = excelData.AsDataSet();
                        DataTable sheet = dataSet.Tables[0];

                        string tempName = Path.GetFileNameWithoutExtension(filePath);
                        tempName = tempName.Replace("t_", "");
                        string dataItemClassName = "EDItem_" + tempName;
                        string dataTableClassName = "EDTable_" + tempName;

                        ExcelMiddleData data = new ExcelMiddleData();
                        data.Init(sheet, filePath);

                        StringBuilder sbProps = new StringBuilder();
                        for (int i = 0; i < data.realColumns.Count; i++)
                        {
                            var type = data.types[i];
                            var prop = data.props[i];
                            var noteArray = data.notes[i];

                            if (prop == "id") continue;

                            sbProps.Append("\t/// <summary>\n");
                            for (int j = 0; j < noteArray.Length; j++)
                            {
                                var note = noteArray[j];
                                sbProps.Append("\t/// " + note + "\n");
                            }
                            sbProps.Append("\t/// </summary>\n");
                            sbProps.Append(string.Format("\tpublic {0} {1};\n", type, prop));
                            sbProps.AppendLine();
                        }

                        string tempStrFile = AssetDatabase.LoadAssetAtPath<TextAsset>(ExcelConvertPathSetting.ExcelTemplateFilePath).text;
                        tempStrFile = tempStrFile.Replace("{0}", dataItemClassName);
                        tempStrFile = tempStrFile.Replace("{1}", dataTableClassName);
                        tempStrFile = tempStrFile.Replace("{2}", sbProps.ToString());

                        string targetFilePath = ExcelConvertPathSetting.GetExcelGenerateCSFilePath() + dataTableClassName + ".cs";

                        SaveFile(tempStrFile, targetFilePath);
                        return;

                        if (File.Exists(targetFilePath))
                        {
                            if (EditorUtility.DisplayDialog("警告", "检测到脚本，是否覆盖:" + targetFilePath, "确定", "取消"))
                            {
                                SaveFile(tempStrFile, targetFilePath);
                            }
                        }
                        else
                        {
                            SaveFile(tempStrFile, targetFilePath);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        private void SaveFile(string str, string filePath)
        {
            if (File.Exists(filePath)) File.Delete(filePath);

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(str);
                }
            }
            Debug.Log("class create: " + filePath);
            AssetDatabase.Refresh();
        }
    }
}