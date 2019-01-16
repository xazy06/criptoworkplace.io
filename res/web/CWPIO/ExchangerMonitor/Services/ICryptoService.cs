namespace ExchangerMonitor.Services
{
    public interface ICryptoService
    {
        byte[] Decrypt(byte[] data);
    }
}