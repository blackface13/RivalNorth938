using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code._4.CORE
{
    public static class GameSettings
    {
        #region Variables
        public enum LayerSettings
        {
            Lane = 8,
            Hero = 9,
            Enemy = 10,
            MapObject = 11,
            MainCamera = 12,
        }

        //Battle
        public static readonly float[] BattleSpeed = new float[] { 1f, 1.5f, 2f, 3f };//Tốc độ trận đấu
        public static int MaxAtkCombo = 2;//Tối da combo atk của player
        #endregion

        #region Functions

        #endregion
    }
}
