# iOS aktarım notu — 2026-10-16 11:14 +03

Bu kopyanın güncel hedefi `yigityvz/cloudora-ios`, `main` dalıdır. Kullanıcı 2026-10-16 tarihinde `yigityvz/cloudora-android` / `cloudora-bahadir` son sürümünün **işleyiş değiştirilmeden** iOS'a aktarılmasını ve iOS deposuna push edilmesini istedi. Kaynak commit: `d7c9416` (v3.2 + belge commitleri).

- Yalnızca iOS build ayarları, Editor platform korumaları ve build belgeleri değiştiriliyor.
- Runtime kodu, sahneler, testler, bölüm verileri ve kaynaklar Android ile aynı. Tasarım konuşmalarındaki 500 jeton, yeni hazne, dünya veya UI önerileri uygulanmıyor.
- Android uzak deposuna push yapılmaz; bu oturumdaki hedef yalnızca iOS `origin/main`. Kullanıcının mevcut isteği bu push için yetkidir. Aşağıdaki eski Android push hedefleri bu iOS kopyasında kullanılmaz.
- Eski iOS geçmişi merge ile korunuyor; force-push yok. Önceki yerel build, `Builds/iOS/Cloudora-before-bahadir-20261016` altında tutuldu.
- Test: 196 toplam, 192 geçti, 4 kaynakta bilinçli atlama, 0 hata. 1–100 kalibrasyonu tamamlandı. Unity Xcode export başarılı. Native Xcode sonucu için `Docs/IOS_STATUS.md` esas alınır.
- Bu güncel kaynak için fiziksel iPhone testi henüz yapılmadı; 13 Ekim testi eski sürümdür.
- Push öncesi durum: doğrulamalar bitince bu kayıtla birlikte iOS main'e gönderilecek.

Aşağıdaki Android geliştirme hafızası tarihsel kaynak olarak korunmuştur.

---

# Cloudora — Proje Hafızası (Claude için oturum başlangıcı)

> Her oturumun başında **ilk iş bu dosyayı oku.** Projeyi anlamak için nelere bakılacağını, hangi kuralların geçerli olduğunu ve işin nerede kaldığını özetler. Ayrıntı her zaman kaynak dosyadadır; burada yazanla kaynak çelişirse **kaynak doğrudur**, bu dosya düzeltilir.
> Son güncelleme: 2026-10-15 21:04 (v3.2 push öncesi).

---

