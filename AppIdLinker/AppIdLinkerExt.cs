using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AppIdLinker.Properties;
using KeePass.Plugins;
using KeePass.Util;
using KeePassLib;
using KeePassLib.Security;

namespace AppIdLinker
{
	/// <summary>
	///     A KeePass plugin for including Android app IDs in the description of entries based on their URLs. This increases
	///     the searchability of relevant password entries for Android apps like Keepass2Android.
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
			if (t != PluginMenuType.Main && t != PluginMenuType.Entry)
				return null;

			ToolStripMenuItem strip = new ToolStripMenuItem
			{
				Text = Resources.AppIdLinkerExt_GetMenuItem_AppIdLinker_Plugin,
				Image = Resources.MenuIcon
			};

			ToolStripMenuItem stripAdd = new ToolStripMenuItem
			{
				Text = Resources.AppIdLinkerExt_GetMenuItem_Add_IDs
			};
			stripAdd.Click += OnAddIdsClicked;

			ToolStripMenuItem stripRemove = new ToolStripMenuItem
			{
				Text = Resources.AppIdLinkerExt_GetMenuItem_Remove_IDs
			};
			stripRemove.Click += OnRemoveIdsClicked;

			strip.DropDownItems.Add(stripAdd);
			strip.DropDownItems.Add(stripRemove);

			return strip;
		}

		private void OnAddIdsClicked(object sender, EventArgs e)
		{
			PwEntry[] entries = _host.MainWindow.GetSelectedEntries();
			if (entries == null || entries.Length <= 0)
				return;

			Stopwatch operationStopwatch = Stopwatch.StartNew();
			_host.MainWindow.SetStatusEx("AppIdLinker: Adding IDs to selected entries.");
			for (int i = 0; i < entries.Length; i++)
			{
				string newNotes = Db.AddToDesc(entries[i].Strings.ReadSafe(PwDefs.UrlField),
					entries[i].Strings.ReadSafe(PwDefs.NotesField));
				entries[i].Strings.Set(PwDefs.NotesField, new ProtectedString(true, newNotes));
			}

			_host.MainWindow.SetStatusEx("AppIdLinker: Finished adding IDs to selected entries. Time taken: " +
			                             operationStopwatch.Elapsed.TotalSeconds + "s");
		}

		private void OnRemoveIdsClicked(object sender, EventArgs e)
		{
			PwEntry[] entries = _host.MainWindow.GetSelectedEntries();
			if (entries == null || entries.Length <= 0)
				return;

			Stopwatch operationStopwatch = Stopwatch.StartNew();
			_host.MainWindow.SetStatusEx("AppIdLinker: Removing IDs from selected entries.");
			for (int i = 0; i < entries.Length; i++)
			{
				string newNotes = Db.RemoveFromDesc(entries[i].Strings.ReadSafe(PwDefs.NotesField));
				entries[i].Strings.Set(PwDefs.NotesField, new ProtectedString(true, newNotes));
			}

			_host.MainWindow.SetStatusEx("AppIdLinker: Finished removing IDs from selected entries. Time taken: " +
			                             operationStopwatch.Elapsed.TotalSeconds + "s");
		}
	}
}