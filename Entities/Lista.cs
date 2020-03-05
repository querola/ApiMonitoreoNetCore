namespace monitoreoApiNetCore.Entities{
    public class Lista{
        public Lista(string file, System.DateTime creationTime, string extension, bool delay)
        {
            File = file;
            CreationTime = creationTime;
            Extension = extension;
            Delay = delay;
        }

        public string File { get; set; }
        public System.DateTime CreationTime { get; set; }
        public string Extension { get; set; }
        public bool Delay { get; set; }
    }
}