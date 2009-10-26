using System;
using System.Reflection;
class Test
{
	static void ShowHelp(MemberInfo member) {
		HelpAttribute a = Attribute.GetCustomAttribute(member,
			typeof(HelpAttribute)) as HelpAttribute;
		if (a == null) {
			Console.WriteLine("No help for {0}", member);
		}
		else {
			Console.WriteLine("Help for {0}:", member);
			Console.WriteLine("  Url={0}, Topic={1}", a.Url, a.Topic);
		}
	}
	static void Main() {
		ShowHelp(typeof(Widget));
		ShowHelp(typeof(Widget).GetMethod("Display"));
	}
}