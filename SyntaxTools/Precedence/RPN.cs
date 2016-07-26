using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.Operators;
using SyntaxTools.Trees;

namespace SyntaxTools.Precedence
{
    public static class RPN
    {
        private static bool PopOperatorByPrecedence(
            bool ToLeftAssociative,
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
        /// <param name="Items"></param>
        /// <returns></returns>

        public static IReadOnlyList<OperatorToken> InfixToRPN(IEnumerable<OperatorToken> Items)
        {
            return InfixToRPN<OperatorToken>(Items, x => x.GetStackType(), x => x.Operator.Associativity == OperatorAssociativity.Left, x => x.Operator.Precedence);
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
            foreach (var To in Items)
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

        /// <summary>
        /// Convert an RPN expression to an expression tree
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Items">The RPN expression</param>
        /// <param name="IsFunction">Return true if the given token is a function or an operator</param>
        /// <param name="ArgumentCount">Return the number of arguments of the given token or operator</param>
        /// <returns></returns>
        public static Tree<T> RPNToTree<T>(IEnumerable<T> Items, Func<T, bool> IsFunction, Func<T, int> ArgumentCount)
        {
            var st = new Stack<Tree<T>>();
            foreach (var Token in Items)
            {
                if (IsFunction(Token))
                {
                    //Extract arguments from the stack
                    var argsCount = ArgumentCount(Token);
                }
                else
                {
                    st.Push(new Tree<T>(Token));
                }
            }
        }
    }
}

