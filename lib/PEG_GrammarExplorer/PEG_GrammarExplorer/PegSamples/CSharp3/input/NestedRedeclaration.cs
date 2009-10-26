class A
{
	void F() {
		int i = 0;
		if (true) {
			int i = 1;			
		}
	}
	void G() {
		if (true) {
			int i = 0;
		}
		int i = 1;				
	}
	void H() {
		if (true) {
			int i = 0;
		}
		if (true) {
			int i = 1;
		}
	}
	void I() {
		for (int i = 0; i < 10; i++)
			H();
		for (int i = 0; i < 10; i++)
			H();
	}
}