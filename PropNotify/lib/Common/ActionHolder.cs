using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace lib.Common
{
    public class ActionHolder<T>
    {
        public ActionHolder(Action<T, string> action, Expression<Func<T, bool>> expressionBool)
        {
            Action = action;
            ExpressionBool = expressionBool;
            ConditionExp = true;
        }
        public ActionHolder(Action<T, string> action, Expression<Func<T, object>> expressionProperty)
        {
            Action = action;
            ExpressionProperty = expressionProperty;
            ConditionExp = false;
        }

        public Action<T, string> Action { get; set; }
        public Expression<Func<T, bool>> ExpressionBool { get; set; }
        public Expression<Func<T, object>> ExpressionProperty { get; set; }
        public bool ConditionExp { get; set; }
    }

    public static class PropActionExtension
    {
        public static void AddCondition<T>(this List<ActionHolder<T>> actionHolder, Action<T, string> action, Expression<Func<T, bool>> expressionBool)
        {
            actionHolder.Add(new ActionHolder<T>(action, expressionBool));
        }
        public static void AddProperty<T>(this List<ActionHolder<T>> actionHolder, Action<T, string> action, Expression<Func<T, object>> expressionProperty)
        {
            actionHolder.Add(new ActionHolder<T>(action, expressionProperty));
        }
    }
}
