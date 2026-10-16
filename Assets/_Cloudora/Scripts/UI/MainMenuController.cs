using System;
using Cloudora.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    public sealed class MainMenuController : MonoBehaviour
    {
        private GameObject _menu;
        private GameObject _settings;
        private Text _progressLabel;

        public void Show() => _menu.SetActive(true);
        public void SetProgress(string world, int level) => _progressLabel.text = $"{world.Replace('-', ' ')}  •  Level {level}".ToUpperInvariant();

        public static MainMenuController Create(Canvas canvas, string world, int level, Action onWorld, Func<bool> sound, Action<bool> setSound, Func<bool> haptics, Action<bool> setHaptics)
        {
            CloudoraVisuals.AddSkyGradient(canvas);
            var host = new GameObject("MainMenuController", typeof(RectTransform));
            host.transform.SetParent(canvas.transform, false);
            SafeAreaFitter.Attach(host);
            var controller = host.AddComponent<MainMenuController>();
            controller.Build(world, level, onWorld, sound, setSound, haptics, setHaptics);
            return controller;
        }

        private void Build(string world, int level, Action onWorld, Func<bool> sound, Action<bool> setSound, Func<bool> haptics, Action<bool> setHaptics)
        {
            _menu = Panel(transform, "MainMenu", new Color(0.91f, 0.97f, 1f, 0.98f), out Transform menuCard);
            Label(menuCard, "CLOUDORA", 76, new Vector2(0.1f, 0.76f), new Vector2(0.9f, 0.9f));
            Label(menuCard, "Fix the sky. Restore the world.", 30, new Vector2(0.1f, 0.68f), new Vector2(0.9f, 0.77f));
            _progressLabel = Label(menuCard, string.Empty, 34, new Vector2(0.12f, 0.5f), new Vector2(0.88f, 0.62f));
            SetProgress(world, level);
            Button(menuCard, "CONTINUE", new Vector2(0f, -80f), () => _menu.SetActive(false));
            Button(menuCard, "WORLD", new Vector2(0f, -190f), () => { _menu.SetActive(false); onWorld?.Invoke(); });
            Button(menuCard, "SETTINGS", new Vector2(0f, -300f), () => _settings.SetActive(true));
            _settings = Panel(transform, "Settings", new Color(0.96f, 0.98f, 1f, 0.99f), out Transform settingsCard);
            Label(settingsCard, "SETTINGS", 58, new Vector2(0.1f, 0.72f), new Vector2(0.9f, 0.88f));
            Button soundButton = Button(settingsCard, string.Empty, new Vector2(0f, 100f), null);
            soundButton.onClick.AddListener(() => { setSound(!sound()); SetToggleLabel(soundButton, "SOUND", sound()); });
            SetToggleLabel(soundButton, "SOUND", sound());
            Button hapticsButton = Button(settingsCard, string.Empty, new Vector2(0f, -10f), null);
            hapticsButton.onClick.AddListener(() => { setHaptics(!haptics()); SetToggleLabel(hapticsButton, "HAPTICS", haptics()); });
            SetToggleLabel(hapticsButton, "HAPTICS", haptics());
            if (ReleaseConfiguration.HasPrivacyPolicy)
                Button(settingsCard, "PRIVACY", new Vector2(0f, -120f), () => Application.OpenURL(ReleaseConfiguration.Current.privacyPolicyUrl));
            if (ReleaseConfiguration.HasSupportEmail)
                Button(settingsCard, "SUPPORT", new Vector2(0f, -230f), () => Application.OpenURL("mailto:" + ReleaseConfiguration.Current.supportEmail));
            Button(settingsCard, "CLOSE", new Vector2(0f, -350f), () => _settings.SetActive(false));
            _settings.SetActive(false);
        }

        private static void SetToggleLabel(Button button, string label, bool enabled)
        {
            button.GetComponentInChildren<Text>().text = $"{label}: {(enabled ? "ON" : "OFF")}";
        }

        private static GameObject Panel(Transform parent, string name, Color color, out Transform content)
        {
            // Tam ekran opak gökyüzü: panel açıkken arkadaki oyun görünmez ve dokunulamaz.
            GameObject go = CloudoraVisuals.CreateBackdrop(parent, name);
            var card = new GameObject("Card", typeof(RectTransform), typeof(Image));
            card.transform.SetParent(go.transform, false);
            RectTransform rect = card.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.08f, 0.12f); rect.anchorMax = new Vector2(0.92f, 0.9f); rect.offsetMin = rect.offsetMax = Vector2.zero;
            Image image = card.GetComponent<Image>(); image.sprite = CloudoraVisuals.RoundedPanel; image.type = Image.Type.Sliced; image.color = color;
            Shadow shadow = card.AddComponent<Shadow>(); shadow.effectColor = new Color(0.08f, 0.18f, 0.35f, 0.25f); shadow.effectDistance = new Vector2(0f, -10f);
            content = card.transform;
            return go;
        }

        private static Button Button(Transform parent, string text, Vector2 position, Action action)
        {
            var go = new GameObject(text, typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            RectTransform rect = go.GetComponent<RectTransform>(); rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f); rect.anchoredPosition = position; rect.sizeDelta = new Vector2(500f, 94f);
            Image image = go.GetComponent<Image>(); image.sprite = CloudoraVisuals.RoundedPanel; image.type = Image.Type.Sliced; image.color = new Color(0.36f, 0.49f, 0.98f);
            go.GetComponent<Button>().onClick.AddListener(() => action?.Invoke());
            Label(go.transform, text, 32, Vector2.zero, Vector2.one).color = Color.white;
            return go.GetComponent<Button>();
        }

        private static Text Label(Transform parent, string value, int size, Vector2 min, Vector2 max)
        {
            var go = new GameObject("Label", typeof(RectTransform), typeof(Text)); go.transform.SetParent(parent, false);
            Text text = go.GetComponent<Text>(); text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); text.fontSize = size; text.text = value.ToUpperInvariant(); text.alignment = TextAnchor.MiddleCenter; text.color = new Color(0.15f, 0.22f, 0.29f); text.raycastTarget = false;
            text.rectTransform.anchorMin = min; text.rectTransform.anchorMax = max; text.rectTransform.offsetMin = text.rectTransform.offsetMax = Vector2.zero;
            return text;
        }
    }
}
