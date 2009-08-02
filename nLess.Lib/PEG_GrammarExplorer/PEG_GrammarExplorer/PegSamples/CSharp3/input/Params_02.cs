using System;
class Test
{
	static void F(params int[] args) {
		Console.Write("Array contains {0} elements:", args.Length);
		foreach (int i in args) 
			Console.Write(" {0}", i);
		Console.WriteLine();
	}
	static void Main() {
		int[] arr = {1, 2, 3};
		F(arr);
		F(10, 20, 30, 40);
		F();
	}
}

