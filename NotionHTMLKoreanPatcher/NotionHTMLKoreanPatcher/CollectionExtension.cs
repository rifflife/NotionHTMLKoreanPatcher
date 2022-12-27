namespace NotionHTMLKoreanPatcher
{
	public static class CollectionExtension
	{
		public static void Enqueue<T>(this Queue<T> queue, IEnumerable<T> array)
		{
			foreach (var item in array)
			{
				queue.Enqueue(item);
			}
		}
	}
}