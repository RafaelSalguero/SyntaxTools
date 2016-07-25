using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTools.Precedence
{
   public  class RPN
    {
        /// <summary>
        /// Token type that will determinine the ordering on the InfixToRPN Function
        /// </summary>
        public enum StackTokenType
        {
            /// <summary>
            /// The token is a value
            /// </summary>
            Value,
            /// <summary>
            /// An operand, this token can be an argument for associativity and precedence getter functions
            /// </summary>
            Operand,
            /// <summary>
            /// The name of a function that is beign called
            /// </summary>
            Function,
            /// <summary>
            /// A comma
            /// </summary>
            Comma,
            /// <summary>
            /// A left parenthesis
            /// </summary>
            LeftParenthesis,

            /// <summary>
            /// A right parenthesis
            /// </summary>
            RightParenthesis,
        }

        private static bool PopOperatorByPrecedence(
            bool  ToLeftAssociative,
            int ToPrecedence,
            int SkPeekPrecedence)
        {
            if (ToLeftAssociative)
                return ToPrecedence <= SkPeekPrecedence;
            else 
                return ToPrecedence < SkPeekPrecedence;
        }


        /// <summary>
        /// Convert infix to RPN notation
        /// </summary>
        /// <param name="GetType">Gets the stack type of the token</param>
        /// <param name="GetPrecedence">Gets the precedence of an operator token</param>
        /// <param name="IsLeftAssociative">Return true if the current token is a left associative operator, false if the token is right associative</param>
        /// <returns></returns>
        public static IReadOnlyList<T> InfixToRPN<T>(IEnumerable<T> Items,
            Func<T, StackTokenType> GetType,
            Func<T, bool> IsLeftAssociative,
            Func<T, int> GetPrecedence)
        {
            Queue<T> Out = new Queue<T>();
            Stack<T> Sk = new Stack<T>();
            foreach(var To in Items)
            {

                StackTokenType StackType = GetType(To);

                switch (StackType)
                {
                    case StackTokenType.Value:
                        Out.Enqueue(To);

                        break;
                    case StackTokenType.Operand:
                        while (
                            (Sk.Count > 0) &&
                            (GetType(Sk.Peek()) == StackTokenType.Operand) && 
                            (PopOperatorByPrecedence(IsLeftAssociative(To), GetPrecedence(To), GetPrecedence(Sk.Peek()))))
                            Out.Enqueue(Sk.Pop());
                        Sk.Push(To);
                        break;
                    case StackTokenType.Function:
                        Out.Enqueue(To);
                        Sk.Push(To);


                        break;
                    case StackTokenType.Comma:
                        {
                            while (!(GetType(Sk.Peek()) == StackTokenType.LeftParenthesis))
                                Out.Enqueue(Sk.Pop());
                        }
                        break;
                    case StackTokenType.LeftParenthesis:
                        Sk.Push(To);
                        break;
                    case StackTokenType.RightParenthesis:
                        {
                            while (Sk.Count > 0 && !(GetType(Sk.Peek()) == StackTokenType.LeftParenthesis))
                                Out.Enqueue(Sk.Pop());
                            if (Sk.Count == 0)
                            {
                                throw new ArgumentException("Wrong parenthesis balance");
                            }
                            Sk.Pop();
                            if (Sk.Count > 0 && GetType(Sk.Peek()) == StackTokenType.Function)
                            {
                                var f = Sk.Pop();
                                Out.Enqueue(f);
                            }
                        }
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            while (Sk.Count > 0)
            {
                Out.Enqueue(Sk.Pop());
            }

            List<T> Ret = new List<T>();
            while (Out.Count > 0)
                Ret.Add(Out.Dequeue());

            //Check for mismatched parenthesis:
            foreach (var Token in Ret)
            {
                var Type = GetType(Token);
                if (Type == StackTokenType.LeftParenthesis || Type == StackTokenType.RightParenthesis)
                    throw new ArgumentException("Mismatched parenthesis");
            }

            return Ret;
        }

    }
}

