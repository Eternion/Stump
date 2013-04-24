DELETE FROM monsters_spells WHERE SpellId NOT IN (SELECT Id FROM spells_templates);
DELETE FROM monsters_spawns WHERE MonsterId NOT IN (SELECT Id FROM monsters_templates);