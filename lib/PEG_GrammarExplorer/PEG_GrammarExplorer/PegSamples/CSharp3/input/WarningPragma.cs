using System;
class Program
{
	[Obsolete]
	static void Foo() {}
	static void Main() {
#pragma warning disable 612
	Foo();
#pragma warning restore 612
	}
}