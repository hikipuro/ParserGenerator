using Hikipuro.Text.Interpreter;
using Result = Hikipuro.Text.Parser.Generator.GeneratorContext.Result;

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

			// 戻り値の準備
			Result result = new Result(Name);
			//Matches = new TokenMatches(Name);
			//IsMatch = false;

			if (context.Fields.ContainsKey(Name) == false) {
				throw new InterpreterException("NonterminalExpression.Interpret() Error");
			}

			GeneratedExpression exp = context.Fields[Name];
			exp.Interpret(context);
			Result itemResult = context.PopResult();

			result.IsMatch = itemResult.IsMatch;
			result.Matches.ConcatTokens(itemResult.Matches);

			context.PushResult(result);
		}
	}
}
