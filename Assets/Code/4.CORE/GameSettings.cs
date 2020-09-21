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
            MapObjectBehind = 13,
            SkillPlayer = 14,
            SkillEnemy = 15,
        }

        //Battle
        public static readonly float[] BattleSpeed = new float[] { 1f, 1.5f, 2f, 3f };//Tốc độ trận đấu
        public static int MaxAtkCombo = 2;//Tối da combo atk của player
        public static float TimeDelayComboNormalAtk = .5f;//Thời gian xóa combo atk
        public static string PathSkillObjects = "Prefabs/Skills/";
        public static Vector3 DefaultPositionObjectSkill = new Vector3(-1000, -1000, 20);
        public static float PositionZDefaultInMap = 20f;

        //Các biến sử dụng trong quá trình chơi
        public static GameObject Player;//Main player
        public static HeroController PlayerController;//Main player controller
        public static ObjectController ObjControl = new ObjectController();
        public static BattleController BattleControl;

        #region Tọa độ các object skill
        public static Dictionary<string, Vector3> SkillsPosition;
        #endregion
        #endregion

        #region Functions
        public static void CreateSkillsPosition()
        {
            SkillsPosition = new Dictionary<string, Vector3>();
            SkillsPosition.Add("BladeAtk1", new Vector3(5, 0, PositionZDefaultInMap));
            SkillsPosition.Add("BladeAtk2", new Vector3(5, 0, PositionZDefaultInMap));
            SkillsPosition.Add("BladeAtk3", new Vector3(3, 0, PositionZDefaultInMap));

            SkillsPosition.Add("StaffAtk2", new Vector3(5, 0, PositionZDefaultInMap));
            SkillsPosition.Add("StaffAtk3", new Vector3(3, 0, PositionZDefaultInMap));
        }
        #endregion
    }
}
