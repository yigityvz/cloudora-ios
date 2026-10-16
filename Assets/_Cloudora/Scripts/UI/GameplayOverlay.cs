using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cloudora.UI
{
    // "No Moves" (bölüm başarısız) ekranında gösterilecek seçenekler.
    public readonly struct FailOptions : IEquatable<FailOptions>
    {
        public readonly bool RetryAvailable;  // can var (ya da 1–5'te ücretsiz)
        public readonly bool RetryFree;       // 1–5: can götürmez
        public readonly bool AdAvailable;
        public readonly int CoinCost;
        public readonly bool CoinsAffordable;

        public FailOptions(bool retryAvailable, bool retryFree, bool adAvailable, int coinCost, bool coinsAffordable)
        {
            RetryAvailable = retryAvailable;
            RetryFree = retryFree;
            AdAvailable = adAvailable;
            CoinCost = coinCost;
            CoinsAffordable = coinsAffordable;
        }

        public bool Equals(FailOptions o) => RetryAvailable == o.RetryAvailable && RetryFree == o.RetryFree &&
                                             AdAvailable == o.AdAvailable && CoinCost == o.CoinCost && CoinsAffordable == o.CoinsAffordable;
        public override bool Equals(object obj) => obj is FailOptions other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(RetryAvailable, RetryFree, AdAvailable, CoinCost, CoinsAffordable);
    }

    public sealed class GameplayOverlay : MonoBehaviour
    {
        private Button _undoButton;
        private Button _shuffleButton;
        private Button _extraButton;
        private GameObject _completePanel;
        private Text _completeReward;
        private Text _levelLabel;
        private Text _tutorialLabel;
        private Text _modeLabel;
        private Text _livesLabel;
        private Text _coinsLabel;
        private Text _adHintLabel;
        private Text _undoBadge;
        private Text _extraBadge;
        private Text _shuffleBadge;
        private GameObject _blockingPanel;
        private GameObject _failPanel;
        private Button _retryButton;
        private Button _adButton;
        private Button _coinsButton;
        private FailOptions? _failOptions;
        private float _pulse;

        private void Update()
        {
            if (_tutorialLabel == null || string.IsNullOrEmpty(_tutorialLabel.text)) return;
            _pulse += Time.unscaledDeltaTime * 4f;
            _tutorialLabel.transform.localScale = Vector3.one * (1f + Mathf.Sin(_pulse) * 0.035f);
        }

        public static GameplayOverlay Create(Canvas canvas, Action onUndo, Action onContinue, Action onWorld,
            Action onExtraCloud, Action onSafeShuffle, Action onMenu, Action onRetry, Action onContinueWithAd, Action onContinueWithCoins,
            Action onLifeAd)
        {
            var root = new GameObject("GameplayOverlay", typeof(RectTransform));
            root.transform.SetParent(canvas.transform, false);
            RectTransform rootRect = root.GetComponent<RectTransform>();
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = rootRect.offsetMax = Vector2.zero;
            SafeAreaFitter.Attach(root);

            var overlay = root.AddComponent<GameplayOverlay>();
            overlay._levelLabel = CreateTopLabel(root.transform, "Level 1", new Vector2(0f, -70f), 42);
            overlay._livesLabel = CreateLivesLabel(root.transform);
            overlay._coinsLabel = CreateCoinsLabel(root.transform);
            // Sadece bir rozet AD durumundayken görünür; AD'nin anlamını açıklar.
            overlay._adHintLabel = CreateTopLabel(root.transform, "<color=#F29A2E><b>AD</b></color>  Watch an ad to refill", new Vector2(0f, -125f), 26);
            overlay._adHintLabel.gameObject.SetActive(false);
            overlay._tutorialLabel = CreateTopLabel(root.transform, string.Empty, new Vector2(0f, -180f), 30);
            // Shuffle'da bulut seçme yönergesi (yalnızca 16+; öğretici yazılar 1–15'te olduğu için çakışmaz).
            overlay._modeLabel = CreateTopLabel(root.transform, string.Empty, new Vector2(0f, -180f), 30);
            overlay._modeLabel.fontStyle = FontStyle.Bold;
            overlay._modeLabel.color = new Color(0.85f, 0.45f, 0.10f);

            // Alt butonlar: üst sıra Extra · Undo · Shuffle; alt sıra World ve Menu, üst sıradakilerin tam aralarında.
            overlay._extraButton = CreateButton(root.transform, "Extra", GridPosition(0, 1), onExtraCloud);
            overlay._undoButton = CreateButton(root.transform, "Undo", GridPosition(1, 1), onUndo);
            overlay._shuffleButton = CreateButton(root.transform, "Shuffle", GridPosition(2, 1), onSafeShuffle);
            CreateButton(root.transform, "World", BetweenColumns(0, 1), onWorld, SecondaryButtonColor);
            CreateButton(root.transform, "Menu", BetweenColumns(1, 2), onMenu, SecondaryButtonColor);
            overlay._undoBadge = CreateBadge(overlay._undoButton.transform);
            overlay._extraBadge = CreateBadge(overlay._extraButton.transform);
            overlay._shuffleBadge = CreateBadge(overlay._shuffleButton.transform);
            overlay._completePanel = CreateCompletePanel(root.transform, onContinue, out overlay._completeReward);
            overlay._blockingPanel = CreateBlockingPanel(root.transform);
            overlay._failPanel = CreateFailPanel(root.transform, onRetry, onContinueWithAd, onContinueWithCoins,
                out overlay._retryButton, out overlay._adButton, out overlay._coinsButton);
            overlay._completePanel.SetActive(false);
            overlay._blockingPanel.SetActive(false);
            overlay._failPanel.SetActive(false);
            // En son: başarısızlık ekranı ve mesajlar açıkken de basılabilsin (ör. canı bitmiş oyuncu).
            CreateLifeAdButton(root.transform, onLifeAd);
            return overlay;
        }

        public void SetUndoAvailable(bool available) => _undoButton.interactable = available;
        public void SetShuffleAvailable(bool available) => _shuffleButton.interactable = available;

        // Limit dolunca soluk görünür ama basılabilir kalır (basınca "Sky Is Full" mesajı çıkar).
        public void SetExtraAvailable(bool available)
        {
            if (!_extraButton.TryGetComponent(out CanvasGroup group)) group = _extraButton.gameObject.AddComponent<CanvasGroup>();
            group.alpha = available ? 1f : 0.45f;
        }

        public void ShowComplete(bool show, int coinsEarned = 0)
        {
            _completePanel.SetActive(show);
            _completeReward.text = show && coinsEarned > 0 ? $"+{coinsEarned} coins" : string.Empty;
        }

        public void SetLevelInfo(int level, string world) => _levelLabel.text = $"{ToTitle(world)}  •  Level {level}";
        public void SetTutorialCue(string cue) => _tutorialLabel.text = string.IsNullOrWhiteSpace(cue) ? string.Empty : cue;
        public void SetModeHint(string hint) => _modeLabel.text = string.IsNullOrWhiteSpace(hint) ? string.Empty : hint;
        public void SetLives(int lives, string countdown) => _livesLabel.text = lives >= 5 ? "5" : $"{lives}  {countdown}";
        public void SetCoins(int coins) => _coinsLabel.text = coins.ToString();

        // Köşe rozetleri: eldeki ücretsiz hak; hak 0 ve reklam varsa turuncu "AD".
        public void SetBoosters(int undo, int extra, int shuffle, bool adAvailable)
        {
            bool anyAd = SetChargeBadge(_undoBadge, undo, adAvailable);
            anyAd |= SetChargeBadge(_extraBadge, extra, adAvailable);
            anyAd |= SetChargeBadge(_shuffleBadge, shuffle, adAvailable);
            _adHintLabel.gameObject.SetActive(anyAd);
        }

        // "No Moves" = bölüm başarısız. Tam ekran; alttaki Undo/Extra/Shuffle bu ekranda kullanılamaz.
        public void ShowFail(FailOptions options)
        {
            _failPanel.SetActive(true);
            if (_failOptions.HasValue && _failOptions.Value.Equals(options)) return;
            _failOptions = options;
            _retryButton.interactable = options.RetryAvailable;
            _retryButton.GetComponentInChildren<Text>().text = options.RetryFree ? "Retry (free)" : "Retry  (-1 life)";
            _adButton.interactable = options.AdAvailable;
            _coinsButton.interactable = options.CoinsAffordable;
            _coinsButton.GetComponentInChildren<Text>().text = $"Continue  ({options.CoinCost} coins)";
            Text[] labels = _failPanel.transform.Find("Card").GetComponentsInChildren<Text>();
            labels[1].text = options.RetryAvailable
                ? "Start over, or continue from your last solvable move."
                : "Out of lives. Continue from your last solvable move, or wait for a life.";
        }

        public void HideFail()
        {
            _failPanel.SetActive(false);
            _failOptions = null;
        }

        private static bool SetChargeBadge(Text badge, int charges, bool adAvailable)
        {
            bool ad = charges <= 0 && adAvailable;
            SetBadge(badge, ad ? "AD" : charges.ToString(), ad ? AdBadgeColor : Color.white);
            return ad;
        }

        private static void SetBadge(Text badge, string value, Color background)
        {
            GameObject go = badge.transform.parent.gameObject;
            go.SetActive(!string.IsNullOrEmpty(value));
            if (string.IsNullOrEmpty(value)) return;
            badge.text = value;
            go.GetComponent<Image>().color = background;
            badge.color = background == Color.white ? new Color(0.11f, 0.16f, 0.21f) : Color.white;
            ((RectTransform)go.transform).sizeDelta = new Vector2(Mathf.Max(BadgeHeight, badge.preferredWidth + 22f), BadgeHeight);
        }

        // Sağ üstte can: kalp ikonu + sayı / geri sayım.
        private static Text CreateLivesLabel(Transform parent)
        {
            Text text = CreateTopLabel(parent, "5", new Vector2(420f, -70f), 34);
            text.alignment = TextAnchor.MiddleLeft;
            text.rectTransform.sizeDelta = new Vector2(240f, 60f);
            Image heart = CreateIcon(text.transform, CloudoraVisuals.Heart, 34f);
            heart.color = new Color(0.88f, 0.32f, 0.42f);
            return text;
        }

        // Sol üstte coin: altın daire ikonu + sayı.
        private static Text CreateCoinsLabel(Transform parent)
        {
            Text text = CreateTopLabel(parent, "0", new Vector2(-340f, -70f), 34);
            text.alignment = TextAnchor.MiddleLeft;
            text.rectTransform.sizeDelta = new Vector2(240f, 60f);
            Image coin = CreateIcon(text.transform, CloudoraVisuals.Pill, 30f);
            coin.color = CoinColor;
            text.gameObject.name = "CoinsLabel";
            return text;
        }

        // Geliştirme için geçici: can göstergesinin altında "reklam izle → +1 can" (ileride yeri/şekli değişecek).
        private static void CreateLifeAdButton(Transform parent, Action onLifeAd)
        {
            var go = new GameObject("LifeAdButton", typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 1f);
            rect.anchoredPosition = new Vector2(430f, -122f);
            rect.sizeDelta = new Vector2(150f, 46f);
            Image image = go.GetComponent<Image>();
            image.sprite = CloudoraVisuals.Pill;
            image.type = Image.Type.Sliced;
            image.pixelsPerUnitMultiplier = CloudoraVisuals.PillRadius * 2f / 46f;
            image.color = AdBadgeColor;
            go.GetComponent<Button>().onClick.AddListener(() => onLifeAd?.Invoke());
            Image heart = CreateIcon(go.transform, CloudoraVisuals.Heart, 26f);
            heart.rectTransform.anchoredPosition = new Vector2(26f, 0f);
            heart.color = Color.white;
            Text text = CreateText(go.transform, "AD +1", 24, TextAnchor.MiddleCenter);
            text.fontStyle = FontStyle.Bold;
            text.rectTransform.offsetMin = new Vector2(34f, 0f);
        }

        private static Image CreateIcon(Transform parent, Sprite sprite, float size)
        {
            var go = new GameObject("Icon", typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            Image image = go.GetComponent<Image>();
            image.sprite = sprite;
            image.preserveAspect = true;
            image.raycastTarget = false;
            RectTransform rect = image.rectTransform;
            rect.sizeDelta = new Vector2(size, size);
            rect.anchorMin = rect.anchorMax = new Vector2(0f, 0.5f);
            rect.anchoredPosition = new Vector2(-24f, 0f);
            return image;
        }

        public void ShowBlock(string title, string detail)
        {
            _blockingPanel.SetActive(true);
            Text[] labels = _blockingPanel.GetComponentsInChildren<Text>();
            labels[0].text = title;
            labels[1].text = detail;
        }
        public void HideBlock() => _blockingPanel.SetActive(false);

        private static Text CreateTopLabel(Transform parent, string value, Vector2 position, int size)
        {
            Text text = CreateText(parent, value, size, TextAnchor.MiddleCenter);
            RectTransform rect = text.rectTransform;
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 1f);
            rect.sizeDelta = new Vector2(880f, 60f);
            rect.anchoredPosition = position;
            text.color = new Color(0.15f, 0.22f, 0.29f);
            return text;
        }

        private static string ToTitle(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? "Cloudora"
                : System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value.Replace('-', ' '));
        }

        private static readonly Color AdBadgeColor = new Color(0.95f, 0.60f, 0.18f);
        private static readonly Color CoinColor = new Color(0.96f, 0.76f, 0.22f);
        private const float BadgeHeight = 38f;

        // Butonun sağ üst köşesine taşan küçük hap şeklinde rozet.
        private static Text CreateBadge(Transform button)
        {
            var go = new GameObject("Badge", typeof(RectTransform), typeof(Image));
            go.transform.SetParent(button, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(-10f, -2f);
            rect.sizeDelta = new Vector2(BadgeHeight, BadgeHeight);
            Image image = go.GetComponent<Image>();
            image.sprite = CloudoraVisuals.Pill;
            image.type = Image.Type.Sliced;
            image.pixelsPerUnitMultiplier = CloudoraVisuals.PillRadius * 2f / BadgeHeight;
            image.raycastTarget = false;
            Shadow shadow = go.AddComponent<Shadow>();
            shadow.effectColor = new Color(0.05f, 0.10f, 0.25f, 0.30f);
            shadow.effectDistance = new Vector2(0f, -2f);
            Text text = CreateText(go.transform, string.Empty, 24, TextAnchor.MiddleCenter);
            text.fontStyle = FontStyle.Bold;
            go.SetActive(false);
            return text;
        }

        private static readonly Color PrimaryButtonColor = new Color(0.27f, 0.40f, 0.92f);
        private static readonly Color SecondaryButtonColor = new Color(0.20f, 0.56f, 0.72f);
        private static readonly Vector2 ButtonSize = new Vector2(300f, 84f);
        private const float ButtonGap = 36f;
        private const float ButtonBottom = 28f;
        private const float RowGap = 20f;

        private static Vector2 GridPosition(int column, int row)
            => new Vector2((column - 1) * (ButtonSize.x + ButtonGap), ButtonBottom + row * (ButtonSize.y + RowGap));

        // Alt sıra: iki sütunun tam ortası, sıra 0.
        private static Vector2 BetweenColumns(int left, int right)
            => new Vector2((GridPosition(left, 0).x + GridPosition(right, 0).x) / 2f, ButtonBottom);

        private static Button CreateButton(Transform parent, string label, Vector2 position, Action action)
            => CreateButton(parent, label, position, action, PrimaryButtonColor);

        private static Button CreateButton(Transform parent, string label, Vector2 position, Action action, Color color)
        {
            var go = new GameObject(label + "Button", typeof(RectTransform), typeof(Image), typeof(Button));
            go.transform.SetParent(parent, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0f);
            rect.pivot = new Vector2(0.5f, 0f);
            rect.anchoredPosition = position;
            rect.sizeDelta = ButtonSize;
            go.GetComponent<Image>().color = color;
            Shadow shadow = go.AddComponent<Shadow>();
            shadow.effectColor = new Color(0.08f, 0.15f, 0.35f, 0.30f);
            shadow.effectDistance = new Vector2(0f, -5f);
            Button button = go.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                // Geliştirme derlemelerinde her basış kare/zamanla yazılır (tek basışta çift işlem kontrolü için).
                if (Debug.isDebugBuild)
                    Debug.Log($"[Input] button={label} frame={Time.frameCount} t={Time.realtimeSinceStartup.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture)}");
                action?.Invoke();
            });
            CreateText(go.transform, label, 34, TextAnchor.MiddleCenter);
            return button;
        }

        private static GameObject CreateCompletePanel(Transform parent, Action onContinue, out Text reward)
        {
            GameObject root = CreateDimmedCard(parent, "LevelCompletePanel", new Vector2(760f, 480f), 0f, out GameObject panel);

            Text title = CreateText(panel.transform, "Sky Balanced!", 58, TextAnchor.MiddleCenter);
            title.color = new Color(0.15f, 0.22f, 0.29f);
            title.rectTransform.anchorMin = new Vector2(0.1f, 0.62f);
            title.rectTransform.anchorMax = new Vector2(0.9f, 0.92f);
            title.rectTransform.offsetMin = title.rectTransform.offsetMax = Vector2.zero;
            reward = CreateText(panel.transform, string.Empty, 36, TextAnchor.MiddleCenter);
            reward.fontStyle = FontStyle.Bold;
            reward.color = new Color(0.80f, 0.55f, 0.05f);
            reward.rectTransform.anchorMin = new Vector2(0.1f, 0.40f);
            reward.rectTransform.anchorMax = new Vector2(0.9f, 0.60f);
            reward.rectTransform.offsetMin = reward.rectTransform.offsetMax = Vector2.zero;
            CreateButton(panel.transform, "Continue", new Vector2(0f, 58f), onContinue);
            return root;
        }

        // Bilgi mesajları (Sky Is Full, Reward…, Out of Lives). Alttaki butonları örtmez; dokununca kapanır.
        // v3.2 (tasarım §11g): pencere açıkken alt butonlar görünür ama çalışmaz — üstlerindeki görünmez kalkana
        // dokunmak yalnızca pencereyi kapatır; butonu kullanmak için yeniden basılır.
        private static GameObject CreateBlockingPanel(Transform parent)
        {
            float bottomInset = ButtonBottom + 2f * ButtonSize.y + 40f;
            GameObject root = CreateDimmedCard(parent, "BlockingPanel", new Vector2(760f, 380f), bottomInset, out GameObject panel);
            root.AddComponent<Button>().onClick.AddListener(() => root.SetActive(false));

            var shield = new GameObject("ButtonShield", typeof(RectTransform), typeof(Image), typeof(Button));
            shield.transform.SetParent(root.transform, false);
            RectTransform shieldRect = shield.GetComponent<RectTransform>();
            shieldRect.anchorMin = Vector2.zero;
            shieldRect.anchorMax = new Vector2(1f, 0f);
            shieldRect.offsetMin = new Vector2(0f, -bottomInset);
            shieldRect.offsetMax = Vector2.zero;
            shield.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            Button shieldButton = shield.GetComponent<Button>();
            shieldButton.transition = Selectable.Transition.None;
            shieldButton.onClick.AddListener(() => root.SetActive(false));
            Text title = CreateText(panel.transform, "Notice", 50, TextAnchor.MiddleCenter);
            title.color = new Color(0.15f, 0.22f, 0.29f);
            title.rectTransform.anchorMin = new Vector2(0.08f, 0.55f);
            title.rectTransform.anchorMax = new Vector2(0.92f, 0.88f);
            title.rectTransform.offsetMin = title.rectTransform.offsetMax = Vector2.zero;
            Text detail = CreateText(panel.transform, string.Empty, 29, TextAnchor.MiddleCenter);
            detail.color = new Color(0.25f, 0.34f, 0.44f);
            detail.rectTransform.anchorMin = new Vector2(0.08f, 0.18f);
            detail.rectTransform.anchorMax = new Vector2(0.92f, 0.55f);
            detail.rectTransform.offsetMin = detail.rectTransform.offsetMax = Vector2.zero;
            return root;
        }

        // "No Moves" başarısızlık ekranı: tam ekran (dokunarak kapanmaz), üç seçenek.
        private static GameObject CreateFailPanel(Transform parent, Action onRetry, Action onAd, Action onCoins,
            out Button retry, out Button ad, out Button coins)
        {
            GameObject root = CreateDimmedCard(parent, "FailPanel", new Vector2(760f, 700f), 0f, out GameObject panel);
            Text title = CreateText(panel.transform, "No Moves", 54, TextAnchor.MiddleCenter);
            title.color = new Color(0.15f, 0.22f, 0.29f);
            title.rectTransform.anchorMin = new Vector2(0.08f, 0.80f);
            title.rectTransform.anchorMax = new Vector2(0.92f, 0.95f);
            title.rectTransform.offsetMin = title.rectTransform.offsetMax = Vector2.zero;
            Text detail = CreateText(panel.transform, string.Empty, 28, TextAnchor.MiddleCenter);
            detail.color = new Color(0.25f, 0.34f, 0.44f);
            detail.rectTransform.anchorMin = new Vector2(0.08f, 0.62f);
            detail.rectTransform.anchorMax = new Vector2(0.92f, 0.80f);
            detail.rectTransform.offsetMin = detail.rectTransform.offsetMax = Vector2.zero;

            Vector2 wide = new Vector2(620f, 92f);
            retry = CreateButton(panel.transform, "Retry", new Vector2(0f, 300f), onRetry, LifeButtonColor);
            ad = CreateButton(panel.transform, "Watch Ad  -  Continue", new Vector2(0f, 180f), onAd, AdBadgeColor);
            coins = CreateButton(panel.transform, "Continue", new Vector2(0f, 60f), onCoins, CoinButtonColor);
            foreach (Button button in new[] { retry, ad, coins }) ((RectTransform)button.transform).sizeDelta = wide;
            retry.gameObject.name = "RetryButton";
            ad.gameObject.name = "AdButton";
            coins.gameObject.name = "CoinsButton";
            return root;
        }

        private static readonly Color LifeButtonColor = new Color(0.88f, 0.32f, 0.42f);
        private static readonly Color CoinButtonColor = new Color(0.86f, 0.62f, 0.10f);

        // Tam ekran yarı saydam karartma + ortada kart.
        private static GameObject CreateDimmedCard(Transform parent, string name, Vector2 size, float bottomInset, out GameObject card)
        {
            var root = new GameObject(name, typeof(RectTransform), typeof(Image));
            root.transform.SetParent(parent, false);
            RectTransform rootRect = root.GetComponent<RectTransform>();
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = new Vector2(0f, bottomInset);
            rootRect.offsetMax = Vector2.zero;
            root.GetComponent<Image>().color = new Color(0.06f, 0.12f, 0.24f, 0.45f);

            card = new GameObject("Card", typeof(RectTransform), typeof(Image));
            card.transform.SetParent(root.transform, false);
            RectTransform rect = card.GetComponent<RectTransform>();
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = size;
            card.GetComponent<Image>().color = new Color(0.97f, 0.98f, 1f);
            Shadow shadow = card.AddComponent<Shadow>();
            shadow.effectColor = new Color(0.05f, 0.10f, 0.25f, 0.30f);
            shadow.effectDistance = new Vector2(0f, -10f);
            return root;
        }

        private static Text CreateText(Transform parent, string value, int size, TextAnchor alignment)
        {
            var go = new GameObject("Label", typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            Text text = go.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.text = value;
            text.fontSize = size;
            text.alignment = alignment;
            text.color = Color.white;
            text.raycastTarget = false;
            return text;
        }
    }
}
