# ChatDB™  
Druga seminarska naloga pri predmetu Informacijski Sistemi UNI-LJ FRI  
http://chatdb.azurewebsites.net/

* **UN:** leonmakoric   **PW:** leonmakoricAA!!11
* **UN:** lenartbezek   **PW:** lenartbezekAA!!11

## Delitev nalog
**Lenart Bezek (63130012)**:
* Osnova [NoDB](https://github.com/lench4991/NoDB)
* Uporabniški vmesnik
* Login in registracija

**Leon Makorič (63120008)**:
* Vzpostavitev podatkovne baze
* Branje in shranjevanje sporočil

## Dodatne funkcionalnosti
* Beleženje in prikaz časa sporočil
* Responsive mobile-first design
* Memory cache za podatke o uporabnikih
* Gostovanje na Azure oblaku

## Premagane težave
Najprej sva imela težave z povezavo na lokalno podatkovno bazo. Za hranjenje zaporedja sporočil v podatkovni bazi sva implementirala tudi čas, kar je prineslo težave s časovnimi pasovi. To sva rešila z UTC formatom.

## Predlagane izboljšave
Pošiljanje sporočil preko postbacka je potratno in zahteva osveževanje s strani uporabnika. To bi se rešilo z uporabo WebSocket protokola, obenem pa precej zmanjšalo obremenitev strežnika in povezave. Po temu bi bilo precej enostavno implementirati tudi brisanje in urejanje sporočil.

![screenshot](https://puu.sh/srW5e/b772ad8f4a.png)

