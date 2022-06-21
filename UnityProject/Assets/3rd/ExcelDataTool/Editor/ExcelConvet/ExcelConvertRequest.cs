using System;
using System.Collections.Generic;
using System.IO;

namespace XFramework.ExcelData.Editor
{
    public class ExcelConvertRequest
    {

        /// <summary>
        /// 生成Excel对应的Class
        /// </summary>
        public void GenerateAllClass(string path)
        {
            var excelPaths = GetAllFilesAtPath(path);

            for (int i = 0; i < excelPaths.Count; i++)
            {
                new ExcelExportToClass().Generate(excelPaths[i]);
            }
        }


        /// <summary>
        /// 生成Excel对应的Asset
        /// </summary>
        public void GenerateAllAsset(string path)
        {
            var excelPaths = GetAllFilesAtPath(path);

            for (int i = 0; i < excelPaths.Count; i++)
            {
                new ExcelExportToAsset().Generate(excelPaths[i]);
            }
        }



        private static List<string> GetAllFilesAtPath(string path)
        {
            List<string> list = new List<string>();

            if (!Directory.Exists(path)) return null;

            foreach (string filePath in Directory.GetFiles(path))
            {
                if (filePath.Contains("~")) continue;

                var ext = Path.GetExtension(filePath);
                if (ValidExtension(ext))
                {
                    list.Add(filePath);
                }
            }

            foreach (var folderPath in Directory.GetDirectories(path))
            {
                var childList = GetAllFilesAtPath(folderPath);
                list.AddRange(childList);
            }

            return list;
        }

        private static bool ValidExtension(string extension)
        {
            string[] extensionArray = new string[] { ".xlsx", ".xlsm" };
            for (int i = 0; i < extensionArray.Length; i++)
            {
                if (extension.Equals(extensionArray[i])) return true;
            }

            return false;
        }


    }
}