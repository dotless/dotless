class A
{
	public static int x;
	int y;
	void F(int[] v, int a, ref int b, out int c) {
		int i = 1;
		c = a + b++;
	}
}