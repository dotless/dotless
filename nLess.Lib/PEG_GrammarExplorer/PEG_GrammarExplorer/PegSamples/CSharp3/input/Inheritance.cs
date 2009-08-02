class A
{
	int x;
	static void F(B b) {
		b.x = 1;		// Ok
	}
}
class B: A
{
	static void F(B b) {
		b.x = 1;		// Error, x not accessible
	}
}