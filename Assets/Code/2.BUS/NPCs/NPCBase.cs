using Assets.Code._4.CORE;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code._2.BUS.NPCs
{
    public class NPCBase : MonoBehaviour
    {
        #region Variables
        [Required]
        [TabGroup("Cấu hình thuộc tính")]
        [Title("Tên NPC")]
        public TextMeshPro TextMessTitle;
        [TabGroup("Cấu hình thuộc tính")]
        [Title("Các object khác")]
        public List<GameObject> ListObject;

        public List<string> Choices { get; set; }//Các lựa chọn khi tap vào NPC
        public List<MessageContent> Content1 { get; set; }//Đoạn hội thoại 1
        public List<MessageContent> Content2 { get; set; }//Đoạn hội thoại 2
        public List<MessageContent> Content3 { get; set; }//Đoạn hội thoại 3
        public List<MessageContent> Content4 { get; set; }//Đoạn hội thoại 4
        public List<MessageContent> Content5 { get; set; }//Đoạn hội thoại 5


        public string NpcName { get; set; }//Tên NPC, lấy mã trong file language
    public Animator Anim { get; set; }

        public virtual void Awake()
        {
            Anim = this.GetComponent<Animator>();
        }

        public virtual void Initialize()
        {
            InitData();
            TextMessTitle.text = GameSystems.Language[NpcName];
        }

        /// <summary>
        /// Khởi tạo dữ liệu cho NPC
        /// </summary>
        public virtual void InitData()
        {
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
