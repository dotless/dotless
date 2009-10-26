delegate void D(int x);
class C
{
	public static void M1(int i) { /* … */ }
	public static void M2(int i) { /* … */ }
}
class Test
{
	static void Main() { 
		D cd1 = new D(C.M1);
		D cd2 = new D(C.M2);
		D cd3 = cd1 + cd2 + cd2 + cd1;	// M1 + M2 + M2 + M1
		cd3 -= cd1;								// => M1 + M2 + M2
		cd3 = cd1 + cd2 + cd2 + cd1;		// M1 + M2 + M2 + M1
		cd3 -= cd1 + cd2;						// => M2 + M1
		cd3 = cd1 + cd2 + cd2 + cd1;		// M1 + M2 + M2 + M1
		cd3 -= cd2 + cd2;						// => M1 + M1
		cd3 = cd1 + cd2 + cd2 + cd1;		// M1 + M2 + M2 + M1
		cd3 -= cd2 + cd1;						// => M1 + M2
		cd3 = cd1 + cd2 + cd2 + cd1;		// M1 + M2 + M2 + M1
		cd3 -= cd1 + cd1;						// => M1 + M2 + M2 + M1
	}
}
