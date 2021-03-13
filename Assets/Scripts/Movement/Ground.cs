using System;
using UnityEngine;

namespace RPG.Movement
{
    public class Ground : MonoBehaviour
    {
        public GroundType groundType = GroundType.Dirt;
        public enum GroundType
        {
            Dirt,
            Grass,
            Wood,
            Water,
            Stone
        }
    }
}