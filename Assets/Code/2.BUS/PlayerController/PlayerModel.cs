using Assets.Code._2.BUS.ItemController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code._2.BUS.PlayerController
{
    /// <summary>
    /// Thuộc tính người chơi
    /// </summary>
    public class PlayerModel
    {
        public string PlayerName { get; set; }
        public Int64 Money { get; set; }
        public float Streng { get; set; }
        public float Def { get; set; }
        public int InventorySlot { get; set; }//Độ rộng rương đồ
        public int LanguageID { get; set; }
        public List<ItemModel> InventoryItems { get; set; }
    }
}
