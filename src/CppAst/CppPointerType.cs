// Copyright (c) Alexandre Mutel. All rights reserved.
// Licensed under the BSD-Clause 2 license.
// See license.txt file in the project root for full license information.

using System;
using System.Text.RegularExpressions;

namespace CppAst
{
    /// <summary>
    /// A C++ pointer type (e.g `int*`)
    /// </summary>
    public sealed class CppPointerType : CppTypeWithElementType
    {
        /// <summary>
        /// Constructor of a pointer type.
        /// </summary>
        /// <param name="elementType">The element type pointed to.</param>
        public CppPointerType(CppType elementType) : base(CppTypeKind.Pointer, elementType)
        {
            SizeOf = IntPtr.Size;
            // Check if it is a function ptr
            Regex regex = new Regex(@"\(\*\w*\d*_*\)");
            IsFunctionPtr = regex.IsMatch(elementType.GetDisplayName());
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (IsFunctionPtr)
            {
				return $"{ElementType.GetDisplayName()}";
			}
			else
            {
				return $"{ElementType.GetDisplayName()}*";
			}
		}

        /// <inheritdoc />
        public override CppType GetCanonicalType()
        {
            var elementTypeCanonical = ElementType.GetCanonicalType();
            if (ReferenceEquals(elementTypeCanonical, ElementType)) return this;
            return new CppPointerType(elementTypeCanonical);
        }

        public bool IsFunctionPtr
        {
            get;
            private set;
        }
    }
}