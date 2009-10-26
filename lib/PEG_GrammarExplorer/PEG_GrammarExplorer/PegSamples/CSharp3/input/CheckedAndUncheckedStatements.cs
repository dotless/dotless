class CheckedAndUncheckedStatements{
	static void Main() {
		int i = int.MaxValue;
		checked {
			Console.WriteLine(i + 1);		// Exception
		}
		unchecked {
			Console.WriteLine(i + 1);		// Overflow
		}
	}
}

