using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppIdLinker.Tests
{
	[TestClass]
	public class DbTests
	{
		private readonly string _testDb = string.Join(
			Environment.NewLine,
			"# Test DB",
			"example.com | com.example.demo, com.example.app4, com.example.juice",
			"demo.com, blue.co.uk | com.fox.test, uk.co.blue.orca, com.demo.test",
			"ztdp.ca | ca.ztdp.idk, ca.ztdp.idk.beta");

		[TestMethod]
		public void AddToDescTest0()
		{
			Db.Init(_testDb);
			string desc = Db.AddToDesc("https://example.com/", "");
			Assert.AreEqual("Associated Android app IDs:\ncom.example.app4\ncom.example.demo\ncom.example.juice",
				desc);
		}

		[TestMethod]
		public void AddToDescTest1()
		{
			Db.Init(_testDb);
			string desc = Db.AddToDesc("http://blue.co.uk/", "Old email used with this account was blegh@bademail.com");
			Assert.AreEqual("Old email used with this account was blegh@bademail.com\n\nAssociated Android app IDs:\ncom.demo.test\ncom.fox.test\nuk.co.blue.orca",
				desc);
		}
	}
}