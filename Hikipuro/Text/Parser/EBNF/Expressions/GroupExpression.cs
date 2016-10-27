﻿using Hikipuro.Text.Parser.EBNF.Generator;
using Hikipuro.Text.Tokenizer;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	/// <summary>
	/// EBNF のグループ処理用.
	/// </summary>
	class GroupExpression : BaseExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			DebugLog(": GroupExpression.Interpret()");

			// 戻り値の準備
			string pattern = string.Empty;
			Generator = GeneratorExpression.CreateGroup();

			// 最初のトークンをチェック
			Token<TokenType> token = context.Current;
			CheckTokenExists(token);
			CheckFirstToken(token);

			bool loop = true;
			while (loop) {
				DebugLog(": GroupExpression: (" + token.Type + ")");

				switch (token.Type) {
				case TokenType.String:
				case TokenType.Name:
					Token<TokenType> nextToken = token.Next;
					if (nextToken != null && nextToken.Type == TokenType.Or) {
						ParseOr(context);
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
				case TokenType.OpenParen:
					pattern += token.Text;
					token = context.Next();
					break;
				case TokenType.CloseParen:
					pattern += token.Text;
					loop = false;
					token = context.Next();
					break;
				case TokenType.String:
					pattern += token.Text;
					ParseTerminal(context);
					token = context.Next();
					break;
				case TokenType.Name:
					pattern += token.Text;
					ParseNonterminal(context, token.Text);
					token = context.Next();
					break;
				case TokenType.Comma:
					CheckComma(token);
					token = context.Next();
					break;
				default:
					ThrowParseException(ErrorMessages.UnexpectedToken, token);
					break;
				}

				CheckTokenExists(token);
				if (token.IsLast) {
					ThrowParseException(ErrorMessages.UnexpectedToken, token);
					break;
				}
			}

			// 戻り値
			Generator.Name = pattern;
			DebugLog(": GroupExpression.Pattern: " + pattern);
		}

		/// <summary>
		/// 最初のトークンをチェックする.
		/// </summary>
		/// <param name="token">トークン.</param>
		private void CheckFirstToken(Token<TokenType> token) {
			CheckTokenType(token, new TokenType[] {
				TokenType.OpenParen
			});
		}
	}
}