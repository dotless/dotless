delegate bool Filter(int i);
class LambdaTest{
void F() {
	int max;
	// Error, max is not definitely assigned
	Filter f = (int n) => n < max;
	max = 5;
	DoWork(f);
}
}