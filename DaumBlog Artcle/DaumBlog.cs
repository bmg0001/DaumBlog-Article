using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using WinHttp;

namespace DaumBlog_Artcle
{
    public class DaumBlog
    {
        public bool Post(string DaumID,string DaumPW,string BlogAddress,string title,string content,int categoryID)
        {
            try
            {
                string Fcookie = "";
                title = HttpUtility.UrlEncode(title, Encoding.UTF8);
                content = HttpUtility.UrlEncode(content, Encoding.UTF8);
                WinHttpRequest wt = new WinHttpRequest();
                wt.Open("GET", "https://logins.daum.net/accounts/presrp.do?id=" + DaumID + "&srpla=" + DaumPW + "&_=1463758848866");
                wt.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
                wt.Send();

                string rid = Regex.Split(Regex.Split(wt.ResponseText, "rid\":\"")[1], "\",\"")[0];
                string srplb = Regex.Split(Regex.Split(wt.ResponseText, "srplb\":\"")[1], "\",\"")[0];

                wt.Open("POST", "https://logins.daum.net/accounts/msrp.do?rid=" + rid + "&srplm1=" + srplb);
                wt.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                wt.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                wt.SetRequestHeader("Referer", "https://logins.daum.net/accounts/msrp.do?rid=" + rid + "&srplm1=" + srplb);
                wt.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
                wt.Send("url=http%3A%2F%2Fwww.daum.net%2F&relative=&mobilefull=1&weblogin=1&id=" + DaumID + "&pw=");


                wt.Open("POST", "https://logins.daum.net/accounts/mobile.do");
                wt.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                wt.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                wt.SetRequestHeader("Referer", "https://logins.daum.net/accounts/loginform.do?mobilefull=1&url=http%3A%2F%2Fm.daum.net%3Fnil_rc%3DKZXd2c");
                wt.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
                wt.Send("url=http%3A%2F%2Fm.daum.net%3Fnil_rc%3DKZXd2c&relative=&mobilefull=1&weblogin=1&id=" + DaumID + "&pw=" + DaumPW);

                string log = wt.GetResponseHeader("X-DaumLogin-Error");
                if (log.IndexOf("200") == -1)
                {
                    throw new Exception ("올바르지 않은 ID 입니다.");
                }

                string[] cookie = wt.GetAllResponseHeaders().Split(new string[] { "\n" }, StringSplitOptions.None);
                int cookieCount = 0;
                foreach (String header in cookie)
                {
                    if (header.StartsWith("Set-Cookie: "))
                    {
                        cookieCount++;
                    }
                }
                String[] cookies = new String[cookieCount];
                cookieCount = 0;
                foreach (String header in cookie)
                {
                    if (header.StartsWith("Set-Cookie: "))
                    {
                        String cookie1 = header.Replace("Set-Cookie: ", "");
                        Fcookie += cookie1;
                        cookies[cookieCount] = cookie1;
                    }
                }

                foreach (string cds in cookies)
                {
                    Fcookie += cds;
                }

                string TS = "TS=\"" + Regex.Split(Regex.Split(Fcookie, "TS=")[1], ";")[0] + "\";";
                string HTS = "HTS=\"" + Regex.Split(Regex.Split(Fcookie, "HTS=")[1], ";")[0];
                string HM_CU = "HM_CU=\"" + Regex.Split(Regex.Split(Fcookie, "HM_CU=")[1], ";")[0] + "\";";
                string PROF = "PROF=\"" + Regex.Split(Regex.Split(Fcookie, "PROF=")[1], ";")[0] + "\";";
                string ALID = "ALID=\"" + Regex.Split(Regex.Split(Fcookie, "ALID=")[1], ";")[0] + "\";";
                string ALCT = "ALCT=\"" + Regex.Split(Regex.Split(Fcookie, "ALCT=")[1], ";")[0] + "\";";
                string LSID = "LSID=\"" + Regex.Split(Regex.Split(Fcookie, "LSID=")[1], ";")[0] + "\";";
                string AGEN = "AGEN=\"" + Regex.Split(Regex.Split(Fcookie, "AGEN=")[1], ";")[0] + "\";";
                string SLEVEL = "SLEVEL=\"" + Regex.Split(Regex.Split(Fcookie, "SLEVEL=")[1], ";")[0] + "\";";
                Fcookie = "";
                Fcookie = TS + HTS + HM_CU + PROF + ALID + ALCT + LSID + AGEN + SLEVEL;

                wt.Open("GET", "http://blog.daum.net/" + BlogAddress);
                wt.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                wt.SetRequestHeader("Accept-Language", "ko-KR,ko;q=0.8,en-US;q=0.6,en;q=0.4");
                wt.SetRequestHeader("Referer", "http://www.daum.net/dsp/blog/_blog/_top/hdn/myBlogNewsListForDaumTopNew.do");
                wt.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
                wt.Send();
                string blogid = Regex.Split(Regex.Split(wt.ResponseText, "blogid=")[1], "&")[0];

                wt.Open("GET", "http://blog.daum.net/_blog/BlogTypeMain.do?blogid=" + blogid.Trim() + "&admin=");
                wt.SetRequestHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                wt.SetRequestHeader("Accept-Language", "ko-KR,ko;q=0.8,en-US;q=0.6,en;q=0.4");
                wt.SetRequestHeader("Referer", "http://www.daum.net/dsp/blog/_blog/_top/hdn/myBlogNewsListForDaumTopNew.do");
                wt.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
                wt.Send();
                string articleOpen = Regex.Split(Regex.Split(wt.ResponseText, "articleopen != \"")[1], "\" ")[0];

                wt.Open("POST", "http://blog.daum.net/_blog/SimpleEditor.do?blogid=" + blogid.Trim());
                wt.SetRequestHeader("Accept", "*/*");
                wt.SetRequestHeader("charset", "utf-8");
                wt.SetRequestHeader("Cookie", Fcookie);
                wt.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                wt.SetRequestHeader("Referer", "http://www.daum.net/dsp/blog/_blog/_top/hdn/myBlogNewsListForDaumTopNew.do");
                wt.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/537.36");
                wt.Send("articleTitle=" + title + "&article=" + content + "&articleOpen=" + articleOpen + "&categoryID=" + categoryID + "&formName=articleTypeList");
                return true;
            }
            catch
            {
                throw new Exception("프로그램 오류가 발생 하였습니다.\n(로그인오류 또는 블로그 서비스 이상)");
            } 
        }
    }
}
