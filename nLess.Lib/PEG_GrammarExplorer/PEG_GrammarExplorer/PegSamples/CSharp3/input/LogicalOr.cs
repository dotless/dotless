class A
{
	static void G(int x, int y) {
		int i;
		if (x >= 0 || (i = y) >= 0) {
			// i not definitely assigned
		}
		else {
			// i definitely assigned
		}
		// i not definitely assigned
	}
}