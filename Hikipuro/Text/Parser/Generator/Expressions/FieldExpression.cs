using ExpressionType = Hikipuro.Text.Parser.Generator.GeneratedParser.ExpressionType;
using Result = Hikipuro.Text.Parser.Generator.GeneratorContext.Result;

namespace Hikipuro.Text.Parser.Generator.Expressions {
	/// <summary>
	/// フィールド.
	/// </summary>
	class FieldExpression : GeneratedExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(GeneratorContext context) {
			DebugLog("FieldExpression.Interpret(): " + Name);

			// 戻り値の準備
			Result result = new Result(Name);

			// 子要素を巡回する
			foreach (GeneratedExpression exp in Expressions) {
				DebugLog("-\tFieldExpression.Expressions : (" + exp.Name + ")");
				exp.Interpret(context);
				Result itemResult = context.PopResult();

				result.Matches.ConcatTokens(itemResult.Matches);
				if (itemResult.IsMatch == false) {
					DebugLog("-\texp.IsMatch == false : (" + exp.Name + ")");
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
			
			// マッチしたらイベントを発生させる
			if (result.IsMatch) {
				context.OnMatchField(Name, result.Matches);
			}

			context.PushResult(result);
		}
	}
}
