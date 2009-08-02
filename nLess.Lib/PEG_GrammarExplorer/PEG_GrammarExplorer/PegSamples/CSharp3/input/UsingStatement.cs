class UsingStatement{
	static void Main() {
		using (TextWriter w = File.CreateText("test.txt")) {
			w.WriteLine("Line one");
			w.WriteLine("Line two");
			w.WriteLine("Line three");
		}
	}
}


