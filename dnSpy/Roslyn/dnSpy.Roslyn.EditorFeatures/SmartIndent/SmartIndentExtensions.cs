// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using dnSpy.Roslyn.EditorFeatures.Extensions;
using dnSpy.Roslyn.Internal.SmartIndent;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace dnSpy.Roslyn.EditorFeatures.SmartIndent
{
    internal static class SmartIndentExtensions
    {
        public static int GetIndentation(this IndentationResult result, ITextView textView, ITextSnapshotLine lineToBeIndented)
        {
            var position = new SnapshotPoint(lineToBeIndented.Snapshot, result.BasePosition);
            var pointInSurfaceSnapshot = textView.BufferGraph.MapUpToSnapshot(position, PointTrackingMode.Positive, PositionAffinity.Successor, textView.TextSnapshot);
            if (!pointInSurfaceSnapshot.HasValue)
            {
                return position.GetContainingLine().GetColumnOfFirstNonWhitespaceCharacterOrEndOfLine(textView.Options);
            }

            var lineInSurfaceSnapshot = pointInSurfaceSnapshot.Value.Snapshot.GetLineFromPosition(pointInSurfaceSnapshot.Value.Position);
            var offsetInLine = pointInSurfaceSnapshot.Value.Position - lineInSurfaceSnapshot.Start.Position;
            return lineInSurfaceSnapshot.GetColumnFromLineOffset(offsetInLine, textView.Options) + result.Offset;
        }
    }
}
