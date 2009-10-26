using System;
class Test
{
	static void F(params object[] args) {
		foreach (object o in args) {
			Console.Write(o.GetType().FullName);
			Console.Write(" ");
		}
		Console.WriteLine();
	}
	static void Main() {
		object[] a = {1, "Hello", 123.456};
		object o = a;
		F(a);
		F((object)a);
		F(o);
		F((object[])o);
	}
}
