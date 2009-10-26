class TypeParameters{
	public class Pair<TFirst,TSecond>
	{
		public TFirst First;
		public TSecond Second;
	}
	public static void Main()
	{
	    Pair<int,string> pair = new Pair<int,string> { First = 1, Second = "two" };
		int i = pair.First;     // TFirst is int
		string s = pair.Second; // TSecond is string
	}
}