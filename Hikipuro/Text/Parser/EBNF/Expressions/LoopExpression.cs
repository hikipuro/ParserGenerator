using Hikipuro.Text.Parser.EBNF.Generator;
using Hikipuro.Text.Tokenizer;
using System.Diagnostics;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	class LoopExpression : BaseExpression {
		public string Pattern = string.Empty;

		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			Debug.WriteLine(": LoopExpression.Interpret()");

			Generator = GeneratorExpression.CreateLoop();

			Token<TokenType> token = context.Current;
			bool loop = true;
			while (loop) {
				//Debug.WriteLine("LoopExpression.loop: " + token.Type);

				if (token.Next != null && token.Next.Type == TokenType.Or) {
					//Pattern += token.Text;
					OrExpression orExp = new OrExpression();
					orExp.Interpret(context);
					Pattern += orExp.Pattern;

					Generator.AddExpression(orExp.Generator);
					token = context.Current;
				}

				switch (token.Type) {
				case TokenType.OpenBrace:
					Pattern += token.Text;
					break;
				case TokenType.CloseBrace:
					Pattern += token.Text;
					loop = false;
					context.Next();
					break;
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
				case TokenType.Comma: {
						Pattern += token.Text;
						//token = context.Next();
					}
					break;
				//case TokenType.Or:
				//	break;
				default:
					loop = false;
					//throw new InterpreterException("LoopExpression.Interpret() Error");
					break;
				}

				if (token.Next == null) {
					break;
				}

				if (loop == false) {
					break;
				}

				token = context.Next();
				if (token == null) {
					loop = false;
				}
			}

			Generator.Name = Pattern;
			Debug.WriteLine(": LoopExpression.Pattern: " + Pattern);
			//Debug.WriteLine("LoopExpression.token.Type: " + token.Type);
		}
	}
}
