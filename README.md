# Cloudora iOS

**Gökyüzünü düzelt. Dünyayı yeniden canlandır.**

Cloudora, iPhone için Unity ile geliştirilmiş, dikey ekranda oynanan bir hava olaylarını sıralama bulmacasıdır. Bu depo iOS uygulamasının kaynak kodunu, oyun içeriklerini ve Xcode derleme araçlarını içerir.

## Gereksinimler

- macOS ve Xcode
- Unity **6000.5.9f1**, **iOS Build Support** modülüyle
- Git LFS
- Cihazda çalıştırmak için yerel Apple geliştirme imzası ve Geliştirici Modu açık bir iPhone

Hedef: **iOS 15.0+**, iPhone, dikey ekran, **IL2CPP / ARM64 / Metal**. Bundle ID: `com.yigityvz.cloudora`.

## Kurulum

```sh
git clone https://github.com/yigityvz/cloudora-ios.git
cd cloudora-ios
git lfs install --local
git lfs pull
```

Projeyi Unity Hub üzerinden açın. Ana sahne: `Assets/_Cloudora/Scenes/02_Gameplay.unity`.

## Test ve derleme

Aynı projeyi açan Unity oturumunu kapatıp depo kökünde çalıştırın:

```sh
./Build-iOS.sh test
./Build-iOS.sh build
```

Oluşan `Builds/iOS/Cloudora/Unity-iPhone.xcodeproj` dosyasını Xcode ile açın. **Unity-iPhone → Signing & Capabilities** bölümünden kendi Team'inizi seçin, iPhone'u bağlayıp **Run** ile çalıştırın. Mevcut export klasörü doluysa yeniden üretmeden önce başka bir konuma taşıyın.

Ayrıntılar: [iOS derleme rehberi](Docs/IOS_BUILD.md) · [Doğrulama durumu](Docs/IOS_STATUS.md) · [Cihaz testleri](Docs/IOS_DEVICE_QA.md).

## Oyun ve proje yapısı

- İlk 100 bölüm sabit bölüm verileri kullanır; 101 ve sonrası mevcut üreticiyle oluşturulur.
- Bulutlar arasında uyumlu hava olayları dokunarak taşınır. Bölümler, zorluk, can, jeton ve yardımcı mekanikleri mevcut oyun sürümüyle korunur.
- İlerleme ve ayarlar yerel JSON kaydında tutulur; yedek kurtarma desteklenir.
- `Assets/_Cloudora/Scripts`: oyun kuralları, arayüz ve servisler.
- `Assets/_Cloudora/Editor`: iOS derleme ve bölüm araçları.
- `Assets/_Cloudora/Tests`: EditMode testleri.
- `Docs`: iOS rehberleri ve bölüm tasarım belgeleri.

Derleme çıktıları, cihaz kayıtları ve imzalama bilgileri Git'e eklenmez. Geliştirme derlemesinde reklam servisi test uygulamasıdır; App Store dağıtımı ayrıca hazırlanmalıdır.

## Katkıda bulunanlar

- [@yigityvz](https://github.com/yigityvz)
- [@bahadir-aktas](https://github.com/bahadir-aktas)
