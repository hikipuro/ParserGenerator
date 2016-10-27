using Hikipuro.Text.Interpreter;
using Hikipuro.Text.Tokenizer;
using System.Diagnostics;

namespace Hikipuro.Text.Parser.EBNF.Generator {
	/// <summary>
	/// 開始位置.
	/// </summary>
	public class GeneratorRootExpression : GeneratorExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(GeneratorContext context) {
			IsMatch = false;
			foreach (GeneratorExpression exp in Expressions) {
				if (DebugFlag) {
					Debug.WriteLine("RootExpression.Expressions : (" + exp.Name + ")");
				}
				exp.Interpret(context);
				if (exp.IsMatch) {
					IsMatch = true;
					break;
				}
			}

			Token<GeneratorTokenType> token = context.Current;

			if (IsMatch == false || token != null) {
				throw new InterpreterException(string.Format(
					"RootExpression.Interpret() Error: (Line: {0}, Index: {1})",
					token.LineNumber,
					token.LineIndex
				));
			}
		}
	}
}
