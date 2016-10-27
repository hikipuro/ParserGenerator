﻿using Hikipuro.Text.Parser.EBNF.Generator;
using System.Diagnostics;

namespace Hikipuro.Text.Parser.EBNF.Expressions {
	class EBNFExpression : BaseExpression {
		/// <summary>
		/// 評価用メソッド.
		/// </summary>
		/// <param name="context">コンテキストオブジェクト.</param>
		public override void Interpret(EBNFContext context) {
			Debug.WriteLine(": EBNFExpression.Interpret()");

			// 戻り値の準備
			GeneratorRootExpression rootExp = new GeneratorRootExpression();
			rootExp.Type = GeneratorExpressionType.Root;
			rootExp.Name = "Root";

			// フィールドの繰り返し
			while (context.Current != null) {
				FieldExpression exp = new FieldExpression();
				exp.Interpret(context);

				// 名前が付いていない場合
				if (exp.Generator.Name == string.Empty) {
					ThrowInterpretException(
						ErrorMessages.NameNotFound, context.Current
					);
				}

				// リストに追加する
				rootExp.AddExpression(exp.Generator);
				context.Fields.Add(exp.Generator.Name, exp.Generator);
			}

			// 終端に文字が残っている場合
			if (context.Next() != null) {
				ThrowInterpretException(
					ErrorMessages.UnexpectedToken, context.Current
				);
			}

			// 戻り値
			context.Root = rootExp;
		}
	}
}
