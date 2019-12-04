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
        [TestCase("'a\''", 0, "a'", 5)]

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
            int tokenLenght=1;
            int quotedValue=1;
            var startChar = line[startIndex];
            string tokenValue = null;

            if (startIndex+1<line.Length && line[startIndex + 1] == startChar)
                return new Token(line, startIndex, tokenLenght+1);
            
            else if (startChar != '"' && startChar != '\'')
            {
                tokenValue=line.Substring(startIndex).Split(' ')[0];
                tokenLenght = tokenValue.Length;
            }
           
            else
                for (int i = startIndex + 1; i < line.Length; i++)
                {
                                                              
                    if (line[i] == startChar && line[i - 1] != '\\')
                    {
                        quotedValue += 1;
                        tokenLenght = tokenValue.Length + quotedValue;  
                        break;
                    }
                    else if (line[i] == '\\' && i + 1 < line.Length&&line[i+1]==startChar)
                        quotedValue++;                      
                    else
                    tokenValue += line[i];
                    tokenLenght = tokenValue.Length + quotedValue;      
                }
            
            if (startIndex + tokenLenght + 1 < line.Length)
                ReadQuotedField(line, startIndex + tokenLenght + 1);
            return new Token(tokenValue, startIndex, tokenLenght);
        }
    }
}
