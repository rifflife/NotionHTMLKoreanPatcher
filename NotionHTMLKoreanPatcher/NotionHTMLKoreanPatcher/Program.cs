using System.Linq;
using System.Runtime.CompilerServices;

namespace NotionHTMLKoreanPatcher
{
	public class Program
	{
		public static string NotionPath = string.Empty;

		private static void Main(string[] args)
		{
			// Get Notion HTML path.
			while (true)
			{
				Console.Write("Notion HTML 경로를 입력하세요 : ");
				var notionPath = Console.ReadLine();

				if (notionPath == null)
				{
					Console.Clear();
					Console.WriteLine($"경로를 다시 입력하세요.");
					continue;
				}

				Console.Clear();

				Console.WriteLine($"경로 : {notionPath}");

				try
				{
					Console.WriteLine($"");

					int showCount = 14;

					foreach (var d in Directory.GetDirectories(notionPath))
					{
						showCount--;
						if (showCount == 0)
						{
							break;
						}

						Console.WriteLine($"Folder : {Path.GetDirectoryName(d)}");
					}

					foreach (var p in Directory.GetFiles(notionPath))
					{
						showCount--;
						if (showCount == 0)
						{
							break;
						}

						Console.WriteLine($"File : {Path.GetFileName(p)}");
					}

					Console.WriteLine($"");

					Console.WriteLine($"해당 경로에 존재하는 파일 목록입니다.");
					Console.WriteLine($"Notion HTML 파일이 맞습니까? y/n");
				}
				catch (Exception ex)
				{
					Console.Clear();
					Console.WriteLine($"경로를 다시 입력하세요.");
					continue;
				}

				var answer = Console.ReadLine();
				if (answer?.ToLower() == "y")
				{
					Console.WriteLine($"한글 호환 작업을 시작하시겠습니까? y/n");

					answer = Console.ReadLine();
					if (answer?.ToLower() == "y")
					{
						NotionPath = notionPath;
						break;
					}
				}
			}

			NotionKoreanPatcher patcher = new NotionKoreanPatcher();
			patcher.Start(NotionPath);
		}
	}
}