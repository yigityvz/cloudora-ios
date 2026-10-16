# crVersion — Cloudora Versiyon Kaydı

`bahadir-claude-versions` dalındaki versiyonların kaydı. Her versiyon kullanıcı onayıyla oluşturulur ve git etiketiyle işaretlenir. Major değişiklikler yeni ana numara alır (`v1`, `v2`, `v3`, … → sıradaki `v4`); bir versiyona sonradan gelen eklemeler ara numara alır (`v3.1`, `v3.2`, …). GitHub'a push yalnızca kullanıcının her seferinde ayrıca verdiği onayla yapılır (iki hedef birlikte: takım reposu `cloudora-bahadir` + `bahadir-aktas/cloudora-android` `main`).

---

## v3.2 — 16–100 yeniden kuruldu; renkler, buton koruması, Shuffle donması düzeltmesi

- **Tarih:** 2026-10-15
- **Commit:** bu dosyanın güncellendiği commit (etiket `v3.2`).
- **Kapsam (kullanıcı):** v3 deseniyle 16–100'ün kurulması; her bölümde tek yenilik, temalar yalnızca görsel (modifier yok). Faz planı: `Docs/V3_BOLUM_VE_ZORLUK_TASARIMI.md` §11l; şablon ve uygulanan plan `Docs/V3_SABLON.md`; görsel özet `Docs/V3_BOLUM_HARITASI.html`.

