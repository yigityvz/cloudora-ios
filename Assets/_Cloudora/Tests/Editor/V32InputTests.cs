using System.Text.RegularExpressions;
using Cloudora.Services;
using Cloudora.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Cloudora.Tests.Editor
{
    // v3.2: Editor'da tek basışın birden çok işlem yapıp yapmadığını görmek için kanıt (2026-10-15, L30'da 21 Undo).
    public sealed class V32InputTests
    {
        [Test]
        public void AnalyticsLinesCarryFrameAndTime()
        {
            // Aynı karede iki olay görünür olsun: "[Analytics] frame=<kare> t=<sn> <olay> ..."
            LogAssert.Expect(LogType.Log, new Regex(@"^\[Analytics\] frame=\d+ t=\d+\.\d{3} test_event level=7$"));
            new DebugAnalyticsService().Track("test_event", new System.Collections.Generic.Dictionary<string, object> { ["level"] = 7 });
        }

        [Test]
        public void InfoPopupLocksBottomButtonsWithoutCoveringThem()
        {
            // Tasarım §11g: bilgi penceresi açıkken alt butonlar görünür kalır ama çalışmaz; basış yalnızca pencereyi kapatır.
            var canvasObject = new GameObject("Canvas", typeof(Canvas));
            try
            {
                GameplayOverlay overlay = GameplayOverlay.Create(canvasObject.GetComponent<Canvas>(),
                    () => { }, () => { }, () => { }, () => { }, () => { }, () => { }, () => { }, () => { }, () => { }, () => { });
                Transform panel = overlay.transform.Find("BlockingPanel");
                var panelRect = (RectTransform)panel;
                Transform shield = panel.Find("ButtonShield");
                Assert.That(shield, Is.Not.Null, "popup must carry a shield over the bottom buttons");
                var rect = (RectTransform)shield;

                // Karartma butonların üstünde biter (butonları örtmez); kalkan oradan ekranın altına kadar iner.
                Assert.That(panelRect.offsetMin.y, Is.GreaterThan(0f));
                Assert.That(rect.anchorMin, Is.EqualTo(Vector2.zero));
                Assert.That(rect.anchorMax, Is.EqualTo(new Vector2(1f, 0f)));
                Assert.That(rect.offsetMin.y, Is.EqualTo(-panelRect.offsetMin.y).Within(0.01f));
                Assert.That(rect.offsetMax.y, Is.EqualTo(0f).Within(0.01f));

                // Görünmez ama dokunuşu yakalar; butonlardan sonra çizilir (üstte).
                Image image = shield.GetComponent<Image>();
                Assert.That(image.raycastTarget, Is.True);
                Assert.That(image.color.a, Is.EqualTo(0f));
                foreach (string name in new[] { "ExtraButton", "UndoButton", "ShuffleButton", "WorldButton", "MenuButton" })
                    Assert.That(panel.GetSiblingIndex(), Is.GreaterThan(overlay.transform.Find(name).GetSiblingIndex()), name);

                overlay.ShowBlock("Undo Refilled", "Reward granted +3. Tap Undo again.");
                Assert.That(shield.gameObject.activeInHierarchy, Is.True);
                shield.GetComponent<Button>().onClick.Invoke();
                Assert.That(panel.gameObject.activeSelf, Is.False, "tapping the shield only closes the popup");
            }
            finally
            {
                Object.DestroyImmediate(canvasObject);
            }
        }

        [Test]
        public void BottomButtonClicksAreLoggedWithFrame()
        {
            var canvasObject = new GameObject("Canvas", typeof(Canvas));
            try
            {
                GameplayOverlay overlay = GameplayOverlay.Create(canvasObject.GetComponent<Canvas>(),
                    () => { }, () => { }, () => { }, () => { }, () => { }, () => { }, () => { }, () => { }, () => { }, () => { });
                Button undo = overlay.transform.Find("UndoButton").GetComponent<Button>();
                LogAssert.Expect(LogType.Log, new Regex(@"^\[Input\] button=Undo frame=\d+ t=\d+\.\d{3}$"));
                undo.onClick.Invoke();
            }
            finally
            {
                Object.DestroyImmediate(canvasObject);
            }
        }
    }
}
