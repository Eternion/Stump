-- teleport isle 1 -> isle 2
SET @npcId = 194;
INSERT INTO npcs_actions (RecognizerType, Npc, `Condition`, Talk_MessageId) VALUES ("Talk", @npcId, "PL<80", 8991); -- level < 80
INSERT INTO npcs_actions (RecognizerType, Npc, `Condition`, Talk_MessageId) VALUES ("Talk", @npcId, "PL>79", 10619); -- level >= 80
INSERT INTO npcs_replies (RecognizerType, Reply, Message, Teleport_Map, Teleport_Cell, Teleport_Direction) VALUES ("Teleport", 23244, 10619, 54172501, 145, 1); 
