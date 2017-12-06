using System;
using System.IO;
using System.Net;

namespace FEUtility
{
    /// <summary>
    /// 上传文件帮助类
    /// </summary>
    public class HttpFileTransfer
    {
        private string _upLoadedFileName = string.Empty;
        private string _upLoadedFilePath = string.Empty;
        private bool isUploadedSucceded = false;
        /// <summary>
        /// 生成的文件名
        /// </summary>
        public string UpLoadedFileName
        {
            set { _upLoadedFileName = value; }
            get { return _upLoadedFileName; }
        }
        /// <summary>
        /// 生成的文件路径
        /// </summary>
        public string UpLoadedFilePath
        {
            set { _upLoadedFilePath = value; }
            get { return _upLoadedFilePath; }
        }
        /// <summary>
        /// 生成的文件只读属性
        /// </summary>
        public bool IsUploadedSucceded
        {
            set { isUploadedSucceded = value; }
            get { return isUploadedSucceded; }
        }
        /// <summary>
        /// 以NET命令获得对服务器路径的权限修改。
        /// </summary>
        public static void NetStartInfo()
        {
            System.Diagnostics.ProcessStartInfo proinfo = new System.Diagnostics.ProcessStartInfo();
            proinfo.FileName = "net.exe ";
            //读取配置文件信息
            //string temp = @"use " + ConfigurationSettings.AppSettings["ServerNameFileUploadPath"] + " /user:" + ConfigurationSettings.AppSettings["UserName"] + " " + ConfigurationSettings.AppSettings["Password"];
            //proinfo.Arguments = temp;d$\kyherp\
            proinfo.Arguments = @"use \\kyhserver\picture /user:upZhiBaoShuUser 123456";
            proinfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;//默认不显示
            System.Diagnostics.Process process;
            process = System.Diagnostics.Process.Start(proinfo);
            while (process.HasExited == false)
            {
                process.WaitForExit(1000);
            }
        }

        /**/
        ///   <summary> 
        ///  WebClient上传文件至服务器，默认不自动改名
        ///   </summary> 
        ///   <param name="fileNamePath"> 文件名，全路径格式 </param> 
        ///   <param name="uriString"> 服务器文件夹路径 </param> 
        public void UpLoadFile(string fileNamePath, string uriString)
        {
            UpLoadFile(fileNamePath, uriString, false);
        }
        /**/
        ///   <summary> 
        ///  WebClient上传文件至服务器
        ///   </summary> 
        ///   <param name="fileNamePath"> 文件名，全路径格式 </param> 
        ///   <param name="uriString"> 服务器文件夹路径 </param> 
        ///   <param name="IsAutoRename"> 是否自动按照时间重命名 </param> 
        public void UpLoadFile(string fileNamePath, string uriString, bool IsAutoRename)
        {
            string fileName = fileNamePath.Substring(fileNamePath.LastIndexOf(@"\") + 1);
            string NewFileName = fileName;
            if (IsAutoRename)
            {
                NewFileName = DateTime.Now.ToString("yyMMddhhmmss") + DateTime.Now.Millisecond.ToString() + fileNamePath.Substring(fileNamePath.LastIndexOf("."));
                _upLoadedFileName = NewFileName;
            }

            string fileNameExt = fileName.Substring(fileName.LastIndexOf(".") + 1);
            if (uriString.EndsWith(@"\") == false)
            {
                uriString = uriString + @"\";
            }
            string monthFileFolderName = string.Empty;
            monthFileFolderName = DateTime.Now.ToString("yyyyMM");//获取年代月份
            if (!Directory.Exists(uriString + monthFileFolderName))
            {
                Directory.CreateDirectory(uriString + monthFileFolderName);

            }
            //服务器新建文件夹路径
            uriString = uriString + monthFileFolderName + @"\" + NewFileName;
            UpLoadedFilePath = uriString;
            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultNetworkCredentials;
            //  要上传的文件 
            FileStream fs = new FileStream(fileNamePath, FileMode.Open, FileAccess.Read);
            // FileStream fs = OpenFile(); 
            BinaryReader r = new BinaryReader(fs);
            byte[] postArray = r.ReadBytes((int)fs.Length);
            Stream postStream = myWebClient.OpenWrite(uriString, "PUT");
            try
            {
                // 使用UploadFile方法可以用下面的格式
                // myWebClient.UploadFile(uriString,"PUT",fileNamePath); 
                if (postStream.CanWrite)
                {

                    postStream.Write(postArray, 0, postArray.Length);
                    IsUploadedSucceded = true;
                    postStream.Close();
                    fs.Dispose();

                }
                else
                {
                    postStream.Close();
                    fs.Dispose();
                    IsUploadedSucceded = false;
                }

            }
            catch (Exception err)//上传失败的处理模式
            {
                IsUploadedSucceded = false;
                postStream.Close();
                fs.Dispose();
                throw err;
            }
            finally
            {
                postStream.Close();
                fs.Dispose();
            }
        }

        ///   <summary> 
        ///  下载服务器文件至客户端
        ///   </summary> 
        ///   <param name="URL"> 被下载的文件地址，绝对路径 </param> 
        ///   <param name="Dir"> 另存放的目录 </param> 
        public void Download(string URL, string Dir)
        {
            WebClient client = new WebClient();
            string fileName = URL.Substring(URL.LastIndexOf("\\") + 1);   // 被下载的文件名 

            string Path = Dir + fileName;    // 另存为的绝对路径＋文件名 
            // Utility.LogWriter log = new Utility.LogWriter();
            try
            {
                WebRequest myre = WebRequest.Create(URL);
            }
            catch (Exception err)
            {
                // MessageBox.Show(exp.Message,"Error");  
                // log.AddLog(err, " 下载日志文件异常！ ", " Log ");
            }

            try
            {
                client.DownloadFile(URL, fileName);
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(fs);
                byte[] mbyte = r.ReadBytes((int)fs.Length);

                FileStream fstr = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write);

                fstr.Write(mbyte, 0, (int)fs.Length);
                fstr.Close();

            }
            catch (Exception err)
            {
                // MessageBox.Show(exp.Message,"Error"); 
                // log.AddLog(err, " 下载日志文件异常！ ", " Log ");
            }
        }
    }
}