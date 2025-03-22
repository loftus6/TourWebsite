namespace TourWebsite.Models.Files
{
    public enum FileType : ushort
    {
        Image = 0,
        Audio = 1,
        Video = 2,
        Model = 3,
    };

    public enum AttachType : ushort
    {
        Uploaded,
        Linked
    };
}
