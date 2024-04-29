using System;

namespace Game.Interfaces
{
    public interface ISteeringDecorator : ISteering
    {
        /// <summary>
        /// Reference to the child.
        /// </summary>
        ISteering Child { get; }

        /// <summary>
        /// Sets the child of the decorator.
        /// </summary>
        /// <param name="child"></param>
        void SetChild(ISteering child);
    }
}