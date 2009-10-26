

using System;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class SimpleAttribute: Attribute 
{

}

[Simple] class Class1 {}
[Simple] interface Interface1 {}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AuthorAttribute: Attribute
{
	private string name;
	public AuthorAttribute(string name) {
		this.name = name;
	}
	public string Name {
		get { return name; }
	}
}

[Author("Brian Kernighan"), Author("Dennis Ritchie")] 
class Class2
{

}


[AttributeUsage(AttributeTargets.Class)]
public class HelpAttribute: Attribute
{
	public HelpAttribute(string url) {		// Positional parameter
		
	}
	public string Topic {						// Named parameter
		get {}
		set {}
	}
	public string Url {
		get {}
	}
}
[Help("http://www.mycompany.com/.../Class1.htm")]
class Class2
{
}
[Help("http://www.mycompany.com/.../Misc.htm", Topic = "Class2")]
class Class3
{
}
