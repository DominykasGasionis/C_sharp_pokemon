namespace PokemonGame;

// Išsaugojimo sistema naudojanti Entity Framework Core su SQLite duomenų baze
public static class SaveSystem
{
    // Sukuria duomenų bazės lenteles jei jų dar nėra
    private static void EnsureDb()
    {
        using var db = new GameDbContext();
        db.Database.EnsureCreated();
    }

    // Tikrina ar yra išsaugotų duomenų duomenų bazėje
    public static bool SaveExists()
    {
        try
        {
            EnsureDb();
            using var db = new GameDbContext();
            return db.GameStates.Any();
        }
        catch
        {
            return false;
        }
    }

    // Išsaugo visą žaidimo būseną į SQLite duomenų bazę
    public static void Save(PokemonRoster roster, Player player, GameSettings settings, Inventory inventory)
    {
        EnsureDb();
        using var db = new GameDbContext();

        // Išvalome senus duomenis prieš rašant naujus
        db.Pokemon.RemoveRange(db.Pokemon);
        db.GameStates.RemoveRange(db.GameStates);
        db.SaveChanges();

        // Įrašome kiekvieną Pokemon su rūšiavimo indeksu tvarkai išlaikyti
        for (int i = 0; i < roster.All.Count; i++)
        {
            var p = roster.All[i];
            db.Pokemon.Add(new SavedPokemonEntity
            {
                Name       = p.Name,
                MaxHp      = p.MaxHp,
                Hp         = p.Hp,
                Attack     = p.Attack,
                Defense    = p.Defense,
                Level      = p.Level,
                Experience = p.Experience,
                XpReward   = p.XpReward,
                SortOrder  = i,
            });
        }

        // Įrašome žaidimo būseną (pozicija, nustatymai, inventorius)
        int[] indices = roster.GetPartyIndices();
        db.GameStates.Add(new SavedGameState
        {
            PlayerX         = player.X,
            PlayerY         = player.Y,
            EncounterChance = settings.EncounterChance,
            Pokeballs       = inventory.Pokeballs,
            Potions         = inventory.Potions,
            PartyIndices    = string.Join(",", indices),
        });

        db.SaveChanges();
    }

    // Nuskaito žaidimo būseną iš SQLite; grąžina null jei nėra išsaugojimo
    public static (PokemonRoster roster, Player player, GameSettings settings, Inventory inventory)? Load()
    {
        try
        {
            EnsureDb();
            using var db = new GameDbContext();

            var state = db.GameStates.FirstOrDefault();
            if (state == null) return null;

            // Nuskaitome Pokemon LINQ užklausa, išrikiuotus pagal įrašymo tvarką
            var all = db.Pokemon
                .OrderBy(p => p.SortOrder)
                .Select(e => new Pokemon(e.Name, e.MaxHp, e.Attack, e.Defense, e.Hp, e.Level, e.Experience, e.XpReward))
                .ToList();

            if (all.Count == 0) return null;

            // Party indeksai saugomi kaip "0,-1,-1" eilutė, konvertuojame atgal
            int[] indices = state.PartyIndices
                .Split(',')
                .Select(int.Parse)
                .ToArray();

            var roster    = new PokemonRoster(all, indices);
            var player    = new Player(state.PlayerX, state.PlayerY);
            var settings  = new GameSettings { EncounterChance = state.EncounterChance };
            var inventory = new Inventory { Pokeballs = state.Pokeballs, Potions = state.Potions };

            return (roster, player, settings, inventory);
        }
        catch
        {
            return null;
        }
    }

    // Ištrina visus išsaugotus duomenis iš duomenų bazės
    public static void Delete()
    {
        try
        {
            EnsureDb();
            using var db = new GameDbContext();
            db.Pokemon.RemoveRange(db.Pokemon);
            db.GameStates.RemoveRange(db.GameStates);
            db.SaveChanges();
        }
        catch { }
    }
}
