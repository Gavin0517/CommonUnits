using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Common.Units
{
    /// <summary>
    /// 读取文件
    /// </summary>
    public class DirFile
    {
        public static string ReadFile(string fullFileName)
        {
            string str2;
            try
            {
                FileStream stream = new FileStream(fullFileName, FileMode.Open);
                StreamReader reader = new StreamReader(stream);
                string str = reader.ReadToEnd();
                reader.Close();
                stream.Close();
                str2 = str;
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return str2;
        }

        public static string ReadFile(string fullPath, string fileName)
        {
            FileStream stream;
            string str2;
            try
            {
                stream = new FileStream(fullPath + "/" + fileName, FileMode.Open);
                str2 = new StreamReader(stream).ReadToEnd();
            }
            catch
            {
                try
                {
                    stream = new FileStream(fullPath + fileName, FileMode.Open);
                    str2 = new StreamReader(stream).ReadToEnd();
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
            return str2;
        }
    }
}