### Değişiklikler
1. **16–30 (Geçiş teması)** — `Editor/V3LadderBuilder.cs`: serpiştirilmiş dizilim (17), kapasite 5 (19), tek boş bulut ve ilk tuzaklar (21, 26, 30); No Moves bantlara dağılmış; final 30.
2. **31–100 (Tema 2 ve 3)** — `Editor/V3ThemeBuilder.cs`: Rüzgar 31, Ay 81; yeni renkle boş bulut ve dizilim "nefes alır" (2 boş, 0 açık), dizilim adımları 0 açık → 1 açık → klasik (39/49, 74, 89/94); tek boş bulut 36 ve 87'de geri döner; tuzak bölümleri döngüde 1 → 2, 75/78/95/98 Zor/Çok zor tuzak; finaller 60 ve 100 parçanın en tuzaklısı ve en az ortanca uzunlukta. Kilitleyen tahta (takılma ≥ 0,5) yalnızca finalde.
3. **Oyuna bağlama:** `AuthoredLevelCatalog.TryGetBuilt` + `PuzzleController.LoadLevel`: 16–100 veri dosyasından (`V3LevelData.Generated.cs`); 101+ eski üretici.
4. **Ölçüm:** çıkmaz ağırlığı 20 (`DeadStateWeight`); `NoMovesDepth` / `NoMovesStateRatio`; uzunluk tercihi yerine "absürt uzunluk" sınırı (çerçevenin %90'lık değeri). `BoardFrame.OpenClouds` (serpiştirilmiş dizilim), `LevelCandidateFactory.DealScattered`.
5. **Renkler:** Kar #7FE3F5, Rüzgar #AEB7C2, Şimşek #25358A (koyu parçada beyaz desen).
6. **Bilgi penceresi açıkken alt butonlar kilitli** (görünmez `ButtonShield`).
7. **Shuffle donması düzeltmesi (Faz 2.6, telefonda bulundu):** 7 bulut + 2 Extra'da Shuffle ~144 sn donuyor ve çözülebilir tahtayı reddediyordu. Oyun içi Shuffle ve No Moves dönüşü artık `PuzzleSolver.CheckSolvable` kullanıyor (ilk çözümde durur, ölçü hesaplamaz).
8. **Geliştirme logları:** `[Input]` buton satırları ve analytics satırlarında kare/zaman (yalnızca debug).

### Doğrulama
- **EditMode:** 196 test; 192 geçti, 4 atlandı (3 bant aralığı testi araçla kurulan parçalara uygulanmıyor, 1 elle çalıştırılan ölçüm), başarısız yok.
- **Unity Editor:** kullanıcı 16–30 (17:56), 31–60 (18:42) ve 61–100'ü (20:00) oynadı; memnun. Geri bildirimle 59 (yalnızca o) ve Tema 2 finali düzeltildi.
- **Telefon (Galaxy S24 FE, "Cloudora v3"):** 16–19, 31–41, 59–61, 75–78, 81–83, 87; renkler ayrılıyor, No Moves dönüşü anında; Shuffle düzeltmesi 83 + 2 Extra'da 10 Shuffle ile doğrulandı (ort. 0,28 sn). Çökme ya da oyunla ilgili exception yok.

### Bilinen sınırlar / açık konular (tasarım §15)
- Finaller (60, 100) ve 5 renkli planlama: ölçü dikkatli insanı tam tahmin etmiyor (Faz 3).
- Tahta ölçeği: 7 bulutta ve Extra alınınca tahta küçülüyor; uzun vadede düzenlenecek.
- Dünya adları ekranda eski listeden (Faz 5.1). 101+ eski sistem: Rainbow modifier ve ani zorluk sıçraması.
- Büyük tahtada gerçekten çözülemeyen bir Shuffle dizilişi hâlâ 200.000 duruma kadar arar (ölçümde görülmedi).

---

## v3.1 — Bulut içi Shuffle, Restart'ın kaldırılması, başarısızlık ekranı, coin

- **Tarih:** 2026-10-14 / 2026-10-15
- **Commit'ler:** `0026a1d` (ana değişiklikler) → `4145b49` (başarısızlık kaydı) → bu dosyanın güncellendiği commit (etiket `v3.1`).
- **Neden:** Ana yazarın Shuffle'dan amacının bir bulutu kendi içinde karıştırmak olduğu anlaşıldı. Orijinal kod (`a18a3e4`) ve v2 bütün tahtayı yeniden üretiyordu. Bunun yanında Restart kaldırıldı, "No Moves" bir başarısızlık ekranına dönüştü ve coin eklendi.

### Değişiklikler
1. **Safe Shuffle (bulut içi)** — `Scripts/Level/CloudShuffler.cs`, `PuzzleController.cs`
   - Shuffle'a basınca seçili bulut bırakılır ve "Select a cloud to shuffle" yazısı çıkar. Seçilen bulut kendi içinde karıştırılır.
   - Sonuç her zaman farklı bir dizilimdir ve tahta çözülebilir kalır; çözücüyle doğrulanır.
   - Değişemeyecek bulutlar (boş, tek renkli, tek elemanlı) seçilemez ve hak düşmez. Özel (modifier'lı) bulutlara dokunulmaz.
   - Hak, karıştırma yapıldığında düşer. Shuffle'a tekrar basmak, boş yere dokunmak ya da başka bir buton iptal eder.
   - Shuffle Undo ile geri alınamaz: Undo geçmişi temizlenir, Undo butonu söner.
   - Açılma koşulu bölüm ≥ 16 (eskiden "üretilmiş bölüm" koşuluydu).
2. **Restart kaldırıldı:** buton, can rozeti, `RestartCost`, ilgili mesajlar ve `level_restarted`. Can sistemi kaldı.
3. **"No Moves" = başarısızlık ekranı** (tam ekran; Undo/Extra/Shuffle bu ekranda kullanılamaz):
   - **Retry:** −1 can, bölüm sıfırdan başlar, ekstra bulutlar silinir. 1–5'te ücretsiz.
   - **Watch Ad / Continue (40 coin):** çözüme ulaşılabilen son duruma döner, gerekirse birkaç hamle geri (`FailRecovery`).
4. **Başarısızlık kaydı** (`Scripts/Services/FailRecord.cs`): Ekran açıkken uygulama kapanırsa tahta, Undo geçmişi ve hamle sayısı kaydedilir. Açılışta aynı ekran geri gelir; kapatıp açmak Retry'ın can bedelini atlatmaz.
5. **Coin** (`Scripts/Progression/CoinEconomy.cs`):
   - Bölüm sonunda Orta 10, Zor 15, Çok zor 20 coin (101+: 10). Devam 40 coin. Başlangıç 0; ileride tutorial görevinde verilecek.
   - Coin sol üstte gösterilir, tamamlama ekranında "+N coins" yazar ve kayıtta (`SaveData.coins`) tutulur.
6. **Buton düzeni:** üst sırada Extra · Undo · Shuffle; alt sırada World ve Menu, üstteki butonların tam aralarında.
7. **Geçici geliştirme butonu:** sağ üstte "AD +1" (reklamla +1 can). Reklamla can almanın asıl yeri ileride belirlenecek.
8. **Diğer:**
   - Bitmiş bölümde Undo engellendi (coin'in iki kez kazanılmaması için).
   - Analytics'e `level_retried`, `level_continued`, `coins_earned` ve `coins_spent` eklendi.
   - Reklam yerlerine `RewardedPlacement.Continue` eklendi.
9. **Klasör düzeni:** Bölüm aracının yalnızca bölüm üretirken kullanılan parçaları (`LevelCandidateFactory`, `LevelCandidateSelector`, `V3LevelBuilder`) `Scripts/Level/` altından `Editor/` altına taşındı; APK'ya girmiyorlar. `BoardFrame` ayrı dosyada oyun tarafında kaldı (`V3LevelPlan` kullanıyor). Çözücü (`PuzzleSolver`) oyun tarafında kaldı; Shuffle ve "devam" onu oyun içinde kullanıyor.
10. **Geri alınanlar:** v2'deki "Shuffle ekstra bulutlara da dağıtır" (`LevelGenerator.Generate` `extraEmptyClouds`) ve v3'ün Shuffle yedek tahtaları.

### Doğrulama
- **EditMode:** 96/96. Bilerek eklenen hatalarla da denendi: güvenlik kontrolü kapalı Shuffle, "devam"ın her zaman son duruma dönmesi, Undo geçmişini yazmayan başarısızlık kaydı. Üçü de testler tarafından yakalandı.
- **Unity Editor ve telefon (Galaxy S24 FE, "Cloudora v3"):**
  - 16–18'de Shuffle (2 ekstra bulutlu tahtada da) ve coin (+10, +10, +15) çalıştı.
  - L19'da 3 hamlede başarısızlık oluştu. Retry'da can düştü. Reklamla devam, hesaplandığı gibi 2 hamle geri gitti.
  - Kapat-aç sonrası başarısızlık ekranı can harcamadan geri geldi.
  - Çökme ya da exception görülmedi.

### Bilinen sınırlar
- Çözücü modifier'ları bilmiyor. Bu yüzden 101+ (eski sistem) bölümlerde Shuffle ve "devam"ın güvenliği yaklaşık; çok büyük tahtalarda "No safe shuffle" çıkabilir.
- Hamle yapılabildiği halde kazanılamayan durumda otomatik başarısızlık yok (kullanıcı kararı). İleride menüden "baştan başlat" eklenecek.
- Bölüm ortasında, başarısız olmadan kapatılırsa bölüm sıfırdan başlar (orijinal davranış).

---

## v3 — Ölçülmüş zorluk: bölüm aracı ve 1–15

- **Tarih:** 2026-10-14
- **Commit:** `b0b2f13` (etiket `v3`).
- **Kapsam (kullanıcı):** İlk 100 bölüm, oyunun yapısı değişmeden, zorluğu belirli bölümlere dağıtılarak yeniden kurulacak. Desen her 5 bölümde bir tekrarlanıyor: Orta, Orta, Zor, Orta, Çok zor ("Kolay" yok). Zorluk göreli ve birikimli. Bölüm aralıkları: 1–5 Tanıtım, 6–15 Alışma, 16–30 Geçiş teması, 31–60 Tema 2, 61–100 Tema 3. Bu versiyonda 1–15 tamamlandı; kalan parçalar v3.x olarak gelecek. Ayrıntılar: `Docs/V3_BOLUM_VE_ZORLUK_TASARIMI.md`.

### Değişiklikler
1. **Bölüm aracı** (`Scripts/Level/`):
   - **Çözücü** (`PuzzleSolver`): tam durum uzayını tarar. Ölçtükleri: en kısa çözüm, çıkmaz oranı, ilerleten ve tuzak hamle oranları, ve "sanal oyuncu" (ileriyi hesaplamayan, mantıklı görünen hamleyi seçen oyuncunun fazla hamlesi ve takılma oranı). Bunlardan birleşik bir puan çıkar.
   - **Aday üretici** (`LevelCandidateFactory`, platformdan bağımsız SplitMix64) ve **seçici** (`LevelCandidateSelector`).
   - **Parça kurucu** (`V3LevelBuilder`): bant aralıklarını aday dağılımından çakışmasız çıkarır. Öğretici yazının gösterdiğini tahtada şart koşar ve aynı bandı parça içinde hafifçe zorlaştırır.
   - **Plan** (`V3LevelPlan`).
   - **Üretilen veri** `V3LevelData.Generated.cs`; Editor menüleri `Cloudora/V3/…` (aday raporu, veri üretimi, kalibrasyon raporu).
2. **1–15** artık ölçülüp seçilmiş tahtalardan geliyor (`AuthoredLevelCatalog` → `V3LevelData`). Öğretici yazılar, kapasite 4, 2 boş bulut, Shuffle kilidi ve 1–5'te can kuralı aynı kaldı. Renk sayısı: 1–5'te 2, 6–15'te 3.
3. **Kalibrasyon:** Telefon ve Editor testlerinin insan verisiyle sanal oyuncu ölçümü eklendi; 1–5 yeniden seçildi.

### Doğrulama
- **EditMode:** 80/80 (v3 commit'inde). Hatalar bilerek eklenerek denendi: renk kuralı, kanonik anahtar, sanal oyuncunun tercihleri, veri dosyasının elle bozulması. Hepsi yakalandı.
- **Unity ve telefon:** 1–5 ve 6–15 test edildi.
  - Ortalama fazla hamle: 1–5'te Orta 0–1, Zor ve Çok zor 1–2. 6–15'te Orta 0,9 < Zor 1,5 < Çok zor 1,75.
  - Süreler 1. parçadan 2. parçaya belirgin şekilde arttı.

### Bilinen açık konular
- 6–15'in ikinci döngüsünde Zor ve Çok zor, Orta'lardan zayıf ayrışıyor. Kullanıcı kararıyla kabul edildi: oyunun başında radikal artış istenmiyor.
- Öğretici yazılar fark edilmiyor; tutorial ileride ele alınacak.

---

## v2 — Temel tasarım, booster/can göstergeleri ve oynanış düzeltmeleri

- **Tarih:** 2026-10-13
- **Dal:** `bahadir-claude-versions` (v1 `adaa0b8` → docs `755cbbc` → docs `d21a94c` → v2)
- **Uygulama:** `com.yigityvz.cloudora` 0.1.0 (version code 1) — kaynak değişmedi. Cihaz testi için ayrı paket adıyla (`com.yigityvz.cloudora.v2`, "Cloudora v2") derlendi; aşağıya bkz.
- **Kapsam kararı (kullanıcı):** "Temel düzeyde tasarım — göze güzel görünsün, her şey yerinde olsun, renkler hafif değişsin." Öneri dokümanından D1, D2, D3 ve D6'nın bir kısmı + test sırasında bulunan hatalar. D4 (World ekranı), D5 (titreşim), ses, zorluk dengesi bu sürümde **yok**.

### 1. Görsel tasarım
1. **Bulut / gökyüzü kontrastı (D1)** — `CloudoraVisuals.cs`, `CloudContainerView.cs`
   - Gökyüzü degradesi: üst `(0.92,0.97,1)` → `(0.56,0.76,0.96)`, alt `(0.96,0.94,1)` → `(0.80,0.85,0.98)`.
   - Bulut gövdesi `(0.93,0.97,1,0.96)` → `(0.99,1,1,1)`; gölge alpha 0.16 → 0.30; yeni ince mavi `Outline` kenarı.
   - Boş slot `(1,1,1,0.08)` → `(0.82,0.88,0.95,0.55)`; Snow rengi `(0.82,0.94,1)` → `(0.72,0.87,1)` (beyaz bulutta kaybolmasın).
   - Seçili bulut beyaza değil sıcak sarıya döner, ölçek 1.04 → 1.06 (bulut beyazlaşınca eski vurgu görünmez olurdu).
2. **Alt butonlar 3+3 eşit ızgara (D2)** — `GameplayOverlay.cs`
   - 300×84 butonlar, x = −336 / 0 / +336; üst sıra Restart / Undo / World (indigo), alt sıra Extra / Shuffle / Menu (camgöbeği); gölge.
   - Üst yazılar yeniden dizildi: seviye −70, ipucu satırı −125, öğretici −180.
3. **Menü ekranları tam opak (D3)** — `MainMenuController.cs`, `WorldScreenController.cs`, `CloudoraVisuals.CreateBackdrop`
   - Main Menu, Settings ve World tam ekran opak gökyüzü arka planı üzerinde; arkadaki oyun görünmez ve dokunulamaz.
   - **Hata düzeltmesi:** World ekranı oyun butonlarının *altında* kalıyordu; açılınca artık en üste gelir.
4. **Pencereler** — "Sky Balanced!" ve "No Moves / Out of Lives" kartları yarı saydam karartma üzerinde. "No Moves" karartması alt butonları örtmez (mesaj o butonları kullanmayı söylüyor).
5. **Yuvarlak köşeler geri alındı** — İlk denemede butonlara/kartlara eklenen yuvarlak köşeler kenar yumuşatma olmadığı için kötü göründü (kullanıcı geri bildirimi) → dikdörtgen. Ana yazarın orijinal yuvarlak öğelerine (Main Menu kart/butonları, bulut slotları) dokunulmadı.

### 2. Booster ve can göstergeleri
1. **Köşe rozetleri** (kullanıcı seçimi; maketle karşılaştırıldı) — Restart / Undo / Extra / Shuffle butonlarının sağ üst köşesinde hap rozet (`CloudoraVisuals.Pill`, kenarı yumuşatılmış sprite).
   - Rozet = eldeki ücretsiz hak (kayıtta tutulur, seviyeler arası taşınır). Hak 0 ve reklam varsa turuncu **AD**; reklam yoksa **0**.
2. **Üstteki "Undo 3 • Extra 1 • Shuffle 1" yazısı kaldırıldı** (rozetlerle çift bilgi olurdu). Yerine yalnızca bir rozet AD durumundayken görünen satır: "**AD** Watch an ad to refill".
3. **Restart can rozeti** — `RestartCost` (Free / Life / Ad / NoLives): bedavaysa rozet yok; can gidecekse kalp + "-1"; can yoksa AD (reklam varsa) veya kalp + "0". Kurallar `RestartLevel` ile birebir aynı (`GetRestartCost`).
4. **Kalp ikonu** — `♥` karakteri Unity'nin yerleşik fontunda yok; Editor'da sistem fontundan çiziliyor ama **Android'de görünmüyordu** (sağ üst can göstergesi v1'den beri kalpsizdi). Kalp artık prosedürel, kenarı yumuşatılmış sprite (`CloudoraVisuals.Heart`): sağ üst can göstergesi ve Restart rozeti. Öğretici ipucundaki aynı sorunlu `☝` kaldırıldı.

### 3. Oynanış kuralları ve hata düzeltmeleri — `PuzzleController.cs`, `LevelGenerator.cs`
1. **Restart/Shuffle sonrası ızgara boyutu (hata, kullanıcı buldu)** — Extra bulutlarla küçülen ızgara, Restart'ta ekstralar silinince küçük kalıyordu. `_boardLayout.Configure` artık `LoadState` sonunda.
2. **Restart ekstra bulutları korur** (kullanıcı kararı) — boş olarak geri eklenir.
3. **Shuffle ekstra bulutları korur ve onlara da dağıtır** (kullanıcı kararı) — `LevelGenerator.Generate(level, seed, extraEmptyClouds)`; ekstralar yeni tahtanın parçası olur. Sayı seviyenin orijinal bulut sayısına (`_baseCloudCount`) göre hesaplanır (ilk denemede art arda iki Shuffle ekstraları sıfırlıyordu — düzeltildi). Restart, Shuffle'ın dağıtımını korur (Shuffle ücretli olacağı için normal kabul edildi).
4. **Extra Cloud seviye başına en fazla 2** (kullanıcı kararı; analiz: boş bulut sayısı renk sayısına ulaşınca bulmaca önemsizleşir). Limit dolunca buton soluklaşır ama basılabilir; mesaj **"Sky Is Full"** (kullanıcı seçeneklerden seçti): üretilmiş seviyede "…Try Undo or Shuffle.", Shuffle kilitli 1–15'te "…Try Undo.". Hak/reklam harcanmaz.
5. **Akıllı "No Moves" mesajı** — yalnızca o an kullanılabilen seçenekler: Undo (geçmiş varsa), Extra Cloud (limit dolmadıysa), Shuffle (üretilmiş seviyede), Restart (can/reklam durumuna göre); hakkı 0 olup reklamla dolanlar "(ad)" + alt satırda "(ad) = watch an ad first"; hiçbiri yoksa "Wait for the next life to regenerate."
6. **Seviye bittikten sonra Restart engellendi** — v1'de "Sky Balanced!" ekrandayken Restart'a basılabiliyor, can harcanıp seviye ikinci kez tamamlanabiliyordu (çift analytics).

### 4. Build aracı — `Assets/_Cloudora/Editor/CloudoraBuildTools.cs`
- `CLOUDORA_DEV_APP_ID` / `CLOUDORA_DEV_PRODUCT_NAME`: **yalnızca development APK**'da paket adı ve görünen ad geçersiz kılınabilir (aynı telefonda sürümleri yan yana kurmak için). Release/Play build'de verilirse build durur. Build sonrası değerler geri yüklenir; `ProjectSettings.asset`'e kalıcı yazılmaz. Değişken verilmezse davranış v1 ile aynı.

### Doğrulama
- **Unity Editor (Play Mode):** Kullanıcı tüm akışları defalarca denedi (renkler, butonlar, menüler, Restart/Shuffle/Extra, limit, rozetler). Editor.log'da exception yok. Ara derlemeler ayrıca Unity'nin Roslyn derleyicisiyle bağımsız kontrol edildi (0 hata).
- **Android cihaz (Galaxy S24 FE):** "Cloudora v2" (`com.yigityvz.cloudora.v2`) v1'in yanına **sıfır kurulum** olarak yüklendi; v1 telefonda kendi kaydıyla duruyor.
  - İlk açılış: varsayılan kayıtla seviye 1; L1 → L21 oynandı, Green Valley tamamlandı.
  - Extra limiti her seviyede tam 2; Shuffle 1–15'te kilitli, 16+'da açık; can bitince ödüllü can teklifi; ara reklam fırsatı.
  - **No Moves:** L19'da (1 boş bulutlu seviye) hesaplanan 3 hamlelik diziyle tetiklendi; reklamlı/reklamsız tüm seçenek kombinasyonları kullanıcı tarafından doğrulandı (`level_failed reason=no_moves`).
  - Kalp ikonları telefonda görünüyor (kullanıcı teyidi).
  - Logcat: çökme, ANR, C# exception yok; yalnızca dev build'e özgü Profiler soket uyarıları ve `AssetPackManager` kontrolü.

### Build ortamı notları (v2'de değişenler)
- **Windows 260 karakter yol sınırı:** `GRADLE_USER_HOME` proje içinde (`Library\GradleHome`) olunca native derleme (`buildCMakeDebug`) `Filename longer than 260 characters` ile düştü. Kısa yol kullanılmalı:
  ```powershell
  $env:GRADLE_USER_HOME = "C:\Dev\gradle-cloudora"
  ```
  Yol değiştirildiyse eski CMake önbelleği proje kökündeki `.utmp\` ve `Library\Bee\Android\Prj\IL2CPP\Gradle\unityLibrary\build\` klasörlerinde kalır; ikisi de silinmeli (git dışı, yeniden üretilir).
- **Yan yana v2 kurulumu:**
  ```powershell
  $env:CLOUDORA_DEV_APP_ID = "com.yigityvz.cloudora.v2"
  $env:CLOUDORA_DEV_PRODUCT_NAME = "Cloudora v2"
  $env:CLOUDORA_BUILD_OUTPUT = "Builds/Android/Cloudora-v2-dev.apk"
  ./Build-Android.ps1 -Mode dev -ProjectPath "C:\Dev\ClaudeProjects\Android-Cloudora\Cloudora"
  adb install -r Builds\Android\Cloudora-v2-dev.apk
  ```
- APK üzerine yazılarak üretilirse dosya boyutu büyüyebilir (Gradle artımlı paketleme boşluğu; içerik aynı). Temiz boyut için build öncesi çıktı dosyası silinmeli.

### Bilinen açık sorunlar / notlar (v2'de dokunulmadı)
1. v1 listesindeki 1, 2, 4, 6, 7, 8 hâlâ açık (Night/Fog desenleri, doğrulayıcı Wind'i bilmiyor, Rainy Coast "Next rule" metni, `Update` GC, ayarların iki yerde tutulması, yerelleştirme yok). #3 (bitmiş seviyede Undo) hâlâ açık; #5 (Restart süre sayacını sıfırlamıyor) hâlâ açık — cihazda L1 368 sn, L16 1101 sn görüldü.
2. "No Moves" yalnızca "hiç legal hamle yok" durumunu yakalar; anlamsız ileri-geri hamleler legal sayıldığı için fiilen çıkmaza giren oyuncu mesajı görmeyebilir. 3 renk + 2 boş bulutlu seviyelerde No Moves matematiksel olarak imkânsız; yalnızca 1 boş bulutlu seviyelerde (19, 29, 39…) mümkün.
3. Reklamla alınan can, "Tap again" ile basılan Restart'ta hemen harcanır (kullanıcı: gerçek reklam gelene kadar kalsın).
4. Hamlesiz Restart'lar (Undo ile her şey geri alınmış / Shuffle sonrası) tahtayı değiştirmez ama `level_restarted` olarak kaydedilir (analytics gürültüsü).
5. Shuffle seed dizisi uygulama yeniden açıldığında aynı seviyede aynı sırayla tekrar eder.
6. 41–80'de Frozen/Locked son buluta konur; Shuffle sonrası son bulut bir ekstra bulut olabilir.
7. Release build hiç cihazda denenmedi (reklam kapalı → rozetler "0", "Reward Unavailable"); build aracı release'i yalnızca AAB üretiyor.
8. Telefondaki v1 "Cloudora" adıyla duruyor ("Cloudora v1" olarak yeniden adlandırılmadı).

---

## v1 — Çalışır durumdaki temel sürüm

- **Tarih:** 2026-10-12
- **Dal:** `bahadir-claude-versions` (`main` @ `0c967b1` üzerinden açıldı)
- **Temel:** Ana yazarın (yigityvz) son kod commit'i `a18a3e4` — *fix: restore gameplay UI on Android startup* (upstream `codex/phase2-15`) + `main`'deki README katkı bölümü (`0c967b1`).
- **Uygulama:** `com.yigityvz.cloudora` 0.1.0 (version code 1), Unity 6000.5.9f1, URP 2D, Android min API 26 / target API 36, IL2CPP, ARM64.

### Amaç
Orijinal projenin bu makinede Editor'da ve gerçek Android cihazda çalıştığı doğrulanmış, sonraki geliştirmeler için güvenli başlangıç noktası.

### Değişiklikler
1. **SkyGradient için eksik `CanvasRenderer` eklendi** — `Assets/_Cloudora/Scripts/UI/CloudoraVisuals.cs`
   - Sorun: Play Mode'da her karede `MissingComponentException: There is no 'CanvasRenderer' attached to the "SkyGradient" game object`. Hata EventSystem'in UI raycast'ini kesiyordu; Editor'da oyun fiilen oynanamıyordu.
   - Önce: `new GameObject("SkyGradient", typeof(RectTransform), typeof(SkyGradientGraphic));`
   - Sonra: `new GameObject("SkyGradient", typeof(RectTransform), typeof(CanvasRenderer), typeof(SkyGradientGraphic));`
2. **`crVersion.md` eklendi** — bu dosya.

### Doğrulama
- **Unity Editor (Play Mode):** Seviye 1–17 oynandı; hata yok.
- **Android cihaz:** Samsung Galaxy S24 FE (SM-S721B), development APK (45.278.886 bayt, 0 build hatası).
  - ~13 dk oyun: seviye 1 → 42, 41 seviye tamamlandı; Green Valley ve Rainy Coast dünyaları bitirildi, Frost Peaks'e (Frozen) geçildi.
  - Undo, Extra Cloud, Restart, sahte ödüllü reklam ve interstitial akışları tetiklendi.
  - Uygulama 6 kez kapatılıp açıldı; her seferinde doğru seviyeden devam etti (kayıt sistemi çalışıyor).
  - Logcat: çökme, ANR veya C# exception yok. Yalnızca zararsız Unity mesajları (`AssetPackManager` kontrolü, dev build'e özgü Profiler multicast socket uyarıları).
- Not: Test edilen APK `add-contributors-file` dalından derlendi; bu dalla tek fark doküman dosyalarıdır (CONTRIBUTORS.md / README), oyun kodu birebir aynıdır.

### Build ortamı notları (bu makine)
- Unity Hub → 6000.5.9f1 → **Android SDK & NDK Tools** ve **OpenJDK** modülleri gerekli.
- Kullanıcı adında ASCII olmayan karakter ("ı") olduğu için Gradle'ın `prefab_command.bat` adımı bozuluyor (`ClassNotFoundException: com.google.prefab.cli.AppKt`). Çözüm, build'i başlatırken süreç bazında:
  ```powershell
  $env:GRADLE_USER_HOME = "C:\Dev\ClaudeProjects\Android-Cloudora\Cloudora\Library\GradleHome"
  ./Build-Android.ps1 -Mode dev -ProjectPath "C:\Dev\ClaudeProjects\Android-Cloudora\Cloudora"
  ```
- Build öncesi Unity Editor kapalı olmalı (proje kilidi).
- Kurulum: `adb install -r Builds/Android/Cloudora-dev.apk` (adb: `...\AndroidPlayer\SDK\platform-tools\adb.exe`).

### Bilinen açık sorunlar (v1'de dokunulmadı)
1. Night/Fog gizlemesi desen yazılarını (`*`, `///`, `+`) gizlemiyor — `CloudContainerView.cs:203-205`.
2. `PuzzleValidator` modifier'ları (özellikle Wind) hesaba katmıyor; 10.000 seviye doğrulaması Wind seviyelerinde oynanabilirliği kanıtlamıyor.
3. Seviye bittikten sonra Undo yapılabiliyor; yeniden çözülünce `CheckWin` ikinci kez çalışıyor (çift analytics / reklam sayacı).
4. Rainy Coast (21–40) "Next rule: Wind" gösteriyor ama üretici bu aralıkta modifier eklemiyor.
5. `RestartLevel` `_levelStartedAt`'ı sıfırlamıyor; süre analytics'i yanlış.
6. `PuzzleController.Update` her karede string üretiyor (GC).
7. Ses/titreşim ayarı hem PlayerPrefs'te hem JSON kayıtta.
8. Arayüz metinleri sabit İngilizce; yerelleştirme yok.
- Gözlem: Cihaz testinde seviye 9'da 5, seviye 31'de 3 yeniden başlatma — zorluk sıçraması olabilir.
  - **Düzeltme:** Zorluk değil. Kullanıcı Restart (ve diğer butonları) **kasıtlı olarak** art arda basarak test etti (L9: 3 sn'de 5, L31: 0,7 sn'de 3 Restart). Hızlı art arda basmaya karşı koruma (debounce) ileride eklenebilecek bir iyileştirme olarak not edildi; mevcut davranış bir kullanıcı hatası değildir.
- Yayın için açık kalanlar: imzalama, gizlilik politikası/destek e-postası, gerçek ekran görüntüleri, Play Console beyanları.

### Cihaz testi logcat gözlemleri (ikinci inceleme)
- Seviye süreleri L9–L41 arası 6–25 sn, 6–13 hamle; tasarım hedefi (≤20: 30–60 sn, 21–100: 60–180 sn) karşılanmıyor.
- Extra Cloud dev build'de fiilen sınırsız (sahte reklam anında dolduruyor; L18'de 2 sn içinde 3 kez — kullanıcının kasıtlı buton testi).
- Her geçersiz hamlede `Handheld.Vibrate()` 500 ms titreşim — uzun/sert.

### Kullanıcı yorumu (Galaxy S24 FE testi sonrası)
0. **Test notu:** Restart, Extra Cloud, Shuffle vb. butonlara hızlı art arda basmalar kullanıcının **kasıtlı buton testi**dir; yanlışlıkla basma veya zorluk kaynaklı değildir. Hızlı basmaya karşı koruma eklenmesi güzel bir özellik olabilir.
1. **Zorluk / tekrar:** Mantık anlaşıldıktan sonra zorluk hissedilmiyor, bölümler hızlıca geçiliyor. 15. seviyeden sonraki üretilen bölümler kendini tekrarlıyor gibi; basit bir matematikle aynı veya çok benzer hamlelerle geçilebiliyor.
2. **World butonu:** Basınca ne olduğu anlaşılmıyor.
3. **Buton hizalaması:** Alt butonlar 3+3 ortalı olabilecekken 4 üst / 4 alt yuvalı ızgarada üst sıranın 1.'si, alt sıranın 3.'sü boş bırakılmış gibi kaymış.
   - Koddan teyit (`GameplayOverlay.cs`): üst sıra -130 / +130 / +390, alt sıra -390 / -155 / +350.
4. **Renk (en dikkat çeken):** Bulut arka planı ekran arka planıyla çakışıyor, göz ayırt edemiyor; birkaç ton daha belirgin olmalı.
   - Koddan teyit: bulut `(0.93, 0.97, 1.0)` ≈ gökyüzü üstü `(0.92, 0.97, 1.0)`.
5. **Menü ekranları:** Continue / World / Settings açıkken arkadaki oyun görünmemeli.
   - Koddan teyit: paneller ekranın ~%84'ünü kaplıyor (anchor 0.08–0.92).
