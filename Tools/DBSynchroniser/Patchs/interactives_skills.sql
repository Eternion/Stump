-- EXECUTE ON:interactives_skills_templates

INSERT INTO interactives_skills (Type, Parameter0, Parameter1) SELECT "JobBook", GROUP_CONCAT(DISTINCT ist.ParentJobId), is1.Id FROM interactives_spawns AS is2 INNER JOIN interactives_spawns AS is1 ON is2.MapId=is1.MapId INNER JOIN interactives_skills_templates AS ist ON ist.InteractiveId=is2.TemplateId WHERE is1.ElementId IN(21817,21816, 44929) GROUP BY is1.Id;
INSERT INTO interactives_spawns_skills (InteractiveSpawnId, SkillId) SELECT isk.Parameter1, isk.Id FROM interactives_skills AS isk WHERE Type="JobBook";
UPDATE interactives_skills SET Parameter1=NULL WHERE Type="JobBook";