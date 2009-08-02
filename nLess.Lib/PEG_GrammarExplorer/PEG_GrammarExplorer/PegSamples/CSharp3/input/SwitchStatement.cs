class SwitchStatement{
	static void Main(string[] args) {
		int n = args.Length;
		switch (n) {
			case 0:
				Console.WriteLine("No arguments");
				break;
			case 1:
				Console.WriteLine("One argument");
				break;
			default:
				Console.WriteLine("{0} arguments", n);
				break;
			}
		}
	}
}