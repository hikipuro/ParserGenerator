﻿using Hikipuro.Text.Parser.Generator;
using Hikipuro.Text.Parser.Generator.Expressions;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	/// <summary>
	/// EBNF のパース用.
	/// </summary>
	class EBNFExpression : BaseExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			DebugLog(": EBNFExpression.Interpret()");

			// 戻り値の準備
			GeneratedExpression root = ExpressionFactory.CreateRoot();

			// フィールドの繰り返し
			while (context.Current != null) {
				FieldExpression exp = new FieldExpression();
				exp.Interpret(context);

				// 名前が付いていない場合はエラー
				if (exp.GeneratedExpression.Name == string.Empty) {
					ThrowParseException(
						ErrorMessages.NameNotFound, context.Current
					);
				}

				// リストに追加する
				root.AddExpression(exp.GeneratedExpression);
				context.Fields.Add(exp.GeneratedExpression.Name, exp.GeneratedExpression);
			}

			// 終端に文字が残っている場合
			if (context.Next() != null) {
				ThrowParseException(
					ErrorMessages.UnexpectedToken, context.Current
				);
			}

			// 戻り値
			context.Root = root;
		}
	}
}
