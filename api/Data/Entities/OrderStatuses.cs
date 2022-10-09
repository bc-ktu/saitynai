namespace api.Data.Entities
{
    public enum OrderStatuses
    {
        Sukurtas,
        Pateiktas,
        Peržiūrimas,
        Atnaujintas,
        Patvirtintas,
        Padarytas, // uzsakymas atliktas, taciau nepristatytas uzsakovui
        Išsiųstas,
        Atsiimtas,
        Atliktas,
        Atšauktas
    }
}
