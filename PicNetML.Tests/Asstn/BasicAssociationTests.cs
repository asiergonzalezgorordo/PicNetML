using NUnit.Framework;
using System.IO;

namespace PicNetML.Tests.Asstn
{
	[TestFixture]
	public class BasicAssociationTests
	{
		[OneTimeSetUp]
		public void RunBeforeAnyTests()
		{
			Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
		}

		[Test, Ignore("Not Implemented")]
		public void implement()
		{
		}
	}
}