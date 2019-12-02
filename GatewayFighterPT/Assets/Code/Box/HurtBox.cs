using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Box
{
    public class HurtBox : MonoBehaviour
    {
        public void Hit()
        {
            Destroy(this.transform.parent.gameObject);
        }
    }
}