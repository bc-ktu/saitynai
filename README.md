# saitynai
T120B165 Saityno taikomųjų programų projektavimas - projektas

**Projektas**: Privati svetainė “Knyga” 

**Sistemos paskirtis**: padėti knygrišiui skaitmenizuotai valdyti užsakymus, gaminamus ir parduodamus produktus. 

**API metodai**: 
- Kurti/skaityti/keisti/trinti užsakymą ir peržiūrėti visus užsakymus 
- Kurti/keisti/trinti/peržiūrėti produktą ir peržiūrėti visus produktus 
- Kurti/keisti/trinti komentarą ir peržiūrėti visus komentarus 

Užsakymas <- Produktas <- Komentaras 

**Rolės**: svečias (neprisijungęs naudotojas), narys, administratorius

**Funkciniai reikalavimai:**
- Neprisijungę vartotojai gali peržiūrėti produktų puslapį, kiekvieno produkto puslapį bei perskaityti komentarus.
- Neprisijungę vartotojai gali teikti užklausą prisijungimui į sistemą.
- Nariai gali kurti užsakymą, į jį pridėti produktus, kurti naujus produktus (juos turi patvirtinti administratorius).
- Prisijungę naudotojai (nariai) gali palikti komentarus, juos redaguoti ir ištrinti.
- Administratorius patvirtina svečio registraciją, gali ištrinti komentarus, produktus ar narius. 
- Administratorius patvirtina užsakymą, jis gali keisti užsakymo būseną.
- Svetainėje nėra vykdomi apmokėjimai. 

**Sistemos architektūra:**
<br />
![Untitled Diagram (1) drawio](https://user-images.githubusercontent.com/113304150/190958590-7b68b32a-4362-4724-bf8a-cce9dc57d93a.png)
<br />

**Pasirinktos technologijos:**
- Frontend dalis: React
- Backend dalis: .NET 6 + SQL Server
