using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(actualToken, new Token(expectedValue, startIndex, expectedLength));
        }

        // Добавьте свои тесты
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
                char[] startChar = {'\'','"'};                  //Записываем какие кавычки " или ';
                string actualline = line.Substring(startIndex);     //Берём подстроку за кавычками;
                int tokenLenght = actualline.Length;                
                for (int i = 0; i < line.Length; i++)
                {
                

                if (startChar.Contains(line[i]) && (line[i] =='\'' || line[i] == '\"' ) && (i - 1 > 0) && (actualline[i - 1] != '\\'))
                    {
                        actualline = line.Substring(startIndex + 1, i-1);
                        tokenLenght = actualline.Length + 2;
                        ReadQuotedField(actualline,i);
                        break;
                    }
                }
            return new Token(actualline, startIndex, tokenLenght);
        }
    }
}
