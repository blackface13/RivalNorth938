using Assets.Code._4.CORE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code._2.BUS.Heroes
{
    public class HeroSkill : SkillController
    {
        #region Variables
        private bool IsStopMotion;//Đã dừng chuyển động trong lượt đánh hay chưa
        private float DelayTimeStopMotion = .002f;//Thời gian tạm dừng khi đánh trúng đối phương
        #endregion

        #region Initialize
        public override void Awake()
        {
            base.Awake();
        }
        public override void OnEnable()
        {
            base.OnEnable();
            IsStopMotion = false;
        }
        #endregion

        #region Functions
        public IEnumerator StopMotionAction()
        {
            Time.timeScale = .01f;
            yield return new WaitForSeconds(DelayTimeStopMotion);
            Time.timeScale = 1;
        }
        #endregion

        #region Physics
        public override void OnTriggerEnter2D(Collider2D col)
        {
            if ((col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Enemy) && gameObject.layer.Equals((int)GameSettings.LayerSettings.SkillPlayer))
            || (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Hero) && gameObject.layer.Equals((int)GameSettings.LayerSettings.SkillEnemy)))
            {
                if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Enemy))
                {
                    //Show combo
                    GameSettings.BattleControl.ComboCount++;
                    GameSettings.BattleControl.ShowCombo();

                    var enemy = col.GetComponent<EnemyController>();

                    //Hất tung đối phương
                    if (IsPushUp)
                    {
                        StartCoroutine(GameSettings.BattleControl.PushUpVictim(enemy.ThisRigid2D, ForcePushUp));
                    }
                    //Đẩy đối phương từ trên xuống
                    else if ((IsPushDown || IsPushDownOnJump) && GameSettings.PlayerController.IsJumping)
                    {
                        StartCoroutine(GameSettings.BattleControl.PushDownVictim(enemy.ThisRigid2D, IsPushDown ? ForcePushDown : ForcePushDownOnJump));
                        if (IsRepelWhenTouchLane)
                        {
                            enemy.IsRepelWhenTouchLane = true;
                            enemy.ForceRepelWhenTouchLane = ForceRepelWhenTouchLane;
                        }
                    }
                    //Giữ đối phương trên không
                    else if (!enemy.IsLaning)
                    {
                        StartCoroutine(GameSettings.BattleControl.PushUpVictim(enemy.ThisRigid2D, ForceKeepPushUp));
                    }
                    //Đẩy lùi nếu trạng thái bình thường
                    else
                    {
                        StartCoroutine(GameSettings.BattleControl.RepelVictim(enemy.ThisRigid2D, this.transform.position, col.gameObject.transform.position, (Random.Range(ForceToVictim.x, ForceToVictim.y) + ForceToVictimBonus), enemy.IsViewLeft));
                    }

                    //Show damage
                    GameSettings.BattleControl.ShowDmgText(col.transform.position, UnityEngine.Random.Range(0001, 5000).ToString());

                    enemy.SetAnimation(EnemyController.Actions.Hited);
                }
            }
        }
        #endregion
    }
}
