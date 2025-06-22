using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Level
{
    public class SetupFPS : MonoBehaviour
    {
        public int FrameRate;
        void Start()
        {
            UnityEngine.Application.targetFrameRate = FrameRate;
            UnityEngine.Debug.Log($"Target frame rate set to {FrameRate} FPS.");
        }
    }
}
