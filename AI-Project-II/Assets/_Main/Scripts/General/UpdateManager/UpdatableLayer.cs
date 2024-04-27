using UnityEngine;

namespace Game.UpdateManager
{
    [System.Serializable]
    public struct UpdateInfo
    {
        public int Order => order;
        public int FrameRate => frameRate;
        

        [SerializeField] private int order;
        [SerializeField] private int frameRate;
    }
    
    public class UpdatableLayer
    {
        public float Delta { get; private set; }
        public UpdateInfo UpdateInfo { get; private set; }

        private float _lastFrame;
        private float _currentFrame;
        private float _timeInterval;

        public UpdatableLayer(UpdateInfo updateInfo)
        {
            UpdateInfo = updateInfo;
            if (UpdateInfo.FrameRate < 1) return;

            _timeInterval = 1 / (float)updateInfo.FrameRate;
        }
    }
}