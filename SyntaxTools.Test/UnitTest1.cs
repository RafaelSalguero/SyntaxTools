using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTools.DataStructures;
using SyntaxTools.Text.LexerUnits;
using SyntaxTools.Text.LexerUnits.StateMachines;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace SyntaxTools.Test
{
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
            Assert.IsTrue(hola1.Equals(hola2));
            Assert.IsTrue(rafa1_1.Equals(rafa1_2));
            Assert.IsFalse(hola1.Equals(rafa1_1));

        }

        [TestMethod]
        public void WordSplitterTest()
        {
            //Words that our splitter will identify
            var hola = new WordParser("hello").SetSymbol();
            var rafa = new WordParser("rafa").SetSymbol();

            //Create a text splitter with two simple word parsers
            var Splitter = new SubstringLexerSeparator(new[] { hola, rafa });

            //Text to sply
            var text = "helloaloharafarafael";

            var result = Splitter.Split(text);
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
