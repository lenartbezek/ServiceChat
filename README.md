# ServiceChat™  
Tretja seminarska naloga pri predmetu Informacijski Sistemi UNI-LJ FRI.  

**Spletna aplikacija**:  
http://is-servicechat.azurewebsites.net/  
**REST API**:   
http://is-servicechat.azurewebsites.net/api  

**UN:** admin   **PW:** Geslo.01

## Delitev nalog
**Lenart Bezek (63130012)**:

* React spletni vmesnik
* WebAPI

**Leon Makorič (63120008)**:

* Android odjemalec

# Delovanje
IIS strežnik streže statično spletno stran, ki vsebuje React aplikacjo, s katero dosežemo veliko boljšo odzivnost kot z WebForms vmesnikom.
Zaradi predpomnenja uporabnikov na strežniku nam ni potrebno izvajati SQL poizvedbe za vsako avtentikacijo, 
kar precej pohitri veliko število zaporednih zahtevkov od enega uporabnika (20ms brez pinga).

Vse končne točke API vmesnika so zaščitene z avtentikacijo, razen točki `POST:/login` in `POST:/register`.
Poleg tega potrebujemo za točki `PUT:/users` in `DELETE:/users/{username}` tudi administratorske pravice.

Na spletnem vmesniku se auth-token shranjuje v `localStorage`, na Android odjemalcu pa v lokalno datoteko `authtoken.txt`.
To obema odjemalcema omogoči, da si zapomnita uporabnika in ga avtomatsko prijavita v naslednji seji.

Oba odjemalca avtomatsko osvežujeta sporočila.

### Predlagane izboljšave.
Delovanje spletne klepetalnice bi bilo treba preseliti na WebSocket protokol za obojestranske povezave.
Zaradi varnosti uporabnikov bi bilo treba sprejemati varne povezave preko https, saj lahko v trenutni situacijo gesla prestreže kdorkoli.

# Entitetno relacijski diagram
![ER diagram](http://i.imgur.com/AV90F0b.png)

# Spletna klepetalnica 
![Spletna klepetalnica](https://puu.sh/tkcWq/f5170d3471.png)

# Administratorska konzola
![Administratorska konzola](https://puu.sh/tkcSj/4716b9d184.png)

# Android odjemalec
![Android odjemalec](https://puu.sh/tkdgK/c05b93a795.png)

