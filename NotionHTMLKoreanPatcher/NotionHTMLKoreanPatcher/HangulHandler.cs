using System.Text;

public class HangulHandler
{
	/// <summary>초성 수</summary>
	public const int INITIAL_COUNT = 19;
	/// <summary>중성 수</summary>
	public const int MEDIAL_COUNT = 21;
	/// <summary>종성 수</summary>
	public const int FINAL_COUNT = 28;
	/// <summary>한글 유니코드 시작 인덱스</summary>
	public const int HANGUL_UNICODE_START_INDEX = 0xac00;
	/// <summary>한글 유니코드 종료 인덱스</summary>
	public const int HANGUL_UNICODE_END_INDEX = 0xD7A3;
	/// <summary>초성 시작 인덱스</summary>
	public const int INITIAL_START_INDEX = 0x1100;
	/// <summary>중성 시작 인덱스</summary>
	public const int MEDIAL_START_INDEX = 0x1161;
	/// <summary>종성 시작 인덱스</summary>
	public const int FINAL_START_INDEX = 0x11a7;

	/// <summary>문자의 한글 여부를 판단합니다.</summary>
	/// <param name="source">문자</param>
	/// <returns>한글이라면 true를 반환합니다.</returns>
	public static bool IsHangul(char source)
	{
		return HANGUL_UNICODE_START_INDEX <= source && source <= HANGUL_UNICODE_END_INDEX;
	}

	/// <summary>문자열의 한글 여부를 판단합니다.</summary>
	/// <param name="source">문자열</param>
	/// <returns>한글이라면 true를 반환합니다.</returns>
	public static bool IsHangul(string source)
	{
		if (source == null)
		{
			return false;
		}

		foreach (var character in source)
		{
			if (!IsHangul(character))
			{
				return false;
			}
		}

		return true;
	}

	public static char[] GetDividedHangulAsArray(char source)
	{
		if (IsHangul(source))
		{
			int index = source - HANGUL_UNICODE_START_INDEX;

			int initial = INITIAL_START_INDEX + index / (MEDIAL_COUNT * FINAL_COUNT);
			int medial = MEDIAL_START_INDEX + (index % (MEDIAL_COUNT * FINAL_COUNT)) / FINAL_COUNT;
			int final = FINAL_START_INDEX + index % FINAL_COUNT;

			if (final == 4519)
			{
				var dividedHangul = new char[2];

				dividedHangul[0] = (char)initial;
				dividedHangul[1] = (char)medial;

				return dividedHangul;
			}
			else
			{
				var dividedHangul = new char[3];

				dividedHangul[0] = (char)initial;
				dividedHangul[1] = (char)medial;
				dividedHangul[2] = (char)final;

				return dividedHangul;
			}
		}

		return new char[1] { source };
	}

	public static string GetDividedHangulAsString(char source)
	{
		if (IsHangul(source))
		{
			int index = source - HANGUL_UNICODE_START_INDEX;

			int initial = INITIAL_START_INDEX + index / (MEDIAL_COUNT * FINAL_COUNT);
			int medial = MEDIAL_START_INDEX + (index % (MEDIAL_COUNT * FINAL_COUNT)) / FINAL_COUNT;
			int final = FINAL_START_INDEX + index % FINAL_COUNT;

			if (final == 4519)
			{
				return $"{(char)initial}{(char)medial}";
			}
			else
			{
				return $"{(char)initial}{(char)medial}{(char)final}";
			}
		}

		return $"{source}";
	}

	/// <summary>문자열 안의 한글을 초성 중성 종성으로 분리합니다.</summary>
	/// <param name="source">원본 문자열</param>
	/// <returns>한글이 분리된 문자열</returns>
	public static string GetDividedHangulAsString(string source)
	{
		if (source == null)
		{
			return string.Empty;
		}

		StringBuilder sb = new StringBuilder();

		foreach (var c in source)
		{
			sb.Append(GetDividedHangulAsString(c));
		}

		return sb.ToString();
	}
}
