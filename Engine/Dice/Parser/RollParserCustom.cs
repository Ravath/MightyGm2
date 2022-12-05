using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QUT.Gppg;
using System.IO;

namespace Engine.Dice.Parser {
	internal partial class Parser
	{
		#region Properties

		#endregion

		/// <summary>
		/// Default constructor.
		/// </summary>
		internal Parser() : base(null) { }

		internal class Lexer : QUT.Gppg.AbstractScanner<ValueType, LexLocation>
		{
			private System.IO.TextReader reader;

			/// <summary>
			/// Trivial constructor.
			/// </summary>
			/// <param name="reader">Le texte ・parser.</param>
			public Lexer( System.IO.TextReader reader )
			{
				this.reader = reader;
			}
			/// <summary>
			/// Unique caracter processing.
			/// </summary>
			/// <returns>Le code du caractere suivant.</returns>
			public override int yylex() {
				{
					char ch;
					int ord = reader.Read();

					// Must check for EOF
					if (ord == -1)
						return (int)Tokens.EOF;
					else
						ch = (char)ord;

					//DIGIT
					if (char.IsDigit(ch))
					{
						yylval.iVal = (int)char.GetNumericValue(ch);
						return (int)Tokens.DIGIT;
					}
					//OTHER
					else
					{
						return ch;
						//Console.Error.WriteLine("Illegal character '{0}'", ch);
						//return yylex();
					}
				}
			}
			/// <summary>
			/// En cas d'erreur, affiche le message dans la console.
			/// </summary>
			/// <param name="format"></param>
			/// <param name="args"></param>
			public override void yyerror( string format, params object[] args )
			{
				Console.Error.WriteLine(format, args);
			}
		}

		/// <summary>
		/// Exception class.
		/// </summary>
		private class ParserException : Exception
		{
			public ParserException( string mess ) : base(mess) { }
		}
	}

}
