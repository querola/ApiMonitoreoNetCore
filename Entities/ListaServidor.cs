namespace monitoreoApiNetCore.Entities{
    public class ListaServidor{
        public ListaServidor(string fileName, System.DateTime creationTime, string fileSize, bool delay)
        {  
            this.FileName = fileName;
            this.CreationTime = creationTime;
            this.FileSize = fileSize;
            this.Delay = delay;
        }
        public string FileName { get; set; }
        public System.DateTime CreationTime { get; set; }
        public string FileSize { get; set; }
        public bool Delay { get; set; }
    }
}