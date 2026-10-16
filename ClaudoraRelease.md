# Cloudora iOS değişiklik kaydı

## 2026-10-16 — iOS depo temizliği

- README iOS kurulum, test ve Xcode akışı için yeniden yazıldı.
- Diğer platformun derleme betiği, Editor araçları, build profili, mağaza yapılandırması ve yayın belgeleri kaldırıldı.
- Yalnızca kaldırılan manifest aracına ait test çıkarıldı; oyun testleri korundu.
- Kullanılmayan JNI modülü kaldırıldı. Ortak oyun kodu, sahneler ve içerikler değişmedi.
- Önceki ayrıntılı geliştirme kayıtları Git geçmişinde korunur.

## 2026-10-16 — iPhone doğrulaması

- Unity export, yerel imzalı Xcode derlemesi, iPhone kurulumu ve başlatma başarılı.
- Kullanıcı ilk kısa test adımlarını yaptığını bildirdi; isteğiyle yedek alınarak cihaz kaydı 80. bölüme getirildi.
- Tam cihaz kabul testi henüz tamamlanmadı.

## 2026-10-16 — mevcut oyunun iOS aktarımı

- Kaynak sürüm d7c9416 temel alınarak oyun davranışı korundu.
- Aktarımda 192 EditMode testi geçti, 4 test atlandı, hata yoktu. İlk 100 bölümün salt okunur kalibrasyonu tamamlandı.
