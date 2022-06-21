using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace XFramework.ExcelData.Editor
{
    public class ExcelExportToAsset
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

                        ExcelMiddleData data = new ExcelMiddleData();
                        data.Init(sheet, filePath);

                        string tempName = Path.GetFileNameWithoutExtension(filePath);
                        tempName = tempName.Replace("t_", "");

                        //string json = LitJson.JsonMapper.ToJson(data.excelMiddleDatas);
                        string json = data.ToJson();

                        string targetFilePath = ExcelConvertPathSetting.GetExcelGenerateAssetFilePath() + tempName + ".json";

                        SaveFile(json, targetFilePath);
                        return;

                        if (File.Exists(targetFilePath))
                        {
                            if (EditorUtility.DisplayDialog("警告", "检测到Asset，是否覆盖:" + targetFilePath, "确定", "取消"))
                            {
                                SaveFile(json, targetFilePath);
                            }
                        }
                        else
                        {
                            SaveFile(json, targetFilePath);
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
            Debug.Log("asset create: " + filePath);
            AssetDatabase.Refresh();
        }



    }
}