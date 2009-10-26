using System;
class A
{
	~A() {
		Console.WriteLine("Destruct instance of A");
	}
}
class B
{
	object Ref;
	public B(object o) {
		Ref = o;
	}
	~B() {
		Console.WriteLine("Destruct instance of B");
	}
}
class Test
{
	static void Main() {
		B b = new B(new A());
		b = null;
		GC.Collect();
		GC.WaitForPendingFinalizers();
	}
}