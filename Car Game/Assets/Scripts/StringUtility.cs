public class StringUtility
{
	public static string Substr(string s, int start, int length)
	{
		return (s.Length > (start + length)) ? 
			s.Substring(start, length) : 
			(s.Length > start) ? "" : s.Substring(start, s.Length - start);
	}
}
