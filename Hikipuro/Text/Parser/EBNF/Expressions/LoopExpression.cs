﻿using Hikipuro.Text.Parser.Generator;
using Hikipuro.Text.Parser.Generator.Expressions;
using Hikipuro.Text.Tokenizer;
using System.Text;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	/// <summary>
	/// EBNF のループ処理用.
	/// </summary>
	class LoopExpression : BaseExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			DebugLog(": LoopExpression.Interpret()");

			// 戻り値の準備
			StringBuilder pattern = new StringBuilder();
			GeneratedExpression exp = ExpressionFactory.CreateLoop();

			// 最初のトークンをチェック
			Token<TokenType> token = context.Current;
			CheckTokenExists(token);
			CheckFirstToken(token);
			pattern.Append(token.Text);

			// 2 番目のトークンをチェック
			token = context.Next();
			CheckTokenExists(token);

			bool loop = true;
			while (loop) {
				DebugLog(": LoopExpression: (" + token.Type + ")");

				switch (token.Type) {
				case TokenType.String:
				case TokenType.Name:
					Token<TokenType> nextToken = token.Next;
					if (nextToken != null && nextToken.Type == TokenType.Or) {
						GeneratedExpression orExp = ParseOr(context);
						exp.AddExpression(orExp);
						pattern.Append(orExp.Name);
						token = context.Current;
						CheckTokenExists(token);
					}
					break;
				}
				if (token.IsLast) {
					ThrowParseException(ErrorMessages.UnexpectedToken, token);
					break;
				}

				switch (token.Type) {
				case TokenType.CloseBrace:
					loop = false;
					pattern.Append(token.Text);
					token = context.Next();
					break;
				case TokenType.String:
					exp.AddExpression(
						ParseTerminal(context)
					);
					pattern.Append(token.Text);
					token = context.Next();
					break;
				case TokenType.Name:
					exp.AddExpression(
						ParseNonterminal(context, token.Text)
					);
					pattern.Append(token.Text);
					token = context.Next();
					break;
				case TokenType.OpenBracket:
					GeneratedExpression optionExp = ParseOption(context);
					exp.AddExpression(optionExp);
					pattern.Append(optionExp.Name);
					token = context.Current;
					break;
				case TokenType.Comma:
					CheckComma(token);
					pattern.Append(token.Text);
					token = context.Next();
					break;
				case TokenType.Minus:
					exp.AddExpression(
						ParseException(context)
					);
					pattern.Append(token.Text);
					token = context.Current;
					break;
				default:
					ThrowParseException(ErrorMessages.UnexpectedToken, token);
					break;
				}

				CheckTokenExists(token);
				if (token.IsLast) {
					//ThrowParseException(ErrorMessages.UnexpectedToken, token);
					break;
				}
			}

			// 戻り値
			exp.Name = pattern.ToString();
			context.PushExpression(exp);
			//GeneratedExpression.Name = pattern;
			DebugLog(": LoopExpression.Pattern: " + pattern);
		}

		/// <summary>
		/// 最初のトークンをチェックする.
		/// </summary>
		/// <param name="token">トークン.</param>
		private void CheckFirstToken(Token<TokenType> token) {
			CheckTokenType(token, new TokenType[] {
				TokenType.OpenBrace
			});
		}
	}
}
