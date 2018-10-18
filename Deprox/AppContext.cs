using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Deprox
{
	class AppContext : ApplicationContext
	{
		private NotifyIcon _trayIcon;
		private ContextMenuStrip _contextMenu;
		private ToolStripMenuItem _closeMenuItem;

		public AppContext()
		{
			Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
			InitializeComponent();
			_trayIcon.Visible = true;
		}

		private void InitializeComponent()
		{
			_trayIcon = new NotifyIcon();

			UpdateIcon();

			_trayIcon.MouseClick += TrayIcon_Click;

			//Optional - Add a context menu to the TrayIcon:
			_contextMenu = new ContextMenuStrip();
			_closeMenuItem = new ToolStripMenuItem();
			_contextMenu.SuspendLayout();

			// 
			// TrayIconContextMenu
			// 
			this._contextMenu.Items.AddRange(new ToolStripItem[] {
			this._closeMenuItem});
			this._contextMenu.Name = "TrayIconContextMenu";
			this._contextMenu.Size = new Size(153, 70);
			// 
			// CloseMenuItem
			// 
			this._closeMenuItem.Name = "CloseMenuItem";
			this._closeMenuItem.Size = new Size(152, 22);
			this._closeMenuItem.Text = "Exit Deprox";
			this._closeMenuItem.Click += new EventHandler(this.CloseMenuItem_Click);

			_contextMenu.ResumeLayout(false);
			_trayIcon.ContextMenuStrip = _contextMenu;
		}

		private void UpdateIcon()
		{
			_trayIcon.Icon = ProxyController.Enabled ? Properties.Resources.On : Properties.Resources.Off;
		}

		private void OnApplicationExit(object sender, EventArgs e)
		{
			_trayIcon.Visible = false;
		}

		private void TrayIcon_Click(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ProxyController.Toggle();
				UpdateIcon();
			}
		}

		private void CloseMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}