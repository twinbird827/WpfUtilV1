using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WpfUtilV1.Common
{
    public class HttpUtil
    {
        /// <summary>
        /// ｸｯｷｰに設定する有効期限を取得します。
        /// </summary>
        /// <param name="d">有効期限の基準となる日付。ﾃﾞﾌｫﾙﾄは現在日時</param>
        /// <returns>有効期限を表す文字列</returns>
        public static string GetExpiresDate(DateTime d = default(DateTime))
        {
            d = default(DateTime) == d ? DateTime.Now : d;
            return d.AddMonths(1).ToString("r", DateTimeFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// HttpWebRequestからHttpWebResponseを表す文字列を取得します。
        /// </summary>
        /// <param name="httpWebRequest"><see cref="HttpWebRequest"/></param>
        /// <returns><see cref="HttpWebResponse"/>のｽﾄﾘｰﾑ文字列</returns>
        public static string GetResponseString(HttpWebRequest httpWebRequest)
        {
            string expression = String.Empty;
            using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                expression = GetResponseString(httpWebResponse);
            }
            return expression;
        }

        /// <summary>
        /// 文字列をUrlｴﾝｺｰﾄﾞ文字列に変換します。
        /// </summary>
        /// <param name="txt">変換前文字列</param>
        /// <returns>変換後文字列</returns>
        public static string ToUrlEncode(string txt)
        {
            return HttpUtility.UrlEncode(txt);
        }

        /// <summary>
        /// Urlｴﾝｺｰﾄﾞ文字列から文字列に変換します。
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string FromUrlEncode(string txt)
        {
            txt = HttpUtility.UrlDecode(txt);
            txt = txt.Replace("&lt;", "<");
            txt = txt.Replace("&gt;", ">");
            txt = txt.Replace("&quot;", "\"");
            txt = txt.Replace("&apos;", "'");
            txt = txt.Replace("&amp;", "&");

            return txt;
        }

        /// <summary>
        /// HttpWebRequestからHttpWebResponseを表す文字列を取得します。
        /// </summary>
        /// <param name="httpWebRequest"><see cref="HttpWebRequest"/></param>
        /// <returns><see cref="HttpWebResponse"/>のｽﾄﾘｰﾑ文字列</returns>
        public static string GetResponseString(HttpWebResponse httpWebResponse, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            string expression = String.Empty;
            using (Stream responseStream = httpWebResponse.GetResponseStream())
            {
                StreamReader streamReader = new StreamReader(responseStream, encoding);
                expression = streamReader.ReadToEnd();
            }
            return expression;
        }

        /// <summary>
        /// IEにｸｯｷｰを設定します。
        /// </summary>
        /// <param name="cookies">設定するｸｯｷｰ</param>
        /// <param name="isDispose">設定後にｸｯｷｰを破棄するかどうか。ﾃﾞﾌｫﾙﾄ=true</param>
        public static void InternetSetCookie(string url, CookieCollection cookies, string cookieData = "{0}; expires={1}", bool isDispose = true)
        {
            InternetSetCookie(url, cookies.OfType<Cookie>().ToArray(), cookieData, isDispose);
        }

        /// <summary>
        /// IEにｸｯｷｰを設定します。
        /// </summary>
        /// <param name="cookies">設定するｸｯｷｰ</param>
        /// <param name="isDispose">設定後にｸｯｷｰを破棄するかどうか。ﾃﾞﾌｫﾙﾄ=true</param>
        public static void InternetSetCookie(string url, Cookie[] cookies, string cookieData = "{0}; expires={1}", bool isDispose = true)
        {
            // 取得したｸｯｷｰをIEに流用
            Array.ForEach(
                cookies,
                cookie => InternetSetCookie(url, cookie, cookieData, isDispose)
            );
        }

        /// <summary>
        /// IEにｸｯｷｰを設定します。
        /// </summary>
        /// <param name="cookies">設定するｸｯｷｰ</param>
        /// <param name="isDispose">設定後にｸｯｷｰを破棄するかどうか。ﾃﾞﾌｫﾙﾄ=true</param>
        public static void InternetSetCookie(string url, Cookie cookie, string cookieData = "{0}; expires={1}", bool isDispose = true)
        {
            // 取得したｸｯｷｰをIEに流用
            Win32Methods.InternetSetCookie(
                url,
                cookie.Name,
                String.Format(cookieData,
                    cookie.Value,
                    GetExpiresDate()
                )
            );

            if (isDispose)
            {
                IEnumerable<Cookie> cookies = new Cookie[] { cookie };
                var disposable = cookies.OfType<IDisposable>().FirstOrDefault();
                if (disposable != null)
                {
                    // 破棄可能なｸｯｷｰは破棄する。
                    disposable.Dispose();
                }
            }
        }

        /// <summary>
        /// 指定したUrlからｲﾒｰｼﾞをﾊﾞｲﾄ配列として取得します。
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns></returns>
        public static async Task<byte[]> DownLoadImageBytesAsync(string url)
        {
            using (var web = new HttpClient())
            {
                return await web.GetByteArrayAsync(url);
            }
        }

        /// <summary>
        /// 指定したﾊﾞｲﾄ配列からﾋﾞｯﾄﾏｯﾌﾟｲﾒｰｼﾞを作成します。
        /// </summary>
        /// <param name="bytes">ﾊﾞｲﾄ配列</param>
        /// <param name="freezing">Freezeするかどうか</param>
        /// <returns></returns>
        public static BitmapImage CreateBitmap(byte[] bytes, bool freezing = true)
        {
            using (var stream = new WrappingStream(new MemoryStream(bytes)))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.DecodePixelWidth = 360;
                bitmap.DecodePixelWidth = 270;
                bitmap.EndInit();
                if (freezing && bitmap.CanFreeze)
                {
                    bitmap.Freeze();
                }
                return bitmap;
            }
        }

        /// <summary>
        /// 指定したﾃﾞｨｽﾊﾟｯﾁｬで、指定したUrlからｲﾒｰｼﾞを取得する非同期ﾀｽｸを取得します。
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dispatcher"></param>
        /// <returns></returns>
        public static async Task<BitmapImage> DownloadImageAsync(string url, Dispatcher dispatcher)
        {
            var bytes = await DownLoadImageBytesAsync(url).ConfigureAwait(false);
            return await dispatcher.InvokeAsync(() => CreateBitmap(bytes));
        }
    }
}
