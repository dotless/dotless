using System;
class X<T>
{
	public static void PrintTypes() {
		Type[] t = {
			typeof(int),
			typeof(System.Int32),
			typeof(string),
			typeof(double[]),
			typeof(void),
			typeof(T),
			typeof(X<T>),
			typeof(X<X<T>>),
			typeof(X<>)
		};
		for (int i = 0; i < t.Length; i++) {
			Console.WriteLine(t[i]);
		}
	}
}
class Test
{
	static void Main() {
		X<int>.PrintTypes();
	}
}
