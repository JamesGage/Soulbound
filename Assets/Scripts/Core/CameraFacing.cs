using System;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        Camera cam;

        private void Start()
        {
            cam = Camera.main;
        }

        private void LateUpdate()
        {
            transform.forward = cam.transform.forward;
        }
    }
}