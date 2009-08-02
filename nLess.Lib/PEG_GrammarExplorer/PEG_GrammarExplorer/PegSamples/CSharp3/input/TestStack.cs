using System;
using Acme.Collections;
class Test
{
   static void Main(){
      Stack s= new Stack();
      s.Push(1);
      s.Push(10);
      s.Push(100);
      Console.WriteLine(s.Pop());
      Console.WriteLine(s.Pop());
   }
}