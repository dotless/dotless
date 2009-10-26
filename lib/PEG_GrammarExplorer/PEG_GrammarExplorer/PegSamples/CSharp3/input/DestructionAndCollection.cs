using System;
class A
{
	~A() {
		Console.WriteLine("Destruct instance of A");
	}
	public void F() {
		Console.WriteLine("A.F");
		Test.RefA = this;
	}
}
class B
{
	public A Ref;
	~B() {
		Console.WriteLine("Destruct instance of B");
		Ref.F();
	}
}
class Test
{
	public static A RefA;
	public static B RefB;
	static void Main() {
		RefB = new B();
		RefA = new A();
		RefB.Ref = RefA;
		RefB = null;
		RefA = null;
		// A and B now eligible for destruction
		GC.Collect();
		GC.WaitForPendingFinalizers();
		// B now eligible for collection, but A is not
		if (RefA != null)
			Console.WriteLine("RefA is not null");
	}
}