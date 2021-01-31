using Assets.Code._4.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code._2.BUS.Heroes.Skill
{
    public class KatanaAtk3_2:HeroSkill
    {
        public override void OnEnable()
        {
            base.OnEnable();
            if (GameSettings.PlayerController.IsJumping)
                this.transform.position -= new UnityEngine.Vector3(0, 3f, 0);
        }
    }
}
