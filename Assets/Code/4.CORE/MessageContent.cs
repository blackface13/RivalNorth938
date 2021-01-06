using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code._4.CORE
{
    /// <summary>
    /// Data nội dung cuộc hội thoại trong game
    /// </summary>
    public struct MessageContent
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public MessageContent(string title, string content)
        {
            Title = title;
            Content = content;
        }
    }
}
