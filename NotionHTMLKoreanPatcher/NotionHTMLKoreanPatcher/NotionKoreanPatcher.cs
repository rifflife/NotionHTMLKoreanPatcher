namespace NotionHTMLKoreanPatcher
{
	public class NotionKoreanPatcher
	{
		public int PatchCount { get; private set; }

		public void Start(string? path)
		{
			if (path == null)
			{
				Console.WriteLine($"잘못된 경로입니다.");
				return;
			}

			PatchCount = 0;

			Console.Clear();
			Console.WriteLine($"변환 작업을 시작합니다.");

			// Divide all Korean characters.
			Queue<string> traversalQueue = new Queue<string>();
			traversalQueue.Enqueue(path);

			while (traversalQueue.Count > 0)
			{
				// Change names
				string curPath = traversalQueue.Dequeue();

				Console.WriteLine($"========================================");
				Console.WriteLine($"Start convert path : {Path.GetFileName(curPath)}");
				Console.WriteLine($"");

				bool isPatched = false;

				// Convert directory names
				foreach (var d in Directory.GetDirectories(curPath))
				{
					if (TryGetRenameHangulPath
					(
						d,
						out var renamedPath,
						out var originDirName,
						out var renameDirName
					))
					{
						if (Directory.Exists(renamedPath))
						{
							MoveFiles(d, renamedPath);

							DirectoryInfo di = new DirectoryInfo(d);

							foreach (FileInfo file in di.EnumerateFiles())
							{
								file.Delete();
							}

							foreach (DirectoryInfo dir in di.EnumerateDirectories())
							{
								dir.Delete(true);
							}

							Directory.Delete(d);
						}
						else
						{
							Directory.Move(d, renamedPath);
							Console.WriteLine($"[Directory] Convert to : \n\"{originDirName}\"\n\"{renameDirName}\"\n");
							PatchCount++;
							isPatched = true;
						}

					}
				}

				// Convert file names
				foreach (var f in Directory.GetFiles(curPath))
				{
					var curFileExtension = Path.GetExtension(f);
					if (curFileExtension.ToLower() != ".html")
					{
						continue;
					}

					if (TryGetRenameHangulPath
					(
						f,
						out var renamedPath,
						out var originFileName,
						out var renameFileName
					))
					{
						File.Move(f, renamedPath);
						Console.WriteLine($"[File] Convert to : \n\"{originFileName}\"\n\"{renameFileName}\"\n");
						PatchCount++;
						isPatched = true;
					}
				}

				if (!isPatched)
				{
					Console.WriteLine($"There is no Korean file or directory...");
				}

				Console.WriteLine();

				traversalQueue.Enqueue(Directory.GetDirectories(curPath));
			}

			Console.WriteLine();
			Console.WriteLine($"Patch finished!");
			Console.WriteLine($"Patch count : {PatchCount}");
		}

		public static bool TryGetRenameHangulPath
		(
			string path,
			out string renamedPath, 
			out string originFileName, 
			out string renameFileName
		)
		{
			var dir = Path.GetDirectoryName(path) ?? string.Empty;
			originFileName = Path.GetFileName(path) ?? string.Empty;
			renameFileName = HangulHandler.GetDividedHangulAsString(originFileName);
			renamedPath = Path.Combine(dir, renameFileName);
			return path != renamedPath;
		}

		public static void MoveFiles(string sourcePath, string targetPath)
		{
			foreach (var f in Directory.GetFiles(sourcePath))
			{
				try
				{
					var target = Path.Combine(targetPath, Path.GetFileName(f));
					var targetDir = Path.GetDirectoryName(target) ?? string.Empty;
					if (!Directory.Exists(targetDir) && !string.IsNullOrWhiteSpace(targetDir))
					{
						Directory.CreateDirectory(targetDir);
					}
					File.Move(f, target);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}

			foreach (var d in Directory.GetDirectories(sourcePath))
			{
				var targetDirName = Path.GetFileName(d) ?? string.Empty;

				var recursiveSource = Path.Combine(sourcePath, targetDirName);
				var recursiveTarget = Path.Combine(targetPath, targetDirName);

				MoveFiles(recursiveSource, recursiveTarget);
			}
		}
	}
}