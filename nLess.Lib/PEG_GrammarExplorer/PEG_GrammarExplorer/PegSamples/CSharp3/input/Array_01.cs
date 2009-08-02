class Test
{
   void TestImplicitTyping(){
	var a = new[] { 1, 10, 100, 1000 };								// int[]
	var b = new[] { 1, 1.5, 2, 2.5 };								// double[]
	var c = new[,] { { "hello", null }, { "world", "!" } };	// string[,]
	var d = new[] { 1, "one", 2, "two" };							// Error
	}
}