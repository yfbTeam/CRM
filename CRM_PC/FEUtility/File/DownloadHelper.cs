using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;

namespace FEUtility
{
    /// <summary>
    /// 提供文件下载
    /// </summary>
    public class DownloadHelper : IHttpHandler
    {
        #region 文件下载方法1

        /// <summary>
        /// 是否加密
        /// </summary>
        public bool IsDecrypt { get; set; }

        /// <summary>
        ///功能说明：文件下载类--不管是什么格式的文件,都能够弹出打开/保存窗口,
        ///包括使用下载工具下载
        ///继承于IHttpHandler接口，可以用来自定义HTTP 处理程序同步处理HTTP的请求
        /// </summary>
        /// <param name="context">HttpContext</param>
        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            HttpRequest request = context.Request;

            Stream iStream = null;

            var buffer = new Byte[10240];

            try
            {
                string filename = IsDecrypt ? EncryptHelper.Decrypt(request["fn"]) : request["fn"];

                string filepath = HttpContext.Current.Server.MapPath("~/") + filename; //待下载的文件路径

                iStream = new FileStream(filepath, FileMode.Open,
                                         FileAccess.Read, FileShare.Read);
                response.Clear();

                long dataToRead = iStream.Length;

                long p = 0;
                if (request.Headers["Range"] != null)
                {
                    response.StatusCode = 206;
                    p = long.Parse(request.Headers["Range"].Replace("bytes=", "").Replace("-", ""));
                }
                if (p != 0)
                {
                    response.AddHeader("Content-Range", "bytes " + p + "-" + ((dataToRead - 1)) + "/" + dataToRead);
                }
                response.AddHeader("Content-Length", ((dataToRead - p)).ToString());
                response.ContentType = "application/octet-stream";
                response.AddHeader("Content-Disposition",
                                   "attachment; filename=" +
                                   HttpUtility.UrlEncode(Encoding.GetEncoding(65001).GetBytes(Path.GetFileName(filename))));

                iStream.Position = p;
                dataToRead = dataToRead - p;

                while (dataToRead > 0)
                {
                    if (response.IsClientConnected)
                    {
                        int length = iStream.Read(buffer, 0, 10240);

                        response.OutputStream.Write(buffer, 0, length);
                        response.Flush();

                        buffer = new Byte[10240];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Write("Error : " + ex.Message);
            }
            finally
            {
                if (iStream != null)
                {
                    iStream.Close();
                }
                response.End();
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }

        #endregion

        #region 文件下载方法2
        /// <summary>
        /// 文件下载方法2
        /// </summary>
        /// <param name="request">request对象</param>
        /// <param name="response">response对象</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="fullPath">文件路径</param>
        /// <param name="speed">间隔</param>
        /// <returns></returns>
        public static bool ResponseFile(HttpRequest request, HttpResponse response, string fileName, string fullPath,
                                        long speed)
        {
            try
            {
                var myFile = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var br = new BinaryReader(myFile);
                try
                {
                    response.AddHeader("Accept-Ranges", "bytes");
                    response.Buffer = false;
                    long fileLength = myFile.Length;
                    long startBytes = 0;

                    const double pack = 10240;
                    //int sleep = 200;   //每秒5次   即5*10K bytes每秒
                    int sleep = (int) Math.Floor(1000*pack/speed) + 1;
                    if (request.Headers["Range"] != null)
                    {
                        response.StatusCode = 206;
                        string[] range = request.Headers["Range"].Split(new[] {'=', '-'});
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                    {
                        //Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength-1, fileLength));
                    }
                    response.AddHeader("Connection", "Keep-Alive");
                    response.ContentType = "application/octet-stream";
                    response.AddHeader("Content-Disposition",
                                        "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int) Math.Floor((fileLength - startBytes)/pack) + 1;

                    for (int i = 0; i < maxCount; i++)
                    {
                        if (response.IsClientConnected)
                        {
                            response.BinaryWrite(br.ReadBytes(int.Parse(pack.ToString())));
                            Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();

                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}