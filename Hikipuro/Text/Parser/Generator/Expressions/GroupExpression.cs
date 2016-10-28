using ExpressionType = Hikipuro.Text.Parser.Generator.GeneratedParser.ExpressionType;
using Result = Hikipuro.Text.Parser.Generator.GeneratorContext.Result;

namespace Hikipuro.Text.Parser.Generator.Expressions {
	/// <summary>
	/// グループの処理用.
	/// </summary>
	class GroupExpression : GeneratedExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(GeneratorContext context) {
			DebugLog("GroupExpression.Interpret(): " + Name + ", " + Expressions.Count);

			// 戻り値の準備
			Result result = new Result(Name);
			//Matches = new TokenMatches(Name);
			//IsMatch = false;

			foreach (GeneratedExpression exp in Expressions) {
				exp.Interpret(context);
				Result itemResult = context.PopResult();

				result.Matches.ConcatTokens(itemResult.Matches);
				if (itemResult.IsMatch == false) {
					if (exp.Type == ExpressionType.Option) {
						continue;
					}
					result.IsMatch = false;
					//throw new InterpreterException("FieldExpression.Interpret() Error");
					break;
				} else {
					result.IsMatch = true;
				}
			}

			context.PushResult(result);
		}
	}
}
