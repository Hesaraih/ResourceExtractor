﻿// <copyright file="LuaAttribute.cs" company="Windower Team">
// Copyright © 2013-2017 Windower Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
// </copyright>

using System.Linq;

namespace ResourceExtractor.Serializers.Lua
{
    using System;
    using System.Collections;
    using System.Globalization;

    internal class LuaAttribute
    {
        public LuaAttribute(string key, object value)
        {
            Key = key;
            Value = value;
        }

        private string Key { get; }

        private object Value { get; }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}={1}", MakeKey(Key), MakeValue(Value));
        }

        private static string MakeKey(object key)
        {
            return string.Format(CultureInfo.InvariantCulture, key is string ? "{0}" : "[{0}]", key);
        }

        private static string MakeValue(object value)
        {
            if (value is string || value is Enum)
            {
                return "\"" + value.ToString().Replace("\"", "\\\"").Replace("\n", "\\n") + "\"";
            }

            var vdict = value as IDictionary;
            if (vdict != null)
            {
                var lines = vdict.Keys.Cast<object>().Select(key => $"{MakeKey(key)}={MakeValue(vdict[key])}").ToList();
                return $"{{{string.Join(",", lines)}}}";
            }

            var venum = value as IEnumerable;
            if (venum != null)
            {
                var lines = venum.Cast<object>().Select(val => $"{MakeKey(val)}={MakeValue(vdict[val])}").ToList();
                return $"{{{string.Join(",", lines)}}}";
            }

            return FormattableString.Invariant($"{value}");
        }
    }
}
