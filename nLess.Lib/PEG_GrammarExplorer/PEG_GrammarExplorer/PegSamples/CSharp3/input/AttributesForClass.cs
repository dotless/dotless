

[Attr1, Attr2("hello")]
partial class A {}
[Attr3, Attr2("goodbye")]
partial class A {}

[Attr1, Attr2("hello"), Attr3, Attr2("goodbye")]
class B {}
