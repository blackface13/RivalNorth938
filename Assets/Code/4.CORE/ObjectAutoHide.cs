using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code._4.CORE
{
    public class ObjectAutoHide: MonoBehaviour
    {
        //Dành cho các hiệu ứng cần hide mà không có script điều khiển
        // Use this for initialization
        public float time;//Thời gian cần hide, set interface
        public List<GameObject> ChilObject;//Các object con
        private void OnEnable()
        {
            StartCoroutine(AutoHiden());
            if(ChilObject.Count>0)
            {
                foreach (var obj in ChilObject)
                    obj.gameObject.SetActive(true);
            }
        }
        IEnumerator AutoHiden()
        {
            yield return new WaitForSeconds(time);
            Hide();
        }
        private void Hide()
        {
            if (ChilObject.Count > 0)
            {
                foreach (var obj in ChilObject)
                    obj.gameObject.SetActive(false);
            }
            gameObject.SetActive(false);
            gameObject.transform.position = GameSettings.DefaultPositionObjectSkill;
            //gameObject.transform.localEulerAngles = new Vector3();
        }
    }
}
