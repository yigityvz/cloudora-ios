# iOS aktarım durumu — 2026-10-16

Kaynak: `yigityvz/cloudora-android`, `cloudora-bahadir`, `d7c9416` (v3.2 + belge kayıtları). Hedef: `yigityvz/cloudora-ios`, `main`.

## Korunan kapsam
Runtime C# kodu, UI/oynanış kodu, kayıt/jeton/başarısızlık/Shuffle davranışları, sahneler, prefablar, bölüm verileri, görseller, oyun kaynak JSON'ları kaynak sürümle aynıdır. Figma/Stitch tasarımları veya konuşulan yeni oyun kuralları uygulanmadı.

Önceki iOS portunun native haptic adaptörü ve safe-area-only layout yaması taşınmadı; kaynak Android runtime bütünüyle esas alındı. Unity `Handheld.Vibrate` çağrısı korunuyor.

## iOS'a özel farklar
- Platform temizliğinde Android Editor araçları, manifest testi, build profili, Google Play yapılandırması ve kullanılmayan JNI modülü kaldırıldı.
- iOS build aracı ve shell betiği: iPhone, portrait, ARM64/IL2CPP, Metal, iOS 15, mevcut ikon, `com.yigityvz.cloudora`. Ürün adı/versiyon kaynak ayarlarından korunur.
- iOS PlayerSettings; yerel Team/profil/sertifika depoya eklenmez.
- Build belgeleri ve iOS artifact ignore kuralları.

## Doğrulama
| Kontrol | Sonuç | Kanıt |
|---|---|---|
| Oyun runtime ve içerik eşliği | PASS | Scripts, Scenes, Art ve Prefabs için temizlik öncesine göre Git diff boş |
| EditMode, iOS hedefi | PASS | Temizlik sonrası 195 toplam, 191 geçti, 4 bilinçli atlama, 0 başarısız; Logs/ios-editmode.xml |
| 1–100 kalibrasyonu | PASS | 100 ayrı bölüm raporu; Logs/ios-v32-calibration.log; veri yeniden üretilmedi |
| Git LFS bütünlüğü | PASS | 6 tarihsel nesne; fsck OK |
| Unity Xcode export | PASS | 0 hata, 5 ikon sıkıştırma uyarısı; Logs/ios-build.log |
| Native iOS ARM64 derleme | PASS | Xcode BUILD SUCCEEDED, CODE_SIGNING_ALLOWED=NO; Logs/ios-v32-xcode-build.log |
| Güncel sürüm imzalama | PASS | Logs/ios-v32-signed-build.log; yerel Team, BUILD SUCCEEDED |
| iPhone SE kurulumu ve açılış | PASS | Logs/ios-v32-device-install.log / ios-v32-device-launch.log; çalışan süreç doğrulandı |
| Fiziksel oynanış | MANUAL TEST IN PROGRESS | Kullanıcı kısa testi yaptığını bildirdi; kayıt bölüm 10'a ilerlemişti. İsteğiyle yedek alınarak test kaydı 80'e getirildi |
| App Store yayını | NOT RUN | Bu çalışma yalnızca platform aktarımıdır |

Native ürün: `/private/tmp/cloudora-ios-bahadir-20261016/Cloudora.app` (geçici; 2026-10-16 cihaz testi için yerel olarak imzalandı). Bundle `com.yigityvz.cloudora`, version `0.1.0`, build `1`, minimum iOS `15.0`. Unity projesi ve Xcode export kalıcı çalışma kopyasında; geçici ürünü macOS silebilir.

Mevcut 512px ikonun sıkıştırma uyarıları ve Unity'nin ürettiği IL2CPP komutlarındaki eski minimum-OS uyarıları korunmuştur; derleme başarılıdır. Bunları gidermek için sanat veya oyun kaynakları değiştirilmedi. Unity'nin mevcut paket sürümü override uyarıları nedeniyle manifest/lock yeniden pinlenmedi.

Fiziksel testte yeni v3.2 sürümüyle normal hamleler, tek bulut Shuffle, Undo, Extra, Retry, 40 coin/reklamla son çözülebilir duruma dönüş ve başarısızlık kaydının kapat-aç davranışı doğrulanmalıdır. Android dalında bilinen sınırlamalar bu portta düzeltilmedi.

Cihaz testi başlangıcı: 2026-10-16 11:32 +03. Kurulumdan önce mevcut uygulamanın Documents klasörü yerel/ignore edilen Logs altında yedeklendi (bölüm 5, tamamlanan 4, can 5). Uygulama silinmeden güncellendi; kayıt değiştirilmedi. İmzalama bilgileri depoya eklenmedi.

16 Ekim depo temizliği sonrası EditMode testleri tekrar geçti. Önceki Xcode ve cihaz sonuçları temizlik öncesi derlemeye aittir. Unity ortak ayar şemalarındaki platform alanları ve oyun görselleri korunmuştur.
