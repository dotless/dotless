class ThrowAndTryStatement{
	static double Divide(double x, double y) {
		if (y == 0) throw new DivideByZeroException();
		return x / y;
	}
	static void Main(string[] args) {
		try {
			if (args.Length != 2) {
				throw new Exception("Two numbers required");
			}
			double x = double.Parse(args[0]);
			double y = double.Parse(args[1]);
			Console.WriteLine(Divide(x, y));
		}
		catch (Exception e) {
			Console.WriteLine(e.Message);
		}
		finally {
			Console.WriteLine("Good bye!");
		}
	}
}
