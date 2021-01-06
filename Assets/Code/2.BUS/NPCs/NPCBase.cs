using Assets.Code._4.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code._2.BUS.NPCs
{
    public class NPCBase : MonoBehaviour
    {
        #region Variables
        public List<string> Choices { get; set; }//Các lựa chọn khi tap vào NPC
        public List<MessageContent> Content1 { get; set; }//Đoạn hội thoại 1
        public List<MessageContent> Content2 { get; set; }//Đoạn hội thoại 2
        public List<MessageContent> Content3 { get; set; }//Đoạn hội thoại 3
        public List<MessageContent> Content4 { get; set; }//Đoạn hội thoại 4
        public List<MessageContent> Content5 { get; set; }//Đoạn hội thoại 5

        public virtual void Initialize()
        {
            Choices = new List<string>();
            Choices.Add(GameSystems.Language["6b1608a22fb38"]);
            Choices.Add(GameSystems.Language["9e2ba2782e839"]);
        }
        #endregion

        #region Functions
        public virtual void Start()
        {
            Initialize();
        }
        #endregion
    }
}
