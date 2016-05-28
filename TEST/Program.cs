using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DaumBlog_Artcle;

namespace TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            DaumBlog dv = new DaumBlog();
            dv.Post("아이디","비밀번호","블로그 뒷 주소","제목","내용",0);
            //dv.Post("다음 아이디","다음 비밀번호","제목","내용",Int32 형식 카테고리 ID);
            Console.ReadLine();
        }
    }
}
