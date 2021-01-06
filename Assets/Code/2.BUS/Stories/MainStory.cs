using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code._2.BUS.Stories
{
    /// <summary>
    /// Mạch truyện chính của game
    /// </summary>
    public class MainStory
    {
        public Dictionary<string, bool> Process;
        public MainStory()
        {
            CreateNewStory();
        }
        private void CreateNewStory()
        {
            Process.Add("P1-Introduction", false);//Giới thiệu đoạn đầu game
        }
    }
}
