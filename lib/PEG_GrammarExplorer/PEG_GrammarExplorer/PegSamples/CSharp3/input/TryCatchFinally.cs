class A
{
	static void F() {
		int i, j;
		try {
			goto LABEL;
			// neither i nor j definitely assigned
			i = 1;
			// i definitely assigned
		}
		catch {
			// neither i nor j definitely assigned
			i = 3;
			// i definitely assigned
		}
		finally {
			// neither i nor j definitely assigned
			j = 5;
			// j definitely assigned
		}
		// i and j definitely assigned
	  LABEL:;
		// j definitely assigned

	}
}