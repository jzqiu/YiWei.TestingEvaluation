using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;

namespace YiWei.Utility
{
    public class WordExportHelper
    {
        /// <summary>
        /// 调用模板生成word
        /// </summary>
        /// <param name="templateFile">模板文件</param>
        /// <param name="fileName">生成的具有模板样式的新文件</param>
        /// <param name="dic"></param>
        public static bool Create(string templateFile, string fileName, Dictionary<string, string> dic)
        {
            try
            {
                Word.Application app = new Word.Application();
                //模板文件拷贝到新文件
                File.Copy(templateFile, fileName);
                //生成documnet对象
                Word.Document doc = new Word.Document();
                object Obj_FileName = fileName;
                object Visible = false;
                object ReadOnly = false;
                object missing = System.Reflection.Missing.Value;

                //打开文件
                doc = app.Documents.Open(ref Obj_FileName, ref missing, ref ReadOnly, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref Visible,
                    ref missing, ref missing, ref missing,
                    ref missing);
                doc.Activate();

                foreach (var item in dic.Keys)
                {
                    if (app.ActiveDocument.Bookmarks.Exists(item))
                    {
                        app.ActiveDocument.Bookmarks.get_Item(item).Select();
                        app.Selection.TypeText(dic[item]);
                    }
                }

                //输出完毕后关闭doc对象
                object IsSave = true;
                doc.Close(ref IsSave, ref missing, ref missing);
                return true;
            }
            catch (Exception ex)
            {
                Log4Helper.Write("生成word异常：" + ex, LogMessageType.Error);
            }

            return false;
        }
    }
}
