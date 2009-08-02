class Outer
{
	static void F(int i) {}
	static void F(string s) {}
	class Inner
	{
		void G() {
			F(1);				// Invokes Outer.Inner.F
			F("Hello");		// Error
		}
		static void F(long l) {}
	}
}