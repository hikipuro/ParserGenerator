using Hikipuro.Text.Parser.EBNF.Generator;
using Hikipuro.Text.Tokenizer;
using System.Diagnostics;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	class OptionExpression : BaseExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			Debug.WriteLine(": OptionExpression.Interpret()");

			Generator = GeneratorExpression.CreateOption();

			Token<TokenType> token = context.Current;
			bool loop = true;

			while (loop) {
				//Debug.WriteLine("OptionExpression: " + token.Text);
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
						Debug.WriteLine("Test: " + exp.Generator.Name);
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
				case TokenType.OpenBracket:
					token = context.Next();
					break;
				case TokenType.CloseBracket:
					loop = false;
					token = context.Next();
					break;
				default:
					ThrowInterpretException(ErrorMessages.UnexpectedToken, token);
					break;
				}
				if (loop == false) {
					break;
				}
				//token = context.Next();
			}
		}
	}
}
