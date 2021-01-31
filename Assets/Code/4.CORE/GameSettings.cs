using Assets.Code._2.BUS.Languages;
using Assets.Code._2.BUS.PlayerController;
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
            NPC = 31
        }

        //Battle 
        public static int MaxAtkCombo = 2;//Tối da combo atk của player
        public static float TimeDelayComboNormalAtk = .5f;//Thời gian xóa combo atk
        public static string PathSkillObjects = "Prefabs/Skills/";
        public static Vector3 DefaultPositionObjectSkill = new Vector3(-1000, -1000, 20);
        public static float PositionZDefaultInMap = 20f;
        public static bool IsAllowActions = false;//Cho phép ng chơi thao tác hay ko
        public static float JoystickPosYLimitDetect = 80f;//Tọa độ Y cho phép detect việc điều khiển lên trên hay xuống dưới

        //Các biến sử dụng trong quá trình chơi
        public static GameObject Player;//Main player
        public static HeroController PlayerController;//Main player controller
        public static ObjectController ObjControl = new ObjectController();
        public static BattleController BattleControl;
        public static Dictionary<string, string> Language;

        #region Tọa độ các object skill
        public static Dictionary<string, Vector3> SkillsPosition;
        #endregion
        #endregion

        #region Functions
        public static void CreateSkillsPosition()
        {
            SkillsPosition = new Dictionary<string, Vector3>();
            //Blade
            SkillsPosition.Add("BladeAtk1", new Vector3(2, 0, PositionZDefaultInMap));
            SkillsPosition.Add("BladeAtk2", new Vector3(2, 0, PositionZDefaultInMap));
            SkillsPosition.Add("BladeAtk3", new Vector3(1, 0, PositionZDefaultInMap));
            SkillsPosition.Add("BladeAtkPushUp", new Vector3(0, 0, PositionZDefaultInMap));
            //Staff
            SkillsPosition.Add("StaffAtk2", new Vector3(2, 0, PositionZDefaultInMap));
            SkillsPosition.Add("StaffAtk3", new Vector3(1, 0, PositionZDefaultInMap));
            //Katana
            SkillsPosition.Add("KatanaAtk1", new Vector3(2, 0.5f, PositionZDefaultInMap));
            SkillsPosition.Add("KatanaAtk2", new Vector3(3, 0.5f, PositionZDefaultInMap));
            SkillsPosition.Add("KatanaAtk3_1", new Vector3(1, 2.4f, PositionZDefaultInMap));
            SkillsPosition.Add("KatanaAtk3_2", new Vector3(0, 0, PositionZDefaultInMap));
            SkillsPosition.Add("KatanaAtkPushUp", new Vector3(0, 0, PositionZDefaultInMap));
            //Katana
            //SkillsPosition.Add("SwordAtk1", new Vector3(2, 0.5f, PositionZDefaultInMap));
            SkillsPosition.Add("SwordAtk2", new Vector3(1.5f, 1f, PositionZDefaultInMap));
            SkillsPosition.Add("SwordAtk3", new Vector3(2.3f, 2f, PositionZDefaultInMap));
        }
        #endregion
    }
}
