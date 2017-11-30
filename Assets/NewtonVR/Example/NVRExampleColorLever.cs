using UnityEngine;
using System.Collections;

namespace NewtonVR.Example
{
    public class NVRExampleColorLever
        : MonoBehaviour
    {
        public UnityEngine.Color From;
        public UnityEngine.Color To;

        public Renderer Result;

        public NVRLever Lever;

        private void Update()
        {
            Result.material.color = UnityEngine.Color.Lerp(From, To, Lever.CurrentValue);
        }
    }
}