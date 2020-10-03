using Assets.Code._4.CORE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
            base.OnTriggerEnter2D(col);

            if (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Enemy) && gameObject.layer.Equals((int)GameSettings.LayerSettings.SkillPlayer))
            {
                GameSettings.BattleControl.ComboCount++;
                GameSettings.BattleControl.ShowCombo();
            }
            //Stop motion (Nhìn chưa đc ưng lắm, để làm sau)
            //if ((col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Enemy) && gameObject.layer.Equals((int)GameSettings.LayerSettings.SkillPlayer))
            //    || (col.gameObject.layer.Equals((int)GameSettings.LayerSettings.Hero) && gameObject.layer.Equals((int)GameSettings.LayerSettings.SkillEnemy)))
            //{
            //    if (!IsStopMotion)
            //    {
            //        StartCoroutine(StopMotionAction());
            //        IsStopMotion = true;
            //    }
            //}
        }
        #endregion
    }
}
