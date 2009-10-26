class WhileStatement{
	static void Main(string[] args) {
		int i = 0;
		while (i < args.Length) {
			Console.WriteLine(args[i]);
			i++;
		}
	}
}
