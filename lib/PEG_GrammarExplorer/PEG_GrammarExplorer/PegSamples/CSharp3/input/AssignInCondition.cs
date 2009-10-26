class A
{
	static void F(int x, int y) {
		int i;
		if (x >= 0 && (i = y) >= 0) {
			// i definitely assigned
		}
		else {
			// i not definitely assigned
		}
		// i not definitely assigned
	}
}