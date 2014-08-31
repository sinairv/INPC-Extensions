using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace NotifyPropertyChangedExtensions
{
    public static class NotifyPropertyChangedExtensions
    {
        public static void HandleOnPropertyChanged<TNotifyPropertyChanged, TSourceProperty>(this TNotifyPropertyChanged source,
                Expression<Func<TNotifyPropertyChanged, TSourceProperty>> sourceProperty,
                Action<TNotifyPropertyChanged> eventHandler)

            where TNotifyPropertyChanged : INotifyPropertyChanged
        {
            if (sourceProperty == null)
                throw new ArgumentNullException("sourceProperty");
            if (eventHandler == null)
                throw new ArgumentNullException("eventHandler");

            var sourceMember = sourceProperty.Body as MemberExpression;
            if (sourceMember == null)
                throw new ArgumentException("Source property must be a member-access property");

            string sourcePropertyName = sourceMember.Member.Name;

            source.PropertyChanged += (sender, args) =>
            {
                if (String.Equals(args.PropertyName, sourcePropertyName, StringComparison.Ordinal))
                {
                    eventHandler(source);
                }
            };
        }

        public static void BindProperty<TNotifyPropertyChanged, TSourceProperty, TTarget, TTargetProperty>(this TNotifyPropertyChanged source,
            Expression<Func<TNotifyPropertyChanged, TSourceProperty>> sourceProperty,
            TTarget target,
            Expression<Func<TTarget, TTargetProperty>> targetProperty,
            BindPropertyOptions options = BindPropertyOptions.None)

            where TNotifyPropertyChanged : INotifyPropertyChanged
        {
            if (sourceProperty == null)
                throw new ArgumentNullException("sourceProperty");
            if (targetProperty == null)
                throw new ArgumentNullException("targetProperty");

            Type sourceBaseType = Nullable.GetUnderlyingType(typeof(TSourceProperty)) ?? typeof(TSourceProperty);
            bool isSourceNullable = sourceBaseType != typeof(TSourceProperty);

            Type targetBaseType = Nullable.GetUnderlyingType(typeof(TTargetProperty)) ?? typeof(TTargetProperty);
            bool isTargetNullable = targetBaseType != typeof(TTargetProperty);

            if (sourceBaseType != targetBaseType)
                throw new ArgumentException("source and target property types must be the same or nullable of each other", "sourceProperty");

            var sourceMember = sourceProperty.Body as MemberExpression;
            if (sourceMember == null)
                throw new ArgumentException("Source property must be a member-access property");

            var targetMember = targetProperty.Body as MemberExpression;
            MemberExpression negatedTargetMember = null;

            if (targetMember == null)
            {
                bool bindingAccepted = sourceBaseType == typeof(bool) && targetProperty.IsNotOfMemberAccess(out negatedTargetMember);
                if (!bindingAccepted)
                    throw new ArgumentException("The target property must be either a member-access or a negated member-access for boolean source types.", "targetProperty");
            }

            string sourcePropertyName = sourceMember.Member.Name;
            string targetPropertyName = targetMember != null ? targetMember.Member.Name : negatedTargetMember.Member.Name;

            var sourcePropertyInfo = source.GetType().GetProperty(sourcePropertyName, BindingFlags.Instance | BindingFlags.Public);
            var targetPropertyInfo = target.GetType().GetProperty(targetPropertyName, BindingFlags.Instance | BindingFlags.Public);
            bool dontUpdateTargetWhenSourceIsNullAndTargetIsNotNullable = options.HasFlag(BindPropertyOptions.DontUpdateWhenSourceIsNullAndTargetIsNotNullable);

            var updateAction = new Action<object>(sender =>
            {
                object sourceValue = sourcePropertyInfo.GetValue(source, null);

                object targetValue = null;
                bool setTarget;

                if (typeof(TSourceProperty) == typeof(TTargetProperty) || sourceValue != null)
                {
                    targetValue = sourceValue;
                    setTarget = true;
                }
                else // if (sourceValue == null)
                {
                    if (isTargetNullable)
                    {
                        targetValue = null;
                        setTarget = true;
                    }
                    else if (!dontUpdateTargetWhenSourceIsNullAndTargetIsNotNullable)
                    {
                        targetValue = default(TTargetProperty);
                        setTarget = true;
                    }
                    else
                    {
                        setTarget = false;
                    }
                }

                if (setTarget)
                {
                    if (negatedTargetMember != null)
                        targetValue = !(bool)(targetValue ?? false);

                    targetPropertyInfo.SetValue(target, targetValue, null);
                }
            });

            if (options.HasFlag(BindPropertyOptions.InitialiseTarget))
                updateAction(source);

            source.PropertyChanged += (sender, args) =>
            {
                if (String.Equals(args.PropertyName, sourcePropertyName, StringComparison.Ordinal))
                {
                    updateAction(sender);
                }
            };
        }

        public static void RaisePropertyChanged<TNotifyPropertyChanged, TProperty>(
            this TNotifyPropertyChanged self,
            Expression<Func<TProperty>> property)

            where TNotifyPropertyChanged : INotifyPropertyChanged
        {
            if (property == null)
                throw new ArgumentNullException("property");

            var sourceMember = property.Body as MemberExpression;
            if (sourceMember == null)
                throw new ArgumentException("Property must be a member-access property");

            string propertyName = sourceMember.Member.Name;

            var eventArgs = new PropertyChangedEventArgs(propertyName);

            Raise(self, "PropertyChanged", eventArgs);
        }

        internal static void Raise<TEventArgs>(this object source, string eventName, TEventArgs eventArgs) where TEventArgs : EventArgs
        {
            var eventInfo = source.GetType().GetField(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (eventInfo == null)
                return;

            var eventDelegate = eventInfo.GetValue(source) as MulticastDelegate;
            if (eventDelegate != null)
            {
                foreach (var handler in eventDelegate.GetInvocationList())
                {
                    handler.Method.Invoke(handler.Target, new object[] { source, eventArgs });
                }
            }
        }
    }
}
