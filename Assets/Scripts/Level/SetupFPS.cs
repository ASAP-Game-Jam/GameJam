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
            // Установка целевой частоты кадров
            FrameRate = 60; // Задайте желаемую частоту кадров
            UnityEngine.Application.targetFrameRate = FrameRate;
            UnityEngine.Debug.Log($"Target frame rate set to {FrameRate} FPS.");
        }
    }
}
