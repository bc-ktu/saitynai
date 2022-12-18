EF migrations: dotnet ef migrations add UpdateEntity 
Database update: dotnet ef database update

---
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
![Untitled Diagram (1) drawio (1)](https://user-images.githubusercontent.com/113304150/190967359-bc15b160-b514-4b72-85bf-289c9443ed93.png)
<br />

**Pasirinktos technologijos:**
- Frontend dalis: React
- Backend dalis: .NET 6 + SQL Server

**API specifikacija**

- Užsakymų CRUD

<table>

<tr> <td> <b> Gauti užsakymus </b> </td> <td> </td></tr> 
<tr> <td> API metodas </td> <td> GET </td></tr>  
<tr> <td> Kelias iki metodo </td> <td> /Orders </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Admin </td></tr>  
<tr> <td> Paskirtis </td> <td> Gauti visus užsakymus  </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> „Authorization“:“ Bearer {access_token}" </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> 

```json
[
  {
    "id": ...,
    "status": ...,
    "dateCreated": ...,
    "dateEditted": ...,
    "total": ...,
    "subtotal": ...
  },
  {
    "id": ...,
    "status": ...,
    "dateCreated": ...,
    "dateEditted": ...,
    "total": ...,
    "subtotal": ...
  }
]
```

</td></tr>  
<tr> <td> Atsakymo kodas</td> <td> 200 (OK) </td></tr> 
<tr> <td> Galimi klaidų kodai </td> <td> 401 - neautorizuotas </td></tr> 
<tr> <td> Užklausos pavyzdys </td> <td> https://knygastoreapi.azurewebsites.net/api/Orders </td></tr> 
<tr> <td> Gauto atsakymo pavyzdys </td> <td> 

```json
[
  {
    "id": 1,
    "status": 0,
    "dateCreated": "2022-11-07T07:23:25.3122631",
    "dateEditted": "2022-11-07T07:24:26.6102921",
    "total": 20,
    "subtotal": 15
  },
  {
    "id": 2,
    "status": 0,
    "dateCreated": "2022-12-03T13:37:39.8529886",
    "dateEditted": null,
    "total": 0,
    "subtotal": 0
  }
]
```

</td></tr>
</table>

<table>
<tr> <td> <b> Gauti užsakymą </b> </td> <td> </td></tr> 
<tr> <td> API metodas </td> <td> GET </td></tr>  
<tr> <td> Kelias iki metodo </td> <td> /Orders/{id} </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Admin, prisijungęs </td></tr>  
<tr> <td> Paskirtis </td> <td> Gauti konkretų užsakymą  </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> „Authorization“:“ Bearer {access_token}" </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> 

``` json
{
  "id": ...,
  "status": ...,
  "dateCreated": ..,
  "dateEditted": ...,
  "total": ...,
  "subtotal": ...
}  
```

</td></tr>  
<tr> <td> Atsakymo kodas</td> <td> 200 (OK) </td></tr> 
<tr> <td> Galimi klaidų kodai </td> <td> 401 - neautorizuotas, 404 - nerastas </td></tr> 
<tr> <td> Užklausos pavyzdys </td> <td> https://knygastoreapi.azurewebsites.net/api/Orders/2 </td></tr> 
<tr> <td> Gauto atsakymo pavyzdys </td> <td> 

```json
{
  "id": 2, 
  "status": 0,  
  "dateCreated": "2022-12-03T13:37:39.8529886", 
  "dateEditted": null, 
  "total": 0, 
  "subtotal": 0   
} 
```

</td></tr>
</table>
