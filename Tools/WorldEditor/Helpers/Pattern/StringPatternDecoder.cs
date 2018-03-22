using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldEditor.Helpers.Pattern
{
    public class StringPatternDecoder
    {
        public string EncodedString { get; private set; }

        public object[] Arguments { get; private set; }

        public StringPatternDecoder(string encodedString, object[] arguments)
        {
            this.EncodedString = encodedString;
            this.Arguments = arguments;
        }

        public int? CheckValidity(bool canThrow = true)
        {
            Stack<int> stack = new Stack<int>();
            int num = 0;
            int num2 = 0;
            int i = 0;
            while (i < this.EncodedString.Length)
            {
                char c = this.EncodedString[i];
                int? result;
                if ((c != '#' && c != '~') || (i + 1 < this.EncodedString.Length && char.IsDigit(this.EncodedString[i + 1])))
                {
                    if (c == '{')
                    {
                        num++;
                    }
                    else
                    {
                        if (c == '}')
                        {
                            num--;
                        }
                    }
                    if (c == '[')
                    {
                        num2++;
                        stack.Push(i);
                    }
                    else
                    {
                        if (c == ']')
                        {
                            num2--;
                            int num3 = stack.Pop();
                            string text = this.EncodedString.Substring(i + 1, num3 - 1 - i);
                            if (string.IsNullOrEmpty(text) || !text.All(new Func<char, bool>(char.IsDigit)))
                            {
                                if (canThrow)
                                {
                                    throw new InvalidPatternException("Attempt a number between [ and ]", num3);
                                }
                                result = new int?(i);
                                return result;
                            }
                        }
                    }
                    i++;
                    continue;
                }
                if (canThrow)
                {
                    throw new InvalidPatternException("Attempt a digit after '" + c + "'", i);
                }
                result = new int?(i);
                return result;
            }
            if (num != 0)
            {
                if (canThrow)
                {
                    throw new InvalidPatternException("'{' not closed");
                }
                int? result = new int?(0);
                return result;
            }
            else
            {
                int? result;
                if (num2 == 0)
                {
                    result = null;
                    return result;
                }
                if (canThrow)
                {
                    throw new InvalidPatternException("'[' not closed");
                }
                result = new int?(0);
                return result;
            }
        }

        public string Decode()
        {
            this.CheckValidity(true);
            return this.DecodeInternal(this.EncodedString, this.Arguments);
        }

        private string DecodeInternal(string str, object[] args)
        {
            StringBuilder stringBuilder = new StringBuilder();
            new Stack<int>();
            new Stack<int>();
            string result;
            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                if (c == '#')
                {
                    int num = int.Parse(str[i + 1].ToString());
                    if (args.Length > num - 1)
                    {
                        stringBuilder.Append(args[num - 1]);
                    }
                    i++;
                }
                else
                {
                    if (c == '~')
                    {
                        int num = int.Parse(str[i + 1].ToString());
                        if (args.Length <= num - 1 || args[num - 1] == null)
                        {
                            result = stringBuilder.ToString();
                            return result;
                        }
                        i++;
                    }
                    else
                    {
                        if (c == '{')
                        {
                            int num2 = 1;
                            int num3 = i + 1;
                            while (num2 > 0 && num3 < str.Length)
                            {
                                if (str[num3] == '{')
                                {
                                    num2++;
                                }
                                else
                                {
                                    if (str[num3] == '}')
                                    {
                                        num2--;
                                    }
                                }
                                num3++;
                            }
                            stringBuilder.Append(this.DecodeInternal(str.Substring(i + 1, num3 - 2 - i), args));
                            i = num3 - 1;
                        }
                        else
                        {
                            if (c == '[')
                            {
                                int num2 = 1;
                                int num3 = i + 1;
                                while (num2 > 0 && num3 < str.Length)
                                {
                                    if (str[num3] == '[')
                                    {
                                        num2++;
                                    }
                                    else
                                    {
                                        if (str[num3] == ']')
                                        {
                                            num2--;
                                        }
                                    }
                                    num3++;
                                }
                                string s = str.Substring(i + 1, num3 - 2 - i);
                                int num4 = int.Parse(s);
                                if (args.Length > num4 - 1)
                                {
                                    stringBuilder.Append(args[num4 - 1]);
                                }
                                i = num3 - 1;
                            }
                            else
                            {
                                stringBuilder.Append(c);
                            }
                        }
                    }
                }
            }
            result = stringBuilder.ToString();
            return result;
        }
    }
}