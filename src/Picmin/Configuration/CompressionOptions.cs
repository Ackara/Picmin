namespace Acklann.Picmin.Configuration
{
    public class CompressionOptions
    {
        public CompressionOptions()
        {
            Quality = 80;
            RemoveMetadata = true;
        }

        public int Quality { get; set; }
        
        public bool RemoveMetadata { get; set; }
    }
}