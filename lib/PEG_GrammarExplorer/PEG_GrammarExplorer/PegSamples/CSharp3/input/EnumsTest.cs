using System;
enum Color
{
	Red,
   Green,
   Blue
}
class Test
{
	static void PrintColor(Color color) {
		switch (color) {
			case Color.Red:
				Console.WriteLine("Red");
				break;
			case Color.Green:
				Console.WriteLine("Green");
				break;
			case Color.Blue:
				Console.WriteLine("Blue");
				break;
			default:
				Console.WriteLine("Unknown color");
				break;
		}
	}
	static void Main() {
		Color c = Color.Red;
		PrintColor(c);
		PrintColor(Color.Blue);
	}
}