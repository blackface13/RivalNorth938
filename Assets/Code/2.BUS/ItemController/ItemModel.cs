
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code._2.BUS.ItemController
{

    [System.Serializable]
    public class ItemModel
    {
        public ItemType Type;
        public int ID;
        public string NameCode;
        public string DesCode;
        public int Level;//Cấp độ trang bị
        public int Quantity;//Số lượng item
        public int Price;//Giá bán
        public float Atk;//Chỉ số tấn công vật lý
        public float BuffAtk;//Phần trăm xuyên giáp
        public float GetHPAtk;//Hút máu vật lý
        public float Critical;//Chí mạng
        public float HP;//Máu
        public float ReHP;//Chỉ số hồi máu mỗi 5s
        public float DefP;//Giáp
        public float DefState;//Kháng hiệu ứng
        public int Special;//Hiệu ứng đặc biệt (thiết kế riêng)
        public enum ItemType
        {
            equip,//Item trang bị
            use,//Item sử dụng
            quest,//Item nhiệm vụ
        }
        public ItemModel(ItemType type, int id, string nameCode, string desCode, int level, int quantity, int price, float atk, float buffatk, float gethpatk, 
            float crit, float hp, float rehp, float defp, float defstate, int special)
        {
            Type = type;
            ID = id;
            NameCode = nameCode;
            DesCode = desCode;
            Level = level;
            Quantity = quantity;
            Price = price;
            Atk = atk;
            BuffAtk = buffatk;
            GetHPAtk = gethpatk;
            Critical = crit;
            HP = hp;
            ReHP = rehp;
            DefP = defp;
            DefState = defstate;
            Special = special;
        }
        public ItemModel() { }

        /// <summary>
        /// Hàm clone một item
        /// </summary>
        /// <returns></returns>
        public ItemModel Clone()
        {
            return (ItemModel)this.MemberwiseClone();
        }
    }
}
