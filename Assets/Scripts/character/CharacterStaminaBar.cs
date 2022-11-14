using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ZaccCharv
{
    public class CharacterStaminaBar : MonoBehaviour
    {
        public GameObject character;
        [SerializeField] private Slider slider;

        public int _current;

        void Start()
        {
            slider = GetComponent<Slider>();
        }

        // Update is called once per frame
        void Update()
        {
            _current = character.gameObject.GetComponent<CharacterStatusBar>().current;
            slider.value = (float)_current / 100;
        }
    }
}