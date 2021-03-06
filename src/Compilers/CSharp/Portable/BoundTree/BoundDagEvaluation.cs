﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Roslyn.Utilities;

#nullable enable

namespace Microsoft.CodeAnalysis.CSharp
{
    partial class BoundDagEvaluation
    {
        public override bool Equals([NotNullWhen(true)] object? obj) => obj is BoundDagEvaluation other && this.Equals(other);
        public virtual bool Equals([NotNullWhen(true)] BoundDagEvaluation? other)
        {
            return !(other is null) &&
                this.Kind == other.Kind &&
                this.Input.Equals(other.Input) &&
                this.Symbol.Equals(other.Symbol, TypeCompareKind.AllIgnoreOptions);
        }
        private Symbol Symbol
        {
            get
            {
                switch (this)
                {
                    case BoundDagFieldEvaluation e: return e.Field.CorrespondingTupleField ?? e.Field;
                    case BoundDagPropertyEvaluation e: return e.Property;
                    case BoundDagTypeEvaluation e: return e.Type;
                    case BoundDagDeconstructEvaluation e: return e.DeconstructMethod;
                    case BoundDagIndexEvaluation e: return e.Property;
                    default: throw ExceptionUtilities.UnexpectedValue(this.Kind);
                }
            }
        }

        public override int GetHashCode()
        {
            return Hash.Combine(Input.GetHashCode(), this.Symbol?.GetHashCode() ?? 0);
        }

        public static bool operator ==(BoundDagEvaluation? left, BoundDagEvaluation? right)
        {
            return (left is null) ? right is null : left.Equals(right);
        }
        public static bool operator !=(BoundDagEvaluation? left, BoundDagEvaluation? right)
        {
            return !(left == right);
        }
    }

    partial class BoundDagIndexEvaluation
    {
        public override int GetHashCode() => base.GetHashCode() ^ this.Index;
        public override bool Equals(BoundDagEvaluation? obj)
        {
            return base.Equals(obj) &&
                // base.Equals checks the kind field, so the following cast is safe
                this.Index == ((BoundDagIndexEvaluation)obj).Index;
        }
    }
}
