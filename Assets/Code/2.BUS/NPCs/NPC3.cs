using Assets.Code._4.CORE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code._2.BUS.NPCs
{
    /// <summary>
    /// Tiều phu
    /// </summary>
    public class NPC3 : NPCBase
    {
        private AnimationObjectControl ObjectWoodController;//object thân cây gỗ
        private WaitForSeconds WaitingAction = new WaitForSeconds(7);
        public override void Start()
        {
            base.Start();
        }
        public override void InitData()
        {
            ObjectWoodController = ListObject[1].GetComponent<AnimationObjectControl>();//animation object thân cây gỗ
            NpcName = "9bd6aaeab5f77";
            Choices = new List<string>();
            Choices.Add(GameSystems.Language["f70adc32fa2c6"]);
            Choices.Add(GameSystems.Language["f70adc32fa2c6"]);

            Content1 = new List<MessageContent>();
            Content1.Add(new MessageContent { Title = "f70adc32fa2c6", Content = "952ae1104f048" });
            Content1.Add(new MessageContent { Title = "f70adc32fa2c6", Content = "c5301ef0eeab3" });
            Content1.Add(new MessageContent { Title = "f70adc32fa2c6", Content = "f9fd7ef19e7ba" });
            Content1.Add(new MessageContent { Title = "f70adc32fa2c6", Content = "c522ca716cb74" });

            Anim.SetTrigger("Action1");

        }

        /// <summary>
        /// Kết thúc animation chặt gỗ
        /// </summary>
        /// <param name="actionId">0: Action2, 1: Stand</param>
        public void EndAction(int actionId)
        {
            if (actionId.Equals(0))
            {
                StartCoroutine(WaitAction());
                ObjectWoodController.PlayAnim();
                Anim.speed = 1;
            }

            Anim.SetTrigger(actionId.Equals(0) ? "Action2" : "Stand");
        }

        IEnumerator WaitAction()
        {
            yield return WaitingAction;
            ObjectWoodController.ResetAnim();
                Anim.speed = 1;
            Anim.SetTrigger("Action1");
        }
    }
}
