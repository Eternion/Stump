using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.Social;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Social;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class EmoteCommand : AddRemoveCommand
    {
        public EmoteCommand()
        {
            Aliases = new[] {"emote"};
            Description = "Add or remove an emote";
            RequiredRole = RoleEnum.GameMaster;
            AddParameter<int>("id", "id", "Emote id", isOptional:true);
            AddParameter("target", "t", "Emote target", converter: ParametersConverter.CharactersConverter);
            AddParameter<bool>("all", "all", "Add all emotes", isOptional: true);
        }
        
        public Character[] GetTargets(TriggerBase trigger)
        {
            Character[] targets = null;
            if (trigger.IsArgumentDefined("target"))
                targets = trigger.Get<Character[]>("target");
            else if (trigger is GameTrigger)
                targets = new []{(trigger as GameTrigger).Character};

            if (targets == null)
                throw new Exception("Target is not defined");

            if (targets.Length == 0)
                throw new Exception("No target found");

            return targets;
        }

        public override void ExecuteAdd(TriggerBase trigger)
        {
            var targets = GetTargets(trigger);
            IEnumerable<Emote> emotes;

            if (trigger.IsArgumentDefined("all"))
                emotes = ChatManager.Instance.Emotes.Values;
            else if (trigger.IsArgumentDefined("id"))
            {
                var emote = ChatManager.Instance.GetEmote(trigger.Get<int>("id"));
                

                if (emote == null)
                {
                    trigger.ReplyError($"Emote {trigger.Bold(trigger.Get<int>("id"))} not found");
                    return;
                }

                emotes = new[] {emote};
            }
            else
            {
                trigger.ReplyError("Specify an emote or -all");
                return;
            }

            foreach(var target in targets)
            {
                foreach(var emote in emotes)
                    target.AddEmote(emote.EmoteId);
            }
        }

        public override void ExecuteRemove(TriggerBase trigger)
        {
           var targets = GetTargets(trigger);
            IEnumerable<Emote> emotes;

            if (trigger.IsArgumentDefined("all"))
                emotes = ChatManager.Instance.Emotes.Values;
            else if (trigger.IsArgumentDefined("id"))
            {
                var emote = ChatManager.Instance.GetEmote(trigger.Get<int>("id"));
                

                if (emote == null)
                {
                    trigger.ReplyError($"Emote {trigger.Bold(trigger.Get<int>("id"))} not found");
                    return;
                }

                emotes = new[] {emote};
            }
            else
            {
                trigger.ReplyError("Specify an emote or -all");
                return;
            }

            foreach(var target in targets)
            {
                foreach(var emote in emotes)
                    target.RemoveEmote(emote.EmoteId);
            }
        }
    }
}