## 0. Nerede kaldık (2026-10-15 kapanış) — yeni oturumda önce bunu oku
- **v3.2:** commit `73611d2`, etiket `v3.2`, dal `bahadir-claude-versions`; üstünde belge commit'leri (`03c27dd` ve 21:04 oturum kapanışı). **Push:** 21:04'de kullanıcı "pusha geçelim" dedi; bu commit'ten sonra iki hedefe (`upstream` `cloudora-bahadir`, `origin` `main`, etiket `v3.2`) push planı sunulup onayla yapılacak. Push'un gerçekleşip gerçekleşmediği ve sonucu: `ClaudoraRelease.md`'nin en üst kayıtları (yeni oturumda `git ls-remote` ile de doğrula). Push öncesi uzak repolar `ae54acc` (v3.1).
- **v3.2'de olan:** 16–100 v3 aracıyla kuruldu ve oyuna bağlandı (16–100 veri dosyasından, 101+ eski üretici); renkler; bilgi penceresinde buton kilidi; telefonda bulunan Shuffle donmasının düzeltmesi (`PuzzleSolver.CheckSolvable`). Ayrıntı: `crVersion.md` v3.2, `ClaudoraRelease.md` 2026-10-15 girdileri.
- **Testler:** EditMode 196 → 192 geçti, 4 atlandı (3 bant aralığı testi araçla kurulan parçalara uygulanmıyor; `V32ShuffleLoadProbe` elle çalıştırılan ölçüm), başarısız yok.
- **Oyun testleri tamam:** Editor'da 16–30 (17:56), 31–60 (18:42), 61–100 (20:00) — kullanıcı memnun. Telefonda seçili bölümler (16–19, 31–41, 59–61, 75–78, 81–83, 87) — renkler ayrılıyor, donma düzeldi.
- **Faz planı durumu (tasarım §11l):** Faz 1 bitti; Faz 2.1, 2.2, 2.3 bitti; **2.5 atlandı** (kullanıcı: "gözüme batan bir şey olmadı"); **2.6'nın acil kısmı yapıldı** (Shuffle/No Moves dönüşü hızlı kontrol), kalan kısmı not (tasarım §15); **2.4 bekliyor**; Faz 3 (simülasyon), Faz 5.1 (dünya listesi) bekliyor. Faz 4 (Editor + telefon testi) fiilen yapıldı.
- **Sıradaki seçenekler (kullanıcıya sorulacak):** (1) v3.2 push (ayrı onay), (2) Faz 2.4 — aracın yenilikleri şablondaki aralıklara yerleştirmesi, (3) Faz 3 — 1–100 simülasyonu; finallerin insan için zorluğu da burada (tasarım §15 "Ölçü insanla uyuşmuyor").
- **Cihaz/kayıt durumu:**
  - Telefonda "Cloudora v3" paketinde **v3.2 derlemesi** (20:45) kurulu; kayıt **87**'de (test için elle ayarlandı). Testten önceki telefon kaydının yedeği: `%USERPROFILE%\AppData\LocalLow\YigitYvz\Cloudora\yedek-telefon-v32-test-oncesi-20261015-2016\` (L31, coin 190).
  - Editor kaydı **101**'de (61–100 testinden sonra). Testten önceki yedek: `yedek-v32-tema3-test-oncesi-20261015-1911\`.
  - Unity Editor kapalı.

## 1. Oturum başında okuma sırası
1. **Bu dosya** (önce §0).
2. `ClaudoraRelease.md` (repo kökü) — yapılan her işlemin kaydı, en yeni en üstte.
3. `Docs/V3_BOLUM_VE_ZORLUK_TASARIMI.md` — v3'ün **asıl tasarım dokümanı**: parçalar/temalar, zorluk ilkeleri, kararlar (§11b–11l), faz planı (§11l), açık konular (§15).
4. `crVersion.md` (repo kökü) — onaylanmış versiyonlar (v1 … v3.2), **baştan sona** oku.
5. `Docs/V3_SABLON.md` ("Uygulanan plan": 1–100 bölüm bölüm tablo) ve `Docs/V3_BOLUM_HARITASI.html` (tek sayfalık görsel özet).
6. İşe göre: ilgili kod dosyaları (bkz. §6 kod haritası).
7. **Çalıştırarak doğrula (kullanıcı kuralı, her oturum):** Unity Editor kapalıyken (kilit: `Temp\UnityLockfile`; açıksa `CloseMainWindow` ile kapat) sırayla:
   - EditMode testleri: `Unity.exe -batchmode -nographics -projectPath <proje> -runTests -testPlatform EditMode -testResults Logs\<ad>.xml -logFile Logs\<ad>.log` → XML'deki `total/passed/failed/skipped`. Beklenen: 196 / 192 / 0 / 4.
   - Bölümlerin ölçümü: `-executeMethod Cloudora.Editor.V3LevelTool.RunCalibrationReportBatch` (`CLOUDORA_V3_CAL` satırları). Önizlemeler: `RunTransitionPreviewBatch`, `RunTheme2PreviewBatch`, `RunTheme3PreviewBatch` → `Logs/v3-preview-<Parça>.csv`. Veri üretimi: `GenerateLevelDataBatch` (veri değişmemeli; `GeneratedDataMatchesBuilder` testi korur).
   - Unity yolu: `C:\Program Files\Unity\Hub\Editor\6000.5.9f1\Editor\Unity.exe`.
