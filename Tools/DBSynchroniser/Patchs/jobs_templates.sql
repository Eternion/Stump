-- EXECUTE ON:jobs_templates

UPDATE jobs_templates SET HarvestedCountMax=7 WHERE Id IN (2,26,28);
UPDATE jobs_templates SET HarvestedCountMax=1 WHERE Id IN (1,24);