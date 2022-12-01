using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZaccCharv;


    [CreateAssetMenu(fileName = "Light2DLerp", menuName = "LerpHelperValue/Light2DLerp")]
    public class Light2DLerp : LerpHelperValue
    {
        public GameObject obj;
        public override float ValueA(GameObject obj)
        {
            float LightValue = obj.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity;

            return LightValue;
        }
    }