8. **Editor oyun testi (kullanıcı kuralı, her oturum):** Claude oynayamaz; kullanıcı oynar, Claude hazırlar ve log'dan doğrular.
   - Batch işleri bitince Unity Editor'ı projeyle aç (`Start-Process Unity.exe -ArgumentList '-projectPath "<proje>"'`); kaydı yedekleyip (SHA kontrolü) gerekirse `currentLevel` / `highestCompletedLevel`'i ayarla (`%USERPROFILE%\AppData\LocalLow\YigitYvz\Cloudora\`).
   - Kısa rutin (5–10 dk): 2–3 bölüm; Undo, Extra, Shuffle birer kez; 1 boş bulutlu bir bölümde başarısızlık + devam. Parça değiştiyse parçanın tamamı.
   - Sonra `Logs/Editor.log`'dan doğrula: `[Analytics]` ve `[Input]` satırları (frame ve t ile), exception; bölüm verisiyle (en kısa çözüm) karşılaştır, tabloyu `ClaudoraRelease.md`'ye yaz.
   - **`Logs/Editor.log` Editor her açılışta üzerine yazılır** → önemli test loglarını `Logs/oyun-testleri/` altına tarihli adla kopyala (§8).
   - Editor açıkken build alma ve `Assets/` altına yazma (oyun sırasında yeniden derleme tetiklenir).

## 2. Çalışma kuralları (kullanıcı: Bahadır; Türkçe konuşur)
- Promptları uygula; **gerçek karar noktalarında onay iste** (kullanıcı fazlarda "her adımda sor" dedi). Oyuncuya görünen metinler için 3–4 somut seçenek sun.
- **Her işlem** (küçük düzeltme ve önemli kontroller dahil) tarih-saatle `ClaudoraRelease.md`'ye, en yeni en üstte, Türkçe yazılır. **Saati her zaman `Get-Date` / `date` ile al, tahmin etme** (2026-10-15'te tahmini saat yazılıp düzeltildi). Kişisel yol yazma (`%USERPROFILE%` kullan).
- Uzun işlerde ara ara yüzde ver.
- **Versiyon:** yalnızca kullanıcı onayıyla; `bahadir-claude-versions` dalında commit + etiket, `crVersion.md`'ye giriş. Eklemeler v3.3…; bir sonraki major değişiklik v4. `ClaudoraRelease.md` her versiyonla birlikte commit'lenir.
- **Push: her seferinde ayrı, açık onay.** Önceki onay sonrakine geçmez. Hedefler: `upstream` (yigityvz/cloudora-android) `cloudora-bahadir` + `origin` (bahadir-aktas/cloudora-android) `main`, etiketlerle. Force yok; takım reposunun `main`'ine dokunulmaz.
- **Her push'tan önce proje hafızasını güncelle (kullanıcı, 2026-10-15):** `Docs/PROJE_HAFIZASI.md` (özellikle §0 "Nerede kaldık": versiyon, push durumu, sıradaki iş) ve gerekiyorsa `ClaudoraRelease.md` güncellenir, commit'lenir; push bu commit'le birlikte yapılır.
- Commit'ler değiştirilmez (amend/rebase yok). Çok satırlı mesaj için `git commit -F` (dosya ya da stdin).
- Gördüğün her tutarsızlığı/çakışmayı (küçük de olsa) kullanıcıya söyle. İstisna: `paylasim/DEVRALMA.md` eski ve aktif kullanılmıyor — kaynak sayma, eskiliğini raporlama, dokunma.
- Kullanıcı testlerinde art arda hızlı basmalar kasıtlı buton testidir (genelde 1–15'te). Bilerek "No Moves" denenen bölümler ve özelliklerle hızlı geçilen bölümler zorluk analizine katılmaz. **Daha önce çözülmüş bölümler tekrar oynanınca** kolay gelir (telefon testi 75/78) — zorluk yorumunda bunu belirt.
- Geçmişte olanı anlatırken ne zaman yapıldığını açık yaz.
- Oyunda görünen metinde ASCII dışı sembol kullanma; Android'de görünmez → sprite ikon.
- Yeni C# dosyalarını CRLF'ye çevir. Python'da Türkçe çıktı için `PYTHONIOENCODING=utf-8`.

## 3. Kaynaklar — ne nerede yazıyor
| Kaynak | İçerik | Durum |
|---|---|---|
| `Docs/V3_BOLUM_VE_ZORLUK_TASARIMI.md` | v3 tasarımı: desen, parçalar/temalar, yöntem, Shuffle/başarısızlık/coin kararları, modifier arşivi, kararlar §11b–11l, faz planı §11l, açık konular §15 | **Güncel, asıl referans** |
| `Docs/V3_SABLON.md` | 1–100 şablonu: ilkeler, yenilik envanteri, hedef eğrisi, **"Uygulanan plan"** (100 satır: hedef/puan/renk/kapasite/boş/dizilim/en kısa/tuzak/No Moves/yenilik) + taslaktan farklar; Faz 1.4 taslağı tarihçe | Güncel (20:20) |
| `Docs/V3_BOLUM_HARITASI.html` | Tek sayfa: gelen özellikler sırası, kurallar, tuzak ve No Moves listeleri, 100 bölümün kartları | Güncel (20:10); bölümler değişirse `Logs/araclar/make_map.py` + `make_html.py` ile yeniden üret |
| `Docs/V3_CERCEVE_ENVANTERI.md` | Faz 1.1 ölçümleri: 60 çerçevenin dağılımları + bulgular | Güncel; puan ağırlıkları değişirse yeniden ölçülmeli |
| `ClaudoraRelease.md` | Her işlemin kaydı; test tabloları (insan hamle/süre), kararların gerekçeleri | Güncel |
| `crVersion.md` | v1, v2, v3, v3.1, **v3.2** ayrıntıları | Güncel |
| `README.md` | Proje tanıtımı | v3.2'ye göre güncellendi |
| `Docs/Cloudora 1–100 Şablonu.html` | Faz 1.4 inceleme sayfasının yerel kopyası (eski) | **Commit'e alınmadı** (kullanıcı kararı); yerini `V3_BOLUM_HARITASI.html` aldı |
| claude.ai "1–100 şablonu" (`https://claude.ai/artifact/PDN14bYos5oRJkJLCjHeMW`) | Faz 1.4 inceleme sayfası (grafik + tablo) | Taslak plana göre; uygulanan plan `V3_SABLON.md`'de |
| `Docs/GELISTIRME_ONERILERI.md` | v1 sonrası öneri listesi | Versiyon planı artık geçerli değil |
| `Docs/PROJECT_PROGRESS.md`, `ANDROID_BUILD.md`, `GOOGLE_PLAY_SUBMISSION.md`, `RELEASE_CHECKLIST.md`, `PRIVACY_RELEASE_REVIEW.md` | Ana yazarın geçmişi ve yayın süreçleri | Tarihsel; yayın onayı yok |
| claude.ai "Cloudora — Temalar, Bölümler ve Zorluk" (`https://claude.ai/artifact/2mwyMmgphECxmDmkSojPWH`) | **Eski** 13 dünyalı yapı (1–400) | v3 öncesi; tarihsel |
| `paylasim/DEVRALMA.md` | Eski devralma notu | **Kullanılmıyor, kaynak değil** |
| `Logs/oyun-testleri/` (git dışı) | Tarihli test logları: Editor 61–100, telefon logcat'leri, Shuffle yük ölçümü, önizleme CSV'leri | Bkz. §8 |
| `Logs/araclar/` (git dışı) | Bölüm haritası ve şablon tablosunu üreten Python betikleri | Bkz. §8 |
| Hafıza dizini (`%USERPROFILE%\.claude\projects\…\memory\`) | Kullanıcı tercihleri ve kurallar | Her oturumda otomatik yüklenir |

## 4. Proje özeti
- Unity **6000.5.9f1**, Android, dikey, tek parmakla hava sıralama bulmacası. Ana yazar **yigityvz**; Bahadır katkıda bulunan. Tek sahne `Assets/_Cloudora/Scenes/02_Gameplay.unity`; arayüz kodla oluşturulur.
- **Dal:** `bahadir-claude-versions`. Etiketler: v1 (`adaa0b8`), v2 (`61088e4`), v3 (`b0b2f13`), v3.1 (`ae54acc`), **v3.2 (`73611d2`, yalnızca yerel)**. v3/v3.1 2026-10-15 00:32'de iki repoya push edildi.
- **Telefon (Samsung Galaxy S24 FE, adb `R5CXA3032LP`):** "Cloudora" (v1), "Cloudora v2", "Cloudora v3" (`com.yigityvz.cloudora.v3`, şu an v3.2 derlemesi) yan yana.

## 5. Oyun kuralları (kod: `Scripts/Core/PuzzleRules.cs`, `CloudState.cs`)
- Bulutlar alttan üste dizili hava parçası yığınları; kapasite 2–7 (1–18'de 4, 19'dan itibaren 5). Türler: Güneş, Yağmur, Kar (6+), Rüzgar (31+), Ay (81+), Şimşek (henüz yok) + Gökkuşağı joker (1–100'de yok).
- Hamle: kaynak → hedef. En üstteki aynı renkli grup **sığdığı kadar otomatik** taşınır. Hedef boş ya da üstü aynı renk olmalı.
- Kazanma: boş olmayan her bulut dolu ve tek renk.
- **Hiç legal hamle kalmazsa = bölüm başarısız** (v3.1): Retry (−1 can; 1–5 ücretsiz) · reklam veya 40 coin ile **çözülebilen son duruma** dönüş (`FailRecovery`). Başarısızlık ekranındayken uygulama kapanırsa durum kayda yazılır (`FailRecord`).
- **Booster'lar:** Undo, Extra Cloud (bölüm başına en fazla 2; tahta küçülür), Shuffle (16'dan itibaren; seçilen tek bulutu karıştırır, sonuç çözülebilir ve farklı). Shuffle ve devam kontrolü **`PuzzleSolver.CheckSolvable`** ile (v3.2; ilk çözümde durur). Hak bitince reklam (dev build'de sahte reklam anında verir).
- **Can:** en fazla 5, 30 dakikada 1. **Coin:** Orta +10, Zor +15, Çok zor +20 (101+ için +10).
- **Restart yok.** Geçici geliştirme butonu: sağ üstte "AD +1". Bilgi penceresi açıkken alt butonlar görünür ama kilitli (`ButtonShield`).
- **Modifier'lar** kodda var, **1–100'de kullanılmıyor** (temalar yalnızca görsel); 101+ eski sistemde var.

## 6. Kod haritası (`Assets/_Cloudora/`)
| Yer | Ne |
|---|---|
| `Scripts/Core/` | Saf kurallar |
| `Scripts/Puzzle/PuzzleController.cs` | Ana akış; `LoadLevel`: 1–15 `AuthoredLevelCatalog.Get`, 16–100 `TryGetBuilt` (veri), 101+ `LevelGenerator`; `SolverStateLimit = 200000` |
| `Scripts/Puzzle/CloudContainerView.cs` | Bulut görünümü, renkler (`GetColor`, `GetPatternColor`) |
| `Scripts/Level/V3LevelPlan.cs` | Parçalar, bant deseni, `ShuffleUnlockLevel = 16`; 16+ için `FrameFor` yok (çerçeveyi kurucu seçer) |
| `Scripts/Level/PuzzleSolver.cs` | Ölçüm çözücüsü `Solve` (bütün durum uzayı + zorluk ölçüleri + sanal oyuncu; araç kullanır) ve oyun içi **`CheckSolvable`** (hızlı) |
| `Scripts/Level/BoardFrame.cs` | Çerçeve: kapasite, renk, boş, `OpenClouds` (serpiştirilmiş dizilim) |
| `Scripts/Level/V3LevelData*.cs` | Seçilmiş 1–100 bölüm (`.Generated.cs` araçla üretilir, elle değiştirilmez) |
| `Scripts/Level/AuthoredLevelCatalog.cs` | 1–15 (öğretici yazılar) + `TryGetBuilt` (16–100) |
| `Scripts/Level/LevelGenerator.cs`, `BalanceProfile.cs` | **Eski** üretici: yalnızca 101+ |
| `Scripts/Level/CloudShuffler.cs` | Bulut içi Shuffle + `FailRecovery` |
| `Scripts/Progression/` | Can, coin, booster, `WorldCatalog` (**dünya adları hâlâ eski liste**) |
| `Scripts/Services/` | Kayıt, `FailRecord`, reklam, analytics (`DebugAnalyticsService`: frame ve t) |
| `Scripts/UI/GameplayOverlay.cs` | HUD, butonlar, `ButtonShield`, `[Input]` debug satırları |
| `Editor/V3LevelBuilder.cs` | Parça dağıtımı (Ladder → Theme → sabit 1–15); 1–15 kurulumu (400 aday, 300.000 durum) |
| `Editor/V3LadderBuilder.cs` | 16–30 kurucusu; ortak havuzlar (`Pool`), `LongMovesThreshold`, `PreviousBoards`, `NoMovesBonus`, `V3Novelty` |
| `Editor/V3ThemeBuilder.cs` | 31–100 kurucusu: segment yapılandırması (renk, dizilim adımları, eğriler, dönüş penceresi, `TrapsByCycle`, `HardTrapLevels`), final kuralları, `EarlyTrapPreference`, `TrapPoolCandidates`, Tema 2 için yalnızca 59'u etkileyen `FallbackTrapPoolCandidates` |
| `Editor/V3LevelTool.cs` | Menüler/batch: veri üretimi (`PartsToBuild` = 5 parça), önizlemeler, envanter, dizilim, kalibrasyon raporları |
| `Editor/LevelCandidateFactory.cs` | Aday tahtalar (`DealScattered` dahil) |
| `Tests/Editor/` | `V3LevelBuildTests`, `V32LadderTests`, `V32ThemeTests`, `V32Theme3Tests`, `V32SolverLoadTests`, `V32VisualTests`, `V32InputTests`, `V32NoMovesTests`, `V32LayoutTests`, `V31GameplayTests`, `LevelToolTests`, `PuzzleFeelTests`; `V32ShuffleLoadProbe` (`[Explicit]`, elle) |

## 7. Bölüm tasarımı (v3) — özet
**Desen:** her 5 bölümde **Orta, Orta, Zor, Orta, Çok zor**; etiketler parçadan parçaya farklı ölçekte, zorluk birikimli, parça geçişlerinde sıçrama yok (geçiş testinde Orta'lar için 0,25 pay: eğriler sınırda birleşir).

| Parça | Bölümler | Ad | Ne geliyor | Durum |
|---|---|---|---|---|
| 1 | 1–5 | Tanıtım | 2 renk, kap. 4, 2 boş, öğretici yazılar | Bitti (v3) |
| 2 | 6–15 | Alışma | Kar (6), "Undo is always safe" (10) | Bitti (v3) |
| 3 | 16–30 | Green Valley (görsel) | Shuffle 16, serpiştirilmiş dizilim 17, kap. 5 (19), tek boş bulut 21; final 30 | Bitti (v3.2) |
| 4 | 31–60 | Tema 2 | Rüzgar 31 (2 boşla nefes), tek boş 36'da döner, 1 açık 39, klasik 49; döngüde 1 tuzak; final 60 | Bitti (v3.2) |
| 5 | 61–100 | Tema 3 | 61–80: yeni görsel tema, klasik 74, tuzaklar 1 → 2, Zor/Çok zor tuzak 75, 78. 81–100: Ay 81, tek boş 87, 1 açık 89, klasik 94, Zor/Çok zor tuzak 95, 98; final 100 | Bitti (v3.2) |

- **Kurallar:** bir bölümde tek yenilik, Orta'da gelir; kapasite/renk geri düşmez; yeni renkle boş bulut ve dizilim "nefes alır"; No Moves zaman zaman ve bantlara dağılmış; kilitleyen tahta (takılma ≥ 0,5) yalnızca finalde; final en az ortanca uzunlukta ve parçanın en tuzaklısı; hamle sayısı absürt olmamalı (çerçevenin %90'lık değeri).
- **Puan:** en kısa hamle ×1 + tuzak ×15 + yanıltıcılık ×10 + çıkmaz ×**20** + sanal oyuncu fazla hamlesi ×1,5 + takılma ×10.
- **Hedef eğrisi (Orta / Zor / Çok zor):** 31–60: 20→24 / 22,5→26,5 / 25,5→28,5 · 61–80: 24→26 / 26,5→28,5 / 28,5→30,5 · 81–100: 26,5→29,5 / 29→32 / 31→34.
- **Yöntem (değişiklik gerekirse):** önce test (`V32*Tests`), sonra kurucu; önizleme CSV'si ile kontrol; `GenerateLevelDataBatch`; tam test; veri dosyasını öncekiyle satır satır karşılaştır (yalnızca hedeflenen bölümler değişmeli — 59 düzeltmesinde ilk deneme 49/50'yi de değiştirmişti); Editor testi; telefon testi.
- **İnsan verisi (2026-10-15):** tuzak bölümleri (1 boş) düşünme süresi uzun ama fazla hamle az; planlama bölümleri (2 boş) fazla hamle 2–6. 81+ (5 renk) belirgin daha uzun sürüyor; kullanıcı "81 ve sonrası öncekilere göre epey zorlayıcı". Finaller (60, 100) insan için kolay kaldı.

## 8. Build, test ve log notları
- Build öncesi Unity Editor kapalı. Ortam: `GRADLE_USER_HOME=C:\Dev\gradle-cloudora`, `CLOUDORA_DEV_APP_ID=com.yigityvz.cloudora.v3`, `CLOUDORA_DEV_PRODUCT_NAME="Cloudora v3"`, `CLOUDORA_BUILD_OUTPUT=Builds/Android/Cloudora-v3-dev.apk` → `./Build-Android.ps1 -Mode dev -ProjectPath <proje>` (arka planda, ~3 dk). Editor menüsünden derlenirse paket v1'in üzerine gider.
- adb: `...\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools\adb.exe`. Kurulum `adb install -r`. Telefon kaydı: `/sdcard/Android/data/com.yigityvz.cloudora.v3/files/cloudora-save.json` (+`.bak`): uygulamayı `am force-stop` ile durdur, `pull` → düzenle (`currentLevel`, `highestCompletedLevel`, `failRecord` temizle) → ikisine de `push` → `monkey -p … 1` ile başlat.
- **logcat:** `adb logcat -v time -s Unity:V` arka planda dosyaya. **Unity batch çalışması adb sunucusunu yeniden başlatır ve logcat yakalamayı keser** → batch'ten sonra yeniden başlat.
- Telefon PC'den yaklaşık 10 kat yavaş (Shuffle ölçümü): PC'de 100 ms ≈ telefonda 1 sn.
- Editor kaydı: `%USERPROFILE%\AppData\LocalLow\YigitYvz\Cloudora\cloudora-save.json` (+ `.bak`); değiştirmeden önce `yedek-*` klasörüne yedekle. Telefon kaydı yedekleri de burada (`yedek-telefon-*`). **Scratchpad geçicidir**, kalıcı yedekleri oraya koyma.
- **Loglar (git dışı, `Logs/`):** `Logs/oyun-testleri/`: `2026-10-15-1911-editor-61-100.log`, `2026-10-15-1206-telefon-16-31-eski-plan.txt` (yeniden tasarım öncesi plan), `2026-10-15-2017-telefon-16-83-donma.txt`, `2026-10-15-2046-telefon-83-87-duzeltme.txt`, `2026-10-15-2038-shuffle-yuk-olcumu-eski-kod.csv`, `v32-v3-preview-*.csv`, `v32-v3-map-data.json`. 16–30 ve 31–60 Editor testlerinin ham logları üzerine yazıldı (analizleri `ClaudoraRelease.md` 17:56 ve 18:42'de). `Logs/araclar/`: `make_map.py` (veri → `Logs/v3-map-data.json`), `make_html.py` (→ bölüm haritası), `update_sablon.py` (→ şablon tablosu), `fix59.py` (uygulanmış yama, tarihçe).
- Play Mode'dayken kod değiştirme.
- Commit dışı bırakılanlar: `ProjectSettings.asset` (build'in yazdığı keystore alanı + boşluk farkları), `.vscode/`, `Docs/Cloudora 1–100 Şablonu.html`.

## 9. Açık konular ve bilinen tutarsızlıklar
- **Final ve ölçü (tasarım §15, kullanıcı "ileride yaparız"):** takılma ölçüsü dikkatli insanı tahmin etmiyor; 60/100 kolay, 59/78/83/90 zorladı. Faz 3'te oyun kayıtları ölçülerle karşılaştırılacak; alternatif: final adaylarını kullanıcı seçer.
- **Faz 2.6 kalanı (tasarım §15):** büyük tahtada gerçekten çözülemeyen bir Shuffle dizilişi hâlâ 200.000 duruma kadar arar (ölçümde görülmedi); gerekirse toplam bütçe ya da aramayı karelere bölme.
- **Tahta ölçeği (tasarım §15):** 7 bulutta (81+) ve Extra alınınca tahta küçülüyor; kullanıcı "uzun vadede değiştirelim".
- **Dünya listesi (Faz 5.1):** ekranda eski adlar (31–40 Rainy Coast, 41–60 Frost Peaks "Frozen", 61–80 sun-desert, 81–100 storm-islands). Dünya kimliği iki yerde: `WorldCatalog` ve `LevelGenerator.WorldId`. Katalog değişince kırılabilecek testler: `WorldProgressionCrossesBoundaryAndRestoresInStages`, `DifficultyWaveHasFinaleAndEasyWorldReset`, `CatalogUsesV3BoardsAndKeepsTutorialFrame`.
- **101+ sıçraması:** 101 eski sistemde, Rainbow modifier'lı, zorluk puanı 68,5 (100: 34).
- **Monotonluk:** 5 renkte en kısa çözüm 14–20 hamle; izlenecek.
- **Kullanıcının tartışmak istediği:** World ekranının kapsamı ve ortaya dokununca kapanması; Shuffle seçim modu daha belirgin olmalı.
- **Diğer açıklar (tasarım §14–15):** öğretici yazıların görünürlüğü, coin sayıları/başlangıç coini, reklamla can almanın yeri ("AD +1"), menüden "bölümü baştan başlat", zorluk sembolü (sprite), tema adları/özellikleri, Şimşek (6. renk) 1–100'de yok.
- **Zararsız kalıntılar:** `LevelCandidateSelector`'daki "Shuffle yedeği" (`backupCount`); `LevelDefinition.difficultyBand` (okunmuyor); `BalanceProfile.DurationTarget` (yalnızca testte).
- **Test notu:** buton denemeleri 1–15'te; o bölümlerin verisi zorluk analizine katılmaz.

## 10. Bu dosyanın bakımı
Önemli bir karar, yeni bir kaynak doküman, parça tamamlanması ya da oturum kapanışında bu dosya (özellikle §0) güncellenir ve değişiklik `ClaudoraRelease.md`'ye yazılır.
