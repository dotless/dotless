class GotoStatement{
	static void Main(string[] args) {
		int i = 0;
		goto check;
		loop:
		Console.WriteLine(args[i++]);
		check:
		if (i < args.Length) goto loop;
	}
}






