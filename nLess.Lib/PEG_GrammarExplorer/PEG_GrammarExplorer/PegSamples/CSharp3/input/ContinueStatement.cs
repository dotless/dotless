class ContinueStatement{
	static void Main(string[] args) {
		for (int i = 0; i < args.Length; i++) {
			if (args[i].StartsWith("/")) continue;
			Console.WriteLine(args[i]);
		}
	}
}





