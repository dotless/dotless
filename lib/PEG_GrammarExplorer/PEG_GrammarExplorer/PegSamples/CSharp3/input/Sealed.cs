sealed class Box<T>: System.ValueType
{
	T value;
	public Box(T t) {
		value = t;
	}
}