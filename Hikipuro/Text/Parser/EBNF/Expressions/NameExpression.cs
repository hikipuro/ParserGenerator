using Hikipuro.Text.Tokenizer;
using System.Diagnostics;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	/// <summary>
	/// 式の左辺.
	/// </summary>
	class NameExpression : BaseExpression {
		public string Name = string.Empty;

		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			DebugLog(": NameExpression.Interpret()");

			Token<TokenType> token = context.Current;
			bool loop = true;

			while (loop) {
				//Debug.WriteLine("NameExpression: " + token.Text);
				switch (token.Type) {
				case TokenType.Name:
					Name += token.Text;
					break;
				case TokenType.Space:
					Name += token.Text;
					break;
				case TokenType.Equal:
					loop = false;
					break;
				default:
					ThrowParseException(ErrorMessages.UnexpectedToken, token);
					break;
				}
				if (loop == false) {
					break;
				}
				token = context.Next();
			}
			Name = Name.TrimEnd();
			DebugLog(": NameExpression: (" + Name + ")");
		}
	}
}
