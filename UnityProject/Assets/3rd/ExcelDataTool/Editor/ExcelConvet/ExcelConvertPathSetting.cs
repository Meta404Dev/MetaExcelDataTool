using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace XFramework.ExcelData.Editor
{
    public static class ExcelConvertPathSetting
    {
        /// <summary>
        /// 插件路径
        /// </summary>
        public const string PluginPath = "Assets/3rd/ExcelDataTool/";

        /// <summary>
        /// 模板路径
        /// </summary>
        public const string ExcelTemplateFilePath = PluginPath + "Editor/ExcelConvet/Template/ExcelDataClassTemplate.txt";

        /// <summary>
        /// 代码生成路径
        /// </summary>
        public const string GenerateCSFilePath = "/HotUpdateScripts/Game/ExcelData/";

        /// <summary>
        /// asset数据生成路径
        /// </summary>
        public const string ASSET_OUTPUT_PATH = "Assets/HotUpdateResources/ExcelData/";

        /// <summary>
        /// 属性名行
        /// </summary>
        public const int EXCEL_ROW_INDEX_Prop = 4 - 1;

        /// <summary>
        /// 注释行
        /// </summary>
        public const int EXCEL_ROW_INDEX_Note = 5 - 1;

        /// <summary>
        /// 类型行
        /// </summary>
        public const int EXCEL_ROW_INDEX_Type = 6 - 1;

        /// <summary>
        /// 内容行
        /// </summary>
        public const int EXCEL_ROW_INDEX_Content_Start = 7 - 1;

        /// <summary>
        /// Excel表格路径
        /// </summary>
        /// <returns></returns>
        public static string GetExcelPath()
        {
            // path: ../../design/config/
            string excelPath = Directory.CreateDirectory(Application.dataPath).Parent.FullName + "\\Excel\\";

            return excelPath;
        }

        /// <summary>
        /// Excel代码完整生成路径
        /// </summary>
        /// <returns></returns>
        public static string GetExcelGenerateCSFilePath()
        {
            string hotfixPath = Application.dataPath.Replace("/Assets", GenerateCSFilePath);
            return hotfixPath;
        }

        /// <summary>
        /// asset数据完整生成路径
        /// </summary>
        /// <returns></returns>
        public static string GetExcelGenerateAssetFilePath()
        {
            string assetGeneratePath = Application.dataPath + ASSET_OUTPUT_PATH;
            assetGeneratePath = assetGeneratePath.Replace("/AssetsAssets", "/Assets");
            return assetGeneratePath;
        }
    }
}
