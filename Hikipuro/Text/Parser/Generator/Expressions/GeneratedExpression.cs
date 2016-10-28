using Hikipuro.Text.Interpreter;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
		/// マッチしたか.
		/// </summary>
		public bool IsMatch = false;

		/// <summary>
		/// マッチした要素.
		/// </summary>
		public TokenMatches Matches;


		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public virtual void Interpret(GeneratorContext context) {
		}

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		public GeneratedExpression() {
			Expressions = new List<GeneratedExpression>();
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
