using System;
using System.Collections.Generic;

namespace Stump.Core.IO
{
    public class StringStream : ICloneable
    {
        private readonly string m_str;
        private int m_pos;

        public StringStream(string s)
            : this(s, 0)
        {
        }

        public StringStream(string s, int initialPos)
        {
            m_str = s;
            m_pos = initialPos;
        }

        public StringStream(StringStream stream)
            : this(stream.m_str, stream.m_pos)
        {
        }

        /// <summary>
        /// Indicates wether we did not reach the end yet.
        /// </summary>
        public bool HasNext
        {
            get
            {
                return m_pos < m_str.Length;
            }
        }

        /// <summary>
        /// The position within the initial string.
        /// </summary>
        public int Position
        {
            get
            {
                return m_pos;
            }
            set
            {
                m_pos = value;
            }
        }

        /// <summary>
        /// The remaining length (from the current position until the end).
        /// </summary>
        public int Length
        {
            get
            {
                return m_str.Length - m_pos;
            }
        }

        /// <summary>
        /// The remaining string (from the current position until the end).
        /// </summary>
        public string Remainder
        {
            get
            {
                if (!HasNext)
                    return "";
                return m_str.Substring(m_pos, Length);
            }
        }

        /// <summary>
        /// The wrapped string.
        /// </summary>
        public string String
        {
            get
            {
                return m_str;
            }
        }

        /// <summary>
        /// [Not implemented]
        /// </summary>
        public string this[int index]
        {
            get
            {
                return "";
            }
        }

        #region ICloneable Members

        public Object Clone()
        {
            var ss = new StringStream(m_str) {m_pos = m_pos};
            return ss;
        }

        #endregion

        /// <summary>
        /// Resets the position to the beginning.
        /// </summary>
        public void Reset()
        {
            m_pos = 0;
        }

        /// <summary>
        /// Increases the position by the given count.
        /// </summary>
        public void Ignore(int charCount)
        {
            m_pos += charCount;
        }

        /// <returns><code>NextLong(-1, \" \")</code></returns>
        public long NextLong()
        {
            return NextLong(-1, " ");
        }

        /// <returns><code>NextLong(defaultVal, \" \")</code></returns>
        public long NextLong(long defaultVal)
        {
            return NextLong(defaultVal, " ");
        }

        /// <returns>The next word as long.</returns>
        /// <param name="defaultVal">What should be returned if the next word cannot be converted into a long.</param>
        /// <param name="seperator">What the next word should be seperated by.</param>
        public long NextLong(long defaultVal, string seperator)
        {
            try
            {
                return long.Parse(NextWord(seperator));
            }
            catch
            {
                return defaultVal;
            }
        }

        /// <returns><code>NextInt(-1, \" \")</code></returns>
        public int NextInt()
        {
            return NextInt(-1, " ");
        }

        /// <returns><code>NextInt(defaultVal, \" \")</code></returns>
        public int NextInt(int defaultVal)
        {
            return NextInt(defaultVal, " ");
        }

        /// <returns>The next word as int.</returns>
        /// <param name="defaultVal">What should be returned if the next word cannot be converted into an int.</param>
        /// <param name="seperator">What the next word should be seperated by.</param>
        public int NextInt(int defaultVal, string seperator)
        {
            try
            {
                return int.Parse(NextWord(seperator));
            }
            catch
            {
                return defaultVal;
            }
        }

        public string PeekNextWord()
        {
            return PeekNextWord(" ");
        }

        public string PeekNextWord(string seperator)
        {
            var pos = m_pos;
            var word = NextWord(seperator);
            m_pos = pos;

            return word;
        }

        /// <summary>
        /// Calls <code>NextWord(" ")</code>.
        /// </summary>
        public string NextWord()
        {
            return NextWord(" ");
        }

        /// <summary>
        /// Moves the position behind the next word in the string, separated by <code>seperator</code> and returns the word.
        /// </summary>
        /// <remarks>Words into quote (" ") are counted as one word.</remarks>
        public string NextWord(string seperator)
        {
            var length = m_str.Length;
            if (m_pos >= length)
                return "";

            int x;
            while ((x = CustomIndexOf(seperator, m_pos, '\"')) == 0)
            {
                m_pos += seperator.Length;
            }

            if (x < 0)
            {
                if (m_pos == length)
                    return "";
                else
                    x = length;
            }
            var word = m_str.Substring(m_pos, x - m_pos);

            m_pos = x + seperator.Length;
            if (m_pos > length)
                m_pos = length;

            return word;
        }

        public string NextWords()
        {
            return NextWords(Length);
        }

        /// <returns><code>NextWords(count, \" \")</code></returns>
        public string NextWords(int count)
        {
            return NextWords(count, " ");
        }

