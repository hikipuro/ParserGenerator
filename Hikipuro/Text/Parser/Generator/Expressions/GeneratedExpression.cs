using Hikipuro.Text.Interpreter;
using Hikipuro.Text.Tokenizer;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using TokenType = Hikipuro.Text.Parser.Generator.GeneratedParser.TokenType;
using ExpressionType = Hikipuro.Text.Parser.Generator.GeneratedParser.ExpressionType;

namespace Hikipuro.Text.Parser.Generator.Expressions {
	/// <summary>
	/// ベースクラス.
	/// </summary>
	public class GeneratedExpression : Expression<GeneratorContext> {
		/// <summary>
		/// デバッグ用.
		/// true で処理中にデバッグメッセージを表示する.
		/// </summary>
		public bool DebugFlag = true;

		/// <summary>
		/// Expression の種類.
		/// </summary>
		public ExpressionType Type = ExpressionType.Nonterminal;

		/// <summary>
		/// 名前.
		/// </summary>
		public string Name = string.Empty;

		/// <summary>
		/// 子要素の Expression のリスト.
		/// </summary>
		public List<GeneratedExpression> Expressions;

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		public GeneratedExpression() {
			Expressions = new List<GeneratedExpression>();
		}
		
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public virtual void Interpret(GeneratorContext context) {
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
		/// Expression を追加する.
		/// </summary>
		/// <param name="exp"></param>
		public void AddExpression(GeneratedExpression exp) {
			Expressions.Add(exp);
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
		/// パースエラーを発生させる.
		/// </summary>
		/// <param name="message">メッセージ.</param>
		/// <param name="token">エラー発生箇所のトークン.</param>
		public void ThrowParseException(string message, Token<TokenType> token = null) {
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

		/// <summary>
		/// オブジェクトの内容を文字列で取得する.
		/// </summary>
		/// <returns></returns>
		public new string ToString() {
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(BuilderExpression) ");
			stringBuilder.Append(Name);
			stringBuilder.Append(" ");
			stringBuilder.Append(ToString(0));
			return stringBuilder.ToString();
		}

		/// <summary>
		/// オブジェクトの内容を文字列で取得する.
		/// </summary>
		/// <param name="depth"></param>
		/// <returns></returns>
		private string ToString(int depth) {
			StringBuilder stringBuilder = new StringBuilder();
			string tab = new string('\t', depth);
			string tab1 = new string('\t', depth + 1);
			stringBuilder.AppendLine("{");
			foreach (GeneratedExpression exp in Expressions) {
				stringBuilder.Append(tab1);
				stringBuilder.Append("(" + exp.Type + ") ");
				stringBuilder.Append(exp.Name);

				if (exp.Type == ExpressionType.Terminal) {
					stringBuilder.AppendLine("");
				} else if (exp.Type == ExpressionType.Nonterminal) {
					stringBuilder.AppendLine("");
				} else {
					stringBuilder.Append(" ");
					string text = exp.ToString(depth + 1);
					stringBuilder.Append(text);
				}
			}
			stringBuilder.Append(tab);
			stringBuilder.AppendLine("}");
			return stringBuilder.ToString();
		}
	}
}
