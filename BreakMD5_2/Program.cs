using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Combinatorics.Collections;
using System.Security.Cryptography;

namespace BreakMD5_2
{
    class Program
    {
        static void Main(string[] args)
        {
            int counter = 0;
            string[] allWords = System.IO.File.ReadAllLines(@"D:\jagenau\Documents\Visual Studio 2013\Projects\BreakMD5\wordTest.txt");
            string allWordsString = string.Join("", allWords);

            char[] allChars = allWordsString.ToCharArray();
            char[] allCharsUnique = allChars.Distinct().ToArray();
            char[] forbiddenChars = allCharsUnique;

            string letters = "poultry outwits ants";
            letters = String.Concat(letters.OrderBy(c => c));
            char[] allowedLetters = letters.ToCharArray();

            string secretHash = "4624d200580677270a54ccff86b9610e";
            secretHash = secretHash.ToUpper();

            foreach (char character in forbiddenChars)
            {
                foreach (char allowedCharacter in allowedLetters)
                {
                    if (allowedCharacter == character)
                    {
                        forbiddenChars = forbiddenChars.Where(val => val != character).ToArray();
                    }
                }
            }
            foreach (string word in allWords)
            {
                bool flag = false;
                foreach (char character in forbiddenChars)
                {
                    if (word.Contains(character))
                    {
                        counter++;
                        Console.WriteLine(counter);
                        allWords = allWords.Where(val => val != word).ToArray();
                        flag = true;
                        break;
                    }
                }
                if (flag) continue;
            }
            combine(allWords, letters, secretHash);
        }
        static List<string> permute(List<string> comb)
        {

            var result =
                from a in comb
                from b in comb
                from c in comb
                where a != b && a != c && b != c
                select a + " " + b + " " + c;

            return result.ToList();

        }
        static void combine(string[] allCurrentWords, string letters, string secret)
        {
          

            Combinations<string> comb = new Combinatorics.Collections.Combinations<string>(allCurrentWords, 3);
    
                foreach (var v in comb)
                {
                    bool flag = false;
                    string currentString = string.Join(" ", v);
                    string currentOrderedString = String.Concat(currentString.OrderBy(c => c));

                    if (currentOrderedString == letters)
                    {
                        string currentStringMd5 = GetMd5Hash(currentString);
                        if (currentStringMd5 == secret)
                        {
                            Console.WriteLine(currentString);
                            flag = true ;
                            break;
                        }
                        else
                        {
                            string[] combA = currentString.Split(' ');
                            List<string> combList = combA.ToList();
                            List<string> permutations = permute(combList);

                            foreach (string perm in permutations)
                            {
                                currentStringMd5 = GetMd5Hash(perm);
                                if (currentStringMd5 == secret)
                                {
                                    Console.WriteLine(perm);
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    } if (flag) break;
                } 
        }

        // Copied from msdn.com FAQ
        public static string GetMd5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
