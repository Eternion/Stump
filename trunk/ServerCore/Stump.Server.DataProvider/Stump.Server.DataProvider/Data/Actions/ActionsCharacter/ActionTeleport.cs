//using System;
//using Stump.DofusProtocol.Enums;
//using Stump.Server.WorldServer.Entities;
//using Stump.Server.WorldServer.Global;

//namespace Stump.Server.WorldServer.Actions.ActionsCharacter
//{
//    public class ActionTeleport : CharacterAction
//    {
//        public ActionTeleport()
//        {
//        }

//        public ActionTeleport(uint mapId, ushort cell, DirectionsEnum direction)
//        {
//            MapId = mapId;
//            Cell = cell;
//            Direction = direction;
//        }

//        public uint MapId
//        {
//            get;
//            set;
//        }

//        public ushort Cell
//        {
//            get;
//            set;
//        }

//        public DirectionsEnum Direction
//        {
//            get;
//            set;
//        }

//        public override void Execute(Character executer)
//        {
//            var map = World.Instance.GetMap(MapId);

//            if (map == null)
//                throw new Exception(string.Format("Map {0} doesn't exists", MapId));

//            executer.ChangeMap(map, Cell, Direction);
//        }
//    }
//}