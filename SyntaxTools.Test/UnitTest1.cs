using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTools.DataStructures;
using SyntaxTools.Text.LexerUnits;
using SyntaxTools.Text.LexerUnits.StateMachines;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SyntaxTools.Text;

namespace SyntaxTools.Test
{

    public static class Extensions
    {
        public static Substring AsSubstring(this string s)
        {
            return new Substring(s);
        }
        public static Substring AsSubstring(this string s, int Index, int Count)
        {
            return new Substring(s, Index, Count);
        }
        public static TokenSubstring AsToken(this Substring s, Guid id)
        {
            return new TokenSubstring(id, s);
        }
    }
    [TestClass]
    public class UnitTest1
    {
        static List<string> Split(string Text, IEnumerable<char> Separators)
        {
            var result = new List<string>();
            //String builder is used for performance
            var B = new StringBuilder();
            foreach (var c in Text)
            {
                if (Separators.Contains(c))
                {
                    //Add the string acumulated before the separator
                    if (B.Length > 0)
                    {
                        result.Add(B.ToString());
                        B.Clear();
                    }
                    //Add the separator:
                    result.Add(c.ToString());
                }
                else
                    //Acumulate the string
                    B.Append(c);
            }

            //If there are an acumulated string, added to the list:
            if (B.Length > 0)
                result.Add(B.ToString());

            return result;
        }

        [TestMethod]
        public void Test()
        {
            var r = Split("hello+there", new[] { '+', '-' });
        }
        [TestMethod]
        public void TokenEquallityTest()
        {
            var holaGuid = Guid.NewGuid();
            var rafaGuid = Guid.NewGuid();

            var str = "holaholarafa";
            var hola1 = str.AsSubstring(0, 4).AsToken(holaGuid);
            var hola2 = str.AsSubstring(4, 4).AsToken(holaGuid);
            var rafa1_1 = str.AsSubstring(8, 4).AsToken(rafaGuid);
            var rafa1_2 = str.AsSubstring(8, 4).AsToken(rafaGuid);

            Assert.IsTrue(hola1.Equals(hola1));
            Assert.IsFalse(hola1.Equals(hola2));
            Assert.IsTrue(rafa1_1.Equals(rafa1_2));
            Assert.IsFalse(hola1.Equals(rafa1_1));

        }



        [TestMethod]
        public void WordSplitterTest3()
        {
            var text = "//this is a comment\r\nresult = \"hi there\" + name";

            //parsers:
            var linejump = new LineJumpParser().SetSymbol();
            var lineComment = new BeginEndParser(new WordParser("//"), new LineJumpParser(), true).SetSymbol();
            var stringLiteral = new StringParser().SetSymbol();
            var whitespace = new WhitespaceParser().SetSymbol();

            var split = LexerSplitter.Split(new Substring(text), new[] { linejump, lineComment, stringLiteral, whitespace });

            var result = split.Select(x => Tuple.Create(x.Substring.Index, x.Substring.Length, x.Symbol));
            var expected = new[]
            {
                Tuple.Create (0,21, lineComment.Token ) ,
                Tuple.Create (21,6, Guid.Empty ) ,
                Tuple.Create (27,1, whitespace.Token ) ,
                Tuple.Create (28,1, Guid.Empty ) ,
                Tuple.Create (29,1, whitespace.Token ) ,
                Tuple.Create (30,10, stringLiteral.Token ) ,
                Tuple.Create (40,1, whitespace.Token ) ,
                Tuple.Create (41,1, Guid.Empty ) ,
                Tuple.Create (42,1, whitespace.Token ) ,
                Tuple.Create (43,4, Guid.Empty ) ,
            };

            Assert.IsTrue(expected.SequenceEqual(result));
        }
        [TestMethod]
        public void WordSplitterTest2()
        {
            var text = "a === bcd ==\t e=+1.0";

            //parsers:
            var equal3 = new WordParser("===").SetSymbol();
            var equals = new WordParser("==").SetSymbol();
            var assign = new WordParser("=").SetSymbol();
            var plus = new WordParser("+").SetSymbol();
            var space = new WhitespaceParser().SetSymbol();
            var number = new NumberParser().SetSymbol();

            var parsers = new[] { equal3, equals, assign, plus, space, number };

            var split = LexerSplitter.Split(text.AsSubstring(), parsers);
            var expected = new[]
            {
                //Unidentified word 'a'
                text.AsSubstring (0,1).AsToken (Guid.Empty ),

                //whitespace
                text.AsSubstring(1,1).AsToken (space.Token ),
            
                //equals3 '==='
                text.AsSubstring(2, 3).AsToken (equal3.Token ),

                //whitespace
                text.AsSubstring(5,1).AsToken (space.Token ),

                //Unidentified word 'bcd'
                text.AsSubstring (6,3).AsToken (Guid.Empty ),

                //whitespace
                text.AsSubstring(9,1).AsToken (space.Token ),

                //equals
                text.AsSubstring (10,2).AsToken (equals.Token),

                //whitespace
                text.AsSubstring(12,2).AsToken (space.Token ),

                //Unidentified word 'e'
                text.AsSubstring (14,1).AsToken (Guid.Empty ),

                //asignment
                text.AsSubstring (15,1).AsToken (assign.Token),

                 //plus
                text.AsSubstring (16,1).AsToken (plus.Token),

                 //number
                text.AsSubstring (17,3).AsToken (number.Token),
            };

            Assert.IsTrue(expected.SequenceEqual(split));
        }

        [TestMethod]
        public void WordSplitterTest()
        {
            //Words that our splitter will identify
            var hola = new WordParser("hello").SetSymbol();
            var rafa = new WordParser("rafa").SetSymbol();

            var parsers = new[] { hola, rafa };

            //Text to split
            var text = "helloaloharafarafael";

            var result = LexerSplitter.Split(new Substring(text), parsers);
            var expected = new[]
            {
                //symbol "hola" on substring (0,5)
                text.AsSubstring (0,5).AsToken (hola.Token),

                //string "holis" on substring (4, 5)
                text.AsSubstring (5,5).AsToken (Guid.Empty),

                //symbol "rafa" on substring (9,4)
                text.AsSubstring (10,4).AsToken (rafa.Token),
                
                //symbol "rafa" on substring (13,4)
                text.AsSubstring (14,4).AsToken (rafa.Token),

                //string "el" on substring (17, 2)
                text.AsSubstring (18,2).AsToken (Guid.Empty)
            };

            Assert.IsTrue(expected.SequenceEqual(result));
        }
    }
}
