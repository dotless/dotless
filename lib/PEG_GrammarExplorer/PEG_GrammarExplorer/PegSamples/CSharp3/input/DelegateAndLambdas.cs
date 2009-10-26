using System;
 public delegate bool CheckExpirationDelegate();
    class DelegateAndLambdas
    {
            static bool F1() { return false; }
	    static void Main()
	    {	
		    CheckExpirationDelegate CheckExpiration= F1;
    	
		    CheckExpiration += delegate() { return true; };
		    CheckExpiration += () => true;
	    }
    }



