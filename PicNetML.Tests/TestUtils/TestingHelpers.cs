using NUnit.Framework;
using System.IO;
using System.Linq;

namespace PicNetML.Tests.TestUtils
{
	public static class TestingHelpers
	{

		public static string GetResourceFileName(string filename)
		{
			var dir = TestContext.CurrentContext.TestDirectory;
			return $@"{dir}\..\..\resources\{filename}";
		}

		public static Runtime LoadSmallRuntime<T>(string filename, int classidx, int count) where T : new()
		{
			string file = GetResourceFileName(filename);
			using (StreamReader fs = File.OpenText(file))
			{
				string[] lines = Enumerable.Range(0, count + 1).
					Select(i => fs.ReadLine()).
					Skip(1).
					ToArray();
				System.Collections.Generic.IEnumerable<T> rows = Runtime.LoadRowsFromLines<T>(lines);
				return Runtime.LoadFromRows(classidx, rows);
			}
		}
	}
}