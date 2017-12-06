using System;
using System.IO;
using System.Net;

namespace FEUtility
{
    /// <summary>
    /// 下载文件
    /// </summary>
    public class DownloadClass
    {
        public long LCurrentPos;
        public long LDownloadFile;
        /// <summary>
        /// 长度
        /// </summary>
        public long LStartPos;
        /// <summary>
        /// 提示信息
        /// </summary>
        public string StrError;
        /// <summary>
        /// 保存的文件路径
        /// </summary>
        public string StrFileName;
        /// <summary>
        /// 要下载的url
        /// </summary>
        public string StrUrl;
        /// <summary>
        /// 下载文件
        /// </summary>
        public void DownloadFile()
        {
            FileStream fs;
            if (File.Exists(StrFileName))
            {
                fs = File.OpenWrite(StrFileName);
                LStartPos = fs.Length;
                fs.Seek(LStartPos, SeekOrigin.Current);
            }
            else
            {
                fs = new FileStream(StrFileName, FileMode.Create);
                LStartPos = 0L;
            }
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(StrUrl);
                long length = request.GetResponse().ContentLength;
                LDownloadFile = length;
                if (LStartPos > 0L)
                {
                    request.AddRange((int) LStartPos);
                }
                Stream ns = request.GetResponse().GetResponseStream();
                var nbytes = new byte[0x200];
                if (ns != null)
                {
                    int nReadSize = ns.Read(nbytes, 0, 0x200);
                    while (nReadSize > 0)
                    {
                        fs.Write(nbytes, 0, nReadSize);
                        nReadSize = ns.Read(nbytes, 0, 0x200);
                        LCurrentPos = fs.Length;
                    }
                }
                fs.Close();
                if (ns != null) ns.Close();
                StrError = "OK";
            }
            catch (Exception ex)
            {
                fs.Close();
                StrError = "下载过程中出现错误:" + ex;
            }
        }
    }
}