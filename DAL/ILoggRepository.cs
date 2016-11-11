namespace WebprosjektBankOblig.DAL
{
    public interface ILoggRepository
    {
        void SkrivLogg(string bruker, bool success, string beskrivelse, int? affectedId);
    }
}