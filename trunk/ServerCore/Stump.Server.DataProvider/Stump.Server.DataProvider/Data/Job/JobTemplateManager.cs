
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.Job
{
    //public class JobTemplateManager : DataManager<int,JobTemplate>
    //{
    //    protected override JobTemplate InternalGetOne(int id)
    //    {
    //        var job = D2OLoader.LoadData<DofusProtocol.D2oClasses.Job>(id);
    //        var skills = D2OLoader.LoadData<Skill>();

    //            var template = new JobTemplate
    //            {
    //                JobId = job.id,
    //                JobToolIds = job.toolIds,
    //                SpecialisationOfId = job.specializationOfId,
    //                Skills = new List<JobSkill>()
    //            };

    //            template.Skills = skills.Where(s => s.parentJobId == job.id).Select(s =>
    //                new JobSkill
    //                {
    //                    SkillId = s.id,
    //                    Job = template,
    //                    InteractiveId = s.interactiveId,
    //                    ModifiableItemType = (ItemTypeEnum)s.modifiableItemType,
    //                    GatheredRessource = s.gatheredRessourceItem,
    //                    IsRepair = s.isRepair,
    //                    IsForgemagus = s.isForgemagus,
    //                    AvailableInHouse = s.availableInHouse,
    //                    CraftableItemIds = s.craftableItemIds
    //                }).ToList();
    //            return template;
    //        }
        
    //    protected override Dictionary<int, JobTemplate> InternalGetAll()
    //    {
    //        var jobs = D2OLoader.LoadData<DofusProtocol.D2oClasses.Job>();
    //        var skills = D2OLoader.LoadData<Skill>();

    //        var jobsTemplates = new Dictionary<int, JobTemplate>(jobs.Count());
    //        foreach (var job in jobs)
    //        {
    //            var template = new JobTemplate
    //            {
    //                JobId = job.id,
    //                JobToolIds = job.toolIds,
    //                SpecialisationOfId = job.specializationOfId,
    //                Skills = new List<JobSkill>()
    //            };

    //            template.Skills = skills.Where(s => s.parentJobId == job.id).Select(s =>
    //                new JobSkill
    //                {
    //                    SkillId = s.id,
    //                    Job = template,
    //                    InteractiveId = s.interactiveId,
    //                    ModifiableItemType = (ItemTypeEnum)s.modifiableItemType,
    //                    GatheredRessource = s.gatheredRessourceItem,
    //                    IsRepair = s.isRepair,
    //                    IsForgemagus = s.isForgemagus,
    //                    AvailableInHouse = s.availableInHouse,
    //                    CraftableItemIds = s.craftableItemIds
    //                }).ToList();
    //            jobsTemplates.Add(job.id, template);
    //        }
    //        return jobsTemplates;
    //    }
    //}
}