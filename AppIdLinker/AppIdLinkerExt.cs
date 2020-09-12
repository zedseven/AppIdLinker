using AppIdLinker.Properties;
using KeePass.Plugins;
using KeePass.Util;
using System;
using System.Drawing;
using System.Windows.Forms;
using KeePassLib;
using KeePassLib.Security;

namespace AppIdLinker
{
	/// <summary>
	///     A KeePass plugin for including Android app IDs in the description of entries based on their URLs. This increases the searchability of relevant password entries for Android apps like Keepass2Android.
	/// </summary>
	public sealed class AppIdLinkerExt : Plugin
	{
		private IPluginHost _host;

		public override Image SmallIcon
		{
			get { return Resources.MenuIcon; }
		}

		public override string UpdateUrl
		{
			get { return "https://ztdp.ca/utility/keepass-plugins-ztdp.txt.gz"; }
		}

		public override bool Initialize(IPluginHost host)
		{
			if (host == null) return false;

			_host = host;

			//Set the version information file signature
			UpdateCheckEx.SetFileSigKey(UpdateUrl, Resources.AppIdLinkerExt_UpdateCheckFileSigKey);

			Db.Init(Resources.DomainAppIdDb);

			return true;
		}

		public override void Terminate()
		{
		}

		public override ToolStripMenuItem GetMenuItem(PluginMenuType t)
		{
			if (t != PluginMenuType.Main)
				return null;

			ToolStripMenuItem tsmi = new ToolStripMenuItem
			{
				Text = Resources.AppIdLinkerExt_GetMenuItem_AppIdLinker_Plugin,
				Image = Resources.MenuIcon
			};

			ToolStripMenuItem tsmiAdd = new ToolStripMenuItem
			{
				Text = Resources.AppIdLinkerExt_GetMenuItem_Add_IDs
			};
			tsmiAdd.Click += OnAddIdsClicked;

			ToolStripMenuItem tsmiRemove = new ToolStripMenuItem
			{
				Text = Resources.AppIdLinkerExt_GetMenuItem_Remove_IDs
			};
			tsmiRemove.Click += OnRemoveIdsClicked;

			tsmi.DropDownItems.Add(tsmiAdd);
			tsmi.DropDownItems.Add(tsmiRemove);

			return tsmi;
		}

		private void OnAddIdsClicked(object sender, EventArgs e)
		{
			PwEntry[] entries = _host.MainWindow.GetSelectedEntries();
			for (int i = 0; i < entries.Length; i++)
			{
				string newNotes = Db.AddToDesc(entries[i].Strings.ReadSafe(PwDefs.UrlField),
					entries[i].Strings.ReadSafe(PwDefs.NotesField));
				//entries[i].Strings.Set(PwDefs.NotesField, new ProtectedString(true, newNotes));
			}
		}

		private void OnRemoveIdsClicked(object sender, EventArgs e)
		{

		}

		/*private void OnEntrySetupClick(object sender, EventArgs e)
		{
			if (_host.MainWindow.GetSelectedEntriesCount() != 1)
				return;

			PatternSetupForm patternSetupForm = new PatternSetupForm(_host.MainWindow.GetSelectedEntry(true));
			patternSetupForm.ShowDialog();
			_host.MainWindow.RefreshEntriesList();
		}

		private void OnEntryDisplayClick(object sender, EventArgs e)
		{
			if (_host.MainWindow.GetSelectedEntriesCount() != 1)
				return;

			PatternDisplayForm patternDisplayForm = new PatternDisplayForm(_host.MainWindow.GetSelectedEntry(true));
			patternDisplayForm.ShowDialog();
			_host.MainWindow.RefreshEntriesList();
		}*/
		}
}