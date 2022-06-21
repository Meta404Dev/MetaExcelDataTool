using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFramework.ExcelData
{
    public class EDTableBase<T> where T : EDItemBase
    {
        public T[] datas;

        private Dictionary<int, T> dicDatas;
        public Dictionary<int, T> DicDatas
        {
            get
            {
                if (dicDatas == null)
                {
                    dicDatas = new Dictionary<int, T>();

                    for (int i = 0; i < datas.Length; i++)
                    {
                        dicDatas.Add(datas[i].id, datas[i]);
                    }
                }

                return dicDatas;
            }
        }



        public T GetExcelItem(int id)
        {
            if (!DicDatas.ContainsKey(id))
            {
                return null;
            }

            return DicDatas[id];
        }
    }
}