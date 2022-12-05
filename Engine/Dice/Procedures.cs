using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Engine.Dice
{
	public static class Procedures
	{
		private static Parser.Parser _parser;
		/// <summary>
		/// Singleton access to the parse.
		/// </summary>
		private static Parser.Parser Parser
		{
			get
			{
				if(_parser == null)
				{
					_parser = new Parser.Parser();
				}
				return _parser;
			}
		}

		public static IRoll Parse(string macro)
		{
			TextReader tr = new StringReader(macro);

			//Parse string
			Parser.Scanner = new Parser.Parser.Lexer(tr);
			
			if (Parser.Parse())
			{
				return Parser.result;
			}
			else
			{
				return null;
			}
		}

		public static string ParseFromText(string value)
		{
			Regex r = new Regex(@"\[\[[^\]]*\]\]");

			// Find first
			Match m = r.Match(value);
			string macro = m.Value;

			// If something found
			while (!String.IsNullOrWhiteSpace(macro))
			{
				string toParse = macro.Substring(2, macro.Length - 4);
				IRoll roll = Parse(macro.Substring(2, macro.Length - 4));
				int replacementResult;
				if (roll == null)
				{
					replacementResult = 0;
				}
				else
				{
					roll.Roll();
					replacementResult = roll.NetResult;
				}
				value = value.Replace(macro, ""+replacementResult);

				// Try find next
				m = r.Match(value);
				macro = m.Value;
			}
			return value;
		}
	}
}
