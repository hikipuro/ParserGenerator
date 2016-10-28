using Hikipuro.Text.Interpreter;

namespace Hikipuro.Text.Parser.Generator.Expressions {
	/// <summary>
	/// 非終端記号.
	/// </summary>
	class NonterminalExpression : GeneratedExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(GeneratorContext context) {
			DebugLog("NonterminalExpression.Interpret(): " + Name);

			Matches = new TokenMatches(Name);
			IsMatch = false;

			if (context.Fields.ContainsKey(Name) == false) {
				throw new InterpreterException("NonterminalExpression.Interpret() Error");
			}

			GeneratedExpression exp = context.Fields[Name];
			exp.Interpret(context);
			IsMatch = exp.IsMatch;
			Matches.ConcatTokens(exp.Matches);
		}
	}
}
