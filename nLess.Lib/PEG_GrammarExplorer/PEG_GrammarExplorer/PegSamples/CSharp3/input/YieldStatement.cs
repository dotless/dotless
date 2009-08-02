class YieldStatement{
	static IEnumerable<int> Range(int from, int to) {
		for (int i = from; i < to; i++) {
			yield return i;
		}
		yield break;
	}
	static void Main() {
		foreach (int x in Range(-10,10)) {
			Console.WriteLine(x);
		}
	}
}
