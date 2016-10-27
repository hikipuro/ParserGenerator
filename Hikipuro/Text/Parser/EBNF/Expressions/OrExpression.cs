using Hikipuro.Text.Parser.EBNF.Generator;
using Hikipuro.Text.Tokenizer;
using System.Diagnostics;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	class OrExpression : BaseExpression {
		public string Pattern = string.Empty;

		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			Debug.WriteLine(": OrExpression.Interpret()");

			Generator = GeneratorExpression.CreateOr();

			Token<TokenType> token = context.Current;
			CheckTokenExists(token);

			bool loop = true;
			while (loop) {
				Debug.WriteLine(": OrExpression.loop: " + token.Type);
				switch (token.Type) {
				case TokenType.String: {
						Pattern += token.Text;
						StringExpression exp = new StringExpression();
						exp.Interpret(context);
						Generator.AddExpression(exp.Generator);
					}
					break;
				case TokenType.Name: {
						Pattern += token.Text;
						GeneratorExpression exp = GeneratorExpression.CreateNonterminal(token.Text);
						Generator.AddExpression(exp);
					}
					break;
				case TokenType.Or:
					Pattern += "|";
					break;
				default:
					loop = false;
					//throw new InterpreterException("OrExpression.Interpret() Error");
					break;
				}

				if (token.Next == null) {
					break;
				}

				if (loop) {
					token = context.Next();
					if (token == null) {
						loop = false;
					}
				}
			}

			Generator.Name = Pattern;
			Debug.WriteLine(": OrExpression.Pattern: " + Pattern);
			//Debug.WriteLine("OrExpression.token.Type: " + token.Type);
		}

		private void CheckFirstToken(Token<TokenType> token) {
			CheckTokenType(token, new TokenType[] {
				TokenType.String,
				TokenType.Name,
				TokenType.OpenBrace,
				TokenType.OpenBracket
			});
		}
	}
}
