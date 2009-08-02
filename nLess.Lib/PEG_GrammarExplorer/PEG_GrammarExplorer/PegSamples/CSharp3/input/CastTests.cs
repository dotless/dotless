

class CastTests
{
	enum E{e1,e1};
	void Test()
        {
            int i;long l;
            i= (int)l;
	    E e= (E)i;
        }
}