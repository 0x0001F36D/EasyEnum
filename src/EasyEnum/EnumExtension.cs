namespace EasyEnum
{
    // required C# 7.3

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;

    public static class EnumExtension
    {
        public static EnumInfo<TEnum> ToEnumInfo<TEnum>(this TEnum @enum) where TEnum : Enum
        {
            return new EnumInfo<TEnum>(@enum);
        }

        public sealed class EnumInfo<TEnum> where TEnum : Enum
        {
            private readonly TEnum _enum;
            private readonly Type _type;

            internal EnumInfo(TEnum @enum)
            {
                this._enum = @enum;
                this._type = @enum.GetType();
                this.Flags = this.HasFlags().ToArray();
            }

            public TEnum[] Flags { get; }
            private IEnumerable<TEnum> HasFlags()
            {
                var values = Enum.GetValues(this._type);
                foreach (TEnum item in values)
                {
                    if (this._enum.HasFlag(item))
                    {
                        yield return item;
                    }
                }
            }

            public IReadOnlyDictionary<TEnum, TAttribute> GetAttributes<TAttribute>(bool captureOnlyMatched = false)
                where TAttribute : Attribute
            {
                var dict = new Dictionary<TEnum, TAttribute>(this.Flags.Length);
                var pipe = captureOnlyMatched ? new Action<TEnum, TAttribute>(matchOnly) : all;

                foreach (var flag in this.Flags)
                {
                    var gca = this._type.GetField(flag.ToString()).GetCustomAttribute<TAttribute>();
                    pipe(flag, gca);
                }

                return new ReadOnlyDictionary<TEnum, TAttribute>(dict);

                void matchOnly(TEnum e, TAttribute a)
                {
                    if (a is TAttribute) dict.Add(e, a);
                }
                void all(TEnum e, TAttribute a)
                    => dict.Add(e, a);
            }

            public UEnum Cast<UEnum>() where UEnum : Enum
            {
                try
                {
                    return (UEnum)(ValueType)this._enum;
                }
                catch 
                {
                    throw new InvalidCastException();
                }
            }


        }


    }
}