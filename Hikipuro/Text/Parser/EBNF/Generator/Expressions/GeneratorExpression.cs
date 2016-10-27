using Hikipuro.Text.Interpreter;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Hikipuro.Text.Parser.EBNF.Generator {
	/// <summary>
	/// ベースクラス.
	/// </summary>
	public class GeneratorExpression : Expression<GeneratorContext> {
		/// <summary>
		/// デバッグ用.
		/// true で処理中にデバッグメッセージを表示する.
		/// </summary>
		public bool DebugFlag = true;

		public GeneratorExpressionType Type = GeneratorExpressionType.Nonterminal;
		public string Name = string.Empty;

		public List<GeneratorExpression> Expressions;
		public bool IsMatch = false;
		public GeneratorTokenMatches Matches;


		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public virtual void Interpret(GeneratorContext context) {
		}

		/// <summary>
		/// コンストラクタ.
		/// </summary>
		public GeneratorExpression() {
			Expressions = new List<GeneratorExpression>();
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

		public new string ToString() {
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(BuilderExpression) ");
			stringBuilder.Append(Name);
			stringBuilder.Append(" ");
			stringBuilder.Append(ToString(0));
			return stringBuilder.ToString();
		}

		public string ToString(int depth) {
			StringBuilder stringBuilder = new StringBuilder();
			string tab = new string('\t', depth);
			string tab1 = new string('\t', depth + 1);
			stringBuilder.AppendLine("{");
			foreach (GeneratorExpression exp in Expressions) {
				stringBuilder.Append(tab1);
				stringBuilder.Append("(" + exp.Type + ") ");
				stringBuilder.Append(exp.Name);

				if (exp.Type == GeneratorExpressionType.Terminal) {
					stringBuilder.AppendLine("");
				} else if (exp.Type == GeneratorExpressionType.Nonterminal) {
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


		public static GeneratorExpression CreateNonterminal(string name) {
			GeneratorNonterminalExpression exp = new GeneratorNonterminalExpression();
			exp.Type = GeneratorExpressionType.Nonterminal;
			exp.Name = name;
			return exp;
		}
		public static GeneratorExpression CreateTerminal(string name) {
			GeneratorTerminalExpression exp = new GeneratorTerminalExpression();
			exp.Type = GeneratorExpressionType.Terminal;
			exp.Name = name;
			return exp;
		}
		public static GeneratorExpression CreateField() {
			GeneratorFieldExpression exp = new GeneratorFieldExpression();
			exp.Type = GeneratorExpressionType.Field;
			return exp;
		}
		public static GeneratorExpression CreateOption() {
			GeneratorOptionExpression exp = new GeneratorOptionExpression();
			exp.Type = GeneratorExpressionType.Option;
			return exp;
		}
		public static GeneratorExpression CreateOr() {
			GeneratorOrExpression exp = new GeneratorOrExpression();
			exp.Type = GeneratorExpressionType.Or;
			return exp;
		}
		public static GeneratorExpression CreateLoop() {
			GeneratorLoopExpression exp = new GeneratorLoopExpression();
			exp.Type = GeneratorExpressionType.Loop;
			return exp;
		}
		public static GeneratorExpression CreateGroup() {
			GeneratorGroupExpression exp = new GeneratorGroupExpression();
			exp.Type = GeneratorExpressionType.Group;
			return exp;
		}

		public void AddExpression(GeneratorExpression exp) {
			Expressions.Add(exp);
		}
	}
}
