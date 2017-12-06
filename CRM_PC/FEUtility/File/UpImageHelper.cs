using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace FEUtility
{
    /// <summary>
    /// 图片上传帮助类
    /// </summary>
    public class UpImageHelper
    {
        #region 私有成员
        private static string _allowFormat = ".jpeg|.jpg|.bmp|.gif"; //允许图片格式
        private static int _allowSize = 1; //允许上传图片大小,默认为1MB
        private static string _wordWater = ""; //文字水印
        private static string _picWater = ""; //图片路径
        private static string _imgwidth = ""; //生成图片宽度集合
        private static string _imgheight = ""; //生成图片高度集合 
        private static bool _limitWidth = true; //是否限制最大宽度
        private static int _maxWidth = 600; //最大宽度
        private static bool _cutImage = true; //是否剪裁图片

        #endregion

        #region 属性

        /// <summary>
        /// 允许图片格式
        /// </summary>
        public static string AllowFormat
        {
            get { return _allowFormat; }
            set { _allowFormat = value; }
        }

        /// <summary>
        /// 允许上传图片大小
        /// </summary>
        public static int AllowSize
        {
            get { return _allowSize; }
            set { _allowSize = value; }
        }

        /// <summary>
        /// 文字水印字符
        /// </summary>
        public static string WordWater
        {
            get { return _wordWater; }
            set { _wordWater = value; }
        }

        /// <summary>
        /// 图片水印
        /// </summary>
        public static string PicWater
        {
            get { return _picWater; }
            set { _picWater = value; }
        }

        /// <summary>
        /// 图片宽度
        /// </summary>
        public static string ImgWidth
        {
            get { return _imgwidth; }
            set { _imgwidth = value; }
        }

        /// <summary>
        /// 图片高度
        /// </summary>
        public static string ImgHeight
        {
            get { return _imgheight; }
            set { _imgheight = value; }
        }

        /// <summary>
        /// 是否限制最大宽度，默认为true
        /// </summary>
        public static bool LimitWidth
        {
            get { return _limitWidth; }
            set { _limitWidth = value; }
        }

        /// <summary>
        /// 最大宽度尺寸，默认为600
        /// </summary>
        public static int MaxWidth
        {
            get { return _maxWidth; }
            set { _maxWidth = value; }
        }

        /// <summary>
        /// 是否剪裁图片，默认true
        /// </summary>
        public static bool CutImage
        {
            get { return _cutImage; }
            set { _cutImage = value; }
        }

        /// <summary>
        /// 限制图片最小宽度，0表示不限制
        /// </summary>
        public static int MinWidth { get; set; }

        #endregion

        #region 枚举
        /// <summary>
        /// 剪切模式
        /// </summary>
        public enum CutMode
        {
            /// <summary>
            /// 根据高宽剪切
            /// </summary>
            CutWh = 1,
            /// <summary>
            /// 根据宽剪切
            /// </summary>
            CutW = 2,
            /// <summary>
            /// 根据高剪切
            /// </summary>
            CutH = 3,
            /// <summary>
            /// 缩放不剪裁
            /// </summary>
            CutNo = 4
        }

        #endregion

        #region 方法

        /// <summary>
        /// 通用图片上传类
        /// </summary>
        /// <param name="postedFile">HttpPostedFile控件</param>
        /// <param name="savePath">保存路径【sys.config配置路径】</param>
        /// <param name="oImgWidth">图片宽度</param>
        /// <param name="oImgHeight">图片高度</param>
        /// <param name="cMode">剪切类型</param>
        /// <param name="fileName"></param>
        /// <returns>【0-系统配置错误,1-上传图片成功，2-格式错误，3-超过文件上传大小】</returns>
        public static int FileSaveAs(HttpPostedFile postedFile, string savePath, int oImgWidth, int oImgHeight,
                                     CutMode cMode, ref string fileName)
        {
            try
            {
                //获取上传文件的扩展名 
                string sEx = Path.GetExtension(postedFile.FileName);
                if (!CheckValidExt(AllowFormat, sEx))
                    return 2; //格式错误  

                //获取上传文件的大小
                int postFileSize = postedFile.ContentLength/1024;

                if (postFileSize > AllowSize)
                    return 3; //超过文件上传大小 

                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                //重命名名称
                string newfileName = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day +
                                     DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second +
                                     DateTime.Now.Millisecond.ToString("000");
                string fName = "s" + newfileName + sEx;
                string fullPath = Path.Combine(savePath, fName);
                postedFile.SaveAs(fullPath);

                //重命名名称
                string sNewfileName = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day +
                                      DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second +
                                      DateTime.Now.Millisecond.ToString("000");
                string sFName = sNewfileName + sEx;
                fileName = sFName;
                string sFullPath = Path.Combine(savePath, sFName);
                CreateSmallPhoto(fullPath, oImgWidth, oImgHeight, sFullPath, PicWater, WordWater, cMode);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                //压缩
                if (postFileSize > 100)
                {
                    CompressPhoto(sFullPath, 100);
                }
                return 1;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// 通用图片上传类
        /// </summary>
        /// <param name="postedFile">HttpPostedFile控件</param>
        /// <param name="savePath">保存路径【sys.config配置路径】</param>
        /// <param name="finame">返回文件名</param>
        /// <param name="fisize">返回文件大小</param>
        /// <returns>【-1,上传失败，0-系统配置错误,1-上传图片成功，2-格式错误，3-超过文件上传大小,4-未上传文件】</returns>
        public static int FileSaveAs(HttpPostedFile postedFile, string savePath, ref string finame, ref int fisize)
        {
            try
            {
                if (string.IsNullOrEmpty(postedFile.FileName))
                    return 4;

                var rd = new Random();
                int rdInt = rd.Next(1000, 9999);
                //重命名名称
                string newfileName = DateTime.Now.Year + DateTime.Now.Month.ToString() + DateTime.Now.Day +
                                     DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second +
                                     DateTime.Now.Millisecond + rdInt;

                //获取上传文件的扩展名 
                string sEx = Path.GetExtension(postedFile.FileName);
                if (!CheckValidExt(AllowFormat, sEx))
                    return 2; //格式错误  

                //获取上传文件的大小
                int postFileSize = postedFile.ContentLength/1024;

                if (postFileSize > AllowSize)
                    return 3; //超过文件上传大小 


                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }


                string fullPath = savePath + newfileName + sEx;

                postedFile.SaveAs(fullPath);


                var bmp = new Bitmap(fullPath);
                int realWidth = bmp.Width;
                int realHeight = bmp.Height;
                bmp.Dispose();

                #region 检测图片宽度限制

                if (MinWidth > 0)
                {
                    if (realWidth < MinWidth)
                    {
                        return -1;
                    }
                }

                #endregion

                #region 监测图片宽度是否超过600，超过的话，自动压缩到600

                if (_limitWidth && realWidth > MaxWidth)
                {
                    int mWidth = MaxWidth;
                    int mHeight = mWidth*realHeight/realWidth;

                    string tempFile = savePath + Guid.NewGuid() + sEx;
                    File.Move(fullPath, tempFile);
                    CreateSmallPhoto(tempFile, mWidth, mHeight, fullPath, "", "");
                    File.Delete(tempFile);
                }

                #endregion

                #region 压缩图片存储尺寸

                if (sEx != null)
                {
                    if (sEx.ToLower() != ".gif")
                    {
                        CompressPhoto(fullPath, 100);
                    }

                    #endregion

                    finame = newfileName + sEx;
                }
                fisize = postFileSize;

                //生成缩略图片高宽
                if (string.IsNullOrEmpty(ImgWidth))
                    return 1;

                string[] oWidthArray = ImgWidth.Split(',');
                string[] oHeightArray = ImgHeight.Split(',');
                if (oWidthArray.Length != oHeightArray.Length)
                    return 0; //系统配置错误

                for (int i = 0; i < oWidthArray.Length; i++)
                {
                    if (Convert.ToInt32(oWidthArray[i]) <= 0 || Convert.ToInt32(oHeightArray[i]) <= 0)
                        continue;

                    string sImg = savePath + newfileName + "_" + i + sEx;
                    if (CutImage)
                        CreateSmallPhoto(fullPath, Convert.ToInt32(oWidthArray[i]), Convert.ToInt32(oHeightArray[i]),
                                         sImg, "", "");
                    else
                        CreateSmallPhoto(fullPath, Convert.ToInt32(oWidthArray[i]), Convert.ToInt32(oHeightArray[i]),
                                         sImg, "", "", CutMode.CutNo);
                }

                #region 给大图添加水印

                if (!string.IsNullOrEmpty(PicWater))
                    AttachPng(PicWater, fullPath);
                else if (!string.IsNullOrEmpty(WordWater))
                    AttachText(WordWater, fullPath);

                #endregion

                return 1;
            }
            catch
            {
                return -1;
            }
        }

        #region 验证格式

        /// <summary>
        /// 验证格式
        /// </summary>
        /// <param name="allType">所有格式</param>
        /// <param name="chkType">被检查的格式</param>
        /// <returns>bool</returns>
        public static bool CheckValidExt(string allType, string chkType)
        {
            string[] sArray = allType.Split('|');

            return sArray.Any(temp => temp.ToLower() == chkType.ToLower());
        }

        #endregion

        #region 根据需要的图片尺寸，按比例剪裁原始图片

        /// <summary>
        /// 根据需要的图片尺寸，按比例剪裁原始图片
        /// </summary>
        /// <param name="nWidth">缩略图宽度</param>
        /// <param name="nHeight">缩略图高度</param>
        /// <param name="img">原始图片</param>
        /// <returns>剪裁区域尺寸</returns>
        public static Size CutRegion(int nWidth, int nHeight, Image img)
        {
            double width;
            double height;

            double nw = nWidth;
            double nh = nHeight;

            double pw = img.Width;
            double ph = img.Height;

            if (nw/nh > pw/ph)
            {
                width = pw;
                height = pw*nh/nw;
            }
            else if (nw/nh < pw/ph)
            {
                width = ph*nw/nh;
                height = ph;
            }
            else
            {
                width = pw;
                height = ph;
            }

            return new Size(Convert.ToInt32(width), Convert.ToInt32(height));
        }

        #endregion

        #region 等比例缩小图片
        /// <summary>
        /// 等比例缩小图片
        /// </summary>
        /// <param name="nWidth">宽度</param>
        /// <param name="nHeight">高度</param>
        /// <param name="img">image对象</param>
        /// <returns>转化后大小</returns>
        public static Size NewSize(int nWidth, int nHeight, Image img)
        {
            double w;
            double h;
            double sw = Convert.ToDouble(img.Width);
            double sh = Convert.ToDouble(img.Height);
            double mw = Convert.ToDouble(nWidth);
            double mh = Convert.ToDouble(nHeight);

            if (sw < mw && sh < mh)
            {
                w = sw;
                h = sh;
            }
            else if ((sw/sh) > (mw/mh))
            {
                w = nWidth;
                h = (w*sh)/sw;
            }
            else
            {
                h = nHeight;
                w = (h*sw)/sh;
            }

            return new Size(Convert.ToInt32(w), Convert.ToInt32(h));
        }

        #endregion

        #region 生成缩略图

        #region 生成缩略图，不加水印

        /// <summary>
        /// 生成缩略图，不加水印
        /// </summary>
        /// <param name="filename">源文件</param>
        /// <param name="nWidth">缩略图宽度</param>
        /// <param name="nHeight">缩略图高度</param>
        /// <param name="destfile">缩略图保存位置</param>
        public static void CreateSmallPhoto(string filename, int nWidth, int nHeight, string destfile)
        {
            Image img = Image.FromFile(filename);
            ImageFormat thisFormat = img.RawFormat;

            Size cutSize = CutRegion(nWidth, nHeight, img);
            var outBmp = new Bitmap(nWidth, nHeight);
            Graphics g = Graphics.FromImage(outBmp);

            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            int nStartX = (img.Width - cutSize.Width)/2;
            int nStartY = (img.Height - cutSize.Height)/2;

            g.DrawImage(img, new Rectangle(0, 0, nWidth, nHeight),
                        nStartX, nStartY, cutSize.Width, cutSize.Height, GraphicsUnit.Pixel);
            g.Dispose();

            //if (thisFormat.Equals(ImageFormat.Gif))
            //{
            //    Response.ContentType = "image/gif";
            //}
            //else
            //{
            //    Response.ContentType = "image/jpeg";
            //}

            // 以下代码为保存图片时，设置压缩质量
            var encoderParams = new EncoderParameters();
            var quality = new long[1];
            quality[0] = 100;

            var encoderParam = new EncoderParameter(Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            ImageCodecInfo[] arrayIci = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegIci = arrayIci.FirstOrDefault(t => t.FormatDescription.Equals("JPEG"));

            if (jpegIci != null)
            {
                //outBmp.Save(Response.OutputStream, jpegICI, encoderParams);
                outBmp.Save(destfile, jpegIci, encoderParams);
            }
            else
            {
                //outBmp.Save(Response.OutputStream, thisFormat);
                outBmp.Save(destfile, thisFormat);
            }

            img.Dispose();
            outBmp.Dispose();
        }

        #endregion

        #region 生成缩略图，加水印
        /// <summary>
        /// 生成缩略图，加水印
        /// </summary>
        /// <param name="filename">源文件</param>
        /// <param name="nWidth">缩略图宽度</param>
        /// <param name="nHeight">缩略图高度</param>
        /// <param name="destfile">缩略图保存位置</param>
        /// <param name="sy">图片水印</param>
        /// <param name="nType"></param>
        public static void CreateSmallPhoto(string filename, int nWidth, int nHeight, string destfile, string sy,
                                            int nType)
        {
            if (nType == 0)
                CreateSmallPhoto(filename, nWidth, nHeight, destfile, sy, "");
            else
                CreateSmallPhoto(filename, nWidth, nHeight, destfile, "", sy);
        }

        #endregion

        #region 生成缩略图

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="filename">源文件</param>
        /// <param name="nWidth">缩略图宽度</param>
        /// <param name="nHeight">缩略图高度</param>
        /// <param name="destfile">缩略图保存位置</param>
        /// <param name="png">图片水印</param>
        /// <param name="text">文本水印</param>
        public static void CreateSmallPhoto(string filename, int nWidth, int nHeight, string destfile, string png,
                                            string text)
        {
            Image img = Image.FromFile(filename);
            ImageFormat thisFormat = img.RawFormat;

            Size cutSize = CutRegion(nWidth, nHeight, img);
            var outBmp = new Bitmap(nWidth, nHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.White);

            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            int nStartX = (img.Width - cutSize.Width)/2;
            int nStartY = (img.Height - cutSize.Height)/2;

            g.DrawImage(img, new Rectangle(0, 0, nWidth, nHeight),
                        nStartX, nStartY, cutSize.Width, cutSize.Height, GraphicsUnit.Pixel);
            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量
            var encoderParams = new EncoderParameters();
            var quality = new long[1];
            quality[0] = 100;

            var encoderParam = new EncoderParameter(Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            ImageCodecInfo[] arrayIci = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegIci = arrayIci.FirstOrDefault(t => t.FormatDescription.Equals("JPEG"));

            if (jpegIci != null)
            {
                outBmp.Save(destfile, jpegIci, encoderParams);
            }
            else
            {
                outBmp.Save(destfile, thisFormat);
            }

            img.Dispose();
            outBmp.Dispose();

            if (!string.IsNullOrEmpty(png))
                AttachPng(png, destfile);

            if (!string.IsNullOrEmpty(text))
                AttachText(text, destfile);
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="filename">源文件</param>
        /// <param name="nWidth">缩略图宽度</param>
        /// <param name="nHeight">缩略图高度</param>
        /// <param name="destfile">缩略图保存位置</param>
        /// <param name="png">图片水印</param>
        /// <param name="text">文本水印</param>
        /// <param name="cMode">剪切模式</param>
        public static void CreateSmallPhoto(string filename, int nWidth, int nHeight, string destfile, string png,
                                            string text, CutMode cMode)
        {
            Image img = Image.FromFile(filename);

            if (nWidth <= 0)
                nWidth = img.Width;
            if (nHeight <= 0)
                nHeight = img.Height;

            int towidth = nWidth;
            int toheight = nHeight;

            switch (cMode)
            {
                case CutMode.CutWh: //指定高宽缩放（可能变形）                
                    break;
                case CutMode.CutW: //指定宽，高按比例                    
                    toheight = img.Height*nWidth/img.Width;
                    break;
                case CutMode.CutH: //指定高，宽按比例
                    towidth = img.Width*nHeight/img.Height;
                    break;
                case CutMode.CutNo: //缩放不剪裁
                    int maxSize = (nWidth >= nHeight ? nWidth : nHeight);
                    if (img.Width >= img.Height)
                    {
                        towidth = maxSize;
                        toheight = img.Height*maxSize/img.Width;
                    }
                    else
                    {
                        toheight = maxSize;
                        towidth = img.Width*maxSize/img.Height;
                    }
                    break;
                default:
                    break;
            }
            nWidth = towidth;
            nHeight = toheight;

            ImageFormat thisFormat = img.RawFormat;

            var cutSize = new Size(nWidth, nHeight);
            if (cMode != CutMode.CutNo)
                cutSize = CutRegion(nWidth, nHeight, img);

            var outBmp = new Bitmap(cutSize.Width, cutSize.Height);

            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.White);

            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            int nStartX = (img.Width - cutSize.Width)/2;
            int nStartY = (img.Height - cutSize.Height)/2;

            //int x1 = (outBmp.Width - nWidth) / 2;
            //int y1 = (outBmp.Height - nHeight) / 2;

            if (cMode != CutMode.CutNo)
                g.DrawImage(img, new Rectangle(0, 0, nWidth, nHeight),
                            nStartX, nStartY, cutSize.Width, cutSize.Height, GraphicsUnit.Pixel);
            else
                g.DrawImage(img, new Rectangle(0, 0, nWidth, nHeight),
                            0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量
            var encoderParams = new EncoderParameters();
            var quality = new long[1];
            quality[0] = 100;

            var encoderParam = new EncoderParameter(Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            ImageCodecInfo[] arrayIci = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegIci = arrayIci.FirstOrDefault(t => t.FormatDescription.Equals("JPEG"));

            if (jpegIci != null)
            {
                outBmp.Save(destfile, jpegIci, encoderParams);
            }
            else
            {
                outBmp.Save(destfile, thisFormat);
            }

            img.Dispose();
            outBmp.Dispose();

            if (!string.IsNullOrEmpty(png))
                AttachPng(png, destfile);

            if (!string.IsNullOrEmpty(text))
                AttachText(text, destfile);
        }

        #endregion

        #endregion

        #region 添加文字水印
        /// <summary>
        /// 添加文字水印
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="file">源文件</param>
        public static void AttachText(string text, string file)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (!File.Exists(file))
                return;

            var oFile = new FileInfo(file);
            string strTempFile = Path.Combine(oFile.DirectoryName, Guid.NewGuid() + oFile.Extension);
            oFile.CopyTo(strTempFile);

            Image img = Image.FromFile(strTempFile);
            ImageFormat thisFormat = img.RawFormat;

            int nHeight = img.Height;
            int nWidth = img.Width;

            var outBmp = new Bitmap(nWidth, nHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.White);

            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(img, new Rectangle(0, 0, nWidth, nHeight),
                        0, 0, nWidth, nHeight, GraphicsUnit.Pixel);

            var sizes = new[] {16, 14, 12, 10, 8, 6, 4};

            Font crFont = null;
            var crSize = new SizeF();

            //通过循环这个数组，来选用不同的字体大小
            //如果它的大小小于图像的宽度，就选用这个大小的字体
            for (int i = 0; i < 7; i++)
            {
                //设置字体，这里是用arial，黑体
                crFont = new Font("arial", sizes[i], FontStyle.Bold);
                //Measure the Copyright string in this Font
                crSize = g.MeasureString(text, crFont);

                if ((ushort) crSize.Width < (ushort) nWidth)
                    break;
            }

            //因为图片的高度可能不尽相同, 所以定义了
            //从图片底部算起预留了5%的空间
            var yPixlesFromBottom = (int) (nHeight*.08);

            //现在使用版权信息字符串的高度来确定要绘制的图像的字符串的y坐标

            float yPosFromBottom = ((nHeight - yPixlesFromBottom) - (crSize.Height/2));

            //计算x坐标
            // ReSharper disable PossibleLossOfFraction
            float xCenterOfImg = (nWidth/2);
            // ReSharper restore PossibleLossOfFraction

            //把文本布局设置为居中
            var strFormat = new StringFormat {Alignment = StringAlignment.Center};

            //通过Brush来设置黑色半透明
            var semiTransBrush2 = new SolidBrush(Color.FromArgb(153, 0, 0, 0));

            //绘制版权字符串
            g.DrawString(text, //版权字符串文本
                         crFont, //字体
                         semiTransBrush2, //Brush
                         new PointF(xCenterOfImg + 1, yPosFromBottom + 1), //位置
                         strFormat);

            //设置成白色半透明
            var semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));

            //第二次绘制版权字符串来创建阴影效果
            //记住移动文本的位置1像素
            g.DrawString(text, //版权文本
                         crFont, //字体
                         semiTransBrush, //Brush
                         new PointF(xCenterOfImg, yPosFromBottom), //位置
                         strFormat);

            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量
            var encoderParams = new EncoderParameters();
            var quality = new long[1];
            quality[0] = 100;

            var encoderParam = new EncoderParameter(Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            ImageCodecInfo[] arrayIci = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegIci = arrayIci.FirstOrDefault(t => t.FormatDescription.Equals("JPEG"));

            if (jpegIci != null)
            {
                outBmp.Save(file, jpegIci, encoderParams);
            }
            else
            {
                outBmp.Save(file, thisFormat);
            }

            img.Dispose();
            outBmp.Dispose();

            File.Delete(strTempFile);
        }

        #endregion

        #region 添加图片水印
        /// <summary>
        /// 添加图片水印
        /// </summary>
        /// <param name="png">水印图片</param>
        /// <param name="file">原图片</param>
        public static void AttachPng(string png, string file)
        {
            if (string.IsNullOrEmpty(png))
                return;

            if (!File.Exists(png))
                return;

            if (!File.Exists(file))
                return;

            var oFile = new FileInfo(file);
            string strTempFile = Path.Combine(oFile.DirectoryName, Guid.NewGuid() + oFile.Extension);
            oFile.CopyTo(strTempFile);

            Image img = Image.FromFile(strTempFile);
            ImageFormat thisFormat = img.RawFormat;
            int nHeight = img.Height;
            int nWidth = img.Width;

            var outBmp = new Bitmap(nWidth, nHeight);
            Graphics g = Graphics.FromImage(outBmp);

            // 设置画布的描绘质量
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(img, new Rectangle(0, 0, nWidth, nHeight),
                        0, 0, nWidth, nHeight, GraphicsUnit.Pixel);

            img.Dispose();

            img = Image.FromFile(png);

            //Bitmap bmpPng = new Bitmap(img);

            //ImageAttributes imageAttr = new ImageAttributes();
            //Color bg = Color.Green;
            //imageAttr.SetColorKey(bg, bg);

            Size pngSize = NewSize(nWidth, nHeight, img);
            g.DrawImage(img,
                        new Rectangle((nWidth - pngSize.Width)/2, (nHeight - pngSize.Height)/2, pngSize.Width,
                                      pngSize.Height),
                        0, 0, img.Width, img.Height, GraphicsUnit.Pixel);

            g.Dispose();

            // 以下代码为保存图片时，设置压缩质量
            var encoderParams = new EncoderParameters();
            var quality = new long[1];
            quality[0] = 100;

            var encoderParam = new EncoderParameter(Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;

            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象。
            ImageCodecInfo[] arrayIci = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegIci = arrayIci.FirstOrDefault(t => t.FormatDescription.Equals("JPEG"));

            if (jpegIci != null)
            {
                outBmp.Save(file, jpegIci, encoderParams);
            }
            else
            {
                outBmp.Save(file, thisFormat);
            }

            img.Dispose();
            outBmp.Dispose();

            File.Delete(strTempFile);
        }

        #endregion

        #region 得到指定mimeType的ImageCodecInfo

        /// <summary> 
        /// 保存JPG时用 
        /// </summary> 
        /// <param name="mimeType"> </param> 
        /// <returns>得到指定mimeType的ImageCodecInfo </returns> 
        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] codecInfo = ImageCodecInfo.GetImageEncoders();
            return codecInfo.FirstOrDefault(ici => ici.MimeType == mimeType);
        }

        #endregion

        #region 保存为JPEG格式，支持压缩质量选项

        /// <summary>
        /// 保存为JPEG格式，支持压缩质量选项
        /// </summary>
        /// <param name="sourceFile">原文件</param>
        /// <param name="fileName">新文件</param>
        /// <param name="qty">EncoderParameter值</param>
        /// <returns></returns>
        public static bool KiSaveAsJpeg(string sourceFile, string fileName, int qty)
        {
            var bmp = new Bitmap(sourceFile);

            try
            {
                var ps = new EncoderParameters(1);

                var p = new EncoderParameter(Encoder.Quality, qty);
                ps.Param[0] = p;

                bmp.Save(fileName, GetCodecInfo("image/jpeg"), ps);

                bmp.Dispose();

                return true;
            }
            catch
            {
                bmp.Dispose();
                return false;
            }
        }

        #endregion

        #region 将图片压缩到指定大小

        /// <summary>
        /// 将图片压缩到指定大小
        /// </summary>
        /// <param name="fileName">待压缩图片</param>
        /// <param name="size">期望压缩后的尺寸</param>
        public static void CompressPhoto(string fileName, int size)
        {
            if (!File.Exists(fileName))
                return;

            int nCount = 0;
            var oFile = new FileInfo(fileName);
            long nLen = oFile.Length;
            while (nLen > size*1024 && nCount < 10)
            {
                string dir = oFile.Directory.FullName;
                string tempFile = Path.Combine(dir, Guid.NewGuid() + "." + oFile.Extension);
                oFile.CopyTo(tempFile, true);

                KiSaveAsJpeg(tempFile, fileName, 70);

                try
                {
                    File.Delete(tempFile);
                }
                catch (Exception)
                {
                }

                nCount++;

                oFile = new FileInfo(fileName);
                nLen = oFile.Length;
            }
        }

        #endregion

        #endregion
    }
}