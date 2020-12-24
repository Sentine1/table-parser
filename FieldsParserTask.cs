using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }
        [TestCase("text", new[] { "text" })]
        [TestCase("text   ", new[] { "text" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("hello   world", new[] { "hello", "world" })]
        [TestCase(@"""hello"" world", new[] { @"hello", "world" })]
        [TestCase(@"""hello my"" world", new[] { @"hello my", "world" })]
        [TestCase(@"""'hello'""", new[] { @"'hello'" })]
        [TestCase(@"'""hello""'", new[] { @"""hello""" })]
        [TestCase(@"' ' "" "" ", new[] { @" ", @" " })]
        [TestCase(@"""hello\""", new[] { @"hello""" })]
        [TestCase(@"'hello\''", new[] { @"hello'" })]
        [TestCase(@"""hello  ", new[] { @"hello  " })]
        [TestCase(@"hello\/", new[] { @"hello\/" })]
        [TestCase(@"", new string[] { })]
        [TestCase(@"""""", new[] { @"" })]
        [TestCase(@"ab""c d""", new[] { @"ab", "c d" })]
        [TestCase(@"""""", new[] { @"" })]
        [TestCase(@"""""\", new[] { @"", @"\" })]
        [TestCase(@"""\\""", new[] { @"\" })]

        //[TestCase(@"""a b"" cd", new[] {@"a b", "cd"})]
        //[TestCase(@"test\\ alll", new[] {@"test\\", @"alll"})]
        //[TestCase(@"hello 'all' my",new[] {@"hello","all","my"})]

        // Вставляйте сюда свои тесты
        public static void RunTests(string input, string[] expectedOutput)
        {
            // Тело метода изменять не нужно
            Test(input, expectedOutput);
        }
        // Скопируйте сюда метод с тестами из предыдущей задачи.
    }

    public class FieldsParserTask
    {
        // При решении этой задаче постарайтесь избежать создания методов, длиннее 10 строк.
        // Подумайте как можно использовать ReadQuotedField и Token в этой задаче.
        public static List<Token> ParseLine(string line)
        {
            char[] quotes = new char[] { '"', '\'' };
            var allToken = new List<Token>(); // сокращенный синтаксис для инициализации коллекции.
            int nextToken = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (quotes.Contains(line[i]) && nextToken <= i)
                {
                    allToken.Add(ReadQuotedField(line, i));
                    nextToken = allToken[allToken.Count - 1].GetIndexNextToToken();
                }
                else if (nextToken <= i && line[i] != ' ')
                {
                    allToken.Add(ReadField(line, i));
                    nextToken = allToken[allToken.Count - 1].GetIndexNextToToken();
                }
            }
            return allToken;
        }

        private static Token ReadField(string line, int startIndex)
        {
            string tokenValue = line.Substring(startIndex).Split(' ', '\'', '"')[0];
            int filedIndex = tokenValue.Length;
            return new Token(tokenValue, startIndex, filedIndex);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }
    }
}
