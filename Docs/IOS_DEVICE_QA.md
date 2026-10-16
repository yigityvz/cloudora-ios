# iPhone cihaz testleri

## 16 Ekim 2026 — yapılanlar

- iPhone SE üzerinde yerel imzalı uygulama kurulumu, açılış ve çalışan süreç doğrulandı.
- Mevcut kayıt yedeklendi, uygulama silinmeden güncellendi.
- Kullanıcı geçerli/geçersiz hamle, Undo ve 5→6 geçişini içeren kısa test isteğine “yaptım” yanıtını verdi. Okunan kayıt bölüm 10 / tamamlanan 9 idi.
- Kullanıcının isteğiyle yeni yedek alınıp cihaz test kaydı 80. bölüme getirildi; can ve jeton korundu. Bu değişiklik yalnızca cihaz kaydındadır.
- Tam test matrisi tamamlanmış değildir. Depo temizliği sonrası yeni Unity testleri `IOS_STATUS.md` içinde kaydedilir; cihazdaki oyun yeniden yüklenmedi.

## Kalan kontroller

- Küçük ekran, çentikli ve Dynamic Island iPhone'larda arayüz ve dokunma alanları.
- Tek bulut Shuffle (16. bölümden itibaren), Extra, Undo, başarısızlık ve Retry.
- 40 jeton/test reklamı ile son çözülebilir duruma dönüş; başarısızlık kaydının kapatıp açınca korunması.
- Arka plana alma, ekran kilidi, yeniden açılış, can yenilenmesi ve ayarların kalıcılığı.
- Titreşim, ses ayarı, uzun oturum performansı ve bellek kullanımı.
- İlk 100 bölümden sonraki mevcut modifier mekaniklerinin kontrolü.

Normal tamamlanmamış tahta yeniden açılışta baştan başlar; başarısızlık kaydı ayrı tutulur. Testte mevcut oyun davranışı esas alınır. Kullanıcı kayıtları ve cihaz kimlikleri Git'e eklenmez.
