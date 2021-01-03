using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code._2.BUS.Languages
{
    public class Vietnamese
    {
        private Dictionary<string, string> Language;

        /// <summary>
        /// Hàm khởi tạo ngôn ngữ
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> CreateLanguage()
        {
            Language = new Dictionary<string, string>();
            Language.Add("a7216e687cd8c", "Hãy nhập tên bạn muốn");
            return Language;
        }
    }
}
