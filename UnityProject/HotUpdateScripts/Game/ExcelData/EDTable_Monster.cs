using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XFramework.ExcelData;

/// <summary>
/// Auto Generate Class!!!
/// </summary>
[System.Serializable]
public class EDItem_Monster : EDItemBase
{
	/// <summary>
	/// 名字
	/// </summary>
	public string name;

	/// <summary>
	/// 攻击
	/// </summary>
	public int attack;

	/// <summary>
	/// 生命值
	/// </summary>
	public int health;

	/// <summary>
	/// 速度
	/// </summary>
	public float speed;

	/// <summary>
	/// 为Boss?
	/// </summary>
	public bool isBoss;

	/// <summary>
	/// 属性值
	/// </summary>
	public float[] params;


    public int ID { get { return id; } }

    public static EDItem_Monster GetById(int id)
    {
        return ExcelDataManager.Instance.GetExcelItem<EDTable_Monster, EDItem_Monster>(id);
    }
}

/// <summary>
/// Auto Generate Class!!!
/// </summary>
public class EDTable_Monster : EDTableBase<EDItem_Monster>
{
    public static EDTable_Monster Get()
    {
        return ExcelDataManager.Instance.GetExcelTable<EDTable_Monster, EDItem_Monster>();
    }

    public static EDItem_Monster GetById(int id)
    {
        return ExcelDataManager.Instance.GetExcelItem<EDTable_Monster, EDItem_Monster>(id);
    }
}