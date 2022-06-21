


# MetaExcelDataTool v0.1

MetaExcelDataTool是为JEngine定制的Excel导表工具，当然你也可以通过很简单的修改，移植到自己的工程项目

# 快速使用

## 1.导入到项目

将 **UnityProject** 目录的下的所有文件全部Copy到自己的Unity工程目录下

## 2.Excel表格规则
- Excel表格路径在 **UnityProject/Excel/** 目录下，支持子目录
- Excel表需要以"t_"前缀开头，后缀格式为xlsm,xlsx
- 只取每个Excel表的第一个sheet
- 第4行为属性名，需要取有意义的名字，最好是英文。每个表必须定义ID属性。如果以‘#’号开头，表示不导出
- 第5行是注释
- 第6行是类型，目前支持的类型有："string", "int", "float", "bool"
- 第7行以下是数据
- 集合数据的分隔符顺序（从低到高）： '|'  ','  ';'   数组的类型为 int[], int[][], int[][][], string[], string[][], string[][][], 以此类推，目前仅支持3阶数组，后面需要可以再加

## 3.导表

打开Unity，点击上方的 **Tools/Excel导入工具** （快捷键：Ctrl+Shift+Alt+U）

- 支持单个表格导出，文件夹导出，所有表格导出
- 导出的asset文件目录 **UnityProject/Assets/HotUpdateResources/ExcelData/**
- 导出的代码文件目录 **UnityProject/HotUpdateScripts/Game/ExcelData/**

## 4.使用

```
var item1 = EDItem_Monster.GetById(1);
var table = EDTable_Monster.Get();

```

## 5.自定义路径

打开**Assets/3rd/ExcelDataTool/Editor/ExcelConvet/ExcelConvertPathSetting** 
所有路径都可以自定义修改

## 6.修改UI生成代码模板

模板文件在**Assets/3rd/ExcelDataTool/Editor/ExcelConvet/Template/ExcelDataClassTemplate.txt**
可以自由修改为你想要的模板


# MetaDataTool优势
- 使用简单，自动生成asset资源和代码文件

# 更新日志
## v0.1（2022.6.21）导表工具

# 支持我
- 通过支付宝请我喝一杯奶茶（支付宝账号：463056265@qq.com）
- 点击Star，你们的Star是我更新的动力

# 友情链接
[JEngine](https://github.com/JasonXuDeveloper/JEngine) 小白也能快速上手，轻松制作可以热更新的游戏
