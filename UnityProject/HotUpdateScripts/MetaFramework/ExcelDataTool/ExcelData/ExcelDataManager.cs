using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework.Res;
using XFramework.Singleton;
using LitJson;

namespace XFramework.ExcelData
{
    public class ExcelDataManager : SingletonTemplate<ExcelDataManager>
    {
        public Dictionary<Type, object> dic = new Dictionary<Type, object>();

        public ExcelDataManager()
        {
            //JsonMapper.RegisterExporter((float val, JsonWriter jw) =>
            //{
            //    jw.Write(double.Parse(val.ToString()));
            //});
            //JsonMapper.RegisterImporter((double val) =>
            //{
            //    return (float)val;
            //});
        }

        /// <summary>
        /// 获取一张表
        /// </summary>
        /// <typeparam name="K">Table</typeparam>
        /// <typeparam name="V">Item</typeparam>
        /// <returns></returns>
        public K GetExcelTable<K, V>() where K : EDTableBase<V> where V : EDItemBase
        {
            Type type = typeof(K);
            if (dic.ContainsKey(type) && dic[type] is K)
                return dic[type] as K;

            var jsonName = typeof(V).Name.Replace("EDItem_", "") + ".json";
            var json = ResLoadTool.funcResLoad("Assets/HotUpdateResources/ExcelData/" + jsonName) as TextAsset;
            if (json == null) return null;

            var asset = LitJson.JsonMapper.ToObject<K>(json.text);
            if (asset != null) dic.Add(type, asset);

            return asset;
        }

        /// <summary>
        /// 获取一个数据
        /// </summary>
        /// <typeparam name="K">Table</typeparam>
        /// <typeparam name="V">Item</typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public V GetExcelItem<K, V>(int id) where K : EDTableBase<V> where V : EDItemBase
        {
            var excelData = GetExcelTable<K, V>();

            if (excelData == null) return null;

            return excelData.GetExcelItem(id);
        }

    }
}