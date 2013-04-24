﻿namespace Stump.Tools.Sniffer
 {
     partial class FormMain
     {
         /// <summary>
         /// Variable nécessaire au concepteur.
         /// </summary>
         private System.ComponentModel.IContainer components = null;

         /// <summary>
         /// Nettoyage des ressources utilisées.
         /// </summary>
         /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
         protected override void Dispose(bool disposing)
         {
             if (disposing && (components != null))
             {
                 components.Dispose();
             }
             base.Dispose(disposing);
         }

         #region Code généré par le Concepteur Windows Form

         /// <summary>
         /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
         /// le contenu de cette méthode avec l'éditeur de code.
         /// </summary>
         private void InitializeComponent()
         {
             this.components = new System.ComponentModel.Container();
             System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
             this.messageTreeView = new System.Windows.Forms.TreeView();
             this.treeViewMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
             this.BtnExportClass = new System.Windows.Forms.ToolStripMenuItem();
             this.messageListView = new System.Windows.Forms.ListView();
             this.ID = ( (System.Windows.Forms.ColumnHeader)( new System.Windows.Forms.ColumnHeader() ) );
             this.PacketName = ( (System.Windows.Forms.ColumnHeader)( new System.Windows.Forms.ColumnHeader() ) );
             this.From = ( (System.Windows.Forms.ColumnHeader)( new System.Windows.Forms.ColumnHeader() ) );
             this.listViewMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
             this.BtnExportMessage = new System.Windows.Forms.ToolStripMenuItem();
             this.BtnRemoveMessage = new System.Windows.Forms.ToolStripMenuItem();
             this.BtnRemoveAllMessage = new System.Windows.Forms.ToolStripMenuItem();
             this.imageListIco = new System.Windows.Forms.ImageList(this.components);
             this.topToolStrip = new System.Windows.Forms.ToolStrip();
             this.BtnStart = new System.Windows.Forms.ToolStripButton();
             this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
             this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
             this.LbByteNumber = new System.Windows.Forms.ToolStripLabel();
             this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
             this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
             this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
             this.LbPacketNumber = new System.Windows.Forms.ToolStripLabel();
             this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
             this.labelPort = new System.Windows.Forms.ToolStripLabel();
             this.imageList32 = new System.Windows.Forms.ImageList(this.components);
             this.imageList24 = new System.Windows.Forms.ImageList(this.components);
             this.panel1 = new System.Windows.Forms.Panel();
             this.btnSend = new System.Windows.Forms.Button();
             this.pGMessage = new System.Windows.Forms.PropertyGrid();
             this.cBSendTo = new System.Windows.Forms.ComboBox();
             this.lbSendTo = new System.Windows.Forms.Label();
             this.cbMessageType = new System.Windows.Forms.ComboBox();
             this.lbMessageType = new System.Windows.Forms.Label();
             this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
             this.toolStripTextBoxPort = new System.Windows.Forms.ToolStripTextBox();
             this.treeViewMenuStrip.SuspendLayout();
             this.listViewMenuStrip.SuspendLayout();
             this.topToolStrip.SuspendLayout();
             this.panel1.SuspendLayout();
             this.SuspendLayout();
             // 
             // messageTreeView
             // 
             this.messageTreeView.BackColor = System.Drawing.SystemColors.ScrollBar;
             this.messageTreeView.ContextMenuStrip = this.treeViewMenuStrip;
             this.messageTreeView.FullRowSelect = true;
             this.messageTreeView.Location = new System.Drawing.Point(319, 28);
             this.messageTreeView.Name = "messageTreeView";
             this.messageTreeView.Size = new System.Drawing.Size(269, 421);
             this.messageTreeView.TabIndex = 1;
             // 
             // treeViewMenuStrip
             // 
             this.treeViewMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnExportClass});
             this.treeViewMenuStrip.Name = "messageMenuStrip";
             this.treeViewMenuStrip.ShowImageMargin = false;
             this.treeViewMenuStrip.Size = new System.Drawing.Size(97, 28);
             // 
             // BtnExportClass
             // 
             this.BtnExportClass.Name = "BtnExportClass";
             this.BtnExportClass.Size = new System.Drawing.Size(96, 24);
             this.BtnExportClass.Text = "Export";
             this.BtnExportClass.Click += new System.EventHandler(this.BtnExportClass_Click);
             // 
             // messageListView
             // 
             this.messageListView.AllowColumnReorder = true;
             this.messageListView.BackColor = System.Drawing.SystemColors.ScrollBar;
             this.messageListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.PacketName,
            this.From});
             this.messageListView.ContextMenuStrip = this.listViewMenuStrip;
             this.messageListView.Cursor = System.Windows.Forms.Cursors.Arrow;
             this.messageListView.FullRowSelect = true;
             this.messageListView.LargeImageList = this.imageListIco;
             this.messageListView.Location = new System.Drawing.Point(1, 26);
             this.messageListView.Name = "messageListView";
             this.messageListView.Size = new System.Drawing.Size(314, 423);
             this.messageListView.SmallImageList = this.imageListIco;
             this.messageListView.TabIndex = 3;
             this.messageListView.UseCompatibleStateImageBehavior = false;
             this.messageListView.View = System.Windows.Forms.View.Details;
             this.messageListView.SelectedIndexChanged += new System.EventHandler(this.messageListView_SelectedIndexChanged);
             // 
             // ID
             // 
             this.ID.Text = "ID";
             this.ID.Width = 80;
             // 
             // PacketName
             // 
             this.PacketName.Text = "Name";
             this.PacketName.Width = 147;
             // 
             // From
             // 
             this.From.Text = "From";
             this.From.Width = 82;
             // 
             // listViewMenuStrip
             // 
             this.listViewMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnExportMessage,
            this.BtnRemoveMessage,
            this.BtnRemoveAllMessage});
             this.listViewMenuStrip.Name = "listViewMenuStrip";
             this.listViewMenuStrip.ShowImageMargin = false;
             this.listViewMenuStrip.Size = new System.Drawing.Size(130, 76);
             // 
             // BtnExportMessage
             // 
             this.BtnExportMessage.Name = "BtnExportMessage";
             this.BtnExportMessage.Size = new System.Drawing.Size(129, 24);
             this.BtnExportMessage.Text = "Export";
             this.BtnExportMessage.Click += new System.EventHandler(this.BtnExportMessage_Click);
             // 
             // BtnRemoveMessage
             // 
             this.BtnRemoveMessage.Name = "BtnRemoveMessage";
             this.BtnRemoveMessage.Size = new System.Drawing.Size(129, 24);
             this.BtnRemoveMessage.Text = "Remove";
             this.BtnRemoveMessage.Click += new System.EventHandler(this.BtnRemoveMessage_Click);
             // 
             // BtnRemoveAllMessage
             // 
             this.BtnRemoveAllMessage.Name = "BtnRemoveAllMessage";
             this.BtnRemoveAllMessage.Size = new System.Drawing.Size(129, 24);
             this.BtnRemoveAllMessage.Text = "Remove All";
             this.BtnRemoveAllMessage.Click += new System.EventHandler(this.BtnRemoveAllMessage_Click);
             // 
             // imageListIco
             // 
             this.imageListIco.ImageStream = ( (System.Windows.Forms.ImageListStreamer)( resources.GetObject("imageListIco.ImageStream") ) );
             this.imageListIco.TransparentColor = System.Drawing.Color.Transparent;
             this.imageListIco.Images.SetKeyName(0, "check.ico");
             this.imageListIco.Images.SetKeyName(1, "delete.ico");
             this.imageListIco.Images.SetKeyName(2, "find.ico");
             this.imageListIco.Images.SetKeyName(3, "flash_red.ico");
             this.imageListIco.Images.SetKeyName(4, "flash_yellow.ico");
             this.imageListIco.Images.SetKeyName(5, "font.ico");
             this.imageListIco.Images.SetKeyName(6, "heart.ico");
             this.imageListIco.Images.SetKeyName(7, "help2.ico");
             this.imageListIco.Images.SetKeyName(8, "history.ico");
             this.imageListIco.Images.SetKeyName(9, "information.ico");
             this.imageListIco.Images.SetKeyName(10, "information2.ico");
             this.imageListIco.Images.SetKeyName(11, "lifebelt.ico");
             this.imageListIco.Images.SetKeyName(12, "lightbulb.ico");
             this.imageListIco.Images.SetKeyName(13, "lightbulb_on.ico");
             this.imageListIco.Images.SetKeyName(14, "nav_left_blue.ico");
             this.imageListIco.Images.SetKeyName(15, "nav_left_green.ico");
             this.imageListIco.Images.SetKeyName(16, "nav_left_red.ico");
             this.imageListIco.Images.SetKeyName(17, "nav_left_yellow.ico");
             this.imageListIco.Images.SetKeyName(18, "nav_plain_blue.ico");
             this.imageListIco.Images.SetKeyName(19, "nav_plain_green.ico");
             this.imageListIco.Images.SetKeyName(20, "nav_plain_red.ico");
             this.imageListIco.Images.SetKeyName(21, "nav_plain_yellow.ico");
             this.imageListIco.Images.SetKeyName(22, "nav_right_blue.ico");
             this.imageListIco.Images.SetKeyName(23, "nav_right_green.ico");
             this.imageListIco.Images.SetKeyName(24, "nav_right_red.ico");
             this.imageListIco.Images.SetKeyName(25, "nav_right_yellow.ico");
             this.imageListIco.Images.SetKeyName(26, "pointer.ico");
             this.imageListIco.Images.SetKeyName(27, "scroll.ico");
             this.imageListIco.Images.SetKeyName(28, "scroll3.ico");
             this.imageListIco.Images.SetKeyName(29, "sign_warning.ico");
             this.imageListIco.Images.SetKeyName(30, "sign_warning_harmful.ico");
             this.imageListIco.Images.SetKeyName(31, "sign_warning_toxic.ico");
             this.imageListIco.Images.SetKeyName(32, "signpost.ico");
             this.imageListIco.Images.SetKeyName(33, "signpost2.ico");
             this.imageListIco.Images.SetKeyName(34, "spellcheck.ico");
             this.imageListIco.Images.SetKeyName(35, "star_blue.ico");
             this.imageListIco.Images.SetKeyName(36, "star_green.ico");
             this.imageListIco.Images.SetKeyName(37, "star_grey.ico");
             this.imageListIco.Images.SetKeyName(38, "star_red.ico");
             this.imageListIco.Images.SetKeyName(39, "star_yellow.ico");
             this.imageListIco.Images.SetKeyName(40, "auction_hammer.ico");
             this.imageListIco.Images.SetKeyName(41, "bookkeeper.ico");
             this.imageListIco.Images.SetKeyName(42, "briefcase.ico");
             this.imageListIco.Images.SetKeyName(43, "briefcase2.ico");
             this.imageListIco.Images.SetKeyName(44, "businessman.ico");
             this.imageListIco.Images.SetKeyName(45, "businesspeople.ico");
             this.imageListIco.Images.SetKeyName(46, "businesspeople2.ico");
             this.imageListIco.Images.SetKeyName(47, "businesswoman.ico");
             this.imageListIco.Images.SetKeyName(48, "businesswoman2.ico");
             this.imageListIco.Images.SetKeyName(49, "cashier.ico");
             this.imageListIco.Images.SetKeyName(50, "chest.ico");
             this.imageListIco.Images.SetKeyName(51, "coin_gold.ico");
             this.imageListIco.Images.SetKeyName(52, "coin_silver.ico");
             this.imageListIco.Images.SetKeyName(53, "gold_bar.ico");
             this.imageListIco.Images.SetKeyName(54, "handshake.ico");
             this.imageListIco.Images.SetKeyName(55, "message.ico");
             this.imageListIco.Images.SetKeyName(56, "message_add.ico");
             this.imageListIco.Images.SetKeyName(57, "message_information.ico");
             this.imageListIco.Images.SetKeyName(58, "message_warning.ico");
             this.imageListIco.Images.SetKeyName(59, "messages.ico");
             this.imageListIco.Images.SetKeyName(60, "money2.ico");
             this.imageListIco.Images.SetKeyName(61, "moneybag.ico");
             this.imageListIco.Images.SetKeyName(62, "pin_blue.ico");
             this.imageListIco.Images.SetKeyName(63, "pin_green.ico");
             this.imageListIco.Images.SetKeyName(64, "pin_grey.ico");
             this.imageListIco.Images.SetKeyName(65, "pin_orange.ico");
             this.imageListIco.Images.SetKeyName(66, "pin_red.ico");
             this.imageListIco.Images.SetKeyName(67, "pin_yellow.ico");
             this.imageListIco.Images.SetKeyName(68, "pin2_blue.ico");
             this.imageListIco.Images.SetKeyName(69, "pin2_green.ico");
             this.imageListIco.Images.SetKeyName(70, "pin2_grey.ico");
             this.imageListIco.Images.SetKeyName(71, "pin2_orange.ico");
             this.imageListIco.Images.SetKeyName(72, "pin2_red.ico");
             this.imageListIco.Images.SetKeyName(73, "pin2_yellow.ico");
             this.imageListIco.Images.SetKeyName(74, "telephone2.ico");
             this.imageListIco.Images.SetKeyName(75, "thought.ico");
             this.imageListIco.Images.SetKeyName(76, "anchor.ico");
             this.imageListIco.Images.SetKeyName(77, "armour.ico");
             this.imageListIco.Images.SetKeyName(78, "bomb.ico");
             this.imageListIco.Images.SetKeyName(79, "bullets.ico");
             this.imageListIco.Images.SetKeyName(80, "calculator.ico");
             this.imageListIco.Images.SetKeyName(81, "castle.ico");
             this.imageListIco.Images.SetKeyName(82, "client_network.ico");
             this.imageListIco.Images.SetKeyName(83, "earth_connection.ico");
             this.imageListIco.Images.SetKeyName(84, "fire.ico");
             this.imageListIco.Images.SetKeyName(85, "fortress.ico");
             this.imageListIco.Images.SetKeyName(86, "gauntlet.ico");
             this.imageListIco.Images.SetKeyName(87, "helmet.ico");
             this.imageListIco.Images.SetKeyName(88, "home.ico");
             this.imageListIco.Images.SetKeyName(89, "knight.ico");
             this.imageListIco.Images.SetKeyName(90, "knight2.ico");
             this.imageListIco.Images.SetKeyName(91, "plug_add.ico");
             this.imageListIco.Images.SetKeyName(92, "plug_delete.ico");
             this.imageListIco.Images.SetKeyName(93, "robber.ico");
             this.imageListIco.Images.SetKeyName(94, "server_client2.ico");
             this.imageListIco.Images.SetKeyName(95, "airplane.ico");
             this.imageListIco.Images.SetKeyName(96, "alarmclock.ico");
             this.imageListIco.Images.SetKeyName(97, "ambulance.ico");
             this.imageListIco.Images.SetKeyName(98, "angel.ico");
             this.imageListIco.Images.SetKeyName(99, "apple.ico");
             this.imageListIco.Images.SetKeyName(100, "banana.ico");
             this.imageListIco.Images.SetKeyName(101, "band_aid.ico");
             this.imageListIco.Images.SetKeyName(102, "bone.ico");
             this.imageListIco.Images.SetKeyName(103, "boxing_gloves_blue.ico");
             this.imageListIco.Images.SetKeyName(104, "boxing_gloves_red.ico");
             this.imageListIco.Images.SetKeyName(105, "car_compact_green.ico");
             this.imageListIco.Images.SetKeyName(106, "dart.ico");
             this.imageListIco.Images.SetKeyName(107, "devil.ico");
             this.imageListIco.Images.SetKeyName(108, "dice_blue.ico");
             this.imageListIco.Images.SetKeyName(109, "dice_red.ico");
             this.imageListIco.Images.SetKeyName(110, "die_blue.ico");
             this.imageListIco.Images.SetKeyName(111, "die_gold.ico");
             this.imageListIco.Images.SetKeyName(112, "die_red.ico");
             this.imageListIco.Images.SetKeyName(113, "dog.ico");
             this.imageListIco.Images.SetKeyName(114, "fish.ico");
             this.imageListIco.Images.SetKeyName(115, "flower_blue.ico");
             this.imageListIco.Images.SetKeyName(116, "flower_red.ico");
             this.imageListIco.Images.SetKeyName(117, "flower_white.ico");
             this.imageListIco.Images.SetKeyName(118, "flower_yellow.ico");
             this.imageListIco.Images.SetKeyName(119, "ghost.ico");
             this.imageListIco.Images.SetKeyName(120, "helicopter.ico");
             this.imageListIco.Images.SetKeyName(121, "hourglass.ico");
             this.imageListIco.Images.SetKeyName(122, "house.ico");
             this.imageListIco.Images.SetKeyName(123, "houses.ico");
             this.imageListIco.Images.SetKeyName(124, "magician.ico");
             this.imageListIco.Images.SetKeyName(125, "motorbike.ico");
             this.imageListIco.Images.SetKeyName(126, "oldtimer.ico");
             this.imageListIco.Images.SetKeyName(127, "pawn_glass_blue.ico");
             this.imageListIco.Images.SetKeyName(128, "pawn_glass_green.ico");
             this.imageListIco.Images.SetKeyName(129, "pawn_glass_grey.ico");
             this.imageListIco.Images.SetKeyName(130, "pawn_glass_red.ico");
             this.imageListIco.Images.SetKeyName(131, "pawn_glass_yellow.ico");
             this.imageListIco.Images.SetKeyName(132, "signal_flag_blue.ico");
             this.imageListIco.Images.SetKeyName(133, "signal_flag_green.ico");
             this.imageListIco.Images.SetKeyName(134, "signal_flag_red.ico");
             this.imageListIco.Images.SetKeyName(135, "signal_flag_white.ico");
             this.imageListIco.Images.SetKeyName(136, "signal_flag_yellow.ico");
             this.imageListIco.Images.SetKeyName(137, "skull.ico");
             this.imageListIco.Images.SetKeyName(138, "spider.ico");
             this.imageListIco.Images.SetKeyName(139, "spider2.ico");
             this.imageListIco.Images.SetKeyName(140, "sports_car.ico");
             this.imageListIco.Images.SetKeyName(141, "target3.ico");
             this.imageListIco.Images.SetKeyName(142, "tractor_blue.ico");
             this.imageListIco.Images.SetKeyName(143, "tree.ico");
             this.imageListIco.Images.SetKeyName(144, "truck_blue.ico");
             this.imageListIco.Images.SetKeyName(145, "bug_green.ico");
             this.imageListIco.Images.SetKeyName(146, "bug_red.ico");
             this.imageListIco.Images.SetKeyName(147, "bug_yellow.ico");
             this.imageListIco.Images.SetKeyName(148, "graph_edge_directed.ico");
             this.imageListIco.Images.SetKeyName(149, "shape_circle.ico");
             this.imageListIco.Images.SetKeyName(150, "shape_ellipse.ico");
             this.imageListIco.Images.SetKeyName(151, "shape_hexagon.ico");
             this.imageListIco.Images.SetKeyName(152, "shape_octagon.ico");
             this.imageListIco.Images.SetKeyName(153, "shape_pentagon.ico");
             this.imageListIco.Images.SetKeyName(154, "shape_rectangle.ico");
             this.imageListIco.Images.SetKeyName(155, "shape_rhomb.ico");
             this.imageListIco.Images.SetKeyName(156, "shape_square.ico");
             this.imageListIco.Images.SetKeyName(157, "shape_triangle.ico");
             // 
             // topToolStrip
             // 
             this.topToolStrip.BackColor = System.Drawing.SystemColors.ControlLight;
             this.topToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
             this.topToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnStart,
            this.toolStripSeparator1,
            this.toolStripLabel3,
            this.LbByteNumber,
            this.toolStripSeparator5,
            this.toolStripSeparator6,
            this.toolStripLabel1,
            this.LbPacketNumber,
            this.toolStripSeparator2,
            this.toolStripTextBoxPort,
            this.labelPort});
             this.topToolStrip.Location = new System.Drawing.Point(0, 0);
             this.topToolStrip.Name = "topToolStrip";
             this.topToolStrip.Size = new System.Drawing.Size(922, 25);
             this.topToolStrip.TabIndex = 4;
             this.topToolStrip.Text = "toolStrip1";
             // 
             // BtnStart
             // 
             this.BtnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
             this.BtnStart.Name = "BtnStart";
             this.BtnStart.Size = new System.Drawing.Size(47, 22);
             this.BtnStart.Text = "Offline";
             this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
             // 
             // toolStripSeparator1
             // 
             this.toolStripSeparator1.Name = "toolStripSeparator1";
             this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
             // 
             // toolStripLabel3
             // 
             this.toolStripLabel3.Name = "toolStripLabel3";
             this.toolStripLabel3.Size = new System.Drawing.Size(75, 22);
             this.toolStripLabel3.Text = "Bytes Reçus :";
             // 
             // LbByteNumber
             // 
             this.LbByteNumber.Name = "LbByteNumber";
             this.LbByteNumber.Size = new System.Drawing.Size(13, 22);
             this.LbByteNumber.Text = "0";
             // 
             // toolStripSeparator5
             // 
             this.toolStripSeparator5.Name = "toolStripSeparator5";
             this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
             // 
             // toolStripSeparator6
             // 
             this.toolStripSeparator6.Name = "toolStripSeparator6";
             this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
             // 
             // toolStripLabel1
             // 
             this.toolStripLabel1.Name = "toolStripLabel1";
             this.toolStripLabel1.Size = new System.Drawing.Size(89, 22);
             this.toolStripLabel1.Text = "Paquets Reçus :";
             // 
             // LbPacketNumber
             // 
             this.LbPacketNumber.Name = "LbPacketNumber";
             this.LbPacketNumber.Size = new System.Drawing.Size(13, 22);
             this.LbPacketNumber.Text = "0";
             // 
             // toolStripSeparator2
             // 
             this.toolStripSeparator2.Name = "toolStripSeparator2";
             this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
             // 
             // labelPort
             // 
             this.labelPort.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
             this.labelPort.Name = "labelPort";
             this.labelPort.Size = new System.Drawing.Size(89, 22);
             this.labelPort.Text = "Listening port : ";
             // 
             // imageList32
             // 
             this.imageList32.ImageStream = ( (System.Windows.Forms.ImageListStreamer)( resources.GetObject("imageList32.ImageStream") ) );
             this.imageList32.TransparentColor = System.Drawing.Color.Transparent;
             this.imageList32.Images.SetKeyName(0, "HelicopterMedical.png");
             this.imageList32.Images.SetKeyName(1, "QuadBikeBlue.png");
             this.imageList32.Images.SetKeyName(2, "Red Arrow.png");
             this.imageList32.Images.SetKeyName(3, "reseau_14299.png");
             // 
             // imageList24
             // 
             this.imageList24.ImageStream = ( (System.Windows.Forms.ImageListStreamer)( resources.GetObject("imageList24.ImageStream") ) );
             this.imageList24.TransparentColor = System.Drawing.Color.Transparent;
             this.imageList24.Images.SetKeyName(0, "HelicopterMedical.png");
             this.imageList24.Images.SetKeyName(1, "QuadBikeBlue.png");
             this.imageList24.Images.SetKeyName(2, "Red Arrow.png");
             // 
             // panel1
             // 
             this.panel1.BackColor = System.Drawing.SystemColors.ScrollBar;
             this.panel1.Controls.Add(this.btnSend);
             this.panel1.Controls.Add(this.pGMessage);
             this.panel1.Controls.Add(this.cBSendTo);
             this.panel1.Controls.Add(this.lbSendTo);
             this.panel1.Controls.Add(this.cbMessageType);
             this.panel1.Controls.Add(this.lbMessageType);
             this.panel1.Location = new System.Drawing.Point(594, 28);
             this.panel1.Name = "panel1";
             this.panel1.Size = new System.Drawing.Size(328, 421);
             this.panel1.TabIndex = 5;
             // 
             // btnSend
             // 
             this.btnSend.Location = new System.Drawing.Point(240, 25);
             this.btnSend.Name = "btnSend";
             this.btnSend.Size = new System.Drawing.Size(66, 23);
             this.btnSend.TabIndex = 5;
             this.btnSend.Text = "Send";
             this.btnSend.UseVisualStyleBackColor = true;
             this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
             // 
             // pGMessage
             // 
             this.pGMessage.CommandsVisibleIfAvailable = false;
             this.pGMessage.Location = new System.Drawing.Point(16, 94);
             this.pGMessage.Name = "pGMessage";
             this.pGMessage.Size = new System.Drawing.Size(290, 309);
             this.pGMessage.TabIndex = 4;
             // 
             // cBSendTo
             // 
             this.cBSendTo.FormattingEnabled = true;
             this.cBSendTo.Location = new System.Drawing.Point(16, 25);
             this.cBSendTo.Name = "cBSendTo";
             this.cBSendTo.Size = new System.Drawing.Size(171, 21);
             this.cBSendTo.TabIndex = 3;
             // 
             // lbSendTo
             // 
             this.lbSendTo.AutoSize = true;
             this.lbSendTo.Location = new System.Drawing.Point(15, 9);
             this.lbSendTo.Name = "lbSendTo";
             this.lbSendTo.Size = new System.Drawing.Size(54, 13);
             this.lbSendTo.TabIndex = 2;
             this.lbSendTo.Text = "Send To :";
             // 
             // cbMessageType
             // 
             this.cbMessageType.FormattingEnabled = true;
             this.cbMessageType.Location = new System.Drawing.Point(16, 67);
             this.cbMessageType.Name = "cbMessageType";
             this.cbMessageType.Size = new System.Drawing.Size(290, 21);
             this.cbMessageType.TabIndex = 1;
             // 
             // lbMessageType
             // 
             this.lbMessageType.AutoSize = true;
             this.lbMessageType.Location = new System.Drawing.Point(13, 51);
             this.lbMessageType.Name = "lbMessageType";
             this.lbMessageType.Size = new System.Drawing.Size(79, 13);
             this.lbMessageType.TabIndex = 0;
             this.lbMessageType.Text = "Message type :";
             // 
             // contextMenuStrip1
             // 
             this.contextMenuStrip1.Name = "contextMenuStrip1";
             this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
             // 
             // toolStripTextBoxPort
             // 
             this.toolStripTextBoxPort.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
             this.toolStripTextBoxPort.Name = "toolStripTextBoxPort";
             this.toolStripTextBoxPort.Size = new System.Drawing.Size(100, 25);
             this.toolStripTextBoxPort.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
             // 
             // FormMain
             // 
             this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
             this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
             this.BackColor = System.Drawing.SystemColors.ControlDark;
             this.ClientSize = new System.Drawing.Size(922, 456);
             this.Controls.Add(this.panel1);
             this.Controls.Add(this.topToolStrip);
             this.Controls.Add(this.messageListView);
             this.Controls.Add(this.messageTreeView);
             this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
             this.MaximizeBox = false;
             this.MinimizeBox = false;
             this.Name = "FormMain";
             this.ShowIcon = false;
             this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
             this.Text = "StumpSniffer";
             this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
             this.Load += new System.EventHandler(this.FormMain_Load);
             this.treeViewMenuStrip.ResumeLayout(false);
             this.listViewMenuStrip.ResumeLayout(false);
             this.topToolStrip.ResumeLayout(false);
             this.topToolStrip.PerformLayout();
             this.panel1.ResumeLayout(false);
             this.panel1.PerformLayout();
             this.ResumeLayout(false);
             this.PerformLayout();

         }

         #endregion

         private System.Windows.Forms.TreeView messageTreeView;
         public System.Windows.Forms.ListView messageListView;
         private System.Windows.Forms.ColumnHeader ID;
         private System.Windows.Forms.ColumnHeader PacketName;
         private System.Windows.Forms.ColumnHeader From;
         private System.Windows.Forms.ToolStrip topToolStrip;
         private System.Windows.Forms.ToolStripButton BtnStart;
         private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
         private System.Windows.Forms.ToolStripLabel toolStripLabel3;
         private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
         private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
         private System.Windows.Forms.ToolStripLabel toolStripLabel1;
         private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
         private System.Windows.Forms.ContextMenuStrip listViewMenuStrip;
         private System.Windows.Forms.ToolStripMenuItem BtnRemoveMessage;
         private System.Windows.Forms.ToolStripMenuItem BtnExportMessage;
         private System.Windows.Forms.ContextMenuStrip treeViewMenuStrip;
         private System.Windows.Forms.ToolStripMenuItem BtnExportClass;
         private System.Windows.Forms.ToolStripMenuItem BtnRemoveAllMessage;
         public System.Windows.Forms.ToolStripLabel LbByteNumber;
         public System.Windows.Forms.ToolStripLabel LbPacketNumber;
         private System.Windows.Forms.ToolStripLabel labelPort;
         private System.Windows.Forms.ImageList imageList32;
         private System.Windows.Forms.ImageList imageList24;
         private System.Windows.Forms.ImageList imageListIco;
         private System.Windows.Forms.Panel panel1;
         private System.Windows.Forms.Label lbSendTo;
         private System.Windows.Forms.Label lbMessageType;
         public System.Windows.Forms.PropertyGrid pGMessage;
         public System.Windows.Forms.ComboBox cBSendTo;
         public System.Windows.Forms.ComboBox cbMessageType;
         public System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
         private System.Windows.Forms.Button btnSend;
         public System.Windows.Forms.ToolStripTextBox toolStripTextBoxPort;
     }
 }