using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Config
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] [Range(1.0f, 10.0f)] private float Agility = 5.0f;

        public float AGI => Agility;
    }
}
