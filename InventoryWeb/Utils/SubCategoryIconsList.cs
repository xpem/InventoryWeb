namespace InventoryWeb.Utils
{
    public static partial class SubCategoryIconsList
    {
        public static string GetIconCode(string name) => Icons.Where((s) => s.name == name).First().unicode;

        public static string GetIconName(string unicodde) => Icons.Where((s) => s.unicode == unicodde).First().name;

        /// <summary>
        ///to index icons for his name
        /// </summary>
        public static readonly IEnumerable<(string name, string unicode)> Icons = new List<(string, string)>()
        {
            new ("couch", "\uf4b8"),
            new ("computer", "\ue4e5"),
            new ("kitchen-set", "\ue51a"),
            new ("tv", "\uf26c"),
            new ("tag", "\uf02b"),
            new ("mobile", "\uf10b"),
            new ("book", "\uf02d"),
            new ("basketball", "\uf434"),
            new ("gamepad", "\uf11b"),
            new ("car", "\uf1b9"),
            new ("motorcycle", "\uf21c"),
            new ("wrench", "\uf0ad"),
            new ("screwdriver", "\uf54a"),
            new ("box", "\uf466"),
            new ("tshirt", "\uf553"),
            new ("plug", "\uf1e6"),
            new ("cube", "\uf1b2"),
            new ("shoe-prints", "\uf54b"),
        };
    }
}
