class DoStatement{
	static void Main() {
		string s;
		do {
			s = Console.ReadLine();
			if (s != null) Console.WriteLine(s);
		} while (s != null);
	}
}

