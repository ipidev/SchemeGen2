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
	public partial class MessageLog : Form
	{
		public MessageLog(string message)
		{
			InitializeComponent();

			UpdateMessage(message);
		}

		public void UpdateMessage(string message)
		{
			textBox.Text = message;
		}
	}
}
