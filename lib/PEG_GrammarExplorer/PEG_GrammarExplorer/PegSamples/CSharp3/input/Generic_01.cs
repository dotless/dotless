namespace Widgets
{
	class Queue {}
	class Queue<TElement> {}
}
namespace MyApplication
{
	using Widgets;
	class X
	{
		Queue q1;			// Non-generic Widgets.Queue
		Queue<int> q2;		// Generic Widgets.Queue
	}
}
//A type-name might identify a constructed type even though it doesn’t specify type parameters directly. This can occur where a type is nested within a generic class declaration, and the instance type of the containing declaration is implicitly used for name lookup (§10.3.8.6):
class Outer<T>
{
	public class Inner {}
	public Inner i;				// Type of i is Outer<T>.Inner
}