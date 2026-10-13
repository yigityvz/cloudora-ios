using Cloudora.Editor;
using Cloudora.Services;
using Cloudora.UI;
using NUnit.Framework;
using UnityEngine;

namespace Cloudora.Tests.Editor
{
    public sealed class IosPortTests
    {
        [TestCase(null, "1")]
        [TestCase("2", "2")]
        public void BuildNumberHasPositiveDefault(string input, string expected)
            => Assert.That(CloudoraIosBuildTools.ReadBuildNumber(input), Is.EqualTo(expected));
        [TestCase("0")]
        [TestCase("-1")]
        [TestCase("abc")]
        public void InvalidBuildNumberFailsClosed(string input)
            => Assert.Throws<UnityEditor.Build.BuildFailedException>(() => CloudoraIosBuildTools.ReadBuildNumber(input));

        [TestCase(1179, 2556, 102, 177)]
        [TestCase(750, 1334, 0, 40)]
        public void PortraitSafeAreaAndBoardFit(float width, float height, float bottom, float top)
        {
            var (min, max) = SafeAreaFitter.CalculateAnchors(new Rect(0, bottom, width, height-bottom-top), new Vector2(width,height));
            Assert.That(min.y * height, Is.EqualTo(bottom).Within(0.01));
            Assert.That(max.y * height, Is.EqualTo(height-top).Within(0.01));
            // CanvasScaler MatchWidthOrHeight=0.5 from the gameplay scene.
            float scale = Mathf.Sqrt(width / 1080f * height / 1920f);
            Vector2 available = new Vector2(width * .9f, (height-bottom-top) * .62f) / scale;
            foreach (int count in new[] {5,8,10,12})
            {
                LayoutMetrics m = AdaptiveBoardLayout.Calculate(available, count, 7);
                int rows = Mathf.CeilToInt(count/(float)m.Columns);
                Assert.That(m.Columns*m.CellSize.x+(m.Columns-1)*m.Spacing.x, Is.LessThanOrEqualTo(available.x+.1f));
                Assert.That(rows*m.CellSize.y+(rows-1)*m.Spacing.y, Is.LessThanOrEqualTo(available.y+.1f));
            }
        }
        [Test] public void EditorHapticDoesNotLoadNativePlugin()
            => Assert.DoesNotThrow(() => new HapticFeedback().InvalidMove());
    }
}
