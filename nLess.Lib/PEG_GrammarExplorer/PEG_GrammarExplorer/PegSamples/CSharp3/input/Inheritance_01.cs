public class A
{
	protected int x;
	static void F(A a, B b) {
		a.x = 1;		// Ok
		b.x = 1;		// Ok
	}
}
public class B: A
{
	static void F(A a, B b) {
		a.x = 1;		// Error, must access through instance of B
		b.x = 1;		// Ok
	}
}
class C<T>
{
	protected T x;
}
class D<T>: C<T>
{
	static void F() {
		D<T> dt = new D<T>();
		D<int> di = new D<int>();
		D<string> ds = new D<string>();
		dt.x = default(T);
		di.x = 123;
		ds.x = "test";
	}
}