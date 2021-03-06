﻿using System.Collections.Generic;
using System.Linq;

namespace AppIdLinker
{
	public static class Db
	{
		private const char PartSeparator = '|';
		private const char ListDelimiter = ',';
		private const char CommentMarker = '#';
		private const string HeaderText = "Associated Android app IDs:";
		private static DbEntry[] _internalDb;

		/// <summary>
		///     Initializes the database.
		/// </summary>
		public static void Init(string dbContents)
		{
			string[] lines = dbContents.Replace("\r", "").Split('\n').Select(e => e.Trim()).ToArray();
			List<DbEntry> workingDb = new List<DbEntry>();
			foreach (string line in lines)
			{
				if (line.Length <= 0 || line[0] == CommentMarker)
					continue;
				string[] lineParts = line.Split(new[] {PartSeparator}, 2);
				if (lineParts.Length != 2 || lineParts[0].Length <= 0 || lineParts[1].Length <= 0)
					continue;
				workingDb.Add(new DbEntry(lineParts[0].Split(ListDelimiter).Select(e => e.Trim()).ToArray(),
					lineParts[1].Split(ListDelimiter).Select(e => e.Trim()).ToArray()));
			}

			_internalDb = workingDb.ToArray();
		}

		public static string AddToDesc(string url, string desc)
		{
			string[] ids = FindAssociatedIds(url);
			if (ids == null)
				return desc;

			List<string> lines = desc.Replace("\r", "").Split('\n').Select(e => e.Trim()).ToList();
			int headerIndex = -1;
			for (int i = 0; i < lines.Count; i++)
			{
				if (lines[i] != HeaderText)
					continue;
				headerIndex = i;
				break;
			}

			if (headerIndex > -1)
			{
				int stopIndex = -1;
				List<string> existingIds = new List<string>();
				for (int i = headerIndex + 1; i < lines.Count; i++)
				{
					if (lines[i].Length <= 0)
					{
						stopIndex = i;
						break;
					}

					existingIds.Add(lines[i]);
				}

				if (stopIndex <= -1)
					stopIndex = lines.Count;

				ids = ids.Union(existingIds).OrderBy(e => e).ToArray();

				if (stopIndex > -1 && stopIndex > headerIndex)
					lines.RemoveRange(headerIndex + 1, stopIndex - 1 - headerIndex);
			}
			else
			{
				if (lines[lines.Count - 1].Length <= 0)
				{
					lines[lines.Count - 1] = HeaderText;
				}
				else
				{
					lines.Add("");
					lines.Add(HeaderText);
				}

				headerIndex = lines.Count - 1;

				ids = ids.OrderBy(e => e).ToArray();
			}

			lines.InsertRange(headerIndex + 1, ids);

			return string.Join("\n", lines);
		}

		public static string RemoveFromDesc(string desc)
		{
			List<string> lines = desc.Replace("\r", "").Split('\n').Select(e => e.Trim()).ToList();
			int headerIndex = -1;
			int stopIndex = -1;
			for (int i = 0; i < lines.Count; i++)
				if (headerIndex <= -1)
				{
					if (lines[i] == HeaderText)
						headerIndex = i;
				}
				else
				{
					if (lines[i].Length > 0)
						continue;
					stopIndex = i;
					break;
				}

			if (headerIndex <= -1)
				return desc;

			if (stopIndex <= -1)
				stopIndex = lines.Count;

			if (headerIndex - 1 >= 0 && lines[headerIndex - 1].Length <= 0)
				lines.RemoveRange(headerIndex - 1, stopIndex + 1 - headerIndex);
			else
				lines.RemoveRange(headerIndex, stopIndex + 1 - headerIndex);

			return string.Join("\n", lines);
		}

		private static string[] FindAssociatedIds(string url)
		{
			int protocolColonIndex = url.IndexOf("://");
			if (protocolColonIndex > -1)
				url = url.Remove(0, protocolColonIndex + 3);
			int slashIndex = url.IndexOf('/');
			if (slashIndex > -1)
				url = url.Remove(slashIndex);
			for (int i = 0; i < _internalDb.Length; i++)
				foreach (string domain in _internalDb[i].Domains)
					if (UrlMatchesDomain(url, domain))
						return _internalDb[i].AppIds;

			return null;
		}

		private static bool UrlMatchesDomain(string url, string domain)
		{
			if (url.Length < domain.Length)
				return false;
			for (int i = domain.Length - 1, j = url.Length - 1; i >= 0; i--, j--)
				if (domain[i] != url[j])
					return false;
			return true;
		}

		private struct DbEntry
		{
			public readonly string[] Domains;
			public readonly string[] AppIds;

			public DbEntry(string[] domains, string[] appIds)
			{
				Domains = domains;
				AppIds = appIds;
			}
		}
	}
}