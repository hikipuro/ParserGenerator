using Hikipuro.Text.Interpreter;
using System.Diagnostics;

namespace Hikipuro.Text.Parser.EBNF.Generator {
	/// <summary>
	/// 非終端記号.
	/// </summary>
	class GeneratorNonterminalExpression : GeneratorExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(GeneratorContext context) {
			if (DebugFlag) {
				Debug.WriteLine("NonterminalExpression.Interpret(): " + Name);
			}
			Matches = new GeneratorTokenMatches(Name);
			IsMatch = false;

			if (context.Fields.ContainsKey(Name) == false) {
				throw new InterpreterException("NonterminalExpression.Interpret() Error");
			}

			GeneratorExpression exp = context.Fields[Name];
			exp.Interpret(context);
			IsMatch = exp.IsMatch;
			Matches.ConcatTokens(exp.Matches);
		}
	}
}
