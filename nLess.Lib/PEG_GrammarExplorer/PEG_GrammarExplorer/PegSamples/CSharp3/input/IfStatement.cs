class IfStatement{
	static void Main(string[] args) {
		if (args.Length == 0) {
			Console.WriteLine("No arguments");
		}
		else {
			Console.WriteLine("One or more arguments");
		}
	}
}
