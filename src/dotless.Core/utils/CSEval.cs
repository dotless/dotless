/* Copyright 2009 dotless project, http://www.dotlesscss.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */

using System.Collections.Generic;
using dotless.Core.engine;
using System;

namespace dotless.Core.utils
{
    public static class CsEval
    {
        public static object StackEval(Expression expression)
        {
            var temporaryStack = new Stack<Entity>();
            var postfix = new List<Entity>();
            foreach (var node in expression)
            {
                if(node is Operator)
                {
                    var oper = (Operator) node;
                    switch (oper.Value)
                    {
                        case "(":
                            temporaryStack.Push(oper);
                            break;
                        case ")":
                            while (!(temporaryStack.Peek() is Operator) && temporaryStack.Peek().Value != ")")
                            {
                                postfix.Add(temporaryStack.Pop());
                            }
                            temporaryStack.Pop();
                            break;
                    }
                    if (temporaryStack.Count > 0)
                    {
                        switch (oper.Value.Trim())
                        {
                            case "+":
                            case "-":
                                while (temporaryStack.Count > 0 && temporaryStack.Peek().Value.Trim() != "(")
                                {
                                    postfix.Add(temporaryStack.Pop());
                                }
                                break;
                            case "/":
                            case "*":
                                while (temporaryStack.Count > 0 && (temporaryStack.Peek().Value.Trim() == "/" || temporaryStack.Peek().Value.Trim() == "*"))
                                {
                                    postfix.Add(temporaryStack.Pop());
                                }
                                break;
                        }
                    }
                    temporaryStack.Push(oper);

                }
                else
                {
                    postfix.Add((Entity)node);
                }
            }
            while(temporaryStack.Count > 0)
            {
                postfix.Add(temporaryStack.Pop());
            }

            var values = new Stack<Entity>();
            foreach (var element in postfix)
            {
                if (element is Operator)
                {
                    var right = values.Pop();
                    var left = values.Pop();
                    switch (element.Value.Trim())
                    {
                        case "+":
                            values.Push(Add(left, right));
                            break;
                        case "-":
                            values.Push(Sub(left, right));
                            break;
                        case "/":
                            values.Push(Div(left, right));
                            break;
                        case "*":
                            values.Push(Mul(left, right));
                            break;
                    }

                }
                else
                {
                    values.Push(element);
                }
            }
            return values.Pop();
        }

        private static Entity Sub(Entity left, Entity right)
        {
            if(left is Color)
            {
                if(right is Number)
                {
                    return (Color) left - ((Number)right).Value;
                }
                if(right is Color)
                {
                    return (Color) left - (Color) right;
                }
                throw new InvalidOperationException();
            }
            if(left is Number)
            {
                if (right is Number)
                {
                    return (Number)left - (Number)right;
                }
                if (right is Color)
                {
                    return ((Number)left).Value - (Color)right;
                }
                throw new InvalidOperationException();
                
            }
            throw new NotImplementedException();
        }

        private static Entity Div(Entity left, Entity right)
        {
            if (left is Color)
            {
                if (right is Number)
                {
                    return (Color)left / ((Number)right).Value;
                }
                if (right is Color)
                {
                    return (Color)left / (Color)right;
                }
                throw new InvalidOperationException();
            }
            if (left is Number)
            {
                if (right is Number)
                {
                    return (Number)left / (Number)right;
                }
                if (right is Color)
                {
                    return ((Number) left).Value/(Color) right;
                }
                throw new InvalidOperationException();

            }
            throw new NotImplementedException();
        }
        private static Entity Mul(Entity left, Entity right)
        {
            if (left is Color)
            {
                if (right is Number)
                {
                    return (Color)left * ((Number)right).Value;
                }
                if (right is Color)
                {
                    return (Color)left * (Color)right;
                }
                throw new InvalidOperationException();
            }
            if (left is Number)
            {
                if (right is Number)
                {
                    return (Number)left * (Number)right;
                }
                if (right is Color)
                {
                    return ((Number) left).Value*(Color) right;
                }
                throw new InvalidOperationException();

            }
            throw new NotImplementedException();
        }
        private static Entity Add(Entity left, Entity right)
        {
            if (left is Color)
            {
                if (right is Number)
                {
                    return (Color)left + ((Number)right).Value;
                }
                if (right is Color)
                {
                    return (Color)left + (Color)right;
                }
                throw new InvalidOperationException();
            }
            if (left is Number)
            {
                if (right is Number)
                {
                    return (Number)left + (Number)right;
                }
                if (right is Color)
                {
                    return ((Number)left).Value + (Color)right;
                }
                throw new InvalidOperationException();

            }
            throw new NotImplementedException();
        }
    }
}