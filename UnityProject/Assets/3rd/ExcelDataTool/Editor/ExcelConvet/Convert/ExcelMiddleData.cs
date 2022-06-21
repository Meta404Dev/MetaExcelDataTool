using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UnityEngine;

namespace XFramework.ExcelData.Editor
{
    public class ExcelMiddleData
    {
        public string excelPath;
        public string sheetName;
        public int realRowCount;
        public int realColumnCount;

        public List<string> types;
        public List<string> props;
        public List<string[]> notes;
        public List<int> realColumns;
        public List<SortedDictionary<string, string>> excelMiddleDatas;

        public void Init(DataTable sheet, string excelPath)
        {
            this.excelPath = excelPath;
            sheetName = sheet.TableName;
            realRowCount = sheet.Rows.Count;
            realColumnCount = sheet.Columns.Count;

            types = new List<string>();
            props = new List<string>();
            notes = new List<string[]>();
            realColumns = new List<int>();
            excelMiddleDatas = new List<SortedDictionary<string, string>>();

            bool haveIdProp = false;

            //init title
            for (int i = 0; i < realColumnCount; i++)
            {
                var prop = sheet.Rows[ExcelConvertPathSetting.EXCEL_ROW_INDEX_Prop][i].ToString();
                if (string.IsNullOrEmpty(prop)) continue;
                if (prop.StartsWith('#')) continue;
                if (prop.ToLower().Equals("id")) haveIdProp = true;

                var note = sheet.Rows[ExcelConvertPathSetting.EXCEL_ROW_INDEX_Note][i].ToString();
                var noteArray = note.Split("\n");

                var type = sheet.Rows[ExcelConvertPathSetting.EXCEL_ROW_INDEX_Type][i].ToString();
                if (string.IsNullOrEmpty(type))
                    throw new Exception("type is null:【" + prop + "】检测到" + (ExcelConvertPathSetting.EXCEL_ROW_INDEX_Type + 1) + "行类型为空（可以通知程序加上）, path: " + excelPath);
                if (!CheckTypeValid(type))
                    throw new Exception("type error:【" + type + "】检测到类型未定义, path: " + excelPath);

                //if (props.Equals("DIYData")) type = GetFormatType(type);

                types.Add(type);
                props.Add(prop);
                notes.Add(noteArray);
                realColumns.Add(i);
            }

            if (!haveIdProp) throw new Exception("id not define,未包含Id字段, path:" + excelPath);

            //convert data
            for (int i = ExcelConvertPathSetting.EXCEL_ROW_INDEX_Content_Start; i < realRowCount; i++)
            {
                if (string.IsNullOrEmpty(sheet.Rows[i][0].ToString())) continue;

                SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
                for (int j = 0; j < realColumns.Count; j++)
                {
                    var realColumn = realColumns[j];
                    var propTitle = props[j];
                    var propType = types[j];
                    var propValue = sheet.Rows[i][realColumn].ToString();

                    //error
                    if (string.IsNullOrEmpty(propValue)) throw new Exception($"值为空,title:【{propTitle}】, row:【{i + 1}】, path:" + excelPath);

                    //empty
                    if (propValue.Equals("-")) propValue = "";

                    //array
                    if (propType.Contains("[]"))
                    {
                        try
                        {
                            if (propValue.Equals("-")) propValue = "";
                            else propValue = ArrayFormatConvert(propValue, propType);
                        }
                        catch
                        {
                            Debug.LogError($"array error,数组错误 title:【{propTitle}】, row:【{i + 1}】, value:【{propValue}】, path:【{excelPath}】");
                        }
                    }

                    if (dic.ContainsKey(propTitle))
                    {
                        Debug.LogError("title repeat:【" + propTitle + "】title重复定义, path: " + excelPath);
                    }
                    else
                    {
                        dic.Add(propTitle, propValue);
                    }
                }

                excelMiddleDatas.Add(dic);
            }
        }

