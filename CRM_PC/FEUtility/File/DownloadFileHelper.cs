using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;

namespace FEUtility
{
    /// <summary>
    /// 下载文件帮助类
    /// </summary>
    public class DownloadFileHelper
    {
        /// <summary>
        /// 文件下载方法2
        /// </summary>
        /// <param name="request">request对象</param>
        /// <param name="response">response对象</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="fullPath">文件路径</param>
        /// <param name="speed">间隔</param>
        /// <returns></returns>
        public static bool ResponseFile(HttpRequest request, HttpResponse response, string fileName, string fullPath, long speed)
        {
            if (speed == 0L)
            {
                speed = 0xfa000L;
            }
            try
            {
                string tagetPath = HttpContext.Current.Server.MapPath("/uploadfiles/" + Guid.NewGuid() + Path.GetExtension(fullPath));
                File.Copy(fullPath, tagetPath);
                fullPath = tagetPath;
                var myFile = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var br = new BinaryReader(myFile);
                try
                {
                    response.AddHeader("Accept-Ranges", "bytes");
                    response.Buffer = false;
                    long fileLength = myFile.Length;
                    long startBytes = 0L;
                    const int pack = 0x2800;
// ReSharper disable PossibleLossOfFraction
                    int sleep = ((int)Math.Floor((decimal)(0x3e8 * pack / speed))) + 1;
// ReSharper restore PossibleLossOfFraction
                    if (request.Headers["Range"] != null)
                    {
                        response.StatusCode = 0xce;
                        startBytes = Convert.ToInt64(request.Headers["Range"].Split(new[] { '=', '-' })[1]);
                    }
                    response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0L)
                    {
                        response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1L, fileLength));
                    }
                    response.AddHeader("Connection", "Keep-Alive");
                    response.ContentType = "application/octet-stream";
                    response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    // ReSharper disable PossibleLossOfFraction
                    int maxCount = ((int)Math.Floor((decimal)((fileLength - startBytes) / pack))) + 1;
                    // ReSharper restore PossibleLossOfFraction
                    for (int i = 0; i < maxCount; i++)
                    {
                        if (response.IsClientConnected)
                        {
                            response.BinaryWrite(br.ReadBytes(pack));
                            Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                }
                catch (IOException oE)
                {
                    string str = oE.Message;
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                    try
                    {
                        File.Delete(fullPath);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

    }
}