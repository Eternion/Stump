ALTER TABLE startup_actions_objects
ADD COLUMN Amount int NOT NULL DEFAULT '1' AFTER ItemTemplate;