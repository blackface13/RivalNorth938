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

            //Cốt truyện
            Language.Add("f70adc32fa2c6", "Năm 905");
            Language.Add("952ae1104f048", "Tĩnh Hải quân - Việt Nam thời bấy giờ vẫn đang bị đàn áp và đô hộ bởi thực dân phương Bắc.");
            Language.Add("c5301ef0eeab3", "Độc Cô Tổn - tướng của triều đại nhà Đường, thời bấy giờ đang nắm quyền trên đất nước Tĩnh Hải quân");
            Language.Add("f9fd7ef19e7ba", "Vì không theo phe Chu Ôn - 1 tay quyền thần nhà Đường, nên đã bị bắt và giết hại");
            Language.Add("c522ca716cb74", "Nội chính nhà Đường đang lung lay, Tĩnh Hải quân không có người cai quản.");



            //Tên npc
            Language.Add("882e003a7e4c3", "Thợ rèn");
            return Language;
        }
    }
}
