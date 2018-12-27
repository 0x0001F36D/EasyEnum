namespace EasyEnum.Dynamic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;

    public sealed class Enum : DynamicObject, IEnumerable<string>, IEnumerable<(string, int)>
    {
        public Enum(string name) : this(name, ",")
        {

        }

        public Enum(string name, string seperator)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrEmpty(seperator))
                throw new ArgumentNullException(nameof(seperator));

            this.Name = name;
            this.Seperator = seperator;

            this._fields = new Dictionary<string, Field>();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = "";
            if (this._fields.TryGetValue(binder.Name, out var r))
            {
                result = r;
            }
            return true;
        }

        public string Name { get; }
        public string Seperator { get; }

        private readonly Dictionary<string, Field> _fields;

        public void Add(string enumField)
        {
            var id = this._fields.Count;
            if (id > 0)
            {
                var last = this._fields.LastOrDefault();
                id = last.Value.Id + 1;
            }
            this.Add((enumField, id));
        }
        public void Add(string enumField, int id)
        {
            this.Add((enumField, id));
        }
        public void Add((string enumField, int id) field)
        {
            this._fields.Add(field.enumField, new Field(field.enumField, field.id, this));
        }

        public Field[] GetValues() => this._fields.Values.ToArray();

        public bool Remove(string enumField)
        {
            return this._fields.Remove(enumField);
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator() => this._fields.Keys.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<(string, int)>)this).GetEnumerator();
        IEnumerator<(string, int)> IEnumerable<(string, int)>.GetEnumerator()
        {
            foreach (var f in this._fields)
            {
                yield return (f.Key, f.Value.Id);
            }
        }


        public sealed class Field : IEquatable<Field>
        {
            internal Field(string name, int id, Enum group)
            {
                this.Id = id;
                this._name = name;
                this._group = group;
                this._hc = name.Split(',').Aggregate(0, (s, x) => s += x.GetHashCode());

            }
            internal int Id { get; }
            private readonly string _name;
            private readonly Enum _group;
            private readonly int _hc;

            public static Field operator |(Field left, Field right)
            {
                if (left is Field lf)
                {
                    if (right is Field rf)
                    {
                        if (!lf._group.Equals(rf._group))
                            throw new NotSupportedException("不同群組不可相加");

                        var list = new HashSet<string>(left._name.Split(new string[1] { left._group.Seperator }, StringSplitOptions.RemoveEmptyEntries));
                        var lc = list.Count;
                        foreach (var ri in right._name.Split(new string[1] { left._group.Seperator }, StringSplitOptions.RemoveEmptyEntries))
                            list.Add(ri);
                        if (lc != list.Count)
                        {
                            var s = new Field(string.Join(left._group.Seperator, list.OrderBy(x => x)), left.Id | right.Id, left._group);
                            return s;
                        }
                    }

                    return left;
                }

                return right;
            }


            public override int GetHashCode() => this._hc;

            public override bool Equals(object obj) => this.Equals(obj as Field);


            public override string ToString() => this._name.ToString();
            public bool Equals(Field other)
            {
                if (other is null)
                    return false;

                return this.GetHashCode() == other.GetHashCode();
            }

            public static explicit operator int(Field field)
            {
                return field.Id;
            }
        }


    }
}