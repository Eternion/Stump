using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Global
{
    class MapMovementAdapter
    {
        public static List<int> getServerMovement(MovementPath movement)
        {
            byte orientation = 0;
            List<int> keys = new List<int>(movement.Path.Count);

            movement.Compress();

            foreach (PathElement pathElement in movement.Path)
            {
                orientation = (byte)pathElement.Orientation;
                keys.Add((orientation & 7) << 12 | pathElement.Step.CellId & 4095);
            }

            keys.Add((orientation & 7) << 12 | movement.End.CellId & 4095);

            return keys;
        }

        public static MovementPath getClientMovement(List<int> keys)
        {
            PathElement last=null;
            MovementPath movementPath = new MovementPath();

            for (int i = 0; i < keys.Count  ;i++ )
            {
                MapPoint mapPoint = MapPoint.FromCellId(keys[i] & 4095);
                PathElement pathElement = new PathElement();
                pathElement.Step = mapPoint;

                if (i == 0)
                    movementPath.Start = mapPoint;
                else
                    last.Orientation = last.Step.OrientationTo(pathElement.Step);
                
                if (i == keys.Count - 1)
                {
                    movementPath.End = mapPoint;
                    break;
                }

                movementPath.Path.Add(pathElement);
                last = pathElement;
            }

           // movementPath.fill();
            return movementPath;
        }

    }
}
