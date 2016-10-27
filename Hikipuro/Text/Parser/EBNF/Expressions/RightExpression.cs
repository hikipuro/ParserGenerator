using Hikipuro.Text.Parser.EBNF.Generator;
using Hikipuro.Text.Tokenizer;
using System.Diagnostics;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	/// <summary>
	/// 式の右辺.
	/// </summary>
	class RightExpression : BaseExpression {

		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			Debug.WriteLine(": RightExpression.Interpret()");

			Token<TokenType> token = context.Current;
			CheckTokenExists(token);
			CheckFirstToken(token);

			bool loop = true;
			while (loop) {
				Debug.WriteLine(": RightExpression: (" + token.Type + ")");
				switch (token.Type) {
				case TokenType.String:
					if (token.Next != null && token.Next.Type == TokenType.Or) {
						OrExpression exp = new OrExpression();
						exp.Interpret(context);
						Generator.AddExpression(exp.Generator);
						token = context.Current;
					} else {
						StringExpression exp = new StringExpression();
						exp.Interpret(context);
						Generator.AddExpression(exp.Generator);
						token = context.Next();
						Debug.WriteLine(": Test: " + exp.Generator.Name);
					}
					break;
				case TokenType.Name: {
						if (token.Next != null && token.Next.Type == TokenType.Or) {
							OrExpression exp = new OrExpression();
							exp.Interpret(context);
							Generator.AddExpression(exp.Generator);
							token = context.Current;
						} else {
							GeneratorExpression exp = GeneratorExpression.CreateNonterminal(token.Text);
							Generator.AddExpression(exp);
							token = context.Next();
						}
					}
					break;
				case TokenType.OpenBrace: {
						LoopExpression exp = new LoopExpression();
						exp.Interpret(context);
						Generator.AddExpression(exp.Generator);
						token = context.Current;
					}
					break;
				case TokenType.OpenBracket: {
						OptionExpression exp = new OptionExpression();
						exp.Interpret(context);
						Generator.AddExpression(exp.Generator);
						token = context.Current;
					}
					break;
				case TokenType.Comma:
					CheckComma(token);
					token = context.Next();
					break;
				case TokenType.Semicolon:
					loop = false;
					break;
				default:
					ThrowInterpretException(ErrorMessages.UnexpectedToken, token);
					break;
				}

				if (token.TokenList.Last() == token) {
					break;
				}
				if (token == null) {
					break;
				}
			}
			//Pattern = pattern;
		}

		private void CheckComma(Token<TokenType> token) {
			if (token.Next.Type == TokenType.Comma) {
				ThrowInterpretException(ErrorMessages.UnexpectedToken, token);
			}
			if (token.Prev.Type == TokenType.Comma) {
				ThrowInterpretException(ErrorMessages.UnexpectedToken, token);
			}
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
