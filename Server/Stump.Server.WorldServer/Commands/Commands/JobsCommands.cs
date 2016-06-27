using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.ORM.SubSonic.Extensions;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Database.Jobs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Jobs;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class JobsCommands : SubCommandContainer
    {
        public JobsCommands()
        {
            Aliases = new[] {"jobs"};
            Description = "Gives commands to manage target's jobs";
            RequiredRole = RoleEnum.Moderator;
        }
    }

    public class JobLevelupCommand : TargetSubCommand
    {
        public JobLevelupCommand()
        {
            Aliases = new[] {"levelup", "up"};
            Description = "Level up (or down) target's job";
            RequiredRole = RoleEnum.Moderator;
            ParentCommandType = typeof (JobsCommands);
            AddParameter<string>("job", "j", "Job name or id");
            AddParameter<int>("level", "lvl", "Amount of level to increase (or decrease)");
            AddTargetParameter(true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var jobStr = trigger.Get<string>("job");
            JobTemplate jobTemplate;
            int jobId;
            if (!int.TryParse(jobStr, out jobId))
            {
                jobTemplate = JobManager.Instance.GetJobTemplates().FirstOrDefault(x => x.Name.ToLower().Contains(jobStr.ToLower()));

                if (jobTemplate == null)
                {
                    trigger.ReplyError($"Job {trigger.Bold(jobStr)} not found");
                    return;
                }

                jobId = jobTemplate.Id;
            }
            else
            {
                jobTemplate = JobManager.Instance.GetJobTemplate(jobId);
                if (jobTemplate == null)
                {
                    trigger.ReplyError($"Job {trigger.Bold(jobId)} not found");
                    return;
                }
            }

            var level = trigger.Get<int>("level");

            foreach (var target in GetTargets(trigger))
            {
                var job = target.Jobs[jobId];

                if (job == null)
                {
                    trigger.ReplyError($"{trigger.Bold(target.Name)} doesn't have job {trigger.Bold(jobTemplate.Name)}");
                    continue;
                }

                if (level + job.Level > ExperienceManager.Instance.HighestJobLevel)
                    job.Experience = ExperienceManager.Instance.GetJobLevelExperience(ExperienceManager.Instance.HighestJobLevel);

                else if (level + job.Level < 1)
                    job.Experience = 0;
                else
                    job.Experience = ExperienceManager.Instance.GetJobLevelExperience((byte) (level + job.Level));

                trigger.Reply($"{trigger.Bold(target.Name)} job {trigger.Bold(jobTemplate.Name)} is now level {job.Level}");
            }
        }
    }
}