        /// <returns>The next <code>count</code> word seperated by <code>seperator</code> as a string.</returns>
        public string NextWords(int count, string seperator)
        {
            string result = "";
            for (int i = 0; i < count && HasNext; i++)
            {
                if (i > 0)
                    result += seperator;
                result += NextWord(seperator);
            }
            return result;
        }

        /// <returns><code>NextWordsArray(count, " ")</code></returns>
        public string[] NextWordsArray(int count)
        {
            return NextWordsArray(count, " ");
        }

        /// <returns>The next <code>count</code> word seperated by <code>seperator</code> as an array of strings.</returns>
        public string[] NextWordsArray(int count, string sep)
        {
            var words = new string[count];
            for (int i = 0; i < count && HasNext; i++)
            {
                words[i] = NextWord(sep);
            }
            return words;
        }

        /// <summary>
        /// Calls <code>RemainingWords(" ")</code>
        /// </summary>
        public string[] RemainingWords()
        {
            return RemainingWords(" ");
        }

        public string[] RemainingWords(string seperator)
        {
            var words = new List<string>();
            while (HasNext)
            {
                words.Add(NextWord(seperator));
            }
            return words.ToArray();
        }

        /// <returns><code>Consume(' ')</code></returns>
        public void ConsumeSpace()
        {
            Consume(' ');
        }

        /// <summary>
        /// Calls <code>SkipWord(" ")</code>.
        /// </summary>
        public void SkipWord()
        {
            SkipWord(" ");
        }

        /// <summary>
        /// Skips the next word, seperated by the given seperator.
        /// </summary>
        public void SkipWord(string seperator)
        {
            SkipWords(1, seperator);
        }

        /// <summary>
        /// Calls <code>SkipWords(count, " ")</code>.
        /// </summary>
        /// <param name="count">The amount of words to be skipped.</param>
        public void SkipWords(int count)
        {
            SkipWords(count, " ");
        }

        /// <summary>
        /// Skips <code>count</code> words, seperated by the given seperator.
        /// </summary>
        /// <param name="count">The amount of words to be skipped.</param>
        public void SkipWords(int count, string seperator)
        {
            NextWords(count, seperator);
        }

        /// <summary>
        /// Consume a whole string, as often as it occurs.
        /// </summary>
        public void Consume(string rs)
        {
            while (HasNext)
            {
                int i = 0;
                for (; i < rs.Length; i++)
                {
                    if (m_str[m_pos + i] != rs[i])
                    {
                        return;
                    }
                }
                m_pos += i;
            }
        }

        /// <summary>
        /// Ignores all directly following characters that are equal to <code>c</code>.
        /// </summary>
        public void Consume(char c)
        {
            while (HasNext && m_str[m_pos] == c)
                m_pos++;
        }

        /// <summary>
        /// Ignores a maximum of <code>amount</code> characters that are equal to <code>c</code>.
        /// </summary>
        public void Consume(char c, int amount)
        {
            for (int i = 0; i < amount && HasNext && m_str[m_pos] == c; i++)
                m_pos++;
        }

        /// <summary>
        /// Consumes the next character, if it equals <code>c</code>.
        /// </summary>
        /// <returns>Wether the character was equal to <code>c</code> (and thus has been deleted)</returns>
        public bool ConsumeNext(char c)
        {
            if (HasNext && m_str[m_pos] == c)
            {
                m_pos++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Consumes the next character, if it equals <code>c</code>.
        /// </summary>
        /// <returns>Wether the character was equal to <code>c</code> (and thus has been deleted)</returns>
        public bool ConsumeNext(string str)
        {
            if (Remainder.StartsWith(str))
            {
                m_pos += str.Length;
                return true;
            }
            return false;
        }

        /// <returns>Wether or not the remainder contains the given string.</returns>
        public bool Contains(string s)
        {
            return s.IndexOf(s, m_pos) > -1;
        }

        /// <returns>Wether or not the remainder contains the given char.</returns>
        public bool Contains(char c)
        {
            return m_str.IndexOf(c, m_pos) > -1;
        }

        public override string ToString()
        {
            return Remainder.Trim();
        }

        public StringStream CloneStream()
        {
            return Clone() as StringStream;
        }

        private int CustomIndexOf(string searchedChar, int startIndex, char containerChar)
        {
            if (searchedChar.Length > 1)
                throw new ArgumentException("searchedChar can only be a char into a string");

            char search = searchedChar[0];

            int index = startIndex;

            bool inContainer = false;
            do
            {
                if (m_str[index] == containerChar)
                    inContainer = !inContainer;

                index++;

                if (index >= m_str.Length)
                    return -1;

            } while (inContainer || m_str[index] != search);

            return index;
        }
    }

}