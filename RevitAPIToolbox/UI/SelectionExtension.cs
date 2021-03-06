﻿using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Techyard.Revit.Database;

namespace Techyard.Revit.UI
{
    public static class SelectionExtension
    {
        public static IEnumerable<Element> SelectByRectangle(this Selection selection,
            Func<Element, bool> filter = null)
        {
            return selection.PickElementsByRectangle(new SelectionFilter(filter));
        }

        public static IEnumerable<Element> SelectMany(this Selection selection, Document document,
            Func<Element, bool> filter = null)
        {
            return selection.PickObjects(ObjectType.Element | ObjectType.LinkedElement, new SelectionFilter(filter))
                .Select(reference => reference.ToElement(document));
        }

        public static Element SelectSingle(this Selection selection, Document document, Func<Element, bool> filter = null)
        {
            return selection.PickObject(ObjectType.Element | ObjectType.LinkedElement, new SelectionFilter(filter))
                .ToElement(document);
        }

        private class SelectionFilter : ISelectionFilter
        {
            private Func<Element, bool> ElementFilter { get; }
            private Func<Reference, XYZ, bool> ReferenceFilter { get; }

            internal SelectionFilter(Func<Element, bool> elementFilter,
                Func<Reference, XYZ, bool> referenceFilter = null)
            {
                ElementFilter = elementFilter;
                ReferenceFilter = referenceFilter;
            }

            public bool AllowElement(Element elem)
            {
                return null == ElementFilter || ElementFilter(elem);
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return null == ReferenceFilter || ReferenceFilter(reference, position);
            }
        }
    }
}
