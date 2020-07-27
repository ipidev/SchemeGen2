using Microsoft.Win32;
using SchemeGen2UI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchemeGen2UI
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			if (String.IsNullOrEmpty(Settings.Default.OutputDirectory))
			{
				string schemesFolder = FindSchemesFolder();
				if (schemesFolder != null)
				{
					Settings.Default.OutputDirectory = schemesFolder;
					Settings.Default.Save();
				}
			}

			PopulateMetaschemesListBox();

			extendedSchemeOptionsComboBox.Text = "Use";
		}

		MessageLog _currentMessageLog = null;

		string FindSchemesFolder()
		{
			object fromWARegistry = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Team17SoftwareLTD\\WormsArmageddon", "PATH", null);
			if (fromWARegistry != null && fromWARegistry is string)
			{
				string schemesDirectory = Path.Combine((string)fromWARegistry, "User\\Schemes");
				if (Directory.Exists(schemesDirectory))
				{
					return schemesDirectory;
				}
			}

			object fromSteamRegistry = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Valve\\Steam", "SteamPath", null);
			if (fromSteamRegistry != null && fromWARegistry is string)
			{
				string schemesDirectory = Path.Combine((string)fromSteamRegistry, "steamapps\\common\\Worms Armageddon\\User\\Schemes");
				if (Directory.Exists(schemesDirectory))
				{
					return schemesDirectory;
				}
			}

			return null;
		}

		void PopulateMetaschemesListBox()
		{
			metaschemesListBox.Items.Clear();

			if (Directory.Exists("Metaschemes"))
			{
				string[] metaschemePaths = Directory.GetFiles("Metaschemes", "*.xml", SearchOption.AllDirectories);
				foreach (string metaschemePath in metaschemePaths)
				{
					try
					{
						metaschemesListBox.Items.Add(new MetaschemeFileInfo(metaschemePath));
					}
					catch (InvalidMetaschemeFileException)
					{
					}
				}
			}
		}

		private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PopulateMetaschemesListBox();
		}
		private void metaschemesListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			generateButton.Enabled = metaschemesListBox.SelectedIndices.Count > 0;

			MetaschemeFileInfo metaschemeFileInfo = null;
			if (metaschemesListBox.SelectedIndex >= 0)
			{
				metaschemeFileInfo = (MetaschemeFileInfo)metaschemesListBox.Items[metaschemesListBox.SelectedIndex];
			}
			
			if (metaschemeFileInfo != null)
			{
				metaschemeNameLabel.Text = metaschemeFileInfo.DisplayName;
				metaschemeAuthorLabel.Text = metaschemeFileInfo.AuthorName;
				metaschemeDescriptionTextBox.Text = metaschemeFileInfo.Description;
			}
			else
			{
				metaschemeNameLabel.Text = "";
				metaschemeAuthorLabel.Text = "";
				metaschemeDescriptionTextBox.Text = "";
			}
		}

		private void outputDirectoryButton_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog2 folderBrowserDialog2 = new FolderBrowserDialog2();
			folderBrowserDialog2.DirectoryPath = Settings.Default.OutputDirectory;

			if (folderBrowserDialog2.ShowDialog(this) == DialogResult.OK)
			{
				Settings.Default.OutputDirectory = folderBrowserDialog2.DirectoryPath;
				Settings.Default.Save();
			}
		}

		private void generateButton_Click(object sender, EventArgs e)
		{
			if (metaschemesListBox.SelectedItems.Count == 0)
			{
				MessageBox.Show(this, "No metaschemes selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (String.IsNullOrEmpty(Settings.Default.OutputDirectory)) 
			{
				MessageBox.Show(this, "No output directory specified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			MetaschemeFileInfo[] metaschemeFileInfos = metaschemesListBox.SelectedItems.Cast<MetaschemeFileInfo>().ToArray();

			int? randomSeed = GetRandomSeed();
			Random rng = randomSeed.HasValue ? new Random(randomSeed.Value) : new Random();
			StringWriter stringWriter = new StringWriter();

			for (int i = 0; i < (int)numberOfSchemesUpDown.Value; ++i)
			{
				int metaSchemeIndex = i % (int)metaschemeFileInfos.Length;

				//Shuffle once we've added one of each.
				if (metaSchemeIndex == 0)
				{
					for (int j = metaschemeFileInfos.Length - 1; j >= 1; --j)
					{
						int randomIndex = rng.Next(j);
						MetaschemeFileInfo temp = metaschemeFileInfos[j];
						metaschemeFileInfos[j] = metaschemeFileInfos[randomIndex];
						metaschemeFileInfos[randomIndex] = temp;
					}
				}

				MetaschemeFileInfo metaschemeFileInfo = metaschemeFileInfos[metaSchemeIndex];
				if (metaschemeFileInfo == null)
					continue;

				bool shouldUseExtendedSchemeOptions = ShouldUseExtendedSchemeOptions(rng);
				string outputPath = GetNewMetaschemeFilePath(metaschemeFileInfo, useGenericNameCheckBox.Checked, i);

				try
				{
					SchemeGen2.SchemeGen2.Generate(metaschemeFileInfo.FullPath, outputPath, randomSeed, stringWriter);
				}
				catch (Exception ex)
				{
					stringWriter.WriteLine("Exception occurred while writing to file {0}: {1}\r\nStack:\r\n{2}", outputPath, ex.Message, ex.StackTrace);
				}
			}

			//Output to message log.
			string messageLogString = stringWriter.ToString();
			if (_currentMessageLog == null)
			{
				if (!String.IsNullOrEmpty(messageLogString))
				{
					_currentMessageLog = new MessageLog(messageLogString);
					_currentMessageLog.FormClosing += delegate(object o, FormClosingEventArgs ee) { _currentMessageLog = null; };
					_currentMessageLog.Show();
				}
			}
			else
			{
				_currentMessageLog.UpdateMessage(!String.IsNullOrEmpty(messageLogString) ? messageLogString : "Scheme(s) generated successfully.");
			}

			stringWriter.Close();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (_currentMessageLog != null)
			{
				_currentMessageLog.Close();
			}
		}

		string GetNewMetaschemeFilePath(MetaschemeFileInfo metaschemeFileInfo, bool useGenericName, int index = 0)
		{
			string newFilePath = null;

			string nameString = useGenericName ? "RandomScheme" : Path.GetFileNameWithoutExtension(metaschemeFileInfo.FullPath);
			string dateString = DateTime.Now.ToString("yyyMMdd");

			while (index++ < 99)
			{
				string newFilename = String.Format("{0}_{1}_{2}.wsc", dateString, nameString, index.ToString("D2"));
				newFilePath = Path.Combine(Settings.Default.OutputDirectory, newFilename);

				if (!File.Exists(newFilePath))
					return newFilePath;
			}

			return newFilePath;
		}

		int? GetRandomSeed()
		{
			if (String.IsNullOrWhiteSpace(randomSeedTextBox.Text))
				return null;

			if (Int32.TryParse(randomSeedTextBox.Text, out int seedAsInt))
				return seedAsInt;

			return randomSeedTextBox.Text.GetHashCode();
		}

		bool ShouldUseExtendedSchemeOptions(Random rng)
		{
			switch (extendedSchemeOptionsComboBox.SelectedIndex)
			{
			case 0:
				return true;

			case 2:
				return rng.NextDouble() >= 0.5;
			}

			return false;
		}
	}
}
