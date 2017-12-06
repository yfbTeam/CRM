using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace FEUtility
{
    /// <summary>
    /// 文件操作公共类
    /// </summary>
    public class FileHelper
    {
        #region 字段定义

        /// <summary>
        /// 同步标识
        /// </summary>
        private static readonly Object sync = new object();

        #endregion

        #region 检测指定目录是否存在

        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>        
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        #endregion

        #region 检测指定文件是否存在

        /// <summary>
        /// 检测指定文件是否存在,如果存在则返回true。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }

        #endregion

        #region 检测指定目录是否为空

        /// <summary>
        /// 检测指定目录是否为空
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                //判断是否存在文件
                string[] fileNames = GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }

                //判断是否存在文件夹
                string[] directoryNames = GetDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 检测指定目录中是否存在指定的文件

        /// <summary>
        /// 检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>        
        public static bool Contains(string directoryPath, string searchPattern)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, false);

                //判断指定文件是否存在
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 检测指定目录中是否存在指定的文件
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param> 
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, true);

                //判断指定文件是否存在
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 创建一个目录

        /// <summary>
        /// 创建一个目录
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        public static bool CreateDirectory(string directoryPath)
        {
            bool flag = true;
            try
            {
                //如果目录不存在则创建该目录
                if (!IsExistDirectory(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                LogService.WriteErrorLog(ex.Message);
            }
            return flag;

        }

        #endregion

        #region 创建一个文件

        #region 创建一个文件

        /// <summary>
        /// 创建一个文件
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void CreateFile(string filePath)
        {
            try
            {
                //如果文件不存在则创建该文件
                if (!IsExistFile(filePath))
                {
                    //获取文件目录路径
                    string directoryPath = GetDirectoryFromFilePath(filePath);

                    //如果文件的目录不存在，则创建目录
                    CreateDirectory(directoryPath);

                    lock (sync)
                    {
                        //创建文件                    
                        using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
                        {
                        }
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

        #region 创建一个文件,并将字节流写入文件

        /// <summary>
        /// 创建一个文件,并将字节流写入文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="buffer">二进制流数据</param>
        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                //如果文件不存在则创建该文件
                if (!IsExistFile(filePath))
                {
                    //获取文件目录路径
                    string directoryPath = GetDirectoryFromFilePath(filePath);

                    //如果文件的目录不存在，则创建目录
                    CreateDirectory(directoryPath);

                    //创建一个FileInfo对象
                    var file = new FileInfo(filePath);

                    //创建文件
                    using (FileStream fs = file.Create())
                    {
                        //写入二进制流
                        fs.Write(buffer, 0, buffer.Length);
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

        #region 创建一个文件,并将字符串写入文件

        #region 重载1

        /// <summary>
        /// 创建一个文件,并将字符串写入文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">字符串数据</param>
        public static void CreateFile(string filePath, string text)
        {
            CreateFile(filePath, text, Encoding.UTF8);
        }

        #endregion

        #region 重载2

        /// <summary>
        /// 创建一个文件,并将字符串写入文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">字符串数据</param>
        /// <param name="encoding">字符编码</param>
        public static void CreateFile(string filePath, string text, Encoding encoding)
        {
            try
            {
                //如果文件不存在则创建该文件
                if (!IsExistFile(filePath))
                {
                    //获取文件目录路径
                    string directoryPath = GetDirectoryFromFilePath(filePath);

                    //如果文件的目录不存在，则创建目录
                    CreateDirectory(directoryPath);

                    //创建文件
                    var file = new FileInfo(filePath);
                    using (FileStream stream = file.Create())
                    {
                        using (var writer = new StreamWriter(stream, encoding))
                        {
                            //写入字符串     
                            writer.Write(text);

                            //输出
                            writer.Flush();
                        }
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

        #endregion

        #endregion

        #region 打开目录

        /// <summary>
        /// 打开目录
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        public static void OpenDirectory(string directoryPath)
        {
            //检测目录是否存在
            if (!IsExistDirectory(directoryPath))
            {
                return;
            }

            //打开目录
            WebUtils.GetMapPath(directoryPath);
        }

        #endregion

        #region 打开文件

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void OpenFile(string filePath)
        {
            //检测文件是否存在
            if (!IsExistFile(filePath))
            {
                return;
            }

            //打开目录
            WebUtils.GetMapPath(filePath);
        }

        #endregion

        #region 从文件绝对路径中获取目录路径

        /// <summary>
        /// 从文件绝对路径中获取目录路径
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static string GetDirectoryFromFilePath(string filePath)
        {
            //实例化文件
            var file = new FileInfo(filePath);

            //获取目录信息
            DirectoryInfo directory = file.Directory;

            //返回目录路径
            return directory.FullName;
        }

        #endregion

        #region 获取文本文件的行数

        /// <summary>
        /// 获取文本文件的行数
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static int GetLineCount(string filePath)
        {
            //创建流读取器
            using (var reader = new StreamReader(filePath))
            {
                //行数
                int i = 0;

                while (true)
                {
                    //如果读取到内容就把行数加1
                    if (reader.ReadLine() != null)
                    {
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }

                //返回行数
                return i;
            }
        }

        #endregion

        #region 获取一个文件的长度

        /// <summary>
        /// 获取一个文件的长度,单位为Byte
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static int GetFileSize(string filePath)
        {
            //创建一个文件对象
            var fi = new FileInfo(filePath);

            //获取文件的大小
            return (int)fi.Length;
        }

        /// <summary>
        /// 获取一个文件的长度,单位为KB
        /// </summary>
        /// <param name="filePath">文件的路径</param>        
        public static double GetFileSizeByKB(string filePath)
        {
            //创建一个文件对象
            var fi = new FileInfo(filePath);

            //获取文件的大小
            return ConvertHelper.ToDouble(ConvertHelper.ToDouble(fi.Length) / 1024, 1);
        }

        /// <summary>
        /// 获取一个文件的长度,单位为MB
        /// </summary>
        /// <param name="filePath">文件的路径</param>        
        public static double GetFileSizeByMB(string filePath)
        {
            //创建一个文件对象
            var fi = new FileInfo(filePath);

            //获取文件的大小
            return ConvertHelper.ToDouble(ConvertHelper.ToDouble(fi.Length) / 1024 / 1024, 1);
        }

        #endregion

        #region
         public static long GetDirectoryLength(string dirPath)
        {
            //判断给定的路径是否存在,如果不存在则退出
            if (!Directory.Exists(dirPath))
                return 0;
            long len = 0;

            //定义一个DirectoryInfo对象
            DirectoryInfo di = new DirectoryInfo(dirPath);

            //通过GetFiles方法,获取di目录中的所有文件的大小
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }
            //获取di中所有的文件夹,并存到一个新的对象数组中,以进行递归
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }

        //也是利用递归的思想,只不过是通过File类的Exits方法来判断

        //所给路径中所对应的是否为文件

        public static long FileSize(string filePath)
        {
            long temp = 0;

            //判断当前路径所指向的是否为文件
            if (File.Exists(filePath) == false)
            {
                string[] str1 = Directory.GetFileSystemEntries(filePath);
                foreach (string s1 in str1)
                {
                    temp += FileSize(s1);
                }
            }
            else
            {
                //定义一个FileInfo对象,使之与filePath所指向的文件向关联,
                //以获取其大小
                FileInfo fileInfo = new FileInfo(filePath);
                return fileInfo.Length;
            }
            return temp;
        }
        #endregion
        #region 获取指定目录中的文件列表

        /// <summary>
        /// 获取指定目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetFileNames(string directoryPath)
        {
            //如果目录不存在，则抛出异常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            //获取文件列表
            return Directory.GetFiles(directoryPath);
        }

        /// <summary>
        /// 获取指定目录及子目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //如果目录不存在，则抛出异常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            try
            {
                if (isSearchChild)
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 获取指定目录中的子目录列表

        /// <summary>
        /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取指定目录及子目录中所有子目录列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 向文本文件写入内容

        /// <summary>
        /// 向文本文件中写入内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">写入的内容</param>        
        public static void WriteText(string filePath, string text)
        {
            WriteText(filePath, text, Encoding.UTF8);
        }

        /// <summary>
        /// 向文本文件中写入内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">写入的内容</param>
        /// <param name="encoding">编码</param>
        public static void WriteText(string filePath, string text, Encoding encoding)
        {
            //向文件写入内容
            File.WriteAllText(filePath, text, encoding);
        }

        #endregion

        #region 向文本文件的尾部追加内容

        /// <summary>
        /// 向文本文件的尾部追加内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="text">写入的内容</param>
        public static void AppendText(string filePath, string text)
        {
            //======= 追加内容 =======
            try
            {
                lock (sync)
                {
                    //创建流写入器
                    using (var writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine(text);
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

        #region 将现有文件的内容复制到新文件中

        /// <summary>
        /// 将源文件的内容复制到目标文件中
        /// </summary>
        /// <param name="sourceFilePath">源文件的绝对路径</param>
        /// <param name="destFilePath">目标文件的绝对路径</param>
        public static void CopyTo(string sourceFilePath, string destFilePath)
        {
            //有效性检测
            if (!IsExistFile(sourceFilePath))
            {
                return;
            }

            try
            {
                //检测目标文件的目录是否存在，不存在则创建
                string destDirectoryPath = GetDirectoryFromFilePath(destFilePath);
                CreateDirectory(destDirectoryPath);

                //复制文件
                var file = new FileInfo(sourceFilePath);
                file.CopyTo(destFilePath, true);
            }
            catch
            {
            }
        }

        #endregion
        public static void CopyFolder(string strFromPath, string strToPath)
        {
            //如果源文件夹不存在，则创建
            if (!Directory.Exists(strFromPath))
            {
                Directory.CreateDirectory(strFromPath);
            }
            //取得要拷贝的文件夹名
            string strFolderName = strFromPath.Substring(strFromPath.LastIndexOf("\\") +
               1, strFromPath.Length - strFromPath.LastIndexOf("\\") - 1);
            //如果目标文件夹中没有源文件夹则在目标文件夹中创建源文件夹
            if (!Directory.Exists(strToPath + "\\" + strFolderName))
            {
                Directory.CreateDirectory(strToPath + "\\" + strFolderName);
            }
            //创建数组保存源文件夹下的文件名
            string[] strFiles = Directory.GetFiles(strFromPath);
            //循环拷贝文件
            for (int i = 0; i < strFiles.Length; i++)
            {
                //取得拷贝的文件名，只取文件名，地址截掉。
                string strFileName = strFiles[i].Substring(strFiles[i].LastIndexOf("\\") + 1, strFiles[i].Length - strFiles[i].LastIndexOf("\\") - 1);
                //开始拷贝文件,true表示覆盖同名文件
                File.Copy(strFiles[i], strToPath + "\\" + strFolderName + "\\" + strFileName, true);
            }
            //创建DirectoryInfo实例
            DirectoryInfo dirInfo = new DirectoryInfo(strFromPath);
            //取得源文件夹下的所有子文件夹名称
            DirectoryInfo[] ZiPath = dirInfo.GetDirectories();
            for (int j = 0; j < ZiPath.Length; j++)
            {
                //获取所有子文件夹名
                string strZiPath = strFromPath + "\\" + ZiPath[j].ToString();
                //把得到的子文件夹当成新的源文件夹，从头开始新一轮的拷贝
                CopyFolder(strZiPath, strToPath + "\\" + strFolderName);
            }
        }
        

        #region 将文件移动到指定目录( 剪切 )

        /// <summary>
        /// 将文件移动到指定目录( 剪切 )
        /// </summary>
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>
        /// <param name="descDirectoryPath">移动到的目录的绝对路径</param>
        public static void MoveToDirectory(string sourceFilePath, string descDirectoryPath)
        {
            //有效性检测
            if (!IsExistFile(sourceFilePath))
            {
                return;
            }

            try
            {
                //获取源文件的名称
                string sourceFileName = GetFileName(sourceFilePath);

                //如果目标目录不存在则创建
                CreateDirectory(descDirectoryPath);

                //如果目标中存在同名文件,则删除
                if (IsExistFile(descDirectoryPath + "\\" + sourceFileName))
                {
                    DeleteFile(descDirectoryPath + "\\" + sourceFileName);
                }

                //目标文件路径
                string descFilePath;
                if (!descDirectoryPath.EndsWith(@"\"))
                {
                    descFilePath = descDirectoryPath + "\\" + sourceFileName;
                }
                else
                {
                    descFilePath = descDirectoryPath + sourceFileName;
                }

                //将文件移动到指定目录
                File.Move(sourceFilePath, descFilePath);
            }
            catch
            {
            }
        }

        #endregion

        #region 将文件移动到指定目录，并指定新的文件名( 剪切并改名 )

        /// <summary>
        /// 将文件移动到指定目录，并指定新的文件名( 剪切并改名 )
        /// </summary>
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>
        /// <param name="descFilePath">目标文件的绝对路径</param>
        public static void Move(string sourceFilePath, string descFilePath)
        {
            //有效性检测
            if (!IsExistFile(sourceFilePath))
            {
                return;
            }

            try
            {
                //获取目标文件目录
                string descDirectoryPath = GetDirectoryFromFilePath(descFilePath);

                //创建目标目录
                CreateDirectory(descDirectoryPath);

                //将文件移动到指定目录
                File.Move(sourceFilePath, descFilePath);
            }
            catch
            {
            }
        }

        #endregion

        #region 将流读取到缓冲区中

        /// <summary>
        /// 将流读取到缓冲区中
        /// </summary>
        /// <param name="stream">原始流</param>
        public static byte[] StreamToBytes(Stream stream)
        {
            try
            {
                //创建缓冲区
                var buffer = new byte[stream.Length];

                //读取流
                stream.Read(buffer, 0, ConvertHelper.ToInt32(stream.Length));

                //返回流
                return buffer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //关闭流
                stream.Close();
            }
        }

        #endregion

        #region 将文件读取到缓冲区中

        /// <summary>
        /// 将文件读取到缓冲区中
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static byte[] FileToBytes(string filePath)
        {
            //获取文件的大小 
            int fileSize = GetFileSize(filePath);

            //创建一个临时缓冲区
            var buffer = new byte[fileSize];

            //创建一个文件
            var file = new FileInfo(filePath);

            //创建一个文件流
            using (FileStream fs = file.Open(FileMode.Open))
            {
                //将文件流读入缓冲区
                fs.Read(buffer, 0, fileSize);

                return buffer;
            }
        }

        #endregion

        #region 将文件读取到字符串中

        /// <summary>
        /// 将文件读取到字符串中
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static string FileToString(string filePath)
        {
            return FileToString(filePath, Encoding.UTF8);
        }

        /// <summary>
        /// 将文件读取到字符串中
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="encoding">字符编码</param>
        public static string FileToString(string filePath, Encoding encoding)
        {
            //创建流读取器
            var reader = new StreamReader(filePath, encoding);
            try
            {
                //读取流
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //关闭流读取器
                reader.Close();
            }
        }

        #endregion

        #region 从文件的绝对路径中获取文件名( 包含扩展名 )

        /// <summary>
        /// 从文件的绝对路径中获取文件名( 包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetFileName(string filePath)
        {
            //获取文件的名称
            var fi = new FileInfo(filePath);
            return fi.Name;
        }

        #endregion

        #region 从文件的绝对路径中获取文件名( 不包含扩展名 )

        /// <summary>
        /// 从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetFileNameNoExtension(string filePath)
        {
            //获取文件的名称
            var fi = new FileInfo(filePath);
            return fi.Name.Split('.')[0];
        }

        #endregion

        #region 从文件的绝对路径中获取扩展名

        /// <summary>
        /// 从文件的绝对路径中获取扩展名
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetExtension(string filePath)
        {
            //获取文件的名称
            var fi = new FileInfo(filePath);
            return fi.Extension;
        }

        #endregion

        #region 清空指定目录

        /// <summary>
        /// 清空指定目录下所有文件及子目录,但该目录依然保存.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void ClearDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                //删除目录中所有的文件
                string[] fileNames = GetFileNames(directoryPath);
                foreach (string t in fileNames)
                {
                    DeleteFile(t);
                }

                //删除目录中所有的子目录
                string[] directoryNames = GetDirectories(directoryPath);
                foreach (string t in directoryNames)
                {
                    DeleteDirectory(t);
                }
            }
        }

        #endregion

        #region 清空文件内容

        /// <summary>
        /// 清空文件内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void ClearFile(string filePath)
        {
            //删除文件
            File.Delete(filePath);

            //重新创建该文件
            CreateFile(filePath);
        }

        #endregion

        #region 删除指定文件

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void DeleteFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                File.Delete(filePath);
            }
        }

        #endregion

        #region 删除指定目录

        /// <summary>
        /// 删除指定目录及其所有子目录
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }

        #endregion

        #region 写文件

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strValue"></param>
        public static void WriteFile(string strFilePath, string strValue)
        {
            var oFile = new FileInfo(strFilePath);
            if (!oFile.Directory.Exists)
                oFile.Directory.Create();

            if (!oFile.Exists)
                oFile.Create().Close();

            var oWrite = new StreamWriter(strFilePath, false, Encoding.UTF8);
            oWrite.Write(strValue);
            oWrite.Flush();
            oWrite.Close();
        }

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strValue"></param>
        /// <param name="charset"></param>
        public static void WriteFile(string strFilePath, string strValue, string charset)
        {
            var oFile = new FileInfo(strFilePath);
            if (!oFile.Directory.Exists)
                oFile.Directory.Create();

            if (!oFile.Exists)
                oFile.Create().Close();

            var oWrite = new StreamWriter(strFilePath, false, Encoding.GetEncoding(charset));
            oWrite.Write(strValue);
            oWrite.Flush();
            oWrite.Close();
        }

        #endregion
    }
    public class WebUtils
    {
        #region 获得当前绝对路径

        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        #endregion

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.QueryString[strName];
        }

        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.Form[strName];
        }

        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName)
        {
            if ("".Equals(GetQueryString(strName)))
            {
                return GetFormString(strName);
            }
            else
            {
                return GetQueryString(strName);
            }
        }


    }
}