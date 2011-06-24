
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Pathfinding;

namespace Stump.Server.WorldServer.Entities
{
    public interface IMovable : ILocableIdentified
    {
        /// <summary>
        ///   Indicate or set if entity is moving.
        /// </summary>
        bool IsMoving
        {
            get;
        }

        bool CanMove();

        /// <summary>
        ///   Move the entity.
        /// </summary>
        void Move(MovementPath movementPath);

        /// <summary>
        ///   Teleport instantly the entity.
        /// </summary>
        /// <param name="to"></param>
        void MoveInstant(ObjectPosition to);

        void MovementEnded();

        void StopMove(ObjectPosition currentObjectPosition);
    }
}