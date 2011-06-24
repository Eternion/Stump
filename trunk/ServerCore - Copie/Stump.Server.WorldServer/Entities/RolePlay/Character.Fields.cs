using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Database.Data.Breeds;
using Stump.Database.WorldServer.Alignment;
using Stump.Database.WorldServer.Character;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Breeds;
using Stump.Server.WorldServer.Dialog;
using Stump.Server.WorldServer.Exchange;
using Stump.Server.WorldServer.Items;
using Stump.Server.WorldServer.Npcs;
using Stump.Server.WorldServer.Spells;
using Stump.Server.WorldServer.World.Actors;

namespace Stump.Server.WorldServer.Entities.RolePlay
{
    public partial class Character
    {
        /// <summary>
        ///   Client associated with the character.
        /// </summary>
        public WorldClient Client
        {
            get;
            set;
        }

        /// <summary>
        ///   Record corresponding this character.
        /// </summary>
        public CharacterRecord Record
        {
            get;
            private set;
        }

        public ActorAlignment Alignment
        {
            get;
            protected set;
        }

        /// <summary>
        ///   Breed of this character.
        /// </summary>
        public PlayableBreedEnum BreedId
        {
            get
            {
                return Record.Breed;
            }
            set { Record.Breed = value; }
        }

        /// <summary>
        ///   Sex of this character.
        /// </summary>
        public SexTypeEnum Sex
        {
            get
            {
                return Record.Sex;
            }
            set
            {
                Record.Sex = value;
            }
        }

        public BreedRecord Breed
        {
            get
            {
                return BreedManager.GetBreed(BreedId);
            }
        }

        /// <summary>
        ///   Character's inventory
        /// </summary>
        public Inventory Inventory
        {
            get;
            private set;
        }

        public IDialogRequest DialogRequest
        {
            get;
            set;
        }

        public bool IsDialogRequested
        {
            get
            {
                return DialogRequest != null;
            }
        }

        public Dialoger Dialoger
        {
            get;
            set;
        }

        public IDialog Dialog
        {
            get
            {
                return Dialoger.Dialog;
            }
        }

        public bool IsInDialog
        {
            get
            {
                return Dialoger != null;
            }
        }

        public bool IsInTrade
        {
            get
            {
                return Dialoger != null && Dialoger is Trader;
            }
        }

        public bool IsInDialogWithNpc
        {
            get
            {
                return Dialoger != null && Dialoger is NpcDialoger;
            }
        }

        public bool IsAway
        {
            get;
            private set;
        }

        public bool IsOccuped
        {
            get
            {
                return IsInDialog || IsDialogRequested || IsAway;
            }
        }

    }
}
