using Hikipuro.Text.Interpreter;
using Hikipuro.Text.Parser.EBNF.Generator;
using Hikipuro.Text.Tokenizer;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	class BaseExpression : Expression<EBNFContext> {
		public GeneratorExpression Generator;

		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public virtual void Interpret(EBNFContext context) {
		}

		public void CheckTokenExists(Token<TokenType> token) {
			if (token != null) {
				return;
			}
			ThrowInterpretException(
				ErrorMessages.TokenNotFound, token
			);
		}

		public void CheckTokenType(Token<TokenType> token, TokenType type) {
			if (token.Type == type) {
				return;
			}
			ThrowInterpretException(
				ErrorMessages.UnexpectedToken, token
			);
		}

		public void CheckTokenType(Token<TokenType> token, TokenType[] typeList) {
			if (typeList == null || typeList.Length <= 0) {
				return;
			}

			bool isMatch = false;
			foreach (TokenType type in typeList) {
				if (token.Type == type) {
					isMatch = true;
					break;
				}
			}

			if (isMatch == false) {
				ThrowInterpretException(
					ErrorMessages.UnexpectedToken, token
				);
			}
		}

		public void ThrowInterpretException(string message, Token<TokenType> token) {
			if (token == null) {
				throw new InterpreterException(string.Format(
					"{0}",
					message
				));
			}
			throw new InterpreterException(string.Format(
				"{0} (Line: {1}, Index: {2}) {3}",
				message,
				token.LineNumber,
				token.LineIndex,
				token.Text
			));
		}
	}
}
