using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Code._2.BUS.FunctionsController
{
    public class MessageController : MonoBehaviour
    {
        #region Variables
        [Required]
        [TabGroup("Cấu hình")]
        [Title("Tiêu đề hội thoại/Tên nhân vật hội thoại")]
        public Text TitleText;
        [Required]
        [TabGroup("Cấu hình")]
        [Title("Nội dung hội thoại")]
        public Text ContentText;

        private int CurrentMess;
        private int TotalMess;
        /// <summary>
        /// Thực hiện khi enable
        /// </summary>
        private void OnEnable()
        {
            CurrentMess = 0;
            TotalMess = GameSystems.GameControl.CurrentNPC.Content1.Count;
            ShowMessage();
        }

        /// <summary>
        /// Bấm để next message
        /// </summary>
        /// <param name="eventData"></param>
        public void NextMessage(BaseEventData eventData)
        {
            if (CurrentMess >= TotalMess - 1)
                GameSystems.GameControl.EndMessageNPC();
            else
            {
                CurrentMess++;
                ShowMessage();
            }
        }

        /// <summary>
        /// Hiển thị nội dung trò chuyện
        /// </summary>
        private void ShowMessage()
        {
            TitleText.text = GameSystems.Language[GameSystems.GameControl.CurrentNPC.Content1[CurrentMess].Title];
            ContentText.text = GameSystems.Language[GameSystems.GameControl.CurrentNPC.Content1[CurrentMess].Content];
        }
        #endregion
    }
}
