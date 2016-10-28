using Hikipuro.Text.Interpreter;
using Hikipuro.Text.Parser.Generator;
using Hikipuro.Text.Parser.Generator.Expressions;
using Hikipuro.Text.Tokenizer;
using System.Diagnostics;
using TokenType = Hikipuro.Text.Parser.EBNF.EBNFParser.TokenType;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	/// <summary>
	/// EBNF のパース時に使用する Expression のベースクラス.
	/// </summary>
	class BaseExpression : Expression<EBNFContext> {
		/// <summary>
		/// デバッグ用.
		/// true で処理中にデバッグメッセージを表示する.
		/// </summary>
		public bool DebugFlag = true;

		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public virtual void Interpret(EBNFContext context) {
		}

		/// <summary>
		/// デバッグ用.
		/// DebugFlag が true の時のみメッセージを表示する.
		/// </summary>
		/// <param name="text">表示するメッセージ.</param>
		public void DebugLog(string text) {
			if (DebugFlag == false) {
				return;
			}
			Debug.WriteLine(text);
		}

		/// <summary>
		/// トークンの null チェック.
		/// null の場合は例外を発生させる.
		/// </summary>
		/// <param name="token">トークン.</param>
		public void CheckTokenExists(Token<TokenType> token) {
			if (token != null) {
				return;
			}
			ThrowParseException(
				ErrorMessages.TokenNotFound, token
			);
		}

		/// <summary>
		/// トークンの種類のチェック.
		/// 引数に指定されたトークンでない場合は例外を発生させる.
		/// </summary>
		/// <param name="token">トークン.</param>
		/// <param name="type">トークンの種類.</param>
		public void CheckTokenType(Token<TokenType> token, TokenType type) {
			if (token.Type == type) {
				return;
			}
			ThrowParseException(
				ErrorMessages.UnexpectedToken, token
			);
		}

		/// <summary>
		/// トークンの種類のチェック.
		/// 引数に指定されたトークンでない場合は例外を発生させる.
		/// </summary>
		/// <param name="token">トークン.</param>
		/// <param name="typeList">トークンの種類.</param>
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
				ThrowParseException(
					ErrorMessages.UnexpectedToken, token
				);
			}
		}

		/// <summary>
		/// コンマが正常な位置にあるかチェックする.
		/// </summary>
		/// <param name="token"></param>
		public void CheckComma(Token<TokenType> token) {
			if (token.Next.Type == TokenType.Comma) {
				ThrowParseException(ErrorMessages.UnexpectedToken, token);
			}
			if (token.Prev.Type == TokenType.Comma) {
				ThrowParseException(ErrorMessages.UnexpectedToken, token);
			}
		}

		/// <summary>
		/// StringExpression の処理を実行する.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public GeneratedExpression ParseTerminal(EBNFContext context) {
			StringExpression exp = new StringExpression();
			exp.Interpret(context);
			return context.PopExpression();
		}

		/// <summary>
		/// 非終端文字の処理を実行する.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="name"></param>
		public GeneratedExpression ParseNonterminal(EBNFContext context, string name) {
			return ExpressionFactory.CreateNonterminal(name);
		}

		public GeneratedExpression ParseField(EBNFContext context) {
			FieldExpression exp = new FieldExpression();
			exp.Interpret(context);
			return context.PopExpression();
		}

		public GeneratedExpression ParseRight(EBNFContext context) {
			RightExpression exp = new RightExpression();
			exp.Interpret(context);
			return context.PopExpression();
		}

		/// <summary>
		/// OrExpression の処理を実行する.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public GeneratedExpression ParseOr(EBNFContext context) {
			OrExpression exp = new OrExpression();
			exp.Interpret(context);
			return context.PopExpression();
		}

		/// <summary>
		/// GroupExpression の処理を実行する.
		/// </summary>
		/// <param name="context"></param>
		public GeneratedExpression ParseGroup(EBNFContext context) {
			GroupExpression exp = new GroupExpression();
			exp.Interpret(context);
			return context.PopExpression();
		}

		/// <summary>
		/// LoopExpression の処理を実行する.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public GeneratedExpression ParseLoop(EBNFContext context) {
			LoopExpression exp = new LoopExpression();
			exp.Interpret(context);
			return context.PopExpression();
		}

		/// <summary>
		/// OptionExpression の処理を実行する.
		/// </summary>
		/// <param name="context"></param>
		public GeneratedExpression ParseOption(EBNFContext context) {
			OptionExpression exp = new OptionExpression();
			exp.Interpret(context);
			return context.PopExpression();
		}

		/// <summary>
		/// ExceptionExpression の処理を実行する.
		/// </summary>
		/// <param name="context"></param>
		public GeneratedExpression ParseException(EBNFContext context) {
			ExceptionExpression exp = new ExceptionExpression();
			exp.Interpret(context);
			return context.PopExpression();
		}

		/// <summary>
		/// パースエラーを発生させる.
		/// </summary>
		/// <param name="message">メッセージ.</param>
		/// <param name="token">エラー発生箇所のトークン.</param>
		public void ThrowParseException(string message, Token<TokenType> token) {
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
