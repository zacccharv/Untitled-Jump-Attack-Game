using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZaccCharv
{
    public class EventManager : MonoBehaviour
    {
        public delegate void CharacterHealthDamaged();
        public static event CharacterHealthDamaged OnCharacterHealthDamaged;

        public delegate void CharacterStaminaDamaged();
        public static event CharacterStaminaDamaged OnCharacterStaminaDamaged;
    }
}