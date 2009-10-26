public unsafe struct Node
{
	public int Value;
	public Node* Left;
	public Node* Right;
}
public struct Node1
{
	public int Value;
	public unsafe Node* Left;
	public unsafe Node* Right;
}
public class A
{
	public unsafe virtual void F() {
		char* p;
	}
}
public class B: A
{
	public override void F() {
		base.F();
	}
}
public unsafe class A1
{
	public virtual void F(char* p) {}
}
public class B1: A1
{
	public unsafe override void F(char* p) {}
}