        /// <summary>
        /// 校验数据类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool CheckTypeValid(string type)
        {
            string[] validType = new string[] { "string", "int", "float", "bool" };
            for (int i = 0; i < validType.Length; i++)
            {
                if (validType[i].Equals(type)) return true;
            }

            //if (type.Contains("CustomED")) return true;

            if (type.Contains("[]")) return true;

            return false;
        }

        /// <summary>
        /// 将Excel表中的格式转为标准Json格式
        /// </summary>
        /// <param name="originStr"></param>
        /// <returns></returns>
        private string ArrayFormatConvert(string originStr, string originType)
        {
            var finalString = SplitWithChar(originStr, originType, ';',
                 (str) =>
                 {
                     return SplitWithChar(str, originType, ',',
                         (str) =>
                         {
                             return SplitWithChar(str, originType, '|', null);
                         });
                 });

            return finalString.ToString();

            //StringBuilder sb = new StringBuilder();

            //var strs1 = originStr.Split(',');
            //if (strs1.Length > 1) sb.Append('[');

            //for (int i = 0; i < strs1.Length; i++)
            //{
            //    var str2 = strs1[i].Split('|');
            //    if (str2.Length > 1) sb.Append('[');

            //    for (int j = 0; j < str2.Length; j++)
            //    {
            //        sb.Append(str2[j]);
            //        if (j < str2.Length - 1) sb.Append(',');
            //    }

            //    if (str2.Length > 1) sb.Append(']');
            //    if (i < strs1.Length - 1) sb.Append(',');
            //}

            //if (strs1.Length > 1) sb.Append(']');

            //return sb.ToString();
        }

        private string SplitWithChar(string originStr, string originType, char splitChar, Func<string, string> funcSplit)
        {
            StringBuilder sb = new StringBuilder();

            var strs = originStr.Split(splitChar);
            if (strs.Length > 1 || CheckTypeAndSplit(originType, splitChar)) sb.Append('[');

            for (int i = 0; i < strs.Length; i++)
            {
                string endStr = null;
                if (funcSplit == null)
                {
                    var type = originType.Replace("[]", "");
                    endStr = GetJsonTypeValue(type, strs[i]);
                }
                else
                {
                    endStr = funcSplit(strs[i]);
                }

                if (!string.IsNullOrEmpty(endStr)) sb.Append(endStr);
                if (i < strs.Length - 1) sb.Append(',');
            }

            if (strs.Length > 1 || CheckTypeAndSplit(originType, splitChar)) sb.Append(']');
            return sb.ToString();
        }

        private bool CheckTypeAndSplit(string propType, char splitChar)
        {
            if (propType.Contains("[][][]"))
            {
                if (splitChar.Equals('|')
                 || splitChar.Equals(',')
                 || splitChar.Equals(';'))
                {
                    return true;
                }
            }
            else if (propType.Contains("[][]"))
            {
                if (splitChar.Equals('|')
                 || splitChar.Equals(','))
                {
                    return true;
                }
            }
            else if (propType.Contains("[]"))
            {
                if (splitChar.Equals('|')) return true;
            }

            return false;
        }





        /// <summary>
        /// 转为json
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"datas\":[");

            for (int i = 0; i < excelMiddleDatas.Count; i++)
            {
                sb.Append("{");

                var dicData = excelMiddleDatas[i];
                for (int j = 0; j < realColumns.Count; j++)
                {
                    var propTitle = props[j];
                    var propType = types[j];
                    var propValue = dicData[propTitle];
                    propValue = GetJsonTypeValue(propType, propValue);

                    sb.Append($"\"{propTitle}\": {propValue}");
                    if (j < realColumns.Count - 1) sb.Append(",");
                }

                sb.Append("}");
                if (i < excelMiddleDatas.Count - 1) sb.Append(",");
            }

            sb.Append("]}");

            return sb.ToString();
        }

        private string GetJsonTypeValue(string type, string value)
        {
            if (type.Equals("string")) return $"\"{value}\"";

            if (string.IsNullOrEmpty(value)) return "null";
            //if (type.Contains("[]")) return $"[{value}]";

            return value;
        }
    }

}