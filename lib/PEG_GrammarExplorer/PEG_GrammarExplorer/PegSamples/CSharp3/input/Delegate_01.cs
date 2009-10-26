delegate double Function(double x);
class Test
{
	static double[] Apply(double[] a, Function f) {
		double[] result = new double[a.Length];
		for (int i = 0; i < a.Length; i++) result[i] = f(a[i]);
		return result;
	}
	static void F(double[] a, double[] b) {
		a = Apply(a, (double x) => Math.Sin(x));
		b = Apply(b, (double y) => Math.Sin(y));
	}
}