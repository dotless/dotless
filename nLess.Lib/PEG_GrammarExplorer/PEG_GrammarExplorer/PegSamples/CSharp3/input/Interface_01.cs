interface ITest
{
	void F();							// F()
	void F(int x);						// F(int)
	void F(ref int x);				// F(ref int)
	void F(out int x);			   // F(out int)  	error
	void F(int x, int y);			// F(int, int)
	int F(string s);					// F(string)
	int F(int x);						// F(int)			error
	void F(string[] a);				// F(string[])
	void F(params string[] a);		// F(string[])		error
}