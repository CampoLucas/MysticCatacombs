using System;

namespace Game.Interfaces
{
    public interface IView : IDisposable
    {
        void UpdateMovementValues(float moveAmount);
        void CrossFade(string stateName, float transitionDuration = 0.2f);
        void CrossFade(int stateHash, float transitionDuration = 0.2f);
        void Play(string stateName);

        void SetFloat(string parameter, float value);
        // void SetTrigger(string value);
        // void SetTrigger(int value);
        // void SetFloat(string parameter, float value);
        // void SetFloat(int id, float value);
        // void SetBool(string parameter, float value);
        // void SetBool(int id, float value);
    }
}