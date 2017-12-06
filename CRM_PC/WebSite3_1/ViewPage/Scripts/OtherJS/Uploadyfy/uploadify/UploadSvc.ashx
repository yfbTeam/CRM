<%@ WebHandler Language="C#" Class="UploadSvc" %>

using System;
using System.Web;
using System.IO;
public class UploadSvc : IHttpHandler
{


    public string GetResponse(string responsevalue)
    {
        //判断提交方式
        HttpContext context = HttpContext.Current;
        
        
        
        string id = "";
        if (context.Request.RequestType.ToLower() == "get")
        {
            if (context.Request.QueryString[responsevalue] == null)
            {
                return "";
            }
            id = context.Request.QueryString[responsevalue];
        }
        else
        {
            if (context.Request.Form[responsevalue] == null)
            {
                return "";
            }
            id = context.Request.Form[responsevalue];
        }
        return id;
    }
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/json";
        HttpPostedFile file = context.Request.Files[0];

        string ext = System.IO.Path.GetExtension(file.FileName);

        string fileName = DateTime.Now.Ticks + ext;

        string p = "/upload/" + fileName;

        string path = context.Server.MapPath("~/upload/" + fileName);

        file.SaveAs(path);

        context.Response.Write("上传成功");
        context.Response.End();
        string oper = GetResponse("Operate");
        object returnobj = null;

        switch (oper.ToLower())
        {
            case "uploadmedia":
                returnobj = UploadImage();
                break;
            case "getmedia":
                returnobj = GetMedia();
                break;
            default:
                break;
        }

        string returnvalue = GetObjectJson(returnobj);
        context.Response.Write(returnvalue);
        context.Response.End();
    }
    private string Upload(HttpContext context)
    {
        HttpPostedFile file = context.Request.Files[0];

        string ext = System.IO.Path.GetExtension(file.FileName);

        string fileName = DateTime.Now.Ticks + ext;

        string p = "/upload/" + fileName;

        string path = context.Server.MapPath("~/upload/" + fileName);

        file.SaveAs(path);

        //context.htm = htm & ("1");
        return "";
    }
    private object GetMedia()
    {
        System.Collections.Generic.List<FileINfo> lit = new System.Collections.Generic.List<FileINfo>();
        string tmpPath = HttpContext.Current.Server.MapPath("/Upload/Media/");
        string imgtype = "*.bmp|*.jpg|*.gif|*.png|*.rar|*.zip|*.docx";
        string[] ImageType = imgtype.Split('|');

        for (int i = 0; i < ImageType.Length; i++)
        {
            string[] dirs = System.IO.Directory.GetFiles(tmpPath, ImageType[i]);
            int j = 0;
            foreach (string dir in dirs)
            {
                System.IO.FileInfo file = new System.IO.FileInfo(dir);
                FileINfo finfo = new FileINfo();
                finfo.FIlename = j.ToString();
                finfo.FileUrll = "../../Upload/Media/" + file.Name;
                lit.Add(finfo);
                j++;
            }
        }
        return lit;
    }
 
   
    /// <summary>
    /// 序列化数据到json
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetObjectJson(object value)
    {
        return "";// Newtonsoft.Json.JsonConvert.SerializeObject(value);
    }
    private class FileINfo
    {
        public string FileUrll { get; set; }
        public string FIlename { get; set; }
    }
    private object UploadImage()
    {
        object returnobj = "ok";
        string who = GetResponse("who");
        HttpFileCollection files = HttpContext.Current.Request.Files;
        int count = files.Count;
        for (int i = 0; i < count; i++)
        {
            HttpPostedFile file = files[i];
            string size = "";
            float fileSize = (float)files[i].ContentLength;

            if (fileSize > 1024 * 1024)
                size = fileSize / (1024 * 1024) + "MB";
            else
                size = fileSize / 1024 + "KB";

            string tmpPath = HttpContext.Current.Server.MapPath("/Upload/Media/");
            string fileName = System.IO.Path.GetFileName(file.FileName);
            try
            {
               //string json = "<script type='text/javascript'>setTimeout(function(){},50);</script>";
                string severlocalpath = tmpPath + fileName;
                file.SaveAs(severlocalpath);
                returnobj = "ok";
            }
            catch (Exception ex)
            {
                returnobj = ex.Message;
            }
        }
        return returnobj;
    }
  

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}