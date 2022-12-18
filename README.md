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
<tr> <td> Kelias iki metodo </td> <td> /api/Orders </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Admin </td></tr>  
<tr> <td> Paskirtis </td> <td> Gauti visus užsakymus  </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> „Authorization“:“ Bearer {access_token}" </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> 

```json
[
 {
    "id": 0,
    "status": 0,
    "dateCreated": "2022-12-18T14:03:11.310Z",
    "dateEditted": "2022-12-18T14:03:11.310Z",
    "total": 0,
    "subtotal": 0
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
<tr> <td> Kelias iki metodo </td> <td> /api/Orders/{id} </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Admin, prisijungęs </td></tr>  
<tr> <td> Paskirtis </td> <td> Gauti konkretų užsakymą  </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> „Authorization“:“ Bearer {access_token}" </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> 

``` json
{
  "id": 0,
  "status": 0,
  "dateCreated": "2022-12-18T14:03:34.658Z",
  "dateEditted": "2022-12-18T14:03:34.658Z",
  "total": 0,
  "subtotal": 0
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


<table>
<tr> <td> <b> Sukurti užsakymą </b> </td> <td> </td></tr> 
<tr> <td> API metodas </td> <td> POST </td></tr>  
<tr> <td> Kelias iki metodo </td> <td> /api/Orders </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Admin, prisijungęs</td></tr>  
<tr> <td> Paskirtis </td> <td> Sukurti naują užsakymą  </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> „Authorization“:“ Bearer {access_token}" </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo kodas</td> <td> 201 (OK) </td></tr> 
<tr> <td> Galimi klaidų kodai </td> <td> 401 - neautorizuotas </td></tr> 
<tr> <td> Užklausos pavyzdys </td> <td> https://knygastoreapi.azurewebsites.net/api/Orders </td></tr> 
<tr> <td> Gauto atsakymo pavyzdys </td> <td> - </td></tr>
</table>

<table>
<tr> <td> <b> Redaguoti užsakymą </b> </td> <td> </td></tr> 
<tr> <td> API metodas </td> <td> PUT </td></tr>  
<tr> <td> Kelias iki metodo </td> <td> /api/Orders/{id} </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Admin, prisijungęs</td></tr>  
<tr> <td> Paskirtis </td> <td> Redaguoti užsakymą  </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> „Authorization“:“ Bearer {access_token}" </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> 

```json
{
  "status": 0
}
```

</td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo kodas</td> <td> 204 (OK) </td></tr> 
<tr> <td> Galimi klaidų kodai </td> <td> 401 - neautorizuotas, 404 - nerastas </td></tr> 
<tr> <td> Užklausos pavyzdys </td> <td> https://knygastoreapi.azurewebsites.net/api/Orders/5 </td></tr> 
<tr> <td> Gauto atsakymo pavyzdys </td> <td> - </td></tr>
</table>


<table>
<tr> <td> <b> Ištrinti užsakymą </b> </td> <td> </td></tr> 
<tr> <td> API metodas </td> <td> DELETE </td></tr>  
<tr> <td> Kelias iki metodo </td> <td> /api/Orders/{id} </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Admin, prisijungęs </td></tr>  
<tr> <td> Paskirtis </td> <td> Ištrinti užsakymą  </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> „Authorization“:“ Bearer {access_token}" </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo kodas</td> <td> 200 (OK) </td></tr> 
<tr> <td> Galimi klaidų kodai </td> <td> 401 - neautorizuotas, 403 - uždrausta, 404 - nerastas </td></tr> 
<tr> <td> Užklausos pavyzdys </td> <td> https://knygastoreapi.azurewebsites.net/api/Orders/4 </td></tr> 
<tr> <td> Gauto atsakymo pavyzdys </td> <td> - </td></tr>
</table>

- Produktų CRUD


<table>
<tr> <td> <b> Gauti produktus </b> </td> <td> </td></tr> 
<tr> <td> API metodas </td> <td> GET </td></tr>  
<tr> <td> Kelias iki metodo </td> <td> /api/Products </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Visi </td></tr>  
<tr> <td> Paskirtis </td> <td> Gauti visus produktus  </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> - </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> 

```json
[
	{
	  "id": 0,
	  "price": 0,
	  "title": "string",
	  "type": "string",
	  "description": "string",
	  "quantity": 0,
	  "canBeBought": true,
	  "isDisplayed": true,
	  "creatorId": "string"
	},
	{
	  "id": 0,
	  "price": 0,
	  "title": "string",
	  "type": "string",
	  "description": "string",
	  "quantity": 0,
	  "canBeBought": true,
	  "isDisplayed": true,
	  "creatorId": "string"
	},
]
```

</td></tr>  
<tr> <td> Atsakymo kodas</td> <td> 200 (OK) </td></tr> 
<tr> <td> Galimi klaidų kodai </td> <td> - </td></tr> 
<tr> <td> Užklausos pavyzdys </td> <td> https://knygastoreapi.azurewebsites.net/api/Products</td></tr> 
<tr> <td> Gauto atsakymo pavyzdys </td> <td> 

```json
[
  {
    "id": 1,
    "price": 14,
    "title": "Pavadinimas1",
    "type": "Tipas1",
    "description": "Aprasymas1",
    "quantity": 2,
    "canBeBought": true,
    "isDisplayed": true,
    "creatorId": "9b039d80-ed62-4a28-9940-04d611340694"
  },
  {
    "id": 2,
    "price": 20,
    "title": "Pavadinimas2",
    "type": "Tipas2",
    "description": "Aprasymas2",
    "quantity": 6,
    "canBeBought": true,
    "isDisplayed": true,
    "creatorId": "9b039d80-ed62-4a28-9940-04d611340694"
  },
] 
```

</td></tr>
</table>

<table>
<tr> <td> <b> Gauti produktą </b> </td> <td> </td></tr> 
<tr> <td> API metodas </td> <td> GET </td></tr>  
<tr> <td> Kelias iki metodo </td> <td> /api/Products/{id} </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Visi </td></tr>  
<tr> <td> Paskirtis </td> <td> Gauti vieną produktą  </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> - </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> 

```json
{
  "id": 0,
  "price": 0,
  "title": "string",
  "type": "string",
  "description": "string",
  "quantity": 0,
  "canBeBought": true,
  "isDisplayed": true,
  "creatorId": "string"
}
```

</td></tr>  
<tr> <td> Atsakymo kodas</td> <td> 200 (OK) </td></tr> 
<tr> <td> Galimi klaidų kodai </td> <td> 404 - nerastas </td></tr> 
<tr> <td> Užklausos pavyzdys </td> <td> https://knygastoreapi.azurewebsites.net/api/Products/2 </td></tr> 
<tr> <td> Gauto atsakymo pavyzdys </td> <td> 

```json
{
  "id": 1,
  "price": 14,
  "title": "Pavadinimas1",
  "type": "Tipas1",
  "description": "Aprasymas1",
  "quantity": 2,
  "canBeBought": true,
  "isDisplayed": true,
  "creatorId": "9b039d80-ed62-4a28-9940-04d611340694"
}

```

</td></tr>
</table>

<table>
<tr> <td> <b> Sukurti produktą </b> </td> <td> </td></tr> 
<tr> <td> API metodas </td> <td> POST </td></tr>  
<tr> <td> Kelias iki metodo </td> <td> /api/Products </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Admin </td></tr>  
<tr> <td> Paskirtis </td> <td> Sukurti naują produktą  </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> „Authorization“:“ Bearer {access_token}" </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> 

```json
{
  "price": 0,
  "title": "string",
  "type": "string",
  "description": "string",
  "quantity": 0,
  "canBeBought": true,
  "isDisplayed": true
}
```

</td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo kodas</td> <td> 201 (OK) </td></tr> 
<tr> <td> Galimi klaidų kodai </td> <td> 401 - neautorizuotas, 400 - bloga užklausa </td></tr> 
<tr> <td> Užklausos pavyzdys </td> <td> https://knygastoreapi.azurewebsites.net/api/Products </td></tr> 
<tr> <td> Gauto atsakymo pavyzdys </td> <td> - </td></tr>
</table>

<table>
<tr> <td> <b> Redaguoti produktą </b> </td> <td> </td></tr> 
<tr> <td> API metodas </td> <td> PUT </td></tr>  
<tr> <td> Kelias iki metodo </td> <td> /api/Products/{id} </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Admin </td></tr>  
<tr> <td> Paskirtis </td> <td> Redaguoti produktą  </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> „Authorization“:“ Bearer {access_token}" </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> 

```json
[
 {
    "id": 0,
    "status": 0,
    "dateCreated": "2022-12-18T14:03:11.310Z",
    "dateEditted": "2022-12-18T14:03:11.310Z",
    "total": 0,
    "subtotal": 0
  }
]
```

 </td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> - </td> </tr> 
<tr> <td> Atsakymo kodas</td> <td> 204 (OK) </td></tr> 
<tr> <td> Galimi klaidų kodai </td> <td> 401 - neautorizuotas, 404 - nerastas </td></tr> 
<tr> <td> Užklausos pavyzdys </td> <td> https://knygastoreapi.azurewebsites.net/api/Products/2 </td></tr> 
<tr> <td> Gauto atsakymo pavyzdys </td> <td> - </td></tr>
</table>

<table>
<tr> <td> <b> Ištrinti produktą </b> </td> <td> </td></tr> 
<tr> <td> API metodas </td> <td> DELETE </td></tr>  
<tr> <td> Kelias iki metodo </td> <td> /api/Products/{id} </td></tr>  
<tr> <td> Vartotojai, galintys Ištrinti produktą </td><td> Admin, prisijungęs vartotojas </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> „Authorization“:“ Bearer {access_token}" </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo kodas</td> <td> 204 (OK) </td></tr> 
<tr> <td> Galimi klaidų kodai </td> <td> 401 - neautorizuotas. 404 - nerastas </td></tr> 
<tr> <td> Užklausos pavyzdys </td> <td> https://knygastoreapi.azurewebsites.net/api/Products/2  </td></tr> 
<tr> <td> Gauto atsakymo pavyzdys </td> <td> - </td></tr>
</table>

<table>
<tr> <td> <b> Gauti užsakymo produktus </b> </td> <td> </td></tr> 
<tr> <td> API metodas </td> <td> GET </td></tr>  
<tr> <td> Kelias iki metodo </td> <td> /api/Orders/{orderId}/Products </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Admin, prisijungęs vartotojas </td></tr>  
<tr> <td> Paskirtis </td> <td> Gauti visus produktus, kurie yra pridėti prie užsakymo  </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> „Authorization“:“ Bearer {access_token}" </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> 

```json
[
 {
    "id": 0,
    "price": 0,
    "title": "string",
    "type": "string",
    "description": "string",
    "quantity": 0,
    "canBeBought": true,
    "isDisplayed": true,
    "creatorId": "string"
  }
]
```

</td></tr>  
<tr> <td> Atsakymo kodas</td> <td> 200 (OK) </td></tr> 
<tr> <td> Galimi klaidų kodai </td> <td> 401 - neautorizuotas, 404 - nerastas </td></tr> 
<tr> <td> Užklausos pavyzdys </td> <td> https://knygastoreapi.azurewebsites.net/api/Orders/3/Products </td></tr> 
<tr> <td> Gauto atsakymo pavyzdys </td> <td> 

```json
[
  {
    "id": 102,
    "price": 10,
    "title": "string",
    "type": "string",
    "description": "string",
    "quantity": 10,
    "canBeBought": false,
    "isDisplayed": false,
    "creatorId": "9b039d80-ed62-4a28-9940-04d611340694"
  },
  {
    "id": 103,
    "price": 15,
    "title": "string",
    "type": "string",
    "description": "string",
    "quantity": 10,
    "canBeBought": false,
    "isDisplayed": false,
    "creatorId": "9b039d80-ed62-4a28-9940-04d611340694"
  }
]
```

</td></tr>
</table>

<table>
<tr> <td> <b> Gauti užsakymo produktą </b> </td> <td> </td></tr> 
<tr> <td> API metodas </td> <td> GET </td></tr>  
<tr> <td> Kelias iki metodo </td> <td> /api/Orders/{orderId}/Product/{productId} </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Admin, prisijungęs naudotojas </td></tr>  
<tr> <td> Paskirtis </td> <td> Gauti produktą, kuris yra pridėtas prie užsakymo  </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> „Authorization“:“ Bearer {access_token}" </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> 

```json
{
  "id": 0,
  "price": 0,
  "title": "string",
  "type": "string",
  "description": "string",
  "quantity": 0,
  "canBeBought": true,
  "isDisplayed": true,
  "creatorId": "string"
}
```

</td></tr>  
<tr> <td> Atsakymo kodas</td> <td> 200 (OK) </td></tr> 
<tr> <td> Galimi klaidų kodai </td> <td> 401 - neautorizuotas, 404 - nerastas </td></tr> 
<tr> <td> Užklausos pavyzdys </td> <td> https://knygastoreapi.azurewebsites.net/api/Orders/1/Products/1  </td></tr> 
<tr> <td> Gauto atsakymo pavyzdys </td> <td> 

```json
{
  "id": 103,
  "price": 15,
  "title": "string",
  "type": "string",
  "description": "string",
  "quantity": 10,
  "canBeBought": false,
  "isDisplayed": false,
  "creatorId": "9b039d80-ed62-4a28-9940-04d611340694"
}
```

</td></tr>
</table>

<table>
<tr> <td> <b> Sukurti užsakymo produktą </b> </td> <td> </td></tr> 
<tr> <td> API metodas </td> <td> POST </td></tr>  
<tr> <td> Kelias iki metodo </td> <td> /api/Orders/{id}/Products </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Admin, prisijungęs naudotojas  </td></tr>  
<tr> <td> Paskirtis </td> <td> Gauti naują produktą per konkretų užsakymą  </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> „Authorization“:“ Bearer {access_token}" </td></tr> 
<tr> <td> Užklausos struktūra </td> <td>

```json
{
  "price": 0,
  "title": "string",
  "type": "string",
  "description": "string",
  "quantity": 0,
  "canBeBought": true,
  "isDisplayed": true
}
```

</td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo kodas</td> <td> 201 (OK) </td></tr> 
<tr> <td> Galimi klaidų kodai </td> <td> 401 - neautorizuotas, 404 - nerastas, 400 - bloga užklausa </td></tr> 
<tr> <td> Užklausos pavyzdys </td> <td> https://knygastoreapi.azurewebsites.net/api/Orders/1/Products/ </td></tr> 
<tr> <td> Gauto atsakymo pavyzdys </td> <td> - </td></tr>
</table>

<table>
<tr> <td> <b> Redaguoti užsakymo produktą </b> </td> <td> </td></tr> 
<tr> <td> API metodas </td> <td> PUT </td></tr>  
<tr> <td> Kelias iki metodo </td> <td> /api/Orders/{id}/Products/{id} </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Admin, prisijungęs naudotojas  </td></tr>  
<tr> <td> Paskirtis </td> <td> Redaguoti produktą, kuris yra pridėtas prie užsakymo. Produktas gali būti išimamas iš užsakymo paduodant parametrą remove=true </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> „Authorization“:“ Bearer {access_token}" </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> 

```json
{
  "price": 0,
  "title": "string",
  "type": "string",
  "description": "string",
  "quantity": 0,
  "canBeBought": true,
  "isDisplayed": true
}
``` 

</td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo kodas</td> <td> 204 (OK) </td></tr> 
<tr> <td> Galimi klaidų kodai </td> <td> 401 - neautorizuotas, 404 - nerastas, 400 - bloga užklausa  </td></tr> 
<tr> <td> Užklausos pavyzdys </td> <td> https://knygastoreapi.azurewebsites.net/api/Orders/1/Products/1 </td></tr> 
<tr> <td> Gauto atsakymo pavyzdys </td> <td> - </td></tr>
</table>

<table>
<tr> <td> <b> Ištrinti užsakymo produktą </b> </td> <td> </td></tr> 
<tr> <td> API metodas </td> <td> DELETE </td></tr>  
<tr> <td> Kelias iki metodo </td> <td> /api/Orders/{id}/Products/{id} </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Admin, prisijungęs naudotojas  </td></tr>  
<tr> <td> Paskirtis </td> <td> Ištrinti produktą, kuris yra pridėtas prie užsakymo  </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> „Authorization“:“ Bearer {access_token}" </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo kodas</td> <td> 204 (OK) </td></tr> 
<tr> <td> Galimi klaidų kodai </td> <td> 401 - neautorizuotas, 404 - nerastas </td></tr> 
<tr> <td> Užklausos pavyzdys </td> <td> https://knygastoreapi.azurewebsites.net/api/Orders/1/Products/1 </td></tr> 
<tr> <td> Gauto atsakymo pavyzdys </td> <td> - </td></tr>
</table>

<table>
<tr> <td> <b> Pridėti prie užsakymo produktą </b> </td> <td> </td></tr> 
<tr> <td> API metodas </td> <td> PATCH </td></tr>  
<tr> <td> Kelias iki metodo </td> <td> /api/Orders/{id}/Products/{id} </td></tr>  
<tr> <td> Vartotojai, galintys pasiekti </td> <td> Admin, prisijungęs naudotojas  </td></tr>  
<tr> <td> Paskirtis </td> <td> Pridėtiegzistuojantį produktą prie užsakymo  </td></tr>  
<tr> <td> Užklausos "Header" dalis </td> <td> „Authorization“:“ Bearer {access_token}" </td></tr> 
<tr> <td> Užklausos struktūra </td> <td> - </td></tr>  
<tr> <td> Atsakymo struktūra </td> <td> 

```json
[
 {
    "id": 0,
    "status": 0,
    "dateCreated": "2022-12-18T14:03:11.310Z",
    "dateEditted": "2022-12-18T14:03:11.310Z",
    "total": 0,
    "subtotal": 0
  }
]
```

</td></tr>  
<tr> <td> Atsakymo kodas</td> <td> 204 (OK) </td></tr> 
<tr> <td> Galimi klaidų kodai </td> <td> 401 - neautorizuotas, 404 - nerastas  </td></tr> 
<tr> <td> Užklausos pavyzdys </td> <td> https://knygastoreapi.azurewebsites.net/api/Orders/1/Products/1 </td></tr> 
<tr> <td> Gauto atsakymo pavyzdys </td> <td> - </td></tr>
</table>
