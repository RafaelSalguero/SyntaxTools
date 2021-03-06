﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.Operators;
using SyntaxTools.Text;

namespace SyntaxTools.Global
{
    public class CommonOperators
    {


        /// <summary>
        /// Addition
        /// </summary>
        public static Operator Add = new Operator(5, OperatorArgumentPosition.Binary, OperatorAssociativity.Left, 2, CommonTokens.Aritmetic.Add);
        /// <summary>
        /// Substraction
        /// </summary>
        public static Operator Sub = new Operator(5, OperatorArgumentPosition.Binary, OperatorAssociativity.Left, 2, CommonTokens.Aritmetic.Minus, GuidNames.SetName("Substraction"));

        /// <summary>
        /// Multiplication
        /// </summary>
        public static Operator Mul = new Operator(6, OperatorArgumentPosition.Binary, OperatorAssociativity.Left, 2, CommonTokens.Aritmetic.Mul);

        /// <summary>
        /// Divition
        /// </summary>
        public static Operator Div = new Operator(6, OperatorArgumentPosition.Binary, OperatorAssociativity.Left, 2, CommonTokens.Aritmetic.Div);

        /// <summary>
        /// Negation
        /// </summary>
        public static Operator Neg = new Operator(7, OperatorArgumentPosition.PrefixUnary, OperatorAssociativity.Right, 1, CommonTokens.Aritmetic.Minus, GuidNames.SetName("Negation"));

        /// <summary>
        /// Power
        /// </summary>
        public static Operator Pow = new Operator(8, OperatorArgumentPosition.Binary, OperatorAssociativity.Right, 2, CommonTokens.Aritmetic.Power);

    }
}
