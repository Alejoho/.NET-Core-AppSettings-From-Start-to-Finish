namespace BasicDemo.Options
{
    public class EmailSettingsOptions
    {
        public bool EnableSystem { get; set; }
        public int TimeOutInSeconds { get; set; }
        public List<string> Servers { get; set; } = [];
        public AdminOptions Admin { get; set; } = new();
    }
}
