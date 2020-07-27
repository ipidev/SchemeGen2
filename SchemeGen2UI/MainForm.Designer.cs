namespace SchemeGen2UI
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.Label numberOfSchemesLabel;
			System.Windows.Forms.Label outputDirectoryLabel;
			System.Windows.Forms.Panel metaschemesRightPanel;
			System.Windows.Forms.Label metaschemeAuthorLabelLabel;
			System.Windows.Forms.Label metaschemeNameLabelLabel;
			System.Windows.Forms.Label useGenericNameLabel;
			System.Windows.Forms.Label extendedSchemeOptionsLabel;
			System.Windows.Forms.Label randomSeedLabel;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.metaschemeDescriptionTextBox = new System.Windows.Forms.TextBox();
			this.metaschemeAuthorLabel = new System.Windows.Forms.Label();
			this.metaschemeNameLabel = new System.Windows.Forms.Label();
			this.listBoxContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.validateSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.metaschemesListBox = new System.Windows.Forms.ListBox();
			this.numberOfSchemesUpDown = new System.Windows.Forms.NumericUpDown();
			this.outputDirectoryTextBox = new System.Windows.Forms.TextBox();
			this.outputDirectoryButton = new System.Windows.Forms.Button();
			this.generateButton = new System.Windows.Forms.Button();
			this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.metaschemesGroupBox = new System.Windows.Forms.GroupBox();
			this.metaschemesTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.generationGroupBox = new System.Windows.Forms.GroupBox();
			this.generationTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.generationLeftPanel = new System.Windows.Forms.Panel();
			this.randomSeedTextBox = new System.Windows.Forms.TextBox();
			this.extendedSchemeOptionsComboBox = new System.Windows.Forms.ComboBox();
			this.useGenericNameCheckBox = new System.Windows.Forms.CheckBox();
			this.generationRightPanel = new System.Windows.Forms.Panel();
			numberOfSchemesLabel = new System.Windows.Forms.Label();
			outputDirectoryLabel = new System.Windows.Forms.Label();
			metaschemesRightPanel = new System.Windows.Forms.Panel();
			metaschemeAuthorLabelLabel = new System.Windows.Forms.Label();
			metaschemeNameLabelLabel = new System.Windows.Forms.Label();
			useGenericNameLabel = new System.Windows.Forms.Label();
			extendedSchemeOptionsLabel = new System.Windows.Forms.Label();
			randomSeedLabel = new System.Windows.Forms.Label();
			metaschemesRightPanel.SuspendLayout();
			this.listBoxContextMenuStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numberOfSchemesUpDown)).BeginInit();
			this.mainTableLayoutPanel.SuspendLayout();
			this.metaschemesGroupBox.SuspendLayout();
			this.metaschemesTableLayoutPanel.SuspendLayout();
			this.generationGroupBox.SuspendLayout();
			this.generationTableLayoutPanel.SuspendLayout();
			this.generationLeftPanel.SuspendLayout();
			this.generationRightPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// numberOfSchemesLabel
			// 
			numberOfSchemesLabel.AutoSize = true;
			numberOfSchemesLabel.Location = new System.Drawing.Point(0, 2);
			numberOfSchemesLabel.Name = "numberOfSchemesLabel";
			numberOfSchemesLabel.Size = new System.Drawing.Size(113, 13);
			numberOfSchemesLabel.TabIndex = 2;
			numberOfSchemesLabel.Text = "Schemes to Generate:";
			// 
			// outputDirectoryLabel
			// 
			outputDirectoryLabel.AutoSize = true;
			outputDirectoryLabel.Location = new System.Drawing.Point(4, 0);
			outputDirectoryLabel.Name = "outputDirectoryLabel";
			outputDirectoryLabel.Size = new System.Drawing.Size(87, 13);
			outputDirectoryLabel.TabIndex = 5;
			outputDirectoryLabel.Text = "Output Directory:";
			// 
			// metaschemesRightPanel
			// 
			metaschemesRightPanel.Controls.Add(this.metaschemeDescriptionTextBox);
			metaschemesRightPanel.Controls.Add(this.metaschemeAuthorLabel);
			metaschemesRightPanel.Controls.Add(metaschemeAuthorLabelLabel);
			metaschemesRightPanel.Controls.Add(this.metaschemeNameLabel);
			metaschemesRightPanel.Controls.Add(metaschemeNameLabelLabel);
			metaschemesRightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			metaschemesRightPanel.Location = new System.Drawing.Point(239, 3);
			metaschemesRightPanel.Name = "metaschemesRightPanel";
			metaschemesRightPanel.Size = new System.Drawing.Size(230, 139);
			metaschemesRightPanel.TabIndex = 2;
			// 
			// metaschemeDescriptionTextBox
			// 
			this.metaschemeDescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.metaschemeDescriptionTextBox.Location = new System.Drawing.Point(6, 34);
			this.metaschemeDescriptionTextBox.Multiline = true;
			this.metaschemeDescriptionTextBox.Name = "metaschemeDescriptionTextBox";
			this.metaschemeDescriptionTextBox.ReadOnly = true;
			this.metaschemeDescriptionTextBox.Size = new System.Drawing.Size(221, 102);
			this.metaschemeDescriptionTextBox.TabIndex = 4;
			this.metaschemeDescriptionTextBox.TabStop = false;
			// 
			// metaschemeAuthorLabel
			// 
			this.metaschemeAuthorLabel.AutoSize = true;
			this.metaschemeAuthorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.metaschemeAuthorLabel.Location = new System.Drawing.Point(52, 17);
			this.metaschemeAuthorLabel.Name = "metaschemeAuthorLabel";
			this.metaschemeAuthorLabel.Size = new System.Drawing.Size(0, 13);
			this.metaschemeAuthorLabel.TabIndex = 3;
			this.metaschemeAuthorLabel.UseMnemonic = false;
			// 
			// metaschemeAuthorLabelLabel
			// 
			metaschemeAuthorLabelLabel.AutoSize = true;
			metaschemeAuthorLabelLabel.Location = new System.Drawing.Point(4, 17);
			metaschemeAuthorLabelLabel.Name = "metaschemeAuthorLabelLabel";
			metaschemeAuthorLabelLabel.Size = new System.Drawing.Size(41, 13);
			metaschemeAuthorLabelLabel.TabIndex = 2;
			metaschemeAuthorLabelLabel.Text = "Author:";
			metaschemeAuthorLabelLabel.UseMnemonic = false;
			// 
			// metaschemeNameLabel
			// 
			this.metaschemeNameLabel.AutoSize = true;
			this.metaschemeNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.metaschemeNameLabel.Location = new System.Drawing.Point(47, 0);
			this.metaschemeNameLabel.Name = "metaschemeNameLabel";
			this.metaschemeNameLabel.Size = new System.Drawing.Size(0, 13);
			this.metaschemeNameLabel.TabIndex = 1;
			this.metaschemeNameLabel.UseMnemonic = false;
			// 
			// metaschemeNameLabelLabel
			// 
			metaschemeNameLabelLabel.AutoSize = true;
			metaschemeNameLabelLabel.Location = new System.Drawing.Point(3, 0);
			metaschemeNameLabelLabel.Name = "metaschemeNameLabelLabel";
			metaschemeNameLabelLabel.Size = new System.Drawing.Size(38, 13);
			metaschemeNameLabelLabel.TabIndex = 0;
			metaschemeNameLabelLabel.Text = "Name:";
			metaschemeNameLabelLabel.UseMnemonic = false;
			// 
			// useGenericNameLabel
			// 
			useGenericNameLabel.AutoSize = true;
			useGenericNameLabel.Location = new System.Drawing.Point(0, 53);
			useGenericNameLabel.Name = "useGenericNameLabel";
			useGenericNameLabel.Size = new System.Drawing.Size(119, 13);
			useGenericNameLabel.TabIndex = 5;
			useGenericNameLabel.Text = "Use Generic Filenames:";
			// 
			// extendedSchemeOptionsLabel
			// 
			extendedSchemeOptionsLabel.AutoSize = true;
			extendedSchemeOptionsLabel.Location = new System.Drawing.Point(0, 29);
			extendedSchemeOptionsLabel.Name = "extendedSchemeOptionsLabel";
			extendedSchemeOptionsLabel.Size = new System.Drawing.Size(136, 13);
			extendedSchemeOptionsLabel.TabIndex = 6;
			extendedSchemeOptionsLabel.Text = "Extended Scheme Options:";
			// 
			// randomSeedLabel
			// 
			randomSeedLabel.AutoSize = true;
			randomSeedLabel.Location = new System.Drawing.Point(0, 75);
			randomSeedLabel.Name = "randomSeedLabel";
			randomSeedLabel.Size = new System.Drawing.Size(78, 13);
			randomSeedLabel.TabIndex = 8;
			randomSeedLabel.Text = "Random Seed:";
			// 
			// listBoxContextMenuStrip
			// 
			this.listBoxContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.validateSelectedToolStripMenuItem});
			this.listBoxContextMenuStrip.Name = "listBoxContextMenuStrip";
			this.listBoxContextMenuStrip.Size = new System.Drawing.Size(164, 48);
			// 
			// refreshToolStripMenuItem
			// 
			this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
			this.refreshToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.refreshToolStripMenuItem.Text = "Refresh";
			this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
			// 
			// validateSelectedToolStripMenuItem
			// 
			this.validateSelectedToolStripMenuItem.Enabled = false;
			this.validateSelectedToolStripMenuItem.Name = "validateSelectedToolStripMenuItem";
			this.validateSelectedToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.validateSelectedToolStripMenuItem.Text = "Validate Selected";
			// 
			// metaschemesListBox
			// 
			this.metaschemesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.metaschemesListBox.ContextMenuStrip = this.listBoxContextMenuStrip;
			this.metaschemesListBox.FormattingEnabled = true;
			this.metaschemesListBox.IntegralHeight = false;
			this.metaschemesListBox.Location = new System.Drawing.Point(3, 3);
			this.metaschemesListBox.Name = "metaschemesListBox";
			this.metaschemesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.metaschemesListBox.Size = new System.Drawing.Size(230, 139);
			this.metaschemesListBox.TabIndex = 1;
			this.metaschemesListBox.SelectedIndexChanged += new System.EventHandler(this.metaschemesListBox_SelectedIndexChanged);
			// 
			// numberOfSchemesUpDown
			// 
			this.numberOfSchemesUpDown.Location = new System.Drawing.Point(139, 0);
			this.numberOfSchemesUpDown.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
			this.numberOfSchemesUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numberOfSchemesUpDown.Name = "numberOfSchemesUpDown";
			this.numberOfSchemesUpDown.Size = new System.Drawing.Size(39, 20);
			this.numberOfSchemesUpDown.TabIndex = 3;
			this.numberOfSchemesUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// outputDirectoryTextBox
			// 
			this.outputDirectoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.outputDirectoryTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SchemeGen2UI.Properties.Settings.Default, "OutputDirectory", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.outputDirectoryTextBox.Location = new System.Drawing.Point(6, 16);
			this.outputDirectoryTextBox.Name = "outputDirectoryTextBox";
			this.outputDirectoryTextBox.ReadOnly = true;
			this.outputDirectoryTextBox.Size = new System.Drawing.Size(190, 20);
			this.outputDirectoryTextBox.TabIndex = 6;
			this.outputDirectoryTextBox.TabStop = false;
			this.outputDirectoryTextBox.Text = global::SchemeGen2UI.Properties.Settings.Default.OutputDirectory;
			// 
			// outputDirectoryButton
			// 
			this.outputDirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.outputDirectoryButton.Image = global::SchemeGen2UI.Properties.Resources.open;
			this.outputDirectoryButton.Location = new System.Drawing.Point(202, 13);
			this.outputDirectoryButton.Name = "outputDirectoryButton";
			this.outputDirectoryButton.Size = new System.Drawing.Size(25, 23);
			this.outputDirectoryButton.TabIndex = 7;
			this.outputDirectoryButton.UseVisualStyleBackColor = true;
			this.outputDirectoryButton.Click += new System.EventHandler(this.outputDirectoryButton_Click);
			// 
			// generateButton
			// 
			this.generateButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.generateButton.Enabled = false;
			this.generateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.generateButton.Location = new System.Drawing.Point(6, 42);
			this.generateButton.Name = "generateButton";
			this.generateButton.Size = new System.Drawing.Size(221, 66);
			this.generateButton.TabIndex = 8;
			this.generateButton.Text = "Generate";
			this.generateButton.UseVisualStyleBackColor = true;
			this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
			// 
			// mainTableLayoutPanel
			// 
			this.mainTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.mainTableLayoutPanel.ColumnCount = 1;
			this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.mainTableLayoutPanel.Controls.Add(this.metaschemesGroupBox, 0, 0);
			this.mainTableLayoutPanel.Controls.Add(this.generationGroupBox, 0, 1);
			this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
			this.mainTableLayoutPanel.RowCount = 2;
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 142F));
			this.mainTableLayoutPanel.Size = new System.Drawing.Size(484, 312);
			this.mainTableLayoutPanel.TabIndex = 9;
			// 
			// metaschemesGroupBox
			// 
			this.metaschemesGroupBox.Controls.Add(this.metaschemesTableLayoutPanel);
			this.metaschemesGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.metaschemesGroupBox.Location = new System.Drawing.Point(3, 3);
			this.metaschemesGroupBox.Name = "metaschemesGroupBox";
			this.metaschemesGroupBox.Size = new System.Drawing.Size(478, 164);
			this.metaschemesGroupBox.TabIndex = 0;
			this.metaschemesGroupBox.TabStop = false;
			this.metaschemesGroupBox.Text = "Available Metaschemes";
			// 
			// metaschemesTableLayoutPanel
			// 
			this.metaschemesTableLayoutPanel.ColumnCount = 2;
			this.metaschemesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.metaschemesTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.metaschemesTableLayoutPanel.Controls.Add(this.metaschemesListBox, 0, 0);
			this.metaschemesTableLayoutPanel.Controls.Add(metaschemesRightPanel, 1, 0);
			this.metaschemesTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.metaschemesTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
			this.metaschemesTableLayoutPanel.Name = "metaschemesTableLayoutPanel";
			this.metaschemesTableLayoutPanel.RowCount = 1;
			this.metaschemesTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.metaschemesTableLayoutPanel.Size = new System.Drawing.Size(472, 145);
			this.metaschemesTableLayoutPanel.TabIndex = 0;
			// 
			// generationGroupBox
			// 
			this.generationGroupBox.Controls.Add(this.generationTableLayoutPanel);
			this.generationGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.generationGroupBox.Location = new System.Drawing.Point(3, 173);
			this.generationGroupBox.Name = "generationGroupBox";
			this.generationGroupBox.Size = new System.Drawing.Size(478, 136);
			this.generationGroupBox.TabIndex = 1;
			this.generationGroupBox.TabStop = false;
			this.generationGroupBox.Text = "Generation Options";
			// 
			// generationTableLayoutPanel
			// 
			this.generationTableLayoutPanel.ColumnCount = 2;
			this.generationTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.generationTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.generationTableLayoutPanel.Controls.Add(this.generationLeftPanel, 0, 0);
			this.generationTableLayoutPanel.Controls.Add(this.generationRightPanel, 1, 0);
			this.generationTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.generationTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
			this.generationTableLayoutPanel.Name = "generationTableLayoutPanel";
			this.generationTableLayoutPanel.RowCount = 1;
			this.generationTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.generationTableLayoutPanel.Size = new System.Drawing.Size(472, 117);
			this.generationTableLayoutPanel.TabIndex = 0;
			// 
			// generationLeftPanel
			// 
			this.generationLeftPanel.Controls.Add(this.randomSeedTextBox);
			this.generationLeftPanel.Controls.Add(randomSeedLabel);
			this.generationLeftPanel.Controls.Add(this.extendedSchemeOptionsComboBox);
			this.generationLeftPanel.Controls.Add(extendedSchemeOptionsLabel);
			this.generationLeftPanel.Controls.Add(useGenericNameLabel);
			this.generationLeftPanel.Controls.Add(this.useGenericNameCheckBox);
			this.generationLeftPanel.Controls.Add(numberOfSchemesLabel);
			this.generationLeftPanel.Controls.Add(this.numberOfSchemesUpDown);
			this.generationLeftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.generationLeftPanel.Location = new System.Drawing.Point(3, 3);
			this.generationLeftPanel.Name = "generationLeftPanel";
			this.generationLeftPanel.Size = new System.Drawing.Size(230, 111);
			this.generationLeftPanel.TabIndex = 0;
			// 
			// randomSeedTextBox
			// 
			this.randomSeedTextBox.Location = new System.Drawing.Point(139, 74);
			this.randomSeedTextBox.Name = "randomSeedTextBox";
			this.randomSeedTextBox.Size = new System.Drawing.Size(88, 20);
			this.randomSeedTextBox.TabIndex = 9;
			// 
			// extendedSchemeOptionsComboBox
			// 
			this.extendedSchemeOptionsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.extendedSchemeOptionsComboBox.FormattingEnabled = true;
			this.extendedSchemeOptionsComboBox.Items.AddRange(new object[] {
            "Use",
            "Ignore",
            "Random"});
			this.extendedSchemeOptionsComboBox.Location = new System.Drawing.Point(139, 26);
			this.extendedSchemeOptionsComboBox.Name = "extendedSchemeOptionsComboBox";
			this.extendedSchemeOptionsComboBox.Size = new System.Drawing.Size(88, 21);
			this.extendedSchemeOptionsComboBox.TabIndex = 7;
			// 
			// useGenericNameCheckBox
			// 
			this.useGenericNameCheckBox.AutoSize = true;
			this.useGenericNameCheckBox.Location = new System.Drawing.Point(139, 53);
			this.useGenericNameCheckBox.Name = "useGenericNameCheckBox";
			this.useGenericNameCheckBox.Size = new System.Drawing.Size(15, 14);
			this.useGenericNameCheckBox.TabIndex = 4;
			this.useGenericNameCheckBox.UseVisualStyleBackColor = true;
			// 
			// generationRightPanel
			// 
			this.generationRightPanel.Controls.Add(outputDirectoryLabel);
			this.generationRightPanel.Controls.Add(this.outputDirectoryTextBox);
			this.generationRightPanel.Controls.Add(this.generateButton);
			this.generationRightPanel.Controls.Add(this.outputDirectoryButton);
			this.generationRightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.generationRightPanel.Location = new System.Drawing.Point(239, 3);
			this.generationRightPanel.Name = "generationRightPanel";
			this.generationRightPanel.Size = new System.Drawing.Size(230, 111);
			this.generationRightPanel.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(484, 312);
			this.Controls.Add(this.mainTableLayoutPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(500, 300);
			this.Name = "MainForm";
			this.Text = "WA Random Scheme Generator";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			metaschemesRightPanel.ResumeLayout(false);
			metaschemesRightPanel.PerformLayout();
			this.listBoxContextMenuStrip.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numberOfSchemesUpDown)).EndInit();
			this.mainTableLayoutPanel.ResumeLayout(false);
			this.metaschemesGroupBox.ResumeLayout(false);
			this.metaschemesTableLayoutPanel.ResumeLayout(false);
			this.generationGroupBox.ResumeLayout(false);
			this.generationTableLayoutPanel.ResumeLayout(false);
			this.generationLeftPanel.ResumeLayout(false);
			this.generationLeftPanel.PerformLayout();
			this.generationRightPanel.ResumeLayout(false);
			this.generationRightPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ContextMenuStrip listBoxContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem validateSelectedToolStripMenuItem;
		private System.Windows.Forms.ListBox metaschemesListBox;
		private System.Windows.Forms.NumericUpDown numberOfSchemesUpDown;
		private System.Windows.Forms.TextBox outputDirectoryTextBox;
		private System.Windows.Forms.Button outputDirectoryButton;
		private System.Windows.Forms.Button generateButton;
		private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
		private System.Windows.Forms.GroupBox metaschemesGroupBox;
		private System.Windows.Forms.GroupBox generationGroupBox;
		private System.Windows.Forms.TableLayoutPanel metaschemesTableLayoutPanel;
		private System.Windows.Forms.TableLayoutPanel generationTableLayoutPanel;
		private System.Windows.Forms.Panel generationLeftPanel;
		private System.Windows.Forms.Panel generationRightPanel;
		private System.Windows.Forms.Label metaschemeNameLabel;
		private System.Windows.Forms.Label metaschemeAuthorLabel;
		private System.Windows.Forms.TextBox metaschemeDescriptionTextBox;
		private System.Windows.Forms.CheckBox useGenericNameCheckBox;
		private System.Windows.Forms.ComboBox extendedSchemeOptionsComboBox;
		private System.Windows.Forms.TextBox randomSeedTextBox;
	}
}

